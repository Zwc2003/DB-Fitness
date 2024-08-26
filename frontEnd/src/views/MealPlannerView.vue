<template>
    <div>
        <NavigationBar />
        <div class="back-button-container">
            <el-button @click="goBack" circle style="font-size: 24px; width: 50px; height: 50px;">
                <el-icon>
                    <arrow-left />
                </el-icon>
            </el-button>
        </div>
        <div class="meal-planner">
            <MealPlanner />
        </div>
      <CommonLayout />
    </div>
</template>

<style scoped>
.meal-planner {
    margin-top: 20vh;;
    grid-column: 1;
    box-shadow: 2px 2px 4px rgba(0, 0, 0, 0.1);
    padding: 10px;
    border-radius: 15px;
    background-color: #f7fbff;
    width: 90vw;
    height: 85vh;
    justify-content: center;
}
.back-button-container {
    position: absolute;
    top: 10vh; /* 调整为你需要的上边距 */
    left: 5vw; /* 调整为你需要的左边距 */
    z-index: 1000; /* 确保按钮在日历表之上 */

}



</style>

<script>
import NavigationBar from "../components/NavigationBar.vue";
import MealPlanner from "../components/MealPlanner.vue";
import { ArrowLeft } from '@element-plus/icons-vue'  // 引入ArrowLeft图标
import CommonLayout from "../components/CommonLayout.vue";

export default {
    components: { NavigationBar, MealPlanner,ArrowLeft, CommonLayout },
    methods: {
        goBack() {
            this.$router.back(); // 使用Vue Router的back方法返回上一页
        }
    }
}
</script>