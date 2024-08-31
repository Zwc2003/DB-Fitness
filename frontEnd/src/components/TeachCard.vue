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
            <b class="bolder">教学课单</b>
          </div>
          <div class="title-right">
            <b class="bolder">{{ courseName }}</b>
            <span class="learning-status">教学中</span>
          </div>
          <div class="icoin-container">
            <el-icon
              :style="{ fontSize: '20px', marginRight: '20px' }"
              @click="handleDocumentClick"
            >
              <Document />
            </el-icon>
            <!-- 编辑按钮 -->
            <el-icon
              :style="{ fontSize: '20px', marginRight: '20px' }"
              @click="handleEditClick"
            >
              <Edit />
            </el-icon>
            <!-- 删除按钮 -->
            <el-icon
              :style="{ fontSize: '20px', marginRight: '20px' }"
              @click="handleDeleteClick"
            >
              <Delete />
            </el-icon>
          </div>
        </div>
      </template>
      <div class="card-firstrow">
        <el-icon>
          <CaretRight />
        </el-icon>
        <b class="bolder">教学进度</b>
        <div class="course-progress" :style="progressStyle">
          {{ courseProgress }}
        </div>
      </div>
      <div class="card-secondrow">
        <el-icon>
          <CaretRight />
        </el-icon>
        <b class="bolder">课程时间</b>
        <div class="course-time" :style="timeStyle">
          {{ startTime }} - {{ endTime }}
        </div>
      </div>
    </el-card>
    <!-- 查看评论按钮 -->
    <el-card class="continue-learn">
      <template #header class="header2"></template>
      <div class="continue-btn" @click="showComments">查看评论</div>
    </el-card>
  </div>

  <!-- 课程模态框 -->
  <CourseModal
    v-if="showModall"
    :isVisible="showModall"
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
    @close="showModall = false"
  />

  <!-- 编辑课程模态框 -->
  <el-dialog v-model="showModal" title="编辑课程" width="50%">
    <el-form :model="editForm">
      <el-form-item label="模态框背景" prop="modalBackground">
        <el-input
          v-model="editForm.modalBackground"
          placeholder="请输入模态框背景"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程标题" prop="courseTitle">
        <el-input
          v-model="editForm.courseTitle"
          placeholder="请输入课程标题"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程开始时间" prop="startTime">
        <el-input
          v-model="editForm.startTime"
          placeholder="请输入课程开始时间"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程结束时间" prop="endTime">
        <el-input
          v-model="editForm.endTime"
          placeholder="请输入课程结束时间"
        ></el-input>
      </el-form-item>
      <el-form-item label="每周几开课" prop="classTime">
        <el-input
          v-model="editForm.classTime"
          placeholder="请输入课程时间"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程标签" prop="features">
        <el-input
          v-model="editForm.features"
          placeholder="请输入课程标签"
        ></el-input>
      </el-form-item>
      <el-form-item label="上传教练头像" prop="instructorImage">
        <el-input
          v-model="editForm.instructorImage"
          placeholder="请上传教练头像"
        ></el-input>
      </el-form-item>
      <el-form-item label="昵称" prop="instructorName">
        <el-input
          v-model="editForm.instructorName"
          placeholder="请输入昵称"
        ></el-input>
      </el-form-item>
      <el-form-item label="荣誉" prop="instructorHonors">
        <el-input
          v-model="editForm.instructorHonors"
          placeholder="请输入您取得的荣誉"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程描述" prop="courseDescription">
        <el-input
          type="textarea"
          v-model="editForm.courseDescription"
          placeholder="请输入课程描述"
        ></el-input>
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="showModal = false">取消</el-button>
      <el-button type="primary" @click="submitEdit">保存修改</el-button>
    </template>
  </el-dialog>

  <!-- 评论模态框 -->
  <el-dialog v-model="showCommentsModal" title="用户评论" width="40%">
    <div v-for="(comment, index) in comments" :key="index" class="comment-row">
      <img :src="comment.avatar" class="comment-avatar" />
      <span class="comment-nickname">{{ comment.nickname }}</span>
      <span class="comment-content">{{ comment.content }}</span>
      <span class="comment-time">{{ comment.time }}</span>
      <span class="comment-rating">{{ comment.rating }} / 5</span>
    </div>
  </el-dialog>
</template>

<script>
import { ref } from "vue";
import CourseModal from "../components/CourseModal.vue";

export default {
  components: {
    CourseModal,
  },
  name: "TeachCard",
  data() {
    return {
      showModal: false,
      showModall: false,
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
      showCommentsModal: false,
      editForm: {
        modalBackground: "",
        courseTitle: "",
        startTime: "",
        endTime: "",
        classTime: "",
        features: "",
        instructorImage: "",
        instructorName: "",
        instructorHonors: "",
        courseDescription: "",
      },
      comments: [
        {
          avatar: "user1.jpg",
          nickname: "用户1",
          content: "课程很棒！",
          time: "2024-08-20",
          rating: 5,
        },
        {
          avatar: "user2.jpg",
          nickname: "用户2",
          content: "内容很丰富",
          time: "2024-08-21",
          rating: 4,
        },
      ],

      showDialog: false,
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
    handleContinue() {
      // 点击时执行的操作，例如跳转
      console.log("继续学习按钮被点击");
    },
    handleDocumentClick() {
      // 预填充编辑表单

      this.showModall = true;
    },
    handleEditClick() {
      // 预填充编辑表单
      this.editForm = {
        modalBackground: "当前背景",
        courseTitle: "当前课程标题",
        startTime: "10:00 AM",
        endTime: "11:00 AM",
        classTime: "1小时",
        features: "特色",
        instructorImage: "教练图片路径",
        instructorName: "教练名称",
        instructorHonors: "教练荣誉",
        courseDescription: "当前课程描述",
      };
      this.showModal = true;
    },
    handleDeleteClick() {
      // 删除课程卡片逻辑
      this.$emit("delete-course"); // 通知父组件删除该课程
    },
    submitEdit() {
      // 保存修改
      this.showModal = false;
      console.log("提交表单数据:", this.editForm);
    },
    showComments() {
      this.showCommentsModal = true;
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
  margin-left: 70px;
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
  width: 600px;
}

.title-left {
  font-size: smaller;
  font-weight: bold;
}

.title-right {
  margin-left: 20px;
  font-size: larger;
}

.learning-status {
  padding: 2px 6px;
  background-color: white;
  color: orange;
  font-size: 12px;
  border: 2px solid orange;
  border-radius: 3px;
  margin-left: 20px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.card-firstrow {
  margin-top: 5px;
  margin-bottom: 7px;
  display: flex;
  align-items: center;
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
}
</style>
