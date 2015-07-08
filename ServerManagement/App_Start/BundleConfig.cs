using System.Web;
using System.Web.Optimization;

namespace ServerManagement
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/Library/jquery-2.1.4.js",
                        "~/Scripts/Library/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                         "~/Scripts/Library/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                         "~/Scripts/Library/Select2/js/select2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/Library/metro.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/core.js",
                      "~/Scripts/script.js",
                      "~/Scripts/Dialog.js",
                      "~/Scripts/SystemSetting.js",
                      "~/Scripts/Notify.js",
                      "~/Scripts/ServerSetting.js",
                      "~/Scripts/SystemServerSetting.js",
                      "~/Scripts/UserSetting.js",
                      "~/Scripts/UserSiteSetting.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(                        
                        "~/Content/font-awesome-4.3.0/css/font-awesome.css",
                        "~/Content/metro.css",
                        "~/Content/metro-icons.css",
                        "~/Scripts/Library/Select2/css/select2.css"
                        ));
        }
    }
}