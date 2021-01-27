export interface CardDeck extends Array<PlayingCard>
{
    count: number;
    faceUp: boolean;
    shuffle(): void;
}

export enum CardRank
{
    None = 0,
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Joker = 14,
}

export enum CardSuit
{
    Any = 0,
    Clubs = 1,
    Diamonds = 2,
    Hearts = 3,
    Spades = 4
}

export enum CardColor
{
    Unknown = -1,
    Any = 0,
    Red = 1,
    Black = 2,
}

export enum CardStackingMode
{
    Disabled = 0,
    SameRank = 1,
    SameSuit = 2,
    SameColor = 4,
    All = SameRank | SameSuit | SameColor
}

export enum SpecialCardStackingMode
{
    Disabled,
    SameType,
    Allow,
}

export interface PlayingCard
{
    suit: CardSuit;
    rank: CardRank;
    color: CardColor;
}

export enum ColorChangerMode
{
    Color,
    Suit,
}

export interface GameConnection
{
    id: string;
    name: string;
}

export interface Player
{
    connection: GameConnection;
    name: string;
}

export interface GameRules
{
    minPlayers: number;
    maxPlayers: number;
}

export interface TurnBasedGameRules extends CardGameRules
{
    canChangeTurnDirection: boolean;
    maxTurns: number;
    maxTurnDuration: string;
}

export interface CardGameRules extends GameRules
{
    numberOfPacks: number;
    numberOfJokersPerPack: boolean;
    excludedCards: CardRank[];
}

export interface TurnBasedCardGameRules extends CardGameRules, TurnBasedGameRules
{
}

export interface GameInfo<TGameRules extends GameRules, TPlayer extends Player>
{
    isStarted: boolean;
    isPaused: boolean;
    rules: TGameRules;
    players: TPlayer[];
}

export interface TurnBasedGameInfo<TGameRules extends TurnBasedGameRules, TPlayer extends Player>
    extends GameInfo<TGameRules, TPlayer>
{
    turnIndex: number;
    currentPlayer: TPlayer;
}

export enum GameHubActions
{
    ConnectUser,
    StartGame,
    FetchGameInfo,
    StopGame,
}

export enum TurnBasedGameHubActions
{
    BeginTurn,
    EndTurn
}

export interface IHubEvent
{
    eventName: string;
}

export enum GameHubEvents
{
    UserConnected,
    UserDisconnected,
    GameStarted,
    GameStopped,
    PlayerAdded,
    PlayerRemoved,
    SpectatorAdded,
    SpectatorRemoved
}

export interface GameStartedHubEvent<TGameInfo extends GameInfo<TGameRules, TPlayer>, TGameRules extends GameRules, TPlayer extends Player> extends IHubEvent
{
    gameInfo: TGameInfo;
}

export interface GameStoppedHubEvent<TGameInfo extends GameInfo<TGameRules, TPlayer>, TGameRules extends GameRules, TPlayer extends Player> extends IHubEvent
{
    gameInfo: TGameInfo;
}

export interface PlayerAddedHubEvent<TPlayer extends Player> extends IHubEvent
{
    player: TPlayer;
}

export interface PlayerRemovedHubEvent<TPlayer extends Player> extends IHubEvent
{
    player: TPlayer;
}

export interface SpectatorAddedHubEvent extends IHubEvent
{
    connection: GameConnection;
}

export interface SpectatorRemovedHubEvent extends IHubEvent
{
    connection: GameConnection;
}

export interface UserConnectedHubEvent extends IHubEvent
{
    connection: GameConnection;
}

export interface UserDisconnectedHubEvent extends IHubEvent
{
    connection: GameConnection;
}

export enum TurnBasedGameHubEvents
{
    TurnBegan,
    TurnEnded
}

export interface GameTurnBeganHubEvent extends IHubEvent
{
    player: Player;
    turnIndex: number;
    turnStartedDateTimeUtc: Date;
}

export interface GameTurnEndedHubEvent extends IHubEvent
{
    player: Player;
    turnIndex: number;
    turnStartedDateTimeUtc: Date;
}

export enum CrazyEightsGameHubActions
{
    ChangeDiscardColor,
    ChangeDiscardSuit,
    DrawCard,
    FetchPlayerHand,
    PlayCard
}

export enum CrazyEightsGameHubEvents
{
    PlayerFinished,
    CardDrawn,
    CardPlayed,
    DiscardColorChanged,
    DiscardSuitChanged
}

export class CrazyEightsGameRules implements TurnBasedCardGameRules
{
    allowColorChangerCardEnding: boolean;
    allowMandatoryDrawCardEnding: boolean;
    allowReversingCardEnding: boolean;
    allowSingleTurnStackingCardEnding: boolean;
    canChangeTurnDirection: boolean;
    colorChangerCardStackingMode: CardStackingMode;
    colorChangerCards: CardRank[];
    colorChangerMode: ColorChangerMode;
    defaultStackingMode: CardStackingMode;
    excludedCards: [];
    eliminationMode: boolean;
    numberOfJokersPerPack: boolean;
    mandatoryDrawCards: Record<CardRank, number>;
    maxPlayers: number;
    maxTurnDuration: string;
    maxTurns: number;
    minPlayers: number;
    numberOfPacks: number;
    reversingCards: CardRank[];
    reversingCardStackingMode: SpecialCardStackingMode;
    startingCardsPerPlayer: number[];
    singleTurnStackingCards: CardRank[];
    skipNextTurnCards: CardRank[];
    skipNextTurnCardStackingMode: SpecialCardStackingMode;
}

export class CrazyEightsPlayer implements Player
{
    cardsInHand: number = 0;
    connection: GameConnection;
    name: string = '';
}

export class CrazyEightsGameInfo implements TurnBasedGameInfo<CrazyEightsGameRules, CrazyEightsPlayer>
{
    currentPlayer: CrazyEightsPlayer;
    turnIndex: number;
    isPaused = false;
    isStarted = false;
    players: CrazyEightsPlayer[] = [];
    spectators: GameConnection[] = [];
    rules: CrazyEightsGameRules;
    discardPile: PlayingCard[] = [];
    stockpileCount = 0;
}

export interface CrazyEightsCardDrawnHubEvent extends IHubEvent
{
    player: CrazyEightsPlayer;
}

export interface CrazyEightsCardPlayedHubEvent extends IHubEvent
{
    player: CrazyEightsPlayer;
    card: PlayingCard;
}

export interface CrazyEightsDiscardColorChangedHubEvent extends IHubEvent
{
    player: CrazyEightsPlayer;
    color: CardColor;
}

export interface CrazyEightsDiscardSuitChangedHubEvent extends IHubEvent
{
    player: CrazyEightsPlayer;
    suit: CardSuit;
}

export interface CrazyEightsPlayerFinishedHubEvent extends IHubEvent
{
    player: CrazyEightsPlayer;
}

export class MessageInfo
{
    id = '';
    name = '';
    message = '';
    dateTimeUtc: Date = new Date();
}