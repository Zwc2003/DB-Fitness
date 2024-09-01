<template>
  <div class="icon-course-container">
    <div class="icon-container">
      <el-icon>
        <ChatSquare />
      </el-icon>
      <span class="hot-text"><b class="bolder">HOT</b></span>
    </div>

    <el-card class="my-card">
      <template #header>
        <div class="card-header">
          <div class="title-left">
            <b class="bolder">继续学习</b>
            <b class="title-right">{{ courseName }}</b>
            <span class="learning-status">学习中</span>
          </div>
          <div class="icoin-container">
            <el-icon
              :style="{ fontSize: '24px', marginRight: '20px' }"
              @click="handleStarClick"
            >
              <Star />
            </el-icon>
            <el-icon :style="{ fontSize: '24px' }" @click="handleChatClick">
              <ChatDotRound />
            </el-icon>
          </div>
        </div>
      </template>
      <div class="card-firstrow">
        <el-icon>
          <CaretRight />
        </el-icon>
        <b class="bolder">学习进度</b>
        <div class="course-progress" :style="progressStyle">
          {{ courseProgress }}
        </div>
      </div>
      <div class="card-secondrow">
        <el-icon>
          <CaretRight />
        </el-icon>
        <b class="bolder">有效日期</b>
        <div class="course-time" :style="timeStyle">
          {{ courseStartTime }} - {{ courseEndTime }}
        </div>
      </div>
    </el-card>
    <el-card class="continue-learn">
      <template #header class="header2"> </template>
      <div class="continue-btn" @click="showModal = true">查看课程</div>
    </el-card>
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
    :isbooked="1"
    @close="showModal = false"
  />

  <!-- 课程结束前弹窗 -->
  <el-dialog v-model="showDialog" title="提示">
    <p>请课程结束再评价课程</p>
    <template #footer>
      <el-button @click="showDialog = false">知道了</el-button>
    </template>
  </el-dialog>

  <!-- 评分弹窗 -->
  <el-dialog v-model="showRateDialog" title="课程评分">
    <el-rate
      v-model="ratingValue"
      :texts="['oops', 'disappointed', 'normal', 'good', 'great']"
      show-text
    />
    <template #footer>
      <el-button @click="submitRating">提交</el-button>
      <el-button @click="showRateDialog = false">取消</el-button>
    </template>
  </el-dialog>

  <!-- 评论输入弹窗 -->
  <el-dialog v-model="showInputDialog" title="发表评论">
    <el-input
      v-model="inputText"
      style="width: 240px"
      placeholder="Please input"
    />
    <template #footer>
      <el-button @click="submitComment">提交</el-button>
      <el-button @click="showInputDialog = false">取消</el-button>
    </template>
  </el-dialog>
</template>

<script>
import { ref } from "vue";
import CourseModal from "../components/CourseModal.vue";

export default {
  components: {
    CourseModal,
  },
  name: "CourseCard",

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
    courseProgress: {
      type: String,
      default: "0节课/0节课",
    },
    classTime: {
      type: String,
      default: "17:00-18:30",
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
        classTime: this.classTime,
        courseProgress: this.courseProgress,
        features: this.features,
        instructorImage: this.instructorImage,
        instructorName: this.instructorName,
        instructorHonors: this.instructorHonors,
      },
      showDialog: false,
      showRateDialog: false,
      showInputDialog: false,
      ratingValue: 0,
      inputText: "",
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
    courseProgress(newVal) {
      this.thecourse.courseProgress = newVal;
    },
    classTime(newVal) {
      this.thecourse.classTime = newVal;
    },
  },

  computed: {
    progressStyle() {
      return {
        color: "#337ecc",
        fontWeight: "bold",
        marginLeft: "10px",
      };
    },
    timeStyle() {
      return {
        color: "#337ecc",
        fontWeight: "bold",
        marginLeft: "10px",
      };
    },
  },

  methods: {
    handleStarClick() {
      const currentTime = new Date();
      const courseEndTime = new Date(this.endTime);
      if (currentTime < courseEndTime) {
        // 如果课程未结束，显示提示弹窗
        this.showDialog = true;
      } else {
        // 如果课程已结束，显示评分弹窗
        this.showRateDialog = true;
      }
    },
    //显示评论的框
    handleChatClick() {
      this.showInputDialog = true;
    },
    //提交评分
    submitRating() {
      // 在此处理评分提交的逻辑，例如发送到后端
      console.log("提交评分:", this.ratingValue);
      this.showRateDialog = false;
    },
    //提交评论
    submitComment() {
      console.log("提交评论:", this.inputText);
      this.showInputDialog = false;
    },
  },
};
</script>

<style scoped>
.bolder {
  font-weight: bold;
}

.icon-course-container {
  display: flex;
  align-items: center;
}

.icon-container {
  position: relative;
  display: inline-flex;
  font-size: 30px;
  color: red;
  margin-right: -10px;
  margin-top: -60px;
}

.hot-text {
  position: absolute;
  top: 5px;
  left: 3px;
  font-size: 10px;
  color: red;
  font-weight: bold;
}

.my-card {
  width: 500px;
  height: 130px;
}

.card-header {
  display: flex;
  align-items: center;
  height: 10px;
  justify-content: space-between;
  width: 470px;
}

.title-left {
  display: flex;
  align-items: center;
  font-size: smaller;
  font-weight: bold;
}

.title-right {
  font-weight: bold;
  font-size: 1.2rem; /* 可以根据需要调整课程名的字体大小 */
}

.title-left .bolder,
.title-left .title-right {
  margin-right: 10px;
}

.learning-status {
  padding: 2px 6px; /* 内边距 */
  background-color: white;
  color: orange; /* 文本颜色 */
  font-size: 12px; /* 字体大小 */
  border: 2px solid orange; /* 边框颜色 */
  border-radius: 3px; /* 边框圆角 */
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* 阴影效果 */
}

.icoin-container {
  display: flex;
  align-items: center;
  cursor: pointer;
}

.card-firstrow {
  display: flex;
  align-items: center; /* 垂直居中对齐 */
  margin-top: 5px;
  margin-bottom: 7px;
}

.card-secondrow {
  display: flex;
  align-items: center; /* 垂直居中对齐 */
}

.continue-learn {
  margin-top: 1px;
  height: 134px;
  padding: 20px;
}

.continue-btn {
  display: block;
  text-align: center;
  padding: 5px;
  width: 120px;
  background-color: orange;
  color: white;
  font-weight: bold;
  cursor: pointer;
  user-select: none;
  border-radius: 3px;
  margin-top: -10px;
}
</style>
