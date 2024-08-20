<template>
  <el-space size="50">
    <div class="camera">
      <el-row style="margin-top: 20%;">
        <el-space :size="50">
          <video ref="video" autoplay></video>
          <canvas ref="canvas" style="display: none"></canvas>
          <el-card class="card">
            <el-space direction="vertical">
              <el-text class="text">Current Emotion:</el-text>
              <el-text class="text"> {{ emotion }}</el-text>
              <el-text class="text" style="margin-top: 100px">Suggestion:</el-text>
              <el-text class="text"> {{ emotionResult }}</el-text>
            </el-space>
          </el-card>
        </el-space>>
      </el-row>
      <el-row justify="center">
        <el-button @click="startCapturing" size="large" type="success" v-show="!this.capturing">Start
          Capturing</el-button>
        <el-button @click="stopCapturing" size="large" type="warning" v-show="this.capturing">Stop Capturing</el-button>
      </el-row>
    </div>
  </el-space>
</template>

<script>
import request from "../api/request.js";
import io from 'socket.io-client';
import { blobToBase64 } from "../api/blob2base64.js";

export default {
  name: "Camera",
  data() {
    return {
      capturing: false,
      captureInterval: null,
      socket: null,
      emotion: '',
      emotionResult: ''
    };
  },
  mounted() {
    this.initCamera();
    // this.socket = io('http://localhost:5001');
    // this.socket.on('video_stream', (data) => {
    //   this.updateVideoStream(data);
    // });
  },
  methods: {
    initCamera() {
      const video = this.$refs.video;
      if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        navigator.mediaDevices.getUserMedia({ video: true })
          .then(stream => {
            video.srcObject = stream;
            video.play();
          })
          .catch(error => {
            console.error("Error accessing the camera: ", error);
          });
      } else {
        console.error("getUserMedia not supported on your browser!");
      }
    },
    startCapturing() {
      if (!this.capturing) {
        this.capturing = true;
        this.captureInterval = setInterval(this.captureFrame, 10); // 每秒捕获一次
      }
    },
    stopCapturing() {
      this.capturing = false;
      clearInterval(this.captureInterval);
    },
    captureFrame() {
      const video = this.$refs.video;
      const canvas = this.$refs.canvas;
      const context = canvas.getContext('2d');

      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;
      context.drawImage(video, 0, 0, canvas.width, canvas.height);

      const dataURL = canvas.toDataURL('image/png');
      this.sendToBackend(dataURL).then(res => {
        console.log(res);
        this.emotion = res.data.emotion;
        this.emotionResult = res.data.emotion_result;
      });
    },
    sendToBackend(dataURL) {
      const blob = this.dataURLtoBlob(dataURL);
      const formData = new FormData();
      formData.append('file', blob, 'capture.png');

      return request('http://localhost:5001/predict', {
        headers: {
          'Content-Type': 'multipart/form-data'
        },
        method: 'POST',
        data: formData
      })
    },
    dataURLtoBlob(dataURL) {
      const binary = atob(dataURL.split(',')[1]);
      const array = [];
      for (let i = 0; i < binary.length; i++) {
        array.push(binary.charCodeAt(i));
      }
      return new Blob([new Uint8Array(array)], { type: 'image/png' });
    },
    updateVideoStream(data) {
      const video = this.$refs.video;
      const canvas = this.$refs.feedback;
      const context = canvas.getContext('2d');

      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;

      const blob = new Blob([data], { type: 'video/webm' });
      let dataUrl = URL.createObjectURL(blob);
      let image = new Image();
      image.onload = () => {
        context.drawImage(image, 0, 0, canvas.width, canvas.height);
      }
      image.src = dataUrl;
    },
  },
  beforeDestroy() {
    if (this.socket) {
      this.socket.disconnect();
    }
  },
};
</script>

<style scoped>
.camera {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.camera video,
.camera canvas {
  width: 100%;
  max-width: 600px;
  margin-bottom: 10px;
}

.text {
  font-size: 30px;
  font-weight: bold;
}

.card {
  width: 400px;
  height: 450px;
}
</style>
