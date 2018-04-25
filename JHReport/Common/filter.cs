using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JHReport.Common
{
    public class filter: ActionFilterAttribute
    {
        filterContextInfo fcinfo;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //--------
            fcinfo = new filterContextInfo(filterContext);
            //fcinfo.actionName;//获取域名
            //fcinfo.controllerName;获取 controllerName 名称

            bool isstate = true;
            //islogin = false;
            if (isstate)//如果满足
            {
                //逻辑代码
                // filterContext.Result = new HttpUnauthorizedResult();//直接URL输入的页面地址跳转到登陆页  
                // filterContext.Result = new RedirectResult("http://www.baidu.com");//也可以跳到别的站点
                //filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "product", action = "Default" }));
            }
            else
            {
                filterContext.Result = new ContentResult { Content = @"抱歉,你不具有当前操作的权限！" };// 直接返回 return Content("抱歉,你不具有当前操作的权限！")
            }
            //--------

            filterContext.HttpContext.Response.Write("我是OnActionExecuting，我在action方法调用前执行<br/>");
            //判断该action方法时候有贴上filter标签  
            if (filterContext.ActionDescriptor.IsDefined(typeof(filter), false))
            {
                //如果有，为该action方法直接返回ContentResult，则该action方法在这里就有了返回值，相当于在这里就结束了，不会在去执行之后的方法，如：OnActionExecuted等  
                filterContext.Result = new ContentResult();
            }
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class MyFilter2Attribute : AuthorizeAttribute
    {



        //在所有action方法过滤器之前执行  
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.HttpContext.Response.Write("我是OnAuthorization，在所有action方法过滤器之前执行<br/>");
            //base.OnAuthorization(filterContext);  
        }
    }

    public class filterContextInfo
    {
        public filterContextInfo(ActionExecutingContext filterContext)
        {
            #region 获取链接中的字符
            // 获取域名
            domainName = filterContext.HttpContext.Request.Url.Authority;

            //获取模块名称
            //  module = filterContext.HttpContext.Request.Url.Segments[1].Replace('/', ' ').Trim();

            //获取 controllerName 名称
            controllerName = filterContext.RouteData.Values["controller"].ToString();

            //获取ACTION 名称
            actionName = filterContext.RouteData.Values["action"].ToString();

            #endregion
        }
        /// <summary>
        /// 获取域名
        /// </summary>
        public string domainName { get; set; }
        /// <summary>
        /// 获取模块名称
        /// </summary>
        public string module { get; set; }
        /// <summary>
        /// 获取 controllerName 名称
        /// </summary>
        public string controllerName { get; set; }
        /// <summary>
        /// 获取ACTION 名称
        /// </summary>
        public string actionName { get; set; }

    }
}