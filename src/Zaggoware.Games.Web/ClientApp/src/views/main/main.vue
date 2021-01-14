<template>
    <div class="game container">
        <form v-if="!connected && canConnect"
              @submit="submitName">
            <p>
                <label for="name">
                    {{ $translateAndCapitalize('labels.name') }}
                </label>

                <input id="name"
                       v-model="name"
                       type="text"
                       name="name" />
            </p>

            <button type="submit">
                {{ $translateAndCapitalize('buttons.connect') }}
            </button>
        </form>

        <div class="game__container">
            <div class="game__players">
                <h3>Players:</h3>
                <ul>
                    <li v-for="player in players"
                        :key="player.id">
                        {{ player.name }}
                        <span v-if="isCurrentPlayer(player)">(T)</span>
                    </li>
                </ul>
            </div>

            <div class="game__spectators">
                <h3>Spectators:</h3>
                <ul>
                    <li v-for="spectator in spectators"
                        :key="spectator.id">
                        {{ spectator.name }}
                    </li>
                </ul>
            </div>

            <div class="game__stockpile">
                <playing-card v-if="stockpileCount > 0"
                              :selectable="canDrawCard"
                              @select="drawCard">
                </playing-card>
            </div>

            <div class="game__discard-pile">
                <playing-card v-if="discardPileCard !== null"
                              :rank="discardPileCard.rank"
                              :suit="discardPileCard.suit">
                </playing-card>
            </div>

            <div v-if="canPickColorOrSuit"
                 class="game__color-suit-chooser">
                <playing-card v-for="card in pickColorOrSuitCards"
                              :rank="card.rank"
                              :suit="card.suit"
                              :selectable="canPickColorOrSuit"
                              @select="pickColorOrSuit">
                </playing-card>
            </div>

            <button v-if="canStart"
                    type="button"
                    class="button game__button game__button--start"
                    @click="startGame">
                {{ $translateAndCapitalize('buttons.startGame') }}
            </button>

            <div class="game__player-hand">
                <h3>My cards:</h3>

                <ul class="playing-cards">
                    <li v-for="card in cardsInHand"
                        class="playing-cards__card-item">
                        {{ formatCard(card) }}
                        <playing-card :rank="card.rank"
                                      :suit="card.suit"
                                      :selectable="canPlayCard"
                                      @select="playCard">
                        </playing-card>
                    </li>
                </ul>

                <button v-if="canEndTurn"
                        type="button"
                        class="button game__button game__button--end-turn"
                        @click="endTurn">
                    {{ $translateAndCapitalize('buttons.endTurn') }}
                </button>

                <div class="chat">
                    <h3>Chat:</h3>
                    <div class="chat__container">
                        <div class="chat__messages" ref="chat-messages">
                            <ul class="chat__messages-list">
                                <li v-for="message in chatMessages"
                                    :key="message.id"
                                    class="chat-message">
                                    <span class="chat-message__name">{{ message.name }}:</span>
                                    <span class="chat-message__message">{{ message.message }}</span>
                                </li>
                            </ul>
                        </div>
                        <div class="chat__new-message-container">
                            <form class="chat__new-message-form"
                                  @submit="sendChatMessage">
                                <input v-model="newMessage"
                                       type="text"
                                       name="message"
                                       autocomplete="off"
                                       class="chat__new-message" />
                                <button type="submit"
                                        class="button chat__button chat__button--new-message">
                                    {{ $translateAndCapitalize('buttons.send') }}
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script src="./main.ts" lang="ts"></script>
<style src="./main.scss" lang="scss"></style>
