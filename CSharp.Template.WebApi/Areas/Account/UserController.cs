using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharp.StackExchangeRedis;
using CSharp.StackExchangeRedis.Core;
using CSharp.Template.IServices.Account;
using CSharp.Template.PersistentObject.Account;
using CSharp.Template.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Type = Google.Protobuf.WellKnownTypes.Type;

namespace CSharp.Template.WebApi.Areas.Account
{
    public class UserController : ApiBaseController
    {
        private Lazy<IUserService> _userService;


        protected IRedisCached _redisCached;

        private ILogger _logger;
        
        public UserController(
            ILoggerFactory loggerFactory,
            IRedisCached redisCached, Lazy<IUserService> userService)
        {
            _logger = loggerFactory.CreateLogger<UserController>();
            _redisCached = redisCached;
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
            _logger.LogInformation("test");
            await _redisCached.StringSetAsync("first:1", "hellow");
            var result = await _userService.Value.GetAll();
            return SuccessResult(result);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(User user)
        {
            await _userService.Value.Insert(user);
            return SuccessResult();
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update(User user)
        {
            await _userService.Value.Update(user);
            return SuccessResult();
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Value.Deletes(u => u.Id == id);
            return SuccessResult();
        }
    }
}