<template>
    <div class="common-layout">
      <el-container>
        <el-header class="header">
          <NavigationBar />
        </el-header>
        <el-container class="main-container">
          <el-main class="main-content">
            <MealRecord />
          </el-main>
        </el-container>
      </el-container>
    </div>
  </template>
  
  <script>
  import NavigationBar from "../components/NavigationBar.vue";
  import MealRecord from "../components/MealRecord.vue";
  
  export default {
    components: {
      NavigationBar, 
      MealRecord,
    }
  }
  </script>
  
  <style scoped>
  .common-layout {
    height: 100vh;
    margin-top: 50px;
  }
  
  .main-container {
    display: flex;
  }
  
  .main-content {
    width:1620px;
  }
  
  </style>
  