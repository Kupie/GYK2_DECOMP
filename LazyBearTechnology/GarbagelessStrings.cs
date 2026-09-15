using System;
using System.Text;

namespace LazyBearTechnology
{
	// Token: 0x02000178 RID: 376
	public static class GarbagelessStrings
	{
		// Token: 0x06000833 RID: 2099 RVA: 0x000290A0 File Offset: 0x000272A0
		public static void IntToCharsWithLeadingZeros(int value, ref char[] chars, int signChars, int startPos)
		{
			int num = startPos + signChars - 1;
			for (int i = signChars; i > 0; i--)
			{
				chars[num] = (char)(48 + value % 10);
				value /= 10;
				num--;
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000290D8 File Offset: 0x000272D8
		public static void StringToChars(ref string text, ref char[] chars)
		{
			int length = text.Length;
			for (int i = 0; i < length; i++)
			{
				chars[i] = text[i];
			}
			chars[length] = '\0';
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0002910C File Offset: 0x0002730C
		public static string CharsToString(ref char[] chars)
		{
			GarbagelessStrings.stringBuilder.Length = 0;
			foreach (char c in chars)
			{
				if (c == '\0')
				{
					break;
				}
				GarbagelessStrings.stringBuilder.Append(c);
			}
			return GarbagelessStrings.stringBuilder.ToString();
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00029154 File Offset: 0x00027354
		public static int GetHashCode(ref char[] chars)
		{
			int num = 5381;
			int num2 = num;
			int num3 = 0;
			while (num3 < chars.Length && chars[num3] != '\0')
			{
				num = ((num << 5) + num) ^ (int)chars[num3];
				if (num3 == chars.Length - 1 || chars[num3 + 1] == '\0')
				{
					break;
				}
				num2 = ((num2 << 5) + num2) ^ (int)chars[num3 + 1];
				num3 += 2;
			}
			return num + num2 * 1566083941;
		}

		// Token: 0x0400052B RID: 1323
		private static StringBuilder stringBuilder = new StringBuilder();
	}
}
