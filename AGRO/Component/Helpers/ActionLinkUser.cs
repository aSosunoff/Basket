using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using Model.Engine.Repository;
using Model.Engine.Service;
using Model.Engine.Service.Interface;

namespace AGRO.Component.Helpers
{
    public static class ActionLinkUser
    {

        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string actionName, string controllerName, IEnumerable<WrapModel<AGRO_CATEGORY>> wrapModels)
        {
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("nav nav-pills nav-stacked");

            ul.InnerHtml += GetTagLi(wrapModels, wrapModels.Where(e => e.LEVEL == 1), actionName, controllerName);

            return new MvcHtmlString(ul.ToString());
        }

        private static string GetTagLi<T>(IEnumerable<WrapModel<T>> wrapModels, IEnumerable<WrapModel<T>> wrapModelsCopy, string actionName, string controllerName, string resLine = "")
        {

            foreach (var element in wrapModelsCopy)
            {
                TagBuilder li = new TagBuilder("li");
                TagBuilder a = new TagBuilder("a");

                a.MergeAttribute("href", Path.Combine(String.Format("/{0}/{1}/?id={2}", controllerName, actionName, element.ITEM.GetValueDecimal("ID"))));

                a.SetInnerText(element.ITEM.GetValueString("NAME"));

                if (element.FLAG_TREE != true)
                {
                    a.AddCssClass("downSubItem");
                    a.InnerHtml += "<span class=\"caret\"></span>";
                    li.InnerHtml += a.ToString();

                    TagBuilder ul = new TagBuilder("ul");
                    ul.AddCssClass("dropdown-menu");

                    IEnumerable<WrapModel<T>> models =
                        wrapModels.Where(
                            e =>
                                e.ITEM.GetValueDecimal("P_ID") == element.ITEM.GetValueDecimal("ID"));
                    ul.InnerHtml += GetTagLi(wrapModels, models, actionName, controllerName);

                    
                    li.InnerHtml += ul.ToString();
                }
                else
                {
                    li.InnerHtml += a.ToString();
                }
                resLine += li.ToString();
            }
            return resLine;
        }

        ////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////// 


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