import { Component, Mixins } from 'vue-property-decorator';
import { BaseMixin } from '@/mixins/base-mixin';
import { CrazyEightsGameHubConnection } from '@/hub-connections/games/crazy-eights-game-hub-connection';
import {
    CardColor,
    CardRank,
    CardSuit,
    ColorChangerMode,
    CrazyEightsColorChangedHubEventModel,
    CrazyEightsGameRules,
    CrazyEightsPlayerInfo,
    CrazyEightsSuitChangedHubEventModel,
    GameConnection,
    MessageInfo,
    PlayingCard
} from '@/models/custom';
import chatHub from '@/hub-connections/chat-hub-connection';

@Component
export default class Main extends Mixins(BaseMixin)
{
    hubConnection: CrazyEightsGameHubConnection;
    connectionId = '';
    name = '';
    paused = false;
    canConnect = false;
    started = false;
    connected = false;
    currentPlayer: CrazyEightsPlayerInfo = null;
    currentPlayerId = '';
    currentTurnIndex = -1;
    cardsInHand: PlayingCard[] = [];
    rules: CrazyEightsGameRules = new CrazyEightsGameRules();
    players: CrazyEightsPlayerInfo[] = [];
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
        return this.isCurrentTurn;
    }

    get canDrawCard(): boolean
    {
        return this.isCurrentTurn;
    }

    get canEndTurn(): boolean
    {
        return this.isCurrentTurn;
    }

    get discardPileCard(): PlayingCard
    {
        return this.discardPile.length > 0 ? this.discardPile[this.discardPile.length - 1] : null;
    }

    created(): void
    {
        this.hubConnection = new CrazyEightsGameHubConnection();
        this.hubConnection.hubConnection.on('UserConnected', (user: GameConnection) =>
        {
            console.log('A user connected:', user);
        });

        this.hubConnection.hubConnection.on('UserDisconnected', (user: GameConnection) =>
        {
            console.log('A user disconnected:', user);
        });

        this.hubConnection.hubConnection.on('PlayerAdded', (player: CrazyEightsPlayerInfo) =>
        {
            console.log('A player was added:', player);
            this.players.push(player);
        });

        this.hubConnection.hubConnection.on('PlayerRemoved', (player: CrazyEightsPlayerInfo) =>
        {
            console.log('A player was removed:', player);

            const index = this.players.findIndex((p) => p.connectionId === player.connectionId);
            if (index >= 0)
            {
                this.players.splice(index, 1);
            }
        });

        this.hubConnection.hubConnection.on('SpectatorAdded', (spectator: GameConnection) =>
        {
            console.log('A spectator was added:', spectator);
            this.spectators.push(spectator);
        });

        this.hubConnection.hubConnection.on('SpectatorRemoved', (spectator: GameConnection) =>
        {
            console.log('A spectator was removed:', spectator);

            const index = this.spectators.findIndex((s: GameConnection) => s.id === spectator.id);
            if (index >= 0)
            {
                this.spectators.splice(index, 1);
            }
        });

        this.hubConnection.hubConnection.on('GameStarted', async () =>
        {
            console.log('Game started.');
            await this.fetchGameInfo();
        });

        this.hubConnection.hubConnection.on('GameStopped', async () =>
        {
            console.log('Game stopped.');
            await this.fetchGameInfo();
            alert('The game has been stopped.');
        });

        this.hubConnection.hubConnection.on('TurnBegan', async (player: CrazyEightsPlayerInfo, turnIndex: number, turnStartDateTimeUtc?: Date) =>
        {
            console.log('A turn began for:', player, turnIndex, turnStartDateTimeUtc);
            await this.fetchGameInfo();
        });

        this.hubConnection.hubConnection.on('TurnEnded', async (player: CrazyEightsPlayerInfo, turnIndex: number, turnStartDateTimeUtc: Date) =>
        {
            console.log('A turn was ended by:', player, turnIndex, turnStartDateTimeUtc);
            await this.fetchGameInfo();
        });

        this.hubConnection.hubConnection.on('CardPlayed', async (player: CrazyEightsPlayerInfo, card: PlayingCard) =>
        {
            console.log('A card was played:', card, 'by:', player);

            this.nextColor = null;
            this.nextSuit = null;

            this.discardPile.push(card);
            await this.fetchGameInfo();
        });

        this.hubConnection.hubConnection.on('CardDrawn', async (player: CrazyEightsPlayerInfo) =>
        {
            console.log('A card was drawn by:', player);

            this.stockpileCount--;
            if (this.stockpileCount <= 0)
            {
                await this.fetchGameInfo();
            }
        });

        this.hubConnection.onColorChanged((model: CrazyEightsColorChangedHubEventModel) =>
        {
            console.log('The next color has been changed to:', model.color);
            this.nextColor = model.color;
            this.nextSuit = null;

            // TODO: Let the player visually know what the next color is.
        });

        this.hubConnection.onSuitChanged((model: CrazyEightsSuitChangedHubEventModel) =>
        {
            console.log('The next suit has been changed to:', model.suit);
            this.nextColor = null;
            this.nextSuit = model.suit;
            this.showNextSuit();
        });

        this.hubConnection.hubConnection.on('PlayerFinished', (player: CrazyEightsPlayerInfo) =>
        {
            alert(`${player.name} emptied their hand.`);
        });

        console.log('Starting GameHub...');

        this.hubConnection.hubConnection.start()
            .then(async () =>
            {
                console.log('GameHub started.');

                this.connectionId = this.hubConnection.hubConnection.connectionId;
                this.name = window.localStorage.getItem('user') ?? '';
                if (this.name)
                {
                    console.log('Welcome back,', this.name);
                    await this.connectUser();
                }
                else
                {
                    this.canConnect = true;
                }

                console.log('Awaiting user connection to game...');
                await this.fetchGameInfo();
            }).catch((error) =>
            {
                return console.error(error.toString());
            });
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

        const game = await this.hubConnection.fetchGameInfo();

        console.log('Fetched game info:', game);

        this.$nextTick(() =>
        {
            this.players = game.players;
            this.spectators = game.spectators;
            this.started = game.started;
            this.rules = game.rules;
            this.discardPile = game.discardPile;
            this.stockpileCount = game.stockpileCount;
            this.currentTurnIndex = game.currentTurnIndex;
            this.currentPlayerId = game.currentPlayerId;
            this.currentPlayer = !!game.currentPlayerId
                ? game.players.find((p: CrazyEightsPlayerInfo) => p.connectionId === game.currentPlayerId)
                : null;
            this.cardsInHand = game.cardsInHand;

            if (game.started && !this.playersSetup)
            {
                // TODO: setup table.
                this.playersSetup = true;
            }

            this.showNextSuit();
        });
    }

    showNextSuit(): void
    {
        if (this.nextSuit === null)
        {
            return;
        }

        const lastCardRank = this.discardPile[this.discardPile.length - 1].rank;
        if (this.rules.colorChangerCards.includes(lastCardRank))
        {
            this.discardPile.splice(this.discardPile.length - 1, 1);

            const color = this.getColorFromSuit(this.nextSuit);
            this.discardPile.push({ suit: this.nextSuit!, color: color, rank: lastCardRank });
        }
    }

    submitName(evt: Event): boolean
    {
        evt.preventDefault();
        this.connectUser();
        return false;
    }

    async connectUser(): Promise<void>
    {
        console.log('Connecting to the game and chat...');

        chatHub.on('MessageReceived', (message: MessageInfo) =>
        {
            console.log('Chat message received:', message);
            this.chatMessages.push(message);
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
                        $el['scrollTop'] = $el['scrollHeight'];
                    });
                });
            }).catch((error) =>
            {
                return console.error(error.toString());
            });

        window.localStorage.setItem('user', this.name);

        await this.hubConnection.connectUser(this.name);

        console.log('Connected to the game.');

        this.connected = true;
    }

    async startGame(): Promise<void>
    {
        if (this.started)
        {
            return;
        }

        this.started = await this.hubConnection.startGame();
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

        const success = await this.hubConnection.playCard(cardIndex, suit, rank);
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

    isCurrentPlayer(player: CrazyEightsPlayerInfo): boolean
    {
        return this.currentPlayerId === player.connectionId;
    }

    async drawCard(): Promise<void>
    {
        if (!this.canDrawCard)
        {
            return;
        }

        const card = await this.hubConnection.drawCard();
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

        await this.hubConnection.endTurn();
    }

    async pickColorOrSuit(suit: CardSuit, rank: CardRank): Promise<void>
    {
        if (!this.canPickColorOrSuit)
        {
            return;
        }

        let result: boolean = false;
        if (this.rules.colorChangerMode === ColorChangerMode.Color)
        {
            const color = this.getColorFromSuit(suit);
            result = await this.hubConnection.changeColor(color);
        }
        else
        {
            result = await this.hubConnection.changeSuit(suit);
        }

        if (!!result)
        {
            this.canPickColorOrSuit = false;
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

        chatHub.send('SendMessage', this.name, this.newMessage).then(() =>
        {
            const messageInfo = new MessageInfo();
            messageInfo.dateTimeUtc = new Date();
            messageInfo.id = Date.now().toString(); // TODO: fix
            messageInfo.name = this.name;
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
