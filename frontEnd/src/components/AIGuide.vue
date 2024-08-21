<template>
  <NavigationBar />
  <div class="container">
    <div class="title">
      <span>健身动作指导</span>
    </div>
    <div class="content">
      <div v-if="!isAnalyzing">
        <el-steps :active="active" align-center class="steps">
          <el-step title="上传健身截图"></el-step>
          <el-step title="输入健身动作类型"></el-step>
          <el-step title="AI分析"></el-step>
        </el-steps>
        <div class="before-tracking">
          <div class="content-left">
            <div class="before-upload" v-if="!isUpload">
              <el-upload action="#" drag class="uploader" list-type="picture" :auto-upload="false"
                :show-file-list="false" :on-change="imgPreview">
                <div class="uploader-icon">
                  <el-icon icon="el-icon-upload"><upload/></el-icon>
                </div>
                <div class="uploader-text">请将图像拖到此处或点击上传</div>
              </el-upload>
            </div>
            <div class="after-upload" v-else>
              <img :src="screenShotUrl" alt="" class="upload-img" />
              <span class="actions">
                <!-- 放大 -->
                <span class="item">
                  <el-icon class="el-icon-zoom-in" @click="beforeImgDialogVisible = true"><ZoomIn/></el-icon>
                </span>
                <!-- 删除 -->
                <span class="item">
                  <el-icon class="el-icon-delete" @click="del"><Delete/></el-icon>
                </span>
              </span>
            </div>
          </div>
          <div class="content-right">
            <el-card v-if="active === 1" shadow="always" class="card">
              <div slot="header" class="clearfix">
                 <span class="upload-title">上传健身截图</span>
              </div>
              <div v-if="!isUpload" class="step1_before_upload">
                <div class="loading-icon">
                  <el-icon class="el-icon-camera-solid" @click="imgCapDialogVisible = true"><camera/></el-icon>
                </div>
                <p>未检测到图像上传，请先在 <b>左侧</b> 上传图像</p>
                <p>或点击
                  <el-icon class="el-icon-camera-solid"><camera/></el-icon>
                  进行拍照
                </p>
              </div>
              <div v-else>
                {{ active++ }}

              </div>
            </el-card>
            <el-card v-if="active === 2" shadow="always" class="card">
              <div slot="header" class="clearfix">
                <span class="upload-title">输入健身动作类型</span>
              </div>
              <el-form class="type-form">
                <el-form-item  label-width="190px" >
                  <el-input v-model="screenshotsCurrent.exerciseName"
                    style="width: 300px; font-size: 20px; color: #000 !important; font-weight: bold; margin-top: 5px">
                  </el-input>
                </el-form-item>
              </el-form>
              <div class="img-tip-step2">
                点击 <b>下一步</b> 开始AI分析
              </div>
              <!-- <el-button type="primary" round class="img-button" @click="active--">上一步</el-button> -->
              <el-button type="primary" round class="img-button" @click="step2Next">下一步</el-button>
            </el-card>
            <el-card v-if="active === 3" shadow="always" class="card step3">
              <div slot="header" class="clearfix">
                <span class="upload-title">AI分析</span>
              </div>
              <div class="step3">
                <div class="img-info-item">
                  {{ '文件名: ' + this.screenShot.name }}
                </div>
                <div class="img-info-item">
                  {{ '类型: ' + this.screenShot.type }}
                </div>
                <div class="img-info-item">
                  {{ '文件大小: ' + this.convertFileSize(this.screenShot.size) }}
                </div>
                <div class="img-info-item">
                  {{ '健身动作类型: ' + this.screenshotsCurrent.exerciseName }}
                </div>
              </div>
              <div class="img-info-step3">
                请确认无误后，点击 <b>AI分析</b> 进行分析
              </div>
              <el-button type="primary" round class="img-button" @click="active--">上一步</el-button>
              <el-button type="primary" round class="img-button" @click="uploadImg">AI分析</el-button>
            </el-card>
          </div>
        </div>
      </div>
      <div class="after-tracking" v-else>
        <div class="content-left">
          <div class="after-success-tracking">
            <img :src="screenShotUrl" alt="" class="upload-img" />
            <span class="actions">
              <!-- 放大 -->
              <span class="item">
                <el-icon class="el-icon-zoom-in" @click="zoomin"><ZoomIn/></el-icon>
              </span>
            </span>
          </div>
        </div>
        <div class="content-right">
          <el-card shadow="always" class="card">
            <div slot="header" class="clearfix">
              <el-tag class="tag" v-if="analysisStatue === 0" type="success">分析成功</el-tag>
              <el-tag class="tag" v-if="analysisStatue === 1">AI分析中</el-tag>
              <el-tag class="tag" v-if="analysisStatue === 2" type="danger">分析失败</el-tag>
            </div>
            <div class="before-success-tracking" v-if="!successAnalyze">
              <div class="tracking" v-if="analysisStatue === 1">
                <div>
                  <el-progress class="progress" type="circle" :percentage="analysisPercentage" :width="300" />
                </div>
                <el-button type="primary" round class="cancel-btn" @click="cancelTrack">取消分析</el-button>
              </div>
              <div class="track-error" v-if="analysisStatue === 2">
                <div>
                  <el-progress class="progress" type="circle" :percentage="analysisPercentage" status="exception" />
                </div>
                <el-button type="primary" round class="cancel-btn" @click="tryAgain">重新分析</el-button>
              </div>
            </div>
            <!-- 在这里展示 后端返回的分析结果，Markdown 文本 -->
            <div class="after-success-tracking" v-else>
              <div class="markdown-content-container">
                <div class="markdown-content-res" v-html="markdownToHtml(markdownText)"></div>
              </div>
              <el-button type="primary" round class="img-info-finish" @click="retrain">重新分析</el-button>
            </div>
          </el-card>
        </div>
      </div>

    </div>
    <!-- 图片显示对话框 -->
    <el-dialog v-model="beforeImgDialogVisible" :modal-append-to-body="false" top="5vh" :show-close="false" class="dialog">
      <img :src="screenShotUrl" alt="" class="dialog-img"/>
    </el-dialog>

    <!--    拍照对话框-->
    <div v-show="imgCapDialogVisible" class="img-cap-dialog">
      <div class="img-cap-dialog-content">
        <p class="img-cap-dialog-content-title">拍照上传</p>
        <div v-show="!onCamara" class="before-open-camara">
          <el-form class="info-form">
            <el-form-item label="开启镜像:">
              <el-switch v-model="mirror" active-color="#262626">
              </el-switch>
            </el-form-item>
          </el-form>
          <el-button type="info" class="setting-btn" @click="imgCapDialogVisible = false">取消拍照</el-button>
          <el-button type="info" class="setting-btn" @click="openCamara">开启摄像头</el-button>
        </div>
        <div v-show="onCamara" class="after-open-camara">
          <div class="media-container">
            <video id="tracking-video" autoplay v-show="!trackFrame" class="cap-img" />
            <img :src="trackFrame" alt="" v-show="trackFrame" class="cap-img" />
          </div>
          <div class="setting-btn-group">
            <!--            <el-button type="info" class="setting-btn" @click="imgCapDialogVisible = false">取消拍照</el-button>-->
            <el-button type="info" class="setting-btn" @click="stopCamara">关闭摄像头</el-button>
            <el-button type="info" class="setting-btn" v-show="!trackFrame" @click="captureImage">拍照</el-button>
            <el-button type="info" class="setting-btn" v-show="trackFrame" @click="reCapture">重新拍照</el-button>
            <el-button type="info" class="setting-btn" v-show="trackFrame" @click="useCapture">使用图片</el-button>
          </div>
        </div>
      </div>
    </div>

    <!-- 搜索栏 -->
    <el-input v-model="searchQuery" placeholder="输入健身动作名称" class="search-input" clearable>
      <!-- 前缀图标 -->
      <template #prefix>
        <el-icon><Search /></el-icon>
      </template>

      <!-- 后缀图标 -->
      <template #suffix>
        <el-icon @click="clearSearch"><Close /></el-icon>
      </template>
    </el-input>


    <!-- 显示上传的图片 -->
    <div v-if="uploadedScreenshots.length" class="screenshot-gallery">
      <el-row :gutter="20">
        <el-col :span="6" v-for="(screenshot, index) in filteredScreenshots" :key="index">
          <el-card @click.native="openDialog(screenshot)"
            :body-style="{ padding: '10px', position: 'relative', height: '400px' }">
            <!-- 删除按钮 -->
            <el-button @click.stop="deleteScreenshot(screenshot)" type="danger"  circle
              style="position: absolute; bottom: 10px; right: 10px;z-index: 10">
              <el-icon class="el-icon-delete"><Delete/></el-icon>
            </el-button>
            <img :src="screenshot.screenshotUrl" alt="Screenshot" class="uploaded-image"
              style="max-height: 380px;height:90%; object-fit: contain; display: block; margin-bottom: 20px;" />
            <div style="text-align: center;">{{ screenshot.exerciseName }}</div>
          </el-card>
        </el-col>
      </el-row>
    </div>

    <!-- 模态窗口展示详情 -->
    <el-dialog v-model="dialogVisible" width="60%">
      <div class="title-container">
        <div class="markdown-content">
          <strong>运动分析详情</strong>
        </div>
      </div>
      <hr class="divider">
      <div class="markdown-content">
        <strong>运动名称:</strong> <span v-html="markdownToHtml(screenshotForShow.exerciseName)"></span>
      </div>
      <hr class="divider">
      <div class="markdown-content">
        <strong>AI建议:</strong>
        <div class="markdown-content-container-sub">
          <span v-html="markdownToHtml(screenshotForShow.AIsuggestion)"></span>
        </div>
      </div>
      <hr class="divider">
      <p class="info">创建时间: {{ screenshotForShow.createTime }}</p>
      <hr class="divider">
      <img :src="screenshotForShow.screenshotUrl" alt="运动图片" class="dialog-image">
    </el-dialog>
  </div>
</template>

<script>
import ElementPlus from 'element-plus'
import axios from 'axios'
// import marked from 'marked'
// eslint-disable-next-line no-unused-vars
import MarkdownIt from 'markdown-it'
const md = new MarkdownIt()
import { Upload,ZoomIn,Delete,Camera,Picture,Clock  } from '@element-plus/icons-vue'

export default {
  name: 'FitnessGuide',
  data() {
    return {
      searchQuery: '',
      dialogVisible: false,
      active: 1,
      isUpload: false, // false
      screenShot: '', // 上传图片File
      screenShotUrl: '', // 上传图片url
      isAnalyzing: false, // 是否开始分析
      beforeImgDialogVisible: false,
      imgCapDialogVisible: false,
      successAnalyze: false, // false
      analysisStatue: 1, // 检测状态（0：成功，1：检测中，2：失败）
      analysisPercentage: 0, // 检测进度
      stopProgress: false, // 终止进度条
      deviceId: '', // 选中设备id
      videoContain: null, // 摄像头视图容器
      mirror: false, // 摄像头镜像
      onCamara: false, // 是否打开摄像头
      videoStream: null, // 摄像头视频流
      trackFrame: '', // 跟踪完成的帧，
      uploadedBase64: '',
      // space
      userID: 0, // 暂时固定的用户ID
      uploadForm: {
        userID: 0,
        exerciseName: '',
        screenshot: null
      },
      screenshotsCurrent:
      {
        exerciseName: '',
        screenshotID: 0,
        screenshotUrl: '',
        AIsuggestion: '',
        createTime: ''
      },
      screenshotForShow:
      {
        exerciseName: '',
        screenshotID: 0,
        screenshotUrl: '',
        AIsuggestion: '',
        createTime: ''
      },
      uploadedScreenshots: [],
      markdownText: '# 标题'
    }
  },
  created() {
    this.getScreenshotFromDB(this.userID)
  },
  computed: {
    filteredScreenshots() {
      if (!this.searchQuery) {
        return this.uploadedScreenshots
      }
      return this.uploadedScreenshots.filter(screenshot => {
        return screenshot.exerciseName.toLowerCase().includes(this.searchQuery.toLowerCase())
      })
    }
  },
  watch: {
    mirror(isMirror) {
      if (isMirror) {
        this.videoContain.style.transform = 'rotateY(180deg)'
      } else {
        this.videoContain.style.transform = 'rotateY(0)'
      }
    }
  },
  mounted() {
    this.videoContain = document.getElementById('tracking-video')
  },
  beforeDestroy() {
    if (this.$store.state.cancelAxios.cancelAxios !== null) {
      this.$store.state.cancelAxios.cancelAxios()
      this.$store.dispatch('delReqUrl', true)
    }
  },
  components: {
    Upload,
    ZoomIn,
    Delete,
    Camera,
    Picture,
    Clock,
  },
  methods: {
    zoomin() {
    console.log('Icon clicked');
    this.beforeImgDialogVisible = true;
  },
    // 图片缩略图
    imgPreview(file) {
      console.log('file is', file)
      if (file.raw.type.split('/')[0] === 'image') {
        this.screenShot = file.raw
        this.screenShotUrl = URL.createObjectURL(file.raw)
        this.isUpload = true
        // 添加读取文件为 Base64 的代码
        const reader = new FileReader()
        reader.readAsDataURL(file.raw)
        reader.onload = () => {
          this.screenshotsCurrent.screenshotUrl = reader.result
          console.log('Base64 string:', this.screenshotsCurrent.screenshotUrl) // 这里可以查看 Base64 字符串
        }
      } else {
        this.$message({
          type: 'warning',
          message: '请上传正确的图像格式'
        })
      }
    },
    // 删除图片
    del() {
      this.$confirm('此操作将删除该图像, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning',
      }).then(() => {
        this.screenShotUrl = ''
        this.isUpload = false
        this.active = 1
        this.$message({
          type: 'success',
          message: '删除成功'
        })
      })
    },
    // 上传图片
    async uploadImg() {
      this.isAnalyzing = true
      this.analysisStatue = 1
      this.analysisPercentage = 0
      if (this.screenShotUrl === '') {
        this.$message.error('请先上传图片或检查图片是否上传成功')
        return
      }
      console.log('开始分析')
      const requestData = {
        userID: this.userID,
        exerciseName: this.screenshotsCurrent.exerciseName,
        screenshotUrl: this.screenshotsCurrent.screenshotUrl
      }
      // 生成随机的更新间隔，例如1到5秒之间
      let randomTimeout = Math.floor(Math.random() * 250) + 50
      this.refreshProgress(randomTimeout)
      axios.post('http://localhost:5273/api/AIGuide/Create', requestData)
        .then(response => {
          this.screenshotsCurrent.screenshotID = response.data.screenshotID
          this.screenshotsCurrent.createTime = new Date(response.data.createTime)
          this.screenshotsCurrent.screenshotUrl = response.data.screenshotUrl
          console.log(response.data.message)
          setTimeout(() => {
            this.getAISuggestions(response.data.screenshotID)
          }, 15000)
        }).catch(error => {
          console.error('Error creating record:', error)
        })

      // 这里写API请求
    },
    getAISuggestions(screenshotID) {
      console.log('获取AI建议 for screenshotID:', screenshotID)
      axios.get(`http://localhost:5273/api/AIGuide/GetAISuggestion/`, {
        params: {
          screenshotID: screenshotID
        }
      })
        .then(response => {
          this.markdownText = response.data.message
          this.analysisPercentage = 100
          this.successAnalyze = true
          this.analysisStatue = 0
          console.log('AI建议:', response.data)
          this.screenshotsCurrent.AIsuggestion = response.data.message
          // eslint-disable-next-line no-undef,no-unused-vars
          const screenshot = {
            exerciseName: this.screenshotsCurrent.exerciseName,
            screenshotID: this.screenshotsCurrent.screenshotID,
            screenshotUrl: this.screenshotsCurrent.screenshotUrl,
            AIsuggestion: this.screenshotsCurrent.AIsuggestion,
            createTime: this.screenshotsCurrent.createTime
          }
          console.log('imgUrl:', this.screenshotsCurrent.screenshotUrl)
          // eslint-disable-next-line no-undef
          this.uploadedScreenshots.push(screenshot)
        })
        .catch(error => {
          console.error('Error getting AI suggestions:', error)
        })
    },
    getScreenshotFromDB(userID) {
      axios.get('http://localhost:5273/api/AIGuide/GetAllDetails', { userID: userID })
        .then(response => {
          console.log(response.data.suggestions)
          response.data.suggestions.forEach(item => {
            const screenshot = {
              exerciseName: item.exerciseName,
              screenshotID: item.screenshotID,
              screenshotUrl: item.screenshotUrl,
              AIsuggestion: item.suggestions,
              createTime: item.createTime
            }
            this.uploadedScreenshots.push(screenshot)
          })
        })
    },
    deleteScreenshot(screenshot) {
      // 查找并删除匹配项
      axios.delete(`http://localhost:5273/api/AIGuide/Delete`, {
        params: {
          screenshotID: screenshot.screenshotID
        }
      })
        .then(response => {
          console.log(response.data.message)

          // 从列表中移除该项
          this.uploadedScreenshots = this.uploadedScreenshots.filter(item => item !== screenshot)
        })
        .catch(error => {
          console.error('删除失败', error)
        })
    },
    markdownToHtml(text) {
      return md.render(text)
    },
    // 重新检测
    retrain() {
      this.$confirm('重新分析将清空本次分析结果, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.isUpload = false
        this.isAnalyzing = false
        this.successAnalyze = false
        this.screenShotUrl = ''
        this.active = 1
        this.$message({
          type: 'success',
          message: '操作成功'
        })
      })
    },
    // 再试一次
    tryAgain() {
      this.isAnalyzing = false
      this.successAnalyze = false
      this.active = 3
    },
    // 开启摄像头
    openCamara() {
      this.$confirm('即将开启摄像头，是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'info'
      }).then(() => {
        this.onCamara = true
        const constraints = {
          video: {
            deviceId: this.deviceId ? this.deviceId : undefined,
            width: { min: 640, ideal: 1280, max: 1920 },
            height: { min: 480, ideal: 720, max: 1080 }
            // width: 256,
            // height: 144
          }
        }
        navigator.mediaDevices.getUserMedia(constraints).then(stream => {
          this.videoContain.srcObject = stream
          this.videoStream = stream
        })
      })
    },
    // 拍照
    captureImage() {
      const canvas = document.createElement('canvas')
      // console.log(video.videoHeight)
      canvas.width = this.videoContain.videoWidth
      canvas.height = this.videoContain.videoHeight
      let ctx = canvas.getContext('2d')
      ctx.clearRect(0, 0, canvas.width, canvas.height)
      if (this.mirror) {
        ctx.translate(canvas.width, 0)
        ctx.scale(-1, 1)
      }
      ctx.drawImage(this.videoContain, 250, 35, 800, 650)
      canvas.toBlob(blob => {
        this.trackFrame = URL.createObjectURL(blob)
        this.screenShot = new File([blob], 'capture.png', { type: 'image/png' })
        this.screenShotUrl = this.trackFrame
      })
    },
    // 重新拍照
    reCapture() {
      this.trackFrame = ''
      this.screenShot = ''
    },
    // 关闭摄像头
    stopCamara() {
      this.videoStream.getTracks()[0].stop()
      this.videoStream = null
      this.videoContain.srcObject = null
      this.onCamara = false
    },
    // 使用照片
    useCapture() {
      this.screenShot = this.trackFrame
      this.trackFrame = ''
      this.imgCapDialogVisible = false
      this.isUpload = true
      this.videoStream.getTracks()[0].stop()
      this.videoStream = null
      this.videoContain.srcObject = null
      this.onCamara = false
    },
    // 文件大小类型换算
    convertFileSize(size) {
      if (size / 1024 > 1) {
        size /= 1024
        if (size / 1024 > 1) {
          size /= 1024
          if (size / 1024 > 1) {
            size /= 1024
            return size.toFixed(2) + ' GB'
          }
          return size.toFixed(2) + ' MB'
        }
        return size.toFixed(2) + ' KB'
      }
      return size.toFixed(2) + ' B'
    },
    // 判断检测类型是否为空
    step2Next() {
      this.active++
    },
    // 进度条更新
    refreshProgress(timeout) {
      let interval = setInterval(() => {
        this.analysisPercentage++
        if (this.analysisPercentage >= 99 || this.stopProgress) {
          this.stopProgress = false
          clearInterval(interval)
        }
      }, timeout)
    },
    // 取消分析
    cancelTrack() {
      this.$confirm('确认取消分析?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.isAnalyzing = false
        this.active = 3
        this.$store.state.cancelAxios.cancelAxios()
        this.$store.dispatch('delReqUrl', true)
      })
    },
    // 在这里实现函数
    handleFileChange(event) {
      this.uploadForm.screenshot = event.target.files[0]
    },
    openDialog(screenshot) {
      console.log('hello', screenshot) // 添加这一行来调试
      this.screenshotForShow = {
        exerciseName: screenshot.exerciseName,
        screenshotUrl: screenshot.screenshotUrl,
        AIsuggestion: screenshot.AIsuggestion,
        createTime: screenshot.createTime
      }
      this.dialogVisible = true
    }
  }
}
</script>

<style scoped>
@import "./src/static/css/AIExercise.css";

.title {
  font-size: 3vw;
  color: #5485c2;
}

.title span {
  letter-spacing: 1vw;
}

.container  {
  background: url("./src/assets/bg5.jpg") no-repeat center;
  background-size: 100% 100%;
  margin-top: 5%;
  margin-left: -13%;
  width: 100%;


}

.img-cap {
  border: none;
  background-color: rgba(0, 0, 0, 0);
  font-size: 2.3vh;
  color: rgb(90, 141, 251);
}

.img-cap:hover {
  font-weight: bold;
  color: rgb(73, 108, 214);
}

.img-cap-dialog {
  background-color: rgba(0, 0, 0, .6);
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100vh;
  z-index: 1000;
  float: top;
}

.img-cap-dialog-content {
  margin: 5vh auto;
  width: 80%;
  height: 74vh;
  background-color: rgba(255, 255, 255, .8);
  border-radius: 25px;
  padding: 8vh 3%;
}

.img-cap-dialog-content-title {
  font-size: 3.5vh;
}

.before-open-camara .info-form {
  margin: 10vh 15vw;
}

.before-open-camara .info-form>>>.el-form-item {
  margin: 5vh 5vw;
}

.before-open-camara .info-form>>>.el-form-item__label {
  font-size: 2.3vh;
}

.before-open-camara .info-form>>>.el-form-item__content {
  text-align: left;
}

.before-open-camara .info-form>>>.el-form-item {
  margin-bottom: 0;
}

.before-open-camara .setting-btn {
  width: 25%;
  margin: 3vh 2vw;
  font-size: 2vh;
  line-height: 3vh;
}

.media-container {
  margin: 5vh auto;
  width: 75vw;
  height: 60vh;
  border-radius: 25px;
  background-color: rgba(62, 61, 61, 0.2);
}

.cap-img {
  width: 100%;
  height: 100%;
  border-radius: 25px;
}

.after-open-camara .setting-btn {
  width: 20%;
}

.actions {
  position: absolute;
  border-radius: 25px;
  width: 100%;
  height: 100%;
  line-height: 50vh;
  left: 0;
  top: 0;
  cursor: default;
  text-align: center;
  color: #fff;
  opacity: 0;
  font-size: 40px;
  background-color: rgba(0, 0, 0, .5);
  transition: opacity .3s;
}

.actions:hover {
  opacity: 1;
}

.actions:hover span {
  display: inline-block;
}

.actions span {
  display: none;
  margin: 0 10%;
  cursor: pointer;
  font-size: 6vh;
}

.uploader-icon {
  font-size: 15px ;
  color: #8c8c8c;
}

.uploader-text {
  margin-top: 5px !important;
  font-size: 45px !important;
  color: #8c8c8c;
}

.title-container {
  display: flex;
  justify-content: center;
  /* 水平居中 */
  align-items: center;
  /* 垂直居中 */
  width: 100%;
  margin-left: -3%;
}

.screenshot-gallery {
  margin-top: 20px;
  width: 80%;
  margin-left: 10%;
}

.uploaded-image {
  width: 100%;
  height: auto;
  display: block;
  margin-bottom: 20px;
  transition: transform 0.3s ease-in-out;
}

.uploaded-image:hover {
  transform: scale(1.1);
}

/* 自定义模态窗口背景颜色 */
.el-dialog__wrapper {
  background-color: rgba(0, 0, 0, 0.8);
  /* 设置背景颜色更深 */
}

.dialog-image {
  width: 100%;
  max-width: 50%;
  display: block;
  margin: 0 auto;
}

.info {
  color: black;
  font-size: 1.5em;
}

.el-dialog__body p,
.el-dialog__body div {
  color: black;
  font-size: 1.5em;
}

.markdown-content-container {
  max-height: 400px;
  /* 设置容器的最大高度 */
  overflow-y: auto;
  /* 启用垂直滚动条 */
  padding: 5px;
  /* 添加一些内边距 */
  border: 1px solid #e0e0e0;
  /* 添加边框以区分区域 */
  margin-bottom: 5px;
  /* 确保与按钮有一定间距 */
}

.markdown-content-container-sub {
  max-height: 200px;
  /* 设置容器的最大高度 */
  overflow-y: auto;
  /* 启用垂直滚动条 */
  padding-left: 5%;
  /* 添加一些内边距 */
  padding-right: 5%;
  text-align: left;
  font-size: 1em !important;
}

.markdown-content {
  color: black;
  font-size: 1.5em !important;
  text-align: left;
  /* 确保文本左对齐 */
  margin-bottom: 10px;
  margin-left: 5%;

}

.markdown-content-res {
  color: black;
  font-size: 1em !important;
  text-align: left;
  /* 确保文本左对齐 */
  margin-bottom: 10px;
  margin-left: 5%;
}

.divider {
  border: 0;
  height: 1px;
  background: #e0e0e0;
  margin: 20px 0;
}

.dialog-image {
  width: 100%;
  max-width: 40%;
  display: block;
  margin: 0 auto;
}

.search-input {
  width: 400px;
  height: 40px;
  margin-left: -60%;
  border-radius: 20px;
  font-size: 20px;
  background-color: #929191;
}
</style>
