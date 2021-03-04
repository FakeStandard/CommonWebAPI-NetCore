using CommonWebAPI.Models;
using CommonWebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace CommonWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MyDbContext _context;
        public MenuController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得側邊欄選單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<MenuVM> menus = new List<MenuVM>();

                var items = _context.Menus.ToList();

                foreach (var item1 in items.Where(m => m.Level == 0).OrderBy(m => m.Order))
                {
                    List<MenuVM> children = new List<MenuVM>();
                    
                    foreach (var item2 in items.Where(m => m.Level == 1 & m.ParentId == item1.ID).OrderBy(m => m.Order))
                    {
                        List<MenuVM> children2 = new List<MenuVM>();

                        foreach (var item3 in items.Where(m => m.Level == 2 & m.ParentId == item1.ID).OrderBy(m => m.Order))
                        {
                            children2.Add(new MenuVM()
                            {
                                ID = item3.ID,
                                AuthName = item3.AuthName,
                                Path = item3.Path
                            });
                        }

                        children.Add(new MenuVM()
                        {
                            ID = item2.ID,
                            AuthName = item2.AuthName,
                            Path = item2.Path,
                            children = children2
                        });
                    }

                    menus.Add(new MenuVM()
                    {
                        ID = item1.ID,
                        AuthName = item1.AuthName,
                        Path = item1.Path,
                        children = children
                    });
                }

                return Ok(menus);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Post(JObject payload)
        {
            return Ok(payload);
        }
    }
}
