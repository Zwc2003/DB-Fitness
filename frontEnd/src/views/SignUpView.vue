<template>
  <div style="width: 1200px; height: 700px;">
    <el-row justify="center">
      <el-col :span="6" style="margin-top:50px">
        <h1 class="title">FitFit</h1>
      </el-col>
    </el-row>
    <el-row justify="center">
      <el-card style="width:60%;margin-top:50px">
        <el-row justify="center" align="bottom">
          <el-col :span="12">
            <h2 style="text-align: center;padding: 20px">用户注册</h2>
            <el-space :size="50">
              <el-form :model="SignUpForm" :rules="rules" label-position="left" label-width="80px" style="width:400px">
                <el-form-item label="真实姓名" prop="name">
                  <el-input v-model="SignUpForm.name"></el-input>
                </el-form-item>
                <el-form-item label="性别" prop="sex">
                  <el-radio-group v-model="SignUpForm.sex">
                    <el-radio label="0">男</el-radio>
                    <el-radio label="1">女</el-radio>
                  </el-radio-group>
                </el-form-item>
                <!--                  <el-form-item label="手机号" prop="phone">-->
                <!--                    <el-input v-model="SignUpForm.phone"></el-input>-->
                <!--                  </el-form-item>-->
                <el-form-item label="密码" prop="password">
                  <el-input v-model="SignUpForm.password" show-password></el-input>
                </el-form-item>
                <el-form-item label="确认密码" prop="verifyPwd">
                  <el-input v-model="SignUpForm.verifyPwd" show-password></el-input>
                </el-form-item>
                <el-form-item label="邮箱" prop="email">
                  <el-input v-model="SignUpForm.email"></el-input>
                </el-form-item>
                <el-form-item label="验证码" prop="verifyCode">
                  <el-row gutter="20">
                    <el-col :span="12">
                      <el-input v-model="SignUpForm.verifyCode"></el-input>
                    </el-col>
                    <el-col :span="10">
                      <el-button v-show="codeShow" type="primary" @click="sendVerifyCode">获取验证码</el-button>
                      <el-button v-show="!codeShow" disabled="disabled">{{ count }}s</el-button>
                    </el-col>
                  </el-row>
                </el-form-item>
                <el-form-item>
                  <el-button type="primary" @click="SignUp" style="text-align: center;width:300px"
                    size="large">完成注册</el-button>
                </el-form-item>
              </el-form>
            </el-space>
          </el-col>
        </el-row>
      </el-card>
    </el-row>
  </div>
</template>

<script>
import { signup, code } from '../api/signup.js'
import axios from "axios";
import store from "../store/index.js";

export default
  {
    data() {
      return {
        SignUpForm:
        {
          name: '',
          sex: '',
          password: '',
          verifyPwd: '',
          phone: '',
          email: '',
          verifyCode: '',
        },
        rules:
        {
          phone: [
            { required: true, message: '请输入手机号', trigger: 'blur' },
            {
              trigger: 'blur',
              validator: (rule, value, callback) => {
                var phoneReg = 11 && /^((13|14|15|16|17|18|19)[0-9]{1}\d{8})$/;
                if (!value)
                  callback();
                else if (phoneReg.test(value)) {
                  callback();
                }
                else {
                  callback(new Error("手机号必须为11位数字且开头为13/14/15/16/17/18/19"));
                }
              }
            }
          ],
          password: [
            { required: true, message: '请输入密码', trigger: 'blur' },
            { min: 6, max: 16, message: '密码长度为6-16位', trigger: 'blur' },
          ],
          verifyPwd: [
            { required: true, message: '请确认密码', trigger: 'blur' },
            {
              // type:'string',
              // message:'密码不一致',
              trigger: 'blur',
              validator: (rule, value, callback) => {
                if (value === '')
                  callback(new Error("请确认密码"));
                else {
                  if (value !== this.SignUpForm.password)
                    callback(new Error("密码不一致"));
                  else
                    callback();
                }
              }
            }
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
          verifyCode: [
            { required: true, message: '请输入验证码', trigger: 'blur' },
            { min: 6, max: 6, message: '验证码长度为6位', trigger: 'blur' },
            {
              trigger: 'blur',
              validator: (rule, value, callback) => {
                if (!value)
                  callback();
                else {
                  if (value === this.vcode)
                    callback();
                  else
                    callback(new Error("验证码错误"));
                }
              }
            }
          ],
          name: [
            { required: true, message: '请输入昵称', transform: value => value, trigger: 'blur' },
            { min: 0, max: 20, message: '昵称过长', trigger: 'blur' },
            {
              type: 'string',
              message: '昵称不能包含空格',
              trigger: 'blur',
              transform(value) {
                if (value && value.indexOf(' ') === -1) {
                  return value
                }
                else {
                  return false
                }
              }
            }
          ],
          sex: [
            { required: true },
          ],
        },
        verifyPwd: '',
        count: '',
        timer: null,
        codeShow: true,
        vcode: '',
      }
    },

    methods:
    {
      sendVerifyCode() {
        console.log(this.SignUpForm);
        const TIME_LIMIT = 60;
        code(this.SignUpForm.email).then(res => {
          this.vcode = res.data.data.toString();
        })
        if (!this.timer) {
          this.count = TIME_LIMIT;
          this.codeShow = false;
          this.timer = setInterval(() => {
            if (this.count > 0) {
              this.count--;
            }
            else {
              this.codeShow = true;
              clearInterval(this.timer);
              this.timer = null;
            }
          }, 1000);
        }
      },

      async SignUp() {
        try {
          // const response = await axios({
          //   method: 'post',
          //   url: 'http://localhost:8080/user/register',
          //   data: this.SignUpForm,
          //   headers: {
          //     'Content-Type': 'application/x-www-form-urlencoded'
          //   }
          // })
          signup(this.SignUpForm).then(res => {
            console.log(res);
          })
        } catch (error) {
          this.$message({
            message: '注册失败',
            type: 'error'
          });
          console.error('Error registering:', error);
        }

        try {
          // store.commit('setRole', this.activeName)
          // store.commit('setUsername', this.SignUpForm.name)
          // store.commit('setToken', response.data)
          // console.log(store.state.role)
          // console.log(store.state.username)
          // console.log(store.state.token)
          // 跳转至首页
          this.$router.push({ name: 'LogInView' });
        } catch (error) {
          this.$message({
            message: '登录失败，用户名或密码错误',
            type: 'error'
          });
          console.error('Error login', error);
        }

      }
    }
  }
</script>

<style scoped>
.signup {
  width: 80%;
  height: auto;
  background-color: white;
  border-radius: 4px;
}

.title {
  text-shadow: -3px 3px 1px #5f565e;
  text-align: center;
  margin-top: 10%;
  color: white;
  font-size: 40px;
}

.el-col-12 {
  flex: 0 0 50%;
  max-width: 100%;
}

.bg-page {
  width: 100%;
  height: 100vh;
  background: url("../assets/background.png") center center no-repeat;
  background-size: 100% 100%;
  position: absolute;
}
</style>