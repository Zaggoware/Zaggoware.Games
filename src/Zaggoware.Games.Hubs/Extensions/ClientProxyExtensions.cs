namespace Zaggoware.Games.Hubs.Extensions
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Zaggoware.Games.Hubs.Models.Events;

    public static class ClientProxyExtensions
    {
        public static Task InvokeEventAsync<TEventModel>(
            this IClientProxy clientProxy,
            TEventModel eventModel)
            where TEventModel : class, IHubEventModel
        {
            return clientProxy.SendAsync(eventModel.EventName, eventModel);
        }
    }
}