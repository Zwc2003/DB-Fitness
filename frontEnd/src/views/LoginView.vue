<template>
    <div class="background">
        <div class="login-container">
            <div class="content">
                <h1 class="title">FitFit</h1>
                <el-card class="login-card">
                    <h2 class="login-title">登录</h2>
                    <el-tabs v-model="activeName" class="tabs">
                        <el-tab-pane label="用户登录" name="user">
                            <el-form :model="LogInForm" :rules="rules" label-position="left" label-width="70px">
                                <el-form-item label="身份" prop="role" required>
                                    <el-radio-group v-model="LogInForm.role">
                                        <el-radio label="user">普通用户</el-radio>
                                        <el-radio label="coach">教练</el-radio>
                                    </el-radio-group>
                                </el-form-item>
                                <el-form-item label="邮箱" prop="email">
                                    <el-input v-model="LogInForm.email" />
                                </el-form-item>
                                <el-form-item label="密码" prop="password">
                                    <el-input v-model="LogInForm.password" type="password" show-password />
                                </el-form-item>
                                <el-row justify="center">
                                    <el-col>
                                        <el-form-item>
                                            <el-button type="primary" class="login-button"
                                                @click="submitForm">登录</el-button>
                                        </el-form-item>
                                    </el-col>
                                </el-row>
                                <el-row justify="end">
                                    <el-col :span="10">
                                        <el-link type="primary" @click="signUp">没有账号？注册一个</el-link>
                                    </el-col>
                                </el-row>
                            </el-form>
                        </el-tab-pane>
                        <el-tab-pane label="管理员登录" name="admin">
                            <el-form :model="LogInForm" :rules="rules" label-position="left" label-width="70px">
                                <el-form-item label="邮箱" prop="email">
                                    <el-input v-model="LogInForm.email" />
                                </el-form-item>
                                <el-form-item label="密码" prop="password">
                                    <el-input v-model="LogInForm.password" type="password" show-password />
                                </el-form-item>
                                <el-row justify="center">
                                    <el-col>
                                        <el-form-item>
                                            <el-button type="primary" class="login-button"
                                                @click="submitForm('admin')">登录</el-button>
                                        </el-form-item>
                                    </el-col>
                                </el-row>
                            </el-form>
                        </el-tab-pane>
                    </el-tabs>
                </el-card>
            </div>
        </div>
    </div>
</template>

<script>
import store from "../store/index.js";
import { ElNotification } from "element-plus";
import axios from "axios";


export default
    {
        data() {
            return {
                LogInForm:
                {
                    email: '',
                    password: '',
                    role: 'user' // 默认值设为用户登录
                },
                rules: {
                    email: [
                        { required: true, message: '请输入邮箱', trigger: 'blur' },
                        {
                            trigger: 'blur',
                            validator: (rule, value, callback) => {
                                const emailReg = /^([a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
                                if (!value) callback();
                                else if (emailReg.test(value)) callback();
                                else callback(new Error("邮箱地址非法"));
                            }
                        }
                    ],
                    password: [
                        { required: true, message: '请输入密码', trigger: 'blur' },
                        { min: 5, max: 16, message: '密码长度为5-16位', trigger: 'blur' }
                    ]
                },
                activeName: 'user',
            };
        },

        methods: {
            async submitForm(role) {
                try {
                    if (role === 'admin') {
                        this.LogInForm.role = 'admin';
                    }
                    const requestData = {
                        email: this.LogInForm.email,  // 这里假设 email 字段用于用户名
                        password: this.LogInForm.password,
                        role: this.LogInForm.role  // 传递用户类型到后端，可能是 'user' 或 'admin'
                    };

                    console.log("发送请求的数据: ", requestData);

                    const response = await axios.get(`http://localhost:8080/api/User/Login`, {
                        params: {
                            email: requestData.email,
                            password: requestData.password,
                            role: requestData.role
                        }
                    });
                    console.log("收到响应的数据: ", response.data.message);

                    const response1 = await axios.get(`http://localhost:8080/api/User/GetPersonalProfile?token=${response.data.token}`);

                    const message = response.data.message;
                    let notificationType = 'info';  // 默认类型为 'info'

                    if(message ==='身份权限不符'){
                        notificationType = 'error';
                    }

                    if (message === '登录成功') {
                        notificationType = 'success';

                        // 只有在登录成功时才进行角色存储和页面跳转
                        store.commit('setRole', requestData.role);
                        store.commit('setToken', response.data.token);
                        // 存储用户信息
                        store.commit('setUserID', response1.data.userID);
                        store.commit('setName', response1.data.userName);

                        // 存储当前用户发帖权限
                        store.commit('setIsPost', response1.data.isPost);

                        store.dispatch('pollIsPost');  // 开启轮询，更新发帖权限

                        if (requestData.role === 'admin') {
                            this.$router.push({ path: '/admin' });
                        } else {
                            //this.$router.push({ name: 'UserProfile' });
                            this.$router.push({ name: 'home' });
                        }
                    } else if (message === '邮箱不存在或错误' || message === '密码错误') {
                        notificationType = 'error';
                    }

                    // 无论成功与否，都显示通知
                    ElNotification({
                        message: message,
                        type: notificationType,
                        duration: 2000
                    });

                } catch (error) {
                    // 捕获请求异常并显示错误信息
                    ElNotification({
                        message: '用户名或密码错误',
                        type: 'error',
                        duration: 2000
                    });
                    console.error('Error login', error);
                }
            },

            signUp() {
                this.$router.push("/SignUp");
            }
        },
    };
</script>

<style scoped>
.background {
    background-image: url('../components/icons/background.jpg');
    background-size: cover;
    background-position: center;
    width: 100%;
    height: 100vh;
    position: absolute;
    top: 0;
    left: 0;
}


.login-container {
    position: absolute;
    top: 0;
    right: 0;
    width: 40%;
    /* 设置容器的宽度，可以根据需求调整 */
    height: 100vh;
    /* 使容器占满整个视口高度 */
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 0 0 0 0;
    /* 只在左侧圆角 */
}

.content {
    width: 70%;
    /* 设置内容区域的宽度，可以根据需求调整 */
    border-radius: 8px;
    /* 圆角 */
}

.title {
    margin-top: 20px;
    margin-bottom: 20px;
    text-align: center;
    font-size: 3.5rem;
    font-weight: bold;
    font-family: 'PingFang SC', sans-serif;
    color: rgb(44, 225, 44);
    /* 标题颜色 */
}

.login-card {
    padding: 10px;
    border: none;
    /* 去除边框 */
    box-shadow: none;
    /* 去除阴影 */
}

.login-title {
    text-align: center;
    margin-bottom: 20px;
    color: #333;
    /* 登录标题颜色 */
}

.tabs {
    margin-top: 20px;
}

.login-button {
    width: 100%;
    background-color: #409EFF;
    /* 设置按钮颜色 */
    color: #fff;
    border-radius: 5px;
}

.login-button:hover {
    background-color: #66b1ff;
    /* 悬停效果 */
}
</style>