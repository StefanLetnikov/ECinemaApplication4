using ECinema.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Domain.DomainModels
{
    public class Order : BaseEntity
    {

        public string UserId { get; set; }
        public ECinemaApplicationUser User { get; set; }


        public IEnumerable<TicketInOrder> TicketInOrders { get; set; }
    }
}
