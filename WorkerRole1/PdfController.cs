using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Text;
//using TuesPechkin;
using System.Drawing.Printing;
using OpenHtmlToPdf;


namespace WorkerRole1
{
    public class PdfController : ApiController
    {
        //[HttpPost]
        //public HttpResponseMessage GeneratePDF(/*PdfViewModel viewModel*/ int id)
        //{
        //    var document = new HtmlToPdfDocument
        //    {
        //        GlobalSettings =  {
        //                                ProduceOutline = true,
        //                                DocumentTitle = "Pretty Websites",
        //                                PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
        //                                Margins =
        //                                {
        //                                    All = 1.375,
        //                                    Unit = Unit.Centimeters
        //                                }
        //        },
        //        Objects = {
        //                        new ObjectSettings { HtmlText = "<h1>Pretty Websites</h1><p>This might take a bit to convert!</p>" },
        //                        new ObjectSettings { PageUrl = "www.google.com" }
        //        }
        //    };

        //    var obj = new ObjectSettings();
        //    obj.LoadSettings.PostItems.Add
        //        (
        //            new PostItem()
        //            {
        //                Name = "paramName",
        //                Value = "paramValue"
        //            }
        //        );

        //    IConverter converter = new ThreadSafeConverter(
        //                                new RemotingToolset<PdfToolset>(
        //                                     new Win64EmbeddedDeployment(
        //                                            new TempFolderDeployment())));

        //    byte[] result = converter.Convert(document);
        //    MemoryStream ms = new MemoryStream(result);

        //    HttpContent content = new StreamContent(ms);
        //    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        //    content.Headers.Add("content-disposition", "attachment;filename=myFile.pdf");

        //    return new HttpResponseMessage()
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Content = content
        //    };
        //}

        [HttpPost]
        public HttpResponseMessage GeneratePDF([FromBody] HtmlDto dto)
        {
            //string inputFilename = "cert-simple2.html";
            //string outputFilename = "cert-simple2.pdf";

            //string html = Encoding.UTF8.GetString(File.ReadAllBytes(inputFilename));

            //byte[] pdf = new SimplePechkin(new GlobalConfig()).Convert(html);

            //string htmlDecoded = HttpUtility.HtmlDecode(dto.html);
            


            var pdfBuilder = Pdf.From(HttpUtility.HtmlDecode(dto.html));


            //    pdfBuilder = pdfBuilder.WithObjectSetting("header.htmlUrl", dto.footerPath);

            if (!String.IsNullOrEmpty(dto.footerPath))
                pdfBuilder = pdfBuilder.WithGlobalSetting("margin.top", "2cm")
                                   .WithGlobalSetting("margin.bottom", "2cm")
                                   //.WithObjectSetting("footer.center","THE FOOTER")
                                   .WithObjectSetting("footer.htmlUrl", dto.footerPath);

            var pdf = pdfBuilder.Content();

            //HttpContent content = new StreamContent();
            ByteArrayContent content = new ByteArrayContent(pdf);
            
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            content.Headers.Add("content-disposition", "attachment;filename=myFile.pdf");

            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = content
            };
        }

        //[HttpPost]
        //public HttpResponseMessage Get1(HtmlDto dto)
        //{
        //    string html = dto?.html;

        //    string msg = String.Format($"Hello! Your string was: {html}");
        //    return new HttpResponseMessage()
        //    {
        //        Content = new StringContent(msg)
        //    };
        //}

        //[HttpPost]
        //public HttpResponseMessage Get2([FromBody] HtmlDto dto)
        //{
        //    string html = dto?.html;

        //    string msg = String.Format($"Hello! Your string was: {html}");
        //    return new HttpResponseMessage()
        //    {
        //        Content = new StringContent(msg)
        //    };
        //}
        //[Route("Pdf/Get3")]
        //[HttpPost]
        //public HttpResponseMessage Get3([FromBody] HtmlDto dto)
        //{
        //    string html = dto?.html;            

        //    if (String.IsNullOrEmpty(html))
        //    {
        //        html = "Nothing!";
        //    }

        //    string msg = String.Format($"Hello! Your string was: {html}");
        //    return new HttpResponseMessage()
        //    {
        //        Content = new StringContent(msg)
        //    };
        //}

        public class HtmlDto
        {
            public HtmlDto() { }

            public string html { get; set; }

            public string footerPath { get; set; }
        }
    }
}