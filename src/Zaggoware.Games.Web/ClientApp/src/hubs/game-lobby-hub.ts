import { HubBase } from '@/hubs/hub-base';
import { GameLobby } from '@/models/custom';

export class GameLobbyHub extends HubBase
{
    constructor(id?: string, password?: string)
    {
        let hubUrl = '/hubs/lobby';
        if (!!id)
        {
            hubUrl += `?lobbyId=${id}`;
            if (!!password)
            {
                hubUrl += `&password=${password}`;
            }
        }

        super(hubUrl);
    }

    fetchLobbies(): Promise<GameLobby[]>
    {
        return this.connection.invoke('FetchLobbies');
    }
}
