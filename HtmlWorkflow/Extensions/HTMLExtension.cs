using HtmlWorkflow.Constants;
using HtmlWorkflow.Models;
using System.Text;

namespace HtmlWorkflow.Extensions
{
    /// <summary>
    /// HTML Extensions class
    /// </summary>
    public static class HtmlExtension
    {
        /// <summary>
        /// Return part of HTML document for <h1> attribute
        /// OUTPUT: <h1 class="className" style="style">{text}</h1>
        /// </summary>
        private static StringBuilder AddHeaderTextElement(this StringBuilder sb, HtmlHelper h1)
        {
            if (!string.IsNullOrEmpty(h1.Text))
            {
                sb.Append(GetTag(HtmlConstants.H1, h1));
            }
            return sb;
        }
        /// <summary>
        /// Return part of HTML document for open <div> block
        /// OUTPUT: <div class="className" style="divStyle">
        /// </summary>
        private static StringBuilder AddOpenDIVElement(this StringBuilder sb, HtmlDivHelper div)
        {
            sb.Append(GetOpenTag(HtmlConstants.DIV, new HtmlHelper { ClassName = div.ClassName, Style = div.Style }));
            return sb;
        }
        /// <summary>
        /// Return part of HTML document for close </div> block
        /// <result> 
        ///     </div>
        /// </result>
        /// </summary>
        private static StringBuilder AddCloseDIVElement(this StringBuilder sb)
        {
            sb.Append(HtmlConstants.CloseDIV);
            return sb;
        }
        /// <summary>
        /// Return part of HTML document for <p> attribute
        /// <result>
        ///     <p class="className" style="divStyle">{text}</p>
        /// </result>
        /// </summary>
        private static StringBuilder AddTextElement(this StringBuilder sb, HtmlHelper p)
        {
            sb.Append(GetTag(HtmlConstants.P, p));
            return sb;
        }
        /// <summary>
        /// Return part of HTML document for image block
        /// <result>
        ///     <div className="divClassName" style="divStyle">
        ///         <img src="imagePath" style="ingStyle" className="imgClassName" />
        ///     </div>
        /// </result>
        /// </summary>
        public static StringBuilder AddImageBlock(this StringBuilder sb, HtmlImgHelper img, HtmlDivHelper div)
        {
            if (!string.IsNullOrEmpty(img.ImagePath))
            {
                var imgTag = GetImageTag(img);
                sb.Append(GetTag(HtmlConstants.DIV, new HtmlHelper
                {
                    Text = imgTag,
                    ClassName = div.ClassName,
                    Style = div.Style
                }));
            }
            return sb;
        }
        /// <summary>
        /// Return part of HTML Document for simple text block
        /// <result>
        ///     <div class="classNameDIV" style="divStyle">
        ///         <p class="classNameP" style="pStyle">{text}</p>
        ///     </div>
        /// </result>
        /// </summary>
        public static StringBuilder AddTextBlock(this StringBuilder sb, HtmlHelper html, HtmlDivHelper div)
        {
            var text = GetTag(HtmlConstants.P, html);
            sb.Append(GetTag(HtmlConstants.DIV, new HtmlHelper { Text = text, ClassName = div.ClassName, Style = div.Style }));
            return sb;
        }
        /// <summary>
        /// Return part of HTML Document for array of simple  text block
        /// <result>
        ///     <div class="classNameDIV" style="divStyle">
        ///         <p class="classNameP1" style="p1Style">{text}</p>
        ///         ...............................
        ///         <p class="classNamePN" style="pnStyle">{text}</p>
        ///     </div>
        /// </result>
        /// </summary>
        public static StringBuilder AddArrayOfTextBlock(this StringBuilder sb, HtmlHelper[] items, HtmlDivHelper div)
        {
            sb.AddOpenDIVElement(div);
            foreach (var item in items)
            {
                sb.AddTextElement(item);
            }
            sb.AddCloseDIVElement();
            return sb;
        }
        /// <summary>
        /// Get Tag
        /// <result>
        ///     <T-Tag class="T-ClassName" style="T-Style">{text}</T-Tag>
        /// </result>
        /// </summary>
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
        private static string GetImageTag(HtmlImgHelper img)
        {
            return $"<{HtmlConstants.IMG} src='{img.ImagePath}' {GetStyle(img.Style)} {GetClassName(img.ClassName)} />";
        }
    }
}
