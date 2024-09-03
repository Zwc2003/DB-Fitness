<!-- <template>
  <div>
    <Comment :myName="myName" :myHeader="myHeader" :comments="comments" />
  </div>
</template>

<script>
import Comment from "../components/Comment.vue";
export default {
  components: {
    Comment,
  },
  data() {
    return {
      myName: "Lana Del Rey",
      myHeader:
        "https://ts1.cn.mm.bing.net/th/id/R-C.6de01afd0e169978aef940229ee2c1de?rik=nsSMkJyKNH2gXQ&riu=http%3a%2f%2fimg.touxiangwu.com%2fuploads%2fallimg%2f2022053119%2foygfaqbppgi.jpg&ehk=4cacxrzeaJv8mVIwBB3pMz17MHUdwML%2fHNn%2bl%2bmyPxQ%3d&risl=&pid=ImgRaw&r=0",
      comments: [
        {
          name: "Lana Del Rey",
          headImg:
            "https://tse2-mm.cn.bing.net/th/id/OIP-C.Erum9yreLBLbYTOMWWzGIwAAAA?rs=1&pid=ImgDetMain",
          comment: "我发布一张新专辑Norman Fucking Rockwell,大家快来听啊",
          time: "2019年9月16日 18:43",
          commentNum: 2,
          like: 15,
          inputShow: false,
          reply: [
            {
              from: "Taylor Swift",
              fromHeadImg:
                "https://ae01.alicdn.com/kf/H94c78935ffa64e7e977544d19ecebf06L.jpg",
              comment: "我很喜欢你的新专辑！！",
              time: "2019年9月16日 18:43",
              commentNum: 1,
              like: 15,
              inputShow: false,
            },
            {
              from: "Ariana Grande",
              fromHeadImg:
                "https://ae01.alicdn.com/kf/Hf6c0b4a7428b4edf866a9fbab75568e6U.jpg",
              comment: "别忘记宣传我们的合作单曲啊",
              time: "2019年9月16日 18:43",
              commentNum: 0,
              like: 5,
              inputShow: false,
            },
          ],
        },
        {
          name: "Taylor Swift",
          headImg:
            "https://ae01.alicdn.com/kf/H94c78935ffa64e7e977544d19ecebf06L.jpg",
          comment: "我发行了我的新专辑Lover",
          time: "2019年9月16日 18:43",
          commentNum: 1,
          like: 5,
          inputShow: false,
          reply: [
            {
              from: "Lana Del Rey",
              fromHeadImg:
                "https://tse2-mm.cn.bing.net/th/id/OIP-C.O1dw-_dDk7WZCI1XK7lXiQAAAA?w=210&h=210&c=7&r=0&o=5&pid=1.7",
              to: "Taylor Swift",
              comment: "新专辑和speak now 一样棒！",
              time: "2019年9月16日 18:43",
              commentNum: 25,
              like: 5,
              inputShow: false,
            },
          ],
        },
        {
          name: "Norman Fucking Rockwell",
          headImg:
            "https://tse2-mm.cn.bing.net/th/id/OIP-C.O1dw-_dDk7WZCI1XK7lXiQAAAA?w=210&h=210&c=7&r=0&o=5&pid=1.7",
          comment: "Plz buy Norman Fucking Rockwell on everywhere",
          time: "2019年9月16日 18:43",
          commentNum: 0,
          like: 5,
          inputShow: false,
          reply: [],
        },
      ],
    };
  },
};
</script> -->
<template>
  <span
    class="all-courses"
    :class="{ active: isActive }"
    @click="handleAllCoursesClick"
  >
    全部课程
  </span>
  <div class="search-container">
    <!-- <span
      :class="{ active: isAllCoursesActive }"
      @click="onAllCoursesClick"
      class="all-courses"
      >全部课程</span
    > -->
    <!-- 搜索框 -->
    <el-input
      v-model="searchKeyword"
      placeholder="请输入关键词"
      @input="onSearch"
      class="search-input"
      clearable
    >
      <template #prefix>
        <el-icon><Search /></el-icon>
      </template>
    </el-input>

    <!-- 筛选区域 -->
    <div class="filters">
      <!-- 课程类别 -->
      <el-dropdown
        @command="onCourseTypeChange"
        v-model:visible="courseTypeVisible"
        class="dropdown"
        hide-on-click="false"
      >
        <span class="el-dropdown-link">
          课程类别
          <el-icon><CaretBottom /></el-icon>
          <el-icon v-if="selectedCourseType"><SuccessFilled /></el-icon>
        </span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item
              v-for="type in courseTypes"
              :key="type"
              :command="type"
              :class="{ selected: selectedCourseType === type }"
            >
              {{ type }}
              <el-icon v-if="selectedCourseType === type"><Check /></el-icon>
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>

      <!-- 价格区间 -->
      <el-dropdown
        @command="onPriceRangeChange"
        v-model:visible="priceRangeVisible"
        class="dropdown"
        hide-on-click="false"
      >
        <span class="el-dropdown-link">
          价格区间
          <el-icon><CaretBottom /></el-icon>
          <el-icon v-if="selectedPriceRange"><SuccessFilled /></el-icon>
        </span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item
              v-for="range in priceRanges"
              :key="range.label"
              :command="range.label"
              :class="{ selected: selectedPriceRange === range.label }"
            >
              {{ range.label }}
              <el-icon v-if="selectedPriceRange === range.label"
                ><Check
              /></el-icon>
            </el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </div>
  </div>
</template>

<script>
import { ref } from "vue";
import {
  Search,
  CaretBottom,
  Check,
  SuccessFilled,
} from "@element-plus/icons-vue";

export default {
  components: {
    [Search.name]: Search,
    [CaretBottom.name]: CaretBottom,
    [Check.name]: Check,
    [SuccessFilled.name]: SuccessFilled,
  },
  setup() {
    const searchKeyword = ref("");
    const selectedCourseType = ref("");
    const selectedPriceRange = ref("");
    const isActive = ref(false);

    const handleAllCoursesClick = () => {
      isActive.value = !isActive.value;
      selectedCourseType.value = "";
      selectedPriceRange.value = "";
      console.log("全部课程按钮被点击了！");
      // 在这里添加你想要触发的其他逻辑
    };

    const courseTypes = ["高强度间歇", "低强度塑形", "儿童趣味课", "有氧训练"];
    const priceRanges = [
      { label: "0-40 活力币", min: 0, max: 40 },
      { label: "40-80 活力币", min: 40, max: 80 },
      { label: "80 以上 活力币", min: 80, max: null },
    ];

    const onSearch = () => {
      isActive.value = false;
      console.log("搜索关键词:", searchKeyword.value);
    };

    const onCourseTypeChange = (courseType) => {
      isActive.value = false;
      selectedCourseType.value = courseType;
      console.log("选中的课程类别:", courseType);
    };

    const onPriceRangeChange = (priceRange) => {
      isActive.value = false;
      selectedPriceRange.value = priceRange;
      const range = priceRanges.find((r) => r.label === priceRange);
      console.log("选中的价格区间:", range.min, range.max);
    };

    return {
      handleAllCoursesClick,
      isActive,
      searchKeyword,
      selectedCourseType,
      selectedPriceRange,
      courseTypes,
      priceRanges,
      onSearch,
      onCourseTypeChange,
      onPriceRangeChange,
    };
  },
};
</script>

<style scoped>
/* .all-courses {
  margin-left: 30px;
} */
.all-courses {
  cursor: pointer;
  color: black;
  text-decoration: none;
  margin-right: 100px;
  font-size: 1.2rem;
}

.all-courses.active {
  font-weight: bolder;
  color: red;
  position: relative;
}

.all-courses.active::after {
  content: "";
  position: absolute;
  left: 0;
  bottom: -5px;
  width: 100%;
  height: 2px;
  background-color: red;
}

.search-container span.active {
  color: red;
  border-bottom: 2px solid red;
}

.filters .el-dropdown-link {
  color: initial;
}

.filters .el-dropdown-menu .el-dropdown-item.selected {
  background-color: initial;
  color: initial;
}

.search-container {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.search-input {
  width: 300px;
}

.filters {
  display: flex;
  justify-content: space-between;
  gap: 20px;
}

.dropdown {
  display: flex;
  align-items: center;
}

.el-dropdown-link {
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 5px;
}

.selected {
  background-color: #f0f9eb;
  color: #67c23a;
}
</style>
