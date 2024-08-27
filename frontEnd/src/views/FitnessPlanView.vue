<script setup lang="ts">

import { reactive, ref } from 'vue'
import type { ComponentSize, FormInstance, FormRules } from 'element-plus'
import type { Action } from 'element-plus'
import NavigationBar from "../components/NavigationBar.vue";
import TimeThread from "../components/TimeThread.vue";
const dialogFormVisible = ref(false)
const formLabelWidth = '110px'
interface RuleForm {
  height: number,
  weight: number,
  BMI: number,
  bodyFatRate:  number,
  Pushups:  number,
  Squats: number,
  Situps:  number,
  Pullup:  number,
  longDistance: number,
  goal: string,
}
const ruleFormRef = ref<FormInstance>()
const form = reactive<RuleForm>({
  height: null,
  weight: null,
  BMI: null,
  bodyFatRate:null,
  Pushups: null,
  Squats: null,
  Situps: null,
  Pullup: null,
  longDistance:null,
  goal:'',
})
const rules = reactive<FormRules<RuleForm>>({
  height: [
    { required: true, message: '请输入身高', trigger: 'blur' },
    { type: 'number', message: '请输入数字', trigger: 'blur' },
  ],
  weight: [
    { required: true, message: '请输入体重', trigger: 'blur' },
    { type: 'number', message: '请输入数字', trigger: 'blur' },
  ],
  BMI: [
    { required: true, message: '请输入BMI', trigger: 'blur' },
    { type: 'number', message: '请输入数字', trigger: 'blur' },
  ],
  bodyFatRate: [
    { required: true, message: '请输入体脂率', trigger: 'blur' },
    { type: 'number', message: '请输入数字', trigger: 'blur' },
  ],
  Pushups: [
    { required: true, message: '请输入俯卧撑个数', trigger: 'blur' },
    { type: 'integer', message: '请输入整数', trigger: 'blur' },
  ],
  Squats: [
    { required: true, message: '请输入深蹲个数', trigger: 'blur' },
    { type: 'integer', message: '请输入整数', trigger: 'blur' },
  ],
  Situps: [
    { required: true, message: '请输入仰卧起坐个数', trigger: 'blur' },
    { type: 'integer', message: '请输入整数', trigger: 'blur' },
  ],
  Pullup: [
    { required: true, message: '请输入引体向上个数', trigger: 'blur' },
    { type: 'integer', message: '请输入整数', trigger: 'blur' },
  ],
  longDistance: [
    { required: true, message: '请输入一千米时间', trigger: 'blur' },
    { type: 'integer', message: '请输入整数', trigger: 'blur' },
  ],
  goal: [
    { required: true, message: '请选择健身目标', trigger: 'blur' },
  ],

})
const submitForm = async (formEl: FormInstance | undefined) => {
  if (!formEl) return
  await formEl.validate((valid, fields) => {
    if (valid) {
      console.log('submit!')
    } else {
      console.log('error submit!', fields)
    }
  })
}
const activeName = ref(1);
</script>

<template>
  <NavigationBar/>
  <div>
  <el-button plain @click="dialogFormVisible = true" class="bot">
    填写你的体测表
  </el-button>
  </div>
  <el-dialog v-model="dialogFormVisible" title="体测信息" width="400">
    <el-form :model="form" :rules="rules"  ref="ruleFormRef">
      <el-form-item label="身高(cm)" :label-width="formLabelWidth" prop="height">
        <el-input v-model="form.height" autocomplete="off" />
      </el-form-item>
      <el-form-item label="体重(kg)" :label-width="formLabelWidth" prop="weight">
        <el-input v-model="form.weight" autocomplete="off" />
      </el-form-item>
      <el-form-item label="BMI" :label-width="formLabelWidth" prop="BMI">
        <el-input v-model="form.BMI" autocomplete="off" />
      </el-form-item>
      <el-form-item label="体脂率" :label-width="formLabelWidth" prop="bodyFatRate">
        <el-input v-model="form.bodyFatRate" autocomplete="off" />
      </el-form-item>
      <el-form-item label="俯卧撑个数" :label-width="formLabelWidth" prop="Pushups">
        <el-input v-model="form.Pushups" autocomplete="off" />
      </el-form-item>
      <el-form-item label="深蹲个数" :label-width="formLabelWidth" prop="Squats">
        <el-input v-model="form.Squats" autocomplete="off" />
      </el-form-item>
      <el-form-item label="仰卧起坐个数" :label-width="formLabelWidth" prop="Situps">
        <el-input v-model="form.Situps" autocomplete="off" />
      </el-form-item>
      <el-form-item label="引体向上个数" :label-width="formLabelWidth" prop="Pullup">
        <el-input v-model="form.Pullup" autocomplete="off" />
      </el-form-item>
      <el-form-item label="一千米时间(s)" :label-width="formLabelWidth" prop="longDistance">
        <el-input v-model="form.longDistance" autocomplete="off" />
      </el-form-item>
      <el-form-item label="健身目标" :label-width="formLabelWidth" prop="goal">
        <el-select v-model="form.goal" placeholder="Please select a target">
          <el-option label="减脂" value="loseWeight" />
          <el-option label="增肌" value="buildMuscle" />
          <el-option label="塑型" value="bodySculpting" />
        </el-select>
      </el-form-item>




    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="dialogFormVisible = false">Cancel</el-button>
        <el-button  type="primary" @click="submitForm(ruleFormRef)">
          Create
        </el-button>
      </div>
    </template>
  </el-dialog>


  <TimeThread></TimeThread>






</template>

<style>
.bot{
  position:absolute;
  left: 320px;
  top: 100px;
  width: 150px;
  height: 100px;
}

.title {
  font-size: 18px;
  font-weight: bold;
  color: #303133;
  padding-bottom: 17px;
  padding-top: 17px;
}
/* 自定义 el-collapse-item 的样式 */
.custom-collapse-item .el-collapse-item__header {
  font-size: 18px;
  font-weight: bold;
  color: #1f2d3d;
  display: flex;
  align-items: center;
  cursor: pointer;
  padding: 10px;
  border-bottom: 1px solid #ebeef5;
  transition: all 0.3s ease;
  background-color: transparent;
}

/* 为标题添加图标 */
.custom-collapse-item .el-collapse-item__header::before {
  content: '📅';
  margin-right: 10px;
}

/* 鼠标悬停时标题的样式 */
.custom-collapse-item .el-collapse-item__header:hover {
  background-color: #e6f7ff;
  color: #409eff;
}

</style>

