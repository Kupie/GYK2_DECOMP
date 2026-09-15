using System;
using System.IO;
using System.Text;
using LazyBearTechnology;
using Sirenix.Serialization;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class OdinBinaryFileSerializer : BaseSerializer
{
	// Token: 0x0600003E RID: 62 RVA: 0x0000322E File Offset: 0x0000142E
	public OdinBinaryFileSerializer(string fileExtension, SerializationContext serializationContext = null, DeserializationContext deserializationContext = null)
	{
		this.fileExtension = fileExtension;
		this.serializationContext = serializationContext;
		this.deserializationContext = deserializationContext;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000324C File Offset: 0x0000144C
	public override bool SerializeAndSave<T>(T data, string directory, string filename, Action callback)
	{
		if (LazyAPI.Platform.IsGuest())
		{
			Debug.LogError("Save Error: Save is not allowed for guests.");
			if (callback != null)
			{
				callback();
			}
			return false;
		}
		data.OnBeforeSerialize();
		byte[] array = this.Serialize<T>(data);
		Debug.Log(string.Format("Serialized save length: {0}", array.Length));
		bool flag = this.SaveBytes(directory, filename, array);
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000032BC File Offset: 0x000014BC
	public bool SaveBytes(string directory, string filename, byte[] bytes)
	{
		if (LazyAPI.Platform.IsGuest())
		{
			Debug.LogError("Save Error: Save is not allowed for guests.");
			return false;
		}
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		string text = directory + filename + this.fileExtension;
		if (LazyAPI.LazyFile.IsSupportingBackupSaves)
		{
			try
			{
				File.WriteAllBytes(text + ".new", bytes);
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
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Error saving file: {0}", ex));
				return false;
			}
		}
		return LazyAPI.LazyFile.WriteAllBytes(text, bytes);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000033EC File Offset: 0x000015EC
	public byte[] Serialize<T>(T data)
	{
		byte[] array2;
		try
		{
			byte[] array = SerializationUtility.SerializeValue<T>(data, DataFormat.Binary, this.serializationContext);
			if (!string.IsNullOrEmpty(OdinBinaryFileSerializer.encryptKey))
			{
				array = this.Encrypt(array);
			}
			array2 = array;
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error during odin serialization:[{0}]", ex));
			array2 = Array.Empty<byte>();
		}
		return array2;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000344C File Offset: 0x0000164C
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
			t = this.Deserialize<T>(array);
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

	// Token: 0x06000043 RID: 67 RVA: 0x000034FC File Offset: 0x000016FC
	public T Deserialize<T>(byte[] byteData) where T : class, ISerializableData, new()
	{
		if (!string.IsNullOrEmpty(OdinBinaryFileSerializer.encryptKey))
		{
			byteData = this.Decrypt(byteData);
		}
		T t = SerializationUtility.DeserializeValue<T>(byteData, DataFormat.Binary, this.deserializationContext);
		t.OnAfterSerialize();
		return t;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0000352B File Offset: 0x0000172B
	public override bool Remove(string directory, string fileName, Action callback)
	{
		bool flag = LazyAPI.LazyFile.Delete(directory + fileName + this.fileExtension);
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000354D File Offset: 0x0000174D
	private string GetBackupPath(string directory, string filename, int backupIndex)
	{
		return string.Format("{0}{1}_backup_{2}{3}", new object[] { directory, filename, backupIndex, this.fileExtension });
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0000357C File Offset: 0x0000177C
	private byte[] Encrypt(byte[] bytes)
	{
		try
		{
			byte[] bytes2 = Encoding.UTF8.GetBytes(OdinBinaryFileSerializer.encryptKey);
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

	// Token: 0x06000047 RID: 71 RVA: 0x000035EC File Offset: 0x000017EC
	private byte[] Decrypt(byte[] bytes)
	{
		try
		{
			byte[] array = new byte[bytes.Length];
			byte[] bytes2 = Encoding.UTF8.GetBytes(OdinBinaryFileSerializer.encryptKey);
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

	// Token: 0x04000041 RID: 65
	private readonly string fileExtension;

	// Token: 0x04000042 RID: 66
	private SerializationContext serializationContext;

	// Token: 0x04000043 RID: 67
	private DeserializationContext deserializationContext;

	// Token: 0x04000044 RID: 68
	public static string encryptKey;
}
