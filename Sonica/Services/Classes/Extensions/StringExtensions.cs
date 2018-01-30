using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Services.Classes.Extensions
{
	public static class StringExtensions
	{
		internal static string StripFromChar(this string Value, char StripFrom)
		{
			if (Value.Contains(StripFrom))
			{
				Value = Value.Substring(0, Value.IndexOf(StripFrom));
			}
			return Value;
		}

		internal static string Remove(this string Value, params string[] What)
		{
			foreach (string s in What)
			{
				Value = Value.Replace(s, string.Empty);
			}
			return Value;
		}

		internal static string KeepWDOnly(this string Value)
		{
			return Regex.Replace(Value, @"[^\w\d]", string.Empty);
		}

		internal static string[] CleanSplit(this string Value, params string[] Separators)
		{
			return Value.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
		}

		internal static string FileNameSafe(this string Value)
		{
			return string.Join("_", Value.Split(Path.GetInvalidFileNameChars()));
		}

		public static string Base64Encode(this string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(this string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

	}
}