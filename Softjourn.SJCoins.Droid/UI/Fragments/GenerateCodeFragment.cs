using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Softjourn.SJCoins.Droid.Services;
using Softjourn.SJCoins.Droid.UI.Activities;
using Environment = Android.OS.Environment;
using File = Java.IO.File;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class GenerateCodeFragment : Fragment
    {
        private ImageView _imageForQrCode;
        private EditText _inputAmount;
        private Button _buttonGenerate;
        private const string ShareType = "image/jpeg";
        private const string ShareFragmentTitle = "Share Image";
        private bool IsShareScreenActive;

        private Bitmap _bitmap;
        private AlertService _alertService;

        public static GenerateCodeFragment NewInstance() => new GenerateCodeFragment();

        #region Fragment Methods

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
            inflater.Inflate(Resource.Layout.fragment_generate_barcode, container, false);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _imageForQrCode = view.FindViewById<ImageView>(Resource.Id.qr_code_image);
            _imageForQrCode.Click += (sender, e) =>
            {
                ShareCode();
            };
            _inputAmount = view.FindViewById<EditText>(Resource.Id.input_amount);
            _buttonGenerate = view.FindViewById<Button>(Resource.Id.btn_generate);
            _buttonGenerate.Click += (sender, e) =>
            {
                GetQrCode(_inputAmount.Text);
                _inputAmount.ClearFocus();
                var imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(_inputAmount.WindowToken, 0);
            };

            _alertService = new AlertService();
        }

        public override void OnResume()
        {
            base.OnResume();
            IsShareScreenActive = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Sets Error for EditText Field
        /// </summary>
        /// <param name="amount"></param>
        public void ShowEditFieldError(string amount)
        {
            _inputAmount.RequestFocus();
            _inputAmount.SetError(amount, null);
        }

        /// <summary>
        /// Generating of QR code based on the
        /// string taken from Api call
        /// </summary>
        /// <param name="image"></param>
        public void ShowImageCode(string image)
        {
            var barcodeWriter = new ZXing.Mobile.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 700,
                    Height = 700
                }
            };
            var barcode = barcodeWriter.Write(image);

            AddImageToGallery(barcode);

            _imageForQrCode.Visibility = ViewStates.Visible;
            _imageForQrCode.SetImageBitmap(barcode);

            _bitmap = barcode;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Calls Activity method to get string needed for generating of QRCode
        /// </summary>
        /// <param name="amount"></param>
        private void GetQrCode(string amount) => ((QrActivity)Activity).GenerateCode(amount);

        /// <summary>
        /// Starts Activity for choosing method of sharing an image of QRCode
        /// </summary>
        private void ShareCode()
        {
            if (IsShareScreenActive) return;

            IsShareScreenActive = true;
            var path = MediaStore.Images.Media.InsertImage(Activity.ContentResolver, _bitmap, "MoneyCode", null);
            var uri = Android.Net.Uri.Parse(path);
            var share = new Intent(Intent.ActionSend);
            share.SetType(ShareType);
            share.PutExtra(Intent.ExtraStream, uri);
            Activity.StartActivity(Intent.CreateChooser(share, ShareFragmentTitle));
        }

        private void AddImageToGallery(Bitmap bitmap)
        {
            var filePath = Environment.ExternalStorageDirectory.AbsolutePath;
            var dir = new File($"{filePath}/SJCoins/");
            if (!dir.Exists()) dir.Mkdirs();

            var imageFile = System.IO.Path.Combine(dir.Path, $"OfflineMoney_{DateTime.Now.Ticks}.png");
            var stream = new FileStream(imageFile, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();

            _alertService.ShowMessageWithUserInteraction(string.Empty, $"QrCode was saved to {dir.Path}", string.Empty, null);
        }

        #endregion
    }
}