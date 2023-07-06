using ECinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECinema.Services.Interface
{
    public interface IShoppingCartService
    {
        public ShoppingCartDto getShoppingCartInfo(string userId);
        public bool DeleteTicketFromShoppingCart(string userId, Guid id);
        public bool OrderNow(string userId);
    }
}