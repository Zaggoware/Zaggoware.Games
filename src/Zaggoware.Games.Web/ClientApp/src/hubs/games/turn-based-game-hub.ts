import {
    GameTurnBeganHubEvent, GameTurnEndedHubEvent,
    Player,
    TurnBasedGameHubActions,
    TurnBasedGameHubEvents,
    TurnBasedGameInfo,
    TurnBasedGameRules
} from '@/models/custom';
import { GameHub } from '@/hubs/games/game-hub';

export abstract class TurnBasedGameHub<
    TGameInfo extends TurnBasedGameInfo<TGameRules, TPlayer>,
    TGameRules extends TurnBasedGameRules,
    TPlayer extends Player>
    extends GameHub<TGameInfo, TGameRules, TPlayer>
{
    protected constructor(hubUrl: string)
    {
        super(hubUrl);
    }

    beginTurn(): Promise<boolean>
    {
        return this.connection.invoke(TurnBasedGameHubActions[TurnBasedGameHubActions.BeginTurn]);
    }

    endTurn(): Promise<boolean>
    {
        return this.connection.invoke(TurnBasedGameHubActions[TurnBasedGameHubActions.EndTurn]);
    }

    onTurnBegan(eventHandler: (evt: GameTurnBeganHubEvent) => void): this
    {
        this.connection.on(TurnBasedGameHubEvents[TurnBasedGameHubEvents.TurnBegan], eventHandler);
        return this;
    }

    onTurnEnded(eventHandler: (evt: GameTurnEndedHubEvent) => void): this
    {
        this.connection.on(TurnBasedGameHubEvents[TurnBasedGameHubEvents.TurnEnded], eventHandler);
        return this;
    }
}
