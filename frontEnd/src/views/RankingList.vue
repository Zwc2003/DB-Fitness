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

      try {
        const response = await axios.get('/api/Achievement/GetAchievementRanking', {
          params: {
            userId: this.userId,
            achievementId,
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
      this.$router.push({ name: 'dataVis' }); // 根据路由配置修改
    }
  },
};
</script>

<style>
.ranking-title {
text-align: center;
margin-bottom: 20px;
color: #333; /* 深灰色 */
}

.ranking-container {
width: 700px; /* 使容器宽度自适应网页大小 */
max-width: 1000px; /* 设置最大宽度 */
margin: 0 auto; /* 居中对齐 */

background-image: url('@/assets/BG.jpg'); /* 设置背景图片路径 */
background-size: cover; /* 背景图片自适应容器大小 */
background-position: center; /* 背景图片居中显示 */
background-repeat: no-repeat; /* 背景图片不重复 */
padding: 40px; /* 内边距确保内容不贴边 */
}

.ranking-item {
font-size: 20px; /* 增加标题字体大小 */
}

.ranking-item .el-collapse-item__header {
background-color: rgba(255, 255, 255, 0.6); /* 收起状态的背景半透明 */
border-radius: 8px; /* 圆角 */
margin-bottom: 10px; /* 项目间距 */
}
.ranking-details {
display: flex;
align-items: center;
padding: 20px; /* 增加内边距 */
background-color: rgba(255, 255, 255, 0.4); /* 展开状态的背景半透明 */
border-radius: 8px; /* 圆角 */
margin-bottom: 10px; /* 项目间距 */
}
.ranking-title-content {
padding: 10px; /* 内容内边距 */
font-weight: bold; /* 字体加粗 */
}

.ranking-details {
display: flex;
align-items: center;
padding: 20px; /* 增加内边距 */
background-color: rgba(255, 255, 255, 0.4); /* 展开状态的背景半透明 */
margin-bottom: 0; /* 去掉底部边距 */
border-radius: 8px; /* 圆角 */
margin-bottom: 10px; /* 项目间距 */
}

.avatar {
width: 100px; /* 增加头像大小 */
height: 100px; /* 增加头像大小 */
border-radius: 50%;
margin-right: 20px; /* 增加头像与文本的间距 */
}

.details {
font-size: 20px; /* 增加文本字体大小 */
}

.name, .score {
color: #333; /* 深灰色 */
}

.name {
font-weight: bold;
font-size: 24px; /* 增加名字字体大小 */
margin: 0; /* 去掉默认边距 */
}

.score {
color: #555; /* 更深的灰色 */
font-size: 20px; /* 增加分数字体大小 */
margin: 5px 0 0; /* 增加分数的上边距 */
}

.back-button {
display: block;
width: 150px; /* 按钮宽度 */
margin: 20px auto; /* 按钮居中 */
background-color: #ccc; /* 灰色背景 */
color: #333; /* 深灰色字体 */
border: none; /* 去掉边框 */
border-radius: 4px; /* 圆角 */
padding: 10px; /* 内边距 */
text-align: center; /* 文本居中 */
cursor: pointer; /* 鼠标指针样式 */
}
</style>
