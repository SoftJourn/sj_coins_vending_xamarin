using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Exceptions;
using UIKit;
using ZXing;
using ZXing.Mobile;
using ZXing.QrCode;

namespace Softjourn.SJCoins.iOS.General.Helper
{
	public class QRCodeHelper
	{
		//Converts Scan result to Cash object
		public Cash ConvertScanResult(ZXing.Result result)
		{
			try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Cash>(result.Text);
            }
            catch (JsonReaderException e)
            {
				throw new JsonReaderExceptionCustom(e.Message);
			}
		}

		public UIImage GenerateQRImage(string content, int width, int height)
		{
			var barcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = ZXing.BarcodeFormat.QR_CODE,
				Options = new ZXing.Common.EncodingOptions
				{
					Width = width,
					Height = height
				}
			};
			return barcodeWriter.Write(content);
		}
	}
}
