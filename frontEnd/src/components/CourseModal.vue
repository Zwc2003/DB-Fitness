<template>
  <div v-if="isVisible" class="course-modal">
    <div class="modal-background" :style="containerStyle">
      <div :style="backgroundStyle"></div>
      <div :style="coursePhotoStyle"></div>

      <div class="modal-content">
        <button class="close-button" @click="closeModal">X</button>
        <div class="course-info">
          <h1 class="course-title">{{ courseName }}</h1>
          <p class="course-details">
            课程有效时间：<b class="boldd">{{ courseStartTime }}</b> -
            <b class="boldd">{{ courseEndTime }}</b>
          </p>
          <p class="course-details">
            上课时间：<b class="boldd">{{ classDate }}</b
            ><b class="boldd">{{ classTime }}</b>
          </p>
          <div class="features">
            <div
              class="feature-item"
              v-for="(feature, index) in features"
              :key="index"
            >
              <div class="parallelogram">
                <span>{{ feature }}</span>
              </div>
            </div>
          </div>
          <div class="instructor-info">
            <img
              :src="instructorImage"
              alt="Instructor"
              class="instructor-image"
            />
            <div class="duiqi">
              <div class="instructor-info">
                <h2 class="instructor-name">
                  <b class="boldd">{{ instructorName }}</b>
                </h2>
                <div class="instructor-tag">
                  <span class="tag">职业教练</span>
                  <el-icon class="trophy-icon"><GoldMedal /></el-icon>
                </div>
              </div>
              <p class="instructor-honors">{{ instructorHonors }}</p>
            </div>
          </div>
          <p class="course-description">
            <b class="boldd">课程须知</b>：{{ courseDescription }}
          </p>

          <p class="nandu" :style="iconContainerStyle">
            <b class="boldd">课程难度</b>:
            <el-icon v-if="courseGrade >= 1" style="margin-right: 10px">
              <Flag />
            </el-icon>
            <el-icon v-if="courseGrade >= 2" style="margin-right: 10px">
              <Flag />
            </el-icon>
            <el-icon v-if="courseGrade >= 3" style="margin-right: 10px">
              <Flag />
            </el-icon>
            <el-icon v-if="courseGrade >= 4" style="margin-right: 10px">
              <Flag />
            </el-icon>
            <el-icon v-if="courseGrade >= 5" style="margin-right: 10px">
              <Flag />
            </el-icon>
          </p>
          <p class="nandu">
            <b class="boldd">课程费用</b>：{{ coursePrice
            }}<el-icon class="coinn"><Coin /></el-icon>
          </p>
        </div>
        <div>
          <div v-if="isbooked == 0" class="yuyue">
            <el-icon style="font-size: 35px" class="ic"
              ><ShoppingTrolley
            /></el-icon>
            <button class="book-button" @click="addToCart">加入购物车</button>
          </div>
          <button
            v-else
            :class="buttonClass"
            @click="handleButtonClick"
            :disabled="isDisabled"
          >
            {{ buttonText }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ElMessage } from "element-plus";

export default {
  props: {
    isbooked: {
      type: Boolean,
      default: 1,
    },
    isVisible: {
      type: Boolean,
      default: 1,
    },
    modalBackground: {
      type: String,
      default:
        "https://www.lesmills.com.cn/uploads/20231104/fbdee91e55d525de27a01e2e0a74040b.png",
    },
    coursePhotoUrl: {
      type: String,
      default:
        "https://www.lesmills.com.cn/uploads/20211207/d5d8d0860359243eee20bc507bf2c231.jpg",
    },
    modalWidth: {
      type: String,
      default: "1000px", // 默认宽度
    },
    modalHeight: {
      type: String,
      default: "600px", // 默认高度
    },
    courseName: {
      type: String,
      default: "肌肉力量训练",
    },
    courseStartTime: {
      type: String,
      default: "2022.03.04",
    },
    courseEndTime: {
      type: String,
      default: "2023.03.04",
    },
    classDate: {
      type: String,
      default: "每天",
    },
    classTime: {
      type: String,
      default: "17:00 - 18:30",
    },
    features: {
      type: Array,
      default: () => {
        return ["感受力量涌现", "助力有氧健身", "训练全身各处"];
      },
    },
    instructorImage: {
      type: String,
      default:
        "https://ts1.cn.mm.bing.net/th?id=OIP-C.FHvYewesyi-IlHOiyjLTLAHaLH&w=204&h=306&c=8&rs=1&qlt=90&r=0&o=6&pid=3.1&rm=2",
    },
    instructorName: {
      type: String,
      default: "王教练",
    },
    instructorHonors: {
      type: String,
      default: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
    },
    courseDescription: {
      type: String,
      default:
        "BODYCOMBAT能训练到你的腿部、手臂、背部和肩膀，对核心部位有显著效果。在课程中你能消耗卡路里、提高协调性、敏捷性和速度，感觉自己充满力量。BODYCOMBAT 训练内容进行调整使之符合自身水平和目标。我们的教练将始终提供多重训练强度供你选择。在开始的时候，你可以每周参加1至2节课，很快你就能体会到骁勇精壮的感觉。",
    },
    courseGrade: {
      type: Number,
      default: 4,
    },
    coursePrice: {
      type: Number,
      default: 1,
    },
  },
  data() {
    return {
      currentState: "", // "remember" | "signIn" | "completed" | "missed"
    };
  },
  emits: ["close"],

  methods: {
    //关闭课程模式
    closeModal() {
      this.$emit("close");
    },

    //点击课程按钮
    handleButtonClick() {
      if (this.isDuringClass) {
        this.currentState = "signedIn";
      } else if (this.isAfterClass && this.currentState !== "signedIn") {
        ElMessage({
          message: "请联系教练补课",
          type: "warning",
        });
      }
    },

    addToCart() {
      // 创建课程对象
      const course = {
        coursePhotoUrl: this.coursePhotoUrl,
        courseName: this.courseName,
        courseStartTime: this.courseStartTime,
        courseEndTime: this.courseEndTime,
        classTime: this.classTime,
        features: this.features,
        instructorImage: this.instructorImage,
        instructorName: this.instructorName,
        instructorHonors: this.instructorHonors,
        courseDescription: this.courseDescription,
        courseGrade: this.courseGrade,
        coursePrice: this.coursePrice,
      };
      // 调用 Vuex mutation，将课程添加到购物车中
      this.$store.commit("ADD_COURSE_TO_CART", course);

      // 可选：添加成功后的提示
      this.$message.success("课程已成功加入购物车！");
    },
  },

  computed: {
    //卡片样式
    containerStyle() {
      return {
        position: "relative",
        width: this.modalWidth,
        height: this.modalHeight,
        overflow: "hidden",
      };
    },
    backgroundStyle() {
      return {
        position: "absolute",
        top: 0,
        left: 0,
        width: "100%",
        height: "100%",
        backgroundImage: `url(${this.modalBackground})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      };
    },
    coursePhotoStyle() {
      return {
        position: "absolute",
        top: 0,
        left: 0,
        width: "50%",
        height: "100%",
        backgroundImage: `url(${this.coursePhotoUrl})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      };
    },

    //获取当前时间
    currentTime() {
      return new Date();
    },
    //获取课程的开始时间与结束时间
    timeRange() {
      const [start, end] = this.classTime.split("-").map((time) => {
        const [hours, minutes] = time.split(":");
        const date = new Date();
        date.setHours(parseInt(hours), parseInt(minutes), 0, 0);
        return date;
      });
      return { start, end };
    },
    //判断是否在课程时间之前
    isBeforeClass() {
      return this.currentTime < this.timeRange.start;
    },
    //判断是否在课程进行中
    isDuringClass() {
      return (
        this.currentTime >= this.timeRange.start &&
        this.currentTime <= this.timeRange.end
      );
    },
    //判断是否在课程时间之后
    isAfterClass() {
      return this.currentTime > this.timeRange.end;
    },
    //按钮的字样
    buttonText() {
      if (this.isBeforeClass) {
        return "记得上课";
      } else if (this.isDuringClass) {
        return this.currentState === "signedIn" ? "已上课" : "签到";
      } else if (this.isAfterClass) {
        return this.currentState === "signedIn" ? "已上课" : "补课";
      }
    },
    //按钮的样式
    buttonClass() {
      if (
        this.isBeforeClass ||
        (this.isDuringClass && this.currentState === "signedIn")
      ) {
        return "book-button blue";
      } else if (this.isDuringClass) {
        return "book-button active";
      } else if (this.isAfterClass) {
        return "book-button dark";
      }
    },
    //课程开始前,已经签到,课程结束后,按钮都不能再被点击
    isDisabled() {
      return (
        this.isBeforeClass ||
        this.currentState === "signedIn" ||
        (this.isAfterClass && this.currentState === "signedIn")
      );
    },
  },
};
</script>

<style scoped>
.boldd {
  font-weight: bold;
}

.duiqi {
  text-align: left;
}

.close-button {
  position: absolute;
  top: 20px;
  right: 20px;
  padding: 10px 20px;
  background-color: #f44336;
  color: white;
  border: none;
  cursor: pointer;
}

.course-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}

.modal-background {
  width: 100%;
  height: 100%;
  background-size: cover;
  background-position: center;
  position: relative;
}

.modal-content {
  position: absolute;
  right: 0;
  top: 0;
  width: 50%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  color: white;
  padding: 20px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.course-title {
  font-size: 2.5em;
  font-weight: bold;
  margin-bottom: 10px;
}

.course-details {
  font-size: 1.2em;
}

.features {
  margin-top: 20px;
  display: flex;
  flex-direction: row;
  align-items: center;
}

.feature-item {
  margin-right: 10px; /* 为每个特性项添加右边距 */
}

.parallelogram {
  width: 100px;
  background-color: #c45656;
  color: white;
  padding: 10px 20px;
  clip-path: polygon(10% 0, 100% 0, 90% 100%, 0% 100%);
  display: inline-block;
  margin: 18px;
  margin-bottom: -5px;
  margin-top: -8px;
}

.feature-item span {
  font-size: 1em;
  font-weight: bold;
  margin-right: 5px;
}

.instructor-info {
  display: flex;
  align-items: center;
  margin-top: 12px;
}

.instructor-image {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  margin-right: 10px;
}

.instructor-name {
  margin-left: 2%;
  font-size: 1.4em;
}

.instructor-info {
  display: flex;
  align-items: center;
}

.instructor-name {
  margin-right: 10px; /* 调整名字和方框之间的间距 */
}

.instructor-tag {
  display: flex;
  align-items: center;
}

.tag {
  display: inline-block;
  padding: 4px 8px;
  background-color: #ffdd57;
  border-radius: 4px;
  margin-right: 8px; /* 调整方框和图标之间的间距 */
  font-size: 14px;
  font-weight: bold;
  color: #333;
}

.trophy-icon {
  font-size: 24px;
  color: gold;
  font-weight: bold;
  stroke-width: 2;
}

.nandu {
  margin-top: 20px;
  text-align: left;
  font-size: 1rem;
}

.coinn {
  margin-left: 5px;
  font-weight: bold;
  font-size: 14px;
  color: gold;
}

.instructor-honors {
  margin-left: 10px;
  font-size: 1.1em;
}

.course-description {
  font-size: 1rem;
  margin-top: 20px;
  text-align: left;
}

.yuyue {
  font-size: 1.2rem;
  margin-left: 300px;
  margin-bottom: 100px;
  display: flex;
  align-items: center;
}

.book-button {
  margin-left: 10px;
  font-size: 20px;
  cursor: pointer;
}

.book-button.blue {
  background-color: blue;
  color: white;

  padding: 10px 20px;
  margin-top: -10%;
}

.book-button.active {
  background-color: green;
  color: white;

  padding: 10px 20px;
  margin-top: -10%;
}

.book-button.dark {
  background-color: gray;
  color: white;

  padding: 10px 20px;
  margin-top: -10%;
}

.book-button[disabled] {
  cursor: not-allowed;
}
</style>
