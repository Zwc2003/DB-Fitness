<template>
<div class="post-container">
    <!-- 标题输入框 -->
    <input type="text" v-model="localTitle" :placeholder="titlePlaceholder" @focus="clearTitlePlaceholder"
        @blur="restoreTitlePlaceholder" class="title-input aligned-placeholder" />
    <!-- 发帖类别选择 -->
    <select v-model="localCategory" class="select-category aligned-placeholder">
        <option value=" " disabled>请选择发帖类别</option>
        <option v-for="category in categories" :key="category" :value="category">{{ category }}</option>
    </select>
    <!-- 工具栏 -->
    <Toolbar class="toolbar" :editor="editorRef" :defaultConfig="toolbarConfig" :mode="mode" />

    <!-- 上传图片与正文编辑器的容器 -->
    <div class="editor-upload-container">
        <!-- 上传图片组件 -->
        <el-upload class="avatar-uploader" :show-file-list="false" :before-upload="beforeAvatarUpload">
            <img v-if="localImgUrl" :src="localImgUrl" class="avatar" />
            <el-icon v-else class="avatar-uploader-icon">
                <Plus />
            </el-icon>
        </el-upload>

        <!-- 正文编辑器 -->
        <div class="editor-container">
            <Editor class="editor" v-model="localContent" :defaultConfig="editorConfig" :mode="mode"
                @onCreated="handleCreated" />
            <!-- 发布按钮 -->
            <button @click="emitAddPost" class="btn-primary">发布帖子</button>
        </div>
    </div>
</div>

</template>

<script>
import '@wangeditor/editor/dist/css/style.css' // 引入 css
import { onBeforeUnmount, ref, shallowRef, onMounted, watch } from 'vue'
import { Editor, Toolbar } from '@wangeditor/editor-for-vue'
import { mapState } from 'vuex'

export default {

    components: { Editor, Toolbar },
    props: {
        title: String,
        content: String,
        category: String,
        imgUrl: String
    },
    computed: {
        ...mapState(['categories'])
    },
    methods: {
        cleanHtml(content) {
            // 使用正则表达式去除 <p></p> 标签
            return content.replace(/<\/?p>/g, '');
        },
        handleSubmit() {
            // 调用 cleanHtml 方法清理内容
            const cleanedContent = this.cleanHtml(this.content);

            // 触发父组件的提交事件，同时传递清理后的内容
            this.$emit('add-post', {
                title: this.title,
                content: cleanedContent,
                category: this.category,
                imgUrl: this.imgUrl
            });
        },
        beforeAvatarUpload(file) {
            this.localImgUrl = null;
            const isJPGorPNG = file.type === 'image/jpeg' || file.type === 'image/png';
            const isLt2M = file.size / 1024 / 1024 < 2;

            if (!isJPGorPNG) {
                this.$message.error('上传图片只能是 JPG 或 PNG 格式!');
                return false;
            }
            if (!isLt2M) {
                this.$message.error('上传图片大小不能超过 2MB!');
                return false;
            }

            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
                this.localImgUrl = reader.result; // 更新 localImgUrl
                this.$emit('update:imgUrl', this.localImgUrl); // 通知父组件更新 imgUrl
            };
            return false;
        },
    },
    setup(props, { emit }) {
        const editorRef = shallowRef()
        const localTitle = ref(props.title)
        const localContent = ref(props.content)
        const localCategory = ref(props.category || ' ') // 如果未选择类别，默认为空格
        const titlePlaceholder = ref('请输入标题')
        const localImgUrl = ref(props.imgUrl || null)

        onMounted(() => {
            setTimeout(() => {
                localContent.value = props.content || ''
            }, 1500)
        })

        const toolbarConfig = {}
        const editorConfig = {
            placeholder: '请输入帖子内容',
            styleWithCSS: true,

        }

        watch(localTitle, (newValue) => {
            emit('update:title', newValue)
        })

        watch(localContent, (newValue) => {
            emit('update:content', newValue)
        })

        watch(localCategory, (newValue) => {
            emit('update:category', newValue)
        })

        watch(localImgUrl, (newValue) => {
            emit('update:imgUrl', newValue)
        })

        // 新增的 watch 逻辑，用于监听 props 的变化
        watch(() => props.title, (newValue) => {
            localTitle.value = newValue;
        });

        watch(() => props.content, (newValue) => {
            localContent.value = newValue;
        });

        watch(() => props.category, (newValue) => {
            localCategory.value = newValue;
        });

        watch(() => props.imgUrl, (newValue) => {
            localImgUrl.value = newValue;
        });


        onBeforeUnmount(() => {
            const editor = editorRef.value
            if (editor == null) return
            editor.destroy()
        })

        const handleCreated = (editor) => {
            editorRef.value = editor
        }

        const clearTitlePlaceholder = () => {
            if (localTitle.value === '') {
                titlePlaceholder.value = ''
            }
        }

        const restoreTitlePlaceholder = () => {
            if (localTitle.value === '') {
                titlePlaceholder.value = '请输入标题'
            }
        }

        const emitAddPost = () => {
            emit('add-post')
        }

        return {
            editorRef,
            localTitle,
            localContent,
            localCategory,
            localImgUrl,
            titlePlaceholder,
            mode: 'default',
            toolbarConfig,
            editorConfig,
            handleCreated,
            clearTitlePlaceholder,
            restoreTitlePlaceholder,
            emitAddPost,

        }
    }
}
</script>

<style scoped>
.post-container {
    border: 1px solid #ccc;
    padding: 0;
    position: relative;
    /* 添加此行以相对定位容器 */
}

.title-input {
    width: 100%;
    margin: 0;
    padding: 10px;
    border: 1px solid blue;
    outline: none;
    font-size: 16px;
    line-height: 1.5;
}

.select-category {
    margin: 0;
    width: 100%;
    padding: 10px;
    border: 1px solid blue;
    outline: none;
    font-size: 16px;
    line-height: 1.5;
    color: #999;
    /* 默认文本颜色为灰色 */
}

.editor-container {
    padding: 0;

    border-right: 1px solid blue;
    border-bottom: 1px solid blue;
    height: 180px;
    position: relative;
    /* 添加此行以相对定位编辑器容器 */
}

.editor {
    height: 180px;
    border: none;
    font-size: 16px;
    line-height: 1.5;
    padding: 0;
    color: #999;
    /* 编辑器默认文本颜色为灰色 */
}

.toolbar {
    border-top: none;
    border-bottom: none;
    margin: 0;
    padding: 0;
    border-left: 1px solid blue;
    border-right: 1px solid blue;
}

/* 新增的按钮样式 */
.btn-primary {
    position: absolute;
    bottom: 10px;
    /* 距离底部10px */
    right: 10px;
    /* 距离右侧10px */
    background-color: #007bff;
    color: #fff;
    padding: 8px 16px;
    border-radius: 5px;
    border: none;
    cursor: pointer;
    font-size: 14px;
}

.post-container {
    display: flex;
    flex-direction: column;
}

.editor-upload-container {
    display: flex;
    align-items: flex-start; /* 顶部对齐 */
    margin-top: 0px; /* 调整与工具栏的间距 */
}

.avatar-uploader  {
    margin-right: 0px; /* 与编辑器的间距 */
    width: 200px; /* 设置上传组件的宽度 */
    height: 180px; /* 设置上传组件的高度 */
    background-color: white;
    border-left: 1px solid blue;
    border-bottom: 1px solid blue;
}

.editor-container {
    flex-grow: 1; /* 占满剩余空间 */
}

</style>
