<template>
    <div class="background">
        <div class="login-container">
            <div class="gradient-background"></div>
            <div class="content">
                <h1 class="title">FitFit</h1>
                <el-card class="login-card">
                    <h2 class="login-title">用户注册</h2>
                    <el-form :model="SignUpForm" :rules="rules" label-position="left" label-width="80px">
                        <el-form-item label="用户昵称" prop="name">
                            <el-input v-model="SignUpForm.name"></el-input>
                        </el-form-item>
                        <!-- <el-form-item label="性别" prop="sex">
                            <el-radio-group v-model="SignUpForm.sex">
                                <el-radio label="0">男</el-radio>
                                <el-radio label="1">女</el-radio>
                            </el-radio-group>
                        </el-form-item> -->
                        <el-form-item label="用户类型" prop="userType">
                            <el-radio-group v-model="SignUpForm.userType">
                                <el-radio label="user">普通用户</el-radio>
                                <el-radio label="coach">教练</el-radio>
                            </el-radio-group>
                        </el-form-item>
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
                                    <el-button v-show="codeShow" type="primary"
                                        @click="sendVerifyCode">获取验证码</el-button>
                                    <el-button v-show="!codeShow" disabled="disabled">{{ count }}s</el-button>
                                </el-col>
                            </el-row>
                        </el-form-item>
                    </el-form>
                    <el-form-item>
                        <el-button type="primary" class="return-button" @click="returnLoginView">返回</el-button>
                        <el-button type="primary" class="login-button" @click="SignUp">完成注册</el-button>
                    </el-form-item>
                </el-card>
            </div>
        </div>
    </div>
</template>

<script>
import store from "../store";
import axios from "axios";
import { ElNotification } from "element-plus";

export default {
    data() {
        return {
            SignUpForm: {
                name: '',
                // sex: '',
                password: '',
                verifyPwd: '',
                email: '',
                verifyCode: '',
                userType: 'user',
            },
            rules: {
                // 保留界面字段的验证规则
                password: [
                    { required: true, message: '请输入密码', trigger: 'blur' },
                    { min: 6, max: 16, message: '密码长度为6-16位', trigger: 'blur' },
                ],
                verifyPwd: [
                    { required: true, message: '请确认密码', trigger: 'blur' },
                    {
                        trigger: 'blur',
                        validator: (rule, value, callback) => {
                            if (value === '') callback(new Error("请确认密码"));
                            else {
                                if (value !== this.SignUpForm.password) callback(new Error("密码不一致"));
                                else callback();
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
                            if (!value) callback();
                            else {
                                if (emailReg.test(value)) callback();
                                else callback(new Error("邮箱地址非法"));
                            }
                        }
                    }
                ],
                verifyCode: [
                    { required: true, message: '请输入验证码', trigger: 'blur' },
                    { min: 6, max: 6, message: '验证码长度为6位', trigger: 'blur' },
                ],
                name: [
                    { required: true, message: '请输入昵称', trigger: 'blur' },
                    { min: 0, max: 20, message: '昵称过长', trigger: 'blur' },
                ],
                sex: [
                    { required: true },
                ],
                userType: [
                    { required: true, message: '请选择用户类型', trigger: 'change' }
                ],
            },
            count: '',
            timer: null,
            codeShow: true,
            vcode: '',
        }
    },

    methods: {
        sendVerifyCode() {
            console.log(this.SignUpForm);
            const TIME_LIMIT = 60;
            const email = this.SignUpForm.email;
            // 使用模拟发送验证码功能
            axios.post(`http://localhost:8080/api/User/SendVerificationCode?email=${email}`)
            // .then(res => {
            //     this.vcode = res.data.message;
            // })
            console.log(res.data.message);
            if (!this.timer) {
                this.count = TIME_LIMIT;
                this.codeShow = false;
                this.timer = setInterval(() => {
                    if (this.count > 0) {
                        this.count--;
                    } else {
                        this.codeShow = true;
                        clearInterval(this.timer);
                        this.timer = null;
                    }
                }, 1000);
            }
        },

        async SignUp() {
            try {
                const requestData = {
                    email: this.SignUpForm.email,
                    password: this.SignUpForm.password,
                    accountName: this.SignUpForm.name,
                    role: this.SignUpForm.userType === 'user' ? 'user' : 'coach',
                    coachName: this.SignUpForm.name,
                    verificationCode: this.SignUpForm.verifyCode
                    //registerTime: new Date().toISOString(), // ISO 8601 格式的注册时间
                    //coachName:this.SignUpForm.coachName
                };

                // // 首先检查验证码是否正确
                // if (this.SignUpForm.verifyCode !== this.vcode) {
                //     ElNotification({
                //         message: "验证码错误，请重新输入",
                //         type: 'error',
                //         duration: 2000
                //     });
                //     // 清空验证码字段
                //     this.SignUpForm.verifyCode = '';
                //     return; // 终止注册流程
                // }
                const response = await axios.post(`http://localhost:8080/api/User/Register`, requestData);

                // 判断后端返回的响应内容
                if (response.data.message === "成功注册") {
                    store.commit('setName', this.SignUpForm.name)
                    console.log('注册成功');
                    ElNotification({
                        message: "注册成功",
                        type: 'success',
                        duration: 2000
                    });

                    // 跳转到登录页面
                    this.$router.push({ name: 'LogInView' });
                } else if (response.data.message === "验证码错误或已过期") {
                    console.log('注册失败：验证码错误或已过期');
                    ElNotification({
                        message: "注册失败：验证码错误或已过期",
                        type: 'error',
                        duration: 2000
                    });
                    // 清空表单内容，要求用户重新填写
                    this.resetForm();
                }
                else if (response.data.message === "注册失败：邮箱已存在") {
                    console.log('注册失败：邮箱已存在');
                    ElNotification({
                        message: "注册失败：邮箱已存在，请重新填写",
                        type: 'error',
                        duration: 2000
                    });
                    // 清空表单内容，要求用户重新填写
                    this.resetForm();
                }


            } catch (error) {
                ElNotification({
                    message: '注册失败，请稍后再试',
                    type: 'error',
                    duration: 2000
                });
                console.error('Error registering:', error);
            }
        },

        // 新增一个方法来重置表单内容
        resetForm() {
            this.$refs['SignUpForm'].resetFields(); // 重置表单字段
        },

        returnLoginView() {
            this.$router.go(-1);
        },
    }
}
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

.return-button {
    width: 16%;
    background-color: #409EFF;
    margin-left: 0;
    margin-right: 5px;
}

.login-button {
    width: 79%;
    background-color: #409EFF;
    color: #fff;
    border-radius: 5px;
    margin-right: 0;
}


.login-button:hover {
    background-color: #66b1ff;
    /* 悬停效果 */
}
</style>