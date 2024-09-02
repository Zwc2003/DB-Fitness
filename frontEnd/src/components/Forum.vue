<template>
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
                <!-- 搜索框 -->
                <div>
                    <el-input v-model="searchQuery" placeholder="搜索帖子..." class="search-box" @input="filterPosts"
                        clearable>
                        <template #prefix>
                            <el-icon>
                                <search />
                            </el-icon>
                        </template>
                    </el-input>
                </div>
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
                        <div v-if="post.imgUrl != `null`" class="post-image">
                            <img :src="post.imgUrl" alt="Post Image" class="image" />
                        </div>
                        <!-- 使用 v-html 直接渲染保存的内容 -->
                        <p class="post-snippet" v-html="renderContent(post.postContent)"></p>
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
import EditArticle from '../components/EditArticle.vue';
import { ElNotification } from 'element-plus';
import { IconCalendar, IconTrophy, IconArrowRight, IconFire, IconHome } from '@arco-design/web-vue/es/icon';
import { postMixin } from '../mixins/postMixin.js';
import { LikeOutlined, MessageOutlined, ShareAltOutlined } from '@ant-design/icons-vue';
import store from '../store/index.js';
import { Search } from '@element-plus/icons-vue'; // 导入放大镜图标
import { commonMixin } from '../mixins/checkLoginState';

export default {
    mixins: [postMixin, commonMixin],
    components: {
        EditArticle,
        IconHome,
        IconCalendar,
        IconTrophy,
        IconArrowRight,
        IconFire,
        LikeOutlined,
        MessageOutlined,
        ShareAltOutlined,
        Search,
    },
    data() {
        return {
            searchQuery: '',  // 新增的搜索查询字段
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
                imgUrl: 'null'
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
        this.checkAvailable();
        this.fetchAllPosts();
        store.dispatch('pollIsPost');  // 开启轮询，更新发帖权限

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
                    // 按时间由近及远排序
                    this.allPosts = response.data.sort((a, b) => new Date(b.postTime) - new Date(a.postTime));
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
            this.filterPosts(); // 使用搜索过滤方法
        },

        filterPosts() {
            const query = this.searchQuery.toLowerCase();
            this.filteredPosts = this.allPosts.filter(post => {
                const matchesTitle = post.postTitle.toLowerCase().includes(query);
                const matchesContent = post.postContent.toLowerCase().includes(query);
                const matchesUser = post.userName.toLowerCase().includes(query);
                const matchesCategory = this.selectedCategory === '全部帖子' || post.postCategory === this.selectedCategory;
                return (matchesTitle || matchesContent || matchesUser) && matchesCategory;
            });
        },

        addPost() {
            // 检查用户是否被禁言
            if (this.$store.state.isPost === 0) {
                ElNotification({
                    title: '警告',
                    message: '您已被禁言，无法发帖。',
                    type: 'warning',
                });
                return; // 阻止发帖
            }

            const token = this.$store.state.token;
            const name = localStorage.getItem('name');

            if (this.newPost.title && this.newPost.content && this.newPost.category) {
                const cleanedContent = this.cleanHtml(this.newPost.content);
                const newPost = {
                    postID: -1,
                    userID: localStorage.getItem('userID'),
                    userName: name,
                    postTitle: this.newPost.title,
                    postContent: cleanedContent,
                    postCategory: this.newPost.category,
                    postTime: new Date().toISOString(),
                    likesCount: 0,
                    forwardCount: 0,
                    commentsCount: 0,
                    refrencepostID: -1,
                    imgUrl: this.newPost.imgUrl === 'null' ? 'null' : this.newPost.imgUrl
                };
                console.log("url", newPost.imgUrl);
                axios.post(`http://localhost:8080/api/Post/PublishPost?token=${token}`, newPost)
                    .then(response => {
                        console.log(response.data);
                        console.log(response.data.postID);
                        newPost.postID = response.data.postID;

                        // 将新帖子添加到所有帖子列表中
                        this.allPosts.unshift(newPost);

                        // 重新过滤并更新热门帖子
                        this.filterPosts();
                        this.updateHotPosts();

                        // 重置表单
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
        },


        cleanHtml(content) {
            // 将 <br> 标签替换为换行符
            let cleanedContent = content.replace(/<br\s*\/?>/gi, '<br/>');

            // 保留 <span> 标签中的样式信息
            cleanedContent = cleanedContent.replace(/<span\s+style="font-family:\s*([^;]+);?">/gi, (match, fontFamily) => {
                return `<span style="font-family:${fontFamily};">`;
            });

            // 处理其他可能的格式标签
            cleanedContent = cleanedContent.replace(/<\/?span[^>]*>/gi, '');

            return cleanedContent;
        },

        renderContent(content) {
            // 这里可以进一步处理内容，例如对其他 HTML 标签的处理
            const plainText = content.replace(/<[^>]+>/g, ''); // 移除所有HTML标签
            return plainText.length > 40 ? plainText.slice(0, 40) + '...' : plainText;
        },

        resetNewPostForm() {
            this.newPost = {
                title: '',
                content: '',
                category: '',
                imgUrl: 'null'
            };
            this.$forceUpdate();
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
/* 新增搜索框样式 */
.search-box {
    height: 50px;
    margin-top: 20px;
    margin-bottom: 20px;
    width: 100%;
    font-size: 15px;
}

/* 其他样式保持不变 */
body {
    margin: 0;
    padding: 0;
    font-family: Arial, sans-serif;
}

.forum-bg {

    min-height: 100vh;
    position: absolute;
    top: 10vh;
    left: 0;
    background-image: url('../assets/images/forum-bg.jpg');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
    width: 100vw;
    justify-content: center;
}

.announcement-content,
.activity-content,
.contest-content {
    text-align: left;
    margin: 20px;
}

/* 导航栏样式 */
.navbar {
    border-radius: 30px;
    margin-top: 2vh;
    background: transparent;
    background-color: rgba(255, 255, 255, 0.6);
    padding: 10px 0;
    position: absolute;
    width: 780px;
    /*z-index: 100;*/
    top: 0;
    transition: background-color 0.3s ease;
    border-bottom: 2px solid #ccc;
    /* 添加下方的横线 */
    display: flex;
    justify-content: space-between;
    /* 保证列表和按钮分布均匀 */
    align-items: center;
    padding-left: 20px;
}

.navbar-list {
    list-style: none;
    display: flex;
    gap: 10px;
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
    margin-top: 5vh;
    /* 在顶部留出导航栏的空间 */
    overflow: auto;
}


.card {
    margin-left: 1%;
    width: 400px;
    height: fit-content;
    background-color: rgba(255, 255, 255, 0.6);
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
    margin-bottom: 10px;
}

.title {
    font-size: 20px;
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

/*.post-footer {
      margin-top: 15px;
      display: flex;
      justify-content: space-between;
      align-items: center;
      font-size: 14px;
      color: #888;
  }*/
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
    display: flex;
    align-items: center;
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
    font-size: 15px !important;
    width: 15px !important;
    height: 15px !important;
    margin-right: 8px;
    display: inline-block;
    line-height: 1;
    /* 确保图标的高度不会因为行高影响 */
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
    /*align-items: left;*/
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
    text-align: left;
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
    /* 固定宽度 */
    height: auto;
    /* 高度自动调整以保持比例 */
    border-radius: 5px;
    display: inline-block;
}
</style>
