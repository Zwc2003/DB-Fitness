<template>
    <div class="container">
        <div class="navigation-bar">
            <NavigationBar />
        </div>
        <div class="meal-planner">
            <MealPlanner />
        </div>
    </div>
</template>

<style scoped>
.container {
    display: grid;
    grid-template-columns: 1fr;
    gap: 100px;
}

.navigation-bar {
    grid-column: 1;
}

.meal-planner {
    grid-column: 1;
    box-shadow: 2px 2px 4px rgba(0, 0, 0, 0.1);
    padding: 10px;
    border-radius: 15px;
    background-color: #f7fbff;
}
</style>

<script>
import NavigationBar from "../components/NavigationBar.vue";
import MealPlanner from "../components/MealPlanner.vue";

export default {
    components: { NavigationBar, MealPlanner },
}
</script>