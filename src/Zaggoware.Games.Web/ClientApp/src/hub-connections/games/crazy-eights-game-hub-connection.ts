import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import {
    CardColor,
    CardRank,
    CardSuit,
    CrazyEightsCardPlayedGame,
    CrazyEightsColorChangedHubEventModel,
    CrazyEightsGameEvents,
    CrazyEightsGameInfo,
    CrazyEightsSuitChangedHubEventModel,
    PlayingCard
} from '@/models/custom';

export class CrazyEightsGameHubConnection
{
    hubConnection: HubConnection = new HubConnectionBuilder()
        .withUrl('/hubs/games/crazy-eights?gameId=1')
        .configureLogging(LogLevel.Information)
        .build();

    changeColor(color: CardColor): Promise<boolean>
    {
        return this.hubConnection.invoke('ChangeColor', color);
    }

    changeSuit(suit: CardSuit): Promise<boolean>
    {
        return this.hubConnection.invoke('ChangeSuit', suit);
    }

    connectUser(name: string): Promise<void>
    {
        return this.hubConnection.send('ConnectUser', name);
    }

    drawCard(): Promise<PlayingCard | null>
    {
        return this.hubConnection.invoke('DrawCard');
    }

    endTurn(): Promise<boolean>
    {
        return this.hubConnection.invoke('EndTurn');
    }

    fetchGameInfo(): Promise<CrazyEightsGameInfo>
    {
        return this.hubConnection.invoke('FetchGameInfo');
    }

    playCard(index: number, suit: CardSuit, rank: CardRank): Promise<boolean>
    {
        return this.hubConnection.invoke('PlayCard', index, suit, rank);
    }

    onColorChanged(eventHandler: (model: CrazyEightsColorChangedHubEventModel) => void): CrazyEightsGameHubConnection
    {
        this.hubConnection.on(CrazyEightsGameEvents[CrazyEightsGameEvents.ColorChanged], eventHandler);
        return this;
    }

    onSuitChanged(eventHandler: (model: CrazyEightsSuitChangedHubEventModel) => void): CrazyEightsGameHubConnection
    {
        this.hubConnection.on(CrazyEightsGameEvents[CrazyEightsGameEvents.SuitChanged], eventHandler);
        return this;
    }

    onCardPlayed(eventHandler: (model: CrazyEightsCardPlayedGame) => void): CrazyEightsGameHubConnection
    {
        this.hubConnection.on(CrazyEightsGameEvents[CrazyEightsGameEvents.CardPlayed], eventHandler);
        return this;
    }

    startGame(): Promise<boolean>
    {
        return this.hubConnection.invoke('StartGame');
    }
}
