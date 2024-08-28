<template>
    <NavigationBar style="z-index: 1000000000" />
    <div class="forum-bg">
        <el-backtop class="backtop-button" />
        <div class="forum-container">
            <!-- 帖子卡片 -->
            <el-card class="card">
                <!-- 站内公告 -->
                <div class="announcement-section">
                    <el-row class="row">
                        <el-col :span="4">
                            <icon-home />
                        </el-col>
                        <el-col :span="20">
                            <el-text class="title">站内公告</el-text>
                        </el-col>
                    </el-row>
                    <div class="announcement-content">
                        <el-divider />
                        <el-text>📢 全新打卡活动上线！快来参与，每日打卡赢取丰厚奖励！<br />🎉 社区迎来全新升级，更多精彩功能等你体验！</el-text>
                        <el-divider />
                    </div>
                </div>

                <el-divider />

                <!-- 打卡活动 -->
                <div class="activity-section">
                    <el-row class="row">
                        <el-col :span="4">
                            <icon-calendar />
                        </el-col>
                        <el-col :span="20">
                            <el-text class="title">打卡活动</el-text>
                        </el-col>
                    </el-row>
                    <div class="activity-content">
                        <el-divider />
                        <el-text>💪 健身达人7天打卡挑战赛：完成7天连续打卡，赢取健身礼包！<br />🏃
                            每日晨跑打卡：坚持跑步，每日签到赢取健康积分！</el-text>
                        <el-divider />
                    </div>
                </div>

                <el-divider />

                <!-- 比赛活动通知 -->
                <div class="contest-section">
                    <el-row class="row">
                        <el-col :span="4">
                            <icon-trophy />
                        </el-col>
                        <el-col :span="20">
                            <el-text class="title">比赛活动</el-text>
                        </el-col>
                    </el-row>
                    <div class="contest-content">
                        <el-divider />
                        <el-text>🏅 社区健身大赛：参与比赛，展现你的健身成果，赢取丰厚奖品！<br />🏆 全年健身挑战赛：累计积分最高者将赢得终极大奖！</el-text>
                        <el-divider />
                    </div>
                </div>
            </el-card>

            <div class="main-content">
                <!-- 导航栏部分 -->
                <nav class="navbar">
                    <ul class="navbar-list">
                        <li class="navbar-item" v-for="category in visibleCategories" :key="category"
                            @click="filterByCategory(category)" :class="{ active: selectedCategory === category }">
                            {{ category }}
                            <span class="underline" v-if="selectedCategory === category"></span>
                        </li>
                    </ul>
                    <!-- 右箭头按钮 -->
                    <button class="scroll-btn" @click="scrollRight">
                        <icon-arrow-right />
                    </button>
                </nav>
                <EditArticle v-model:title="newPost.title" v-model:content="newPost.content"
                    v-model:category="newPost.category" v-model:imgUrl="newPost.imgUrl" @add-post="addPost" />

                <!-- 帖子列表部分 -->
                <div v-for="post in filteredPosts" :key="post.postID" class="post-item">
                    <div class="post-content">
                        <h3 class="post-title" @click="viewPost(post.postID)">
                            {{ post.postTitle }}
                            <span class="category-tag">{{ post.postCategory }}</span>
                        </h3>
                        <!-- 图片展示 -->
                        <div v-if="post.imgUrl" class="post-image">
                            <img :src="post.imgUrl" alt="Post Image" class="image" />
                        </div>
                        <p class="post-snippet">{{ truncatedContent(post.postContent) }}</p>
                    </div>
                    <div class="post-footer">
                        <span class="post-author">{{ post.userName }}</span>
                        <span class="post-actions">
                            <span class="icon-with-text no-click">
                                <LikeOutlined />
                                <span>{{ post.likesCount }}</span>
                            </span>
                            <span class="icon-with-text no-click">
                                <MessageOutlined />
                                <span>{{ post.commentsCount }}</span>
                            </span>
                            <span class="icon-with-text no-click">
                                <ShareAltOutlined />
                                <span>{{ post.forwardCount }}</span>
                            </span>
                        </span>
                    </div>

                </div>
            </div>

            <!-- 右侧栏：热帖展示区域 -->
            <div class="right-sidebar">
                <div class="hot-posts-section">
                    <el-row class="row">
                        <el-col :span="4">
                            <icon-fire />
                        </el-col>
                        <el-col :span="20">
                            <el-text class="title">热帖推荐</el-text>
                        </el-col>
                    </el-row>
                    <div class="hot-posts-content">
                        <el-divider />
                        <el-text v-for="hotPost in hotPosts" :key="hotPost.postID" @click="viewPost(hotPost.postID)"
                            class="hot-post-title">
                            <icon-fire class="icon-fire-small" /> {{ hotPost.postTitle }}
                        </el-text>
                        <el-divider />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { mapState } from 'vuex';
import NavigationBar from '../components/NavigationBar.vue';
import EditArticle from '../components/EditArticle.vue';
import { ElNotification } from 'element-plus';
import { IconThumbUp, IconMessage, IconCalendar, IconTrophy, IconArrowRight, IconFire, IconHome, IconShareAlt } from '@arco-design/web-vue/es/icon';
import { postMixin } from '../mixins/postMixin.js';
import { LikeOutlined, MessageOutlined, ShareAltOutlined } from '@ant-design/icons-vue';


export default {
    mixins: [postMixin],
    components: {
        NavigationBar,
        EditArticle,
        IconHome,
        IconCalendar,
        IconTrophy,
        IconArrowRight,
        IconFire,
        LikeOutlined,
        MessageOutlined,
        ShareAltOutlined,
    },
    data() {
        return {
            newPost: {
                postID: null,
                userID: null,
                userName: '',
                postTitle: '',
                postContent: '',
                postCategory: '',
                postTime: '',
                likesCount: null,
                forwardCount: null,
                commentsCount: null,
                refrencePostID: null,
                imgUrl: ''
            },
            allPosts: [],
            filteredPosts: [],
            hotPosts: [],
            selectedCategory: "全部帖子",
            currentIndex: 0,
        };
    },
    computed: {
        ...mapState(["categories"]),
        visibleCategories() {
            const doubledCategories = [...this.categories, ...this.categories];
            const startIndex = this.currentIndex % this.categories.length;
            return doubledCategories.slice(startIndex, startIndex + 6);
        },
    },
    created() {
        this.fetchAllPosts();
    },
    methods: {
        scrollRight() {
            this.currentIndex = (this.currentIndex + 1) % this.categories.length;
        },

        fetchAllPosts() {
            const token = localStorage.getItem('token');
            this.getAllPosts(token)
                .then(response => {
                    this.filteredPosts = this.allPosts;
                    this.updateHotPosts();
                })
                .catch(error => {
                    ElNotification({
                        title: '错误',
                        message: '获取帖子时发生错误，请稍后再试。',
                        type: 'error',
                    });
                });
        },

        getAllPosts(token) {
            return axios.get(`http://localhost:8080/api/Post/GetAllPost?token=${token}`)
                .then(response => {
                    this.allPosts = response.data;
                    this.filteredPosts = this.allPosts;
                    this.updateHotPosts();
                    return response;
                })
                .catch(error => {
                    ElNotification({
                        title: '错误',
                        message: '获取所有帖子时发生错误，请稍后再试。',
                        type: 'error',
                    });
                    throw error;
                });
        },

        filterByCategory(category) {
            this.selectedCategory = category;
            if (category === "全部帖子") {
                this.filteredPosts = this.allPosts;
            } else {
                this.filteredPosts = this.allPosts.filter(post => post.postCategory === category);
            }
        },

        addPost() {
            const isPost = localStorage.getItem('isPost');
            console.log("isPost", isPost)
            // 检查用户是否被禁言
            if (isPost === 0) {
                ElNotification({
                    title: '警告',
                    message: '您已被禁言，无法发帖。',
                    type: 'warning',
                });
                //return; // 阻止发帖
            }
            else {
                const token = this.$store.state.token;
                const name = localStorage.getItem('name');

                if (this.newPost.title && this.newPost.content && this.newPost.category) {
                    const cleanedContent = this.cleanHtml(this.newPost.content);
                    const newPost = {
                        postID: -1,
                        userID: -1,
                        userName: name,
                        postTitle: this.newPost.title,
                        postContent: cleanedContent,
                        postCategory: this.newPost.category,
                        postTime: new Date().toISOString(),
                        likesCount: 0,
                        forwardCount: 0,
                        commentsCount: 0,
                        refrencepostID: -1,
                        imgUrl: this.newPost.imgUrl
                    };
                    console.log("url", this.newPost.imgUrl)
                    axios.post(`http://localhost:8080/api/Post/PublishPost?token=${token}`, newPost)
                        .then(response => {
                            this.allPosts.push(newPost);
                            this.updateHotPosts();
                            this.resetNewPostForm();
                            ElNotification({
                                title: '成功',
                                message: '帖子发布成功！',
                                type: 'success',
                            });
                        })
                        .catch(error => {
                            ElNotification({
                                title: '错误',
                                message: '发布帖子时发生错误，请稍后再试。',
                                type: 'error',
                            });
                        });
                } else {
                    ElNotification({
                        title: '警告',
                        message: '请填写所有字段！',
                        type: 'warning',
                    });
                }
            }
        },

        cleanHtml(content) {
            return content.replace(/<\/?p>/g, '');
        },

        resetNewPostForm() {
            this.newPost = {
                title: '',
                content: '',
                category: '',
            };
            this.$forceUpdate();
        },

        deletePost(postID) {
            //const token = this.$store.state.token;
            const token = localStorage.getItem('token');
            axios.delete('http://localhost:8080/api/Post/DeletePostByPostID', {
                params: {
                    token: token,
                    postID: postID
                }
            })
                .then(response => {
                    this.allPosts = this.allPosts.filter(post => post.postID !== postID);
                    this.filteredPosts = this.filteredPosts.filter(post => post.postID !== postID);
                    this.updateHotPosts();
                    ElNotification({
                        title: '成功',
                        message: '帖子删除成功！',
                        type: 'success',
                    });
                })
                .catch(error => {
                    ElNotification({
                        title: '错误',
                        message: '删除帖子时发生错误，请稍后再试。',
                        type: 'error',
                    });
                });
        },

        updateHotPosts() {
            this.hotPosts = this.allPosts
                .slice()
                .sort((a, b) => (b.likesCount + b.commentsCount) - (a.likesCount + a.commentsCount))
                .slice(0, 10);
        },

        truncatedContent(content) {
            return content.length > 30 ? content.slice(0, 30) + '...' : content;
        },
    },
};
</script>

<style scoped>
body {
    margin: 0;
    padding: 0;
    font-family: Arial, sans-serif;
}

.forum-bg {
    background-image: url('../components/icons/forum-bg.jpg');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
    width: 100%;
    min-height: 100vh;
    position: absolute;
    top: 0;
    left: 0;
}

/* 导航栏样式 */
.navbar {
    margin-top: 4.5%;
    background: transparent;
    background-color: rgba(255, 255, 255, 0.6);
    padding: 10px 0;
    position: absolute;
    width: 830px;
    z-index: 100;
    top: 0;
    transition: background-color 0.3s ease;
    border-bottom: 2px solid #ccc;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.navbar-list {
    list-style: none;
    display: flex;
    gap: 30px;
    margin: 0;
    padding: 0;
    align-items: center;
}

.navbar-item {
    width: 105px;
    position: relative;
    color: black;
    cursor: pointer;
    padding: 0 0;
    transition: color 0.3s ease, transform 0.3s ease;
    text-align: center;
    font-size: 16px;
}

.navbar-item::after {
    content: '';
    position: absolute;
    left: 0;
    bottom: -2px;
    width: 0;
    height: 2px;
    background-color: #2575fc;
    transition: width 0.3s ease;
}

.navbar-item:hover {
    font-weight: bold;
    color: blue;
    transform: scale(1.1);
}

.navbar-item:hover::after {
    width: 100%;
}

.navbar-item.active {
    font-weight: bold;
    color: blue;
}

.navbar-item.active::after {
    width: 100%;
    background-color: blue;
}

.scroll-btn {
    background: none;
    border: none;
    cursor: pointer;
    font-size: 20px;
    color: black;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    transition: background-color 0.3s ease;
}

.scroll-btn:hover {
    background-color: rgba(0, 0, 0, 0.1);
}

.forum-container {
    display: flex;
    justify-content: space-between;
    padding-top: 60px;
    padding-right: 50px;
    max-width: 100%;
    margin-top: 5%;
    overflow: auto;
}

.card {
    margin-left: 1%;
    width: 400px;
    height: fit-content;
    background-color: rgba(255, 255, 255, 0.6);
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    margin-bottom: 10px;
    text-align: left;
}

.title {
    font-size: 14px;
    font-weight: bolder;
    color: #000;
    padding-left: 8px;
}

.text {
    font-size: 14px;
    color: #000;
    padding-left: 16px;
}

.tagLine {
    margin-left: 16px;
    margin-bottom: 10px;
}

.tag {
    margin-right: 5px;
    margin-bottom: 5px;
}

.row {
    display: flex;
    align-items: center;
    justify-content: center;
    margin-left: 20px;
}

.main-content {
    width: 1200px;
    padding-left: 60px;
    padding-right: 0;
}

.post-item {
    background-color: rgba(255, 255, 255, 0.6);
    color: #000;
    padding: 20px;
    margin-bottom: 20px;
    border-radius: 5px;
    border: 2px solid #ddd;
}

.post-content {
    text-align: left;
}

.post-title {
    font-size: 18px;
    color: #007bff;
    margin-bottom: 10px;
    cursor: pointer;
}

.post-title .category-tag {
    background-color: #f0f0f0;
    border-radius: 50px;
    padding: 3px 8px;
    font-size: 12px;
    color: #555;
    margin-left: 10px;
}

.post-snippet {
    font-size: 16px;
    color: #666;
}

.post-footer {
    margin-top: 15px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-size: 14px;
    color: #888;
    gap: 20px;
}

.post-actions {
    color: blue;
    display: flex;
    gap: 15px;
    align-items: center;
}

.icon-with-text {
    color: inherit;
    display: flex;
    align-items: center;
    cursor: default;
    gap: 5px;
}

.icon-with-text.no-click {
    pointer-events: none;
    cursor: default;
}

.post-author {
    font-weight: bold;
}

.icon-fire-small {
    font-size: 10px !important;
    width: 10px;
    height: 10px;
    margin-right: 8px;
    display: inline-block;
    line-height: 1;
}

.right-sidebar {
    margin-right: 1%;
    margin-left: 20px;
    width: 400px;
    height: fit-content;
    background-color: rgba(255, 255, 255, 0.6);
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    margin-bottom: 10px;
    width: 25%;
    padding-left: 0;
    display: flex;
    flex-direction: column;
}

.hot-posts-section {
    margin-top: 30px;
}

.hot-posts-content {
    padding-left: 16px;
    padding-right: 16px;
}

.hot-post-title {
    font-size: 14px;
    color: #007bff;
    cursor: pointer;
    display: block;
    margin-bottom: 8px;
    display: flex;
    align-items: center;
}

.hot-post-title:hover {
    text-decoration: underline;
}

.post-image {
    text-align: left;
    margin: 10px 0;
}

.post-image .image {
    width: 200px;
    height: auto;
    border-radius: 5px;
    display: inline-block;
}
</style>
