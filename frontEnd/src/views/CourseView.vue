<template>
  <navigation-bar />
  <div class="trans">
    <router-link v-if="userRole === '普通用户'" to="/userhome">
      <el-button type="primary">我的主页</el-button>
    </router-link>
    <router-link v-else-if="userRole === '教练'" to="/publishcourse">
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
        :instructorName="course.instructorName"
        :instructorHonors="course.instructorHonors"
        :instructorImage="course.instructorImage"
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

export default {
  components: {
    CourseHome,
  },
  data() {
    return {
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
    toggleRole() {
      this.userRole = this.userRole === "普通用户" ? "教练" : "普通用户";
    },
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

    //大厅获取课程API
    async fetchCourses() {
      try {
        const response = await fetch("/api/Course/GetAllCourse", {
          method: "GET",
        });

        if (!response.ok) {
          throw new Error("Failed to fetch courses");
        }

        const coursesData = await response.json();

        this.courses = coursesData.map((course) => ({
          coursePhotoUrl: course.coursePhotoUrl || "",
          courseName: course.courseName || "Unnamed Course",
          courseDescription:
            course.courseDescription || "No description available",
          courseStartTime: course.courseStartTime || "",
          courseEndTime: course.courseEndTime || "",
          courseGrade: course.courseGrade || 0,
          coursePrice: course.coursePrice || 0,
          instructorName: course.instructorName || "Unknown Instructor",
          instructorHonors: course.instructorHonors || "No honors available",
          instructorImage: course.instructorImage || "",
          features: course.features || [],
        }));
      } catch (error) {
        console.error("Error:", error);
      }
    },
  },
  mounted() {
    this.fetchCourses();
    // 假设这是从后端获取的课程数据
    (this.courses = [
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/static/index_news/images/temp/courseimg.jpg",
        courseName: "举重阻尼训练",
        courseDescription:
          "举重阻尼训练是一种结合了力量训练与控制力训练的高效健身课程。提高肌肉力量和爆发力，增强肌肉控制和稳定性，本课程适合各种健身水平的人士，无论是初学者还是经验丰富的运动员，都可以在教练的指导下找到适合自己的训练强度。",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-08",
        courseGrade: "4",
        coursePrice: "50",
        instructorName: "朴男",
        instructorHonors:
          "男Krisun，极限运动员，世界纪录保持者，抖音创作者，其抖音号“朴男Krisun”，拥有粉丝169.3万",
        instructorImage: "/images/pn.jpg",
        features: ["感受力量涌现", "增强肌肉控制", "训练全身各处"],
        classTime: "10:00-11:00",
        isbooked: 0,
      },
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/uploads/20211207/d5d8d0860359243eee20bc507bf2c231.jpg",
        courseName: "平衡与专注、持久与柔软",
        courseDescription:
          "持久与柔软瑜伽课程旨在帮助学员通过瑜伽的练习，提升身心的平衡，增强专注力，同时提高身体的持久力和柔软度。本课程适合所有级别的瑜伽爱好者，无论是初学者还是有经验的练习者，都可以在这个课程中找到适合自己的挑战和放松",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-09",
        courseGrade: "2",
        coursePrice: "10",
        instructorName: "帕梅拉",
        instructorHonors: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
        instructorImage: "/images/p.jpg",
        features: ["提升身心平衡", "助力有氧健身", "维护心理健康"],
        classTime: "10:00-11:00",
        isbooked: 0,
      },
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/uploads/20211207/30c00cdf6752eeda8356ecd01893dabd.jpg",
        courseName: "30到45分钟核心训练",
        courseDescription:
          "核心肌群是身体的中心力量，对于维持姿势、提高运动表现和预防受伤至关重要。这门30到45分钟的核心训练课程专为忙碌的现代人设计，旨在通过高强度、集中的训练，快速有效地加强你的核心力量和稳定性。",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-09",
        courseGrade: "3",
        coursePrice: "30",
        instructorName: "鹿晨辉",
        instructorHonors: "鹿晨辉，国家级健美一级裁判和国家职业健身培训师。",
        instructorImage: "/images/l.jpg",
        features: ["感受力量涌现", "增强肌肉控制", "减少受伤风险"],
        classTime: "10:00-11:00",
        isbooked: 0,
      },
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/uploads/20211207/e4466eb6aace56fdaff24fc4c3c318af.jpg",
        courseName: "瘦身、紧致、有型",
        courseDescription:
          "这门课程专为追求健康、紧致身材的学员设计。通过结合有氧运动、力量训练和功能性练习，本课程旨在帮助学员减少体脂、塑造肌肉线条，同时提升整体的身体力量和灵活性，让身体更加有型。",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-09",
        courseGrade: "2",
        coursePrice: "20",
        instructorName: "帕梅拉",
        instructorHonors: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
        instructorImage: "/images/p.jpg",
        features: ["增强心肺功能", "助力有氧健身", "塑造紧致线条"],
        classTime: "10:00-11:00",
        isbooked: 0,
      },
    ]),
      (this.filteredCourses = this.courses); // 默认展示所有课程
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
