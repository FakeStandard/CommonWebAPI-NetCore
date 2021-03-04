using CommonWebAPI.Models;
using CommonWebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CommonWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private MyDbContext _context;
        public UserController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得所有用戶資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string? query)
        {
            try
            {
                List<UserVM> result;

                if (query == null)
                {
                    result = _context.users.GroupJoin(
                        _context.Roles,
                        u => u.RoleID,
                        r => r.ID,
                        (x, y) => new { u = x, r = y })
                        .SelectMany(
                        x => x.r.DefaultIfEmpty(),
                        (x, y) => new UserVM
                        {
                            ID = x.u.ID,
                            Name = x.u.Name,
                            Account = x.u.Account,
                            Email = x.u.Email,
                            Mobile = x.u.Mobile,
                            RoleName = y.Name,
                            State = x.u.State
                        }).ToList();
                }
                else
                {
                    result = _context.users.Where(m => m.Name.Contains(query)).GroupJoin(
                        _context.Roles,
                        u => u.RoleID,
                        r => r.ID,
                        (x, y) => new { u = x, r = y })
                        .SelectMany(
                        x => x.r.DefaultIfEmpty(),
                        (x, y) => new UserVM
                        {
                            ID = x.u.ID,
                            Name = x.u.Name,
                            Account = x.u.Account,
                            Email = x.u.Email,
                            Mobile = x.u.Mobile,
                            RoleName = y.Name,
                            State = x.u.State
                        }).ToList();
                }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 取得特定用戶資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var user = _context.users.Find(id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 新增一筆用戶資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(UserVM item)
        {
            try
            {
                Users user = new Users();
                user.Name = item.Name;
                user.Account = item.Account;
                user.Password = item.Password;
                user.Email = item.Email;
                user.Mobile = item.Mobile;
                user.RoleID = 0;
                user.State = false;

                _context.users.Add(user);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 更新指定用戶資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(UserVM item)
        {
            try
            {
                var user = _context.users.Find(item.ID);

                if (user != null)
                {
                    _context.Entry(user).CurrentValues.SetValues(item);
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
        /// 更新用戶角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPatch("{id}/role/{roleId}")]
        public IActionResult Patch(int id, int roleId)
        {
            try
            {
                var user = _context.users.Find(id);
                user.RoleID = roleId;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 更新用戶狀態
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPatch("{id}/{state}")]
        public IActionResult Patch(int id, bool state)
        {
            try
            {
                var user = _context.users.First(m => m.ID == id);
                user.State = state;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 刪除指定用戶
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var users = _context.users.SingleOrDefault(m => m.ID == id);
                _context.users.Remove(users);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
