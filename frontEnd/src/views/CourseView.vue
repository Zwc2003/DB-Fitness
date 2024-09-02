<template>
  <navigation-bar />
  <div class="trans">
    <router-link v-if="userRole === 'user'" to="/userhome">
      <el-button type="primary">我的主页</el-button>
    </router-link>
    <router-link v-else-if="userRole === 'coach'" to="/publishcourse">
      <el-button type="primary">我的主页</el-button>
    </router-link>
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
              <b class="boldd">{{ option.label }}</b>
              <div
                v-if="
                  hoveredOption === option.key || selectedOption === option.key
                "
                class="underline"
              ></div>
            </span>
          </div>

          <div class="red-box" @click="showRecommendation">
            想知道什么课程适合我
          </div>
        </div>
      </div>
    </div>
    <div class="search-bar">
      <!-- 搜索框 -->
      <el-input
        v-model="searchKey"
        placeholder="搜索健身课程"
        style="width: 300px"
        @keyup.enter="searchCourses"
      >
        <template #append>
          <el-button @click="searchCourses">搜索</el-button>
        </template>
      </el-input>

      <!-- 价格区间选择器 -->
      <el-select
        v-model="selectedPriceRange"
        placeholder="选择价格区间"
        style="margin-left: 16px; width: 180px"
        @change="updatePriceRange"
      >
        <el-option label="0-30活力币" :value="{ min: 0, max: 30 }"></el-option>
        <el-option
          label="30-60活力币"
          :value="{ min: 30, max: 60 }"
        ></el-option>
        <el-option
          label="60-100活力币"
          :value="{ min: 60, max: 100 }"
        ></el-option>
      </el-select>
    </div>
    <!-- 课程内容展示区域 -->
    <div class="course-grid">
      <CourseHome
        v-for="(course, index) in courses"
        :key="index"
        :coursePhotoUrl="course.coursePhotoUrl"
        :courseName="course.courseName"
        :courseStartTime="course.courseStartTime"
        :courseEndTime="course.courseEndTime"
        :courseGrade="course.courseGrade"
        :coursePrice="course.coursePrice"
        :courseFeatures="course.courseFeatures"
        :courseDescription="course.courseDescription"
        :instructorName="course.coachName"
        :instructorHonors="course.instructorHonors"
        :instructorImage="course.iconURL"
        :classTime="course.classTime"
        :features="course.features"
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
import CourseHome from "../components/CourseHome.vue";
import axios from "axios";

export default {
  components: {
    CourseHome,
  },
  data() {
    return {
      searchKey: "", // 搜索关键词
      minPrice: 0, // 最小价格
      maxPrice: 10000, // 最大价格
      searchType: "", // 课程类型
      userRole: "普通用户",
      courses: [],
      isDialogVisible: false,
      dialogMessage: "",
      hoveredOption: null,
      selectedOption: null,
      filterOptions: [
        { key: "全部", label: "全部课程" },
        { key: "塑身", label: "塑身课程" },
        { key: "高强度", label: "高强度间歇课程" },
        { key: "青少年", label: "青少年趣味课程" },
      ],
    };
  },
  methods: {
    // 更新价格区间参数
    updatePriceRange(value) {
      this.minPrice = value.min;
      this.maxPrice = value.max;
      this.searchCourses();
    },
    getCourseValue(courseName) {
    switch (courseName) {
      case "高强度间歇":
        return 1;
      case "低强度塑形":
        return 2;
      case "儿童趣味课":
        return 3;
      case "有氧训练":
        return 4;
      default:
        return 0; // 如果输入的课程名称不在列表中，返回0或其他默认值
    }
  },
    // 搜索课程
    searchCourses() {
      // 调用后端API进行课程筛选
      axios
        .get(`http://localhost:8080/api/Course/SearchCourse?`,{params:{
          token:localStorage.getItem("token"),
          keyword: this.searchKey,
          typeID: this.getCourseValue(this.searchType),
          minPrice: this.minPrice,
          maxPrice: this.maxPrice
        }})
        .then((response) => {
          this.courses =[];
          this.courses = response.data.map((item) => {
            if (item.features) {
              item.features = item.features.split("#");
            }
            return item;
          });
          console.log(this.courses);
          //this.filteredCourses = this.courses; // 默认展示所有课程
        })
        .catch((error) => {
          console.error("Error fetching courses:", error);
          ElNotification({
            title: "错误",
            message: "获取所有课程时发生错误，请稍后再试。",
            type: "error",
          });
          throw error;
        });
    },

    //关键词搜索函数
    selectOption(option) {
      this.selectedOption = option;
      this.showInput = ["fitness", "price", "category"].includes(option);
      this.inputValue = "";
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
    //axios
    //大厅获取课程API
    async fetchCourses() {
      const token = localStorage.getItem("token");
      return axios
        .get(`http://localhost:8080/api/Course/GetAllCourse?token=${token}`)
        .then((response) => {
          this.courses = response.data.map((item) => {
            if (item.features) {
              item.features = item.features.split("#");
            }
            return item;
          });
          console.log(this.courses);
          //this.filteredCourses = this.courses; // 默认展示所有课程
        })
        .catch((error) => {
          console.error("Error fetching courses:", error);
          ElNotification({
            title: "错误",
            message: "获取所有课程时发生错误，请稍后再试。",
            type: "error",
          });
          throw error;
        });
    },
  },
  mounted() {
    this.fetchCourses();
    this.userRole = localStorage.getItem("role");
  },
};
</script>

<style scoped>
.boldd {
  font-weight: bolder;
}
.trans {
  position: absolute;
  top: 13%;
  left: 74%;
  z-index: 9999;
  width: 300px;
}

.trans .el-button {
  padding: 10px 20px;
  font-size: 16px;
  width: 130px;
  height: 40px;
}

.hero-section {
  position: relative;
  width: 160vh;
  height: 80vh;
  background-image: url("https://www.lesmills.com.cn/uploads/20230316/f3e86ab4e56683a1481b0c0b4562d799.png");
  background-size: cover;
  background-position: center;
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

.search-bar {
  position: absolute;
  right: 5%;
  top: calc(100% + 10px);
  display: flex;
  align-items: center;
}

.course-grid {
  position: absolute;
  top: calc(100% + 25px); /* 在 bottom-bar 正下方 20px */
  left: 0;
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  row-gap: 20px;
  column-gap: 50px;
  margin-left: 180px;
}
</style>
