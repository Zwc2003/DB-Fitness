<template>
  <el-button  @click="loadWeeksData" class="button" type="primary">
    生成健身计划
  </el-button>

  <el-collapse class="list" v-model="activeName">
    <el-collapse-item
      v-for="(week, index) in weeks.plan"
      :key="index"
      :title="titles[index]"
      :name="index"
      class="custom-collapse-item"
    >
      <el-timeline class="line">
        <el-timeline-item
          v-for="(item, index2) in week"
          :key="index2"
          :timestamp="item.timestamp"
          class="custom-timeline-item"
          placement="top"
        >
          <el-container>
            <el-popover placement="right" :width="500" trigger="click">
              <template #reference>
                <el-card class="card" shadow="hover">
                  <img :src="item.coverUrl" alt="Event Image" class="event-image" />
                  <template #footer>{{ item.workoutName }}</template>
                </el-card>
              </template>
              <div style="padding-left: 0">
                <el-table :data="item.exercises" :row-style="{height:50+'px'}" stripe class="table">

                  <el-table-column prop="exerciseName" label="动作" width="150" />
                  <el-table-column prop="count" label="组数" />
                  <el-table-column prop="time" label="时间" />
                  <el-table-column label="演示">
                    <template v-slot="{ row }">
                      <div>
                        <img ref="imageRef" @click="open(row)" :src="row.coverUrl" alt="Click to open the Message Box" class="gif" />
                      </div>
                    </template>
                  </el-table-column>
                </el-table>
              </div>
              <el-button @click="finish(index*7+index2)" class="over" type="primary" :disabled="buttonDisabled[index*7+index2]"> {{ buttonText[index*7+index2] }}</el-button>
            </el-popover>
          </el-container>
        </el-timeline-item>
      </el-timeline>
    </el-collapse-item>
  </el-collapse>


  <el-calendar
      :range="[startDate, endDate]"
      :date-cell="customDateCell"
  ></el-calendar>
</template>

<style scoped>
.button{
  position: absolute;
  left: 32%;
  top: 12px;
  width: 100px;
  height: 40px;
}
.list{
  position:absolute;
  left: 20%;
  top: 60px;
  width: 60%;
}
.line{
  position: relative;
  left: 10px;
  top: 10px;
}
.over{
  position: relative;
  margin-top: 10px;
  left: 38%;
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
>>>.el-calendar__header {
  display: none;
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
  content: url(../assets/images/strength.png); /* 鼠标悬停时显示的图片路径 */
}

.dialog-video{
  height:65%;
}
.el-container {
  width: 300px; /* 与卡片宽度一致 */
  height: 200px; /* 与卡片高度一致 */
}
</style>

<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import {ElMessageBox, ElNotification} from 'element-plus';
import axios from 'axios';
import dayjs from 'dayjs';

// 设置特定的开始日期，比如 2024年9月1日
const startDate = ref(dayjs('2024-09-01').toDate());
// 设置结束日期为28天后的日期
const endDate = ref(dayjs(startDate.value).add(27, 'day').toDate());
// 定义需要背景变蓝色的日期数组
const blueDays = ['2024-09-03', '2024-09-10', '2024-09-15'];
const customDateCell = ({ date }) => {
  const dateString = dayjs(date).format('YYYY-MM-DD'); // 将日期格式化为字符串
  const isBlueDay = blueDays.includes(dateString); // 判断是否是蓝色背景的日期

  return h(
      'div',
      {
        style: {
          backgroundColor: isBlueDay ? 'blue' : '', // 设置蓝色背景
          color: isBlueDay ? 'white' : 'black', // 确保蓝色背景下文字可读
          padding: '10px',
          borderRadius: '5px',
        },
      },
      date.getDate() // 显示日期
  );
};

const weeks = ref([]);
const titles = ["第一周", "第二周", "第三周", "第四周"];
const loading = ref(true);
const activeName = ref([0]);
const isDisable=ref(true);
const buttonDisabled=ref([false, false, false, false, false, false, false,
  false, false, false, false, false, false, false,
  false, false, false, false, false, false, false,
  false, false, false, false, false, false, false,]);
const buttonText=ref(['完成计划','完成计划','完成计划','完成计划','完成计划','完成计划','完成计划',
  '完成计划','完成计划','完成计划','完成计划','完成计划','完成计划','完成计划',
  '完成计划','完成计划','完成计划','完成计划','完成计划','完成计划','完成计划',
  '完成计划','完成计划','完成计划','完成计划','完成计划','完成计划','完成计划',])
function finish(index){
  buttonDisabled.value[index]=true;
  buttonText.value[index]='今日计划已完成';
  axios.get('http://localhost:8080/api/Achievement/UpdateFitnessPlanAchievement', {
    params: {
      token: localStorage.getItem('token'),
      workoutIndex:index
    }
  })
}
function loadWeeksData() {
  loading.value = true;
  axios.get('http://localhost:8080/api/FitnessPlan/GetPlan', {
    params: {
      token: localStorage.getItem('token')
    }
  }).then(response => {
    weeks.value = response.data; // 假设数据结构中 weeks 在顶层
    if (response.data.message !== "fail") {
      console.log(weeks.value.plan[1][1].isCompleted);
      ElNotification({
      message: "健身计划成功生成！",
      type: 'success',
      duration: 2000
    });
      for(let i=0;i<4;i++){
        for(let j=0;j<7;j++){
          if(weeks.value.plan[i][j].isCompleted=="true"){
            buttonDisabled[i*7+j]=true;
            buttonText[i*7+j]='今日计划已完成';
          }
          else{
            buttonDisabled[i*7+j]=false;
            buttonText[i*7+j]='完成计划';
          }
        }
      }
    }
    else{
      ElNotification({
        message: "请先填写体测表！",
        type: 'error',
        duration: 2000
      });
    }
    //localStorage.setItem('savedFitnessPlan', JSON.stringify(weeks.value)); // 保存数据到本地存储
    loading.value = false;

  }).catch(error => {
    console.error('Failed to fetch data:', error);
    loading.value = false;
  });
}

function loadSavedPlan() {
    loadWeeksData();
}

const open = (row) => {
  ElMessageBox.alert(
    '<div class="video-div"><video controls autoplay width="90%" src="' + row.gifUrl + '" ></video></div>',
    row.explanation,
    {
      dangerouslyUseHTMLString: true,
      confirmButtonText: 'OK',
      customStyle: {
        'max-width': '45%',
        height: '90%'
      }
    }
  );
}

onMounted(() => {
  loadSavedPlan(); // 组件挂载时加载保存的数据或生成新的计划
});
</script>
