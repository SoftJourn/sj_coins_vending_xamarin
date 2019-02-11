﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Softjourn.SJCoins.Core.API.Model;
using Softjourn.SJCoins.Core.Exceptions;
using Softjourn.SJCoins.Core.Utils;

namespace Softjourn.SJCoins.Core.Managers
{
    public sealed class QrManager
    {
        /// <summary>
        /// Starts scanning and converts scanned result to Cash Object
        /// </summary>
        /// <returns></returns>
        public async Task<Cash> GetCodeFromQr()
        {
            var result = await ScanPhoto();

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
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts Cash Object to String
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string ConvertCashObjectToString(Cash amount)
        {
            return JsonConvert.SerializeObject(amount);
        }

        /// <summary>
        /// Scan QRCode using ZXing
        /// </summary>
        /// <returns></returns>
        private static async Task<ZXing.Result> ScanPhoto()
        {
            await PermissionsUtils.CheckCameraPermissiomAsync();

            try
            {
                var scanner = new ZXing.Mobile.MobileBarcodeScanner();
                var result = await scanner.Scan();

                return result;
            }
            catch (ZXing.ReaderException)
            {
            }

            return default(ZXing.Result);
        }
    }
}
