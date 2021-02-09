<template>
    <div class="game">
        <router-view></router-view>

        <div v-if="showMenuModal"
             class="game__modal">
            <div class="game__modal-container">
                <div class="game__modal-title">
                    <h3>{{ $translateAndCapitalize('titles.menu') }}</h3>
                </div>
                <div class="game__modal-content">
                    <ul class="game__modal-menu">
                        <li class="game__modal-menu-item">
                            <button class="game__button game__button--stretch"
                                    tabindex="1"
                                    @click="showMenuModal = false">
                                {{ $translateAndCapitalize('buttons.backToGame') }}
                            </button>
                        </li>
                        <li class="game__modal-menu-item">
                            <button class="game__button game__button--stretch"
                                    tabindex="2">
                                {{ $translateAndCapitalize('buttons.options') }}
                            </button>
                        </li>
                        <li class="game__modal-menu-item">
                            <button class="game__button game__button--stretch"
                                    tabindex="3">
                                {{ $translateAndCapitalize('buttons.help') }}
                            </button>
                        </li>
                        <li class="game__modal-menu-item">
                            <button class="game__button game__button--stretch"
                                    tabindex="4"
                                    @click="quitGame()">
                                {{ $translateAndCapitalize('buttons.quitGame') }}
                            </button>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</template>

<script src="./game.ts" lang="ts"></script>
<style src="./game.scss" lang="scss"></style>
