using HtmlWorkflow.Constants;
using HtmlWorkflow.Models;
using System.Text;

namespace HtmlWorkflow.Extensions
{
    public static class HtmlTableExtension
    {
        /// <result>
        ///     <th style="style" className="className">{text}</th>
        /// </result>
        public static StringBuilder AddThElement(this StringBuilder sb, HtmlHelper html)
        {
            sb.Append(GetTag(HtmlConstants.TH, html));
            return sb;
        }
        /// <result>
        ///     <td style="style" className="className">{text}</td>
        /// </result>
        public static StringBuilder AddTdElement(this StringBuilder sb, HtmlHelper html)
        {
            sb.Append(GetTag(HtmlConstants.TD, html));
            return sb;
        }
        /// <result>
        /// <tr>
        ///     <th class="classTh1" style="styleTh1">{textTh1}</th>
        ///     ..................................................
        ///     <th class="classThn" style="styleThN">{textThN}</th>
        /// </tr>
        /// or
        /// <tr>
        ///     <td class="classTd1" style="styleHd1">{textTd1}</td>
        ///     ..................................................
        ///     <td class="classTdn" style="styleHdN">{textTdN}</td>
        /// </tr>
        /// </result>
        public static StringBuilder AddTrBlockElement(this StringBuilder sb, HtmlHelper[] items, TableMode tableMode)
        {
            var inlineBlock = new StringBuilder();
            foreach (var item in items)
            {
                switch (tableMode)
                {
                    case TableMode.Th:
                        {
                            inlineBlock.AddThElement(item);
                            break;
                        }
                    case TableMode.Td:
                        {
                            inlineBlock.AddTdElement(item);
                            break;
                        }
                }
            }
            sb.AppendLine(GetTag(HtmlConstants.TR, new HtmlHelper { Text = inlineBlock.ToString() }));
            return sb;
        }
        /// <summary>
        /// <result>
        /// <table style="tableStyle" class="tableClass">
        /// <tr>
        ///     <th></th>
        ///     .........
        /// </tr>
        /// <tr>
        ///     <td></td>
        ///     .........
        /// </tr>
        /// ............
        /// <tr>
        ///     <td></td>
        ///     .........
        /// </tr>
        /// </table>
        /// </result>
        /// </summary>
        public static StringBuilder AddTableElement(this StringBuilder sb, List<HtmlHelper[]> tds, HtmlHelper[]? ths = null, HtmlHelper? table = null)
        {
            //tr block
            var tableSB = new StringBuilder();
            if (ths is not null && ths.Length > 0) 
            {
                tableSB.AddTrBlockElement(ths, TableMode.Th);
            }
            //ht blocks
            foreach (var td in tds)
            {
                tableSB.AddTrBlockElement(td, TableMode.Td);
            }
            var tableTag = GetTag(HtmlConstants.TABLE, new HtmlHelper { Text = tableSB.ToString() });
            sb.Append(GetTag(HtmlConstants.DIV, new HtmlHelper
            {
                Text = tableTag,
                Style = table.Style,
                ClassName = table.ClassName
            }));
            return sb;
        }
        /// Get Tag
        /// <result>
        ///     <T-Tag class="T-ClassName" style="T-Style">{text}</T-Tag>
        /// </result>
        private static string GetTag(string tag, HtmlHelper html)
        {
            return GetOpenTag(tag, html) + $"{html.Text}" + GetCloseTag(tag);
        }
        private static string GetOpenTag(string tag, HtmlHelper html)
        {
            return $"<{tag} {GetClassName(html.ClassName)} {GetStyle(html.Style)}>";
        }
        private static string GetClassName(string className)
        {
            return !string.IsNullOrEmpty(className) ? $"class='{className}'" : string.Empty;
        }
        private static string GetStyle(string style)
        {
            return !string.IsNullOrEmpty(style) ? $"style='{style}'" : string.Empty;
        }
        private static string GetCloseTag(string tag)
        {
            return $"</{tag}>";
        }
    }
}
