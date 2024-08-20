<template>
    <div class="head_con">
        <div class="logo-container">
            <img src="../assets/logo.png" alt="FitFit" class="logo">
        </div>
        <div class="wrapper">
            <nav>
                <input type="radio" name="tab" id="home" :checked="$route.path === '/'">
                <input type="radio" name="tab" id="comment" :checked="$route.path === '/Tweets'">
                <input type="radio" name="tab" id="message" :checked="$route.path === '/fit'">
                <input type="radio" name="tab" id="star" :checked="$route.path === '/camera'">
                <input type="radio" name="tab" id="user" :checked="$route.path === '/data'">
                <!--这一行是注释-->
                <label for="home" class="home" @click="delayedNavigation('/')">

                    <el-icon>
                        <House />
                    </el-icon>
                    首页
                </label>
                <label for="comment" class="comment" @click="delayedNavigation('/Tweets')">
                    <router-link to="/Tweets">
                        <el-icon>
                            <ChatLineRound />
                        </el-icon>
                        健身推文
                    </router-link>
                </label>
                <label for="message" class="message" @click="delayedNavigation('/fit')">
                    <router-link to="/fit">
                        <el-icon>
                            <Collection />
                        </el-icon>
                        健身课程
                    </router-link>
                </label>
                <label for="star" class="star" @click="delayedNavigation('/admin')">
                    <router-link to="/">
                        <el-icon>
                            <Star />
                        </el-icon>
                        管理员
                    </router-link>
                </label>
                <label for="user" class="user" @click="delayedNavigation('/data')">
                    <el-icon>
                        <DataAnalysis />
                    </el-icon>
                    数据统计
                </label>
                <div class="tab"></div>
            </nav>
        </div>
        <div class="avatar-container">
            <el-dropdown>
                <img src="../assets/user.jpeg" alt="User" class="dropdownlink">
                <template #dropdown>
                    <el-dropdown-menu>
                        <el-dropdown-item>
                            <el-icon>
                                <Setting />
                            </el-icon>
                            账号设置
                        </el-dropdown-item>
                        <el-dropdown-item>
                            <el-icon>
                                <Switch />
                            </el-icon>
                            切换账号
                        </el-dropdown-item>
                    </el-dropdown-menu>
                </template>
            </el-dropdown>
        </div>
    </div>
</template>

<style>
* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: flex-start;
    text-align: center;
    background: #fff;
}

.head_con {
    display: flex;
    align-items: center;
    position: absolute;
    top: 3%;
    left: 0;
    width: 100%;
    padding-left: 5%;
    padding-right: 3%;
}

.logo-container {
    margin-right: 20px;
}

.logo {
    height: 50px;
    display: block;
}

.wrapper,
.avatar-container {
    height: 50px;
    display: flex;
    align-items: center;
    margin-left: auto;
}

.wrapper {
    width: 60vw;
    line-height: 50px;
    background-color: white;
    box-shadow: 0px 5px 15px rgba(0, 0, 0, 0.25);
    border-radius: 50px;
}

.wrapper nav {
    display: flex;
    flex: 1;
    position: relative;
}

.wrapper nav label {
    flex: 1;
    width: 100%;
    position: relative;
    z-index: 1;
    cursor: pointer;
}

.wrapper nav label a {
    position: relative;
    z-index: -1;
    color: #333;
    font-size: 16px;
    font-weight: 500;
    text-decoration: none;
}

.wrapper nav input {
    display: none;
}

.wrapper nav .tab {
    position: absolute;
    height: 100%;
    width: 20%;
    left: 0px;
    bottom: 0px;
    background: linear-gradient(to right, #f09819, #ff5858);
    border-radius: 50px;
    transition: 0.6s cubic-bezier(0.68, -0.55, 0.265, 1.55);
}

.wrapper nav #home:checked~label.home a,
.wrapper nav #comment:checked~label.comment a,
.wrapper nav #message:checked~label.message a,
.wrapper nav #star:checked~label.star a,
.wrapper nav #user:checked~label.user a {
    color: #fff;
    transition: 0.6s;
}

.wrapper nav #comment:checked~.tab {
    left: 20%;
}

.wrapper nav #message:checked~.tab {
    left: 40%;
}

.wrapper nav #star:checked~.tab {
    left: 60%;
}

.wrapper nav #user:checked~.tab {
    left: 80%;
}

.avatar-container {
    position: relative;
    display: flex;
    align-items: center;
    height: 50px;
    border: none;
}

.dropdownlink {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    cursor: pointer;
    border: 5px solid #fff;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.20);
}

.example-showcase .el-dropdown-link {
    cursor: pointer;
    color: var(--el-color-primary);
    display: flex;
    align-items: center;
}

@media (min-width: 1024px) {
    #app {
        display: grid;
        grid-template-columns: 1fr;
        padding: 0 2rem;
    }
}
</style>

<script>

import router from "../router/index.js";

export default {
    name: "NavigationBar",

    methods: {
        router() {
            return router
        },
        delayedNavigation(target) {
            setTimeout(() => {
                this.router().push(target); // 替换为你的目标路由
            }, 500); // 1000 毫秒 = 1 秒
        }
    },
}
// class Circle {
//     constructor({ origin, speed, color, angle, context }) {
//         this.origin = origin
//         this.position = { ...this.origin }
//         this.color = color
//         this.speed = speed
//         this.angle = angle
//         this.context = context
//         this.renderCount = 0
//     }
//
//     draw() {
//         this.context.fillStyle = this.color
//         this.context.beginPath()
//         this.context.arc(this.position.x, this.position.y, 2, 0, Math.PI * 2)
//         this.context.fill()
//     }
//
//     move() {
//         this.position.x = (Math.sin(this.angle) * this.speed) + this.position.x
//         this.position.y = (Math.cos(this.angle) * this.speed) + this.position.y + (this.renderCount * 0.3)
//         this.renderCount++
//     }
// }
//
// class Boom {
//     constructor({ origin, context, circleCount = 10, area }) {
//         this.origin = origin
//         this.context = context
//         this.circleCount = circleCount
//         this.area = area
//         this.stop = false
//         this.circles = []
//     }
//
//     randomArray(range) {
//         const length = range.length
//         const randomIndex = Math.floor(length * Math.random())
//         return range[randomIndex]
//     }
//
//     randomColor() {
//         const range = ['8', '9', 'A', 'B', 'C', 'D', 'E', 'F']
//         return '#' + this.randomArray(range) + this.randomArray(range) + this.randomArray(range) + this.randomArray(range) + this.randomArray(range) + this.randomArray(range)
//     }
//
//     randomRange(start, end) {
//         return (end - start) * Math.random() + start
//     }
//
//     init() {
//         for (let i = 0; i < this.circleCount; i++) {
//             const circle = new Circle({
//                 context: this.context,
//                 origin: this.origin,
//                 color: this.randomColor(),
//                 angle: this.randomRange(Math.PI - 1, Math.PI + 1),
//                 speed: this.randomRange(1, 6)
//             })
//             this.circles.push(circle)
//         }
//     }
//
//     move() {
//         this.circles.forEach((circle, index) => {
//             if (circle.position.x > this.area.width || circle.position.y > this.area.height) {
//                 return this.circles.splice(index, 1)
//             }
//             circle.move()
//         })
//         if (this.circles.length == 0) {
//             this.stop = true
//         }
//     }
//
//     draw() {
//         this.circles.forEach(circle => circle.draw())
//     }
// }
//
// class CursorSpecialEffects {
//     constructor() {
//         this.computerCanvas = document.createElement('canvas')
//         this.renderCanvas = document.createElement('canvas')
//
//         this.computerContext = this.computerCanvas.getContext('2d')
//         this.renderContext = this.renderCanvas.getContext('2d')
//
//         this.globalWidth = window.innerWidth
//         this.globalHeight = window.innerHeight
//
//         this.booms = []
//         this.running = false
//     }
//
//     handleMouseDown(e) {
//         console.log(e.clientX,e.clientY)
//         const boom = new Boom({
//             origin: { x: e.clientX, y: e.clientY },
//             context: this.computerContext,
//             area: {
//                 width: this.globalWidth,
//                 height: this.globalHeight
//             }
//         })
//         boom.init()
//         this.booms.push(boom)
//         this.running || this.run()
//     }
//
//     handlePageHide() {
//         this.booms = []
//         this.running = false
//     }
//
//     init() {
//         const style = this.renderCanvas.style
//         style.position = 'fixed'
//         style.top = style.left = 0
//         style.zIndex = '999999999999999999999999999999999999999999'
//         style.pointerEvents = 'none'
//
//         style.width = this.renderCanvas.width = this.computerCanvas.width = this.globalWidth
//         style.height = this.renderCanvas.height = this.computerCanvas.height = this.globalHeight
//
//         document.body.append(this.renderCanvas)
//
//         window.addEventListener('mousedown', this.handleMouseDown.bind(this))
//         window.addEventListener('pagehide', this.handlePageHide.bind(this))
//     }
//
//     run() {
//         this.running = true
//         if (this.booms.length == 0) {
//             return this.running = false
//         }
//
//         requestAnimationFrame(this.run.bind(this))
//
//         this.computerContext.clearRect(0, 0, this.globalWidth, this.globalHeight)
//         this.renderContext.clearRect(0, 0, this.globalWidth, this.globalHeight)
//
//         this.booms.forEach((boom, index) => {
//             if (boom.stop) {
//                 return this.booms.splice(index, 1)
//             }
//             boom.move()
//             boom.draw()
//         })
//         this.renderContext.drawImage(this.computerCanvas, 0, 0, this.globalWidth, this.globalHeight)
//     }
// }
//
// const cursorSpecialEffects = new CursorSpecialEffects()
// cursorSpecialEffects.init()
//
// !(function () {
//     function n(n, e, t) {
//         return n.getAttribute(e) || t;
//     }
//     function e(n) {
//         return document.getElementsByTagName(n);
//     }
//     function t() {
//         var t = e("script"),
//             o = t.length,
//             i = t[o - 1];
//         return {
//             l: o,
//             z: n(i, "zIndex", -1),
//             o: n(i, "opacity", 0.6),
//             c: n(i, "color", "148,0,211"),
//             n: n(i, "count", 99),
//         };
//     }
//     function o() {
//         (a = m.width =
//             window.innerWidth ||
//             document.documentElement.clientWidth ||
//             document.body.clientWidth),
//             (c = m.height =
//                 window.innerHeight ||
//                 document.documentElement.clientHeight ||
//                 document.body.clientHeight);
//     }
//     function i() {
//         r.clearRect(0, 0, a, c);
//         var n, e, t, o, m, l;
//         s.forEach(function (i, x) {
//             for (
//                 i.x += i.xa,
//                 i.y += i.ya,
//                 i.xa *= i.x > a || i.x < 0 ? -1 : 1,
//                 i.ya *= i.y > c || i.y < 0 ? -1 : 1,
//                 r.fillRect(i.x - 0.5, i.y - 0.5, 1, 1),
//                 e = x + 1;
//                 e < u.length;
//                 e++
//             )
//                 (n = u[e]),
//                     null !== n.x &&
//                     null !== n.y &&
//                     ((o = i.x - n.x),
//                         (m = i.y - n.y),
//                         (l = o * o + m * m),
//                         l < n.max &&
//                         (n === y &&
//                             l >= n.max / 2 &&
//                             ((i.x -= 0.03 * o), (i.y -= 0.03 * m)),
//                             (t = (n.max - l) / n.max),
//                             r.beginPath(),
//                             (r.lineWidth = t / 2),
//                             (r.strokeStyle = "rgba(" + d.c + "," + (t + 0.2) + ")"),
//                             r.moveTo(i.x, i.y),
//                             r.lineTo(n.x, n.y),
//                             r.stroke()));
//         }),
//             x(i);
//     }
//     var a,
//         c,
//         u,
//         m = document.createElement("canvas"),
//         d = t(),
//         l = "c_n" + d.l,
//         r = m.getContext("2d"),
//         x =
//             window.requestAnimationFrame ||
//             window.webkitRequestAnimationFrame ||
//             window.mozRequestAnimationFrame ||
//             window.oRequestAnimationFrame ||
//             window.msRequestAnimationFrame ||
//             function (n) {
//                 window.setTimeout(n, 1e3 / 45);
//             },
//         w = Math.random,
//         y = { x: null, y: null, max: 2e4 };
//     (m.id = l),
//         (m.style.cssText =
//             "position:fixed;top:0;left:0;z-index:" + d.z + ";opacity:" + d.o),
//         e("body")[0].appendChild(m),
//         o(),
//         (window.onresize = o),
//         (window.onmousemove = function (n) {
//             (n = n || window.event), (y.x = n.clientX), (y.y = n.clientY);
//         }),
//         (window.onmouseout = function () {
//             (y.x = null), (y.y = null);
//         });
//     for (var s = [], f = 0; d.n > f; f++) {
//         var h = w() * a,
//             g = w() * c,
//             v = 2 * w() - 1,
//             p = 2 * w() - 1;
//         s.push({ x: h, y: g, xa: v, ya: p, max: 6e3 });
//     }
//     (u = s.concat([y])),
//         setTimeout(function () {
//             i();
//         }, 100);
// })();
</script>