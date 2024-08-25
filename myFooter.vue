<template>
    <div>
      <el-input v-model="input" placeholder="请输入内容" @keyup.enter="sendMessage">
        <template #suffix>
          <div class="input-suffix">
            <emoji-picker @select-emoji="addEmoji" />
            <image-upload />
          </div>
        </template>
        <template #append>
          <el-button @click="sendMessage">发送</el-button> 
        </template>
      </el-input>
    </div>
  </template>
  
  
  <script>
  import store from "../store/index.js";
  import EmojiPicker from './EmojiPicker.vue';
  import ImageUpload from './ImageUpload.vue';
  
  export default {
    name: 'myFooter',
    components: {
      EmojiPicker,
      ImageUpload
    },
    data() {
      return {
        input: ''
      };
    },
  
    methods: {
      async sendMessageTo() {
        try {
          // 从 store.state 中提取 receiverID 和 senderID
          const receiverID = store.state.targetInfomation.id;
          const senderID = localStorage.getItem('userID');
          const token = localStorage.getItem('token'); 
          // 获取当前时间
          const currentTime = new Date().toLocaleString();
  
          // 构建请求体
          const messageData = {
            messageID:-1,
            senderID: senderID,
            receiverID: receiverID,
            messageType: '', // 这里为空字符串
            content: this.input, // 获取用户输入的消息内容
            sendTime:currentTime,
          };
  
          // 发送 POST 请求到后端 API
          const response = await axios.post(`http://localhost:8080/api/Message/SendMessage`, messageData);
          console.log('Message sent successfully:', response.data);
  
        } catch (error) {
          console.error('Error sending message:', error);
        }
      },
  
      sendMessage() {
        //控制台打印用户输入信息
        console.log('用户输入的信息为：', this.input);
  
        // 获取当前时间
        const currentTime = new Date().toLocaleString();
  
        if(this.input){
          var msg={
            targetName: store.state.targetInfomation.name,
            targetId:store.state.targetInfomation.id,
            list:{
              is_me: true,//用来判断是聊天对象发送的消息还是我发送的消息
              time: currentTime,//发送信息的时间
              message: this.input,
              messageType: '', // 这里为空字符串
            }
          }
        store.commit('addMessage', msg);
        //调用后端API发送信息，同时需要调用API来监听并接收信息
        this.sendMessageTo();
        }
        //需要将输入框进行清空
        this.input='';
        console.log('清空输入框,输入框内容为：',this.input);
      },
      addEmoji(emoji) {
        this.input += emoji;
      }
    }
  }
  </script>
  
  <style>
  .input-suffix {
    display: flex;
    align-items: center; /* 确保子元素在垂直方向上居中对齐 */
    gap: 10px; /* 控制两个组件之间的间距 */
  }
  </style>
  