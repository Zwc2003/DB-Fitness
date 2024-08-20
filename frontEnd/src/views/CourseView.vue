<template>
  <NavigationBar />
  <div class="card-container" style="margin-top:15%">
    <!-- 使用 Card 组件显示活动卡片 -->
    <el-card v-for="activity in activities" :key="activity.id" class="activity-card">
      <!-- 活动图片 -->
      <img :src="activity.imageUrl" class="activity-img" alt="activity image">
      <!-- 活动标题 -->
      <div class="activity-title">{{ activity.title }} <span class="tag">{{ activity.tagText }}</span></div>
      <!-- 活动导师 -->
      <div class="activity-teacher">
        <img :src="activity.teacher.imageUrl" class="teacher-img" alt="teacher image">
        <div class="teacher-name">{{ activity.teacher.name }}</div>
      </div>
      <!-- 活动简介 -->
      <p class="activity-description">{{ activity.description }}</p>
      <!-- 活动时间 -->
      <div class="activity-time">时间: {{ activity.time }}</div>
      <!-- 活动地点 -->
      <div class="activity-location">地点: {{ activity.location }}</div>
      <!-- 报名按钮 -->
      <el-button @click="toggleSignUp(activity)" type="primary" class="signup-button">{{ activity.isSignedUp ? '取消报名' :
      '报名' }}</el-button>
    </el-card>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import NavigationBar from "../components/NavigationBar.vue";

// 活动数据
const activities = ref([
  {
    id: 1,
    title: '爆发力量训练',
    description: '专注于加强腹部、背部和臀部肌肉，提高身体爆发力',
    imageUrl: '/images/activity1.jpg',
    time: '2024-06-15 10:00',
    location: '健身房A',
    teacher: {
      name: '朴男',
      imageUrl: '/images/pn.jpg'
    },
    tagText: 'L2初级进阶',
    isSignedUp: false // 初始报名状态为未报名
  },
  {
    id: 2,
    title: '下肢力量训练',
    description: '增强大腿、臀部和小腿肌肉，提高爆发力、耐力和运动表现',
    imageUrl: '/images/activity2.jpg',
    time: '2024-06-16 15:00',
    location: '健身房B',
    teacher: {
      name: '鹿晨辉',
      imageUrl: '/images/l.jpg'
    },
    tagText: 'L1入门级别',
    isSignedUp: false // 初始报名状态为未报名
  },
  {
    id: 3,
    title: '马甲线塑造虐腹训练',
    description: '强化核心，打造腹部肌肉轮廓',
    imageUrl: '/images/activity3.jpg',
    time: '2024-06-20 18:00',
    location: '瑜伽房',
    teacher: {
      name: '帕梅拉',
      imageUrl: '/images/p.jpg'
    },
    tagText: 'L4高级专业',
    isSignedUp: false // 初始报名状态为未报名
  },
  {
    id: 4,
    title: '紧致臀腿线条进阶训练',
    description: '瑜伽站立稳定类体式，塑造臀腿线条',
    imageUrl: '/images/activity4.jpg',
    time: '2024-06-25 14:00',
    location: '瑜伽房',
    teacher: {
      name: '帕梅拉',
      imageUrl: '/images/p.jpg'
    },
    tagText: 'L2初级进阶',
    isSignedUp: false // 初始报名状态为未报名
  }
]);

// 切换报名状态
const toggleSignUp = (activity) => {
  activity.isSignedUp = !activity.isSignedUp;
};
</script>

<style scoped>
.card-container {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  width: 800px;
  /* 父容器宽度为两个卡片的宽度之和 */
}

.activity-card {
  width: 380px;
  /* 每个卡片的宽度 */
  position: relative;
}

.activity-img {
  width: 100%;
  height: 180px;
  object-fit: cover;
}

.activity-title {
  font-size: 20px;
  font-weight: bold;
  margin-bottom: 10px;
}

.activity-teacher {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.teacher-img {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  margin-right: 10px;
}

.teacher-name {
  font-weight: bold;
}

.activity-time,
.activity-location {
  margin-top: 10px;
}

.tag {
  background: green;
  color: white;
  padding: 2px 6px;
  border-radius: 4px;
  margin-left: 5px;
  font-size: 8px;
  /* 设置标签文本字号 */
}

.activity-description {
  font-size: 12px;
  /* 活动描述的字号 */
}

.signup-button {
  position: absolute;
  bottom: 15px;
  /* 距离卡片底部的距离 */
  right: 20px;
  /* 距离卡片右侧的距离 */
}
</style>