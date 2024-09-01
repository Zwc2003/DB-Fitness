<template>
  <div>
    <h1>课程信息</h1>
    <CourseCard
      v-for="(usercourse, index) in usercourses"
      :key="index"
      :coursePhotoUrl="usercourse.coursePhotoUrl"
      :courseName="usercourse.courseName"
      :courseDescription="usercourse.courseDescription"
      :courseStartTime="usercourse.courseStartTime"
      :courseEndTime="usercourse.courseEndTime"
      :courseGrade="usercourse.courseGrade"
      :coursePrice="usercourse.coursePrice"
      :courseProgress="usercourse.courseProgress"
      :features="usercourse.features"
      :instructorImage="usercourse.instructorImage"
      :instructorName="usercourse.instructorName"
      :instructorHonors="usercourse.instructorHonors"
    />
  </div>
  <!-- <div v-for="course in usercourses" :key="course.courseName">
      <img :src="course.coursePhotoUrl" alt="course image" />
      <h2>{{ course.courseName }}</h2>
      <p>{{ course.courseDescription }}</p>
      <p>教练: {{ course.instructorName }}</p>
    </div>
  </div> -->
</template>

<script>
import { mapGetters } from "vuex";
import CourseCard from "../components/CourseCard.vue";

export default {
  components: {
    CourseCard,
  },
  computed: {
    usercourses() {
      return this.getUserCourses;
    },
    ...mapGetters(["getUserCourses"]),
  },
};
</script>
