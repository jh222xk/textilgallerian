using System.Web.Optimization;
using AdminView;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(
    typeof (SemanticUIStart), "PostStart")]

namespace AdminView
{
    public static class SemanticUIStart
    {
        public static void PostStart()
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/semantic").
                Include("~/Scripts/semantic.js",
                    "~/Scripts/semantic.site.js"));

            BundleTable.Bundles.Add(new StyleBundle("~/Content/semantic").
                Include("~/Content/semantic.css",
                    "~/Content/semantic.site.css"));
        }
    }
}