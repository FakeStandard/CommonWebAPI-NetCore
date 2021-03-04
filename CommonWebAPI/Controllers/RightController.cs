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
    public class RightController : ControllerBase
    {
        private MyDbContext _context;
        public RightController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得所有權限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var rights = _context.Rights.ToList();

                return Ok(rights);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 取得所有權限的樹狀圖
        /// </summary>
        /// <returns></returns>
        [HttpGet("{type}")]
        public IActionResult Get(string type)
        {
            try
            {
                if (type.Equals("tree"))
                {
                    List<RightTreeVM> tree = new List<RightTreeVM>();

                    var items = _context.Rights.ToList();

                    foreach (var Lv1 in items.Where(m => m.Level == 0).OrderBy(m => m.Order))
                    {
                        List<RightTreeVM> children = new List<RightTreeVM>();

                        foreach (var Lv2 in items.Where(m => m.Level == 1 && m.ParentId == Lv1.Id).OrderBy(m => m.Order))
                        {
                            List<RightTreeVM> children2 = new List<RightTreeVM>();

                            foreach (var Lv3 in items.Where(m => m.Level == 2 && m.ParentId == Lv2.Id).OrderBy(m => m.Order))
                            {
                                children2.Add(new RightTreeVM()
                                {
                                    Id = Lv3.Id,
                                    AuthName = Lv3.AuthName,
                                    Path = Lv3.Path,
                                    ParentId = Lv3.ParentId,
                                    children = null
                                });
                            }

                            children.Add(new RightTreeVM()
                            {
                                Id = Lv2.Id,
                                AuthName = Lv2.AuthName,
                                Path = Lv2.Path,
                                ParentId = Lv2.ParentId,
                                children = children2
                            });
                        }

                        tree.Add(new RightTreeVM()
                        {
                            Id = Lv1.Id,
                            AuthName = Lv1.AuthName,
                            Path = Lv1.Path,
                            ParentId = Lv1.ParentId,
                            children = children
                        });
                    }

                    return Ok(tree);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
