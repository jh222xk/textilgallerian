using System.Web.Optimization;

namespace AdminView
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-2.1.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/semantic").Include(
                "~/Scripts/semantic.js"));

            bundles.Add(new ScriptBundle("~/bundles/CreateCoupon").Include(
                "~/Scripts/CreateCoupon.js"));

            bundles.Add(new ScriptBundle("~/bundles/FlashMessage").Include(
                "~/Scripts/FlashMessage.js"));

            bundles.Add(new ScriptBundle("~/bundles/CreateRole").Include(
                "~/Scripts/CreateRole.js"));
            bundles.Add(new ScriptBundle("~/bundles/JSON").Include(
             "~/Scripts/JSON.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/Css/site.css",
               "~/Content/Css/semantic.min.css"));
        }
    }
}