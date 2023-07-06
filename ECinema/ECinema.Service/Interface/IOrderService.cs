using ECinema.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Services.Interface
{
    public interface IOrderService
    {
        public List<Order> GetAllOrders();
        Order GetOrderDetails(BaseEntity model);
    }
}