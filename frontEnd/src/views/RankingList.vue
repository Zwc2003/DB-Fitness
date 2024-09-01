<template>
  <div>
    <h1 class="ranking-title">排行榜</h1>
    
    <!-- 展示我的名次和进度 -->
    <div class="my-ranking">
      <p>我的排名: {{ myRanking }}</p>
      <p>我的进度: {{ myProgress }}</p>
    </div>

    <el-collapse v-model="activeName" accordion class="ranking-container">
      <el-collapse-item 
        v-for="(item, index) in leaderboard" 
        :key="index" 
        :title="`Rank ${item.rank}`" 
        :name="index.toString()"
        class="ranking-item"
      >
        <!-- 展开后的内容 -->
        <div class="ranking-details">
          <img :src="item.avatar" alt="Avatar" class="avatar" />
          <div class="details">
            <p class="name">{{ item.name }}</p>
            <p class="score">进度: {{ item.score }}</p>
          </div>
        </div>
      </el-collapse-item>
    </el-collapse>

    <el-button class="back-button" @click="navigateToDataView">返回勋章墙</el-button>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      activeName: '0', // 默认展开第一个项
      myRanking: null, // 我的名次
      myProgress: null, // 我的进度
      leaderboard: [], // 排行榜用户
    };
  },
  mounted() {
    this.fetchAchievementRanking();
  },
  methods: {
    async fetchAchievementRanking() {
      const { achievementId } = this.$route.query;
    const userId = localStorage.getItem('userId'); // 从localStorage获取userId
    const token = localStorage.getItem('token'); // 从localStorage获取token
      try {
        const response = await axios.get('/api/Achievement/GetAchievementRanking', {
        params: {
          userId, // 添加 userId 参数
          achievementId, // 添加 achievementId 参数
        },
        headers: {
          Authorization: `Bearer ${token}`, // 使用 Bearer token 进行身份验证
        },
        });

        const data = response.data;
        this.myRanking = data.myRanking;
        this.myProgress = data.progress;
        this.leaderboard = data.rankingUsers.map(user => ({
          name: user.userName,
          score: user.progress,
          avatar: user.userIcon,
          rank: user.userRank,
        }));
      } catch (error) {
        console.error('获取成就排行榜失败:', error);
      }
    },
    navigateToDataView() {
      this.$router.push({ name: 'achievements' }); // 根据路由配置修改
    }
  },
};
</script>

<style>
/* 样式保持不变 */
.ranking-title {
  text-align: center;
  margin-bottom: 20px;
  color: #333;
}

.ranking-container {
  width: 700px;
  max-width: 1000px;
  margin: 0 auto;
  background-image: url('../assets/images/BG.jpg');
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  padding: 40px;
}

.ranking-item {
  font-size: 20px;
}

.ranking-item .el-collapse-item__header {
  background-color: rgba(255, 255, 255, 0.6);
  border-radius: 8px;
  margin-bottom: 10px;
}

.ranking-details {
  display: flex;
  align-items: center;
  padding: 20px;
  background-color: rgba(255, 255, 255, 0.4);
  border-radius: 8px;
  margin-bottom: 10px;
}

.avatar {
  width: 100px;
  height: 100px;
  border-radius: 50%;
  margin-right: 20px;
}

.details {
  font-size: 20px;
}

.name {
  font-weight: bold;
  font-size: 24px;
  margin: 0;
}

.score {
  color: #555;
  font-size: 20px;
  margin: 5px 0 0;
}

.back-button {
  display: block;
  width: 150px;
  margin: 20px auto;
  background-color: #ccc;
  color: #333;
  border: none;
  border-radius: 4px;
  padding: 10px;
  text-align: center;
  cursor: pointer;
}
</style>
