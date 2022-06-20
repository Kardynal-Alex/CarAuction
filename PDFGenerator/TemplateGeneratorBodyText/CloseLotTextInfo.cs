using HtmlWorkflow.Constants;
using HtmlWorkflow.Extensions;
using HtmlWorkflow.Models;
using PDFGenerator.Models;
using System;
using System.Text;

namespace PDFGenerator.TemplateGeneratorBodyText
{
    /// <summary>
    /// Special class for generating contents of pdf
    /// </summary>
    public class CloseLotTextInfo
    {
        /// <summary>
        /// Geh html string of content pdf file
        /// </summary>
        /// <param name="lot"></param>
        /// <param name="futureOwnerLot"></param>
        /// <returns></returns>
        public string GetHTMLString(LotData lot, UserData futureOwnerLot)
        {
            StringBuilder text = new StringBuilder();
            text.Append(HTMLConstants.OpenHTML)
                .Append(HTMLConstants.OpenHead)
                .Append(HTMLConstants.CloseHead)
                .Append(HTMLConstants.OpenBody);

            text.AddArrayOfTextBlock(new HTMLHelper[]
            {
               new HTMLHelper { Text = "<h3>Congratulate you  with your new car</h3>" },
               new HTMLHelper { Text = $"<h5>{lot.NameLot}</h5>" }
            }, "header");
            text.AddArrayOfTextBlock(new HTMLHelper[]
            {
               new HTMLHelper { Text = $"Small descriptions: {lot.Description}" },
               new HTMLHelper { Text = $"Price to pay: {lot.CurrentPrice} $" }
            }, "description");
            text.AddArrayOfTextBlock(new HTMLHelper[]
            {
                new HTMLHelper { Text = $"Former owner contacts:", ClassName = "owner-contact" },
                new HTMLHelper { Text = $"~ Email: {lot.User.Email}" },
                new HTMLHelper { Text = $"~ FullName: {lot.User.Name}  {lot.User.Surname}" }
            }, "contacts");

            text.AddImageBlock(lot.Image, "image");

            text.AddTextBlock(new HTMLHelper
            {
                Text = DateTime.Now.Date.ToShortDateString()
            }, "date-now-info");
            text.Append(HTMLConstants.CloseBody)
                .Append(HTMLConstants.CloseHTML);
            return text.ToString();
        }
    }
}
