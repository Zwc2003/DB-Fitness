import { createStore } from 'vuex'

// 在 store.js 中
export default createStore({
    state: {
        role: localStorage.getItem('role') || 'unAuthenticated',
        username: localStorage.getItem('username') || '',
        token: localStorage.getItem('token') || '',
        recipe: localStorage.getItem('recipe') || '',
        // role: 'unAuthenticated',
        // username: '',
        // token: '',
        targetInfomation:{
            id:'',
            img:'',
            name:'',
        },
        //用户列表
        userList:[], //只返回好友的ID，无其他字段，使用其他字段从userListInformation中获取
        //用户列表的相关信息
        userListInformation:[
          // {
          //   id:0,
          //   name: '同济',
          //   img: userImage,
          //   age:'18',
          //   gender:'男',
          //   tags:'...',
          //   introduction:'',
          //   goalType:'',
          //   goalWeight:'',
          // }, {
          //   id:0,
          //   name: '同济',
          //   img: userImage,
          //   age:'18',
          //   gender:'男',
          //   tags:'...',
          //   introduction:'',
          //   goalType:'',
          //   goalWeight:'',
          // }, {
          //   id:1,
          //   name: '大学',
          //   img: userImage,
          //   age:'18',
          //   gender:'男',
          //   tags:'...',
          //   introduction:'',
          //   goalType:'',
          //   goalWeight:'',
          // }, {
          //   id:2,
          //   name: '学院',
          //   img: userImage,
          //   age:'18',
          //   gender:'男',
          //   tags:'...',
          //   introduction:'',
          //   goalType:'',
          //   goalWeight:'',
          // }
        ],
        //聊天记录
        MessageList: [
            // {
            //   targetName: "同济",
            //   targetId:0,
            //   list: [
            //     {
            //       is_me: false,//用来判断是聊天对象发送的消息还是我发送的消息
            //       time: new Date().toLocaleString(),//发送信息的时间
            //       message: "哈喽",
            //       messageType: '',
            //     },
            //     {
            //       is_me: true,
            //       time: "",
            //       message: "https://wxls-cms.oss-cn-hangzhou.aliyuncs.com/online/2024-04-18/218da022-f4bf-456a-99af-5cb8e157f7b8.jpg",
            //       messageType: 'image', // 指定消息类型为图片
            //     },
            //     {
            //         is_me: false,//用来判断是聊天对象发送的消息还是我发送的消息
            //         time: "",//发送信息的时间
            //         message: "hello",
            //         messageType: '',
            //       },
            //       {
            //         is_me: true,
            //         time: "",
            //         message: "hi",
            //         messageType: '',
            //       },
            //   ],
            // },
          ],
    },
    mutations: {
        setRole(state, role) {
            state.role = role;
            // 将数据保存到 LocalStorage
            localStorage.setItem('role', role);
        },
        setUsername(state, username) {
            state.username = username;
            localStorage.setItem('username', username);
        },
        setToken(state, token) {
            state.token = token;
            localStorage.setItem('token', token);
        },
        setRecipe(state, recipe) {
            state.recipe = recipe;
            localStorage.setItem('recipe', recipe);
        },
        removeRecipe(state, recipe) {
            state.recipe = undefined;
            localStorage.removeItem('recipe');
        },
        setTargetInformation(state, info) {
                state.targetInfomation = info;
                console.log('targetInfomation:',state.targetInfomation);
        },
        setUserList(state, userList) {
            state.userList = userList;
            localStorage.setItem('userList', userList);
            console.log('Updated Friend List:', state.userList);
        },
        setUserListInformation(state, userListInformation) {
          state.userListInformation = userListInformation;
          localStorage.setItem('userListInformation', userListInformation);
          console.log('User List Information Updated:', state.userListInformation);
        },
        addMessage(state, message) {
          const targetName = message.targetName;
          const targetId = message.targetId;
          const target = state.MessageList.find(item => item.targetName === targetName);

          if (target) {
            target.list.push({
              is_me: message.list.is_me,
              time: message.list.time,
              message: message.list.message,
              messageType:message.list.messageType
            });
            console.log(message.list.message);
          } else{
            // 如果没有找到匹配的 targetName，可以选择添加新的对象
            if(targetName){
              state.MessageList.push({
                targetName: targetName,
                list: [
                  {
                    is_me: message.list.is_me,
                    time:  message.list.time,
                    message: message.list.message,
                    messageType:message.list.messageType
                  },
                ],
              });
            }
          }
          localStorage.setItem('MessageList', state.MessageList);
          console.log('Updated Message List:', state.MessageList);
        }
    },
    actions: {
        // 在这里可以添加异步操作
    },
    modules: {
        // 在这里可以添加模块
    },
});
