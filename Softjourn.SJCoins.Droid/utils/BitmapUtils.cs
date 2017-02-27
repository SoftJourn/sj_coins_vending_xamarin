using System;
using System.Collections.Generic;
using System.Linq;

using Android.Graphics;

namespace Softjourn.SJCoins.Droid.Utils
{
    public class BitmapUtils
    {
        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            var height = options.OutHeight;
            var width = options.OutWidth;
            var inSampleSize = 1;

            if (height <= reqHeight && width <= reqWidth) return inSampleSize;

            var halfHeight = height / 2;
            var halfWidth = width / 2;

            while ((halfHeight / inSampleSize) > reqHeight
                   && (halfWidth / inSampleSize) > reqWidth)
            {
                inSampleSize *= 2;
            }
            return inSampleSize;
        }
    }
}