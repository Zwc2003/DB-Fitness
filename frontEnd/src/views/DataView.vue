<template>
  <NavigationBar />
  <FitnessChart style="margin-top:13%" />
</template>

<script>

import { defineComponent } from "vue";
import { RouterView } from 'vue-router';
import FitnessChart from '../components/dataVis.vue';
import NavigationBar from "../components/NavigationBar.vue";
import EquipmentGuide from "../components/EquipmentGuide.vue"

export default defineComponent({
  components: { NavigationBar, EquipmentGuide }
})
</script>