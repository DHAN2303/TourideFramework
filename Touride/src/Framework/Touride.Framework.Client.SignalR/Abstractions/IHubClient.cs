﻿namespace Touride.Framework.Client.SignalR.Abstractions
{
    public interface IHubClient
    {
        Task Send(object data);

        event Action<object> OnBroadcastAction;
    }
}
