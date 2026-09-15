using System;
using System.Text;

namespace LazyBearTechnology
{
	// Token: 0x02000108 RID: 264
	public static class CjkWrapper
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x0001C197 File Offset: 0x0001A397
		private static bool IsSmallKana(char c)
		{
			return "ぁぃぅぇぉっゃゅょゎゕゖァィゥェォッャュョヮヵヶ".IndexOf(c) >= 0;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001C1AA File Offset: 0x0001A3AA
		private static bool IsNoStartOther(char c)
		{
			return "、。，．,.：；:;！？!?・）］｝〕〉》」』】〙〗”’ー〜～ヽヾゝゞ々〆〇〵\u309b\u309c".IndexOf(c) >= 0;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001C1BD File Offset: 0x0001A3BD
		private static bool IsNoEnd(char c)
		{
			return "（［｛〔〈《「『【〘〖“‘".IndexOf(c) >= 0;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001C1D0 File Offset: 0x0001A3D0
		public static string Wrap(string text, int maxWidth, SmallKanaKinsoku smallKana = SmallKanaKinsoku.Oidashi)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text ?? string.Empty;
			}
			if (maxWidth < 1)
			{
				maxWidth = 1;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = text.Replace("\r\n", "\n").Replace("\r", "\n").Split('\n', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append('\n');
				}
				stringBuilder.Append(CjkWrapper.WrapParagraph(array[i], maxWidth, smallKana));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001C258 File Offset: 0x0001A458
		private static string WrapParagraph(string text, int maxWidth, SmallKanaKinsoku smallKana)
		{
			if (text.Length == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length + 8);
			int num = 0;
			int num2 = 0;
			foreach (char c in text)
			{
				int characterWidth = CjkWrapper.GetCharacterWidth(c);
				if (num2 + characterWidth <= maxWidth || stringBuilder.Length <= num)
				{
					stringBuilder.Append(c);
					num2 += characterWidth;
				}
				else if (CjkWrapper.IsNoStartOther(c) || (CjkWrapper.IsSmallKana(c) && smallKana == SmallKanaKinsoku.Oikomi))
				{
					stringBuilder.Append(c);
					num2 += characterWidth;
				}
				else
				{
					int num3 = ((CjkWrapper.IsSmallKana(c) && smallKana == SmallKanaKinsoku.Oidashi) ? 1 : 0);
					int num4 = CjkWrapper.ComputeCarry(stringBuilder, num, num3, smallKana);
					if (num4 < 0)
					{
						stringBuilder.Append(c);
						num2 += characterWidth;
					}
					else
					{
						string text2 = ((num4 > 0) ? stringBuilder.ToString(stringBuilder.Length - num4, num4) : string.Empty);
						stringBuilder.Length -= num4;
						stringBuilder.Append('\n');
						num = stringBuilder.Length;
						stringBuilder.Append(text2);
						stringBuilder.Append(c);
						num2 = CjkWrapper.MeasureWidth(text2) + characterWidth;
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001C390 File Offset: 0x0001A590
		private static int ComputeCarry(StringBuilder sb, int lineStart, int minCarry, SmallKanaKinsoku smallKana)
		{
			int num = sb.Length - lineStart;
			int i = minCarry;
			if (i > num)
			{
				return -1;
			}
			bool flag = smallKana == SmallKanaKinsoku.Oidashi;
			bool flag2 = true;
			IL_0077:
			while (flag2)
			{
				flag2 = false;
				while (i < num)
				{
					char c = sb[sb.Length - i - 1];
					if (!CjkWrapper.IsNoEnd(c) && (!flag || !CjkWrapper.IsSmallKana(c)))
					{
						IL_0057:
						while (flag && i > 0 && i < num && CjkWrapper.IsSmallKana(sb[sb.Length - i]))
						{
							i++;
							flag2 = true;
						}
						goto IL_0077;
					}
					i++;
					flag2 = true;
				}
				goto IL_0057;
			}
			if (i >= num)
			{
				return -1;
			}
			return i;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001C420 File Offset: 0x0001A620
		private static int MeasureWidth(string s)
		{
			int num = 0;
			foreach (char c in s)
			{
				num += CjkWrapper.GetCharacterWidth(c);
			}
			return num;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001C454 File Offset: 0x0001A654
		private static int GetCharacterWidth(char c)
		{
			if ((c >= 'ᄀ' && c <= 'ᅟ') || (c >= '⺀' && c <= '〾') || (c >= 'ぁ' && c <= '㏿') || (c >= '㐀' && c <= '䶿') || (c >= '一' && c <= '鿿') || (c >= 'ꀀ' && c <= '\ua4cf') || (c >= '가' && c <= '힣') || (c >= '豈' && c <= '\ufaff') || (c >= '︰' && c <= '\ufe4f') || (c >= '\uff00' && c <= '｠') || (c >= '￠' && c <= '￦'))
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x0400026E RID: 622
		private const string SmallKana = "ぁぃぅぇぉっゃゅょゎゕゖァィゥェォッャュョヮヵヶ";

		// Token: 0x0400026F RID: 623
		private const string NoStartOther = "、。，．,.：；:;！？!?・）］｝〕〉》」』】〙〗”’ー〜～ヽヾゝゞ々〆〇〵\u309b\u309c";

		// Token: 0x04000270 RID: 624
		private const string NoEnd = "（［｛〔〈《「『【〘〖“‘";
	}
}
