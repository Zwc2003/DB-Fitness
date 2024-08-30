<template>
  <div>
    <!-- 勋章展示 -->
    <div class="video-container">
      <video class="responsive-video" autoplay loop muted>
        <source src="/src/assets/videos/medal.mp4" type="video/mp4" />
        您的浏览器不支持视频标签。
      </video>
    </div>

    <!-- 已获得的勋章展示 -->
    <div class="achieved-badges">
      <h2>我的勋章墙</h2>
      <div class="badge-container">
        <!-- 循环展示已获得的勋章 -->
        <div
        v-for="badge in achievedBadges"
        :key="badge.achievementId"
        @click="navigateToRankingList(badge.achievementId)"
        >
          <img :src="badge" alt="Achieved Badge" class="badge-icon achieved" />
        </div>
      </div>
    </div>

    <!-- 未获得的勋章展示 -->
    <div class="unachieved-badges">
      <h2>未获得的勋章</h2>
      <div class="badge-container">
        <!-- 循环展示未获得的勋章 -->
        <div
          class="badge"
          v-for="(badge, index) in unachievedBadges"
          :key="index"
          @click="navigateToRankingList"
        >
          <img
            :src="badge"
            alt="Unachieved Badge"
            class="badge-icon unachieved"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { useRouter } from 'vue-router';
import thumb from '../assets/badges/Achievement_2.png'; // 点赞
import comment from '../assets/badges/Achievement_5.png'; // 评论
import post from '../assets/badges/Achievement_6.png'; // 发帖数
import information from '../assets/badges/Achievement_1.png'; // 个人信息完成
import login from '../assets/badges/Achievement_2.png'; // 累计登录天数
import course from '../assets/badges/Achievement_3.png'; // 完成课程数量
import train from '../assets/badges/Achievement_8.png'; // 训练计划完成天数
import food from '../assets/badges/Achievement_7.png'; // 饮食计划完成天数

export default {
  name: 'BadgeWall',
  data() {
    return {
      achievedBadges: [], // 已获得的勋章
      unachievedBadges: [], // 未获得的勋章
      badgeImages: {
        '累计登录天数': login,
        '完成课程数量': course,
        '训练计划完成天数': train,
        '饮食计划完成天数': food,
        '被点赞': thumb,
        '被评论': comment,
        '发帖数': post,
        '个人信息完成': information,
      }
    };
  },
  mounted() {
    this.fetchAchievements();
  },
  methods: {

    async fetchAchievements() {
      try {
        const response = await axios.get('http://localhost:8080/api/Achievement/GetAchievement', {
          params: { token: localStorage.getItem('token') }
        });
        const achievements = response.data.achievements;

        this.achievedBadges = achievements
          .filter(achievement => achievement.isAchieved === 'true')
          .map(achievement => ({
            ...achievement,
            imgSrc: this.badgeImages[achievement.name]
          }));

        this.unachievedBadges = achievements
          .filter(achievement => achievement.isAchieved === 'false')
          .map(achievement => ({
            ...achievement,
            imgSrc: this.badgeImages[achievement.name]
          }));
      } catch (error) {
        console.error('获取成就数据失败:', error);
      }
    },

    navigateToRankingList() {
      this.$router.push({ name: 'RankingList' , query: { achievementId } });
    }
  }
}
</script>

<style scoped>
.video-container {
  justify-content: center;
  position: absolute; /* 绝对定位 */
  top: 0; /* 视频紧贴网页顶部 */
  left: 0; /* 视频紧贴网页左侧 */
  width: 100vw; /* 视频宽度覆盖整个视口宽度 */
  height: 100vh; /* 视频高度覆盖整个视口高度 */
  z-index: -1; /* 确保视频在其他内容的下方 */
  overflow: hidden; /* 防止视频内容超出容器 */
}
.badge-header {
  text-align: center;
  margin-top: 60px; /* 距离导航栏下方的距离，根据实际情况调整 */
  color: #333; /* 深灰色字体 */
  font-size: 3vw; /* 根据视口宽度自适应 */
  font-weight: bold; /* 设置字体加粗 */
}
.badge-header h2 {
  font-size: 3vw; /* 增大字体 */
  font-weight: bold; /* 字体加粗 */
  text-align: center; /* 居中对齐 */
  margin-top: 60px; /* 距离导航栏的距离 */
  color: #333; /* 深灰色字体 */
}


.responsive-video {
  margin-top: 10vh;
  width: 90vw; /* 视频宽度覆盖整个视口宽度 */
  height: 60vh; /* 视频高度覆盖整个视口高度 */
  object-fit: cover; /* 保持视频的宽高比，确保其覆盖整个容器 */
  border: none; /* 去掉边框 */
}

.achieved-badges,
.unachieved-badges {
  margin: 80px;
  text-align: center; /* 将内部内容居中 */
}

.badge-container {
  display: flex;
  overflow: hidden; /* 隐藏超出容器的内容 */
  position: relative;
  width: 100%; /* 容器宽度全屏 */
  justify-content: center; /* 居中对齐内容 */
}

.badge-scroll {
  display: flex;
  flex-wrap: nowrap; /* 确保滚动项在一行展示 */
  width: fit-content; /* 宽度根据内容动态调整 */
  animation: scroll 15s linear infinite; /* 动画持续时间15秒，线性动画，无限循环 */
}

.badge {
  margin: 0 10px; /* 间距 */
  flex-shrink: 0; /* 防止缩小 */
  display: flex;
  align-items: center;
  justify-content: center;
}

.badge-icon {
  width: 10vw; /* 根据视口宽度自适应 */
  height: 10vw; /* 根据视口宽度自适应 */
  max-width: 100px; /* 设置最大宽度 */
  max-height: 100px; /* 设置最大高度 */
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #ccc;
  color: #333; /* 深灰色字体 */
}

.badge-icon.achieved {
  background-color: gold; /* 已获得的勋章颜色 */
}

.badge-icon.unachieved {
  background-color: gray; /* 未获得的勋章颜色 */
}

h2 {
  text-align: center;
  color: #333; /* 深灰色字体 */
  font-size: 2vw; /* 根据视口宽度自适应 */
}

@keyframes scroll {
  0% {
    transform: translateX(0);
  }
  100% {
    transform: translateX(-100%);
  }
}
</style>

  