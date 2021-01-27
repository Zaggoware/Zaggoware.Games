import {
    CardColor,
    CardRank,
    CardSuit,
    CrazyEightsCardDrawnHubEvent,
    CrazyEightsCardPlayedHubEvent,
    CrazyEightsDiscardColorChangedHubEvent,
    CrazyEightsDiscardSuitChangedHubEvent, CrazyEightsGameHubActions,
    CrazyEightsGameHubEvents,
    CrazyEightsGameInfo,
    CrazyEightsGameRules,
    CrazyEightsPlayer, CrazyEightsPlayerFinishedHubEvent,
    PlayingCard
} from '@/models/custom';
import { TurnBasedGameHub } from '@/hubs/games/turn-based-game-hub';

export class CrazyEightsGameHub extends TurnBasedGameHub<CrazyEightsGameInfo, CrazyEightsGameRules, CrazyEightsPlayer>
{
    constructor(gameId: string)
    {
        super(`/hubs/games/crazy-eights?gameId=${gameId}`);
    }

    changeDiscardColor(color: CardColor): Promise<boolean>
    {
        return this.connection.invoke(CrazyEightsGameHubActions[CrazyEightsGameHubActions.ChangeDiscardColor], color);
    }

    changeDiscardSuit(suit: CardSuit): Promise<boolean>
    {
        return this.connection.invoke(CrazyEightsGameHubActions[CrazyEightsGameHubActions.ChangeDiscardSuit], suit);
    }

    drawCard(): Promise<PlayingCard | null>
    {
        return this.connection.invoke(CrazyEightsGameHubActions[CrazyEightsGameHubActions.DrawCard]);
    }

    fetchPlayerHand(): Promise<PlayingCard[]>
    {
        return this.connection.invoke(CrazyEightsGameHubActions[CrazyEightsGameHubActions.FetchPlayerHand]);
    }

    onCardDrawn(eventHandler: (evt: CrazyEightsCardDrawnHubEvent) => void): this
    {
        this.connection.on(CrazyEightsGameHubEvents[CrazyEightsGameHubEvents.CardDrawn], eventHandler);
        return this;
    }

    onCardPlayed(eventHandler: (evt: CrazyEightsCardPlayedHubEvent) => void): this
    {
        this.connection.on(CrazyEightsGameHubEvents[CrazyEightsGameHubEvents.CardPlayed], eventHandler);
        return this;
    }

    onDiscardColorChanged(eventHandler: (evt: CrazyEightsDiscardColorChangedHubEvent) => void): this
    {
        this.connection.on(CrazyEightsGameHubEvents[CrazyEightsGameHubEvents.DiscardColorChanged], eventHandler);
        return this;
    }

    onDiscardSuitChanged(eventHandler: (evt: CrazyEightsDiscardSuitChangedHubEvent) => void): this
    {
        this.connection.on(CrazyEightsGameHubEvents[CrazyEightsGameHubEvents.DiscardSuitChanged], eventHandler);
        return this;
    }

    onPlayerFinished(eventHandler: (evt: CrazyEightsPlayerFinishedHubEvent) => void): this
    {
        this.connection.on(CrazyEightsGameHubEvents[CrazyEightsGameHubEvents.PlayerFinished], eventHandler);
        return this;
    }

    playCard(index: number, suit: CardSuit, rank: CardRank): Promise<boolean>
    {
        return this.connection.invoke(CrazyEightsGameHubActions[CrazyEightsGameHubActions.PlayCard], index, suit, rank);
    }
}
