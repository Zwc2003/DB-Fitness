<template>
    <NavigationBar />
    <div class="spacer"></div> <!-- 添加一个间隔元素 -->
    <adminEquipment />
</template>

<script>

import { defineComponent } from "vue";
import { RouterView } from 'vue-router';
import NavigationBar from "../components/NavigationBar.vue";
import FitnessGuide from "../components/FitnessGuide.vue";
import AIGuide from "../components/AIGuide.vue"
import adminEquipment from "../components/adminEquipment.vue"
import AddDiet from "../components/AddDiet.vue"

export default defineComponent({
    components: { NavigationBar, FitnessGuide, AIGuide, adminEquipment, AddDiet }
})
</script>

<style scoped>
.spacer {
    margin-top: 150px;
    /* 根据需要调整间距 */
}
</style>
