namespace Zaggoware.Games.Web.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        private static readonly ConcurrentBag<MessageInfo> Messages =
            new ConcurrentBag<MessageInfo>();

        public Task<List<MessageInfo>> FetchMessages()
        {
            var list = Messages.OrderBy(m => m.DateTimeUtc).ToList();
            return Task.FromResult(list);
        }

        // TODO: Implement different chat groups (Global, Lobby, Team, Private, etc.)
        public Task SendMessage(string name, string message)
        {
            if (Messages.Count > 100)
            {
                for (var i = 0; i < Messages.Count - 100; i++)
                {
                    Messages.TryTake(out var removedMessage);
                }
            }

            var dateTimeUtc = DateTime.UtcNow;
            var messageInfo = new MessageInfo(Guid.NewGuid(), name, message, dateTimeUtc);
            Messages.Add(messageInfo);
            return Clients.Others.SendAsync("MessageReceived", messageInfo);
        }

        public readonly struct MessageInfo
        {
            public MessageInfo(Guid id, string name, string message, DateTime dateTimeUtc)
            {
                Id = id;
                Name = name;
                Message = message;
                DateTimeUtc = dateTimeUtc;
            }

            public Guid Id { get; }

            public DateTime DateTimeUtc { get; }

            public string Message { get; }

            public string Name { get; }
        }
    }
}