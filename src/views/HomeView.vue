<template>
  <div aria-label="A complete example of page header">
    <el-page-header @back="onBack">
      <template #breadcrumb>
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: './page-header.html' }">
            FitFit
          </el-breadcrumb-item>
          <el-breadcrumb-item>
            <a href="./page-header.html">健身课程</a>
          </el-breadcrumb-item>
        </el-breadcrumb>
      </template>
      <template #content>
        <div class="flex items-center">
          <el-avatar
            class="mr-3"
            :size="32"
            src="https://ts3.cn.mm.bing.net/th?id=OIP-C.9khWcYup3srhgw3V1fi7-QHaHa&w=250&h=250&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2"
          />
          <span class="text-large font-600 mr-3">李华</span>
          <span
            class="text-sm mr-2"
            style="color: var(--el-text-color-regular)"
          >
            2033458
          </span>
          <el-tag>欢迎您~</el-tag>
        </div>
      </template>
      <template #extra>
        <div class="flex items-center">
          <el-button>
            {{ userRole }}
          </el-button>
          <router-link v-if="userRole === '普通用户'" to="/userhome">
            <el-button type="primary">我的主页</el-button>
          </router-link>
          <router-link v-else-if="userRole === '教练'" to="/publishcourse">
            <el-button type="primary">我的主页</el-button>
          </router-link>
        </div>
      </template>
    </el-page-header>
  </div>
  <div id="app">
    <div class="hero-section-wrapper">
      <div class="hero-section">
        <div class="bottom-bar">
          <img
            class="left-image"
            src="https://tse4-mm.cn.bing.net/th/id/OIP-C.sfCxPFaiDHtJbebl8q6mOAHaHa?w=192&h=193&c=7&r=0&o=5&pid=1.7"
            alt="左侧图片"
          />
          <div class="filter-options">
            <span
              v-for="option in filterOptions"
              :key="option.key"
              @mouseover="hoveredOption = option.key"
              @mouseleave="hoveredOption = null"
              @click="selectOption(option.key)"
              :class="{ active: selectedOption === option.key }"
            >
              <b>{{ option.label }}</b>
              <div
                v-if="
                  hoveredOption === option.key || selectedOption === option.key
                "
                class="underline"
              ></div>
            </span>
          </div>
          <div v-if="showInput" class="input-box">
            <input v-model="inputValue" placeholder="请输入内容" />
            <button @click="submitInput">提交</button>
          </div>
          <div class="red-box" @click="showRecommendation">
            想知道什么课程适合我
          </div>
        </div>
      </div>
    </div>

    <!-- 课程内容展示区域 -->
    <div class="course-grid">
      <CourseHome
        v-for="(course, index) in courses"
        :key="index"
        :image="course.image"
        :courseName="course.courseName"
        :courseFeatures="course.courseFeatures"
        :courseDescription="course.courseDescription"
        :width="300"
        :height="400"
      />
    </div>

    <!-- 弹窗 -->
    <el-dialog v-model="isDialogVisible" title="提示">
      <p>{{ dialogMessage }}</p>
      <span slot="footer" class="dialog-footer">
        <el-button @click="isDialogVisible = false">确定</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import CourseHome from "@/components/CourseHome.vue";
import axios from "axios";

export default {
  components: {
    CourseHome,
  },
  data() {
    return {
      userRole: "普通用户",
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
      isDialogVisible: false,
      dialogMessage: "",
      hoveredOption: null,
      selectedOption: null,
      showInput: false, //弹窗
      inputValue: "", //用户输入的筛选的特征
      filterOptions: [
        //筛选课程的标签
        { key: "all", label: "全部课程" },
        { key: "fitness", label: "课程关键词" },
        { key: "price", label: "课程价格" },
        { key: "category", label: "课程类别" },
      ],

      apiUrl: "/api/Course",
      courseDetails: {},
      courseList: [],
      courseComments: [],
      searchResults: [],
      reservationStatus: "",
      paymentStatus: "",
      cancellationStatus: "",
      commentStatus: "",
      ratingStatus: "",
    };
  },
  methods: {
    selectOption(option) {
      this.selectedOption = option;
      this.showInput = ["fitness", "price", "category"].includes(option);
      this.inputValue = ""; // 清空输入框内容
    },
    showRecommendation() {
      // 弹出推荐课程的模态框逻辑
      console.log("显示推荐课程模态框");
    },
    submitInput() {
      // 将输入框的内容发送给后端
      console.log(`提交的内容: ${this.inputValue}`);
      // 发送请求到后端
      // axios.post('/api/filter', { [this.selectedOption]: this.inputValue })
      //   .then(response => {
      //     // 处理后端返回的筛选结果
      //   });
    },
    showRecommendation() {
      // 判断用户是否有上过课程
      if (this.courses.length === 0) {
        this.dialogMessage = "您无课程记录，请先选择课程，才能根据您的喜好推荐";
      } else {
        // 根据用户喜好推荐课程的逻辑
        this.dialogMessage = "根据您的课程记录，推荐的课程如下：...";
        // 更新 filteredCourses，显示推荐的课程
      }
      this.isDialogVisible = true;
    },

    //搜索课程
    async searchCourses(keyword, typeID, minPrice = null, maxPrice = null) {
      try {
        const response = await axios.get(`${this.apiUrl}/SearchCourse`, {
          params: { keyword, typeID, minPrice, maxPrice },
        });
        this.searchResults = response.data;
      } catch (error) {
        console.error("Error searching courses:", error);
      }
    },

    //获取所有课程,课程中心大厅展示
    async getAllCourses() {
      try {
        const response = await axios.get(`${this.apiUrl}/GetAllCourse`);
        this.courseList = response.data;
      } catch (error) {
        console.error("Error fetching all courses:", error);
      }
    },

    //预约课程
    async reserveCourse(classID, payMethod) {
      try {
        const response = await axios.post(`${this.apiUrl}/ReserveCourse`, {
          classID,
          payMethod,
        });
        this.reservationStatus = response.data;
      } catch (error) {
        console.error("Error reserving course:", error);
      }
    },

    //课程详情展示
    async getCourseById(classID) {
      try {
        const response = await axios.get(`${this.apiUrl}/GetCourseByClassID`, {
          params: { classID },
        });
        this.courseDetails = response.data;
      } catch (error) {
        console.error("Error fetching course details:", error);
      }
    },

    //用户身份

    //查看课程评论
    async getCourseComments(classID) {
      try {
        const response = await axios.get(
          `${this.apiUrl}/GetCourseCommentByClassID`,
          {
            params: { classID },
          }
        );
        this.courseComments = response.data;
      } catch (error) {
        console.error("Error fetching course comments:", error);
      }
    },
  },
  mounted() {
    // 假设这是从后端获取的课程数据
    (this.courses = [
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
          "https://www.lesmills.com.cn/uploads/20211207/d5d8d0860359243eee20bc507bf2c231.jpg",
        courseName: "平衡与专注、持久与柔软",
        courseFeatures: "力量训练",
        courseDescription:
          "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
      },
      {
        image:
          "https://www.lesmills.com.cn/uploads/20211207/30c00cdf6752eeda8356ecd01893dabd.jpg",
        courseName: "30到45分钟核心训练",
        courseFeatures: "力量训练",
        courseDescription:
          "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
      },
      {
        image:
          "https://www.lesmills.com.cn/uploads/20211207/e4466eb6aace56fdaff24fc4c3c318af.jpg",
        courseName: "瘦身、紧致、有型",
        courseFeatures: "力量训练",
        courseDescription:
          "适合希望迅速实现瘦身、紧致和健美效果的人士。 通过重复多次举起轻量级到中量级的重量",
      },
    ]),
      (this.filteredCourses = this.courses); // 默认展示所有课程
  },
};
</script>

<style scoped>
.el-page-header {
  width: 1200px;
  margin-top: -50px;
  margin-right: -1200px;
  margin-bottom: 100px;
}

.hero-section {
  position: relative;
  width: 160vh;
  height: 80vh;
  background-image: url("https://www.lesmills.com.cn/uploads/20230316/f3e86ab4e56683a1481b0c0b4562d799.png");
  background-size: cover;
  background-position: center;
  margin-left: -95px;
  margin-top: 30px;
}

.hero-title {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  color: white;
  font-size: 3rem;
  font-weight: bold;
}

.bottom-bar {
  margin-bottom: -50px;
  margin-left: 10%;
  position: absolute;
  bottom: 0;
  width: 80%;
  padding: 10px 20px;
  background-color: white;
  border-bottom: 5px solid black; /* 控制黑色底边的厚度 */
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.left-image {
  width: 100px;
  height: auto;
}

.filter-options {
  display: flex;
  gap: 50px;
  font-size: 1rem;
}

.filter-options span {
  position: relative;
  cursor: pointer;
}

.filter-options .underline {
  position: absolute;
  bottom: -2px;
  left: 0;
  width: 100%;
  height: 2px;
  background-color: red;
}

.filter-options .active {
  font-weight: bold;
  color: red;
}

.red-box {
  background-color: red;
  color: white;
  padding: 10px 20px;
  cursor: pointer;
}

.input-box {
  margin-left: 20px;
}

.input-box input {
  padding: 5px;
  margin-right: 10px;
}

.input-box button {
  padding: 5px 10px;
  background-color: blue;
  color: white;
  cursor: pointer;
}

.course-grid {
  position: absolute;
  top: calc(100% + 20px); /* 在 bottom-bar 正下方 20px */
  left: 0;
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  row-gap: 20px;
  column-gap: 50px;
  margin-left: 180px;
}
</style>
