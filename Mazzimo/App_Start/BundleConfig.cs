using System.Web;
using System.Web.Optimization;

namespace Mazzimo
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/cssPdf")
                .Include("~/Content/Pdf.css"));

            bundles.Add(new ScriptBundle("~/bundles/AngularApp")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Factories", "*.js")
                .IncludeDirectory("~/Scripts/Services", "*.js"));

        }
    }
}