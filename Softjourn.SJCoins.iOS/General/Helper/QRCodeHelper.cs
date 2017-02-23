using System;
using Softjourn.SJCoins.Core.API.Model;

namespace Softjourn.SJCoins.iOS.General.Helper
{
	public class QRCodeHelper
	{
		//Converts Scan result to Cash object 
		public Cash ConvertScanResult(ZXing.Result result) => Newtonsoft.Json.JsonConvert.DeserializeObject<Cash>(result.Text);
	}
}
