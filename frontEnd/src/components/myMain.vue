<template>
  <div class="divMain">
     <!-- 先循环找到聊天对象 -->
    <div v-for="(list, index) in MessageList" :key="index">
      <!-- 判断是否有聊天记录 -->
      <div v-if="list.targetName === target.name">
        <!-- 循环显示聊天记录 -->
        <div v-for="(message, index) in list.list" :key="index" :class="{'message-item': true, 'right-aligned': message.is_me}">
          <el-avatar v-if="!message.is_me" :src="user.img" class="avatar" @click="showUserInfo(user)"></el-avatar>

          <!-- 弹出框 -->
          <el-dialog
            v-model="dialogVisible"
            width="40%"
            :modal="false"
            @close="handleClose"
          >
            <div class="user-info">
              <!-- 用户头像 -->
              <el-avatar :src="selectedUser.img" class="user-info-avatar" size="large"></el-avatar>
              
              <!-- 用户详细信息 -->
              <el-row :gutter="20" class="user-info-details">
                <el-col :span="12"><p>昵称: {{ selectedUser.name }}</p></el-col>
                <el-col :span="12"><p>年龄: {{ selectedUser.age }}</p></el-col>
                <el-col :span="12"><p>性别: {{ selectedUser.gender }}</p></el-col>
                <el-col :span="12"><p>标签: {{ selectedUser.tags }}</p></el-col>
                <el-col :span="24"><p>简介: {{ selectedUser.introduction || '暂无简介' }}</p></el-col>
                <el-col :span="12"><p>目标类型: {{ selectedUser.goalType || '未设置' }}</p></el-col>
                <el-col :span="12"><p>目标体重: {{ selectedUser.goalWeight || '未设置' }}</p></el-col>
              </el-row>
            </div>
            
            <span slot="footer" class="dialog-footer">
              <el-button @click="dialogVisible = false">关闭</el-button>
            </span>
          </el-dialog>

          <!-- 显示消息内容 -->
          <!-- 如果是 5 分钟的倍数，或者与上一条消息的时间差超过 5 分钟，显示时间 -->
          <div v-if="shouldShowTime(index,list.list)&&message.is_me" class="message-time">{{ message.time }}</div>

           <!-- 判断并渲染 -->
          <template v-if="isImage(message.messageType)">
            <el-image
              :src="message.message"
              :preview-src-list="[message.message]" 
              fit="cover"
              style="max-width: 200px; margin-left: 10px;"
            />
          </template>
          <template v-else>
            <span class="content">{{ message.message }}</span>
          </template>
        
          <div v-if="shouldShowTime(index,list.list)&&!message.is_me" class="message-time">{{ message.time }}</div>
          <el-avatar v-if="message.is_me" :src="target.img" class="avatar right-avatar"></el-avatar>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import store from "../store/index.js";

export default {
name: 'myMain',
data() {
  return {
    dialogVisible: false,
    selectedUser: {
      img: '',
      name: '',
      age: '',
      gender: '',
      tags: '',
      introduction: '',
      goalType: '',
      goalWeight: ''
    }
  };
},
computed: {
    users(){
      return store.state.userListInformation;//用户列表的信息
    },
    target(){
      return store.state.targetInfomation;
    },
    MessageList(){
      return store.state.MessageList;
    },
    user(){
      return store.state.targetInfomation;
    },
  },
methods: {
  // 显示用户信息弹出框
  showUserInfo(target) {
    const userInformation = this.users.find(user => user.id === target.id);
    this.selectedUser = userInformation;
    this.dialogVisible = true;
    console.log(this.dialogVisible)
  },
  // 处理弹出框关闭事件
  handleClose() {
    this.dialogVisible = false;
  },
  shouldShowTime(index,messageList) {
    if (index === 0) return true; // 总是显示第一条消息的时间
    const currentMessage = messageList[index];
    const previousMessage = messageList[index - 1];
    const currentTime = new Date(currentMessage.time);
    const previousTime = new Date(previousMessage.time);
    const timeDifference = (currentTime - previousTime) / 1000 / 60; // 时间差，单位为分钟
    return timeDifference >= 5; // 如果时间差大于等于 5 分钟，显示时间
  },
  isImage(message) {
    console.log(message);
    return message === "image";
  }
}
}
</script>

<style scoped>
.message-time {
color: gray;
font-size: 0.8em;
margin-bottom: 10px;
}

.message-item {
display: flex;
align-items: center;
margin-bottom: 10px;
}

.right-aligned {
justify-content: flex-end;
}

.avatar {
width: 40px;
height: 40px;
}

.right-avatar {
margin-left: 10px;
}

.content {
background-color: greenyellow;
padding: 10px;
border-radius: 10px;
font-weight: bold;
margin-left: 10px;
margin-right: 10px;
max-width: 60%;
line-height: 1.2; /* 调整行高 */
}

.user-info {
display: flex;
flex-direction: column;
align-items: center;
}

.user-info-avatar {
margin-bottom: 20px;
}

.user-info-details {
width: 100%;
margin-top: 20px;
}

.user-info-details p {
margin: 5px 0;
}

</style>
