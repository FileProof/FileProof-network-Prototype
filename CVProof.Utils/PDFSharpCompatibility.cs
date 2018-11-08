using System;
using System.Collections.Generic;
using System.Text;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.IO;
using iText.Forms.Util;
using iText.Pdfa;
using iText.Kernel;
using iText.Layout;


namespace CVProof.Utils
{
    public static class PDFSharpCompatibility
    {
        public static PdfDocument CompatibleOpen(MemoryStream inputStream, PdfDocumentOpenMode openMode)
        {
            PdfDocument pdfDocument = null;
            inputStream.Position = 0;

            try
            {
                pdfDocument = PdfReader.Open(inputStream, openMode);
            }
            catch (PdfSharp.Pdf.IO.PdfReaderException)
            {
                inputStream.Position = 0;
                MemoryStream outputStream = new MemoryStream();

                iText.Kernel.Pdf.WriterProperties writerProperties = new iText.Kernel.Pdf.WriterProperties();
                writerProperties.SetPdfVersion(iText.Kernel.Pdf.PdfVersion.PDF_1_4);

                iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader(inputStream);

                iText.Kernel.Pdf.PdfDocument pdfStamper = new iText.Kernel.Pdf.PdfDocument(pdfReader, new iText.Kernel.Pdf.PdfWriter(outputStream, writerProperties));

                iText.Forms.PdfAcroForm pdfForm = iText.Forms.PdfAcroForm.GetAcroForm(pdfStamper, true);
                if (pdfForm != null)
                {
                    pdfForm.FlattenFields();
                }
                writerProperties.SetFullCompressionMode(false);

                pdfStamper.GetWriter().SetCloseStream(false);
                pdfStamper.Close();

                pdfDocument = PdfReader.Open(outputStream, openMode);
            }
            return pdfDocument;
        }
    }
}
