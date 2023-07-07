using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ECinema.Domain.DTO;
using ECinema.Domain.DomainModels;
using ECinema.Services.Interface;

namespace ECinema.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            var allTickets = this._ticketService.GetAllTickets();
            return View(allTickets);
        }


        //Tickets/AddTicketToCart
        public IActionResult AddTicketToCart(Guid? id)
        {
            var model = this._ticketService.GetShoppingCartInfoAsync(id);
            return View(model);
        }

        //Add to cart post akcija
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicketToCart([Bind("TicketId", "Quantity")] AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _ticketService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(item);
        }



        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetDetailsForTicketAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieName,MovieDescription,MovieRating,TicketPrice,MovieImage")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _ticketService.CreateNewTicketAsync(ticket);
                return RedirectToAction(nameof(Index));
            }
            
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetDetailsForTicketAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }
            
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MovieName,MovieDescription,MovieRating,TicketPrice,MovieImage")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(ticket);
            }
            
            try
            {
                await _ticketService.UpdateExistingTicketAsync(ticket);
            }
            catch (DbUpdateConcurrencyException)
            {
                var isTicketExist = await TicketExistsAsync(ticket.Id);
                
                if (!isTicketExist)
                {
                    return NotFound();
                }

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var ticket = await _ticketService.GetDetailsForTicketAsync(id);

            if (ticket is null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TicketExistsAsync(Guid id)
        {
            return await _ticketService.GetDetailsForTicketAsync(id) != null;
        }
    }
}