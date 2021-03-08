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
    public class ParamsController : ControllerBase
    {
        private MyDbContext _context;
        public ParamsController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得分類參數列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}/{type}")]
        public IActionResult Get(int id, string type)
        {
            try
            {
                int t = type == "many" ? 0 : 1;

                var items = (from m in _context.Params
                             where m.CategoriesID == id & m.Type == t
                             select new ParamsVM
                             {
                                 ID = m.ID,
                                 CategoriesID = m.CategoriesID,
                                 attrName = m.Name,
                                 attrVals = (from t in _context.ParamsTags
                                             where t.ParamsID == m.ID
                                             select new ParamsTagsVM
                                             {
                                                 ID = t.ID,
                                                 ParamsID = t.ParamsID,
                                                 Name = t.Name
                                             }).ToList()
                             }).ToList();

                return Ok(items);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 新增一筆分類參數
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(ParamsVM item)
        {
            try
            {
                int t = item.attrType == "many" ? 0 : 1;

                Params obj = new Params()
                {
                    CategoriesID = item.CategoriesID,
                    Name = item.attrName,
                    Type = (byte)t
                };

                _context.Params.Add(obj);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 取得一筆分類參數
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var item = _context.Params.Find(id);

                if (item != null)
                {
                    ParamsVM obj = new ParamsVM()
                    {
                        ID = item.ID,
                        CategoriesID = item.CategoriesID,
                        attrName = item.Name
                    };

                    return Ok(obj);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 修改一筆分類參數
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{id}/{attrName}")]
        public IActionResult Patch(int id, string attrName)
        {
            try
            {
                var item = _context.Params.Find(id);

                if (item != null)
                {
                    item.Name = attrName;
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
        /// 刪除一筆分類參數
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _context.Params.Find(id);

                if (item != null)
                {
                    _context.Params.Remove(item);
                    _context.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 新增一個分類參數的標籤
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("tag")]
        public IActionResult Post(ParamsTagsVM item)
        {
            try
            {
                ParamsTags obj = new ParamsTags()
                {
                    ParamsID = item.ParamsID,
                    Name = item.Name
                };

                _context.ParamsTags.Add(obj);
                _context.SaveChanges();

                var last = _context.ParamsTags.Where(m => m.ParamsID == item.ParamsID).OrderBy(m => m.ID).LastOrDefault();

                return Ok(last.ID);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 刪除一個分類參數的標籤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("tag/{id}")]
        public IActionResult DeleteTag(int id)
        {
            try
            {
                var item = _context.ParamsTags.Find(id);
                if (item != null)
                {
                    _context.ParamsTags.Remove(item);
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
