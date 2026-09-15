using System;
using System.IO;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200017E RID: 382
	public class JsonDataController
	{
		// Token: 0x06000887 RID: 2183 RVA: 0x00029FA8 File Offset: 0x000281A8
		public static T Load<T>(string resourcePath)
		{
			StreamReader streamReader = new StreamReader(Application.persistentDataPath + resourcePath, true);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			return JsonUtility.FromJson<T>(text);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00029FD8 File Offset: 0x000281D8
		public static T LoadOverwrite<T>(string resourcePath, T objectToOverwrite)
		{
			StreamReader streamReader = new StreamReader(Application.persistentDataPath + resourcePath, true);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			JsonUtility.FromJsonOverwrite(text, objectToOverwrite);
			return objectToOverwrite;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0002A010 File Offset: 0x00028210
		public static bool Save<T>(T data, string resourcePath)
		{
			string text = Application.persistentDataPath + resourcePath;
			string text2 = JsonUtility.ToJson(data, true);
			StreamWriter streamWriter = new StreamWriter(text);
			streamWriter.Write(text2);
			streamWriter.Close();
			return true;
		}
	}
}
