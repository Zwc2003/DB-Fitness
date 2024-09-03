<template>
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
</script>
