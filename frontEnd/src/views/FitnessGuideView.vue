<template>
    <navigation-bar />
    <div class="spacer"></div> <!-- 添加一个间隔元素 -->
    <AIGuide />
    <CommonLayout />
</template>

<script>

import { defineComponent } from "vue";
import { RouterView } from 'vue-router';
import FitnessGuide from "../components/FitnessGuide.vue";
import AIGuide from "../components/AIGuide.vue"
import adminEquipment from "../components/adminEquipment.vue"
import AddDiet from "../components/AddDiet.vue"
import CommonLayout from '../components/CommonLayout.vue';
import {ElNotification} from "element-plus";

export default defineComponent({
    components: {  FitnessGuide, AIGuide, adminEquipment, AddDiet,CommonLayout },
    created() {
      let token = localStorage.getItem('token');
      if (token == null) {
        ElNotification({
          title: '提示',
          message: '请先登录',
          type: 'warning',
          duration: 2000
        })
        this.$router.push('/login')
      }
    }
})
</script>

<style scoped>
.spacer {
    margin-top: 20px;
    /* 根据需要调整间距 */
}
</style>
