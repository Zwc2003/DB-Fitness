import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/HomeView.vue";
import PublishCourse from "@/views/PublishCourse.vue";
import UserHomePage from "@/views/UserHomePage.vue";
import Debug from "@/views/Debug.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "HomeView",
      component: HomeView,
    },
    //健身教练管理课程界面
    {
      path: "/publishcourse",
      name: "PublishCourse",
      component: PublishCourse,
    },
    //普通用户管理课程界面
    {
      path: "/userhome",
      name: "UserHomePage",
      component: UserHomePage,
    },
    {
      path: "/debug",
      name: "Debug",
      component: Debug,
    },
  ],
});
export default router;
