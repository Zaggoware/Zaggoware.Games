<template>
    <div class="app">
        <component :is="'layout'"
                   class="app__content">
            <router-view></router-view>
        </component>
    </div>
</template>

<script lang="ts" src="./app.ts"></script>
<style lang="scss" src="./app.scss"></style>
