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
            <b class="title-right">{{ eeditForm.courseName }}</b>
            <span class="learning-status">教学中</span>
          </div>
          <div class="icoin-container">
            <el-icon
              :style="{ fontSize: '24px', marginRight: '20px' }"
              @click="handleDocumentClick"
            >
              <Document />
            </el-icon>
            <el-icon
              :style="{ fontSize: '24px', marginRight: '20px' }"
              @click="showModal = true"
            >
              <Edit />
            </el-icon>
            <el-icon :style="{ fontSize: '24px' }" @click="handleDeleteClick">
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
        <!-- <div class="course-progress" :style="progressStyle">
          {{ courseProgress }}
        </div> -->
      </div>
      <div class="card-secondrow">
        <el-icon>
          <CaretRight />
        </el-icon>
        <b class="bolder">课程时间</b>
        <div class="course-time" :style="timeStyle">
          {{ eeditForm.courseStartTime }} - {{ eeditForm.courseEndTime }}
        </div>
      </div>
    </el-card>
    <el-card class="continue-learn">
      <template #header class="header2"></template>
      <div class="continue-btn" @click="showComments = true">查看评论</div>
    </el-card>
  </div>

  <!-- 课程模态框,你这里isbooked就必须是1 -->
  <CourseModal
    v-if="showModall"
    :isVisible="showModall"
    :thecourse="eeditForm"
    @close="showModall = false"
  />

  <!-- 编辑课程模态框 -->
  <el-dialog v-model="showModal" title="编辑课程" width="50%">
    <el-form :model="form">
      <el-form-item label="课程名称">
        <el-input
          v-model="eeditForm.courseName"
          placeholder="例如:30到45分钟核心训练"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程图片">
        <el-upload
          class="upload-demo"
          :file-list="fileList"
          :on-change="handleFileUpload"
          :before-upload="beforeUpload"
          :show-file-list="false"
        >
          <div class="image-upload-container">
            <el-image
              v-if="eeditForm.coursePhotoUrl"
              :src="eeditForm.coursePhotoUrl"
              fit="cover"
            ></el-image>
            <div v-else class="upload-placeholder">
              <el-icon><plus /></el-icon>
              <span>点击上传</span>
            </div>
          </div>
        </el-upload>
      </el-form-item>
      <el-form-item label="课程描述">
        <el-input
          v-model="eeditForm.courseDescription"
          placeholder="核心肌群是身体的中心力量，对于维持姿势、提高运动表现和预防受伤至关重要。"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程容量">
        <el-input
          v-model="eeditForm.capacity"
          placeholder="请填入课程容量"
        ></el-input>
      </el-form-item>
      <el-form-item label="课程开始时间">
        <el-date-picker
          v-model="eeditForm.courseStartTime"
          type="date"
          placeholder="选择日期"
        />
      </el-form-item>
      <el-form-item label="课程结束时间">
        <el-date-picker
          v-model="eeditForm.courseEndTime"
          type="date"
          placeholder="选择日期"
        />
      </el-form-item>
      <el-form-item label="课程难度">
        <el-radio-group v-model="eeditForm.courseGrade">
          <el-radio :label="1">1</el-radio>
          <el-radio :label="2">2</el-radio>
          <el-radio :label="3">3</el-radio>
          <el-radio :label="4">4</el-radio>
          <el-radio :label="5">5</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="课程价格">
        <el-input-number
          v-model="eeditForm.coursePrice"
          :min="0"
          :max="3000"
          :step="1"
          placeholder="请填入一个数字"
        />
      </el-form-item>
      <el-form-item label="课程特征">
        <div>
          <el-tag
            v-for="(feature, index) in eeditForm.features"
            :key="index"
            closable
            @close="removeFeature(index)"
            style="margin-right: 8px"
          >
            {{ feature }}
          </el-tag>
          <el-input
            v-model="inputFeature"
            placeholder="输入并按回车,如 [力量, 增肌]"
            @keyup.enter.native="addFeature"
            style="width: 200px"
          ></el-input>
        </div>
      </el-form-item>
      <el-form-item label="课程分类">
        <el-select v-model="eeditForm.typeID" placeholder="请选择课程分类">
          <el-option label="高强度间歇" value="1"></el-option>
          <el-option label="儿童趣味课" value="3"></el-option>
          <el-option label="低强度塑形" value="2"></el-option>
          <el-option label="有氧训练" value="4"></el-option>
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submitForm">提交</el-button>
        <el-button @click="showModal = false">取消</el-button>
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="showModal = false">取消</el-button>
      <el-button type="primary" @click="submitEdit">保存修改</el-button>
    </template>
  </el-dialog>

  <!-- 评论模态框 -->
  <el-dialog v-model="showComments" title="用户评论" width="40%">
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
import CourseModal from "../components/CourseModal.vue";

export default {
  components: {
    CourseModal,
  },
  name: "TeachCard",
  props: {
    //作为传入的老师创建的教学课程的相关参数
    editForm: Object,
  },
  data() {
    return {
      eeditForm: {},
      showModal: false, //编辑课程的视窗
      showModall: false, //查看课程的视窗
      showCommentsModal: false, //查看课程评论的视窗
      //课程的相关评论
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
  created() {
    this.eeditForm = {
      classID: this.editForm.classID,
      typeID: this.editForm.typeID,
      courseName: this.editForm.courseName,
      capacity: this.editForm.capacity,
      courseDescription: this.editForm.courseDescription,
      coursePrice: this.editForm.coursePrice,
      courseStartTime: this.editForm.courseStartTime,
      courseEndTime: this.editForm.courseEndTime,
      courseGrade: this.editForm.courseGrade,
      coursePhotoUrl: this.editForm.coursePhotoUrl,
      features: this.editForm.features,
      isbooked: 1, // 默认值，可以根据需要调整
    };
  },

  methods: {
    //教练查看课程
    handleDocumentClick() {
      this.showModall = true;
    },

    //教练删除课程
    handleDeleteClick() {
      this.$emit("delete-teachcourse", this.eeditForm.courseName);
    },

    handleFileUpload(file) {
      this.eeditForm.coursePhotoUrl = URL.createObjectURL(file.raw);
    },
    beforeUpload(file) {
      this.imagePreview = "";
      const isJPGorPNG =
        file.type === "image/jpeg" || file.type === "image/png";
      const isLt2M = file.size / 1024 / 1024 < 2;

      if (!isJPGorPNG) {
        this.$message.error("上传头像图片只能是 JPG 或 PNG 格式!");
        return false;
      }
      if (!isLt2M) {
        this.$message.error("上传头像图片大小不能超过 2MB!");
        return false;
      }
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        this.imagePreview = reader.result;
        this.eeditForm.coursePhotoUrl = this.imagePreview;
      };
      return false;
    },

    //-------------------------------------- API接口------------------------------------------------------
    //教练修改课程API(完结版)
    submitEdit() {
      //!!!!editForm的最早的来源是GetCoachParticipatedCourse.API传入的数据
      this.showModal = false;
      axios
        .post(
          `http://localhost:8080/api/Course/ModifyCourse?token=${token}`,
          this.eeditForm
        )
        .then((response) => {
          console.log("课程修改成功:", response.data);
          this.showModal = false;
          //!!!!不要清数据
          ElNotification({
            title: "成功",
            message: "课程修改成功！",
            type: "success",
          });
        })
        .catch((error) => {
          console.log("课程修改错误:", error);
          ElNotification({
            title: "错误",
            message: "修改课程时发生错误，请稍后再试。",
            type: "error",
          });
        });
    },

    //教练删除课程的API(完结版)
    deleteCourseByClassID() {
      const token = localStorage.getItem("token");
      const classID = this.eeditForm.classID;
      axios
        .delete(`http://localhost:8080/api/Course/DeleteCourseByClassID`, {
          params: {
            token: token,
            classID: classID,
          },
        })
        .then((response) => {
          console.log("教练删除课程成功:", response.data);
        })
        .catch((error) => {
          console.error("教练删除课程失败:", error);
        });
    },
  },

  computed: {
    //课程卡片上的静态字体样式
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
  padding: 2px 6px;
  background-color: white;
  color: orange;
  font-size: 12px;
  border: 2px solid orange;
  border-radius: 3px;
  margin-left: 20px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
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
