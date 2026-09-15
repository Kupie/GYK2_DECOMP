using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class PlayerPrefsSerializer : BaseSerializer
{
	// Token: 0x06000048 RID: 72 RVA: 0x0000365C File Offset: 0x0000185C
	public override bool SerializeAndSave<T>(T data, string directory, string filename, Action callback)
	{
		data.OnBeforeSerialize();
		string text = JsonUtility.ToJson(data);
		LazyAPI.PlayerPrefs.SetString(filename, text);
		LazyAPI.PlayerPrefs.Save();
		if (callback != null)
		{
			callback();
		}
		return true;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000036A4 File Offset: 0x000018A4
	public override void LoadAndDeserialize<T>(string directory, string filename, Action<T> callback)
	{
		string @string = LazyAPI.PlayerPrefs.GetString(filename, "");
		if (string.IsNullOrEmpty(@string))
		{
			if (callback != null)
			{
				callback(default(T));
				return;
			}
		}
		else
		{
			T t = JsonUtility.FromJson<T>(@string);
			t.OnAfterSerialize();
			if (callback != null)
			{
				callback(t);
			}
		}
	}
}
