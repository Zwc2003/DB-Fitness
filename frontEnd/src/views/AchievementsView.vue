<template>
    <div class="container">
        <navigation-bar />
        <div class="achievements">
            <Achievements />
        </div>
        <CommonLayout />
    </div>
</template>

<style scoped>
.container {
    display: grid;
    grid-template-columns: 1fr;
    gap: 100px;
}



.achievements {
    grid-column: 1;
    box-shadow: 2px 2px 4px rgba(0, 0, 0, 0.1);
    padding: 10px;
    border-radius: 15px;
    background-color: #f7fbff;
    width: 50vw;
    height: 80vh;
    margin-top: 150vh;
    margin-bottom: 10vh;
    justify-content: center;
}
</style>

<script>
import Achievements from "../components/Achievements.vue";
import CommonLayout from "../components/CommonLayout.vue";

export default {
    components: {  Achievements,CommonLayout },
}
</script>