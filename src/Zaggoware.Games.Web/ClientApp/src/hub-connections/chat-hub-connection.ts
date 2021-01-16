import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export const chatHub = new HubConnectionBuilder()
    .withUrl('/hubs/chat')
    .configureLogging(LogLevel.Information)
    .build();

export default chatHub;
