<template>
  <navigation-bar />
  <Camera />
</template>

<script>
import Camera from "../components/Camera.vue";
import { defineComponent } from "vue";

export default defineComponent({
  components: { Camera }
})

</script>

<style></style>