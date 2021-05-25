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
    public class UploadController : ControllerBase
    {
        /// <summary>
        /// 圖片上傳
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(object file)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
