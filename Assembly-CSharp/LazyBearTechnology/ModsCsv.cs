using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LazyBearTechnology
{
	// Token: 0x02000C52 RID: 3154
	public static class ModsCsv
	{
		// Token: 0x0600506C RID: 20588 RVA: 0x0017CA58 File Offset: 0x0017AC58
		public static void WriteKeyValueFile(string path, IEnumerable<KeyValuePair<string, string>> rows)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("key,value\r\n");
			foreach (KeyValuePair<string, string> keyValuePair in rows)
			{
				stringBuilder.Append(ModsCsv.Escape(keyValuePair.Key));
				stringBuilder.Append(',');
				stringBuilder.Append(ModsCsv.Escape(keyValuePair.Value ?? string.Empty));
				stringBuilder.Append("\r\n");
			}
			File.WriteAllText(path, stringBuilder.ToString(), ModsCsv.Utf8Bom);
		}

		// Token: 0x0600506D RID: 20589 RVA: 0x0017CB00 File Offset: 0x0017AD00
		public static Dictionary<string, string> ReadKeyValueFile(string path)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			if (!File.Exists(path))
			{
				return dictionary;
			}
			List<string[]> list = ModsCsv.Parse(File.ReadAllText(path));
			for (int i = 0; i < list.Count; i++)
			{
				string[] array = list[i];
				if (array.Length >= 2)
				{
					string text = array[0];
					string text2 = array[1];
					if ((i != 0 || !string.Equals(text, "key", StringComparison.OrdinalIgnoreCase) || !string.Equals(text2, "value", StringComparison.OrdinalIgnoreCase)) && !string.IsNullOrEmpty(text))
					{
						dictionary[text] = text2 ?? string.Empty;
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x0017CB90 File Offset: 0x0017AD90
		private static string Escape(string value)
		{
			if (value == null)
			{
				return "\"\"";
			}
			if (value.IndexOfAny(new char[] { ',', '"', '\n', '\r' }) < 0)
			{
				return value;
			}
			return "\"" + value.Replace("\"", "\"\"") + "\"";
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x0017CBE8 File Offset: 0x0017ADE8
		private static List<string[]> Parse(string text)
		{
			List<string[]> list = new List<string[]>();
			List<string> list2 = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (flag)
				{
					if (c == '"')
					{
						if (i + 1 < text.Length && text[i + 1] == '"')
						{
							stringBuilder.Append('"');
							i++;
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				else if (c == '"')
				{
					flag = true;
				}
				else if (c == ',')
				{
					list2.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				else if (c != '\r')
				{
					if (c == '\n')
					{
						list2.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
						if (list2.Count > 1 || (list2.Count == 1 && !string.IsNullOrEmpty(list2[0])))
						{
							list.Add(list2.ToArray());
						}
						list2.Clear();
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
			}
			if (flag || stringBuilder.Length > 0 || list2.Count > 0)
			{
				list2.Add(stringBuilder.ToString());
				if (list2.Count > 1 || (list2.Count == 1 && !string.IsNullOrEmpty(list2[0])))
				{
					list.Add(list2.ToArray());
				}
			}
			return list;
		}

		// Token: 0x040041ED RID: 16877
		private static readonly Encoding Utf8Bom = new UTF8Encoding(true);
	}
}
