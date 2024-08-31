<template>
  <div
    class="course-card"
    :style="{ width: width + 'px', height: height + 'px' }"
    :class="{ hovered: hovered }"
    @mouseover="hovered = true"
    @mouseleave="hovered = false"
  >
    <div class="image-section">
      <img :src="coursePhotoUrl" alt="Course Image" />
    </div>
    <div class="content-section">
      <h3 class="bigcoursename">
        <b>{{ courseName }}</b>
      </h3>
      <div v-if="hovered" class="description">
        <p class="coursefeature">
          <b>{{ courseName }}</b>
        </p>
        <p>{{ courseDescription }}</p>
        <button @click="showModal = true">进入课程</button>
      </div>
    </div>
  </div>

  <!-- 课程模态框 -->
  <CourseModal
    v-if="showModal"
    :isVisible="showModal"
    :courseName="thecourse.courseName"
    :courseStartTime="thecourse.courseStartTime"
    :courseEndTime="thecourse.courseEndTime"
    :courseGrade="thecourse.courseGrade"
    :coursePrice="thecourse.coursePrice"
    :courseDescription="thecourse.courseDescription"
    :classTime="thecourse.classTime"
    :features="thecourse.features"
    :instructorImage="thecourse.instructorImage"
    :instructorName="thecourse.instructorName"
    :instructorHonors="thecourse.instructorHonors"
    :coursePhotoUrl="thecourse.coursePhotoUrl"
    @close="showModal = false"
  />
</template>

<script>
import { ref } from "vue";
import CourseModal from "../components/CourseModal.vue";

export default {
  name: "CourseHome",
  components: {
    CourseModal,
  },

  props: {
    coursePhotoUrl: {
      type: String,
      required: true,
      default:
        "https://www.lesmills.com.cn/static/index_news/images/temp/courseimg.jpg",
    },
    courseName: {
      type: String,
      required: true,
      default: "举重阻尼训练",
    },
    courseDescription: {
      type: String,
      required: true,
      default:
        "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
    },
    width: {
      type: Number,
      default: 300,
    },
    height: {
      type: Number,
      default: 400,
    },
    courseStartTime: {
      type: String,
      required: true,
      default: "2024-08-08T17:42:16.103Z",
    },
    courseEndTime: {
      type: String,
      required: true,
      default: "2024-08-08T17:42:16.103Z",
    },
    courseGrade: {
      type: Number,
      required: true,
      default: 0,
    },
    coursePrice: {
      type: Number,
      required: true,
      default: 0,
    },
    instructorName: {
      type: String,
      required: true,
      default: "王教练",
    },
    instructorHonors: {
      type: String,
      required: true,
      default: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
    },
    instructorImage: {
      type: String,
      required: true,
      default:
        "https://ts1.cn.mm.bing.net/th?id=OIP-C.FHvYewesyi-IlHOiyjLTLAHaLH&w=204&h=306&c=8&rs=1&qlt=90&r=0&o=6&pid=3.1&rm=2",
    },
    features: {
      type: Array,
      required: true,
      default: ["感受力量涌现", "助力有氧健身", "训练全身各处"],
    },
  },

  data() {
    return {
      showModal: false,
      thecourse: {
        coursePhotoUrl: this.coursePhotoUrl,
        courseName: this.courseName,
        courseDescription: this.courseDescription,
        courseStartTime: this.courseStartTime,
        courseEndTime: this.courseEndTime,
        courseGrade: this.courseGrade,
        coursePrice: this.coursePrice,
        isVisible: false,
        classTime: "每周三",
        features: this.features,
        instructorImage: this.instructorImage,
        instructorName: this.instructorName,
        instructorHonors: this.instructorHonors,
      },
    };
  },

  watch: {
    coursePhotoUrl(newVal) {
      this.thecourse.coursePhotoUrl = newVal;
    },
    courseName(newVal) {
      this.thecourse.courseName = newVal;
    },
    courseDescription(newVal) {
      this.thecourse.courseDescription = newVal;
    },
    courseStartTime(newVal) {
      this.thecourse.courseStartTime = newVal;
    },
    courseEndTime(newVal) {
      this.thecourse.courseEndTime = newVal;
    },
    courseGrade(newVal) {
      this.thecourse.courseGrade = newVal;
    },
    coursePrice(newVal) {
      this.thecourse.coursePrice = newVal;
    },
    instructorName(newVal) {
      this.thecourse.instructorName = newVal;
    },
    instructorHonors(newVal) {
      this.thecourse.instructorHonors = newVal;
    },
    instructorImage(newVal) {
      this.thecourse.instructorImage = newVal;
    },
    features(newVal) {
      this.thecourse.features = newVal;
    },
  },
  setup(props, { emit }) {
    const hovered = ref(false);

    const enterCourse = () => {
      // 这里可以添加点击按钮后的逻辑，比如跳转到课程页面
      console.log("进入课程");
      // 例如: emit('courseEntered', courseId);
    };

    return {
      hovered,
      enterCourse,
    };
  },
};
</script>

<style scoped>
.bigcoursename {
  font-family: "ZCOOL QingKe HuangYou", cursive;
  font-size: 1.3rem;
}
.course-card {
  display: flex;
  flex-direction: column;
  border: 1.5px solid #fefefe;
  transition:
    transform 0.3s,
    box-shadow 0.3s;
  overflow: hidden;
  position: relative;
}

.course-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
}

.image-section {
  flex: 1 1 50%;
  background-size: cover;
  background-position: center;
  position: relative;
  overflow: hidden;
}
.coursefeature {
  font-size: larger;
  margin-top: -20px;
}
.image-section img {
  width: 100%;
  height: 100%;
  transition: filter 0.3s;
}

.course-card.hovered .image-section img {
  filter: blur(5px);
}

.content-section {
  flex: 1;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding: 20px;
  background-color: rgb(251, 246, 246);
  transition: background-color 0.3s;
}

.course-card.hovered .content-section {
  background-color: red;
}

.description {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100%;
  color: white;
}

.description p {
  margin-bottom: 1rem; /* 设置段落之间的间距 */
}

/* 确保最后一个段落没有底部边距 */
.description p:last-child {
  margin-bottom: 0;
}

button {
  padding: 10px 20px;
  background-color: white;
  color: red;
  border: none;
  cursor: pointer;
  margin-top: 20px;
}

.course-card:hover .content-section h3 {
  visibility: hidden;
}
</style>
