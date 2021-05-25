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
    public class GoodsController : ControllerBase
    {
        private MyDbContext _context;
        public GoodsController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得所有商品列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string? query)
        {
            try
            {
                if (query == null)
                    return Ok(_context.Goods.ToList());
                else
                    return Ok(_context.Goods.Where(m => m.Name.Contains(query)).ToList());
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 刪除一筆指定商品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _context.Goods.Find(id);
                if (item != null)
                {
                    _context.Goods.Remove(item);
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
        /// 新增一筆商品
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(GoodsVM item)
        {
            try
            {
                // open a transacation
                // add goods object
                Goods obj = new Goods()
                {
                    Name = item.Name, // 商品名稱
                    Price = item.Price, // 商品價格
                    Weight = item.Weight, // 商品重量
                    Amount = item.Amount, // 商品數量
                    Introduce = item.Introduce, // 商品內容
                    CategoriesID = item.CategoriesID, // 商品分類
                    CreateTime = DateTime.Now // 建立時間
                };
                _context.Goods.Add(obj);

                // add goodsdetail object
                foreach (var o in item.GoodsDetails)
                {
                    GoodsDetail objDetaill = new GoodsDetail()
                    {
                        GoodsID = 0, // 商品ID
                        CategoriesID = item.CategoriesID, // 商品分類ID
                        ParamsID = o.ParamsID, // 參數ID
                        Type = o.Type, // 動態參數or靜態屬性
                        Content = o.Content // 商品內容
                    };

                    _context.GoodsDetail.Add(objDetaill);
                }

                //_context.SaveChanges();
                int ID = obj.ID;

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
