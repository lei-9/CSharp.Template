using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp.Template.IServices.Account;
using CSharp.Template.PersistentObject.Account;
using Microsoft.AspNetCore.Mvc;
using Type = Google.Protobuf.WellKnownTypes.Type;

namespace CSharp.Template.WebApi.Areas.Account
{
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 获取所有员工信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetAll")]
        [ProducesResponseType(typeof(List<User>), 200)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userService.GetAll();
                return new JsonResult(result);
            }
            catch(Exception ex)
            {
                
            }

            return new JsonResult("error");
        }
    }
}