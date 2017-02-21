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
using Java.Lang;
using Softjourn.SJCoins.Droid.UI.Activities;
using ZXing;
using ZXing.Common;

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

            _imageForQrCode.Visibility = ViewStates.Visible;
            _imageForQrCode.SetImageBitmap(barcode);
        }
    }
}