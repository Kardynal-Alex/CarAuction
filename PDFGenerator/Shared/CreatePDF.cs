using DinkToPdf;
using DinkToPdf.Contracts;
using PDFGenerator.Models;
using PDFGenerator.TemplateGeneratorBodyText;

namespace PDFGenerator.Shared
{
    public class CreatePDF
    {
        private readonly IConverter converter;
        public CreatePDF(IConverter converter)
        {
            this.converter = converter;
        }
       
        public byte[] CloseLotCreatePDF(LotData lot, UserData futureOwnerLot)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
                //Out = @"D:\GeneratedPDF.pdf"
            };
            var htmlText = new CloseLotTextInfo();

            string stylePath = @"D:\Asp.Net(project)\CarAuction\PDFGenerator\assets\info.css";
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlText.GetHTMLString(lot, futureOwnerLot),
                //Page = "URL", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = stylePath },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Cars & Bids" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = converter.Convert(pdf);

            return file;
        }
    }
}
