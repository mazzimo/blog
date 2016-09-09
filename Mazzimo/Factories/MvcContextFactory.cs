using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mazzimo.Factories
{
    public class MvcContextFactory : IContextFactory
    {
        public Uri GetCurrentRequestUri()
        {
            return System.Web.HttpContext.Current.Request.Url;
        }
    }
}