<template>
    <div class="detail-container">
        <el-space direction="vertical" size="20" class="guide">
            <el-image v-if="guide.url" :src="guide.url" alt="运动图片"
                style="width: 100%; max-width: 500px; margin-bottom: 20px;"></el-image>
            <el-card v-if="guide" class="card">
                <el-space direction="vertical">
                    <el-text class="title">运动名称:</el-text>
                    <el-text class="content">{{ guide.exerciseName }}</el-text>
                    <el-text class="title">AI建议:</el-text>
                    <el-text class="content">{{ guide.aisuggestion }}</el-text>
                    <el-image v-if="guide.gifUrl" :src="guide.gifUrl" alt="正规动作"
                        style="width: 100%; max-width: 500px; margin-top: 20px;"></el-image>
                </el-space>
            </el-card>
            <el-button @click="goBack" type="primary" style="margin-top: 20px;">返回</el-button>
        </el-space>
    </div>
</template>

<script>
export default {
    name: "FitnessDetail",
    data() {
        return {
            guide: null,
        };
    },
    created() {
        this.fetchGuide(this.$route.params.exerciseName);
    },
    methods: {
        fetchGuide(exerciseName) {
            // 使用静态数据代替实际API调用
            const fakeData = {
                exerciseName: exerciseName,
                url: 'https://via.placeholder.com/150',
                aisuggestion: '建议1',
                gifUrl: 'https://via.placeholder.com/150',
            };
            this.guide = fakeData;
        },
        goBack() {
            this.$router.push({ name: 'FitnessGuide' });
        },
    },
};
</script>

<style scoped>
.detail-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
}

.guide {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 20px;
}

.title {
    font-size: 20px;
    font-weight: bold;
}

.content {
    font-size: 16px;
}

.card {
    width: 100%;
    max-width: 500px;
    height: auto;
}
</style>
