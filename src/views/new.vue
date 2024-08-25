<template>
  <div aria-label="A complete example of page header">
    <el-page-header @back="onBack">
      <template #breadcrumb>
        <el-breadcrumb separator="/">
          <el-breadcrumb-item :to="{ path: './page-header.html' }">
            FitFit
          </el-breadcrumb-item>
          <el-breadcrumb-item>
            <a href="./page-header.html">健身课程</a>
          </el-breadcrumb-item>
          <el-breadcrumb-item>我的课程</el-breadcrumb-item>
        </el-breadcrumb>
      </template>
      <template #content>
        <div class="flex items-center">
          <el-avatar
            class="mr-3"
            :size="32"
            src="https://ts3.cn.mm.bing.net/th?id=OIP-C.9khWcYup3srhgw3V1fi7-QHaHa&w=250&h=250&c=8&rs=1&qlt=90&o=6&pid=3.1&rm=2"
          />
          <span class="text-large font-600 mr-3">李华</span>
          <span
            class="text-sm mr-2"
            style="color: var(--el-text-color-regular)"
          >
            2033458
          </span>
          <el-tag>欢迎您~</el-tag>
        </div>
      </template>
      <template #extra>
        <div class="flex items-center">
          <el-button>vip2</el-button>
          <el-button type="primary" class="ml-2" @click="toggleCartSidebar"
            >课程购物车</el-button
          >
        </div>
      </template>

      <!-- 右边栏 -->
      <transition name="slide">
        <div v-if="isCartVisible" class="cart-sidebar">
          <!-- 收回箭头 -->
          <el-icon style="font-size: 20px"><ShoppingTrolley /></el-icon>
          <b>课程购物车</b>
          <div class="cart-header">
            收起
            <el-icon @click="toggleCartSidebar" style="font-size: 24px">
              <Right />
            </el-icon>
          </div>
          <div class="cart-titles">
            <span>课程名称</span>
            <span>课程单价</span>
            <span>课程时间</span>
            <span>操作</span>
          </div>
          <!-- 购物车内容 -->
          <div
            v-for="(course, index) in cartCourses"
            :key="index"
            class="cart-item"
          >
            <el-checkbox v-model="course.selected" />
            <img :src="course.image" alt="课程图片" class="course-image" />
            <span>{{ course.name }}</span>
            <span>￥{{ course.price }}</span>
            <span>{{ course.time }}</span>
            <span class="delete-text" @click="removeCourse(index)">删除</span>
          </div>
          <!-- 统计信息 -->
          <div class="cart-summary">
            <span>
              共{{ cartCourses.length }}个课程，已选择{{
                selectedCourses.length
              }}个，课程共计：
              <strong class="total-price">￥{{ totalPrice }}</strong>
            </span>
            <el-button type="primary" class="checkout-button"
              >下单结算</el-button
            >
          </div>
        </div>
      </transition>
    </el-page-header>
  </div>
</template>

<script>
import { ref, computed } from "vue";

export default {
  setup() {
    const isCartVisible = ref(false);

    const cartCourses = ref([
      // 示例课程数据
      {
        image:
          "https://tse2-mm.cn.bing.net/th/id/OIP-C.GIvUZUnbp2xh7xKqzV5CPgHaE7?w=268&h=180&c=7&r=0&o=5&pid=1.7",
        name: "运动健身",
        price: 100,
        time: "2022 06 07-2023 06 07",
        selected: true,
      },
      {
        image:
          "https://tse3-mm.cn.bing.net/th/id/OIP-C.VqoEEkEfYw9eANM7GUlz3AHaEo?w=276&h=180&c=7&r=0&o=5&pid=1.7",
        name: "普拉提",
        price: 200,
        time: "2022 01 01-2023 01 01",
        selected: false,
      },
      {
        image:
          "https://tse3-mm.cn.bing.net/th/id/OIP-C.oXrQec5a4Au63MDb2vLCRwHaE8?w=246&h=180&c=7&r=0&o=5&pid=1.7",
        name: "长跑",
        price: 150,
        time: "2022 11 13-2023 11 13",
        selected: true,
      },
    ]);

    const toggleCartSidebar = () => {
      isCartVisible.value = !isCartVisible.value;
    };

    const removeCourse = (index) => {
      cartCourses.value.splice(index, 1);
    };

    const selectedCourses = computed(() =>
      cartCourses.value.filter((course) => course.selected)
    );

    const totalPrice = computed(() =>
      selectedCourses.value.reduce((sum, course) => sum + course.price, 0)
    );

    return {
      isCartVisible,
      cartCourses,
      toggleCartSidebar,
      removeCourse,
      selectedCourses,
      totalPrice,
    };
  },
};
</script>

<style scoped>
.cart-sidebar {
  position: fixed;
  top: 0;
  right: 0;
  width: 400px;
  height: 100%;
  background-color: #fff;
  box-shadow: -2px 0 5px rgba(0, 0, 0, 0.1);
  padding: 20px;
  z-index: 1000;
}

.cart-header {
  display: flex;
  justify-content: flex-end;
}

.cart-titles {
  margin-top: 20px;
  display: flex;
  justify-content: space-between;
  margin-bottom: 10px;
  font-weight: bold;
}

.cart-item {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.cart-item > * {
  margin-right: 20px; /* 调整子元素之间的间距 */
}

.course-image {
  width: 100px;
  height: 100px;
  margin-left: 15px;
}

.delete-text {
  color: red;
  cursor: pointer;
}

.delete-text:hover {
  text-decoration: underline;
}

.cart-summary {
  margin-top: 20px;
  font-size: 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.total-price {
  color: red;
  font-weight: bold;
}

.checkout-button {
  background-color: #409eff;
  color: #fff;
  margin-left: auto;
}

.slide-enter-active,
.slide-leave-active {
  transition: all 0.3s ease;
}

.slide-enter,
.slide-leave-to /* .slide-leave-active in <2.1.8 */ {
  transform: translateX(100%);
}
.el-page-header {
  width: 1200px;
  margin-top: -390px;
  margin-right: 100px;
}
</style>
