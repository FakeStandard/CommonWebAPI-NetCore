using CommonWebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private MyDbContext _context;
        public LoginController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 登入頁面
        /// </summary>
        /// <param name="objects"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(LoginVM item)
        {
            var result = _context.users.Any(m => m.Account == item.username && m.Password == item.password);

            if (result)
            {
                return Ok(result);
            }

            return NotFound();
        }
    }
}
