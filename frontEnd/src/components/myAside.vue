<template>
    <div style="height: 550px; overflow-y: auto;">
      <el-row style="height:40px">
        <el-input  v-model="query"  placeholder="搜索好友"  @keyup.enter="search"  clearable>
          <template #append>
            <el-button @click="search">
              <template #icon>
                <Search />
              </template> 
            </el-button>
          </template>
        </el-input>
      </el-row>
      <el-row >
        <el-table @row-click="handleClick" :data="filteredtest" :show-header="false"  style="width: 100%;">
          <el-table-column > 
            <template v-slot="scope">
              <div style="display: flex; align-items: center;">
                
                <el-avatar :src="scope.row.img" />
                <span style="margin-left: 8px;">{{ scope.row.name }}</span>
              </div>
            </template>
          </el-table-column>
        </el-table>
      </el-row>
    </div>
  </template>
  

<script>
import { Search } from '@element-plus/icons-vue'
import { ElTable, ElTableColumn } from 'element-plus'
import store from "../store/index.js";
// import userImage from '@/assets/logo.png';

  export default {
    name: 'myAside',

    components: {
    Search,
    ElTable,
    ElTableColumn,
  },

    data() {
    return {
      query: '',
      // test: [
      //   {
      //     id:1,
      //     name: '同济',
      //     img: userImage
      //   },
      //   {
      //     id:2,
      //     name: '大学',
      //     img: userImage
      //   },
      //   {
      //     id:2,
      //     name: '大学',
      //     img: userImage
      //   },
      //   {
      //     id:3,
      //     name: '学院',
      //     img: userImage
      //   }
      // ]
    };
  },

    computed: {
      users(){
        return store.state.userListInformation;//用户列表的信息
      },
      filteredtest() {
        const query = this.query.toLowerCase();
        const filtered = this.users.filter(data => !query || data.name.toLowerCase().includes(query));
        console.log('Filtered data count:', filtered.length);
        return filtered;
      }
    },

    methods: {
      //补充添加好友逻辑：点击+后，浮窗搜索好友姓名，调用后端接口，会返回所有的相关用户列表，点击即代表添加好友（没有设置验证通过的逻辑），记得刷新当前好友列表
      search() {
        console.log('搜索内容:', this.query);
        // 搜索逻辑:  按名字搜索现在好友列表中的好友(前端即可实现)
      },
      handleClick(row) {
        console.log('Row clicked:', row);
        // 在这里处理点击事件
        store.commit('setTargetInformation', { id:row.id,img: row.img, name: row.name});
      }
  }
  }
</script>

<style>
.custom-content {
  width: 150px; /* 设置固定宽度 */
  max-width: 200px; /* 设置最大宽度 */
  padding: 10px; /* 设置内边距 */
  height:200px;
  /* position: relative; 使子元素能够相对于此容器进行绝对定位 */
  /* display: inline-block; 保持元素在同一行 */
}
.custom-avatar {
  transform: translate(-120%, 0%);
  width: 50px; /* el-avatar 的宽度 */
  height: 50px; /* el-avatar 的高度 */

}
.name-label {
  position: absolute; /* 绝对定位 */
  top: 20px; /* 距离顶部 0 */
  left: 60%; 
  transform: translate(-50%, -50%);
  white-space: nowrap; /* 防止文本换行 */
  font-size: 18px; /* 设置字体大小 */
}
.ID-label {
  position: absolute; /* 绝对定位 */
  top: 50px; /* 距离顶部 0 */
  left: 60%; 
  transform: translate(-50%, -50%);
  white-space: nowrap; 
  font-size: 10px; /* 设置字体大小 */
}
</style>
