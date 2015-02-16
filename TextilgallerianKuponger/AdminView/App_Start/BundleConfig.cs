using System.Web;
using System.Web.Optimization;

namespace AdminView {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/semantic").Include(
                      "~/Scripts/semantic.min.js",
                      "~/Scripts/semantic.site.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Css/semantic.min.css",
                      "~/Content/Css/site.css"));
        }
    }
}
