using System;
using System.Text;

namespace LazyBearTechnology
{
	// Token: 0x02000C54 RID: 3156
	public static class ModsJsonComments
	{
		// Token: 0x06005072 RID: 20594 RVA: 0x0017CD98 File Offset: 0x0017AF98
		public static string Strip(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return json;
			}
			StringBuilder stringBuilder = new StringBuilder(json.Length);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			for (int i = 0; i < json.Length; i++)
			{
				char c = json[i];
				char c2 = ((i + 1 < json.Length) ? json[i + 1] : '\0');
				if (flag3)
				{
					if (c == '\n')
					{
						flag3 = false;
						stringBuilder.Append(c);
					}
				}
				else if (flag4)
				{
					if (c == '*' && c2 == '/')
					{
						flag4 = false;
						i++;
					}
				}
				else if (flag)
				{
					stringBuilder.Append(c);
					if (flag2)
					{
						flag2 = false;
					}
					else if (c == '\\')
					{
						flag2 = true;
					}
					else if (c == '"')
					{
						flag = false;
					}
				}
				else if (c == '"')
				{
					flag = true;
					stringBuilder.Append(c);
				}
				else if (c == '/' && c2 == '/')
				{
					flag3 = true;
					i++;
				}
				else if (c == '/' && c2 == '*')
				{
					flag4 = true;
					i++;
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
