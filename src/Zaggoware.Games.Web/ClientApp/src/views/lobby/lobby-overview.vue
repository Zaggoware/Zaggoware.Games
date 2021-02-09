<template>
    <div class="game__lobbies">
        <h1>Lobbies</h1>
        <ul class="game__lobby-list">
            <li v-for="lobby in lobbies"
                :key="lobby.id"
                class="game__lobby-list-item">
                {{ lobby.name }} ({{ lobby.activeConnections }} players)
                <router-link to="/game/crazy-eights/1">
                    Join
                </router-link>
            </li>
        </ul>
    </div>
</template>

<script src="./lobby-overview.ts" lang="ts"></script>
