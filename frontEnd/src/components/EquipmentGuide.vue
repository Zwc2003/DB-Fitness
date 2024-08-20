<template>
    <div>
        <h2>健身指南</h2>
        <el-space size="50">
            <div class="guide">
                <el-row style="margin-top: 20px;" gutter="20">
                    <el-col :span="6" v-for="equipment in equipments" :key="equipment.name">
                        <el-card @click="goToEquipmentGuide(equipment.name)" class="equipment-card"
                            :body-style="{ padding: '10px' }">
                            <img :src="equipment.imageUrl" alt="equipment image" class="equipment-image"/>
                            <div>{{ equipment.name }}</div>
                        </el-card>
                    </el-col>
                </el-row>
            </div>
        </el-space>
    </div>
</template>

<script>
export default {
    name: "EquipmentGuide",
    data() {
        return {
            equipments: [
                { name: '瑜伽垫', instructions: '用于卧推、仰卧起坐等。用户可以平躺在瑜伽垫上，进行各种推举和腹部锻炼。', imageUrl: '../../logo.png' },
                { name: '杠铃', instructions: '用于各种推举、划船、深蹲和硬拉等动作。用户双手握住杠铃，按特定的动作轨迹进行锻炼。', imageUrl: '../../logo.png' },
                { name: '哑铃', instructions: '用于肩部推举、侧平举、弯举等。用户双手各握住一只哑铃，按动作要求进行举起和放下。', imageUrl: '../../logo.png' },
                { name: '斜板', instructions: '用于上斜卧推和哑铃飞鸟等。用户仰卧在斜板上，进行推举或其他动作。', imageUrl: '../../logo.png' },
                { name: '双杠', instructions: '用于双杠臂屈伸。用户双手握住双杠，身体悬空进行屈伸动作。', imageUrl: '../../logo.png' },
                { name: '杠杆（引体向上）', instructions: '用于引体向上。用户双手握住杠杆，进行向上拉起和缓慢放下的动作。', imageUrl: '../../logo.png' },
                { name: '高位拉杆', instructions: '用于高位拉下。用户坐在拉力器上，双手握住拉杆，向下拉至胸部然后缓慢放回。', imageUrl: '/images/lat_pulldown.png' },
                { name: '绳索拉力器', instructions: '用于面拉、绳索下拉等。用户双手握住绳索，按动作要求进行拉动。', imageUrl: '/images/cable_machine.png' },
                { name: '腿弯举机', instructions: '用于腿弯举。用户俯卧在机器上，双脚勾住器械进行腿部弯曲和伸直。', imageUrl: '/images/leg_curl_machine.png' },
                { name: '椭圆机', instructions: '用于有氧运动。用户双脚踏在踏板上，双手握住把手，进行上下交替的踏步运动。', imageUrl: '/images/elliptical.png' },
                { name: '爬楼机', instructions: '用于有氧运动。用户双脚交替踏在踏板上，模拟爬楼梯的运动。', imageUrl: '/images/stepper.png' },
                { name: '跑步机', instructions: '用于有氧运动。用户站在跑步机上，调节速度和坡度，进行跑步或快走。', imageUrl: '/images/treadmill.png' },
                { name: '单车机', instructions: '用于有氧运动。用户坐在单车机上，双脚踏在踏板上进行踩踏运动。', imageUrl: '/images/stationary_bike.png' },
                { name: '药球', instructions: '用于俄罗斯转体等。用户双手握住药球，按动作要求进行左右转动或其他锻炼动作。', imageUrl: '/images/medicine_ball.png' },
            ],
        };
    },
    methods: {
        goToEquipmentGuide(name) {
            const equipment = this.equipments.find(e => e.name === name);
            this.$router.push({ name: 'EquipmentDetails', params: { equipmentName: equipment.name, instructions: equipment.instructions } });
        },
    },
};
</script>

<style scoped>
.guide {
    display: flex;
    flex-direction: column;
    align-items: center;
}

.equipment-card {
    cursor: pointer;
    transition: transform 0.2s;
    margin-bottom: 20px;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    white-space: nowrap;
    padding: 10px;
    text-align: center;
}

.equipment-card:hover {
    transform: scale(1.05);
}

.equipment-image {
    width: 100%;
    height: auto;
    margin-bottom: 10px;
}
</style>
