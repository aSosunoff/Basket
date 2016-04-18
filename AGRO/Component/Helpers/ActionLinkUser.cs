﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AGRO.Component.Helpers
{
    public static class ActionLinkUser
    {
        private static string BuildLink(string glyphiconClass, string linkText, string actionName, string controllerName, int count)
        {
            TagBuilder a = new TagBuilder("a");
            a.MergeAttribute("href", Path.Combine(String.Format("/{0}/{1}/", controllerName, actionName)));
            a.SetInnerText(linkText);
                TagBuilder span = new TagBuilder("span");
                span.AddCssClass(String.Format("glyphicon {0}", glyphiconClass));
                    TagBuilder spanBadge = new TagBuilder("span");
                    spanBadge.AddCssClass("badge");
                    spanBadge.SetInnerText(Convert.ToString(count));
                span.InnerHtml += spanBadge.ToString();
            a.InnerHtml += span.ToString();
            return a.ToString();
        }

        public static MvcHtmlString ActionLinkContractCount(this HtmlHelper htmlHelper, string linkText,string actionName, string controllerName, int count)
        {
            return new MvcHtmlString(BuildLink("glyphicon-list-alt", linkText, actionName, controllerName, count));
        }

        public static MvcHtmlString ActionLinkBasketCount(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, int count)
        {
            return new MvcHtmlString(BuildLink("glyphicon-shopping-cart", linkText, actionName, controllerName, count));
        }
    }
}