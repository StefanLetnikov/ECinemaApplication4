using ECinema.Domain.DomainModels;
using ECinema.Domain.DTO;
using ECinema.Repository.Interface;
using ECinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECinema.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<TicketInOrder> _ticketInOrderRepository;
        private readonly IUserRepository _userRepository;


        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, IUserRepository userRepository, IRepository<TicketInOrder> ticketInOrderRepository, IRepository<Order> orderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _ticketInOrderRepository = ticketInOrderRepository;
            _orderRepository = orderRepository;
        }

        public bool DeleteTicketFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = _userRepository.Get(userId);


                var userShoppingCart = loggedInUser.UserCart;
                var itemToDelete = userShoppingCart.TicketInShoppingCarts.Where(z => z.Ticket.Id.Equals(id)).FirstOrDefault();

                userShoppingCart.TicketInShoppingCarts.Remove(itemToDelete);

                _shoppingCartRepository.UpdateAsync(userShoppingCart);
                return true;
            }
            return false;
        }

        public ShoppingCartDto GetShoppingCartInfo(string userId)
        {

            var loggedInUser = _userRepository.Get(userId);


            var userShoppingCart = loggedInUser.UserCart;

            var allTickets = userShoppingCart.TicketInShoppingCarts.ToList();

            var allTicketPrices = allTickets.Select(z => new {
                TicketPrice = z.Ticket.TicketPrice,
                Quantity = z.Quantity
            }).ToList();

            var totalPrice = 0;
            foreach (var item in allTicketPrices)
            {
                totalPrice += item.Quantity * item.TicketPrice;
            }

            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Tickets = allTickets,
                TotalPrice = totalPrice
            };

            return scDto;
        }

        public async Task<bool> OrderNowAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }
            
            var loggedInUser = _userRepository.Get(userId);
            var userShoppingCart = loggedInUser.UserCart;

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                User = loggedInUser,
                UserId = userId
            };

            await _orderRepository.InsertAsync(order);
            
            List<TicketInOrder> ticketInOrders = new List<TicketInOrder>();

            var result = userShoppingCart.TicketInShoppingCarts.Select(z => new TicketInOrder
            {
                TicketId = z.Ticket.Id,
                OrderedTicket = z.Ticket,
                OrderId = order.Id,
                UserOrder = order
            }).ToList();

            ticketInOrders.AddRange(result);

            foreach (var item in ticketInOrders)
            {
                await _ticketInOrderRepository.InsertAsync(item);
            }

            loggedInUser.UserCart.TicketInShoppingCarts.Clear();
            _userRepository.Update(loggedInUser);
            
            return true;
        }
    }
}