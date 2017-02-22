using System;
using System.Threading.Tasks;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.Managers
{
    public class QrManager
    {

        //Starts scanning and converts scanned result to Cash Object
        public async Task<Cash> GetCodeFromQr()
        {
            var result = await ScanPhoto();
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Cash>(result.Text);
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        //Converts Cash Object to String
        public string ConvertCashObjectToString(Cash amount)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(amount);
        }

        //Scan QRCode using ZXing
        private async Task<ZXing.Result> ScanPhoto()
        {
            await PermissionsUtils.CheckCameraPermissiomAsync();

            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
            var result = await scanner.Scan();

            return result;
        }
    }
}
