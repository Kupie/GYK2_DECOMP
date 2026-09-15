using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LazyBearTechnology
{
	// Token: 0x02000C49 RID: 3145
	public static class ArabicShaper
	{
		// Token: 0x0600501F RID: 20511 RVA: 0x00179DE8 File Offset: 0x00177FE8
		public static string ShapeForRender(string text, bool keepTashkeel = true)
		{
			string text2 = ArabicShaper.Shape(text);
			if (!keepTashkeel)
			{
				text2 = ArabicShaper.RemoveTashkeel(text2);
			}
			return ArabicShaper.FixDirectionalRuns(text2);
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x00179E0C File Offset: 0x0017800C
		public static string RemoveTashkeel(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				if (ArabicShaper.IsTashkeel(text[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			for (int j = 0; j < text.Length; j++)
			{
				if (!ArabicShaper.IsTashkeel(text[j]))
				{
					stringBuilder.Append(text[j]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x00179E8C File Offset: 0x0017808C
		private static bool IsTashkeel(char c)
		{
			return (c >= '\u064b' && c <= '\u0652') || c == '\u0670';
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x00179EA8 File Offset: 0x001780A8
		public static string Shape(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			bool flag = false;
			foreach (char c in text)
			{
				if (c >= 'ء' && c <= 'ي')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			for (int j = 0; j < text.Length; j++)
			{
				char c2 = text[j];
				char[] array;
				if (!ArabicShaper.Forms.TryGetValue(c2, out array))
				{
					stringBuilder.Append(c2);
				}
				else
				{
					bool flag2 = ArabicShaper.JoinsToFollowing(ArabicShaper.PrevSolid(text, j));
					if (c2 == 'ل')
					{
						int num;
						char c3 = ArabicShaper.NextSolid(text, j, out num);
						char[] array2;
						if (ArabicShaper.LamAlef.TryGetValue(c3, out array2))
						{
							stringBuilder.Append(flag2 ? array2[1] : array2[0]);
							for (int k = j + 1; k < num; k++)
							{
								stringBuilder.Append(text[k]);
							}
							j = num;
							goto IL_0148;
						}
					}
					int num2;
					bool flag3 = array[3] != '\0' && ArabicShaper.AcceptsJoinFromPreceding(ArabicShaper.NextSolid(text, j, out num2));
					char c4;
					if (flag2 && flag3)
					{
						c4 = array[3];
					}
					else if (flag2)
					{
						c4 = ((array[1] != '\0') ? array[1] : array[0]);
					}
					else if (flag3)
					{
						c4 = array[2];
					}
					else
					{
						c4 = array[0];
					}
					stringBuilder.Append(c4);
				}
				IL_0148:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x0017A018 File Offset: 0x00178218
		public static string FixDirectionalRuns(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			List<ValueTuple<int, int, char>> list = new List<ValueTuple<int, int, char>>(text.Length);
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '<')
				{
					int num = ArabicShaper.FindTagEnd(text, i);
					if (num > 0)
					{
						list.Add(new ValueTuple<int, int, char>(i, num - i + 1, 'N'));
						i = num + 1;
						continue;
					}
				}
				char c2;
				if (c == '\n' || c == '\r')
				{
					c2 = 'B';
				}
				else if ((c >= '٠' && c <= '٩') || (c >= '۰' && c <= '۹'))
				{
					c2 = 'L';
				}
				else if (ArabicShaper.IsStrongRtl(c))
				{
					c2 = 'R';
				}
				else if (char.IsLetter(c) || char.IsDigit(c))
				{
					c2 = 'L';
				}
				else
				{
					c2 = 'N';
				}
				list.Add(new ValueTuple<int, int, char>(i, 1, c2));
			}
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			int num2;
			for (int j = 0; j < list.Count; j = num2 + 1)
			{
				num2 = j;
				while (num2 < list.Count && list[num2].Item3 != 'B')
				{
					num2++;
				}
				ArabicShaper.AppendFixedLine(stringBuilder, text, list, j, num2);
				if (num2 >= list.Count)
				{
					break;
				}
				stringBuilder.Append(text, list[num2].Item1, list[num2].Item2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x0017A17C File Offset: 0x0017837C
		private static void AppendFixedLine(StringBuilder sb, string text, [TupleElementNames(new string[] { "start", "length", "cls" })] List<ValueTuple<int, int, char>> units, int lineStart, int lineEnd)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = lineStart; i < lineEnd; i++)
			{
				char item = units[i].Item3;
				if (item == 'L')
				{
					flag = true;
				}
				else if (item == 'R')
				{
					flag2 = true;
				}
			}
			if (flag && !flag2)
			{
				for (int j = lineEnd - 1; j >= lineStart; j--)
				{
					sb.Append(text, units[j].Item1, units[j].Item2);
				}
				return;
			}
			List<ValueTuple<int, int>> list = new List<ValueTuple<int, int>>();
			int k = lineStart;
			while (k < lineEnd)
			{
				if (units[k].Item3 != 'L')
				{
					k++;
				}
				else
				{
					int num = k;
					int num2 = k + 1;
					while (num2 < lineEnd && units[num2].Item3 != 'R')
					{
						if (units[num2].Item3 == 'L')
						{
							num = num2;
						}
						num2++;
					}
					int num3 = k;
					if (num3 > lineStart && units[num3 - 1].Item2 == 1 && "+-#$".IndexOf(text[units[num3 - 1].Item1]) >= 0)
					{
						num3--;
					}
					if (num + 1 < lineEnd && units[num + 1].Item2 == 1 && "%".IndexOf(text[units[num + 1].Item1]) >= 0)
					{
						num++;
					}
					list.Add(new ValueTuple<int, int>(num3, num));
					k = num2;
				}
			}
			int num4 = 0;
			int l = lineStart;
			while (l < lineEnd)
			{
				if (num4 < list.Count && list[num4].Item1 == l)
				{
					for (int m = list[num4].Item2; m >= list[num4].Item1; m--)
					{
						sb.Append(text, units[m].Item1, units[m].Item2);
					}
					l = list[num4].Item2 + 1;
					num4++;
				}
				else
				{
					ValueTuple<int, int, char> valueTuple = units[l];
					int item2 = valueTuple.Item1;
					int item3 = valueTuple.Item2;
					if (item3 == 1)
					{
						sb.Append(ArabicShaper.MirrorBracket(text[item2]));
					}
					else
					{
						sb.Append(text, item2, item3);
					}
					l++;
				}
			}
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x0017A3D4 File Offset: 0x001785D4
		private static int FindTagEnd(string text, int open)
		{
			if (open + 1 >= text.Length)
			{
				return -1;
			}
			char c = text[open + 1];
			if (!char.IsLetter(c) && c != '/' && c != '#')
			{
				return -1;
			}
			int num = Math.Min(text.Length, open + 130);
			for (int i = open + 1; i < num; i++)
			{
				if (text[i] == '>')
				{
					return i;
				}
				if (text[i] == '<')
				{
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x0017A448 File Offset: 0x00178648
		private static bool IsStrongRtl(char c)
		{
			return (c >= '\u0590' && c <= '\u08ff') || (c >= 'יִ' && c <= '﷿') || (c >= 'ﹰ' && c <= '\ufeff');
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x0017A481 File Offset: 0x00178681
		private static char MirrorBracket(char c)
		{
			if (c <= '[')
			{
				if (c == '(')
				{
					return ')';
				}
				if (c == ')')
				{
					return '(';
				}
				if (c == '[')
				{
					return ']';
				}
			}
			else
			{
				if (c == ']')
				{
					return '[';
				}
				if (c == '{')
				{
					return '}';
				}
				if (c == '}')
				{
					return '{';
				}
			}
			return c;
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x0017A4BD File Offset: 0x001786BD
		private static bool IsTransparent(char c)
		{
			return (c >= '\u064b' && c <= '\u065f') || c == '\u0670' || (c >= '\u0610' && c <= '\u061a');
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x0017A4F0 File Offset: 0x001786F0
		private static char PrevSolid(string text, int index)
		{
			for (int i = index - 1; i >= 0; i--)
			{
				if (!ArabicShaper.IsTransparent(text[i]))
				{
					return text[i];
				}
			}
			return '\0';
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x0017A524 File Offset: 0x00178724
		private static char NextSolid(string text, int index, out int solidIndex)
		{
			for (int i = index + 1; i < text.Length; i++)
			{
				if (!ArabicShaper.IsTransparent(text[i]))
				{
					solidIndex = i;
					return text[i];
				}
			}
			solidIndex = text.Length;
			return '\0';
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x0017A568 File Offset: 0x00178768
		private static bool JoinsToFollowing(char c)
		{
			char[] array;
			return c == 'ـ' || (ArabicShaper.Forms.TryGetValue(c, out array) && array[3] > '\0');
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x0017A598 File Offset: 0x00178798
		private static bool AcceptsJoinFromPreceding(char c)
		{
			char[] array;
			return c == 'ـ' || (ArabicShaper.Forms.TryGetValue(c, out array) && array[1] > '\0');
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x0017A5C8 File Offset: 0x001787C8
		// Note: this type is marked as 'beforefieldinit'.
		static ArabicShaper()
		{
			Dictionary<char, char[]> dictionary = new Dictionary<char, char[]>();
			Dictionary<char, char[]> dictionary2 = dictionary;
			char c = 'ء';
			char[] array = new char[4];
			array[0] = 'ﺀ';
			dictionary2.Add(c, array);
			Dictionary<char, char[]> dictionary3 = dictionary;
			char c2 = 'آ';
			char[] array2 = new char[4];
			array2[0] = 'ﺁ';
			array2[1] = 'ﺂ';
			dictionary3.Add(c2, array2);
			Dictionary<char, char[]> dictionary4 = dictionary;
			char c3 = 'أ';
			char[] array3 = new char[4];
			array3[0] = 'ﺃ';
			array3[1] = 'ﺄ';
			dictionary4.Add(c3, array3);
			Dictionary<char, char[]> dictionary5 = dictionary;
			char c4 = 'ؤ';
			char[] array4 = new char[4];
			array4[0] = 'ﺅ';
			array4[1] = 'ﺆ';
			dictionary5.Add(c4, array4);
			Dictionary<char, char[]> dictionary6 = dictionary;
			char c5 = 'إ';
			char[] array5 = new char[4];
			array5[0] = 'ﺇ';
			array5[1] = 'ﺈ';
			dictionary6.Add(c5, array5);
			dictionary.Add('ئ', new char[] { 'ﺉ', 'ﺊ', 'ﺋ', 'ﺌ' });
			Dictionary<char, char[]> dictionary7 = dictionary;
			char c6 = 'ا';
			char[] array6 = new char[4];
			array6[0] = 'ﺍ';
			array6[1] = 'ﺎ';
			dictionary7.Add(c6, array6);
			dictionary.Add('ب', new char[] { 'ﺏ', 'ﺐ', 'ﺑ', 'ﺒ' });
			Dictionary<char, char[]> dictionary8 = dictionary;
			char c7 = 'ة';
			char[] array7 = new char[4];
			array7[0] = 'ﺓ';
			array7[1] = 'ﺔ';
			dictionary8.Add(c7, array7);
			dictionary.Add('ت', new char[] { 'ﺕ', 'ﺖ', 'ﺗ', 'ﺘ' });
			dictionary.Add('ث', new char[] { 'ﺙ', 'ﺚ', 'ﺛ', 'ﺜ' });
			dictionary.Add('ج', new char[] { 'ﺝ', 'ﺞ', 'ﺟ', 'ﺠ' });
			dictionary.Add('ح', new char[] { 'ﺡ', 'ﺢ', 'ﺣ', 'ﺤ' });
			dictionary.Add('خ', new char[] { 'ﺥ', 'ﺦ', 'ﺧ', 'ﺨ' });
			Dictionary<char, char[]> dictionary9 = dictionary;
			char c8 = 'د';
			char[] array8 = new char[4];
			array8[0] = 'ﺩ';
			array8[1] = 'ﺪ';
			dictionary9.Add(c8, array8);
			Dictionary<char, char[]> dictionary10 = dictionary;
			char c9 = 'ذ';
			char[] array9 = new char[4];
			array9[0] = 'ﺫ';
			array9[1] = 'ﺬ';
			dictionary10.Add(c9, array9);
			Dictionary<char, char[]> dictionary11 = dictionary;
			char c10 = 'ر';
			char[] array10 = new char[4];
			array10[0] = 'ﺭ';
			array10[1] = 'ﺮ';
			dictionary11.Add(c10, array10);
			Dictionary<char, char[]> dictionary12 = dictionary;
			char c11 = 'ز';
			char[] array11 = new char[4];
			array11[0] = 'ﺯ';
			array11[1] = 'ﺰ';
			dictionary12.Add(c11, array11);
			dictionary.Add('س', new char[] { 'ﺱ', 'ﺲ', 'ﺳ', 'ﺴ' });
			dictionary.Add('ش', new char[] { 'ﺵ', 'ﺶ', 'ﺷ', 'ﺸ' });
			dictionary.Add('ص', new char[] { 'ﺹ', 'ﺺ', 'ﺻ', 'ﺼ' });
			dictionary.Add('ض', new char[] { 'ﺽ', 'ﺾ', 'ﺿ', 'ﻀ' });
			dictionary.Add('ط', new char[] { 'ﻁ', 'ﻂ', 'ﻃ', 'ﻄ' });
			dictionary.Add('ظ', new char[] { 'ﻅ', 'ﻆ', 'ﻇ', 'ﻈ' });
			dictionary.Add('ع', new char[] { 'ﻉ', 'ﻊ', 'ﻋ', 'ﻌ' });
			dictionary.Add('غ', new char[] { 'ﻍ', 'ﻎ', 'ﻏ', 'ﻐ' });
			dictionary.Add('ف', new char[] { 'ﻑ', 'ﻒ', 'ﻓ', 'ﻔ' });
			dictionary.Add('ق', new char[] { 'ﻕ', 'ﻖ', 'ﻗ', 'ﻘ' });
			dictionary.Add('ك', new char[] { 'ﻙ', 'ﻚ', 'ﻛ', 'ﻜ' });
			dictionary.Add('ل', new char[] { 'ﻝ', 'ﻞ', 'ﻟ', 'ﻠ' });
			dictionary.Add('م', new char[] { 'ﻡ', 'ﻢ', 'ﻣ', 'ﻤ' });
			dictionary.Add('ن', new char[] { 'ﻥ', 'ﻦ', 'ﻧ', 'ﻨ' });
			dictionary.Add('ه', new char[] { 'ﻩ', 'ﻪ', 'ﻫ', 'ﻬ' });
			Dictionary<char, char[]> dictionary13 = dictionary;
			char c12 = 'و';
			char[] array12 = new char[4];
			array12[0] = 'ﻭ';
			array12[1] = 'ﻮ';
			dictionary13.Add(c12, array12);
			Dictionary<char, char[]> dictionary14 = dictionary;
			char c13 = 'ى';
			char[] array13 = new char[4];
			array13[0] = 'ﻯ';
			array13[1] = 'ﻰ';
			dictionary14.Add(c13, array13);
			dictionary.Add('ي', new char[] { 'ﻱ', 'ﻲ', 'ﻳ', 'ﻴ' });
			ArabicShaper.Forms = dictionary;
			ArabicShaper.LamAlef = new Dictionary<char, char[]>
			{
				{
					'آ',
					new char[] { 'ﻵ', 'ﻶ' }
				},
				{
					'أ',
					new char[] { 'ﻷ', 'ﻸ' }
				},
				{
					'إ',
					new char[] { 'ﻹ', 'ﻺ' }
				},
				{
					'ا',
					new char[] { 'ﻻ', 'ﻼ' }
				}
			};
		}

		// Token: 0x040041AC RID: 16812
		private const char Tatweel = 'ـ';

		// Token: 0x040041AD RID: 16813
		private const char Lam = 'ل';

		// Token: 0x040041AE RID: 16814
		private static readonly Dictionary<char, char[]> Forms;

		// Token: 0x040041AF RID: 16815
		private static readonly Dictionary<char, char[]> LamAlef;
	}
}
