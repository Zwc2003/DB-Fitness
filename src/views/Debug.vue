<template>
  <div>
    <!-- 橙色按钮，带有白色粗加号 -->
    <el-button type="primary" @click="showModal = true" class="plus-button">
      <i class="el-icon-plus">
        <el-icon class="el-icon-my-circle-plus">
          <Plus />
        </el-icon>
      </i>
    </el-button>

    <!-- 弹出的表单 -->
    <el-dialog v-model="showModal" title="编辑课程" width="50%">
      <el-form :model="form">
        <el-form-item label="课程名称">
          <el-input v-model="form.name"></el-input>
        </el-form-item>
        <el-form-item label="进度">
          <el-input v-model="form.progress"></el-input>
        </el-form-item>
        <el-form-item label="时间">
          <el-input v-model="form.time"></el-input>
        </el-form-item>
      </el-form>
      <template #footer>
        <el-button @click="showModal = false">取消</el-button>
        <el-button type="primary" @click="saveCourse">保存</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, reactive } from "vue";

const showModal = ref(false);

const form = reactive({
  name: "",
  progress: "",
  time: "",
});

const teachCourses = ref([]);

function saveCourse() {
  // 将用户输入的课程信息保存到 teachCourses 数组中
  teachCourses.value.push({
    name: form.name,
    progress: form.progress,
    time: form.time,
  });

  // 清空表单
  form.name = "";
  form.progress = "";
  form.time = "";

  // 关闭弹窗
  showModal.value = false;
}
</script>

<style scoped>
.plus-button {
  background-color: orange;
  color: white;
  font-size: 24px;
  width: 100px;
  height: 50px;
  border-radius: 5px;
  display: flex;
  justify-content: center;
  align-items: center;
}

.plus-button .el-icon-plus {
  font-weight: bold;
}

.el-icon-my-circle-plus {
  font-weight: bold;
  font-size: larger;
}
</style>
