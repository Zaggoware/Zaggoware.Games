import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

export abstract class HubBase
{
    connection: HubConnection;

    protected constructor(hubUrl: string)
    {
        this.connection = new HubConnectionBuilder()
            .withUrl(hubUrl)
            .build();
    }

    start(): Promise<void>
    {
        return this.connection.start()
            .catch((reason) =>
            {
                console.error('Could not connect to the hub:', reason);
            });
    }

    stop(): Promise<void>
    {
        return this.connection.stop();
    }
}