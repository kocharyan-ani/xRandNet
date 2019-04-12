using System;

using Core.Enumerations;

namespace Core.Events
{
    public class NetworkEventArgs : EventArgs
    {
        public NetworkStatus Status { get; private set; }

        public NetworkEventArgs(NetworkStatus status)
        {
            Status = status;
        }
    }

    public delegate void NetworkStatusUpdateHandler(Object sender, NetworkEventArgs e);
}
