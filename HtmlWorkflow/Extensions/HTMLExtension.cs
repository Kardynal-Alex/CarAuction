using HtmlWorkflow.Constants;
using HtmlWorkflow.Models;
using System.Text;

namespace HtmlWorkflow.Extensions
{
    /// <summary>
    /// HTML Extensions class
    /// </summary>
    public static class HTMLExtension
    {
        /// <summary>
        /// Return part of HTML document for <h1> attribute
        /// OUTPUT: <h1>{text}</h1>
        /// </summary>
        private static StringBuilder AddHeaderTextElement(this StringBuilder sb, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                sb.Append(GetTag(HTMLConstants.H1, text));
            }
            return sb;
        }
        /// <summary>
        /// Return part of HTML document for open <div> block
        /// OUTPUT: <div class="className">
        /// </summary>
        private static StringBuilder AddOpenDIVElement(this StringBuilder sb, string className = null)
        {
            sb.Append(GetOpenTag(HTMLConstants.DIV, className));
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
            sb.Append(HTMLConstants.CloseDIV);
            return sb;
        }
        /// <summary>
        /// Return part of HTML document for <p> attribute
        /// <result>
        ///     <p class="className">{text}</p>
        /// </result>
        /// </summary>
        private static StringBuilder AddTextElement(this StringBuilder sb, HTMLHelper html)
        {
            sb.Append(GetTag(HTMLConstants.P, html.Text, html.ClassName));
            return sb;
        }
        /// <summary>
        /// Return part of HTML document for image block
        /// </summary>
        public static StringBuilder AddImageBlock(this StringBuilder sb, string imagePath, string className)
        {
            if (!string.IsNullOrEmpty(imagePath) && !string.IsNullOrEmpty(className))
            {
                sb.Append($@"<div class='{className}'>
                                <img src='https://localhost:44325/{imagePath}' 
                                style='width:100%;height:auto;align-items:center;'/>
                           </div>");
            }
            return sb;
        }
        /// <summary>
        /// Return part of HTML Document for simple text block
        /// <result>
        ///     <div class="classNameDIV">
        ///         <p class="classNameP">{text}</p>
        ///     </div>
        /// </result>
        /// </summary>
        public static StringBuilder AddTextBlock(this StringBuilder sb, HTMLHelper html, string className = null)
        {
            var text = GetTag(HTMLConstants.P, html.Text, html.ClassName);
            sb.Append(GetTag(HTMLConstants.DIV, text, className));
            return sb;
        }
        /// <summary>
        /// Return part of HTML Document for array of simple  text block
        /// <result>
        ///     <div class="classNameDIV">
        ///         <p class="classNameP1">{text}</p>
        ///         ...............................
        ///         <p class="classNamePN">{text}</p>
        ///     </div>
        /// </result>
        /// </summary>
        public static StringBuilder AddArrayOfTextBlock(this StringBuilder sb, HTMLHelper[] items, string className = null)
        {
            sb.AddOpenDIVElement(className);
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
        ///     <T-Tag class="T-ClassName">{text}</T-Tag>
        /// </result>
        /// </summary>
        private static string GetTag(string tag, string text, string className = null)
        {
            return GetOpenTag(tag, className) + $"{text}" + GetCloseTag(tag);
        }
        private static string GetOpenTag(string tag, string className = null)
        {
            return $"<{tag} {(!string.IsNullOrEmpty(className) ? $"class='{className}'" : string.Empty)}>";
        }
        private static string GetCloseTag(string tag)
        {
            return $"</{tag}>";
        }
    }
}
