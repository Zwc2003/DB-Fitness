import "./assets/main.css";

import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
//补充
import ElementPlus from "element-plus";
import "element-plus/dist/index.css";
import * as ElementPlusIconsVue from "@element-plus/icons-vue";
//不知道为什么store无法被import，说明stores/index.js写的是有问题的，日后要更正
//import store from "./store";
import ArcoVue from "@arco-design/web-vue";
import "@arco-design/web-vue/dist/arco.css";
import { calendarVue } from "calendar-vue3";
import "calendar-vue3/dist/style.css";

const app = createApp(App);

app.use(router);
//补充
app.use(ElementPlus);
//app.use(store);
app.use(ArcoVue);

app.mount("#app");

for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
  app.component(key, component);
}
