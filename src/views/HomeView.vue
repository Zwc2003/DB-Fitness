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
          <el-button>vip2</el-button>
          <el-button type="primary" class="ml-2" @click="toggleCartSidebar"
            >课程购物车</el-button
          >
        </div>
      </template>

      <!-- 右边栏 -->
      <transition name="slide">
        <div v-if="isCartVisible" class="cart-sidebar">
          <!-- 收回箭头 -->
          <el-icon style="font-size: 20px"><ShoppingTrolley /></el-icon>
          <b>课程购物车</b>
          <div class="cart-header">
            收起
            <el-icon @click="toggleCartSidebar" style="font-size: 24px">
              <Right />
            </el-icon>
          </div>
          <div class="cart-titles">
            <span>课程名称</span>
            <span>课程单价</span>
            <span>课程时间</span>
            <span>操作</span>
          </div>
          <!-- 购物车内容 -->
          <div
            v-for="(course, index) in cartCourses"
            :key="index"
            class="cart-item"
          >
            <el-checkbox v-model="course.selected" />
            <img :src="course.image" alt="课程图片" class="course-image" />
            <span>{{ course.name }}</span>
            <span>￥{{ course.price }}</span>
            <span>{{ course.time }}</span>
            <span class="delete-text" @click="removeCourse(index)">删除</span>
          </div>
          <!-- 统计信息 -->
          <div class="cart-summary">
            <span>
              共{{ cartCourses.length }}个课程，已选择{{
                selectedCourses.length
              }}个，课程共计：
              <strong class="total-price">￥{{ totalPrice }}</strong>
            </span>
            <el-button type="primary" class="checkout-button"
              >下单结算</el-button
            >
          </div>
        </div>
      </transition>
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
import { Search } from "@element-plus/icons-vue";
import { ref, computed } from "vue";
// const input1 = ref("");
export default {
  components: {
    CourseHome,
  },
  setup() {
    const isCartVisible = ref(false);

    const cartCourses = ref([
      // 示例课程数据
      {
        image:
          "https://tse2-mm.cn.bing.net/th/id/OIP-C.GIvUZUnbp2xh7xKqzV5CPgHaE7?w=268&h=180&c=7&r=0&o=5&pid=1.7",
        name: "运动健身",
        price: 100,
        time: "2022 06 07-2023 06 07",
        selected: true,
      },
      {
        image:
          "https://tse3-mm.cn.bing.net/th/id/OIP-C.VqoEEkEfYw9eANM7GUlz3AHaEo?w=276&h=180&c=7&r=0&o=5&pid=1.7",
        name: "普拉提",
        price: 200,
        time: "2022 01 01-2023 01 01",
        selected: false,
      },
      {
        image:
          "https://tse3-mm.cn.bing.net/th/id/OIP-C.oXrQec5a4Au63MDb2vLCRwHaE8?w=246&h=180&c=7&r=0&o=5&pid=1.7",
        name: "长跑",
        price: 150,
        time: "2022 11 13-2023 11 13",
        selected: true,
      },
    ]);

    const toggleCartSidebar = () => {
      isCartVisible.value = !isCartVisible.value;
    };

    const removeCourse = (index) => {
      cartCourses.value.splice(index, 1);
    };

    const selectedCourses = computed(() =>
      cartCourses.value.filter((course) => course.selected)
    );

    const totalPrice = computed(() =>
      selectedCourses.value.reduce((sum, course) => sum + course.price, 0)
    );

    return {
      isCartVisible,
      cartCourses,
      toggleCartSidebar,
      removeCourse,
      selectedCourses,
      totalPrice,
    };
  },

  data() {
    return {
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
      // filteredCourses: [],
      isDialogVisible: false,
      dialogMessage: "",
      hoveredOption: null,
      selectedOption: null,
      showInput: false,
      inputValue: "",
      filterOptions: [
        { key: "all", label: "全部课程" },
        { key: "fitness", label: "课程关键词" },
        { key: "price", label: "课程价格" },
        { key: "category", label: "课程类别" },
      ],
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
.cart-sidebar {
  position: fixed;
  top: 0;
  right: 0;
  width: 400px;
  height: 100%;
  background-color: #fff;
  box-shadow: -2px 0 5px rgba(0, 0, 0, 0.1);
  padding: 20px;
  z-index: 1000;
}

.cart-header {
  display: flex;
  justify-content: flex-end;
}

.cart-titles {
  margin-top: 20px;
  display: flex;
  justify-content: space-between;
  margin-bottom: 10px;
  font-weight: bold;
}

.cart-item {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.cart-item > * {
  margin-right: 20px; /* 调整子元素之间的间距 */
}

.course-image {
  width: 100px;
  height: 100px;
  margin-left: 15px;
}

.delete-text {
  color: red;
  cursor: pointer;
}

.delete-text:hover {
  text-decoration: underline;
}

.cart-summary {
  margin-top: 20px;
  font-size: 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.total-price {
  color: red;
  font-weight: bold;
}

.checkout-button {
  background-color: #409eff;
  color: #fff;
  margin-left: auto;
}

.slide-enter-active,
.slide-leave-active {
  transition: all 0.3s ease;
}

.slide-enter,
.slide-leave-to /* .slide-leave-active in <2.1.8 */ {
  transform: translateX(100%);
}
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
