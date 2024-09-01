<template>
  <div>
    <div class="square" @click="handleClick">
      <el-icon v-if="iconVisible">
        <Select />
      </el-icon>
    </div>
  </div>
</template>

<script>
import { ElIcon } from "element-plus";
import { Select } from "@element-plus/icons-vue";

export default {
  components: {
    ElIcon,
    Select,
  },
  props: {
    usercourses: {
      type: Array,
      required: true,
    },
    todaycourses: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      iconVisible: false,
    };
  },
  methods: {
    handleClick() {
      this.iconVisible = !this.iconVisible; // Toggle icon visibility

      // Update todaycourses array based on usercourses
      this.todaycourses.forEach((course) => {
        if (
          course.name === this.usercourses[0].courseName &&
          course.time === this.usercourses[0].classTime
        ) {
          course.attended = 1; // Mark as attended
        } else {
          course.attended = 0; // Mark as not attended
        }
      });
    },
  },
};
</script>

<style>
.square {
  width: 30px;
  height: 30px;
  border: 2px solid black;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
}

/* .el-icon {
  position: absolute;
  top: 10px;
  right: 10px;
} */
</style>
