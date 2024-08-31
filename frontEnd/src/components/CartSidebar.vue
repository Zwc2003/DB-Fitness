<template>
  <div>
    <!-- 显示购物车按钮 -->
    <el-button type="primary" @click="toggleCartSidebar" class="cart-button">
      课程购物车
    </el-button>

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
          <img
            :src="course.coursePhotoUrl"
            alt="课程图片"
            class="course-image"
          />
          <span>{{ course.courseName }}</span>
          <span
            >{{ course.coursePrice }}<el-icon><Coin /></el-icon
          ></span>
          <span>{{ course.courseStartTime }}-{{ course.courseEndTime }}</span>
          <span class="delete-text" @click="removeCourse(index)">删除</span>
        </div>
        <!-- 统计信息 -->
        <div class="cart-summary">
          <span>
            共{{ cartCourses.length }}个课程，已选择{{
              selectedCourses.length
            }}个，课程共计：
            <strong class="total-price"
              >{{ totalPrice }}<el-icon><Coin /></el-icon
            ></strong>
          </span>
          <el-button type="primary" class="checkout-button" @click="checkout"
            >下单结算</el-button
          >
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
import { ref, computed } from "vue";

export default {
  props: {
    cartCourses: {
      type: Array,
      required: true,
      default: [],
    },
    isCartVisible: {
      type: Boolean,
      required: true,
      default: true,
    },
    usercourses: {
      type: Array,
      required: true,
      default: [],
    },
  },
  emits: [
    "update:isCartVisible",
    "removeCourse",
    "update:userCourses",
    "checkout",
  ],
  setup(props, { emit }) {
    const toggleCartSidebar = () => {
      emit("update:isCartVisible", !props.isCartVisible);
    };

    const removeCourse = (index) => {
      emit("removeCourse", index);
    };

    const selectedCourses = computed(() =>
      props.cartCourses.filter((course) => course.selected)
    );

    const totalPrice = computed(() =>
      selectedCourses.value.reduce(
        (total, course) => total + course.coursePrice,
        0
      )
    );

    const checkout = () => {
      emit("checkout", selectedCourses.value);
    };

    return {
      toggleCartSidebar,
      removeCourse,
      selectedCourses,
      totalPrice,
      checkout,
    };
  },
};
</script>

<style scoped>
.cart-button {
  margin-bottom: 20px;
}

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
</style>
