<template>
  <NavigationBar />
  <CommonLayout style="margin-top: 0vh;" />
</template>

<script>

import {defineComponent} from "vue";
import { RouterView } from 'vue-router';
import CommonLayout from '../components/CommonLayout.vue';
import NavigationBar from "../components/NavigationBar.vue";
export default defineComponent({
  components: {NavigationBar, CommonLayout}
})

</script>