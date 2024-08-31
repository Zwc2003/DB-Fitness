<template>
  <CartSidebar
    :cartCourses="cartCourses"
    :isCartVisible="isCartVisible"
    @update:isCartVisible="isCartVisible = $event"
    @removeCourse="removeCourse"
    class="cart-sidebarr"
  />
  <div aria-label="A complete example of page header">
    <el-page-header @back="onBack">
      <template #breadcrumb>
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: './page-header.html' }">
            FitFit
          </el-breadcrumb-item>
          <el-breadcrumb-item :to="{ name: 'course' }">
            健身课程
          </el-breadcrumb-item>
          <el-breadcrumb-item>我的课程</el-breadcrumb-item>
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
      <div class="my-course-title">我的课程</div>
      <div class="coursee-list">
        <CourseCard
          v-for="(usercourse, index) in usercourses"
          :key="index"
          :courseName="usercourse.name"
          :courseProgress="usercourse.progress"
          :startTime="usercourse.coursestartTime"
          :endTime="usercourse.courseendTime"
        />
      </div>
      <div class="course-list">
        <h2>您今日的课程列表</h2>
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
      <h2>我的活跃度</h2>
      <div class="chart-content">
        <div id="line-chart" ref="lineChart" class="chart"></div>
        <div class="stage-info">
          <h3>称号：{{ currentStageName }}</h3>
          <p>{{ currentStageDescription }}</p>
          <p>总健身时间: {{ totalWorkoutHours }} 小时</p>
          <p>下一阶段需要: {{ nextStageUnlockHours }} 小时</p>
        </div>
      </div>
      <p class="encouragement">{{ encouragementMessage }}</p>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from "vue";
import * as echarts from "echarts";
import { ElNotification as notify } from "element-plus";
import { dataType } from "element-plus/es/components/table-v2/src/common";
import { computed } from "vue";
import CourseCard from "../components/CourseCard.vue";
import CartSidebar from "../components/CartSidebar.vue";

export default {
  components: {
    CourseCard,
    CartSidebar,
  },

  data() {
    return {
      usercourses: [
        {
          name: "普拉提",
          progress: "25节/32节课",
          coursestartTime: "2017.05.01",
          courseendTime: "2018.06.05",
        },
        {
          name: "瑜伽",
          progress: "15节课/20节课",
          coursestarTime: "2018.01.01",
          courseendTime: "2018.12.01",
        },
        {
          name: "运动",
          progress: "15节课/20节课",
          coursestartTime: "2018.01.01",
          courseendTime: "2024.12.01",
        },
      ],
      courseData: [
        { date: "2024-08-01", duration: 1.5 },
        { date: "2024-08-02", duration: 2 },
        { date: "2024-08-03", duration: 0.5 },
        { date: "2024-08-04", duration: 1 },
        { date: "2024-08-05", duration: 2.5 },
        { date: "2024-08-06", duration: 1.5 },
        { date: "2024-08-07", duration: 2 },
        { date: "2024-08-08", duration: 0.5 },
        { date: "2024-08-09", duration: 1 },
        { date: "2024-08-10", duration: 2.5 },
      ],
      encouragementMessage: "",
      totalWorkoutHours: 0,
      stages: [
        {
          name: "武韵新秀",
          unlockHours: 0,
          description: "入门之旅，强身健体的第一步。",
        },
        {
          name: "气功精进",
          unlockHours: 30,
          description: "初窥门径，气功渐入佳境。",
        },
        {
          name: "鹤舞长空",
          unlockHours: 80,
          description: "身轻如燕，身姿矫健如鹤。",
        },
        {
          name: "金刚不坏",
          unlockHours: 150,
          description: "强筋健骨，体魄如钢。",
        },
        {
          name: "神行百变",
          unlockHours: 250,
          description: "速度与敏捷的完美结合。",
        },
        {
          name: "易筋洗髓",
          unlockHours: 400,
          description: "内外兼修，境界升华。",
        },
        {
          name: "大宗师",
          unlockHours: 600,
          description: "登峰造极，出神入化。",
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
      isCartVisible: false,
      cartCourses: [
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
      ],
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
    onBack() {
      this.$router.go(-1); // 或者使用 window.history.back() 如果没有 Vue Router
    },
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
          name: "每日课程在线时间 (小时)",
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

    //鼓励话语生成函数
    generateEncouragementMessage() {
      this.totalWorkoutHours = this.courseData.reduce(
        (sum, item) => sum + item.duration,
        0
      );

      if (this.totalWorkoutHours > 1000) {
        this.encouragementMessage =
          "太棒拉！你已经是一位业余健身的天花板了，期待你的自律与积极的精神去引领更多的人参与健身，继续加油！";
      } else if (this.totalWorkoutHours > 500) {
        this.encouragementMessage =
          "不错哦！转眼间，轻舟已过万重山，保持这个节奏，你会越来越棒！";
      } else {
        this.encouragementMessage =
          "开始锻炼总是最难的，坚持下去，你会看到改变！";
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

    removeCourse(index) {
      this.cartCourses.splice(index, 1);
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
</script>

<style scoped>
.cart-sidebarr {
  position: absolute;
  top: 10%;
  right: 5%;
  height: 4%;
  background-color: #fff;
  box-shadow: -2px 0 5px rgba(0, 0, 0, 0.1);
  padding: 0px;
}

.el-page-header {
  width: 1300px;
  margin-top: -380px;
  margin-right: -1200px;
}

.poem-entry {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.poem-content {
  flex: 1;
  margin-left: -780px;
  margin-right: 10px;
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
  margin-left: -100px;
  color: #79bbff;
  font-weight: bold;
  font-size: 15px;
  margin-bottom: 10px;
}

.course-calender {
  margin-top: 500px;
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

.course-list {
  max-width: 35%; /* 控制“今日课程列表”模块的宽度 */
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
  /* display: flex; */
  /* flex-direction: column; */
  /* align-items: flex-start; */
  max-width: 1200px;
  margin-top: 20px;

  /* text-align: center; */
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
  margin-left: -100px;
  font-size: 1.5em;
  color: #333;
  font-weight: bold;
}
</style>
