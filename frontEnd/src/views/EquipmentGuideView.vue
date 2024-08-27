<template>
    <NavigationBar />
    <div class="spacer"> <!-- 添加一个间隔元素 -->
    <EquipmentGuide />
</div>
    <CommonLayout />
</template>

<script>

import { defineComponent } from "vue";
import { RouterView } from 'vue-router';
import NavigationBar from "../components/NavigationBar.vue";
import EquipmentGuide from "../components/EquipmentGuide.vue"
import CommonLayout from "../components/CommonLayout.vue";

export default defineComponent({
    components: { NavigationBar, EquipmentGuide }
})
</script>

<style scoped>
.spacer {
    margin-top: 5vh;
    /* 根据需要调整间距 */
}
</style>
