<template>
    <div class="container">
        <NavigationBar />
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
    margin-top: 200vh;
    margin-bottom: -10vh;
    justify-content: center;
    background: url('../assets/images/bg5.jpg') no-repeat center center;
}
</style>

<script>
import NavigationBar from "../components/NavigationBar.vue";
import Achievements from "../components/Achievements.vue";
import CommonLayout from "../components/CommonLayout.vue";

export default {
    components: { NavigationBar, Achievements,CommonLayout },
}
</script>