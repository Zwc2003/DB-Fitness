<template>
  <div aria-label="A complete example of page header">
    <el-page-header @back="onBack">
      <template #breadcrumb>
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ name: 'HomeView' }">
            FitFit
          </el-breadcrumb-item>
          <el-breadcrumb-item :to="{ name: 'HomeView' }">
            健身课程
          </el-breadcrumb-item>
          <el-breadcrumb-item>我的教学</el-breadcrumb-item>
        </el-breadcrumb>
      </template>
      <template #content>
        <div class="flex items-center">
          <el-avatar
            class="mr-3"
            :size="32"
            src="https://tse2-mm.cn.bing.net/th/id/OIP-C.xWtwoLeZd-P4SoSZOMGyogHaHZ?w=206&h=205&c=7&r=0&o=5&pid=1.7"
          />
          <span class="text-large font-600 mr-3">王教练</span>
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

      <div class="poem-display">
        <div class="poem-content">
          <span class="poem-text" v-if="selectedPoem">
            {{ selectedPoem.text }}
          </span>
        </div>
        <div class="author-container" v-if="selectedPoem">
          <span class="author-title">
            {{ selectedPoem.author }} - {{ selectedPoem.title }}
          </span>
        </div>
      </div>
    </el-page-header>
  </div>
  <div class="empty-row"></div>
  <div class="aaa">
    <div class="course-calender">
      <div class="my-course-title">我的课堂</div>
      <div class="coursee-list">
        <TeachCard
          v-for="(teachcourse, index) in teachcourses"
          :key="index"
          :courseName="teachcourse.name"
          :courseProgress="teachcourse.progress"
          :courseTime="teachcourse.time"
        />
        <!-- 橙色按钮，带有白色粗加号 -->
        <el-button type="primary" @click="showModal = true" class="plus-button">
          <i class="el-icon-plus">
            <el-icon class="el-icon-my-circle-plus">
              <Plus />
            </el-icon>
          </i>
        </el-button>

        <!-- 弹出的表单 -->
        <el-dialog v-model="showModal" title="发布新课程" width="50%">
          <el-form :model="form">
            <el-form-item label="课程名称">
              <el-input
                v-model="form.name"
                placeholder="请输入课程名称 普拉提"
              ></el-input>
            </el-form-item>
            <el-form-item label="进度">
              <el-input
                v-model="form.progress"
                placeholder="请输入课程进度 1节课/20节课"
              ></el-input>
            </el-form-item>
            <el-form-item label="时间">
              <el-input
                v-model="form.time"
                placeholder="请输入开始到结束时间 2021.01.01-2022.01.01"
              ></el-input>
            </el-form-item>
          </el-form>
          <template #footer>
            <el-button @click="showModal = false">取消</el-button>
            <el-button type="primary" @click="saveCourse">保存</el-button>
          </template>
        </el-dialog>
      </div>
      <div class="course-list">
        <h2>您今日的教学任务</h2>
        <el-row
          v-for="(course, index) in courses"
          :key="index"
          class="course-item"
        >
          <div class="course-item">
            <!-- 圆形 -->
            <div
              :class="['circle', circleColors[index % circleColors.length]]"
            ></div>
            <!-- 课程名称 -->
            <div class="course-name">{{ course.name }}</div>
            <!-- 上课时间 -->
            <div class="course-time">{{ course.time }}</div>
            <!-- 状态框 -->
            <div class="status-box">
              <el-tag :type="getStatusType(course)">
                {{ getStatusText(course) }}
              </el-tag>
            </div>
          </div>
        </el-row>
      </div>
    </div>

    <div class="chart-container">
      <h2>我的课程报名人数</h2>
      <div class="chart-content">
        <div id="line-chart" ref="lineChart" class="chart"></div>
        <div class="stage-info">
          <h3>称号：{{ currentStageName }}</h3>
          <p>{{ currentStageDescription }}</p>
          <p>总学生: {{ totalWorkoutHours }} 位</p>
          <p>下一称号需要: {{ nextStageUnlockHours }} 学生</p>
        </div>
      </div>
      <p class="encouragement">{{ encouragementMessage }}</p>
    </div>
  </div>
</template>

<script>
import { ref, reactive } from "vue";
import * as echarts from "echarts";
import { ElNotification as notify } from "element-plus";
import { computed } from "vue";
import TeachCard from "@/components/TeachCard.vue";

export default {
  components: {
    TeachCard,
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
      showModal: false,
      form: {
        name: "",
        progress: "",
        time: "",
      },
      teachcourses: [
        {
          name: "普拉提",
          progress: "25节/32节课",
          time: "2017.05.01-2018.06.05",
        },
        {
          name: "瑜伽",
          progress: "15节课/20节课",
          time: "2018.01.01-2018.12.01",
        },
        {
          name: "运动",
          progress: "15节课/20节课",
          time: "2018.01.01-2018.12.01",
        },
      ],
      courseData: [
        { date: "2024-08-01", duration: 3 },
        { date: "2024-08-02", duration: 5 },
        { date: "2024-08-03", duration: 9 },
        { date: "2024-08-04", duration: 10 },
        { date: "2024-08-05", duration: 2 },
        { date: "2024-08-06", duration: 4 },
        { date: "2024-08-07", duration: 2 },
        { date: "2024-08-08", duration: 5 },
        { date: "2024-08-09", duration: 1 },
        { date: "2024-08-10", duration: 2 },
      ],
      encouragementMessage: "",
      totalWorkoutHours: 0,
      stages: [
        {
          name: "教练",
          unlockHours: 0,
          description: "初窥门径，教学渐入佳。",
        },
        {
          name: "师父",
          unlockHours: 10,
          description: "熟能生巧，技艺日臻完善。",
        },
        {
          name: "堂主",
          unlockHours: 30,
          description: "炉火纯青，传道授业解惑。",
        },
        {
          name: "教头",
          unlockHours: 60,
          description: "登峰造极，桃李满天下。",
        },
        {
          name: "宗师",
          unlockHours: 100,
          description: "返璞归真，大道至简。",
        },
      ],
      // 古诗数据
      poems: [
        {
          text: "蹴鞠屡过飞鸟上，秋千竞出垂杨里。 少年分日作遨游，不用清明兼上巳。",
          author: "王维",
          title: "《寒食城东即事》",
        },
        {
          text: "十月冰床遍九城，游人曳去一毛轻。风和日暖时端坐，疑在琉璃世界行。",
          author: "杨静亭",
          title: "《都门杂咏》",
        },
        {
          text: "少年骑马入咸阳，鹘似身轻蝶似狂。蹴鞠场边万人看，秋千旗下一春忙。",
          author: "陆游",
          title: "《晚春感事》",
        },
        {
          text: "黄沙连海路无尘，边草长枯不见春。日暮拂云堆下过，马前逢著射雕人。",
          author: "杜牧",
          title: "《游边》",
        },
        {
          text: "㸌如羿射九日落，矫如群帝骖龙翔。来如雷霆收震怒，罢如江海凝清光。",
          author: "杜甫",
          title: "《观公孙大娘弟子舞剑器行》",
        },
      ],
      selectedPoem: null, // 保存选中的诗

      courses: [
        { name: "瑜伽", time: "10:00 - 11:00", attended: false },
        { name: "普拉提", time: "13:00 - 14:00", attended: true },
        { name: "跑步", time: "18:00 - 19:00", attended: false },
      ],
      circleColors: ["blue", "yellow", "red", "orange", "green"],
    };
  },

  mounted() {
    // 组件挂载后立即选择一首随机古诗
    this.selectRandomPoem();
    //上课时间统计图
    this.initChart();
    //生成鼓励的话
    this.generateEncouragementMessage();
  },

  methods: {
    // 随机生成一首诗
    selectRandomPoem() {
      const index = Math.floor(Math.random() * this.poems.length);
      this.selectedPoem = this.poems[index];
    },

    //初始化折线图
    initChart() {
      const chart = echarts.init(this.$refs.lineChart);

      const option = {
        title: {
          text: "",
          left: "center",
        },
        xAxis: {
          type: "category",
          data: this.courseData.map((item) => item.date),
          boundaryGap: false,
        },
        yAxis: {
          type: "value",
          name: "每日新报名人数",
        },
        series: [
          {
            data: this.courseData.map((item) => item.duration),
            type: "line",
            smooth: true,
            areaStyle: {},
            lineStyle: {
              color: "#409EFF",
            },
            itemStyle: {
              color: "#409EFF",
            },
          },
        ],
      };

      chart.setOption(option);
    },

    saveCourse() {
      // 将用户输入的课程信息保存到 teachCourses 数组中
      this.teachcourses.push({
        name: this.form.name,
        progress: this.form.progress,
        time: this.form.time,
      });

      // 清空表单
      this.form.name = "";
      this.form.progress = "";
      this.form.time = "";

      // 关闭弹窗
      this.showModal = false;
    },

    //鼓励话语生成函数
    generateEncouragementMessage() {
      this.totalWorkoutHours = this.courseData.reduce(
        (sum, item) => sum + item.duration,
        0
      );

      if (this.totalWorkoutHours > 100) {
        this.encouragementMessage = "心手相应，以武入道！";
      } else if (this.totalWorkoutHours > 50) {
        this.encouragementMessage = "厚积薄发，功到自然成，你会越来越棒！";
      } else {
        this.encouragementMessage =
          "教学相长，砺志铸魂，开始总是最难的，坚持下去，你会看到改变！";
      }
    },

    // 继续学习按钮的点击事件处理函数
    handleContinue() {
      // 点击时执行的操作，例如跳转
      console.log("继续学习按钮被点击");
    },

    getStatusText(course) {
      const currentTime = new Date();
      const [startHour, startMinute] = course.time
        .split(" - ")[0]
        .split(":")
        .map(Number);
      const startTime = new Date();
      startTime.setHours(startHour, startMinute);

      const [endHour, endMinute] = course.time
        .split(" - ")[1]
        .split(":")
        .map(Number);
      const endTime = new Date();
      endTime.setHours(endHour, endMinute);

      if (currentTime < startTime) {
        return "提醒我";
      } else if (currentTime > endTime && course.attended) {
        return "已完课";
      } else if (currentTime > endTime && !course.attended) {
        return "补课";
      } else {
        return "进行中";
      }
      return "";
    },
    getStatusType(course) {
      const currentTime = new Date();
      const [startHour, startMinute] = course.time
        .split(" - ")[0]
        .split(":")
        .map(Number);
      const startTime = new Date();
      startTime.setHours(startHour, startMinute);

      if (currentTime < startTime) {
        return "warning";
      } else {
        return "success";
      }
    },
  },

  computed: {
    currentStage() {
      return this.stages
        .slice()
        .reverse()
        .find((stage) => this.totalWorkoutHours >= stage.unlockHours);
    },
    currentStageName() {
      return this.currentStage?.name || "";
    },
    currentStageDescription() {
      return this.currentStage?.description || "";
    },
    currentStageImage() {
      return this.currentStage?.image || "";
    },
    nextStageUnlockHours() {
      const nextStage = this.stages.find(
        (stage) => this.totalWorkoutHours < stage.unlockHours
      );
      return nextStage ? nextStage.unlockHours : "已达最高境界";
    },
  },
};

const onBack = () => {
  notify("Back");
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
  margin-top: 40px;
  margin-left: -50px;
}

.poem-entry {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.poem-content {
  flex: 1;
  margin-right: 10px; /* 保持一些间距 */
  margin-top: 20px;
  margin-bottom: 30px;
}

.poem-text {
  margin: 0;
  font-weight: bold;
}

.author-container {
  flex: 0 0 auto;
  display: flex;
  justify-content: flex-end;
}

.author-title {
  margin: 0;
  font-weight: bold;
}

.empty-row {
  margin-top: 20px; /* 空行的高度 */
  height: 0;
  overflow: hidden;
}

.my-course-title {
  margin-left: -73px;
  color: #79bbff;
  font-weight: bold;
  font-size: 15px;
  margin-bottom: 10px;
}

.course-calender {
  display: flex;
  justify-content: space-between;
  align-items: flex-start; /* 保证两个模块从同一高度开始 */
  padding: 20px;
}

.coursee-list,
.course-list {
  flex: 1;
  margin: 0 20px;
}

.coursee-list {
  display: flex;
  flex-direction: column;
  gap: 50px;
  padding: 10px;
}

.course-card {
  width: 300px;
  height: 150px;
}

.plus-button {
  background-color: orange;
  color: white;
  font-size: 24px;
  width: 100px;
  height: 40px;
  margin-left: 50%;
  border-radius: 5px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.plus-button .el-icon-plus {
  font-weight: bold;
}

.el-icon-my-circle-plus {
  font-weight: bold;
  font-size: larger;
}

/* 控制“今日课程列表”模块的宽度 */
.course-list {
  max-width: 35%;
  padding: 40px;
  background-color: #f9f9f9;
}

h2 {
  margin-bottom: 20px;
  font-weight: bold;
}

.course-item {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.circle {
  width: 30px;
  height: 30px;
  background-color: blue;
  border-radius: 50%;
  margin-right: 20px; /* 控制圆形与课程名称的间隔 */
}

.course-name {
  flex: 1;
  margin-right: 20px; /* 控制名称与时间的间隔 */
  font-size: 1rem;
  width: 60px;
}

.course-time {
  flex: 2;
  margin-right: 20px; /* 控制时间与状态框的间隔 */
}

.status-box {
  flex: 1;
}
/* 定义不同颜色的圆形样式 */
.blue {
  background-color: #79bbff;
}

.yellow {
  background-color: #e6e8eb;
}

.red {
  background-color: #f89898;
}

.orange {
  background-color: #b88230;
}

.green {
  background-color: #95d475;
}

.chart-container {
  max-width: 1200px;
  margin-top: 20px;
}

.chart-container h2 {
  margin-left: -50px;
  color: #79bbff;
  font-weight: bold;
  font-size: 16px;
  text-align: left;
  margin-bottom: -2%;
}

.chart-content {
  max-width: 1200px;
  display: flex;
  justify-content: space-between;
  margin-top: 20px;
}

.chart {
  width: 1000px;
  height: 300px;
  margin-bottom: 20px;
}

.stage-info {
  font-family: "Noto Serif SC", serif;

  margin-top: 50px;
  width: 300px;
  margin-left: -50px;
  text-align: left;
  font-size: 1.2rem;
}

.stage-info p {
  line-height: 1.8; /* 或者使用 margin-bottom 调整 */
  margin-bottom: 10px;
  color: #337ecc;
  font-weight: bond;
}

.stage-info h3 {
  color: blueviolet;
  line-height: 1.8; /* 或者使用 margin-bottom 调整 */
  margin-bottom: 10px;
  font-weight: bold;
}

.encouragement {
  font-family: "ZCOOL XiaoWei", serif;
  margin-top: -5%;
  margin-left: 20%;
  font-size: 1.5em;
  color: #333;
  font-weight: bold;
}
</style>
