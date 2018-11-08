using System;
using System.Collections.Generic;
using System.Text;
using OpenHtmlToPdf;
using System.IO;
using System.Diagnostics;



namespace CVProof.Export.PDF
{
    public class Html2Pdf
    {
        public static byte[] Variant1()
        {
            string inputFilename = "cert-simple2.html";
            string outputFilename = "cert-simple2.pdf";

            string html = Encoding.UTF8.GetString(File.ReadAllBytes(inputFilename));

            //byte[] pdf = new SimplePechkin(new GlobalConfig()).Convert(html);

            
            var pdf = Pdf
                .From(html)                
                .Content();
            //.OfSize(PaperSize.A4)
            //.WithTitle("Title")
            //.WithoutOutline()
            //.WithMargins(1.25.Centimeters())
            //.Portrait()
            //.Comressed()                        
            //.Content();

            //File.WriteAllBytes(outputFilename, pdf);

            //var p = new Process();

            //p.StartInfo = new ProcessStartInfo(Path.Combine(Directory.GetCurrentDirectory(), outputFilename))
            //{
            //    UseShellExecute = true
            //};

            //p.Start();

            return pdf;
        }

    }
}
