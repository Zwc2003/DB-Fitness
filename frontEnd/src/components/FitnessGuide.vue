<template>
    <div>
        <h2>健身指南</h2>

        <!-- 上传健身视频截图 -->
        <div class="upload-section">
            <el-form :model="uploadForm" inline>
                <el-form-item label="动作类型名称">
                    <el-input v-model="uploadForm.exerciseName"></el-input>
                </el-form-item>
                <el-form-item label="视频截图">
                    <input type="file" @change="handleFileChange" />
                </el-form-item>
                <el-button type="primary" @click="uploadScreenshot">上传</el-button>
            </el-form>
        </div>

        <!-- 显示上传的图片 -->
        <div v-if="uploadedScreenshots.length" class="screenshot-gallery">
            <el-row :gutter="20">
                <el-col :span="6" v-for="(screenshot, index) in uploadedScreenshots" :key="index">
                    <el-card @click="goToDetail(screenshot)" :body-style="{ padding: '10px' }">
                        <img :src="screenshot.url" alt="Screenshot" class="uploaded-image" />
                        <div>{{ screenshot.exerciseName }}</div>
                    </el-card>
                </el-col>
            </el-row>
        </div>
    </div>
</template>

<script>
export default {
    name: "FitnessGuide",
    data() {
        return {
            userID: 'fixed-user-id', // 固定的用户ID
            uploadForm: {
                exerciseName: '',
                screenshot: null
            },
            uploadedScreenshots: [
                { exerciseName: '动作一', url: 'https://via.placeholder.com/150', aisuggestion: '建议一', gifUrl: 'https://via.placeholder.com/150' },
                { exerciseName: '动作二', url: 'https://via.placeholder.com/150', aisuggestion: '建议二', gifUrl: 'https://via.placeholder.com/150' },
                { exerciseName: '动作三', url: 'https://via.placeholder.com/150', aisuggestion: '建议三', gifUrl: 'https://via.placeholder.com/150' },
                { exerciseName: '动作四', url: 'https://via.placeholder.com/150', aisuggestion: '建议四', gifUrl: 'https://via.placeholder.com/150' },
            ], // 存储假数据
        };
    },
    methods: {
        handleFileChange(event) {
            this.uploadForm.screenshot = event.target.files[0];
        },
        uploadScreenshot() {
            if (this.uploadForm.exerciseName && this.uploadForm.screenshot) {
                const newScreenshot = {
                    exerciseName: this.uploadForm.exerciseName,
                    url: URL.createObjectURL(this.uploadForm.screenshot), // 本地显示上传的图片
                    aisuggestion: 'AI建议', // 假数据
                    gifUrl: 'https://via.placeholder.com/150' // 假数据
                };

                // 将上传的截图信息添加到 uploadedScreenshots 数组中
                this.uploadedScreenshots.push(newScreenshot);

                // 重置上传表单
                this.uploadForm.exerciseName = '';
                this.uploadForm.screenshot = null;
            } else {
                alert("请填写动作名称并选择截图");
            }
        },
        goToDetail(screenshot) {
            this.$router.push({
                name: 'FitnessDetail',
                params: {
                    exerciseName: screenshot.exerciseName,
                    url: screenshot.url,
                    aisuggestion: screenshot.aisuggestion,
                    gifUrl: screenshot.gifUrl
                }
            });
        }
    }
};
</script>

<style scoped>
body,
h2,
h3,
h4,
p,
.el-form-item__label,
.el-input__inner,
.el-button {
    color: black;
    /* 设置所有文字为黑色 */
}

.el-form-item__label {
    color: #666;
    /* 设置标签文字稍微淡一些 */
}

h2,
h3 {
    margin-top: 20px;
}

.el-form {
    margin-bottom: 20px;
}

.upload-section {
    display: flex;
    justify-content: flex-end;
    margin-bottom: 20px;
}

.screenshot-gallery {
    margin-top: 20px;
}

.uploaded-image {
    max-width: 100%;
    height: auto;
    cursor: pointer;
}
</style>
