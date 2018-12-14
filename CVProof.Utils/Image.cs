using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CVProof.Utils
{
    public static class ImageHelper
    {
        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };              // PNG
            var tiff = new byte[] { 73, 73, 42 };                  // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };                 // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 };          // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };         // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }

        public static bool isKnownFormat(byte[] imageFile)
        {
            if (imageFile == null)
                return false;

            return GetImageFormat(imageFile) != ImageFormat.unknown;
        }

        private static System.Drawing.Image GetImage(byte[] imageFile)
        {
            System.Drawing.Image ret = null;

            using (var stream = new System.IO.MemoryStream(imageFile))
            {
                try
                {
                    ret = System.Drawing.Image.FromStream(stream);
                }
                catch(Exception e)
                {
                }
            }

            return ret;
        }

        public static int GetImageWidth(byte[] imageFile)
        {
            return GetImage(imageFile)?.Width ?? 0;
        }
        public static int GetImageHeight(byte[] imageFile)
        {
            return GetImage(imageFile)?.Height ?? 0;
        }

        public static bool IsImageFit(byte[] imageFile, int px)
        {
            bool ret = false;

            System.Drawing.Image image = GetImage(imageFile);

            if (image != null)
            {
                ret = image.Height > 0
                    && image.Width > 0
                    && image.Height <= px
                    && image.Width <= px;
            }

            return ret;
        }

    }
}
