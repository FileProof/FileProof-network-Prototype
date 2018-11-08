using System;
using System.Diagnostics;
using System.Collections;
using System.Globalization;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;
using CVProof.Utils;

namespace CVProof.Export.PDF
{
    public class Concat
    {
        public static string[] GetFiles()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            FileInfo[] fileInfos = dirInfo.GetFiles("?.pdf");
            ArrayList list = new ArrayList();
            foreach (FileInfo info in fileInfos)
            {
                list.Add(info.FullName);
            }
            return (string[])list.ToArray(typeof(string));
            //DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            //FileInfo[] fileInfos = dirInfo.GetFiles("*.pdf");
        }

        /// <summary>
        /// Imports all pages from a list of documents.
        /// </summary>
        public static void Variant1()
        {
            // Get some file names
            string[] files = GetFiles();

            // Open the output document
            PdfDocument outputDocument = new PdfDocument();

            // Iterate files
            foreach (string file in files)
            {
                // Open the document to import pages from it.
                PdfDocument inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

                // Iterate pages
                int count = inputDocument.PageCount;
                for (int idx = 0; idx < count; idx++)
                {
                    // Get the page from the external document...
                    PdfPage page = inputDocument.Pages[idx];
                    // ...and add it to the output document.
                    outputDocument.AddPage(page);
                }
            }

            // Save the document...
            string filename = "SimpleConcat.pdf";
            outputDocument.Save(filename);
            // ...and start a viewer.
            var p = new Process();

            p.StartInfo = new ProcessStartInfo(Path.Combine(Directory.GetCurrentDirectory(), filename))
            {
                UseShellExecute = true
            };

            p.Start();
        }

        public static byte[] DoConcat(byte[] file1, byte[] file2)
        {
            byte[] ret;

            PdfDocument outputDocument = new PdfDocument();
            PdfDocument inputDocument1 = new PdfDocument();
            PdfDocument inputDocument2 = new PdfDocument();

            using (MemoryStream stream1 = new MemoryStream(file1))
            using (MemoryStream stream2 = new MemoryStream(file2))
            {
                inputDocument1 = PDFSharpCompatibility.CompatibleOpen(stream1, PdfDocumentOpenMode.Import);
                inputDocument2 = PDFSharpCompatibility.CompatibleOpen(stream2, PdfDocumentOpenMode.Import);
            }

            int count = inputDocument1.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfPage page = inputDocument1.Pages[idx];
                outputDocument.AddPage(page);
            }

            count = inputDocument2.PageCount;
            for (int idx = 0; idx < count; idx++)
            {
                PdfPage page = inputDocument2.Pages[idx];
                outputDocument.AddPage(page);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                outputDocument.Save(ms, false);

                ret = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Flush();
                ms.Read(ret, 0, (int)ms.Length);

            }
            return ret;
        }
    }
}
