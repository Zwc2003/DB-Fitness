<template>
    <div>
        <el-space direction="vertical" size="20" class="guide">
            <el-image v-if="guide.imageUrl" :src="guide.imageUrl" alt="器械图片"
                style="width: 100%; max-width: 500px; margin-bottom: 20px;"></el-image>
            <el-card v-if="guide" class="card">
                <el-space direction="vertical">
                    <el-text class="title">器械名称:</el-text>
                    <el-text class="content">{{ guide.equipmentName }}</el-text>
                    <el-text class="title">使用方法:</el-text>
                    <el-text class="content">{{ guide.instructions }}</el-text>
                </el-space>
            </el-card>
            <el-button @click="goBack" type="primary" style="margin-top: 20px;">返回</el-button>
        </el-space>
    </div>
</template>

<script>
export default {
    name: "EquipmentDetails",
    data() {
        return {
            guide: null,
        };
    },
    created() {
        this.fetchGuide(this.$route.params.equipmentName);
    },
    methods: {
        fetchGuide(equipmentName) {
            // 使用静态数据代替实际API调用
            const fakeData = {
                equipmentName: equipmentName,
                instructions: "这是使用方法的假数据。",
                imageUrl: '../../logo.png', // 替换为实际图片URL
            };
            this.guide = fakeData;
        },
        goBack() {
            this.$router.push({ name: 'EquipmentGuide' });
        },
    },
};
</script>

<style scoped>
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
