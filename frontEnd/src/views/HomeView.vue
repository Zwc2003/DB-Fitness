<template>
  <navigation-bar />
  <div>
    <video ref="videoPlayer" class="background-image" autoplay loop muted>
      <source src="../assets/videos/HomeVideo.mp4" type="video/mp4">
      Your browser does not support the video tag.
    </video>
<!--    <div class="overlay-content">-->
<!--      <img src="../assets/logo.png" alt="Overlay Image" class="overlay-image">-->
<!--      &lt;!&ndash; 你可以在这里添加更多内容 &ndash;&gt;-->
<!--      <h1 class="text">你的健身最终选择</h1>-->
<!--    </div>-->
  </div>

<!--  <div class="member-list">-->
<!--    <el-row>-->
<!--      <el-space :size="20">-->
<!--        <el-col v-for="member in memberList" :key="member.name" :span="4">-->
<!--          <el-card style="width: 360px;height:400px">-->
<!--            <img :src="member.avatar" class="avatar" alt="avatar" />-->
<!--            <div class="info">-->
<!--              <h2 style="font-weight: bold">{{ member.name }}</h2>-->
<!--              <h3 style="margin-top: 10px">{{ member.position }}</h3>-->
<!--            </div>-->
<!--          </el-card>-->
<!--        </el-col>-->
<!--      </el-space>-->
<!--    </el-row>-->
<!--  </div>-->
</template>

<script>


export default {
  components: {  },
  data() {
    return {

    }
  }
};
</script>

<style scoped>
.avatar {
  width: 100%;
  height: auto;
}

.member-list {
  display: flex;
  justify-content: space-around;
  flex-wrap: wrap;
  margin-top: 1100px;
  margin-left: -35%;
}

.text {
  color: #0b2d47;
  font-weight: bold;
  font-size: 100px;
}

.background-image {
  position: absolute;
  top: -20px;
  left: 0;
  width: 100%;
  height: 103%;
  object-fit: cover;
  /* 使图片覆盖整个背景区域 */
  z-index: -1;
  /* 使背景图片位于最底层 */
}

.overlay-content {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  pointer-events: none;
  /* 使得点击事件不会影响到背景 */
}

.overlay-image {
  width: 500px;
  height: auto;
  /* 你可以添加更多样式来调整图片位置和大小 */
}
</style>
