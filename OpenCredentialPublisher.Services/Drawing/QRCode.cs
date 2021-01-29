using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenCredentialPublisher.Services.Drawing
{
    public class QRCodeUtility
    {
        public static byte[] Create(string url)
        {
            using var generator = new QRCodeGenerator();
            var codeData = generator.CreateQrCode($"{url}", QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(codeData);
            var qrCodeBytes = qrCode.GetGraphic(5);
            return qrCodeBytes;
        }
    }
}
