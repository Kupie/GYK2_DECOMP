using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

namespace LazyBearTechnology
{
	// Token: 0x020000E3 RID: 227
	public static class Common
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x000156A8 File Offset: 0x000138A8
		public static byte[] MergeByteArrays(byte[] b1, byte[] b2)
		{
			byte[] array = new byte[b1.Length + b2.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((i < b1.Length) ? b1[i] : b2[i - b1.Length]);
			}
			return array;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x000156E8 File Offset: 0x000138E8
		public static byte[] MergeByteArrays(byte[] b1, List<byte> b2)
		{
			byte[] array = new byte[b1.Length + b2.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ((i < b1.Length) ? b1[i] : b2[i - b1.Length]);
			}
			return array;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00015730 File Offset: 0x00013930
		public static byte[] ReadBinaryDataFromStream(MemoryStream stream)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			if (binaryReader == null)
			{
				Debug.LogError("ReadBinaryDataFromStream(" + ((stream != null) ? stream.ToString() : null) + ") error!");
				return null;
			}
			List<byte> list = new List<byte>();
			try
			{
				for (;;)
				{
					byte b = binaryReader.ReadByte();
					list.Add(b);
				}
			}
			catch (Exception)
			{
			}
			binaryReader.Close();
			return list.ToArray();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000157A0 File Offset: 0x000139A0
		public static byte[] ReadBinaryResource(string resourceName)
		{
			TextAsset textAsset = Resources.Load(resourceName) as TextAsset;
			if (textAsset == null)
			{
				Debug.LogError("Error ReadBinaryResource '" + resourceName + "' !");
				return null;
			}
			Stream stream = new MemoryStream(textAsset.bytes);
			byte[] array = Common.ReadBinaryDataFromStream(stream as MemoryStream);
			stream.Close();
			return array;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000157F6 File Offset: 0x000139F6
		public static string Base64Encode(byte[] b)
		{
			return Convert.ToBase64String(b).Replace("+", "-").Replace("/", "_")
				.Split("="[0], StringSplitOptions.None)[0];
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001582F File Offset: 0x00013A2F
		public static byte[] Base64Decode(string s)
		{
			s = s.Replace("-", "+");
			s = s.Replace("_", "/");
			return Convert.FromBase64String(s);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001585B File Offset: 0x00013A5B
		public static string Base64Encode(string s)
		{
			return Common.Base64Encode(Encoding.UTF8.GetBytes(s));
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00015870 File Offset: 0x00013A70
		public static int Checksum(byte[] b)
		{
			int num = 0;
			foreach (byte b2 in b)
			{
				num += (int)b2;
			}
			return num;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00015898 File Offset: 0x00013A98
		public static int CountTrailingNumbers(string s)
		{
			int num = 0;
			while (s.Substring(s.Length - 1)[0] >= "0"[0] && s.Substring(s.Length - 1)[0] <= "9"[0])
			{
				num++;
				s = s.Substring(0, s.Length - 1);
			}
			return num;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00015904 File Offset: 0x00013B04
		public static string FormatTime(int seconds, string daysSuffix, string hoursSuffix, string minSuffix, string secSuffix)
		{
			int num = (int)Mathf.Floor((float)seconds / 86400f);
			seconds -= num * 86400;
			int num2 = (int)Mathf.Floor((float)seconds / 3600f);
			seconds -= num2 * 3600;
			int num3 = (int)Mathf.Floor((float)seconds / 60f);
			seconds -= num3 * 60;
			int num4 = seconds;
			string text = num4.ToString() ?? "";
			string text2 = num3.ToString() ?? "";
			string text3 = num2.ToString() ?? "";
			string text4 = num.ToString() ?? "";
			if (minSuffix == ":" && num4 < 10)
			{
				text = "0" + text;
			}
			if (hoursSuffix == ":" && num3 < 10)
			{
				text2 = "0" + text2;
			}
			if (num > 0)
			{
				return string.Concat(new string[] { text4, daysSuffix, text3, hoursSuffix, text2 });
			}
			if (num2 > 0)
			{
				return string.Concat(new string[] { text3, hoursSuffix, text2, minSuffix, text, secSuffix });
			}
			if (num3 > 0)
			{
				return text2 + minSuffix + text + secSuffix;
			}
			if (minSuffix == ":")
			{
				text = "00:" + text;
			}
			return text + secSuffix;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00015A70 File Offset: 0x00013C70
		public static string FormatInt(int i)
		{
			string text = i.ToString() ?? "";
			string text2 = "";
			int num = 0;
			for (;;)
			{
				text2 = text[text.Length - 1].ToString() + text2;
				if (text.Length == 1)
				{
					break;
				}
				text = text.Substring(0, text.Length - 1);
				if (++num > 2)
				{
					text2 = " " + text2;
					num = 0;
				}
			}
			return text2;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00015AE8 File Offset: 0x00013CE8
		public static List<Rect> FillRectWithSquares(List<int> availableSquares, Vector2 rect)
		{
			List<Rect> list = new List<Rect>();
			bool[,] array = new bool[(int)rect.x, (int)rect.y];
			int num = 0;
			while ((float)num < rect.x)
			{
				int num2 = 0;
				while ((float)num2 < rect.y)
				{
					array[num, num2] = false;
					num2++;
				}
				num++;
			}
			List<int> list2 = new List<int>();
			list2.AddRange(availableSquares);
			list2.Sort();
			num = 0;
			while ((float)num < rect.x)
			{
				int num2 = 0;
				while ((float)num2 < rect.y)
				{
					if (!array[num, num2])
					{
						int i = list2.Count;
						int num3 = 0;
						int j = 0;
						int k = 0;
						while (i > 0)
						{
							i--;
							num3 = list2[i];
							if ((float)(num + num3) <= rect.x && (float)(num2 + num3) <= rect.y)
							{
								bool flag = true;
								for (j = 0; j < num3; j++)
								{
									for (k = 0; k < num3; k++)
									{
										if (array[num + j, num2 + k])
										{
											flag = false;
											break;
										}
									}
									if (!flag)
									{
										break;
									}
								}
								if (flag)
								{
									break;
								}
							}
							if (i == 0)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"FillRectWithSquares() couldn't fill rectangle correctly! (Maybe you don't have 1x1 square?)\nDidn't fill square at (",
									num.ToString(),
									", ",
									num2.ToString(),
									") of square ",
									rect.x.ToString(),
									"x",
									rect.y.ToString()
								}));
							}
						}
						try
						{
							for (j = 0; j < num3; j++)
							{
								for (k = 0; k < num3; k++)
								{
									array[num + j, num2 + k] = true;
								}
							}
						}
						catch (Exception)
						{
							Debug.LogError("Map is out of range: " + (num + j).ToString() + ", " + (num2 + k).ToString());
						}
						list.Add(new Rect((float)num, (float)num2, (float)num3, (float)num3));
					}
					num2++;
				}
				num++;
			}
			return list;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00015D0C File Offset: 0x00013F0C
		public static byte[] StringToByteArray(string s)
		{
			byte[] array = new byte[s.Length];
			for (int i = 0; i < s.Length; i++)
			{
				array[i] = (byte)s[i];
			}
			return array;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00015D44 File Offset: 0x00013F44
		public static string MemoryToString(long memory)
		{
			if (memory < 1024L)
			{
				return memory.ToString() + " b";
			}
			if (memory < 1048576L)
			{
				return Mathf.Round((float)(memory / 1024L)).ToString() + " Kb";
			}
			return Mathf.Round((float)(memory / 1048576L)).ToString() + " Mb";
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00015DB8 File Offset: 0x00013FB8
		public static object LoadResource(string name)
		{
			long usedHeapSizeLong = Profiler.usedHeapSizeLong;
			object obj = Resources.Load(name);
			if (obj == null)
			{
				Debug.LogWarning("Error loading resource \"" + name + "\" - resource not found!");
				return null;
			}
			if (Common.debugMemory)
			{
				Debug.Log("Resource \"" + name + "\" loaded. Memory spent: " + Common.MemoryToString(Profiler.usedHeapSizeLong - usedHeapSizeLong));
			}
			return obj;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00015E18 File Offset: 0x00014018
		public static object LoadResource(string name, object objectClass)
		{
			long usedHeapSizeLong = Profiler.usedHeapSizeLong;
			object obj = Resources.Load(name, objectClass.GetType());
			if (obj == null)
			{
				Debug.LogWarning("Error loading resource \"" + name + "\" - resource not found!");
				return null;
			}
			if (Common.debugMemory)
			{
				Debug.Log("Resource \"" + name + "\" loaded. Memory spent: " + Common.MemoryToString(Profiler.usedHeapSizeLong - usedHeapSizeLong));
			}
			return obj;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00015E7C File Offset: 0x0001407C
		public static int ConvertStringDateToTimestamp(string s)
		{
			string[] array = s.Split("."[0], StringSplitOptions.None);
			if (array.Length != 3)
			{
				Debug.LogError("Unknown date format: " + s);
				return 0;
			}
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (int)(new DateTime(int.Parse(array[2]), int.Parse(array[1]), int.Parse(array[0]), 0, 0, 0, DateTimeKind.Utc) - dateTime).TotalSeconds;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00015EF8 File Offset: 0x000140F8
		public static string ToAnyBase(int n, string chars)
		{
			string text = "";
			int length = chars.Length;
			do
			{
				text = chars[n % length].ToString() + text;
				n = (int)Mathf.Floor((float)n * 1f / (float)length);
			}
			while (n > 0);
			return text;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00015F44 File Offset: 0x00014144
		public static int FromAnyBase(string s, string chars)
		{
			int num = 0;
			int length = chars.Length;
			for (int i = 0; i < s.Length; i++)
			{
				num = num * length + chars.IndexOf(s[i]);
			}
			return num;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015F7E File Offset: 0x0001417E
		public static string ToBase62(int n)
		{
			return Common.ToAnyBase(n, Common.base62);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00015F8B File Offset: 0x0001418B
		public static int FromBase62(string s)
		{
			return Common.FromAnyBase(s, Common.base62);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00015F98 File Offset: 0x00014198
		public static int GetNowTimestamp()
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return (int)(DateTime.UtcNow - dateTime).TotalSeconds;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00015FCC File Offset: 0x000141CC
		public static int GetYearDay()
		{
			return DateTime.Now.DayOfYear;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00015FE8 File Offset: 0x000141E8
		private static string MD5Sum(string strToEncrypt)
		{
			byte[] bytes = new UTF8Encoding().GetBytes(strToEncrypt);
			byte[] array = new MD5CryptoServiceProvider().ComputeHash(bytes);
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text += Convert.ToString(array[i], 16).PadLeft(2, "0"[0]);
			}
			return text.PadLeft(32, "0"[0]);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00016056 File Offset: 0x00014256
		public static string FixURLString(string s)
		{
			s = s.Replace("&quot;", "\"");
			s = s.Replace("&#xA;", "\n");
			return s;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00016080 File Offset: 0x00014280
		private static string URLEncode(string s)
		{
			s = s.Replace(" ", "%20");
			s = s.Replace("?", "%3F");
			s = s.Replace("&", "%26");
			s = s.Replace("=", "%3D");
			s = s.Replace(",", "%2C");
			s = s.Replace("'", "%27");
			s = s.Replace("\"", "%22");
			s = s.Replace("$", "%24");
			s = s.Replace("\r", "%0D");
			s = s.Replace("\n", "%0A");
			s = s.Replace(":", "%2F");
			s = s.Replace("/", "%3A");
			return s;
		}

		// Token: 0x040001E7 RID: 487
		public static bool debugMemory = false;

		// Token: 0x040001E8 RID: 488
		public static string loremIpsumShort = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";

		// Token: 0x040001E9 RID: 489
		public static string loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

		// Token: 0x040001EA RID: 490
		public static bool useEtcTextures = false;

		// Token: 0x040001EB RID: 491
		public static bool pvrSupported = true;

		// Token: 0x040001EC RID: 492
		public static string defaultShaderName = "ex2D/Alpha Blended";

		// Token: 0x040001ED RID: 493
		public static string base62 = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		// Token: 0x040001EE RID: 494
		public static string baseReadable = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
	}
}
