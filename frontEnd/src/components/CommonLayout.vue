<template>
    <div class="common-layout">
      <el-container>
        <el-header class="custom-header">
          {{target.name}}
        </el-header>
        <el-container>
          <el-aside width="200px"><MyAside/></el-aside>
          <el-container>
            <el-main><MyMain/></el-main>
            <el-footer><MyFooter/></el-footer>
          </el-container>
        </el-container>
      </el-container>
    </div>
  </template>
  
  <script>
  import MyAside from './myAside.vue';
  import MyFooter from './myFooter.vue';
  import MyMain from './myMain.vue';
  import store from "../store/index.js"; 
  import axios from 'axios';
//   import * as signalR from '@microsoft/signalr';
  
    export default {
      name: 'CommonLayout',
    //   data(){
    //     connection: null
    //   },
      computed: {
        target(){
          console.log("计算属性target被调用");
          return store.state.targetInfomation;
        }
      },
      components: {
        MyAside,MyMain,MyFooter
      },
      created() {
        //this.fetchUserProfile();
        //this.fetchFriendList();
        this.getFriendInformation();
        //this.getChatHistory()
      },
    //   async mounted() {
    //   // 创建SignalR连接，并强制使用WebSocket传输
    //   this.connection = new signalR.HubConnectionBuilder()
    //       .withAutomaticReconnect() // 自动重连
    //       .configureLogging(signalR.LogLevel.Information) // 启用日志记录，帮助调试
    //       .withHubProtocol(new signalR.JsonHubProtocol()) // 使用JSON作为协议
    //       .withUrl('https://localhost:5173/chatHub', {
    //           transport: signalR.HttpTransportType.WebSockets // 强制使用WebSocket传输
    //       })
    //       .build();
  
    //   const message ={
    //    messageID:null,
    //    senderID:null,
    //    receiverID:null,
    //    messageType:'',
    //    content:'',
    //    sendTime: null
    //   }
          
    //   // 监听来自服务器的消息事件
    //   this.connection.on('ReceiveMessage', (message) => {
    //       const target = store.state.userListInformation.find(user => user.id === message.senderID);
    //       var msg = {
    //           targetName: target.name,
    //           targetId: message.senderID,
    //           list: {
    //               is_me: false, // 判断是聊天对象发送的消息还是我发送的消息
    //               time: message.sendTime, // 发送信息的时间
    //               message: message.content,
    //               messageType: message.messageType
    //           }
    //       };
    //       store.commit('addMessage', msg);
    //   });
  
    //   // 启动SignalR连接
    //   try {
    //       await this.connection.start();
    //       console.log('SignalR connected with WebSocket');
    //   } catch (err) {
    //       console.error('Error while starting SignalR connection:', err);
    //   }
    // },
    // beforeDestroy() {
    //     // 确保在组件销毁时断开连接
    //     if (this.connection) {
    //         this.connection.stop();
    //     }
    // },
  
      methods: {
  
        // async fetchUserProfile() {
        //   try {
        //     const token = store.state.token; // 假设 token 存储在 Vuex 的 store 中,在测试的时候直接拿一个字符串
  
        //     const response = await axios.get('http://localhost:8080/api/User/GetPersonalProfile', {
        //       params: { 
        //         token: token // 将 Token 作为查询参数
        //       }
        //     });
        //     console.log('User Profile:', response.data);
        //     // 在这里处理响应数据
        //     // 提取 id, name, iconURL (img)
        //     const info = {
        //       id: response.data.userID,
        //       name: response.data.userName,
        //       img: response.data.iconURL ,
        //     };
  
        //     // 提交到 Vuex store
        //     store.commit('setMyInformation', info);
  
        //   } catch (error) {
        //     console.error('Error fetching user profile:', error);
        //   }
        // },
  
        async  getFriendInformation() {
          try {
            const token = localStorage.getItem('token');
            const response = await axios.get(`http://localhost:8080/api/User/GetFriendList?token=${token}`);
            const userList = response.data;  // 获取 userList
            
            const userListInformation = []; // 用于存储用户详细信息
            for (const user of userList) {
              const response = await axios.get('http://localhost:8080/api/User/GetProfileByUserID', {
                params: {
                  token: token, // 将 Token 作为查询参数 
                  userID: user //用户ID   
                } 
              });
              const userInfo = response.data;
              
              // 构建新的用户信息对象
              const userInformation = {
                id: userInfo.userID,
                name: userInfo.userName,
                img: userInfo.iconURL , // 使用返回的 iconURL 或原始 img
                age: userInfo.age,
                gender: userInfo.gender,
                tags: userInfo.tags,
                introduction: userInfo.introduction,
                goalType: userInfo.goalType,
                goalWeight: userInfo.goalWeight
              };
              // 将用户信息推入 userListInformation 数组
              userListInformation.push(userInformation);
            }
  
            // 将新的用户详细信息列表提交到 Vuex store
            store.commit('setUserListInformation', userListInformation);
            store.commit('setUserList', userList);
            console.log('Updated User List Information:', store.state.userListInformation);
            this.getChatHistory();
          } catch (error) {
            console.error('Error fetching user information:', error);
          }
        },
  
        // async fetchFriendList() {
        //   try {
        //     // const userID = store.state.myInformation.id; // userID 存储在 Vuex 的 store 中
        //     const token = store.state.token; // 假设 token 存储在 Vuex 的 store 中,在测试的时候直接拿一个字符串
  
        //     const response = await axios.get('http://localhost:8080/api/User/Searchfriend', {
        //       params: { 
        //         token: token 
        //       }
        //     });
        //     console.log('Friend List:', response.data);
  
        //     // 假设 response.data 是包含用户信息对象的数组
        //     const userList = response.data.map(friend => ({
        //       id: friend.userID,
        //       name: friend.userName,
        //       img: friend.iconURL 
        //     }));
  
        //     // 提交到 Vuex store
        //     store.commit('setUserList', userList);
        //   } catch (error) {
        //     console.error('Error fetching friend list:', error);
        //   }
        // },
  
        async getChatHistory() {
          try {
            //console.log("1",userList);
            const userList =localStorage.getItem('userList');
            //console.log("2",userList);
            const token = localStorage.getItem('token');
            //console.log("3",token);
  
            for (const user of userList) {
              const response = await axios.get(`http://localhost:8080/api/Message/GetChatHistory?userID=${user}`);
  
              // 获取返回的数据
              const data = response.data;            
  
              const userListInformation =localStorage.getItem('userListInformation');
               // 遍历每条消息
              data.forEach(message => {
                // 根据 messageType 判断并处理消息
                if (message.senderID  === user) { //发送方是好友
                  var target_user = userListInformation.find(user => user.id === message.senderID);
                  var msg={
                    targetName: target_user.name ,
                    targetId:target_user.id,
                    list:{
                      is_me: false,//用来判断是聊天对象发送的消息还是我发送的消息
                      time: message.sendTime,//发送信息的时间
                      message: message.content,
                      messageType: message.messageType, 
                    }
                  }
                  store.commit('addMessage', msg);
                }else {
                  var target_user = userListInformation.find(user => user.id === message.receiverID);
                  var msg={
                    targetName: target_user.name,
                    targetId:target_user.id,
                    list:{
                      is_me: true,//用来判断是聊天对象发送的消息还是我发送的消息
                      time: message.sendTime,//发送信息的时间
                      message: message.content,
                      messageType: message.messageType, 
                    }
                  }
                  store.commit('addMessage', msg);
                }
              });
              // 打印处理后的聊天记录
              console.log('Chat History:', data);
            }
          } catch (error) {
            // 处理请求错误
            console.error('Error fetching chat history:', error);
          }
        }
      }   
    }
  </script>
  
  <style>
    .common-layout {
      height: 550px;
      width: 958.4px;
    }
  
    .el-container {
      height: 550px;
    }
  
    .el-header{
    border-bottom: 1px solid #ccc; /* 下边框 */
    }
    .el-footer {
    /* background-color:white; */
    color: #333;
    text-align: center;
    line-height: 60px;
    border-top: 1px solid #ccc;    /* 上边框 */
    border-right: 1px solid #ccc;  /* 右边框 */
    border-bottom: 1px solid #ccc; /* 下边框 */
    border-left: 1px solid #ccc;  /* 左边框 */
  }
  
  .el-aside {
  
    /* background-color: white; */
    color: #333;
    text-align: center;
    height: 550px;
    border-top: 1px solid #ccc;    /* 上边框 */
    border-right: 1px solid #ccc;  /* 右边框 */
    border-bottom: 1px solid #ccc; /* 下边框 */
    border-left: 1px solid #ccc;  /* 左边框 */
  }
  
  .el-main {
    /* background-color: white; */
    color: #333;
    height: 280px;
    border-top: 1px solid #ccc;    /* 上边框 */
    border-right: 1px solid #ccc;  /* 右边框 */
    border-bottom: 1px solid #ccc; /* 下边框 */
    border-left: 1px solid #ccc;  /* 左边框 */
  }
  .custom-header {
    text-align: center; /* 水平居中 */
    line-height: 60px;  /* 使文本垂直居中，对应你的header高度 */
    font-size: 24px;    /* 可选：调整字体大小 */
  }
  </style>
    