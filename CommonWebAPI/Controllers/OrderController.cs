using CommonWebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private MyDbContext _context;
        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得全部訂單
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string? query)
        {
            try
            {
                if (query == null)
                {
                    List<OrdersVM> list = (from m in _context.Orders
                                           select new OrdersVM()
                                           {
                                               ID = m.ID,
                                               OrderNumber = m.OrderNum,
                                               OrderPay = m.IsPay,
                                               OrderPrice = m.OrderPrice,
                                               OrderSend = m.IsSend,
                                               CreateTime = m.CreateTime
                                           }).ToList();
                    return Ok(list);
                }

                else
                {
                    List<OrdersVM> list = (from m in _context.Orders
                                           where m.OrderNum.Contains(query)
                                           select new OrdersVM()
                                           {
                                               ID = m.ID,
                                               OrderNumber = m.OrderNum,
                                               OrderPay = m.IsPay,
                                               OrderPrice = m.OrderPrice,
                                               OrderSend = m.IsSend,
                                               CreateTime = m.CreateTime
                                           }).ToList();
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
