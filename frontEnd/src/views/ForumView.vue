<template>
    <div>
        <NavigationBar />
        <Forum />
        <CommonLayout />
    </div>
</template>


<script>
import NavigationBar from "../components/NavigationBar.vue";
import Forum from "../components/Forum.vue";
import CommonLayout from "../components/CommonLayout.vue";

export default {
    components: { NavigationBar, Forum,CommonLayout },
}
</script>