<template>
  <div style="width: 1200px; height: 700px">
    <el-row justify="center">
      <el-col :span="8">
        <h1 class="title">FitFit</h1>
      </el-col>
    </el-row>

    <el-row justify="center" align="bottom">
      <el-card style="width: 60%; height: 100%; align-items: center;padding: 10px;margin-top:50px">
        <el-col class="login">
          <h2 style="text-align: center;">登录</h2>
          <el-tabs v-model="activeName" class="tabs">
            <el-tab-pane label="用户登录" name="user">
              <el-form :model="LogInForm" :rules="rules" label-position="left" label-width="70px">
                <el-form-item label="邮箱" prop="email">
                  <el-input v-model="LogInForm.email" />
                </el-form-item>
                <el-form-item label="密码" prop="password">
                  <el-input v-model="LogInForm.password" type="password" show-password />
                </el-form-item>
                <el-row jsutify="center">
                  <el-col>
                    <el-form-item>
                      <el-button type="primary" style="width:200px;" @click="submitForm">登录</el-button>
                    </el-form-item>
                  </el-col>
                </el-row>

                <el-row justify="end">
                  <el-col :span="9">
                    <el-link type="primary" @click="signUp">没有账号？注册一个</el-link>
                  </el-col>
                </el-row>

              </el-form>
            </el-tab-pane>
            <!--            <el-tab-pane label="管理员登录" name="admin">-->
            <!--              <el-form :model="AdminForm" :rules="rules" label-position="left" label-width="70px">-->
            <!--                <el-form-item label="用户名" prop="name">-->
            <!--                  <el-input v-model="AdminForm.name" />-->
            <!--                </el-form-item>-->
            <!--                <el-form-item label="密码" prop="password">-->
            <!--                  <el-input v-model="AdminForm.password" type="password" show-password />-->
            <!--                </el-form-item>-->
            <!--                <el-row jsutify="center">-->
            <!--                  <el-col>-->
            <!--                    <el-form-item>-->
            <!--                      <el-button type="primary" style="width:200px;" @click="submitAdminForm" >登录</el-button>-->
            <!--                    </el-form-item>-->
            <!--                  </el-col>-->
            <!--                </el-row>-->
            <!--                <el-row justify="end">-->
            <!--                  <el-col :span="9">-->
            <!--                    <el-link type="primary" @click="signUp">没有账号？注册一个</el-link>-->
            <!--                  </el-col>-->
            <!--                </el-row>-->
            <!--              </el-form>-->
            <!--            </el-tab-pane>-->
          </el-tabs>
        </el-col>
      </el-card>
    </el-row>
  </div>
</template>

<script>
import store from "../store/index.js";
import axios from "axios";
import { userLogin } from "../api/functions.js";

export default
  {
    data() {
      return {
        LogInForm:
        {
          email: '',
          password: ''
        },
        AdminForm:
        {
          name: '',
          password: ''
        },
        rules:
        {
          username: [
            { required: true, message: '请输入用户名', trigger: 'blur' },
            { min: 2, max: 16, message: '用户名为2-16位', trigger: 'blur' }
          ],
          password: [
            { required: true, message: '请输入密码', trigger: 'blur' },
            { min: 5, max: 16, message: '密码长度为5-16位', trigger: 'blur' }
          ],
          name: [
            { required: true, message: '请输入用户名', trigger: 'blur' },
            { min: 2, max: 16, message: '用户名为2-16位', trigger: 'blur' }
          ],
          email: [
            { required: true, message: '请输入邮箱', trigger: 'blur' },
            {
              trigger: 'blur',
              validator: (rule, value, callback) => {
                const emailReg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
                if (!value)
                  callback();
                else {
                  if (emailReg.test(value))
                    callback();
                  else
                    callback(new Error("邮箱地址非法"));
                }
              }
            }
          ],
        },
        remember: false,
        activeName: 'user',
      }
    },
    mounted() {

    },
    methods:
    {
      parallax(e) {
        document.querySelectorAll(".layer").forEach((layer) => {
          const speed = layer.getAttribute("data-speed") || 2;
          const x = (window.innerWidth - e.pageX * speed) / 100;
          const y = (window.innerHeight - e.pageY * speed) / 100;
          //设置 X轴和Y轴同时同时移动
          layer.style.transform = `translateX(${x}px)`, `translateY(${y}px)`;
        })
      },

      async submitForm() {
        try {
          const response = await axios({
            method: 'post',
            url: 'http://localhost:8080/user/login',
            data: {
              email: this.LogInForm.email,
              password: this.LogInForm.password
            },
          })

          store.commit('setRole', this.activeName)
          store.commit('setToken', response.data.data.token)
          console.log(store.state.role)
          console.log(store.state.token)
          // 跳转至首页
          this.$router.push({ name: 'home' });


        } catch (error) {
          this.$message({
            message: '用户名或密码错误',
            type: 'error'
          });
          console.error('Error login', error);
        }

      },

      async submitAdminForm() {
        try {
          const response = await axios({
            method: 'post',
            url: 'http://localhost:8080/login',
            data: {
              name: this.AdminForm.name,
              pwd: this.AdminForm.password
            },
            // headers: {
            //   'Content-Type': 'application/x-www-form-urlencoded'
            // }
          })

          store.commit('setRole', this.activeName)
          store.commit('setUsername', this.AdminForm.name)
          store.commit('setToken', response.data.data.token)
          console.log(store.state.role)
          console.log(store.state.username)
          console.log(store.state.token)
          // 跳转至首页
          this.$router.push({ name: 'Audit' });


        } catch (error) {
          this.$message({
            message: '用户名或密码错误',
            type: 'error'
          });
          console.error('Error login', error);
        }

      },

      signUp() {
        this.$router.push("/SignUp")
      }
    }
  }
</script>

<style scoped>
.login {
  width: 500px;
  height: auto;
  background-color: white;
  border-radius: 10px;
  align-items: center;
  margin-left: 90px
}

.title {
  text-shadow: -3px 3px 1px #5f565e;
  text-align: center;
  margin-top: 30%;
  color: white;
  font-size: 40px;
}

.tabs>.el-tabs__content {
  width: 500px;
  padding: 32px;
  color: #6b778c;
  font-size: 32px;
  font-weight: 600;
}
</style>