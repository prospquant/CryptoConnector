using HQConnector.Dto.DTO.Balance;
using HQConnector.Dto.DTO.Order;
using HQConnector.Dto.DTO.Position;
using System;
using System.Collections.Generic;

namespace HQConnector.Core.Interfaces.Socket
{
    public interface ISocketPrivateData
    {

        event Action<IEnumerable<Balance>> UpdateBalances;
        event Action<IEnumerable<Order>> UpdateOrders;
        event Action<IEnumerable<Position>> UpdatePositions;

     
        void OnMessageReceived(object sender, EventArgs e);
        void ReconnectSocket();

        void SubscribeToBalances();
        void SubscribeToOrders();
        void SubscribeToPosition();


        void UnsubscribeToBalances();
        void UnsubscribeToOrders();
        void UnsubscribeToPosition();
    }
}