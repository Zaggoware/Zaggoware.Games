import { Dictionary } from 'vue-router/types/router';

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

export interface CardGameRules extends GameRules
{
    numberOfPacks: number;
    numberOfJokersPerPack: boolean;
    excludedCards: CardRank[];
}

export interface TurnBasedCardGameRules extends CardGameRules
{
    maxTurns: number;
}

export interface IHubEventModel
{
}

export class CrazyEightsGameRules implements TurnBasedCardGameRules
{
    allowColorChangerCardEnding: boolean;
    allowMandatoryDrawCardEnding: boolean;
    allowReversingCardEnding: boolean;
    allowSingleTurnStackingCardEnding: boolean;
    colorChangerCardStackingMode: CardStackingMode;
    colorChangerCards: CardRank[];
    colorChangerMode: ColorChangerMode;
    defaultStackingMode: CardStackingMode;
    excludedCards: [];
    eliminationMode: boolean;
    numberOfJokersPerPack: boolean;
    mandatoryDrawCards: Record<CardRank, number>;
    maxPlayers: number;
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

export class CrazyEightsPlayerInfo
{
    id: string = '';
    name: string = '';
    cardsInHand: number = 0;
}

export class CrazyEightsGameInfo
{
    currentTurnIndex = -1;
    currentPlayerId = '';
    paused = false;
    started = false;
    players: CrazyEightsPlayerInfo[] = [];
    cardsInHand: PlayingCard[] = [];
    spectators: GameConnection[] = [];
    rules: CrazyEightsGameRules;
    discardPile: PlayingCard[] = [];
    stockpileCount = 0;
}

export enum CrazyEightsGameEvents
{
    GameStarted,
    GameStopped,
    PlayerAdded,
    PlayerFinished,
    PlayerRemoved,
    SpectatorAdded,
    SpectatorRemoved,
    CardDrawn,
    CardPlayed,
    ColorChanged,
    SuitChanged,
    TurnBegan,
    TurnEnded,
}

export class CrazyEightsCardPlayedGame implements IHubEventModel
{
    player: CrazyEightsPlayerInfo;
    card: PlayingCard;
}

export class CrazyEightsColorChangedHubEventModel implements IHubEventModel
{
    player: CrazyEightsPlayerInfo;
    color: CardColor;
}

export class CrazyEightsSuitChangedHubEventModel implements IHubEventModel
{
    player: CrazyEightsPlayerInfo;
    suit: CardSuit;
}

export class MessageInfo
{
    id = '';
    name = '';
    message = '';
    dateTimeUtc: Date = new Date();
}