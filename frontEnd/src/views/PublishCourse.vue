<template>
  <div class="aaa">
    <div aria-label="A complete example of page header">
      <el-page-header @back="onBack">
        <template #breadcrumb>
          <el-breadcrumb separator="/">
            <el-breadcrumb-item :to="{ name: 'HomeView' }">
              FitFit
            </el-breadcrumb-item>
            <el-breadcrumb-item :to="{ name: 'course' }">
              健身课程
            </el-breadcrumb-item>
            <el-breadcrumb-item>我的教学</el-breadcrumb-item>
          </el-breadcrumb>
        </template>
        <template #content>
          <div class="flexitems-center">
            <el-avatar
              class="mr-3"
              :size="32"
              src="https://tse2-mm.cn.bing.net/th/id/OIP-C.xWtwoLeZd-P4SoSZOMGyogHaHZ?w=206&h=205&c=7&r=0&o=5&pid=1.7"
            />
            <span>王教练</span>
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
      <div class="my-course-title">我的课堂</div>
      <div class="coursee-list">
        <TeachCard
          v-for="(teachcourse, index) in teachcourses"
          :key="index"
          :coursePhotoUrl="teachcourse.coursePhotoUrl"
          :courseName="teachcourse.courseName"
          :courseDescription="teachcourse.courseDescription"
          :courseStartTime="teachcourse.courseStartTime"
          :courseEndTime="teachcourse.courseEndTime"
          :courseGrade="teachcourse.courseGrade"
          :coursePrice="teachcourse.coursePrice"
          :courseProgress="teachcourse.courseProgress"
          :features="teachcourse.features"
          :instructorImage="teachcourse.instructorImage"
          :instructorName="teachcourse.instructorName"
          :instructorHonors="teachcourse.instructorHonors"
          :classTime="teachcourse.classTime"
          @delete-teachcourse="removeCourse"
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
                v-model="newCourse.courseName"
                placeholder="30到45分钟核心训练"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程图片">
              <el-input
                v-model="newCourse.coursePhotoUrl"
                placeholder="复制图片连接至此处"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程描述">
              <el-input
                v-model="newCourse.courseDescription"
                placeholder="核心肌群是身体的中心力量，对于维持姿势、提高运动表现和预防受伤至关重要。"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程开始时间">
              <el-input
                v-model="newCourse.courseStartTime"
                placeholder="2024-8-8"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程结束时间">
              <el-input
                v-model="newCourse.courseEndTime"
                placeholder="2022-9-8"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程等级">
              <el-input
                v-model="newCourse.courseGrade"
                placeholder="2"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程价格">
              <el-input
                v-model="newCourse.coursePrice"
                placeholder="30"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程进度">
              <el-input
                v-model="newCourse.courseProgress"
                placeholder="0节课/30节课"
              ></el-input>
            </el-form-item>
            <el-form-item label="课程特征">
              <div>
                <el-tag
                  v-for="(feature, index) in newCourse.features"
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
              <el-input
                v-model="newCourse.courseType"
                placeholder="高强度间歇/儿童趣味课/低强度塑形/有氧……"
              ></el-input>
            </el-form-item>
            <el-form-item label="教练图片">
              <el-input
                v-model="newCourse.instructorImage"
                placeholder="复制图片链接至此处"
              ></el-input>
            </el-form-item>
            <el-form-item label="教练名称">
              <el-input
                v-model="newCourse.instructorName"
                placeholder="鹿晨辉"
              ></el-input>
            </el-form-item>
            <el-form-item label="教练荣誉">
              <el-input
                v-model="newCourse.instructorHonors"
                placeholder="国家级健美一级裁判和国家职业健身培训师"
              ></el-input>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="submitForm">提交</el-button>
              <el-button @click="showModal = false">取消</el-button>
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
          v-for="(course, index) in teachcourses"
          :key="index"
          class="course-item"
        >
          <div class="course-item">
            <!-- 圆形 -->
            <div
              :class="['circle', circleColors[index % circleColors.length]]"
            ></div>
            <!-- 课程名称 -->
            <div class="course-name">
              {{ truncateCourseName(course.courseName) }}
            </div>
            <!-- 上课时间 -->
            <div class="course-time">{{ course.classTime }}</div>
            <!-- 状态框 -->
            <div class="status-box">
              <el-tag :type="getStatusType(course)">
                {{ getStatusText(course) }}
              </el-tag>
            </div>
            <!-- 添加的正方形框和图标 -->
            <div class="square" @click="handleClick(course)">
              <el-icon v-if="course.attended">
                <Select />
              </el-icon>
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
import * as echarts from "echarts";
import TeachCard from "../components/TeachCard.vue";
import { mapGetters, mapActions } from "vuex";

export default {
  components: {
    TeachCard,
  },

  data() {
    return {
      showModal: false,
      newCourse: {
        coursePhotoUrl: "",
        courseName: "",
        courseDescription: "",
        courseStartTime: "",
        courseEndTime: "",
        courseGrade: "",
        coursePrice: "",
        courseProgress: "",
        features: [],
        courseType: "",
        instructorImage: "",
        instructorName: "",
        instructorHonors: "",
        classTime: "17:00 - 18:30",
      },
      inputFeature: "",
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
      selectedPoem: null,
      circleColors: ["blue", "yellow", "red", "orange", "green"],
      isCartVisible: false,
    };
  },

  mounted() {
    // 初始化诗句
    this.selectRandomPoem();
    // 初始化图表
    this.initChart();
    //生成鼓励的话
    this.generateEncouragementMessage();

    // 检查并加载全局存储的 teachcourses
    if (!this.getTeachCourses.length) {
      // 初始化 teachcourses 数据
      const initialCourses = [
        // 初始数据...
      ];
      this.updateTeachCourses(initialCourses);
    }
  },

  methods: {
    //每日教学列表的名字长度问题
    truncateCourseName(name) {
      if (name.length > 5) {
        return name.slice(0, 5) + "...";
      } else {
        return name;
      }
    },

    //决定今日课程状态
    handleClick(course) {
      course.attended = course.attended ? 0 : 1;
    },

    //更新我的课程到store
    ...mapActions(["updateTeachCourses"]),

    //删除教练课程
    ...mapActions(["deleteTeachCourse"]),
    removeCourse(courseId) {
      this.deleteTeachCourse(courseId);
    },

    //添加
    ...mapActions(["addTeachCourse"]),
    openModal() {
      console.log("Opening modal");
      this.showModal = true;
    },
    addFeature() {
      const feature = this.inputFeature.trim();
      if (feature && !this.newCourse.features.includes(feature)) {
        this.newCourse.features.push(feature);
      }
      this.inputFeature = ""; // 清空输入框
    },
    removeFeature(index) {
      this.newCourse.features.splice(index, 1);
    },
    submitForm() {
      this.addTeachCourse(this.newCourse);
      this.newCourse = {
        coursePhotoUrl: "",
        courseName: "",
        courseDescription: "",
        courseStartTime: "",
        courseEndTime: "",
        courseGrade: "",
        coursePrice: "",
        courseProgress: "",
        features: [],
        courseType: "",
        instructorImage: "",
        instructorName: "",
        instructorHonors: "",
        classTime: "17:00 - 18:30",
      };
      this.showModal = false;
    },

    //回退<-back
    onBack() {
      this.$router.go(-1);
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

    //获取今日课程状态
    getStatusText(course) {
      const currentTime = new Date();
      const [startHour, startMinute] = course.classTime
        .split(" - ")[0]
        .split(":")
        .map(Number);
      const startTime = new Date();
      startTime.setHours(startHour, startMinute);

      const [endHour, endMinute] = course.classTime
        .split(" - ")[1]
        .split(":")
        .map(Number);
      const endTime = new Date();
      endTime.setHours(endHour, endMinute);

      if (currentTime < startTime) {
        return "提醒我";
      } else if (currentTime > endTime && course.attended) {
        return "已打卡";
      } else if (currentTime > endTime && !course.attended) {
        return "未打卡";
      } else {
        return "进行中";
      }
      return "";
    },

    //获取状态的字体样式
    getStatusType(course) {
      const currentTime = new Date();
      const [startHour, startMinute] = course.classTime
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
    // 从store获取教练课程数组
    teachcourses() {
      return this.getTeachCourses;
    },
    ...mapGetters(["getTeachCourses"]),

    // 用户当前教练阶段
    currentStage() {
      return this.stages
        .slice()
        .reverse()
        .find((stage) => this.totalWorkoutHours >= stage.unlockHours);
    },

    //用户当前阶段称号与描述
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
  left: 8%;
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
  margin-top: 20px;
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
  align-items: flex-start;
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
  margin-right: 20px;
  font-size: 1rem;
  width: 100px;
}

.course-time {
  flex: 2;
  margin-right: 20px;
}

.status-box {
  flex: 1;
}

.square {
  margin-left: 5px;
  font-size: 1.5rem;
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
  line-height: 1.8;
  margin-bottom: 10px;
  color: #337ecc;
  font-weight: bond;
}

.stage-info h3 {
  color: blueviolet;
  line-height: 1.8;
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
