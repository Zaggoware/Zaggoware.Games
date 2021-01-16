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
                <h3>{{ $translateAndCapitalize('titles.players') }}</h3>
                <ul class="connection-list player-list">
                    <li v-for="player in players"
                        :key="player.connectionId"
                        class="connection-list__item player-list__player"
                        :class="{ 'player-list__player--current': isCurrentPlayer(player) }">
                        {{ player.name }}
                        <span v-if="player.cardsInHand > 0">({{ player.cardsInHand }} {{ $translate('labels.cards') }})</span>
                    </li>
                </ul>
            </div>

            <div class="game__spectators">
                <h3>{{ $translateAndCapitalize('titles.spectators') }}</h3>
                <ul class="connection-list spectator-list">
                    <li v-for="spectator in spectators"
                        :key="spectator.id"
                        class="connection-list__item">
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
                <h3>{{ $translateAndCapitalize('titles.chooseSuit') }}</h3>
                <div class="game__color-suit-chooser-container">
                    <playing-card v-for="card in pickColorOrSuitCards"
                                  :rank="card.rank"
                                  :suit="card.suit"
                                  :selectable="canPickColorOrSuit"
                                  @select="pickColorOrSuit">
                    </playing-card>
                </div>
            </div>

            <button v-if="canStart"
                    type="button"
                    class="button game__button game__button--start"
                    @click="startGame">
                {{ $translateAndCapitalize('buttons.startGame') }}
            </button>

            <div class="game__player-hand">
                <h3>{{ $translateAndCapitalize('titles.myCards') }}</h3>

                <ul class="playing-cards">
                    <li v-for="card in cardsInHand"
                        class="playing-cards__card-item">
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
            </div>

            <div class="chat">
                <h3>Chat:</h3>
                <div class="chat__container">
                    <div ref="chat-messages"
                         class="chat__messages">
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
</template>

<script src="./main.ts" lang="ts"></script>
<style src="./main.scss" lang="scss"></style>
