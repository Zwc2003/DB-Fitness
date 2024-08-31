<template>
  <div v-if="isVisible" class="course-modal">
    <div class="modal-background" :style="containerStyle">
      <div :style="backgroundStyle"></div>
      <div :style="coursePhotoStyle"></div>

      <div class="modal-content">
        <button class="close-button" @click="closeModal">X</button>
        <div class="course-info">
          <h1 class="course-title">{{ courseTitle }}</h1>
          <p class="course-details">
            课程有效时间：{{ startTime }} - {{ endTime }}
          </p>
          <p class="course-details">每周上课时间：{{ classTime }}</p>
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
            <h2 class="instructor-name">{{ instructorName }}</h2>
            <p class="instructor-honors">{{ instructorHonors }}</p>
          </div>
          <p class="course-description">
            <b>课程须知</b>：{{ courseDescription }}
          </p>
          <!-- 播放按钮 -->
          <button class="play-button" @click="showVideoModal = true">
            播放视频
          </button>

          <!-- 视频模态框 -->
          <div v-if="showVideoModal" class="video-modal">
            <video controls width="100%" height="100%">
              <source :src="videoUrl" type="video/mp4" />
              您的浏览器不支持视频播放。
            </video>
            <button class="close-button" @click="showVideoModal = false">
              关闭
            </button>
          </div>
        </div>

        <div class="yuyue">
          <el-icon style="font-size: 25px"><ShoppingTrolley /></el-icon>
          <button class="book-button" @click="addToCart">加入购物车</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref } from "vue";

export default {
  props: {
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
    classTime: {
      type: String,
      default: "每周三",
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
    }, // 教练头像图片路径
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
  },

  setup(props) {
    // 控制视频模态框显示的变量
    const showVideoModal = ref(false);

    // 视频URL，替换为你的视频文件路径
    const videoUrl = "path-to-your-video.mp4";

    const addToCart = () => {
      // 添加到购物车的逻辑
    };

    return {
      showVideoModal,
      videoUrl,
      addToCart,
    };
  },

  emits: ["close"],

  methods: {
    closeModal() {
      this.$emit("close");
    },

    playVideo() {
      // 这里可以添加点击播放按钮时的逻辑
      console.log("播放视频");
    },
  },

  computed: {
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
        width: "50%", // 左半边覆盖
        height: "100%",
        backgroundImage: `url(${this.coursePhotoUrl})`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      };
    },
  },
};
</script>

<style scoped>
.video-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.8);
  display: flex;
  justify-content: center;
  align-items: center;
}

video {
  max-width: 90%;
  max-height: 90%;
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
  background-color: #c45656; /* 文字背景色 */
  color: white; /* 文字颜色 */
  padding: 10px 20px; /* 文字内边距 */
  clip-path: polygon(10% 0, 100% 0, 90% 100%, 0% 100%); /* 创建平行四边形 */
  display: inline-block; /* 使平行四边形能够包含在.feature-text span中 */
}

.feature-item span {
  font-size: 1em; /* 特性字体大小 */
  font-weight: bold; /* 特性字体加粗 */
  margin-right: 5px; /* 为特性文本添加右边距 */
}

.instructor-info {
  display: flex;
  align-items: center;
  margin-top: 20px;
}

.instructor-image {
  width: 60px;
  height: 60px;
  border-radius: 50%;
  margin-right: 10px;
}

.instructor-name {
  font-size: 1.4em;
}

.instructor-honors {
  margin-left: 10px;
  font-size: 1.1em;
}

.course-description {
  font-size: 1em;
  margin-top: 20px;
}

.play-button {
  padding: 10px 20px;
  background-color: white;
  color: red;
  border: none;
  cursor: pointer;
  font-size: 1.2em;
  margin-top: 20px;
}

.yuyue {
  font-size: 1.2rem;
  margin-left: 300px;
  margin-bottom: 100px;
}
</style>
