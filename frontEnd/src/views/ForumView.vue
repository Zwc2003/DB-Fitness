<template>
    <div>
        <navigation-bar />
        <Forum />
        <CommonLayout />
    </div>
</template>


<script>
import Forum from "../components/Forum.vue";
import CommonLayout from "../components/CommonLayout.vue";
import {commonMixin} from '../mixins/checkLoginState';

export default {
  mixins: [commonMixin],
  components: {Forum, CommonLayout},
  created() {
    this.checkAvailable()
  }
}
</script>