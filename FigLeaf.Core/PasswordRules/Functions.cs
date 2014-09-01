using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FigLeaf.Core.PasswordRules
{
	public static class Functions
	{
		public static Func<string, string, string> GetBinaryFunction(string functionName)
		{
			switch (functionName)
			{
				case "Add": return Add;
				case "Left": return Left;
				case "Right": return Right;
				default: throw new ApplicationException("Invalid binary function name: " + functionName);
			}
		}

		public static Func<string, string> GetUnaryFunction(string functionName)
		{
			switch (functionName)
			{
				case "Reverse": return Reverse;
				case "Upper": return Upper;
				case "Lower": return Lower;
				case "Digits": return Digits;
				case "RemoveFileExtension": return RemoveFileExtension;
				case "FileExtension": return FileExtension;
				case "Len": return Len;
				default: throw new ApplicationException("Invalid unary function name: " + functionName);
			}
		}

		public static string Add(string arg1, string arg2)
		{
			return arg1 + arg2;
		}

		public static string Left(string arg1, string arg2)
		{
			if (string.IsNullOrEmpty(arg1))
				return arg1;

			int len = int.Parse(arg2);
			if (arg1.Length <= len)
				return arg1;

			return arg1.Substring(0, len);
		}

		public static string Right(string arg1, string arg2)
		{
			if (string.IsNullOrEmpty(arg1))
				return arg1;

			int len = int.Parse(arg2);
			if (arg1.Length <= len)
				return arg1;

			return arg1.Substring(arg1.Length - len, len);
		}

		public static string Reverse(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return arg;

			return new string(arg.Reverse().ToArray());
		}

		public static string Upper(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return arg;

			return arg.ToUpperInvariant();
		}

		public static string Lower(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return arg;

			return arg.ToLowerInvariant();
		}

		public static string Digits(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return arg;

			var sbResult = new StringBuilder();

			foreach (char c in arg)
				if (Char.IsDigit(c))
					sbResult.Append(c);

			return sbResult.ToString();
		}

		public static string RemoveFileExtension(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return arg;

			return System.IO.Path.GetFileNameWithoutExtension(arg);
		}

		public static string FileExtension(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return arg;

			string result = System.IO.Path.GetExtension(arg);
			if (result != null && result.Length > 1)
				result = result.Substring(1); // remove '.'

			return result;
		}

		public static string Len(string arg)
		{
			if (string.IsNullOrEmpty(arg))
				return "0";

			return arg.Length.ToString(CultureInfo.InvariantCulture);
		}
	}
}
