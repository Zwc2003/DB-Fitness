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
            <b class="title-right">{{ thecourse.courseName }}</b>
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
          {{ nowprogress }}节课/{{ allprogress }}节课
        </div>
      </div>
      <div class="card-secondrow">
        <el-icon>
          <CaretRight />
        </el-icon>
        <b class="bolder">有效日期</b>
        <div class="course-time" :style="timeStyle">
          {{ courseStartTim }} - {{ courseEndTim }}
        </div>
      </div>
    </el-card>
    <el-card class="continue-learn">
      <template #header class="header2"> </template>
      <div class="continue-btn" @click="showModal = true">查看课程</div>
    </el-card>
  </div>

  <!-- 课程模态框,你这里isbooked就必须是1 -->
  <CourseModal
    v-if="showModal"
    :isVisible="showModal"
    :thecourse="thecourse"
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
import CourseModal from "../components/CourseModal.vue";

export default {
  components: {
    CourseModal,
  },
  name: "CourseCard",

  //传入的是GetParticipatedCourseByUserID API的数据
  props: {
    thecourse: Object,
  },

  data() {
    return {
      showModal: false, //课程视窗
      showDialog: false, //message弹窗
      showRateDialog: false,
      showInputDialog: false,
      ratingValue: 0,
      inputText: "",
      nowprogress: 0,
      allprogress: 0,
      courseStartTim: new Date(this.thecourse.courseStartTime)
        .toISOString()
        .split("T")[0],
      courseEndTim: new Date(this.thecourse.courseEndTime)
        .toISOString()
        .split("T")[0],
    };
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

  mounted() {
    this.calculateProgress();
  },

  methods: {
    //评分
    handleStarClick() {
      const currentTime = new Date();
      const courseEndTime = new Date(this.thecourse.courseEndTime);
      if (currentTime < courseEndTime) {
        // 如果课程未结束，显示提示弹窗
        this.showDialog = true;
      } else {
        this.showRateDialog = true;
      }
    },
    //评论
    handleChatClick() {
      const currentTime = new Date();
      const courseEndTime = new Date(this.thecourse.courseEndTime);
      if (currentTime < courseEndTime) {
        // 如果课程未结束，显示提示弹窗
        this.showDialog = true;
      } else {
        this.showInputDialog = true;
      }
    },

    //提交评分
    submitRating() {
      gradeCourse();
      this.showRateDialog = false;
    },

    //提交评价
    submitComment() {
      commentCourse();
      this.showInputDialog = false;
    },

    //计算课程进度
    calculateProgress() {
      const courseStartTime = new Date(this.courseStartTim);
      const courseEndTime = new Date(this.courseEndTim);

      const today = new Date();

      // 计算allprogress，即课程总天数
      const timeDiff = courseEndTime.getTime() - courseStartTime.getTime();
      console.log(this.thecourse);
      this.allprogress =
        Math.ceil(timeDiff / (1000 * 60 * 60 * 24)) /
        this.thecourse.schedules.length; // 总天数

      // 计算nowprogress
      if (today < courseStartTime) {
        this.nowprogress = 0;
      } else if (today > courseEndTime) {
        this.nowprogress = this.allprogress;
      } else {
        const currentTimeDiff = today.getTime() - courseStartTime.getTime();
        this.nowprogress =
          Math.ceil(currentTimeDiff / (1000 * 60 * 60 * 24)) /
          this.thecourse.schedules.length; // 今天距离开始的天数
      }
    },
    //-------------------------------------- API接口函数-----------------------------------------------------
    //用户上传对课程的评分(完结版)
    gradeCourse() {
      const token = localStorage.getItem("token");
      const classID = thecourse.classID;
      const postData = {
        token: token,
        classID: classID,
        grade: this.ratingValue,
      };
      axios
        .post(`http://localhost:8080/api/Course/GradeCourse`, postData)
        .then((response) => {
          console.log("评分成功:", response.data);
        })
        .catch((error) => {
          console.error("评分失败:", error);
        });
    },
    commentCourse() {
      const token = localStorage.getItem("token");
      const classID = thecourse.classID;
      const postData = {
        token: token,
        classID: classID,
        comment: this.inputText,
      };
      axios
        .post("http://localhost:8080/api/Course/PublishComment", postData)
        .then((response) => {
          console.log("评论成功:", response.data);
        })
        .catch((error) => {
          console.error("评论失败:", error);
        });
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
  font-size: 1.2rem;
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
  align-items: center;
  margin-top: 5px;
  margin-bottom: 7px;
}

.card-secondrow {
  display: flex;
  align-items: center;
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
