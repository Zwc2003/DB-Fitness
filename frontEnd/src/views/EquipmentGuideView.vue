<template>
    <NavigationBar />
    <div class="spacer"></div> <!-- 添加一个间隔元素 -->
    <EquipmentGuide />
</template>

<script>

import { defineComponent } from "vue";
import { RouterView } from 'vue-router';
import NavigationBar from "../components/NavigationBar.vue";
import EquipmentGuide from "../components/EquipmentGuide.vue"

export default defineComponent({
    components: { NavigationBar, EquipmentGuide }
})
</script>

<style scoped>
.spacer {
    margin-top: 20px;
    /* 根据需要调整间距 */
}
</style>
