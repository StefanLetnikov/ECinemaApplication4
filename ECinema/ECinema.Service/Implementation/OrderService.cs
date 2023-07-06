using ECinema.Domain.DomainModels;
using ECinema.Repository.Interface;
using ECinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository _orderRepository)
        {
            this._orderRepository = _orderRepository;
        }
        public List<Order> GetAllOrders()
        {
            return this._orderRepository.GetAllOrders();
        }

        public Order GetOrderDetails(BaseEntity model)
        {
            return this._orderRepository.GetOrderDetails(model);
        }
    }
}