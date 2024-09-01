//import { l } from 'vite/dist/node/types.d-aGj9QkWt';
import { createStore } from "vuex";
import axios from "axios";

// 在 store.js 中
export default createStore({
  state: {
    role: localStorage.getItem("role") || "unAuthenticated",
    username: localStorage.getItem("username") || "",
    token: localStorage.getItem("token") || "",
    recipe: localStorage.getItem("recipe") || "",
    userID: localStorage.getItem("userID") || "",
    name: localStorage.getItem("name") || "",
    isPost: localStorage.getItem("isPost") || "",
    iconUrl: localStorage.getItem("iconUrl") || "",
    // role: 'unAuthenticated',
    // username: '',
    // token: '',
    categories: [
      "全部帖子",
      "健身计划",
      "饮食营养",
      "健身打卡",
      "健身问答",
      "健身挑战",
      "设备器材",
      "健身分享",
      "活动赛事",
      "初学指南",
    ],
    targetInfomation: {
      id: "",
      img: "",
      name: "",
    },
    //用户列表
    userList: [], //只返回好友的ID，无其他字段，使用其他字段从userListInformation中获取
    //购物车数据
    cartCourses: [
      {
        coursePhotoUrl:
          "https://tse2-mm.cn.bing.net/th/id/OIP-C.GIvUZUnbp2xh7xKqzV5CPgHaE7?w=268&h=180&c=7&r=0&o=5&pid=1.7",
        courseName: "运动健身",
        courseDescription:
          "这是一门综合性的运动健身课程，旨在通过多样化的训练方法，全面提升学员的身体素质、力量、耐力和灵活性。课程结合了有氧运动、力量训练、核心锻炼和柔韧性训练，适合各种健身水平的学员。",
        courseStartTime: "2022.06.07",
        courseEndTime: "2024.08.09",
        courseGrade: "3",
        coursePrice: 50,
        instructorName: "朴男",
        instructorHonors:
          "男Krisun，极限运动员，世界纪录保持者，抖音创作者，其抖音号“朴男Krisun”，拥有粉丝169.3万",
        instructorImage: "/images/pn.jpg",
        features: ["增强心肺功能", "提升肌肉力量", "塑造紧致线条"],
        selected: true,
        classTime: "10:00-11:00",
      },
      {
        coursePhotoUrl:
          "https://tse3-mm.cn.bing.net/th/id/OIP-C.VqoEEkEfYw9eANM7GUlz3AHaEo?w=276&h=180&c=7&r=0&o=5&pid=1.7",
        courseName: "普拉提",
        courseDescription:
          "这门课程专为追求健康、紧致身材的学员设计。通过结合有氧运动、力量训练和功能性练习，本课程旨在帮助学员减少体脂、塑造肌肉线条，同时提升整体的身体力量和灵活性，让身体更加有型。",
        courseStartTime: "2022.08.08",
        courseEndTime: "2024.08.09",
        courseGrade: "2",
        coursePrice: 20,
        instructorName: "帕梅拉",
        instructorHonors: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
        instructorImage: "/images/p.jpg",
        features: ["增强心肺功能", "助力有氧健身", "塑造紧致线条"],
        selected: false,
        classTime: "10:00-11:00",
      },
      {
        coursePhotoUrl:
          "https://tse3-mm.cn.bing.net/th/id/OIP-C.oXrQec5a4Au63MDb2vLCRwHaE8?w=246&h=180&c=7&r=0&o=5&pid=1.7",
        courseName: "长跑",
        courseDescription:
          "核心肌群是身体的中心力量，对于维持姿势、提高运动表现和预防受伤至关重要。这门30到45分钟的核心训练课程专为忙碌的现代人设计，旨在通过高强度、集中的训练，快速有效地加强你的核心力量和稳定性。",
        courseStartTime: "2022.08.08",
        courseEndTime: "2024.08.09",
        courseGrade: "2",
        coursePrice: 20,
        instructorName: "鹿晨辉",
        instructorHonors: "鹿晨辉，国家级健美一级裁判和国家职业健身培训师。",
        instructorImage: "/images/l.jpg",
        features: ["感受力量涌现", "增强肌肉控制", "减少受伤风险"],
        selected: true,
        classTime: "10:00-11:00",
      },
    ],
    //活力币
    vitalityCoins: 100,
    //存储用户课程表
    usercourses: [
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/static/index_news/images/temp/courseimg.jpg",
        courseName: "举重阻尼训练",
        courseDescription:
          "举重阻尼训练是一种结合了力量训练与控制力训练的高效健身课程。提高肌肉力量和爆发力，增强肌肉控制和稳定性，本课程适合各种健身水平的人士，无论是初学者还是经验丰富的运动员，都可以在教练的指导下找到适合自己的训练强度。",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-08",
        courseGrade: "4",
        coursePrice: "50",
        instructorName: "朴男",
        instructorHonors:
          "男Krisun，极限运动员，世界纪录保持者，抖音创作者，其抖音号“朴男Krisun”，拥有粉丝169.3万",
        instructorImage: "/images/pn.jpg",
        features: ["感受力量涌现", "增强肌肉控制", "训练全身各处"],
        courseProgress: "25节/32节课",
        classTime: "10:00-11:00",
      },
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/uploads/20211207/d5d8d0860359243eee20bc507bf2c231.jpg",
        courseName: "平衡与专注、持久与柔软",
        courseDescription:
          "持久与柔软瑜伽课程旨在帮助学员通过瑜伽的练习，提升身心的平衡，增强专注力，同时提高身体的持久力和柔软度。本课程适合所有级别的瑜伽爱好者，无论是初学者还是有经验的练习者，都可以在这个课程中找到适合自己的挑战和放松",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-09",
        courseGrade: "2",
        coursePrice: "10",
        instructorName: "帕梅拉",
        instructorHonors: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
        instructorImage: "/images/p.jpg",
        features: ["提升身心平衡", "助力有氧健身", "维护心理健康"],
        courseProgress: "25节/32节课",
        classTime: "10:00-11:00",
      },
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/uploads/20211207/30c00cdf6752eeda8356ecd01893dabd.jpg",
        courseName: "30到45分钟核心训练",
        courseDescription:
          "核心肌群是身体的中心力量，对于维持姿势、提高运动表现和预防受伤至关重要。这门30到45分钟的核心训练课程专为忙碌的现代人设计，旨在通过高强度、集中的训练，快速有效地加强你的核心力量和稳定性。",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-09",
        courseGrade: "3",
        coursePrice: "30",
        instructorName: "鹿晨辉",
        instructorHonors: "鹿晨辉，国家级健美一级裁判和国家职业健身培训师。",
        instructorImage: "/images/l.jpg",
        features: ["感受力量涌现", "增强肌肉控制", "减少受伤风险"],
        courseProgress: "25节/32节课",
        classTime: "10:00-11:00",
      },
      {
        coursePhotoUrl:
          "https://www.lesmills.com.cn/uploads/20211207/e4466eb6aace56fdaff24fc4c3c318af.jpg",
        courseName: "瘦身、紧致、有型",
        courseDescription:
          "这门课程专为追求健康、紧致身材的学员设计。通过结合有氧运动、力量训练和功能性练习，本课程旨在帮助学员减少体脂、塑造肌肉线条，同时提升整体的身体力量和灵活性，让身体更加有型。",
        courseStartTime: "2022-08-08",
        courseEndTime: "2024-08-09",
        courseGrade: "2",
        coursePrice: "20",
        instructorName: "帕梅拉",
        instructorHonors: "拥有国际认证的健身教练资格，包括ACE和NSCA的专业证书",
        instructorImage: "/images/p.jpg",
        features: ["增强心肺功能", "助力有氧健身", "塑造紧致线条"],
        courseProgress: "25节/32节课",
        classTime: "10:00-11:00",
      },
    ],
    //用户列表的相关信息
    userListInformation: [
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
    //购物车下单后添加课程到我的
    ADD_COURSES_TO_USER(state, newCourses) {
      state.usercourses = [...state.usercourses, ...newCourses];
    },
    //购物车下单后从购物车除去
    REMOVE_COURSE_FROM_CART(state, index) {
      state.cartCourses.splice(index, 1);
    },
    //更新活力币
    UPDATE_VITALITY_COINS(state, amount) {
      state.vitalityCoins -= amount;
    },
    //更新用户课程表
    updateUserCourses(state, courses) {
      state.usercourses = courses;
    },
    //课程大厅点击预约进入购物车
    ADD_COURSE_TO_CART(state, course) {
      state.cartCourses.push(course);
    },
    setIsPost(state, isPost) {
      state.isPost = isPost;
      localStorage.setItem("isPost", isPost);
    },

    setUserID(state, userID) {
      state.userID = userID;
      localStorage.setItem("userID", userID);
    },
    setName(state, name) {
      state.name = name;
      localStorage.setItem("name", name);
    },
    setRole(state, role) {
      state.role = role;
      // 将数据保存到 LocalStorage
      localStorage.setItem("role", role);
    },
    setUsername(state, username) {
      state.username = username;
      localStorage.setItem("username", username);
    },
    setIconUrl(state, iconUrl) {
      state.iconUrl = iconUrl;
      localStorage.setItem("iconUrl", iconUrl);
    },
    setToken(state, token) {
      state.token = token;
      localStorage.setItem("token", token);
    },
    setRecipe(state, recipe) {
      state.recipe = recipe;
      localStorage.setItem("recipe", recipe);
    },
    removeRecipe(state, recipe) {
      state.recipe = undefined;
      localStorage.removeItem("recipe");
    },
    setTargetInformation(state, info) {
      state.targetInfomation = info;
      console.log("targetInfomation:", state.targetInfomation);
    },
    setUserList(state, userList) {
      state.userList = userList;
      localStorage.setItem("userList", userList);
      console.log("Updated Friend List:", state.userList);
    },
    setUserListInformation(state, userListInformation) {
      state.userListInformation = userListInformation;
      localStorage.setItem("userListInformation", userListInformation);
      console.log("User List Information Updated:", state.userListInformation);
    },
    addMessage(state, message) {
      const targetName = message.targetName;
      const targetId = message.targetId;
      const target = state.MessageList.find(
        (item) => item.targetName === targetName
      );

      if (target) {
        target.list.push({
          is_me: message.list.is_me,
          time: message.list.time,
          message: message.list.message,
          messageType: message.list.messageType,
        });
        console.log(message.list.message);
      } else {
        // 如果没有找到匹配的 targetName，可以选择添加新的对象
        if (targetName) {
          state.MessageList.push({
            targetName: targetName,
            list: [
              {
                is_me: message.list.is_me,
                time: message.list.time,
                message: message.list.message,
                messageType: message.list.messageType,
              },
            ],
          });
        }
      }
      localStorage.setItem("MessageList", state.MessageList);
      console.log("Updated Message List:", state.MessageList);
    },
  },
  actions: {
    //上传用户课程表
    updateUserCourses({ commit }, courses) {
      commit("updateUserCourses", courses);
    },
    pollIsPost({ commit, state }) {
      setInterval(async () => {
        try {
          const response = await axios.get(
            `http://localhost:8080/api/User/GetPersonalProfile?token=${state.token}`
          );
          const newIsPost = response.data.isPost;
          console.log("isPost", newIsPost);
          if (newIsPost !== state.isPost) {
            commit("setIsPost", newIsPost);
          }
        } catch (error) {
          console.error("Error polling isPost status:", error);
        }
      }, 3000); // 每5秒检查一次
    },
  },
  modules: {
    // 在这里可以添加模块pollIsPost
  },
  getters: {
    getUserCourses: (state) => state.usercourses,
  },
});
