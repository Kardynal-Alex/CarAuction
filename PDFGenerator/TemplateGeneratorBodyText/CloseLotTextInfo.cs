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
            text.Append($@"<html>
                            <head>
                            </head>
                            <body>");
            text.Append(@$"<div class='header' style='text-align:center;font-size:40px;'>
                                <h3>Congratulate you</h3>
                                <h5>{ lot.NameLot }</h5>
                            </div>
                            <div class='description'  style='font-size:30px;'>
                                <p>{ lot.Description }</p>
                                <p>Price to pay { lot.CurrentPrice } $</p>
                            </div>");
            text.Append($@"<div class='contacts'  style='font-size:25px;'>
                                <p>Former owner contact { lot.User.Email }</p>
                                <p>{ lot.User.Name }  { lot.User.Surname }</p>
                            </div>");
            text.Append($@"<div class='image'>
                                <img src='https://localhost:44325/{lot.Image}' 
                                style='width:100%;height:auto;align-items:center;'/>
                            </div>");
            text.Append($@"<div style='font-size:20px;font-weight:600;text-align:right;'>
                                <p>{ DateTime.Now.Date.ToShortDateString() }</p>
                            </div>");
            text.Append(@"  </body>
                          </html>");
            return text.ToString();
        }
    }
}
