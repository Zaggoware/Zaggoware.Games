<template>
    <div :class="classes" @click="selectCard">
        {{ formatLabel() }}
    </div>
</template>

<script src="./playing-card.ts" lang="ts"></script>
<style src="./playing-card.scss" lang="scss"></style>