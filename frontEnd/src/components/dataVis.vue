<template>
    <div>
        <el-space :size="100" direction="vertical">
            <el-row :gutter="50">
                <el-space direction="horizontal" size="large">
                    <el-col>
                        <el-card style="width: 550px">
                            <div ref="monthChart" style="width: 500px; height: 300px;"></div>
                        </el-card>
                    </el-col>
                    <el-col>
                        <el-card style="width: 550px">
                            <div ref="weekChart" style="width: 500px; height: 300px;"></div>
                        </el-card>
                    </el-col>
                </el-space>
            </el-row>


            <el-row :gutter="50">
                <el-space direction="horizontal" size="large">
                    <el-col :span="8">
                        <el-card style="width: 550px">
                            <div ref="typeChart" style="width: 500px; height: 300px;"></div>
                        </el-card>
                    </el-col>
                    <el-col :span="8">
                        <el-card style="width: 550px">
                            <div class="stats" style="padding: 20px">
                                <p>累计锻炼时间：{{ totalDuration }} 小时</p>
                                <p>累计上课次数：{{ totalClasses }}</p>
                                <p>累计运动次数：{{ totalActivities }}</p>
                            </div>
                        </el-card>
                    </el-col>
                </el-space>

            </el-row>
        </el-space>
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
.stats {
    margin: 0;
    padding: 4px;
    font-size: 14px;
    border-color: red;
}

.test {
    border-color: red;
}
</style>