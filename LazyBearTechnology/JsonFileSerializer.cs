using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class JsonFileSerializer : BaseSerializer
{
	// Token: 0x06000038 RID: 56 RVA: 0x00002F98 File Offset: 0x00001198
	public JsonFileSerializer(string fileExtension)
	{
		this.fileExtension = fileExtension;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002FA8 File Offset: 0x000011A8
	public override bool SerializeAndSave<T>(T data, string directory, string filename, Action callback)
	{
		bool flag = true;
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		data.OnBeforeSerialize();
		string text = directory + filename + this.fileExtension;
		if (LazyAPI.LazyFile.IsSupportingBackupSaves)
		{
			try
			{
				LazyAPI.LazyFile.WriteAllText(text + ".new", JsonUtility.ToJson(data));
				if (File.Exists(this.GetBackupPath(directory, filename, 3)))
				{
					File.Delete(this.GetBackupPath(directory, filename, 3));
				}
				if (File.Exists(this.GetBackupPath(directory, filename, 2)))
				{
					File.Move(this.GetBackupPath(directory, filename, 2), this.GetBackupPath(directory, filename, 3));
				}
				if (File.Exists(this.GetBackupPath(directory, filename, 1)))
				{
					File.Move(this.GetBackupPath(directory, filename, 1), this.GetBackupPath(directory, filename, 2));
				}
				if (File.Exists(text))
				{
					File.Move(text, this.GetBackupPath(directory, filename, 1));
				}
				File.Move(text + ".new", text);
				goto IL_011C;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Error saving file: {0}", ex));
				flag = false;
				goto IL_011C;
			}
		}
		flag = LazyAPI.LazyFile.WriteAllText(text, JsonUtility.ToJson(data));
		IL_011C:
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000030F0 File Offset: 0x000012F0
	public override void LoadAndDeserialize<T>(string directory, string filename, Action<T> callback)
	{
		T t = JsonUtility.FromJson<T>(LazyAPI.LazyFile.ReadAllText(directory + filename + this.fileExtension));
		t.OnAfterSerialize();
		if (callback != null)
		{
			callback(t);
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003130 File Offset: 0x00001330
	public override void LoadAndDeserializeAll<T>(string directory, [TupleElementNames(new string[] { "data", "fileName" })] Action<List<ValueTuple<T, string>>> callback)
	{
		List<ValueTuple<T, string>> list = new List<ValueTuple<T, string>>();
		foreach (string text in LazyAPI.LazyFile.GetFiles(directory, this.fileExtension, SearchOption.TopDirectoryOnly))
		{
			try
			{
				T t = JsonUtility.FromJson<T>(LazyAPI.LazyFile.ReadAllText(text));
				if (t != null)
				{
					t.OnAfterSerialize();
					list.Add(new ValueTuple<T, string>(t, text.Remove(0, directory.Length)));
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Error while reading save slot, file: {0}. Exception: {1}", text, ex));
			}
		}
		if (callback != null)
		{
			callback(list);
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000031E0 File Offset: 0x000013E0
	public override bool Remove(string directory, string fileName, Action callback)
	{
		bool flag = LazyAPI.LazyFile.Delete(directory + fileName + this.fileExtension);
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00003202 File Offset: 0x00001402
	private string GetBackupPath(string directory, string filename, int backupIndex)
	{
		return string.Format("{0}{1}_backup_{2}{3}", new object[] { directory, filename, backupIndex, this.fileExtension });
	}

	// Token: 0x04000040 RID: 64
	private readonly string fileExtension;
}
