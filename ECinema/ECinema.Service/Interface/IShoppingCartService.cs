using ECinema.Domain.DTO;
using System;
using System.Threading.Tasks;

namespace ECinema.Services.Interface
{
    public interface IShoppingCartService
    {
        public ShoppingCartDto GetShoppingCartInfo(string userId);
        public bool DeleteTicketFromShoppingCart(string userId, Guid id);
        public Task<bool> OrderNowAsync(string userId);
    }
}