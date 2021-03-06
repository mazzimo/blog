﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mazzimo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Post",
                url: "{id}",
                defaults: new { controller = "Home", action = "Post" });

            routes.MapRoute(
                name: "CvPdf",
                url: "cv/{id}-print",
                defaults: new { controller = "Home", action = "CvPrint" });

            routes.MapRoute(
                name: "Cv",
                url: "cv/{id}",
                defaults: new { controller = "Home", action = "Cv" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}