<template>


  <el-button plain @click="loadWeeksData" class="button">
    生成健身计划
  </el-button>

  <el-collapse class='list' v-model="activeName">
    <el-collapse-item
        v-for="(week,index) in weeks.plan"
        :key="index"
        :title="titles[index]"
        class="custom-collapse-item"
    >
      <el-timeline class="line" >
        <el-timeline-item
            v-for="(item, index) in week"
            :key="index"
            :timestamp="item.timestamp"
            class="custom-timeline-item"
            placement="top"
        >
          <el-container>
            <el-popover placement="right" :width="500" trigger="click">
              <template #reference>
                <el-card class="card" shadow="hover">
                  <img :src="item.coverUrl" alt="Event Image"  class="event-image"/>
                  <template #footer>{{ item.workoutName }}</template>
                </el-card>
              </template>
              <div style="padding-left: 20px">
                <el-table :data="item.exercises" :row-style="{height:50+'px'}" stripe  class="table">
                  <el-table-column type="selection" width="55" />
                  <el-table-column prop="exerciseName" label="动作" width="150"/>
                  <el-table-column prop="count" label="组数" />
                  <el-table-column prop="time" label="时间"  />
                  <el-table-column label="演示" >
                    <template v-slot="{ row }">
                      <div>
                        <img ref="imageRef" @click="open(row)" :src="row.coverUrl" alt="Click to open the Message Box" class="gif"/>
                      </div>
                    </template>
                  </el-table-column>
                </el-table>
              </div>
            </el-popover>
          </el-container>
        </el-timeline-item>
      </el-timeline>
    </el-collapse-item>
  </el-collapse>


</template>

<style scoped>
.button{
  position: absolute;
  left: 480px;
  top: 100px;
  width: 100px;
}
.list{
  position:absolute;
  left: 320px;
  top: 150px;
  width: 800px;
}
.line{
  position: relative;
  left: 10px;
  top: 10px;
}
/* 自定义 el-timeline-item 的时间戳样式 */
>>>.custom-timeline-item .el-timeline-item__timestamp {

  font-size: 16px;
  font-weight: bold;
  color: black; /* 设置时间戳的颜色 */
  padding: 5px 10px;
  border-radius: 5px;
  text-align: left;
}
>>>.custom-timeline-item .el-timeline-item__tail{
  border-color: red;
}
/* 自定义 el-timeline-item 的节点样式 */
>>>.custom-timeline-item .el-timeline-item__node {
  background-color: rgb(72, 147, 255); /* 设置节点的边框颜色 */

}
>>>.el-card:hover{
  box-shadow:5px 10px 16px #0ff !important;
}
>>>.el-card:active{
  box-shadow:5px 10px 16px #ff5858 !important;
}
.card {
  max-width: 300px;
  height: 200px; /* 设定卡片的高度 */
}

.event-image {
  width: 100%; /* 图片宽度占满容器 */
  height: auto;
}
>>>.el-card__body{
  height: 166px;
}
>>>.el-card .el-card__footer{
  padding-top:0;
  color:white;
  font-size: 1.2em; /* 设置字体大小 */
  font-weight: bold; /* 设置字体加粗 */
  background-color: darkblue;
}

.table{
  width: 1500px;
  height:325px;
}
.gif {
  width: 50px; /* 设置图片的宽度 */
  height: 50px; /* 设置图片的高度 */
  object-fit: cover; /* 保持图片的宽高比，裁剪超出部分 */
}
.gif:hover {
  content: url(../assets/play.jpeg); /* 鼠标悬停时显示的图片路径 */
}
.dialog-video{
  width:600px;
  height:600px;
}
</style>

<script lang="ts" setup>
import { ref } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import type { Action } from 'element-plus' ;
import axios from 'axios';
const weeks = ref([]);
const titles=["第一周","第二周", "第三周", "第四周"];
const loading = ref(true);
function loadWeeksData() {
  loading.value = true;
  axios.get('http://localhost:8080/api/FitnessPlan/GetPlan',
      {
          params: {
            token: localStorage.getItem('token')
          }
      }
  ).then(response => {
    weeks.value = response.data; // 假设数据结构中 weeks 在顶层
    loading.value = false;
    console.log(response.data);
  }).catch(error => {
    console.error('Failed to fetch data:', error);
    loading.value = false;
  });
}


const open = (row) => {
  ElMessageBox.alert(

      '<div class="video-div"><video controls autoplay src="' + row.gifUrl + '" class="dialog-video"></video></div>',
      row.explanation,
      {
        dangerouslyUseHTMLString: true,
        confirmButtonText: 'OK',
        customStyle: {
          'max-width': '55%',
          height: '100%'
        },
        callback: (action: Action) => {
          
        },
      }
  )
}
</script>