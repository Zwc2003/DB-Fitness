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
    <div class="meal-record">
      <MealRecord />
    </div>
  </div>
</template>

<script>
import NavigationBar from "../components/NavigationBar.vue";
import MealRecord from "../components/MealRecord.vue";

export default {
  components: {
    NavigationBar,
    MealRecord,
  },
  methods: {
    goBack() {
      this.$router.back(); // 使用Vue Router的back方法返回上一页
    }
  }
}
</script>

<style scoped>
.meal-record {
  margin-top: 20vh;
  width: 90vw;
  height: 85vh;
  justify-content: center;
}

.back-button-container {
  position: absolute;
  top: 10.5vh;
  /* 调整为你需要的上边距 */
  left: 4vw;
  /* 调整为你需要的左边距 */
  z-index: 1000;
  /* 确保按钮在日历表之上 */

}
</style>