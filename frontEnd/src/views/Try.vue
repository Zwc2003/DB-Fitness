<template>
  <div class="search-bar">
    <!-- 搜索框 -->
    <el-input
      v-model="searchKey"
      placeholder="搜索健身课程"
      style="width: 300px"
      @keyup.enter="searchCourses"
    >
      <template #append>
        <el-button @click="searchCourses">搜索</el-button>
      </template>
    </el-input>

    <!-- 价格区间选择器 -->
    <el-select
      v-model="selectedPriceRange"
      placeholder="选择价格区间"
      style="margin-left: 16px; width: 180px"
      @change="updatePriceRange"
    >
      <el-option label="0-30活力币" :value="{ min: 0, max: 30 }"></el-option>
      <el-option label="30-60活力币" :value="{ min: 30, max: 60 }"></el-option>
      <el-option
        label="60-100活力币"
        :value="{ min: 60, max: 100 }"
      ></el-option>
    </el-select>
  </div>
</template>

<script>
export default {
  data() {
    return {
      searchKey: "", // 搜索关键词
      selectedPriceRange: null, // 选中的价格区间
    };
  },
  methods: {
    // 更新价格区间参数
    updatePriceRange(value) {
      this.minPrice = value.min;
      this.maxPrice = value.max;
      this.searchCourses();
    },
    // 搜索课程
    searchCourses() {
      const params = {
        key: this.searchKey,
        minprice: this.minPrice,
        maxprice: this.maxPrice,
      };
      // 调用后端API进行课程筛选
      console.log("筛选参数:", params);
      // this.fetchCourses(params); // 这里调用实际的API方法
    },
  },
};
</script>

<style scoped>
.search-bar {
  position: absolute;
  right: 5%;
  display: flex;
  align-items: center;
}
</style>
