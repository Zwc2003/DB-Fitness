<template>
  <div>
    <h3>已预约的课程：</h3>
    <ul>
      <li v-for="course in bookedCourses" :key="course.id">
        {{ course.courseName }}
      </li>
    </ul>
  </div>
</template>
<script>
import { computed } from "vue";
import { useStore } from "vuex";

export default {
  setup() {
    const store = useStore();
    const bookedCourses = computed(() => store.state.bookedCourses);

    return { bookedCourses };
  },
};
</script>
