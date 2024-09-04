<template>
    <div>
        <navigation-bar />
        <Forum />
        <common-layout />
    </div>
</template>


<script>
import Forum from "../components/Forum.vue";
import {commonMixin} from '../mixins/checkLoginState';

export default {
  mixins: [commonMixin],
  components: {Forum},
  created() {
    this.checkAvailable()
  }
}
</script>