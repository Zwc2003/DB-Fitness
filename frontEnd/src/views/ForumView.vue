<template>
    <div>
        <NavigationBar />
        <Forum />
        <CommonLayout />
    </div>
</template>


<script>
import NavigationBar from "../components/NavigationBar.vue";
import Forum from "../components/Forum.vue";
import CommonLayout from "../components/CommonLayout.vue";
import {ElNotification} from "element-plus";

export default {
    components: { NavigationBar, Forum,CommonLayout },
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
}


</script>