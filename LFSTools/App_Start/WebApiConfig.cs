﻿using System.Web.Http;

namespace LFSTools
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified config.
        /// </summary>
        /// <param name="config">The config.</param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
