<template>
  <div class="aaa">
    <div aria-label="A complete example of page header">
      <div class="kkk">
        <el-card class="vitality-card" shadow="hover">
          <div class="vitality-container">
            <el-icon>
              <Coin />
            </el-icon>
            <span>活力币：</span>
            <span>{{ vitalityCoins }}</span>
          </div>
        </el-card>
        <CartSidebar
          :cartCourses="cartCourses"
          :isCartVisible="isCartVisible"
          :usercourses="usercourses"
          @update:isCartVisible="handleCartVisibility"
          @removeCourse="removeCourseFromCart"
          @update:usercourses="updateUserCourses"
          @checkout="handleCheckout"
          class="cart"
        />
      </div>
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
          <div class="flexitems-center">
            <el-avatar
              class="mr-3"
              :size="32"
              src="https://ts3.cn.mm.bing.net/th?id=OIP-C.9khWcYup3srhgw3V1fi7-QHaHa&w=250&h=250&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2"
            />
            <span>李华</span>
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

    <div class="course-calender">
      <div class="my-course-title">我的课程</div>
      <div class="coursee-list">
        <CourseCard
          v-for="(usercourse, index) in usercourses"
          :key="index"
          :coursePhotoUrl="usercourse.coursePhotoUrl"
          :courseName="usercourse.courseName"
          :courseDescription="usercourse.courseDescription"
          :courseStartTime="usercourse.courseStartTime"
          :courseEndTime="usercourse.courseEndTime"
          :courseGrade="usercourse.courseGrade"
          :coursePrice="usercourse.coursePrice"
          :courseProgress="usercourse.courseProgress"
          :features="usercourse.features"
          :instructorImage="usercourse.instructorImage"
          :instructorName="usercourse.instructorName"
          :instructorHonors="usercourse.instructorHonors"
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
import { ElMessage, ElMessageBox } from "element-plus"; // 引入Element Plus的消息提示组件和消息框组件

export default {
  components: {
    CourseCard,
    CartSidebar,
  },

  data() {
    return {
      vitalityCoins: 100,
      usercourses: [
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
          courseProgress: "25节/32节课",
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
          instructorHonors:
            "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
          instructorImage: "/images/p.jpg",
          features: ["提升身心平衡", "助力有氧健身", "维护心理健康"],
          courseProgress: "25节/32节课",
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
          courseProgress: "25节/32节课",
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
          instructorHonors:
            "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
          instructorImage: "/images/p.jpg",
          features: ["增强心肺功能", "助力有氧健身", "塑造紧致线条"],
          courseProgress: "25节/32节课",
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
          coursePhotoUrl:
            "https://tse2-mm.cn.bing.net/th/id/OIP-C.GIvUZUnbp2xh7xKqzV5CPgHaE7?w=268&h=180&c=7&r=0&o=5&pid=1.7",
          courseName: "运动健身",
          courseDescription:
            "这是一门综合性的运动健身课程，旨在通过多样化的训练方法，全面提升学员的身体素质、力量、耐力和灵活性。课程结合了有氧运动、力量训练、核心锻炼和柔韧性训练，适合各种健身水平的学员。",
          courseStartTime: "2022.06.07",
          courseEndTime: "2024.08.09",
          courseGrade: "3",
          coursePrice: 50,
          instructorName: "朴男",
          instructorHonors:
            "男Krisun，极限运动员，世界纪录保持者，抖音创作者，其抖音号“朴男Krisun”，拥有粉丝169.3万",
          instructorImage: "/images/pn.jpg",
          features: ["增强心肺功能", "提升肌肉力量", "塑造紧致线条"],
          selected: true,
        },
        {
          coursePhotoUrl:
            "https://tse3-mm.cn.bing.net/th/id/OIP-C.VqoEEkEfYw9eANM7GUlz3AHaEo?w=276&h=180&c=7&r=0&o=5&pid=1.7",
          courseName: "普拉提",
          courseDescription:
            "这门课程专为追求健康、紧致身材的学员设计。通过结合有氧运动、力量训练和功能性练习，本课程旨在帮助学员减少体脂、塑造肌肉线条，同时提升整体的身体力量和灵活性，让身体更加有型。",
          courseStartTime: "2022.08.08",
          courseEndTime: "2024.08.09",
          courseGrade: "2",
          coursePrice: 20,
          instructorName: "帕梅拉",
          instructorHonors:
            "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
          instructorImage: "/images/p.jpg",
          features: ["增强心肺功能", "助力有氧健身", "塑造紧致线条"],
          selected: false,
        },
        {
          coursePhotoUrl:
            "https://tse3-mm.cn.bing.net/th/id/OIP-C.oXrQec5a4Au63MDb2vLCRwHaE8?w=246&h=180&c=7&r=0&o=5&pid=1.7",
          courseName: "长跑",
          courseDescription:
            "核心肌群是身体的中心力量，对于维持姿势、提高运动表现和预防受伤至关重要。这门30到45分钟的核心训练课程专为忙碌的现代人设计，旨在通过高强度、集中的训练，快速有效地加强你的核心力量和稳定性。",
          courseStartTime: "2022.08.08",
          courseEndTime: "2024.08.09",
          courseGrade: "2",
          coursePrice: 20,
          instructorName: "鹿晨辉",
          instructorHonors: "鹿晨辉，国家级健美一级裁判和国家职业健身培训师。",
          instructorImage: "/images/l.jpg",
          features: ["感受力量涌现", "增强肌肉控制", "减少受伤风险"],
          selected: true,
        },
      ],
    };
  },

  mounted() {
    this.selectRandomPoem();
    this.initChart();
    this.generateEncouragementMessage();
  },

  methods: {
    onBack() {
      this.$router.go(-1);
    },

    selectRandomPoem() {
      const index = Math.floor(Math.random() * this.poems.length);
      this.selectedPoem = this.poems[index];
    },

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

    handleContinue() {
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

    handleCartVisibility(isVisible) {
      this.isCartVisible = isVisible;
    },
    removeCourseFromCart(index) {
      this.cartCourses.splice(index, 1);
    },
    updateUserCourses(newCourses) {
      this.usercourses = newCourses;
    },
    handleCheckout(selectedCourses) {
      // 计算所选课程的总价格
      const totalPrice = selectedCourses.reduce(
        (total, course) => total + course.coursePrice,
        0
      );

      // 检查余额是否足够
      if (this.vitalityCoins >= totalPrice) {
        // 如果足够，扣除金额并进行结算
        this.vitalityCoins -= totalPrice;

        // 将选中的课程添加到 userCourses 数组中
        this.usercourses = [...this.usercourses, ...selectedCourses];

        // 从购物车中移除选中的课程
        selectedCourses.forEach((course) => {
          const index = this.cartCourses.indexOf(course);
          if (index !== -1) {
            this.removeCourseFromCart(index);
          }
        });

        // 弹出成功提示
        ElMessageBox.alert(
          `下单成功！您剩余的活力币余额为：${this.vitalityCoins}`,
          "订单确认",
          {
            confirmButtonText: "确定",
            type: "success",
          }
        );
      } else {
        // 如果余额不足，弹出错误提示
        ElMessage({
          message: "余额不足，无法完成结算。",
          type: "error",
        });
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
</script>

<style scoped>
.aaa {
  position: absolute;
  display: flex;
  flex-direction: column;
  top: 5%;
}

.kkk {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  position: absolute;
  top: 4%;
  right: 2%;
  gap: 10px; /* 设置两个组件之间的间距 */
}

.vitality-card {
  background-color: yellow;
  padding: 4px;
  border-radius: 5px;
  margin-top: -8%;
}

.vitality-container {
  display: flex;
  align-items: center;
  font-size: 16px;
  font-weight: bold;
}

.vitality-container .el-icon {
  margin-right: 5px;
}

.el-page-header {
  width: 110%;
  margin-left: -5%;
}

.poem-entry {
  display: flex;
}

.poem-content {
  flex: 1;
  margin-left: -65%;
}

.poem-text {
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
  margin-bottom: 20px;
}

.my-course-title {
  margin-left: -5%;
  color: #79bbff;
  font-weight: bold;
  font-size: 15px;
}

.course-calender {
  margin-top: 3%;
  display: flex;
  justify-content: space-between;
  align-items: flex-start; /* 保证两个模块从同一高度开始 */
}

.coursee-list,
.course-list {
  flex: 1;
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
  margin-right: 20px;
}

.course-name {
  flex: 1;
  margin-right: 20px;
  font-size: 1rem;
  width: 60px;
}

.course-time {
  flex: 2;
  margin-right: 20px;
}

.status-box {
  flex: 1;
}

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
  margin-top: 30px;
}

.chart-container h2 {
  margin-left: -5%;
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
