using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Softjourn.SJCoins.Droid.UI.Activities;
using ZXing;

namespace Softjourn.SJCoins.Droid.UI.Fragments
{
    public class GenerateCodeFragment : Fragment
    {
        private ImageView _imageForQrCode;
        private EditText _inputAmount;
        private Button _buttonGenerate;

        public static GenerateCodeFragment NewInstance()
        {
            var fragment = new GenerateCodeFragment();
            return fragment;
        }

        public GenerateCodeFragment()
        {

        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_generate_barcode, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _imageForQrCode = view.FindViewById<ImageView>(Resource.Id.qr_code_image);
            _inputAmount = view.FindViewById<EditText>(Resource.Id.input_amount);
            _buttonGenerate = view.FindViewById<Button>(Resource.Id.btn_generate);
            _buttonGenerate.Click += (sender, e) =>
            {
                GetQrCode(_inputAmount.Text);
            };
        }

        private void GetQrCode(string amount)
        {
            ((QrActivity)Activity).GenerateCode(amount);
        }

        public void ShowEditFieldError(string amount)
        {
            _inputAmount.RequestFocus();
            _inputAmount.SetError(amount,null);
        }

        public void ShowImageCode(string image)
        {
            //var bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            //var stream = new MemoryStream();
            //bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);  // this is the diff between iOS and Android
            //stream.Position = 0;

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 600,
                    Width = 600
                }
            };
            var bmp = writer.Write(image);
            _imageForQrCode.Visibility = ViewStates.Visible;
            _imageForQrCode.SetImageBitmap(BitmapFactory.DecodeByteArray(bmp, 0, bmp.Length));
            
        }
    }
}