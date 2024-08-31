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
          </div>
          <div class="title-right">
            <b class="bolder">{{ courseName }}</b>
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
          {{ startTime }} - {{ endTime }}
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
      showDialog: false,
      showRateDialog: false,
      showInputDialog: false,
      ratingValue: 0,
      inputText: "",
    };
  },

  props: {
    // 定义组件的属性
    courseName: {
      type: String,
      default: "名称",
    },
    courseProgress: {
      type: String,
      default: "0节课/0节课",
    },
    startTime: {
      type: String,
      default: "2022-08-23T14:00:00",
    },
    endTime: {
      type: String,
      default: "2024-08-23T14:00:00",
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
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.icoin-container {
  margin-left: 100px;
  padding-left: 100px;
  display: flex;
  align-items: center;
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
  justify-content: flex-start;
  width: 500px;
}

.title-left {
  font-size: smaller;
  font-weight: bold;
}

.title-right {
  margin-left: 20px; /* 将课程名推向右侧 */
  font-size: larger; /* 可以根据需要调整课程名的字体大小 */
}

.learning-status {
  padding: 2px 6px; /* 内边距 */
  background-color: white;
  color: orange; /* 文本颜色 */
  font-size: 12px; /* 字体大小 */
  border: 2px solid orange; /* 边框颜色 */
  border-radius: 3px; /* 边框圆角 */
  margin-left: 20px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* 阴影效果 */
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
