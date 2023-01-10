using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Semple_Logistic.Models;

# nullable disable

namespace Semple_Logistic.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory ?? throw new ArgumentNullException(nameof(helperFactory));
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            TagBuilder result = new TagBuilder("div");

            int page = CurrentCountPage();

            AddPage(urlHelper, result, 1, "First");

            for (int i = page; i <= page + PageModel.ItemsPerPage; i++)
            {
                if (i > PageModel.TotalPages)
                {
                    break;
                }

                AddPage(urlHelper, result, i, i.ToString());
            }

            AddPage(urlHelper, result, PageModel.TotalPages, "Last");

            output.Content.AppendHtml(result.InnerHtml);
        }

        private void AddPage(IUrlHelper urlHelper, TagBuilder result, int i, string pageName)
        {
            TagBuilder tag = new TagBuilder("a");
            PageUrlValues["currentPage"] = i;
            tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);

            if (PageClassesEnabled)
            {
                tag.AddCssClass(PageClass);
                tag.AddCssClass(i == PageModel.CurrentPage
                    ? PageClassSelected : PageClassNormal);
            }

            tag.InnerHtml.Append(pageName);
            result.InnerHtml.AppendHtml(tag);
        }

        private int CurrentCountPage()
        {
            if (PageModel.CurrentPage < 5)
            {
                return 1;
            }
            else if (PageModel.CurrentPage > PageModel.TotalPages - 5)
            {
                return PageModel.TotalPages - 5;
            }
            else
            {
                return PageModel.CurrentPage - 1;
            }
        }
    }
}
