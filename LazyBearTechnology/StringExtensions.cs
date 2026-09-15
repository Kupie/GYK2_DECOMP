using System;
using System.Text;

// Token: 0x02000025 RID: 37
public static class StringExtensions
{
	// Token: 0x060000A6 RID: 166 RVA: 0x00004A40 File Offset: 0x00002C40
	public static string ConcatWithSeparator(this string str, string strToAppend, string separator = "\n")
	{
		StringBuilder stringBuilder = new StringBuilder(str);
		if (!string.IsNullOrEmpty(str))
		{
			stringBuilder.Append(separator);
		}
		return stringBuilder.Append(strToAppend).ToString();
	}
}
