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
import AdminEquipmentView from '../views/AdminEquipmentView.vue';

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
    {
      path: '/Tweets',
      name: 'tweets',
      component: TweetsView,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue')
    },
    //登录页面
    {
      // path:'/login',
      path: "/login",
      name: 'LogInView',
      component: LoginView,
      meta: { requiresAuth: false }
    },
    //注册页面
    {
      path: '/SignUp',
      name: 'SignUpView',
      component: SignUpView,
      meta: { requiresAuth: false }
    },
    //相机页面
    {
      path: '/admin',
      name: 'AdminView',
      component: AdminEquipmentView,
      meta: { requiresAuth: false }
    },
    //数据页面
    {
      path: '/data',
      name: 'EquipmentGuide',
      component: EquipmentGuide,
      meta: { requiresAuth: false }
    },
    {
      path: '/equipment/:equipmentName',
      name: 'EquipmentDetails',
      component: EquipmentDetails,
      props: route => ({
        equipmentName: route.query.equipmentName,
        instructions: route.query.instructions,
      }),
    },
    {
      path: '/fit',
      name: 'FitnessGuide',
      component: FitnessGuide,
      meta: { requiresAuth: false },
    },
    {
      path: '/detail',
      name: 'FitnessDetail',
      component: FitnessDetail
    },
  ]
})

export default router
