using ECinema.Domain.DomainModels;
using ECinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECinema.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbSet<Order> _entities;

        public OrderRepository(ApplicationDbContext context)
        {
            _entities = context.Set<Order>();
        }
        
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _entities
                .Include(z => z.TicketInOrders)
                .Include(z => z.User)
                .Include("TicketInOrders.OrderedTicket")
                .ToListAsync();
        }

        public async Task<Order> GetOrderDetailsAsync(BaseEntity model)
        {
            return await _entities
                .Include(z => z.TicketInOrders)
                .Include(z => z.User)
                .Include("TicketInOrders.OrderedTicket")
                .SingleOrDefaultAsync(z => z.Id == model.Id);
        }
    }
}