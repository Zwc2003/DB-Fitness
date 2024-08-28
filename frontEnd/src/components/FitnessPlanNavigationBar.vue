<template>
    <div class="head_con">
        <div class="logo-container">
            <img src="../assets/logo.png" alt="FitFit" class="logo">
        </div>
        <div class="wrapper">
            <nav>
                <input type="radio" name="tab" id="home" :checked="$route.path === '/'">
                <input type="radio" name="tab" id="comment" :checked="$route.path === '/Tweets'">
                <input type="radio" name="tab" id="message" :checked="$route.path === '/course'">
                <input type="radio" name="tab" id="star" :checked="$route.path === '/camera'">
                <input type="radio" name="tab" id="user" :checked="$route.path === '/data'">
                <input type="radio" name="tab" id="plan" :checked="$route.path === '/fitnessplan'">

                <!--这一行是注释-->
                <label for="home" class="home" @click="delayedNavigation('/')">

                        <el-icon>
                            <House />
                        </el-icon>
                        首页
                </label>
                <label for="comment" class="comment" @click="delayedNavigation('/Tweets')">
                  <router-link to="/Tweets">
                    <el-icon>
                        <ChatLineRound />
                    </el-icon>
                    健身推文
                  </router-link>
                </label>
                <label for="message" class="message" @click="delayedNavigation('/course')">
                  <router-link to="/course">
                        <el-icon>
                            <Collection />
                        </el-icon>
                        健身课程
                  </router-link>
                </label>
                <label for="star" class="star" @click="delayedNavigation('/camera')">
                    <router-link to="/">
                        <el-icon>
                            <Star />
                        </el-icon>
                        AI爱健身
                    </router-link>
                </label>
                <label for="user" class="user" @click="delayedNavigation('/data')">
                  <el-icon>
                      <DataAnalysis />
                  </el-icon>
                  数据统计
                </label>
                <label for="plan" class="plan" @click="delayedNavigation('/fitnessplan')">
                  <el-icon>
                    <Operation />
                  </el-icon>
                  健身计划
                </label>
                <div class="tab"></div>

            </nav>
        </div>
        <div class="avatar-container">
            <el-dropdown>
                <img src="../assets/user.jpeg" alt="User" class="dropdownlink">
                <template #dropdown>
                    <el-dropdown-menu>
                        <el-dropdown-item>
                            <el-icon>
                                <Setting />
                            </el-icon>
                            账号设置
                        </el-dropdown-item>
                        <el-dropdown-item>
                            <el-icon>
                                <Switch />
                            </el-icon>
                            切换账号
                        </el-dropdown-item>
                    </el-dropdown-menu>
                </template>
            </el-dropdown>
        </div>
    </div>
</template>

<style>
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: flex-start;
    text-align: center;
    background: #fff;
}

.head_con {
    display: flex;
    align-items: center;
    position: absolute;
    top: 3%;
    left: 0;
    width: 100%;
    padding-left: 5%;
    padding-right: 3%;
}

.logo-container {
    margin-right: 20px;
}

.logo {
    height: 50px;
    display: block;
}

.wrapper,
.avatar-container {
    height: 50px;
    display: flex;
    align-items: center;
    margin-left: auto;
}

.wrapper {
    width: 60vw;
    line-height: 50px;
    background-color: white;
    box-shadow: 0px 5px 15px rgba(0, 0, 0, 0.25);
    border-radius: 50px;
}

.wrapper nav {
    display: flex;
    flex: 1;
    position: relative;
}

.wrapper nav label {
    flex: 1;
    width: 100%;
    position: relative;
    z-index: 1;
    cursor: pointer;
}

.wrapper nav label a {
    position: relative;
    z-index: -1;
    color: #333;
    font-size: 16px;
    font-weight: 500;
    text-decoration: none;
}

.wrapper nav input {
    display: none;
}

.wrapper nav .tab {
    position: absolute;
    height: 100%;
    width: 16.7%;
    left: 0px;
    bottom: 0px;
    background: linear-gradient(to right, #f09819, #ff5858);
    border-radius: 50px;
    transition: 0.6s cubic-bezier(0.68, -0.55, 0.265, 1.55);
}

.wrapper nav #home:checked~label.home a,
.wrapper nav #comment:checked~label.comment a,
.wrapper nav #message:checked~label.message a,
.wrapper nav #star:checked~label.star a,
.wrapper nav #user:checked~label.user a,
.wrapper nav #plan:checked~label.plan a {
    color: #fff;
    transition: 0.6s;
}

.wrapper nav #comment:checked~.tab {
    left: 16.7%;
}

.wrapper nav #message:checked~.tab {
    left: 33.4%;
}

.wrapper nav #star:checked~.tab {
    left: 50%;
}

.wrapper nav #user:checked~.tab {
    left: 66.7%;
}
.wrapper nav #plan:checked~.tab {
  left: 83.4%;
}
.avatar-container {
    position: relative;
    display: flex;
    align-items: center;
    height: 50px;
    border: none;
}

.dropdownlink {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    cursor: pointer;
    border: 5px solid #fff;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.20);
}

.example-showcase .el-dropdown-link {
    cursor: pointer;
    color: var(--el-color-primary);
    display: flex;
    align-items: center;
}

@media (min-width: 1024px) {
    #app {
        display: grid;
        grid-template-columns: 1fr;
        padding: 0 2rem;
    }
}
</style>

<script>

import router from "../router/index.js";
import Canvas from "../components/Canvas.vue";
import {Operation} from "@element-plus/icons-vue";

export default {
  name: "FitnessPlanNavigationBar", 
  components: {Operation, Canvas},
  methods: {
    router() {
      return router
    },
    delayedNavigation(target) {
      setTimeout(() => {
        this.router().push(target); // 替换为你的目标路由
      }, 500); // 1000 毫秒 = 1 秒
    }
  },
}
</script>