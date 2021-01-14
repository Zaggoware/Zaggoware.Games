namespace Zaggoware.Games.Hubs.Extensions
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Zaggoware.Games.Hubs.Models;

    public static class ClientProxyExtensions
    {
        public static Task InvokeEventAsync<TEventEnum, TEventModel>(
            this IClientProxy clientProxy,
            TEventEnum @event,
            TEventModel eventModel)
            where TEventEnum : Enum
            where TEventModel : class, IHubEventModel
        {
            return clientProxy.SendAsync(@event.ToString(), eventModel);
        }
    }
}