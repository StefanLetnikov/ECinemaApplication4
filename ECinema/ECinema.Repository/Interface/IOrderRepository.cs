using ECinema.Domain.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECinema.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderDetailsAsync(BaseEntity model);
    }
}