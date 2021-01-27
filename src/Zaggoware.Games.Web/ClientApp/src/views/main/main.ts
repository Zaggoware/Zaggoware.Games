import { Component, Mixins } from 'vue-property-decorator';
import { BaseMixin } from '@/mixins/base-mixin';
import { CrazyEightsGameHub } from '@/hubs/games/crazy-eights-game-hub';
import {
    CardColor,
    CardRank,
    CardSuit,
    ColorChangerMode, CrazyEightsGameInfo,
    CrazyEightsGameRules,
    CrazyEightsPlayer,
    GameConnection,
    MessageInfo,
    PlayingCard
} from '@/models/custom';
import chatHub from '@/hubs/chat-hub';

@Component
export default class Main extends Mixins(BaseMixin)
{
    hub: CrazyEightsGameHub;
    connectionId = '';
    userName = '';
    paused = false;
    canConnect = false;
    started = false;
    connected = false;
    currentPlayer: CrazyEightsPlayer = null;
    currentPlayerId = '';
    currentTurnIndex = -1;
    cardsInHand: PlayingCard[] = [];
    rules: CrazyEightsGameRules = new CrazyEightsGameRules();
    players: CrazyEightsPlayer[] = [];
    playersSetup = false;
    spectators: GameConnection[] = [];
    discardPile: PlayingCard[] = [];
    stockpileCount = 0;
    chatMessages: MessageInfo[] = [];
    newMessage = '';
    nextColor?: CardColor = null;
    nextSuit?: CardSuit = null;
    canPickColorOrSuit = false;
    pickColorOrSuitCards: PlayingCard[] = [];

    get canStart(): boolean
    {
        return !this.started
            && this.players.length >= this.rules.minPlayers
            && this.players.length <= this.rules.maxPlayers;
    }

    get isCurrentTurn(): boolean
    {
        return this.currentPlayerId === this.connectionId;
    }

    get canPlayCard(): boolean
    {
        return this.started && this.isCurrentTurn;
    }

    get canDrawCard(): boolean
    {
        return this.started && this.isCurrentTurn;
    }

    get canEndTurn(): boolean
    {
        return this.started && this.isCurrentTurn;
    }

    get discardPileCard(): PlayingCard
    {
        return this.discardPile?.length > 0 ? this.discardPile[this.discardPile.length - 1] : null;
    }

    created(): void
    {
        this.hub = new CrazyEightsGameHub(this.$route.query.gameId as string ?? '1');
        this.hub
            .onUserConnected((evt) =>
            {
                console.log('A user connected.', evt);
            })
            .onUserDisconnected((evt) =>
            {
                console.log('A user disconnected.', evt);
            })
            .onPlayerAdded((evt) =>
            {
                console.log('A player was added.', evt);
                if (!this.players.find((p) => p.connection.id === evt.player.connection.id))
                {
                    this.players.push(evt.player);
                }
            })
            .onPlayerRemoved((evt) =>
            {
                console.log('A player was removed.', evt);

                const index = this.players.findIndex((p) => p.connection.id === evt.player.connection.id);
                if (index >= 0)
                {
                    this.players.splice(index, 1);
                }
            })
            .onSpectatorAdded((evt) =>
            {
                console.log('A spectator was added.', evt);
                if (!this.spectators.find((s) => s.id === evt.connection.id))
                {
                    this.spectators.push(evt.connection);
                }
            })
            .onSpectatorRemoved((evt) =>
            {
                console.log('A spectator was removed.', evt);

                const index = this.spectators.findIndex((s: GameConnection) => s.id === evt.connection.id);
                if (index >= 0)
                {
                    this.spectators.splice(index, 1);
                }
            })
            .onGameStarted(async (evt) =>
            {
                console.log('Game started.', evt);

                this.onUpdateGameInfo(evt.gameInfo);

                await this.fetchPlayerHand();
            })
            .onGameStopped((evt) =>
            {
                console.log('Game stopped.', evt);

                this.started = false;
                this.onUpdateGameInfo(evt.gameInfo);
            })
            .onTurnBegan(async (evt) =>
            {
                console.log('Turn', evt.turnIndex, 'began for', evt.player, 'at', evt.turnStartedDateTimeUtc);
                await this.fetchGameInfo();
                await this.fetchPlayerHand();
            })
            .onTurnEnded(async (evt) =>
            {
                console.log('Turn', evt.turnIndex, 'ended for', evt.player);
                await this.fetchGameInfo();
                await this.fetchPlayerHand();
            })
            .onCardPlayed(async (evt) =>
            {
                console.log('A card was played:', evt.card, 'by:', evt.player);

                this.nextColor = null;
                this.nextSuit = null;

                this.discardPile.push(evt.card);
                await this.fetchGameInfo();
                await this.fetchPlayerHand();
            })
            .onCardDrawn(async (evt) =>
            {
                console.log('A card was drawn by:', evt.player);

                this.stockpileCount--;
                await this.fetchGameInfo();
                await this.fetchPlayerHand();
            })
            .onDiscardColorChanged((evt) =>
            {
                console.log('The next color has been changed to:', evt.color);
                this.nextColor = evt.color;
                this.nextSuit = null;

                // TODO: Let the player visually know what the next color is.
            })
            .onDiscardSuitChanged((evt) =>
            {
                console.log('The next suit has been changed to:', evt.suit);
                this.nextColor = null;
                this.nextSuit = evt.suit;
                this.showNextSuit();
            })
            .onPlayerFinished((evt) =>
            {
                alert(`${evt.player.name} emptied their hand.`);
            });

        const userName = window.localStorage.getItem('userName');
        if (!!userName)
        {
            this.userName = userName;
            this.connect();
        }
        else
        {
            this.canConnect = true;
        }
    }

    connect(): Promise<void>
    {
        this.canConnect = false;

        console.log('Starting GameHub...');

        return this.hub.connect(this.userName)
            .then(async () =>
            {
                console.log('GameHub started.');

                this.$nextTick(() => this.connected = true);

                this.connectionId = this.hub.connection.connectionId;
                await this.connectChat();

                console.log('Awaiting user connection to game...');
                await this.fetchGameInfo();
            }).catch((error) =>
            {
                this.connected = false;
                this.canConnect = true;
                return console.error(error.toString());
            });
    }

    async connectChat(): Promise<void>
    {
        console.log('Starting ChatHub...');

        chatHub.on('MessageReceived', (message: MessageInfo) =>
        {
            console.log('Chat message received:', message);
            this.chatMessages.push(message);

            this.$nextTick(() =>
            {
                const $el = this.$refs['chat-messages'] as unknown;
                if (!!$el)
                {
                    $el['scrollTop'] = $el['scrollHeight'];
                }
            });
        });

        chatHub.start()
            .then(async () =>
            {
                console.log('ChatHub started.');
                console.log('Fetching chat messages...');

                chatHub.invoke('FetchMessages').then((messages: MessageInfo[]) =>
                {
                    console.log('Fetched chat messages:', messages);
                    this.chatMessages = messages;

                    this.$nextTick(() =>
                    {
                        const $el = this.$refs['chat-messages'] as unknown;
                        if (!!$el)
                        {
                            $el['scrollTop'] = $el['scrollHeight'];
                        }
                    });
                });
            }).catch((error) =>
            {
                return console.error(error.toString());
            });
    }

    async beforeDestroy(): Promise<void>
    {
        await this.hub.disconnect();
    }

    generateUserName(): string
    {
        let result = '';
        const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        const charactersLength = characters.length;
        for (let i = 0; i < 8; i++)
        {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }

        return result;
    }

    getColorFromSuit(suit: CardSuit): CardColor
    {
        let color: CardColor;
        switch (suit)
        {
            case CardSuit.Clubs:
            case CardSuit.Spades:
                color = CardColor.Black;
                break;

            case CardSuit.Diamonds:
            case CardSuit.Hearts:
                color = CardColor.Red;
                break;

            case CardSuit.Any:
                color = CardColor.Any;
                break;

            default:
                color = CardColor.Unknown;
                break;
        }

        return color;
    }

    async fetchGameInfo(): Promise<void>
    {
        console.log('Fetching game info...');

        const gameInfo = await this.hub.fetchGameInfo();

        console.log('Fetched game info.', gameInfo);

        this.onUpdateGameInfo(gameInfo);
    }

    async fetchPlayerHand(): Promise<void>
    {
        console.log('Fetching player hand...');

        this.cardsInHand = await this.hub.fetchPlayerHand() || [];

        console.log('Fetched player hand.', this.cardsInHand);
    }

    onUpdateGameInfo(gameInfo: CrazyEightsGameInfo): void
    {
        this.players = gameInfo.players;
        this.spectators = gameInfo.spectators;
        this.started = gameInfo.isStarted;
        this.rules = gameInfo.rules;
        this.discardPile = gameInfo.discardPile || [];
        this.stockpileCount = gameInfo.stockpileCount;
        this.currentTurnIndex = gameInfo.turnIndex;
        this.currentPlayerId = !!gameInfo.currentPlayer ? gameInfo.currentPlayer.connection.id : null;
        this.currentPlayer = gameInfo.currentPlayer;

        if (gameInfo.isStarted && !this.playersSetup)
        {
            // TODO: setup table.
            this.playersSetup = true;
        }

        this.showNextSuit();
    }

    showNextSuit(): void
    {
        if (this.nextSuit === null || !this.discardPile)
        {
            return;
        }

        const lastCardRank = this.discardPile[this.discardPile.length - 1].rank;
        if (this.rules.colorChangerCards.includes(lastCardRank))
        {
            this.discardPile.splice(this.discardPile.length - 1, 1);

            const color = this.getColorFromSuit(this.nextSuit);
            this.discardPile.push({ suit: this.nextSuit, color: color, rank: lastCardRank });
        }
    }

    submitConnectForm(evt: Event): boolean
    {
        evt.preventDefault();

        if (!this.userName || this.userName.match(/^\s+$/))
        {
            this.userName = this.generateUserName();
        }

        window.localStorage.setItem('userName', this.userName);
        this.connect();

        return false;
    }

    async startGame(): Promise<void>
    {
        if (this.started)
        {
            return;
        }

        this.started = await this.hub.startGame();
    }

    formatCard(card: PlayingCard): string
    {
        return this.$translate('labels.cardFormat',
            [
                this.$translate(`labels.cardRank${card.rank}`),
                this.$translate(`labels.cardSuit${card.suit}`)
            ]);
    }

    async playCard(suit: CardSuit, rank: CardRank): Promise<void>
    {
        console.log('Play Card', suit, rank);
        if (!this.canPlayCard)
        {
            return;
        }

        const cardIndex = this.cardsInHand.findIndex((c: PlayingCard) => c.suit === suit && c.rank === rank);
        if (cardIndex < 0)
        {
            // TODO: feedback to user.
            console.log('Cannot play card.');
            return;
        }

        const success = await this.hub.playCard(cardIndex, suit, rank);
        if (!success)
        {
            // TODO: feedback to user.
            console.log('Could not play card.');
            return;
        }

        if (this.rules.colorChangerCards.includes(rank))
        {
            this.pickColorOrSuitCards = [
                { suit: CardSuit.Hearts, rank: CardRank.Jack, color: CardColor.Red },
                { suit: CardSuit.Diamonds, rank: CardRank.Jack, color: CardColor.Red },
                { suit: CardSuit.Clubs, rank: CardRank.Jack, color: CardColor.Black },
                { suit: CardSuit.Spades, rank: CardRank.Jack, color: CardColor.Black }
            ];
            this.canPickColorOrSuit = true;
        }
    }

    isCurrentPlayer(player: CrazyEightsPlayer): boolean
    {
        return this.currentPlayerId === player.connection.id;
    }

    async drawCard(): Promise<void>
    {
        if (!this.canDrawCard)
        {
            return;
        }

        const card = await this.hub.drawCard();
        if (!!card)
        {
            this.cardsInHand.push(card);
        }
    }

    async endTurn(): Promise<void>
    {
        if (!this.canEndTurn)
        {
            return;
        }

        await this.hub.endTurn();
    }

    async pickColorOrSuit(suit: CardSuit): Promise<void>
    {
        if (!this.canPickColorOrSuit)
        {
            return;
        }

        let result = false;
        if (this.rules.colorChangerMode === ColorChangerMode.Color)
        {
            const color = this.getColorFromSuit(suit);
            result = await this.hub.changeDiscardColor(color);
        }
        else
        {
            result = await this.hub.changeDiscardSuit(suit);
        }

        if (!!result)
        {
            this.canPickColorOrSuit = false;
            await this.endTurn();
        }
    }

    sendChatMessage(evt: Event): boolean
    {
        evt.preventDefault();
        if (!this.newMessage || !chatHub.connectionId)
        {
            return false;
        }

        console.log('Sending chat message:', this.newMessage);

        chatHub.send('SendMessage', this.userName, this.newMessage).then(() =>
        {
            const messageInfo = new MessageInfo();
            messageInfo.dateTimeUtc = new Date();
            messageInfo.id = Date.now().toString(); // TODO: fix
            messageInfo.name = this.userName;
            messageInfo.message = this.newMessage;
            this.chatMessages.push(messageInfo);

            this.newMessage = '';

            this.$nextTick(() =>
            {
                const $el = this.$refs['chat-messages'] as unknown;
                $el['scrollTop'] = $el['scrollHeight'];
            });
        });

        return false;
    }
}
