using CSharp.Template.WebApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CSharp.Template.WebApi.Extensions
{
    public class ApiBaseController : ControllerBase
    {
        #region 返回数据封装

        //Newtonsoft.json 配置
        protected readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore, //忽略循环引用
            DateFormatString = "yyyy-MM-dd HH:mm:ss", //日期格式化，默认的格式不好看
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() //json中属性开头字母小写的驼峰命名
        };

        /// <summary>
        /// 成功的返回
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        protected IActionResult SuccessResult(bool success = true, string msg = null, int statusCode = StatusCodes.Status200OK)
        {
            return Content(JsonConvert.SerializeObject(new BaseResponse(success, msg, statusCode), _jsonSerializerSettings));
        }

        
        /// <summary>
        /// 成功的返回
        /// </summary>
        /// <param name="success"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <param name="statusCode"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IActionResult SuccessResult<T>(T data = default, bool success = true, string msg = null, int statusCode = StatusCodes.Status200OK)
        {
            return Content(JsonConvert.SerializeObject(new BaseResponse<T>(success, data, msg, statusCode), _jsonSerializerSettings));
        }

        /// <summary>
        /// 失败的返回
        /// </summary>
        /// <param name="success"></param>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <param name="statusCode"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IActionResult FailResult<T>(string msg = null, int statusCode = StatusCodes.Status500InternalServerError, bool success = false, T data = default)
        {
            return Content(JsonConvert.SerializeObject(new BaseResponse<T>(success, data, msg, statusCode), _jsonSerializerSettings));
        }

        #endregion
    }
}