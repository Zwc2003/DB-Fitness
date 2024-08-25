<template>
  <div id="app">
    
  </div>
</template>

<script>
import CourseHome from "@/components/CourseHome.vue";

export default {
  name: "App",
  components: {
    CourseHome,
  },
  data() {
    return {
      // 从后端获取的课程数据
      courses: [
        {
          image:
            "https://www.lesmills.com.cn/static/index_news/images/temp/courseimg.jpg",
          courseName: "举重阻尼训练",
          courseFeatures: "力量训练",
          courseDescription:
            "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
        },
        {
          image:
            "https://www.lesmills.com.cn/static/index_news/images/temp/courseimg.jpg",
          courseName: "举重阻尼训练",
          courseFeatures: "力量训练",
          courseDescription:
            "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
        },
        {
          image:
            "https://www.lesmills.com.cn/static/index_news/images/temp/courseimg.jpg",
          courseName: "举重阻尼训练",
          courseFeatures: "力量训练",
          courseDescription:
            "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
        },
        {
          image:
            "https://www.lesmills.com.cn/static/index_news/images/temp/courseimg.jpg",
          courseName: "举重阻尼训练",
          courseFeatures: "力量训练",
          courseDescription:
            "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
        },
      ],
    };
  },
};
</script>

<style>
.course-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  row-gap: 20px;
  column-gap: 50px;
  margin-left: 85px;
}
</style>
