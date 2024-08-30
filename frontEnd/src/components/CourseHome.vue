<template>
  <div
    class="course-card"
    :style="{ width: width + 'px', height: height + 'px' }"
    :class="{ hovered: hovered }"
    @mouseover="hovered = true"
    @mouseleave="hovered = false"
  >
    <div class="image-section">
      <img :src="image" alt="Course Image" />
    </div>
    <div class="content-section">
      <h3 class="bigcoursename">
        <b>{{ courseName }}</b>
      </h3>
      <div v-if="hovered" class="description">
        <p class="coursefeature">
          <b>{{ courseFeatures }}</b>
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
    :modalBackground="thecourse.background"
    :courseTitle="thecourse.title"
    :startTime="thecourse.start"
    :endTime="thecourse.end"
    :classTime="thecourse.classTime"
    :features="thecourse.features"
    :instructorImage="thecourse.instructorImage"
    :instructorName="thecourse.instructorName"
    :instructorHonors="thecourse.instructorHonors"
    :courseDescription="thecourse.courseDescription"
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
  data() {
    return {
      showModal: false,
      thecourse: {
        isVisible: false,
        background:
          "https://www.lesmills.com.cn/uploads/20231104/fbdee91e55d525de27a01e2e0a74040b.png",
        title: "肌肉力量训练",
        start: "2022.03.04",
        end: "2023.03.04",
        classTime: "每周三",
        features: ["感受力量涌现", "助力有氧健身", "训练全身各处"],
        instructorImage:
          "https://ts1.cn.mm.bing.net/th?id=OIP-C.FHvYewesyi-IlHOiyjLTLAHaLH&w=204&h=306&c=8&rs=1&qlt=90&r=0&o=6&pid=3.1&rm=2",
        instructorName: "王教练",
        instructorHonors: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
        courseDescription:
          "BODYCOMBAT能训练到你的腿部、手臂、背部和肩膀，对核心部位有显著效果。在课程中你能消耗卡路里、提高协调性、敏捷性和速度，感觉自己充满力量。BODYCOMBAT 训练内容进行调整使之符合自身水平和目标。我们的教练将始终提供多重训练强度供你选择。在开始的时候，你可以每周参加1至2节课，很快你就能体会到骁勇精壮的感觉。",
      },
    };
  },
  props: {
    image: {
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
    courseFeatures: {
      type: String,
      required: true,
      default: "力量训练",
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
