using ECinema.Domain.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECinema.Services.Interface
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllOrders();
        Task<Order> GetOrderDetails(BaseEntity model);
    }
}