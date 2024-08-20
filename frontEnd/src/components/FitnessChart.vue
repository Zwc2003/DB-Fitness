<template>
    <div class="grid-container">
        <div class="grid-item" ref="monthChart" style="width: 100%; height: 100%;"></div>
        <div class="grid-item" ref="weekChart" style="width: 100%; height: 100%;"></div>
        <div class="grid-item" ref="typeChart" style="width: 100%; height: 100%;"></div>
        <div class="grid-item stats">
            <p>累计锻炼时间：{{ totalDuration }} 小时</p>
            <p>累计上课次数：{{ totalClasses }}</p>
            <p>累计运动次数：{{ totalActivities }}</p>
        </div>
    </div>
</template>

<script>
import * as echarts from 'echarts';

export default {
    name: 'FitnessChart',
    data() {
        return {
            monthChart: null,
            weekChart: null,
            typeChart: null,
            fitnessData: [5, 10, 8, 15, 12, 18, 10, 16, 14, 19, 20, 22],
            weekFitnessData: [3, 4, 5, 0, 2, 6, 1],
            typeData: [
                { value: 335, name: '跑步' },
                { value: 310, name: '瑜伽' },
                { value: 234, name: '举重' },
                { value: 135, name: '有氧' },
                { value: 1548, name: '自行车' }
            ],
            totalDuration: 120,
            totalClasses: 50,
            totalActivities: 180
        };
    },
    mounted() {
        this.monthChart = echarts.init(this.$refs.monthChart);
        this.weekChart = echarts.init(this.$refs.weekChart);
        this.typeChart = echarts.init(this.$refs.typeChart);
        this.initMonthChart();
        this.initWeekChart();
        this.initTypeChart();
    },
    methods: {
        initMonthChart() {
            const option = {
                title: {
                    text: '月度健身次数'
                },
                tooltip: {
                    trigger: 'axis'
                },
                xAxis: {
                    type: 'category',
                    data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月']
                },
                yAxis: {
                    type: 'value'
                },
                series: [{
                    data: this.fitnessData,
                    type: 'line',
                    smooth: true
                }]
            };
            this.monthChart.setOption(option);
        },
        initWeekChart() {
            const option = {
                title: {
                    text: '本周健身次数'
                },
                tooltip: {
                    trigger: 'axis'
                },
                xAxis: {
                    type: 'category',
                    data: ['星期日', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六']
                },
                yAxis: {
                    type: 'value'
                },
                series: [{
                    data: this.weekFitnessData,
                    type: 'line',
                    smooth: true
                }]
            };
            this.weekChart.setOption(option);
        },
        initTypeChart() {
            const option = {
                title: {
                    text: '健身类型分布'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: '{a} <br/>{b} : {c} ({d}%)'
                },
                series: [
                    {
                        name: '健身类型',
                        type: 'pie',
                        radius: '55%',
                        data: this.typeData,
                        emphasis: {
                            itemStyle: {
                                shadowBlur: 10,
                                shadowOffsetX: 0,
                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                            }
                        }
                    }
                ]
            };
            this.typeChart.setOption(option);
        }
    }
}
</script>

<style>
.grid-container {
    display: grid;
    grid-template-columns: 1fr 1fr;
    /* 两列，每列占一半宽度 */
    grid-template-rows: 1fr 1fr;
    /* 两行，每行占一半高度 */
    width: 100vw;
    /* 整个容器宽度为视口宽度的100% */
    height: 100vh;
    /* 整个容器高度为视口高度的100% */
    margin: 0;
    /* 去掉默认的边距 */
    overflow: hidden;
    /* 防止内容溢出 */
}

.grid-item {
    background-color: #f5f5f5;
    /* 每个网格的背景颜色 */
    display: flex;
    align-items: center;
    /* 内容垂直居中 */
    justify-content: center;
    /* 内容水平居中 */
    padding: 10px;
    /* 内边距 */
    box-sizing: border-box;
    /* 边框和内边距包含在宽度和高度内 */
}

.stats p {
    margin: 0;
    /* 移除默认的边距 */
    padding: 4px;
    /* 细微的内边距 */
    font-size: 14px;
    /* 文本大小 */
}
</style>