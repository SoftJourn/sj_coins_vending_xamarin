using System;
namespace Softjourn.SJCoins.iOS.General.Extensions
{
	public static class VerifyingExtensions
	{
		public static bool IsNullOrEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}
	}
}
