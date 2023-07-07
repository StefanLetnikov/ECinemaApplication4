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
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        
        public TicketService(IRepository<Ticket> ticketRepository, IUserRepository userRepository, 
            IRepository<TicketInShoppingCart> ticketInShoppingCartRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
        }

        public async Task<bool> AddToShoppingCart(AddToShoppingCartDto item, string userId)
        {
            var user = _userRepository.Get(userId);
            var userShoppingCart = user.UserCart;
            
            if (userShoppingCart is null)
            {
                return false;
            }
            
            var ticket = await GetDetailsForTicketAsync(item.TicketId);
            
            if (ticket is null)
            {
                return false;
            }
            
            TicketInShoppingCart itemToAdd = new TicketInShoppingCart
            {
                Ticket = ticket,
                TicketId = ticket.Id,
                ShoppingCart = userShoppingCart,
                ShoppingCartId = userShoppingCart.Id,
                Quantity = item.Quantity,
            };

            await _ticketInShoppingCartRepository.InsertAsync(itemToAdd);
            
            return true;
        }

        public async Task CreateNewTicketAsync(Ticket t)
        {
            await _ticketRepository.InsertAsync(t);
        }

        public async Task DeleteTicketAsync(Guid? id)
        {
            var ticket = await GetDetailsForTicketAsync(id);
            await _ticketRepository.DeleteAsync(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return _ticketRepository.GetAll().ToList();
        }

        public async Task<Ticket> GetDetailsForTicketAsync(Guid? id)
        {
            return await _ticketRepository.GetAsync(id);
        }

        public async Task<AddToShoppingCartDto> GetShoppingCartInfoAsync(Guid? id)
        {
            var ticket = await GetDetailsForTicketAsync(id);
            
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                SelectedTicket = ticket,
                TicketId = ticket.Id,
                Quantity = 1
            };

            return model;
        }

        public async Task UpdateExistingTicketAsync(Ticket ticket)
        {
            await _ticketRepository.UpdateAsync(ticket);
        }
    }
}