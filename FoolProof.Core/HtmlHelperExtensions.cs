using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace FoolProof.Core
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent ModelValidation(
            this IHtmlHelper htmlHelper,
            string message = null,
            object htmlAttributes = null,
            string tag = null
        )
        {
            var modelPrefix = htmlHelper.ViewData.TemplateInfo?.HtmlFieldPrefix;

            var tagBlder = new TagBuilder("div");
            var validAttrs = htmlHelper.GetValidationAttributes();
            tagBlder.MergeAttributes(validAttrs);
            
            if (!tagBlder.Attributes.ContainsKey("data-model-validation"))
                tagBlder.MergeAttribute("data-model-validation", "true");

            if (!tagBlder.Attributes.ContainsKey("data-model-prefix") && !string.IsNullOrEmpty(modelPrefix))
                tagBlder.MergeAttribute("data-model-prefix", modelPrefix);

            var elemName = htmlHelper.ViewData.TemplateInfo?.GetFullHtmlFieldName("ModelValidationHandler");
            
            if (!tagBlder.Attributes.ContainsKey("name"))
                tagBlder.MergeAttribute("name", elemName);

            if (!tagBlder.Attributes.ContainsKey("id"))
            {
                var elemId = htmlHelper.ViewData.TemplateInfo?.GetFullHtmlFieldName("ModelValidationHandler")?.Replace('.', '_');
                tagBlder.MergeAttribute("id", elemId);
            }

            htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = elemName;
            try
            {
                var attrsDict = tagBlder.Attributes.ToDictionary(x => x.Key, x => (object)x.Value);
                var validMsgElem = htmlHelper.ValidationMessage(null, message, htmlAttributes, tag);
                tagBlder.InnerHtml.AppendHtml(validMsgElem);
                return tagBlder;
            }
            finally
            {
                htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = modelPrefix;
            }
        }

        public static IDictionary<string, string> GetValidationAttributes(this IHtmlHelper htmlHelper)
        {
            var attributeProvider = htmlHelper.ViewContext.HttpContext.RequestServices.GetService<ValidationHtmlAttributeProvider>();
            var modelExplorer = htmlHelper.ViewData.ModelExplorer;
            
            var result = new Dictionary<string, string>();
            attributeProvider.AddValidationAttributes(htmlHelper.ViewContext, modelExplorer, result);
            return result;
        }
    }
}
