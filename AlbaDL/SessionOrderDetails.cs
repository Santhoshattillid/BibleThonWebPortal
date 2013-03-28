using System;
using System.Collections.Generic;

namespace AlbaDL
{
    [Serializable]
    public class SessionOrderDetails
    {
        public SessionOrderDetails()
        {
            OrderProcess = new OrderProcess();
            ListOrders = new List<OrderItems>();
        }

        public OrderProcess OrderProcess { get; set; }

        public List<OrderItems> ListOrders { get; set; }

        public CardDetails CardDetails { get; set; }

        public string CustomerName { get; set; }

        public decimal OrderGrandTotal { get; set; }
    }
}