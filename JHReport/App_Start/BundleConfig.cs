using System.Web;
using System.Web.Optimization;

namespace JHReport
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/3rd/css").Include(
                "~/3rd/bootstrap-3.3.7-dist/css/bootstrap.min.css",
                "~/3rd/AdminLTE2.4.3/bower_components/font-awesome/css/font-awesome.min.css",
                "~/3rd/AdminLTE2.4.3/dist/css/AdminLTE.min.css",
                "~/3rd/AdminLTE2.4.3/dist/css/skins/_all-skins.min.css"
                ));
            bundles.Add(new ScriptBundle("~/3rd/js").Include(
                "~/3rd/bootstrap-3.3.7-dist/js/bootstrap.min.js",
                "~/3rd/AdminLTE2.4.3/dist/js/adminlte.min.js"
                ));
        }
    }
}
