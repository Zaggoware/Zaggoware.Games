import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import {
    GameHubActions,
    GameHubEvents,
    GameInfo,
    GameRules,
    GameStartedHubEvent,
    GameStoppedHubEvent,
    Player,
    PlayerAddedHubEvent,
    PlayerRemovedHubEvent,
    SpectatorAddedHubEvent,
    SpectatorRemovedHubEvent,
    UserConnectedHubEvent,
    UserDisconnectedHubEvent
} from '@/models/custom';

export abstract class GameHub<TGameInfo extends GameInfo<TGameRules, TPlayer>, TGameRules extends GameRules, TPlayer extends Player>
{
    connection: HubConnection;

    protected constructor(hubUrl: string)
    {
        this.connection = new HubConnectionBuilder()
            .withUrl(hubUrl)
            .build();
    }

    connect(userName: string): Promise<void>
    {
        return this.connection.start()
            .then(() =>
            {
                return this.connection.invoke(GameHubActions[GameHubActions.ConnectUser], userName);
            })
            .catch((reason) =>
            {
                console.error('Could not connect to the GameHub:', reason);
            });
    }

    disconnect(): Promise<void>
    {
        return this.connection.stop();
    }

    fetchGameInfo(): Promise<TGameInfo>
    {
        return this.connection.invoke(GameHubActions[GameHubActions.FetchGameInfo]);
    }

    onGameStarted(eventHandler: (evt: GameStartedHubEvent<TGameInfo, TGameRules, TPlayer>) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.GameStarted], eventHandler);
        return this;
    }

    onGameStopped(eventHandler: (evt: GameStoppedHubEvent<TGameInfo, TGameRules, TPlayer>) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.GameStopped], eventHandler);
        return this;
    }

    onPlayerAdded(eventHandler: (evt: PlayerAddedHubEvent<TPlayer>) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.PlayerAdded], eventHandler);
        return this;
    }

    onPlayerRemoved(eventHandler: (evt: PlayerRemovedHubEvent<TPlayer>) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.PlayerRemoved], eventHandler);
        return this;
    }

    onSpectatorAdded(eventHandler: (evt: SpectatorAddedHubEvent) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.SpectatorAdded], eventHandler);
        return this;
    }

    onSpectatorRemoved(eventHandler: (evt: SpectatorRemovedHubEvent) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.SpectatorRemoved], eventHandler);
        return this;
    }

    onUserConnected(eventHandler: (evt: UserConnectedHubEvent) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.UserConnected], eventHandler);
        return this;
    }

    onUserDisconnected(eventHandler: (evt: UserDisconnectedHubEvent) => void): this
    {
        this.connection.on(GameHubEvents[GameHubEvents.UserDisconnected], eventHandler);
        return this;
    }

    startGame(): Promise<boolean>
    {
        return this.connection.invoke(GameHubActions[GameHubActions.StartGame]);
    }

    stopGame(): Promise<boolean>
    {
        return this.connection.invoke(GameHubActions[GameHubActions.StopGame]);
    }
}
