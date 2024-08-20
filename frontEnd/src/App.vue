<template>
  <RouterView />
</template>

<script>
import { RouterView } from 'vue-router';
import FitnessChart from './components/dataVis.vue'

export default {
  name: 'App',
  components: {
    FitnessChart
  }
}
</script>