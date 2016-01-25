using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Mazzimo.Helpers
{
    public static class HtmlHelperExt
    {
        public static MvcHtmlString HtmlLink(this HtmlHelper html, string url, string text, object htmlAttributes)
        {
            TagBuilder tb = new TagBuilder("a");
            tb.InnerHtml = text;
            tb.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tb.MergeAttribute("href", url);
            return MvcHtmlString.Create(tb.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString Element(this HtmlHelper helper, string tagName, object htmlAttributes, string innerHtml = null)
        {
            // Create tag builder
            var builder = new TagBuilder(tagName);
            TagRenderMode renderMode = TagRenderMode.SelfClosing;

            if (innerHtml != null)
            {
                renderMode = TagRenderMode.Normal;
                builder.InnerHtml = innerHtml;
            }
            // Add attributes
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            // Render tag
            return MvcHtmlString.Create(builder.ToString(renderMode));
        }
    }
}