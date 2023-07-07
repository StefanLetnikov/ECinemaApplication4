using ECinema.Domain.DomainModels;
using ECinema.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ECinema.Services.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Task<Ticket> GetDetailsForTicketAsync(Guid? id);
        Task CreateNewTicketAsync(Ticket t);
        Task UpdateExistingTicketAsync(Ticket ticket);
        Task<AddToShoppingCartDto> GetShoppingCartInfoAsync(Guid? id);
        Task DeleteTicketAsync(Guid? id);
        Task<bool> AddToShoppingCart(AddToShoppingCartDto item, string userId);
    }
}