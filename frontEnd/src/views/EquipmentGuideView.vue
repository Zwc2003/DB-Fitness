<template>
    <navigation-bar />
    <div class="spacer"></div> <!-- 添加一个间隔元素 -->
    <EquipmentGuide />
    <CommonLayout />
</template>

<script>

import { defineComponent } from "vue";
import { RouterView } from 'vue-router';
import EquipmentGuide from "../components/EquipmentGuide.vue"
import CommonLayout from "../components/CommonLayout.vue";

export default defineComponent({
    components: {  EquipmentGuide }
})
</script>

<style scoped>
.spacer {
    margin-top: 20px;
    /* 根据需要调整间距 */
}
</style>
