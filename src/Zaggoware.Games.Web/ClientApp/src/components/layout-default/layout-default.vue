<template>
    <main class="layout">
        <header class="layout__header">
            <h1 class="layout__header-title">{{ $translateAndCapitalize('titles.appName') }}</h1>
        </header>

        <div class="content">
            <slot />
        </div>
    </main>
</template>

<script src="./layout-default.ts" lang="ts"></script>
<style src="./layout-default.scss" lang="scss"></style>
