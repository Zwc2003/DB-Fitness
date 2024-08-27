<template>
    <div class="bg">
        <div class="user-profile">
            <div class="profile-container">
                <aside class="profile-sidebar">
                    <div class="back-button-container">
                        <el-button @click="goBack" circle style="font-size: 24px; width: 50px; height: 50px;">
                            <el-icon>
                                <arrow-left />
                            </el-icon>
                        </el-button>
                    </div>
                    <div class="avatar-wrapper" @click="showLargeImage">
                        <img :src="imagePreview || profile.iconURL || defaultAvatar" alt="avatar" class="avatar">
                        <span class="edit-icon" @click.stop="triggerFileInput">&#9998;</span>
                    </div>
                    <h2>{{ profile.userName }}</h2>
                    <div class="tags">
                        <span v-for="(tag, index) in profile.tags" :key="index" class="tag"
                            :style="{ backgroundColor: tag.color }">{{ tag.text }}</span>
                        <span v-if="addingTag" class="tag-input">
                            <input type="text" v-model="newTag" @blur="addTag" @keyup.enter="addTag" />
                        </span>
                        <span class="add-tag" @click="addingTag = true">+</span>
                    </div>
                    <input type="file" @change="handleFileUpload" ref="fileInput" class="file-input" />
                </aside>

                <main class="profile-main">
                    <!-- 第一行：用户ID、邮箱、注册时间 -->
                    <section class="profile-info">
                        <div class="info-row">
                            <div class="profile-viewer">
                                <label>用户ID</label>
                                <p>{{ profile.userID }}</p>
                            </div>
                            <div class="profile-viewer">
                                <label>邮箱</label>
                                <p>{{ profile.email }}</p>
                            </div>
                            <div class="profile-viewer">
                                <label>注册时间</label>
                                <p>{{ profile.registrationTime }}</p>
                            </div>
                        </div>
                    </section>

                    <!-- 第三行：昵称、年龄、性别 -->
                    <section class="profile-info">
                        <div class="info-row uniform-row">
                            <EditableField label="昵称" :value="profile.userName" type="input"
                                @save="profile.userName = $event" />
                            <EditableField label="年龄" :value="profile.age" type="input" @save="profile.age = $event" />
                            <div class="profile-editor">
                                <label>性别</label>
                                <select v-model="profile.gender">
                                    <option value="男">男</option>
                                    <option value="女">女</option>
                                </select>
                            </div>
                        </div>
                    </section>

                    <!-- 第四行：类型、体重 -->
                    <section class="profile-info">
                        <div class="info-row uniform-row">
                            <EditableField label="类型" :value="profile.goalType" type="input"
                                @save="profile.goalType = $event" />
                            <EditableField label="体重" :value="profile.goalWeight" type="input"
                                @save="profile.goalWeight = $event" />

                        </div>
                    </section>

                    <!-- 第五行：会员状态 -->
                    <section class="profile-editor">
                        <label>会员状态:</label>
                        <p> {{ profile.isMember ? '会员' : '普通用户' }}</p>
                    </section>

                    <!-- 第六行：个性签名 -->
                    <section class="profile-info">
                        <div class="signature-editor signature-textarea">
                            <label>个性签名</label>
                            <textarea v-model="profile.introduction" @blur="emitSave('introduction')"></textarea>
                        </div>
                    </section>

                    <!-- 保存和取消按钮 -->
                    <div class="profile-actions">
                        <button @click="cancelEdit" class="cancel-button">取消</button>
                        <button @click="saveProfile" class="save-button">保存</button>
                    </div>

                    <!-- 帖子列表部分 -->
                    <section class="post-list">
                        <h3>用户的帖子</h3>
                        <div v-if="posts.length === 0">该用户尚未发布任何帖子。</div>
                        <ul v-else>
                            <li v-for="post in posts" :key="post.postID" class="post-item">
                                <h4>{{ post.postTitle }}</h4>
                                <p>{{ post.postContent }}</p>
                                <small>{{ new Date(post.postTime).toLocaleString() }}</small>
                            </li>
                        </ul>
                    </section>

                    <!-- 活力币余额模块 -->
                    <section class="vitality-balance">
                        <h3>活力币余额</h3>
                        <div class="balance-display">
                            <p class="balance-amount">¥ {{ vigorTokenBalance }}</p>
                        </div>
                        <h3>余额变动记录</h3>
                        <table class="balance-records">
                            <thead>
                                <tr>
                                    <th>记录ID</th>
                                    <th>变动原因</th>
                                    <th>变动量</th>
                                    <th>余额</th>
                                    <th>创建时间</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="record in vigorTokenRecords" :key="record.recordID">
                                    <td>{{ record.recordID }}</td>
                                    <td>{{ record.reason }}</td>
                                    <td :class="{ positive: record.change > 0, negative: record.change < 0 }">
                                        {{ record.change > 0 ? '+￥' : '￥' }}{{ record.change }}
                                    </td>
                                    <td>{{ '￥' }}{{ record.balance }}</td>
                                    <td>{{ new Date(record.createTime).toLocaleString() }}</td>
                                </tr>
                            </tbody>
                        </table>
                    </section>
                </main>
            </div>

            <!-- 模态框显示大图 -->
            <div v-if="isLargeImageVisible" class="modal" @click="hideLargeImage">
                <img :src="imagePreview || profile.iconURL || defaultAvatar" alt="Large avatar" class="large-avatar">
            </div>
        </div>
    </div>
</template>

<script>
import defaultAvatar from '../assets/images/default-avatar.png';
import EditableField from './EditableField.vue';
import axios from 'axios';
import { ElNotification } from 'element-plus';
import { mapState } from 'vuex';

export default {
    components: {
        EditableField,
    },
    data() {
        return {
            profile: {
                userID: null,
                userName: '',
                password: '',
                salt: '',
                email: '',
                registrationTime: '',
                iconURL: '',
                age: null,
                gender: '',
                tags: [],
                introduction: '',
                goalType: '',
                goalWeight: null,
                isMember: null,
            },
            posts: [],
            colors: ['#e57373', '#81c784', '#64b5f6', '#ffb74d', '#ba68c8', '#4db6ac'],
            addingTag: false,
            newTag: '',
            originalProfile: null,
            defaultAvatar,
            imagePreview: null,
            isLargeImageVisible: false,
            vigorTokenBalance: 0,
            vigorTokenRecords: []
        };
    },
    computed: {
        ...mapState(['token'])
    },
    created() {
        this.fetchUserProfile();
        this.fetchUserPosts();
        this.getVigorTokenBalance();
        this.getVigorTokenRecordsFromDB();

    },
    methods: {
        goBack() {
            this.$router.back();
        },
        async fetchUserProfile() {
            const token = localStorage.getItem('token');
            try {
                const response = await axios.get(`http://localhost:8080/api/User/GetPersonalProfile?token=${token}`);
                this.profile = response.data;
                this.originalProfile = JSON.parse(JSON.stringify(this.profile));
                ElNotification({
                    title: '成功',
                    message: '用户资料获取成功',
                    type: 'success',
                });
            } catch (error) {
                ElNotification({
                    title: '错误',
                    message: '获取用户资料失败',
                    type: 'error',
                });
            }
        },
        async fetchUserPosts() {
            const token = localStorage.getItem('token');
            try {
                const response = await axios.get(`http://localhost:8080/api/Post/GetPersonalPost?token=${token}`);
                this.posts = response.data;
                ElNotification({
                    title: '成功',
                    message: '用户帖子获取成功',
                    type: 'success',
                });
            } catch (error) {
                ElNotification({
                    title: '错误',
                    message: '获取用户帖子失败',
                    type: 'error',
                });
            }
        },
        handleFileUpload(event) {
            const file = event.target.files[0];
            if (file) {
                this.beforeAvatarUpload(file); // 调用图片上传前的处理方法
            }
        },
        beforeAvatarUpload(file) {
            this.imagePreview = '';
            const isJPGorPNG = file.type === 'image/jpeg' || file.type === 'image/png';
            const isLt2M = file.size / 1024 / 1024 < 2;

            if (!isJPGorPNG) {
                this.$message.error('上传头像图片只能是 JPG 或 PNG 格式!');
                return false;
            }
            if (!isLt2M) {
                this.$message.error('上传头像图片大小不能超过 2MB!');
                return false;
            }

            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
                this.imagePreview = reader.result; // 将Base64格式图片赋值给 imagePreview
                this.profile.iconURL = this.imagePreview; // 将Base64格式图片赋值给 profile.iconURL
            };
            return false; // 阻止默认的上传行为
        },
        emitSave(field) {
            console.log(`${field} updated:`, this.profile[field]);
        },
        triggerFileInput() {
            this.$refs.fileInput.click();
        },
        showLargeImage() {
            this.isLargeImageVisible = true;
        },
        hideLargeImage() {
            this.isLargeImageVisible = false;
        },
        addTag() {
            if (this.newTag) {
                const colorIndex = Math.floor(Math.random() * this.colors.length);
                this.profile.tags.push({ text: this.newTag, color: this.colors[colorIndex] });
                this.newTag = '';
                this.addingTag = false;
            }
        },
        cancelEdit() {
            this.profile = JSON.parse(JSON.stringify(this.originalProfile));
            ElNotification({
                title: '提示',
                message: '编辑已取消',
                type: 'info',
            });
        },
        async saveProfile() {
            try {
                const response = await axios.put('http://localhost:8080/api/User/UpdateProfile', {
                    userID: this.profile.userID,
                    userName: this.profile.nickname,
                    password: this.profile.password,
                    salt: null,
                    email: this.profile.email,
                    iconURL: this.profile.iconURL,
                    age: this.profile.age,
                    gender: this.profile.gender,
                    tags: this.profile.tags.map(tag => `${tag.text}:${tag.color}`).join(','),
                    introduction: this.profile.introduction,
                    goalType: this.profile.goalType,
                    goalWeight: this.profile.goalWeight,
                    isMember: this.profile.isMember,
                    isPost: -1,
                    isDelete: -1
                }, {
                    headers: {
                        Authorization: `Bearer ${this.token}`
                    }
                });

                this.originalProfile = JSON.parse(JSON.stringify(this.profile));
                ElNotification({
                    title: '成功',
                    message: '保存成功！',
                    type: 'success',
                });
            } catch (error) {
                ElNotification({
                    title: '错误',
                    message: '保存资料失败',
                    type: 'error',
                });
            }
        },
        async getVigorTokenBalance() {
            const token = localStorage.getItem('token');
            try {
                const response = await axios.get(`http://localhost:8080/api/User/GetVigorTokenBalance?token=${token}`);
                this.vigorTokenBalance = response.data.balance;
                ElNotification({
                    title: '成功',
                    message: '活力币余额获取成功',
                    type: 'success',
                });
            } catch (error) {
                ElNotification({
                    title: '错误',
                    message: '获取活力币余额失败:',
                    type: 'error',
                });
            }
        },
        async getVigorTokenRecordsFromDB() {
            const token = localStorage.getItem('token');
            try {
                const response =await axios.get(`http://localhost:8080/api/User/GetVigorTokenReacords?token=${token}`);
                this.vigorTokenRecords = response.data.records.map(item => ({
                    recordID: item.recordID,
                    reason: item.reason,
                    change: item.change,
                    balance: item.balance,
                    createTime: item.createTime
                }));

                // 按 recordID 从大到小排序
                this.vigorTokenRecords.sort((a, b) => b.recordID - a.recordID);
                ElNotification({
                    title: '成功',
                    message: '活力币变动记录获取成功',
                    type: 'success',
                });
            } catch (error) {
                ElNotification({
                    title: '错误',
                    message: '获取活力币变动记录失败',
                    type: 'error',
                });
            }
        }
    }
};
</script>

<style scoped>
.bg {
    background-image: url('../components/icons/forum-bg.jpg');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
    width: 100%;
    /*height: max-content;*/
    min-height: 100%;
    position: absolute;
    top: 0;
    left: 0;
    padding-top: 30px;
    padding-bottom: 30px;
}

.user-profile {
    display: flex;
    justify-content: center;
    align-items: center;
    height: max-content;
    width: 100%;
    background-color: transparent;
}

.profile-container {
    display: flex;
    width: 100%;
    max-width: 80vw;
    margin: auto;
    background-color: rgba(253, 248, 248, 0.5);
    /* 半透明的背景 */
    backdrop-filter: blur(10px);
    /* 添加模糊效果，模拟磨砂感 */
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1),
        /* 主阴影 */
        0 6px 20px rgba(0, 0, 0, 0.1);
    /* 次阴影，增强立体感 */
    border-radius: 15px;
    /* 圆角半径，增加平滑感 */
    justify-content: space-between;
    overflow: hidden;
    /* 避免子元素溢出边界 */
}

.profile-sidebar {
    width: 20%;
    padding: 20px;
    text-align: center;
    display: flex;
    flex-direction: column;
    justify-content: center;
    background-color: transparent;
    border: none;
}

.profile-main {
    width: 80%;
    margin-left: 0px;
    padding: 20px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    background-color: transparent;
}

.avatar-wrapper {
    position: relative;
    cursor: pointer;
}

.avatar {
    width: 200px;
    height: 200px;
    border-radius: 50%;
    position: relative;
}

.edit-icon {
    position: absolute;
    bottom: 0;
    right: 0;
    background-color: #42b983;
    color: white;
    border-radius: 50%;
    cursor: pointer;
    font-size: 14px;
    padding: 5px 10px;
    transform: translate(1%, 1%);
    /* 调整 transform，使图标更加贴近头像的右下角 */
}

.file-input {
    display: none;
}

.tags {
    display: flex;
    flex-wrap: wrap;
    gap: 5px;
    margin-top: 10px;
    justify-content: center;
}

.tag {
    border-radius: 15px;
    padding: 5px 10px;
    font-size: 14px;
    color: white;
    margin: 3px;
    display: inline-block;
}

.tag-input input {
    padding: 5px;
    border-radius: 15px;
    border: 1px solid #ccc;
}

.add-tag {
    background-color: #42b983;
    color: white;
    border-radius: 50%;
    padding: 5px 10px;
    cursor: pointer;
    font-size: 18px;
    line-height: 1;
    display: flex;
    align-items: center;
}

.profile-info {
    display: flex;
    flex-direction: column;
    gap: 20px;
    font-size: 20px;
    color: #333;
    text-align: left;
}

.info-row {
    display: flex;
    justify-content: space-between;
    gap: 20px;
}

.uniform-row>.editable-field,
.uniform-row>.profile-editor {
    flex: 1;
}


p {
    height: 50px;
    margin: 5px 0;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
    background-color: transparent;
    font-size: 20px;
    text-align: left;
    color: #333;
    line-height: 24px;
    overflow: auto;
}

.profile-editor,
.profile-viewer {
    display: flex;
    flex-direction: column;
    width: 32%;
    margin-bottom: 10px;
    font-size: 20px;
    color: #333;
    text-align: left;
}

input,
select {
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 4px;
    background-color: transparent;
    height: 50px;
    /* 增加输入框和选择框的高度 */
    font-size: 16px;
    width: 100%;
}

.modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.large-avatar {
    max-width: 90%;
    max-height: 90%;
    border-radius: 10px;
}

.signature-editor {
    display: flex;
    flex-direction: column;
    width: 100%;
    margin-bottom: 10px;

}

.signature-textarea textarea {
    height: 150px;
    font-size: 20px;
    line-height: 1.5;
    width: 100%;
    border-radius: 4px;
    background-color: transparent;
    resize: none;
    border: 1px solid #ccc;
    padding: 10px;

}

.profile-actions {
    display: flex;
    justify-content: flex-end;
    margin-top: 20px;
    font-size: 20px
}

.cancel-button,
.save-button {
    padding: 10px 20px;
    margin-left: 10px;
    border-radius: 4px;
    cursor: pointer;
}

.cancel-button {
    background-color: #d9534f;
    color: white;
}

.save-button {
    background-color: #5cb85c;
    color: white;
}

/* 新增的帖子列表样式 */
.post-list {
    margin-top: 20px;
    max-height: 30vh;
    overflow: auto;
}

.post-item {
    padding: 10px;
    border-bottom: 1px solid #ccc;
}

.post-item h4 {
    margin: 0;
    font-size: 18px;
    text-align: left;
}

.post-item p {
    margin: 5px 0;
    font-size: 14px;
    color: #555;
}

.post-item small {
    color: #999;
}

.balance-display {
    display: flex;
    align-items: center;
    /* 垂直居中 */
}

.balance-amount {
    font-size: 24px;
    color: #333;
    margin-bottom: 10px;
    /* 移除默认的外边距 */
    text-align: center;
    /* 文本居中 */
    line-height: 1.5;
    /* 调整行高 */
    background: none;
    /* 移除背景 */
    border: none;
    /* 移除边框 */
}

.balance-records {
    width: 100%;
    border-collapse: collapse;
    margin-top: 10px;
}

.balance-records th,
.balance-records td {
    border: 1px solid #dbd7d7;
    padding: 8px;
    text-align: center;
    font-size: 20px;
}

.balance-records tr:nth-child(even) {
    background-color: #f2f2f2;
}

.balance-records tr:hover {
    background-color: #ddd;
}

.positive {
    color: #005800;
}

.negative {
    color: #910000;
}

h3 {
    font-size: 20px;
    color: #333;
    text-align: left;

}

/* 针对 input 输入框 */
input[type="text"],
input[type="number"],
select {
    font-size: 20px;
    padding: 8px;
    border-radius: 4px;
    border: 1px solid #ccc;
    width: 100%;
    box-sizing: border-box;
}

/* 强制应用样式到 EditableField 组件内部 */
::v-deep .editable-field input {
    font-size: 20px;
    /* 设置字体大小 */
    padding: 8px;
    /* 添加内边距 */
    border-radius: 4px;
    border: 1px solid #ccc;
    width: 100%;
    box-sizing: border-box;
}

.back-button-container {
    position: absolute;
    top: 10px;
    /* 调整为你需要的上边距 */
    left: 10px;
    /* 调整为你需要的左边距 */
    z-index: 1000;
}
</style>
