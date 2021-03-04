using CommonWebAPI.Models;
using CommonWebAPI.ViewModels;
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
    public class RoleController : ControllerBase
    {
        private readonly MyDbContext _context;
        public RoleController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得角色資訊及權限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetA()
        {
            try
            {
                List<RoleRightVM> result = new List<RoleRightVM>();

                var roles = _context.Roles.ToList();

                foreach (var role in roles)
                {
                    List<RightVM> children = new List<RightVM>();

                    var rights = _context.RoleRights.Join(
                        _context.Rights,
                        rr => rr.RightID,
                        ri => ri.Id,
                        (rr, ri) => new { rr, ri }).Where(m => m.rr.RoleID == role.ID).ToList();

                    foreach (var right in rights.Where(m => m.ri.Level == 0).OrderBy(m => m.ri.Order))
                    {
                        List<RightVM> children1 = new List<RightVM>();

                        foreach (var ch1 in rights.Where(m => m.ri.Level == 1 & m.ri.ParentId == right.ri.Id).OrderBy(m => m.ri.Order))
                        {
                            List<RightVM> children2 = new List<RightVM>();

                            foreach (var ch2 in rights.Where(m => m.ri.Level == 2 & m.ri.ParentId == ch1.ri.Id).OrderBy(m => m.ri.Order))
                            {
                                children2.Add(new RightVM()
                                {
                                    Id = ch2.ri.Id,
                                    AuthName = ch2.ri.AuthName,
                                    Path = ch2.ri.Path,
                                    ParentId = ch2.ri.ParentId,
                                    Level = ch2.ri.Level
                                });
                            }

                            children1.Add(new RightVM()
                            {
                                Id = ch1.ri.Id,
                                AuthName = ch1.ri.AuthName,
                                Path = ch1.ri.Path,
                                ParentId = ch1.ri.ParentId,
                                Level = ch1.ri.Level,
                                children = children2
                            });
                        }

                        children.Add(new RightVM()
                        {
                            Id = right.ri.Id,
                            AuthName = right.ri.AuthName,
                            Path = right.ri.Path,
                            ParentId = right.ri.ParentId,
                            Level = right.ri.Level,
                            children = children1
                        });
                    }

                    result.Add(new RoleRightVM()
                    {
                        ID = role.ID,
                        RoleName = role.Name,
                        RoleDesc = role.Description,
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

        /// <summary>
        /// 取得特定的角色資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var role = _context.Roles.Find(id);

                RoleVM item = new RoleVM()
                {
                    ID = role.ID,
                    RoleName = role.Name,
                    RoleDesc = role.Description
                };

                return Ok(item);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 新增一筆角色資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(RoleVM item)
        {
            try
            {
                Roles role = new Roles();
                role.Name = item.RoleName;
                role.Description = item.RoleDesc;

                _context.Roles.Add(role);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 更新特定的角色資料
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(RoleVM item)
        {
            try
            {
                var role = _context.Roles.Find(item.ID);

                if (role != null)
                {
                    role.Name = item.RoleName;
                    role.Description = item.RoleDesc;

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
        /// 刪除一筆角色資料(連帶所有權線設置也刪除)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var role = _context.Roles.Find(id);
                _context.Roles.Remove(role);

                var items = _context.RoleRights.Where(m => m.RoleID == id).ToList();
                _context.RoleRights.RemoveRange(items);

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 刪除角色的特定權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="rightId"></param>
        /// <returns></returns>
        [HttpDelete("{roleId}/{rightId}")]
        public IActionResult Delete(int roleId, int rightId)
        {
            try
            {
                var item = _context.RoleRights.Where(m => m.RoleID == roleId & m.RightID == rightId).FirstOrDefault();

                if (item != null)
                {
                    _context.RoleRights.Remove(item);
                    _context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    _context.SaveChanges();

                    var items = _context.RoleRights.Where(m => m.RoleID == roleId).Join(
                        _context.Rights,
                        rr => rr.RightID,
                        r => r.Id,
                        (rr, r) => new { r.Id, r.AuthName, r.Path, r.Level, r.ParentId, r.Order }).ToList();

                    List<RightVM> result = new List<RightVM>();

                    foreach (var ch in items.Where(m => m.Level == 0).OrderBy(m => m.Level))
                    {
                        List<RightVM> children1 = new List<RightVM>();

                        foreach (var Lv1 in items.Where(m => m.Level == 1 & m.ParentId == ch.Id).OrderBy(m => m.Level))
                        {
                            List<RightVM> children2 = new List<RightVM>();

                            foreach (var Lv2 in items.Where(m => m.Level == 2 & m.ParentId == Lv1.Id).OrderBy(m => m.Level))
                            {
                                children2.Add(new RightVM()
                                {
                                    Id = Lv2.Id,
                                    AuthName = Lv2.AuthName,
                                    ParentId = Lv2.ParentId,
                                    Path = Lv2.Path,
                                    Level = Lv2.Level,
                                    children = null
                                });
                            }

                            children1.Add(new RightVM()
                            {
                                Id = Lv1.Id,
                                AuthName = Lv1.AuthName,
                                ParentId = Lv1.ParentId,
                                Path = Lv1.Path,
                                Level = Lv1.Level,
                                children = children2
                            });
                        }

                        result.Add(new RightVM()
                        {
                            Id = ch.Id,
                            AuthName = ch.AuthName,
                            ParentId = ch.ParentId,
                            Path = ch.Path,
                            Level = ch.Level,
                            children = children1
                        });
                    }

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
        /// 修改特定角色的權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="rids"></param>
        /// <returns></returns>
        [HttpPost("{roleId}/{rids}")]
        public IActionResult Post(int roleId, string? rids)
        {
            try
            {
                string[] str = rids.Split(',');

                var items = _context.RoleRights.Where(m => m.RoleID == roleId).ToList();

                _context.RoleRights.RemoveRange(items);

                List<RoleRights> result = new List<RoleRights>();

                for (int i = 0; i < str.Length; i++)
                {
                    result.Add(new RoleRights()
                    {
                        RoleID = roleId,
                        RightID = int.Parse(str[i].ToString())
                    });
                }

                _context.RoleRights.AddRange(result);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// 刪除特定角色的所有權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost("{roleId}")]
        public IActionResult Post(int roleId)
        {
            try
            {
                var items = _context.RoleRights.Where(m => m.RoleID == roleId).ToList();

                _context.RoleRights.RemoveRange(items);
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
