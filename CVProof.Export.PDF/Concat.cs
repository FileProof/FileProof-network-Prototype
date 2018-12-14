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
