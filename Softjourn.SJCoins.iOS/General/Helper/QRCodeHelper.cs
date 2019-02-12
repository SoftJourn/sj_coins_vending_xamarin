using Newtonsoft.Json;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Exceptions;
using UIKit;
using ZXing;

namespace Softjourn.SJCoins.iOS.General.Helper
{
	public class QRCodeHelper
	{
		//Converts Scan result to Cash object
		public Cash ConvertScanResult(Result result)
		{
			try
            {
                return JsonConvert.DeserializeObject<Cash>(result.Text);
            }
            catch (JsonReaderException e)
            {
				throw new JsonReaderExceptionCustom(e.Message);
			}
			catch (JsonSerializationException e)
            {
				throw new JsonReaderExceptionCustom(e.Message);
			}
		}

		public UIImage GenerateQRImage(string content, int width, int height)
		{
			var barcodeWriter = new ZXing.Mobile.BarcodeWriter
			{
				Format = BarcodeFormat.QR_CODE,
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
