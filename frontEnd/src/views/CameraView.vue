<template>
  <NavigationBar />
  <Camera />
</template>

<script>
import Camera from "../components/Camera.vue";
import NavigationBar from "../components/NavigationBar.vue";
import { defineComponent } from "vue";

export default defineComponent({
  components: { NavigationBar, Camera }
})

</script>

<style></style>