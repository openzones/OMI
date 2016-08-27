using System.Web.Mvc;

namespace OMInsurance.WebApps.Helpers
{
    public static class ActionImageHelper
    {
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controller, string id, string src, string alt, int h = 300, int w = 300)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Action(action, controller, new { filename = src }));
            imgBuilder.MergeAttribute("alt", alt);
            imgBuilder.MergeAttribute("height", h.ToString());
            imgBuilder.MergeAttribute("width", w.ToString());
            imgBuilder.MergeAttribute("id", id);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            return MvcHtmlString.Create(imgHtml);
        }
    }
}