using HtmlWorkflow.Constants;
using HtmlWorkflow.Extensions;
using HtmlWorkflow.Models;
using PDFGenerator.Models;
using PDFGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDFGenerator.TemplateGeneratorBodyText
{
    public class CloseLotTextInfo
    {
        public string GetHTMLString(LotData lot, UserData futureOwnerLot)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HtmlConstants.OpenHTML)
              .Append(HtmlConstants.OpenHead)
              .Append(HtmlConstants.CloseHead)
              .Append(HtmlConstants.OpenBody);

            sb.AddArrayOfTextBlock(new HtmlHelper[]
            {
               new HtmlHelper { Text = "<h3>Congratulate you  with your new car</h3>" },
               new HtmlHelper { Text = $"<h5>{lot.NameLot}</h5>" }
            }, new HtmlDivHelper { ClassName = "header" });
            sb.AddArrayOfTextBlock(new HtmlHelper[]
            {
               new HtmlHelper { Text = $"Small descriptions: {lot.Description}" },
               new HtmlHelper { Text = $"Price to pay: {lot.CurrentPrice} $" }
            }, new HtmlDivHelper { ClassName = "description" });
          
            sb.AddTableElement(
                new List<HtmlHelper[]>
                {
                    new HtmlHelper[]
                    {
                        new HtmlHelper { Text = "Email to contact" },
                        new HtmlHelper { Text = $"{lot.User.Email}" }
                    },
                    new HtmlHelper[]
                    {
                        new HtmlHelper { Text = "Fullname owner" },
                        new HtmlHelper { Text = $"{lot.User.Name}  {lot.User.Surname}" }
                    },
                    new HtmlHelper[]
                    {
                        new HtmlHelper { Text = "Car Brand" },
                        new HtmlHelper { Text = $"{CarBrandUtils.GetCarBrandName(lot.CarBrand)}" }
                    }
                }, null, new HtmlHelper { ClassName = "table-info" });

            sb.AddImageBlock(
                new HtmlImgHelper
                {
                    ImagePath = $"https://localhost:44325/{lot.Image}",
                    Style = "width:100%;height:auto;align-items:center;"
                },
                new HtmlDivHelper
                {
                    ClassName = "image"
                });

            sb.AddTextBlock(
                new HtmlHelper { Text = DateTime.Now.Date.ToShortDateString() },
                new HtmlDivHelper { ClassName = "date-now-info" });
            sb.Append(HtmlConstants.CloseBody)
              .Append(HtmlConstants.CloseHTML);
            return sb.ToString();
        }
    }
}
