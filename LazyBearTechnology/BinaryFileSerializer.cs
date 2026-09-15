using System;
using System.IO;
using System.Text;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class BinaryFileSerializer : BaseSerializer
{
	// Token: 0x06000031 RID: 49 RVA: 0x00002C06 File Offset: 0x00000E06
	public BinaryFileSerializer(string fileExtension)
	{
		this.fileExtension = fileExtension;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002C18 File Offset: 0x00000E18
	public override bool SerializeAndSave<T>(T data, string directory, string filename, Action callback)
	{
		bool flag = true;
		if (LazyAPI.Platform.IsGuest())
		{
			Debug.LogError("Save Error: Save is not allowed for guests.");
			if (callback != null)
			{
				callback();
			}
			return false;
		}
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		string text = directory + filename + this.fileExtension;
		data.OnBeforeSerialize();
		byte[] array = LazySerializer.Serialize<T>(data);
		if (!string.IsNullOrEmpty(BinaryFileSerializer.encryptKey))
		{
			array = this.Encrypt(array);
		}
		Debug.Log(string.Format("Serialized save length: {0}", array.Length));
		if (LazyAPI.LazyFile.IsSupportingBackupSaves)
		{
			try
			{
				File.WriteAllBytes(text + ".new", array);
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
				goto IL_0157;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Error saving file: {0}", ex));
				flag = false;
				goto IL_0157;
			}
		}
		flag = LazyAPI.LazyFile.WriteAllBytes(text, array);
		IL_0157:
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002D98 File Offset: 0x00000F98
	public override void LoadAndDeserialize<T>(string directory, string filename, Action<T> callback)
	{
		string text = directory + filename + this.fileExtension;
		if (!LazyAPI.LazyFile.Exists(text))
		{
			Debug.LogError("Save file not found: [" + text + "]");
			if (callback != null)
			{
				callback(default(T));
			}
		}
		T t = default(T);
		try
		{
			byte[] array = LazyAPI.LazyFile.ReadAllBytes(text);
			if (!string.IsNullOrEmpty(BinaryFileSerializer.encryptKey))
			{
				array = this.Decrypt(array);
			}
			t = LazySerializer.Deserialize<T>(array);
			t.OnAfterSerialize();
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error while reading save file {0}. Ex: {1}", text, ex));
			if (callback != null)
			{
				callback(default(T));
			}
			return;
		}
		if (callback != null)
		{
			callback(t);
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002E68 File Offset: 0x00001068
	public override bool Remove(string directory, string fileName, Action callback)
	{
		bool flag = LazyAPI.LazyFile.Delete(directory + fileName + this.fileExtension);
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002E8A File Offset: 0x0000108A
	private string GetBackupPath(string directory, string filename, int backupIndex)
	{
		return string.Format("{0}{1}_backup_{2}{3}", new object[] { directory, filename, backupIndex, this.fileExtension });
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002EB8 File Offset: 0x000010B8
	private byte[] Encrypt(byte[] bytes)
	{
		try
		{
			byte[] bytes2 = Encoding.UTF8.GetBytes(BinaryFileSerializer.encryptKey);
			byte[] array = new byte[bytes.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = bytes[i] ^ bytes2[i % bytes2.Length];
			}
			return array;
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Can't encrypt data, exception:[{0}]", ex));
		}
		return bytes;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002F28 File Offset: 0x00001128
	private byte[] Decrypt(byte[] bytes)
	{
		try
		{
			byte[] array = new byte[bytes.Length];
			byte[] bytes2 = Encoding.UTF8.GetBytes(BinaryFileSerializer.encryptKey);
			for (int i = 0; i < bytes.Length; i++)
			{
				array[i] = bytes[i] ^ bytes2[i % bytes2.Length];
			}
			return array;
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Can't decrypt data, exception:[{0}]", ex));
		}
		return bytes;
	}

	// Token: 0x0400003E RID: 62
	private readonly string fileExtension;

	// Token: 0x0400003F RID: 63
	public static string encryptKey;
}
