using ECinema.Domain.DomainModels;
using ECinema.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECinema.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("[action]")]
        public async Task<List<Order>> GetAllActiveOrders()
        {
            return await _orderService.GetAllOrders();
        }

        [HttpPost("[action]")]
        public async Task<Order> GetDetailsForOrder(BaseEntity model)
        {
            return await _orderService.GetOrderDetails(model);
        }

    }
}