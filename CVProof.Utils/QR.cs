using System.IO;
using QRCoder;
using System.Drawing;

namespace CVProof.Utils
{
    public static class QR
    {

        public static Bitmap GetQRCode(string link)
        {
            Bitmap ret = null;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            ret = qrCode.GetGraphic(20);

            return ret;
        }

        public static string GetBase64Code(string link)
        {
            System.Drawing.Image img = GetQRCode(link);
            byte[] bytes = null;

            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                bytes = stream.ToArray();
            }

            return System.Convert.ToBase64String(bytes);
        }

        public static string GetPureBase64(string link)
        {

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            return qrCode.GetGraphic(4);
        }
    }
}
