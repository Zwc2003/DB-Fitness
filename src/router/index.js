import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/views/HomeView.vue";
import GetCourseCar from "@/views/GetCourseCar.vue";
import PublishCourse from "@/views/PublishCourse.vue";
import UserHomePage from "@/views/UserHomePage.vue";
import New from "@/views/new.vue";

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
    //普通用户课程购物车界面
    {
      path: "/getcourse",
      name: "GetCourseCar",
      component: GetCourseCar,
    },
    //普通用户管理课程界面
    {
      path: "/userhome",
      name: "UserHomePage",
      component: UserHomePage,
    },
    {
      path: "/new",
      name: "New",
      component: New,
    },
  ],
});
export default router;
