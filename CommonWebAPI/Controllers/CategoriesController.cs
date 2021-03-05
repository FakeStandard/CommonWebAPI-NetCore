using CommonWebAPI.Models;
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
    public class CategoriesController : ControllerBase
    {
        private MyDbContext _context;
        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得所有分類
        /// </summary>
        /// <param name="type">null or 1.2.3</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string? type)
        {
            try
            {
                List<CategoriesVM> result = new List<CategoriesVM>();

                var items = _context.Categories.ToList();

                foreach (var item in items.Where(m => m.Level == 0).ToList())
                {
                    List<CategoriesVM> children = null;

                    if (type == null | type != "1")
                    {
                        children = new List<CategoriesVM>();

                        foreach (var ch1 in items.Where(m => m.Level == 1 & m.ParentID == item.ID).OrderBy(m => m.Order).ToList())
                        {
                            List<CategoriesVM> children1 = null;

                            if (type == null | type != "2")
                            {
                                children1 = new List<CategoriesVM>();

                                foreach (var ch2 in items.Where(m => m.Level == 2 & m.ParentID == ch1.ID).OrderBy(m => m.Order).ToList())
                                {
                                    children1.Add(new CategoriesVM()
                                    {
                                        CatID = ch2.ID,
                                        CatName = ch2.Name,
                                        CatParentID = ch2.ParentID,
                                        CatLevel = ch2.Level,
                                        CatDeleted = ch2.Deleted,
                                        Order = ch2.Order,
                                        children = children1
                                    });
                                }
                            }

                            children.Add(new CategoriesVM()
                            {
                                CatID = ch1.ID,
                                CatName = ch1.Name,
                                CatParentID = ch1.ParentID,
                                CatLevel = ch1.Level,
                                CatDeleted = ch1.Deleted,
                                Order = ch1.Order,
                                children = children1
                            });
                        }
                    }

                    result.Add(new CategoriesVM()
                    {
                        CatID = item.ID,
                        CatName = item.Name,
                        CatParentID = item.ParentID,
                        CatLevel = item.Level,
                        CatDeleted = item.Deleted,
                        Order = item.Order,
                        children = children
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            try
            {
                var item = _context.Categories.Find(Id);

                if (item != null)
                {
                    CategoriesVM result = new CategoriesVM()
                    {
                        CatID = item.ID,
                        CatName = item.Name
                    };

                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 新增一筆分類
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(CategoriesVM items)
        {
            try
            {
                // 取得資料表內最大的 Order，再 +1 成為新資料的 order
                int order = 0;
                order = _context.Categories.Where(m => m.ParentID == items.CatParentID & m.Level == items.CatLevel).Select(m => m.Order).DefaultIfEmpty().Max();

                Categories categories = new Categories();
                categories.Name = items.CatName;
                categories.Level = items.CatLevel;
                categories.ParentID = items.CatParentID;
                categories.Deleted = items.CatDeleted;
                categories.Order = order + 1;

                _context.Categories.Add(categories);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 更新特定分類的名稱
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPatch("{id}/catName/{name}")]
        public IActionResult Patch(int id, string name)
        {
            try
            {
                var o = _context.Categories.Find(id);

                if(o != null)
                {
                    o.Name = name;
                    _context.SaveChanges();

                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 刪除一筆分類資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _context.Categories.Find(id);

                if (item != null)
                {
                    item.Deleted = true;
                    _context.SaveChanges();
                    return Ok();
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
