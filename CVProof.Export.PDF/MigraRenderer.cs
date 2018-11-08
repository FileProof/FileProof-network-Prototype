using System;
using System.Collections.Generic;
using System.Text;
using MigraDoc.Rendering;
using Microsoft.AspNetCore.Hosting.Internal;


namespace CVProof.Export.PDF
{
    public class MigraRenderer
    {
        public static string Render()
        {
            string filename = null;

            try
            {
                // Create an invoice form with the sample invoice data.
                var invoice = new MigraForm("D:\\invoice.xml");

                // Create the document using MigraDoc.
                var document = invoice.CreateDocument();
                document.UseCmykColor = true;

                #if DEBUG
                // For debugging only...
                MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
                var document2 = MigraDoc.DocumentObjectModel.IO.DdlReader.DocumentFromFile("MigraDoc.mdddl");
                //document = document2;
                // With PDFsharp 1.50 beta 3 there is a known problem: the blank before "by" gets lost while persisting as MDDDL.
#endif
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var enc1252 = Encoding.GetEncoding(1252);

                // Create a renderer for PDF that uses Unicode font encoding.
                var pdfRenderer = new PdfDocumentRenderer(true);

                // Set the MigraDoc document.
                pdfRenderer.Document = document;

                // Create the PDF document.
                pdfRenderer.RenderDocument();

                // Save the PDF document...
                filename = "Invoice.pdf";
                #if DEBUG
                // I don't want to close the document constantly...
                filename = "Invoice-" + Guid.NewGuid().ToString("N").ToUpper() + ".pdf";
                #endif
                pdfRenderer.Save(filename);
                // ...and start a viewer.

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return filename;
        }

    }
}
