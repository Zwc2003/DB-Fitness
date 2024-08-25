import { createApp } from "vue";
import { createStore } from "vuex";

const store = createStore({
  state: {
    courses: JSON.parse(localStorage.getItem("courses")) || [], // 总的课程池
    userCourses: JSON.parse(localStorage.getItem("userCourses")) || [], // 每个用户自己的课程池
  },
  mutations: {
    // 教练添加新课程
    addCourse(state, course) {
      state.courses.push(course);
      localStorage.setItem("courses", JSON.stringify(state.courses));
    },
    // 教练更新课程信息
    updateCourse(state, updatedCourse) {
      const index = state.courses.findIndex((c) => c.id === updatedCourse.id);
      if (index !== -1) {
        state.courses.splice(index, 1, updatedCourse);
        localStorage.setItem("courses", JSON.stringify(state.courses));
      }
    },
    // 教练删除课程
    removeCourse(state, courseId) {
      const updatedCourses = state.courses.filter(
        (course) => course.id !== courseId
      );
      state.courses = updatedCourses;
      localStorage.setItem("courses", JSON.stringify(state.courses));
    },
    // 普通用户选择课程
    selectCourse(state, { userID, course }) {
      // 检查用户ID是否存在于userCourses对象中
      if (!state.userCourses[userID]) {
        state.userCourses[userID] = []; // 如果不存在，初始化为空数组
      }
      // 确保不重复添加相同的课程
      const courseExists = state.userCourses.some((uc) => uc.id === course.id);
      if (!courseExists) {
        state.userCourses.push(course);
        localStorage.setItem("userCourses", JSON.stringify(state.userCourses));
      }
    },
  },
  actions: {
    // 异步选择课程
    // async selectCourseAsync({ commit, state }, course) {
    //   try {
    //     // 假设添加到服务器
    //     await API.selectCourse(course);
    //     // 如果成功，提交mutation更新本地状态
    //     commit("selectCourse", course);
    //   } catch (error) {
    //     console.error("选择课程失败:", error);
    //   }
    // },
  },
  modules: {
    // 在这里可以添加模块
  },
});

const app = createApp({
  /* 根组件 */
});

// 将 store 实例作为插件安装
app.use(store);
