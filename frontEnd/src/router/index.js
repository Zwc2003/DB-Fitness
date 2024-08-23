import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from "../views/LoginView.vue";
import SignUpView from "../views/SignUpView.vue";
import CourseView from '../views/CourseView.vue';
import TweetsView from "../views/TweetsView.vue";
import CameraView from "../views/CameraView.vue";
// import FitnessChart from './components/dataVis.vue'
import DataView from "../views/DataView.vue";
import EquipmentGuide from "../views/EquipmentGuideView.vue";
import EquipmentDetails from '../components/EquipmentDetails.vue';
import adminEquipment from "../components/adminEquipment.vue"
import AddDiet from "../components/AddDiet.vue"
import FitnessGuide from "../views/FitnessGuideView.vue"
import FitnessDetail from "../components/FitnessDetail.vue"
import AdminEquipmentView from '../views/AdminEquipmentView.vue'
import MealRecordView from '../views/MealRecordView.vue'
import MealPlannerView from '../views/MealPlannerView.vue'
import HealthyDiet from '../views/HealthyDiet.vue'
import AddFoodView from '../views/AddFoodView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    //首页
    {
      path: "/",
      name: 'home',
      component: HomeView,
      meta: { requiresAuth: false }
    },
    //课程报名
    {
      path: "/course",
      name: 'course',
      component: CourseView,
      meta: { requiresAuth: false }
    },
    //健身器材
    {
      path: '/equipment',
      name: 'tweets',
      component: TweetsView,
    },
    {
      path: '/admin',
      name: 'AdminView',
      component: AdminEquipmentView,
      meta: { requiresAuth: false }
    },
    //ai健身
    {
      path: '/aifit',
      name: 'FitnessGuide',
      component: FitnessGuide,
      meta: { requiresAuth: false },
    },
    // // 论坛界面
    // {
    //   path: '/forum',
    //   name: 'ForumView',
    //   component: ForumView
    // },

    // // 用户信息界面
    // {
    //   path: '/user/:userID',
    //   //path: '/',
    //   name: 'UserProfile',
    //   component: UserProfile,
    //   props: true
    // },
    //成就页面
    // {
    //   path: '/data',
    //   name: 'dataVis',
    //   component: DataView,
    //   meta: { requiresAuth: false }
    // },
    // //排行榜页面
    // {
    //   path: '/ranking-list',
    //   name: 'RankingList',
    //   component: RankingList,
    //   meta: { requiresAuth: false }
    // }
    // 论坛界面
    // {
    //   path: '/fitnessplan',
    //   name: 'FitnessPlanView',
    //   component: FitnessPlanView
    // },
    // 健身课程
    // {
    //   path: '/course',
    //   name: 'MainCourse',
    //   component: MainCourse
    // },
    //聊天室页面
    // {
    //   path:'/chat',
    //   name:'chatRoom',
    //   component:chatRoom,
    // }
    //饮食计划页面
    {
      path: "/mealPlanner",
      name: 'MealPlanner',
      component: MealPlannerView,
      meta: { requiresAuth: false }
    },
    //饮食记录页面
    {
      path: "/mealRecord",
      name: 'MealRecord',
      component: MealRecordView,
      meta: { requiresAuth: false }
    },
    //健康饮食页面
    {
      path: "/healthyDiet",
      name: 'healtyhDiet',
      component: HealthyDiet,
      meta: { requiresAuth: false }
    },
     //增加食物部分————管理员功能
     {
      path: "/addFood",
       name: 'AddFood',
      component: AddFoodView,
      meta: { requiresAuth: false }
   },
  ]
})

export default router
