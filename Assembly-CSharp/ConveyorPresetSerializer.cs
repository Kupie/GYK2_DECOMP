using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using LazyBearTechnology;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x0200048A RID: 1162
public class ConveyorPresetSerializer : BaseSerializer
{
	// Token: 0x06001ED3 RID: 7891 RVA: 0x00091BD0 File Offset: 0x0008FDD0
	public ConveyorPresetSerializer(string fileExtension, SerializationContext serializationContext = null, DeserializationContext deserializationContext = null)
	{
		this.fileExtension = fileExtension;
		this.serializationContext = serializationContext;
		this.deserializationContext = deserializationContext;
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x00091BF0 File Offset: 0x0008FDF0
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
		if (!Directory.Exists(directory))
		{
			Directory.CreateDirectory(directory);
		}
		string text = directory + filename + this.fileExtension;
		data.OnBeforeSerialize();
		byte[] array = this.Serialize<T>(data);
		Debug.Log(string.Format("Serialized save length: {0}", array.Length));
		bool flag = LazyAPI.LazyFile.WriteAllBytes(text, array);
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x06001ED5 RID: 7893 RVA: 0x00091C80 File Offset: 0x0008FE80
	public byte[] Serialize<T>(T data)
	{
		byte[] array2;
		try
		{
			byte[] array = SerializationUtility.SerializeValue<T>(data, DataFormat.Binary, this.serializationContext);
			if (!string.IsNullOrEmpty(ConveyorPresetSerializer.encryptKey))
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

	// Token: 0x06001ED6 RID: 7894 RVA: 0x00091CE0 File Offset: 0x0008FEE0
	public override void LoadAndDeserialize<T>(string directory, string filename, Action<T> callback)
	{
		AsyncOperationHandle<TextAsset> asyncOperationHandle = Addressables.LoadAssetAsync<TextAsset>(directory + filename + this.fileExtension);
		asyncOperationHandle.WaitForCompletion();
		string text = directory + filename + this.fileExtension;
		if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
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
			byte[] bytes = asyncOperationHandle.Result.bytes;
			t = this.Deserialize<T>(bytes);
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

	// Token: 0x06001ED7 RID: 7895 RVA: 0x00091DAC File Offset: 0x0008FFAC
	public T Deserialize<T>(byte[] byteData) where T : class, ISerializableData, new()
	{
		if (!string.IsNullOrEmpty(ConveyorPresetSerializer.encryptKey))
		{
			byteData = this.Decrypt(byteData);
		}
		T t = SerializationUtility.DeserializeValue<T>(byteData, DataFormat.Binary, this.deserializationContext);
		t.OnAfterSerialize();
		return t;
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x00091DDB File Offset: 0x0008FFDB
	public override bool Remove(string directory, string fileName, Action callback)
	{
		bool flag = LazyAPI.LazyFile.Delete(directory + fileName + this.fileExtension);
		if (callback != null)
		{
			callback();
		}
		return flag;
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x00091E00 File Offset: 0x00090000
	private byte[] Encrypt(byte[] bytes)
	{
		try
		{
			byte[] bytes2 = Encoding.UTF8.GetBytes(ConveyorPresetSerializer.encryptKey);
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

	// Token: 0x06001EDA RID: 7898 RVA: 0x00091E70 File Offset: 0x00090070
	private byte[] Decrypt(byte[] bytes)
	{
		try
		{
			byte[] array = new byte[bytes.Length];
			byte[] bytes2 = Encoding.UTF8.GetBytes(ConveyorPresetSerializer.encryptKey);
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

	// Token: 0x06001EDB RID: 7899 RVA: 0x00091EE0 File Offset: 0x000900E0
	public override void LoadAndDeserializeAll<T>(string directory, [TupleElementNames(new string[] { "data", "fileName" })] Action<List<ValueTuple<T, string>>> callback)
	{
		IList<TextAsset> list = Addressables.LoadAssetsAsync<TextAsset>("ConveyorPresets", null).WaitForCompletion();
		List<ValueTuple<T, string>> list2 = new List<ValueTuple<T, string>>();
		if (list == null || list.Count == 0)
		{
			Debug.LogWarning("No conveyor presets found by label [ConveyorPresets]");
			if (callback != null)
			{
				callback(list2);
			}
			return;
		}
		foreach (TextAsset textAsset in list)
		{
			try
			{
				byte[] bytes = textAsset.bytes;
				T t = this.Deserialize<T>(bytes);
				if (t != null)
				{
					list2.Add(new ValueTuple<T, string>(t, textAsset.name));
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("Error while reading save slot, file: {0}. Exception: {1}", textAsset.name, ex));
			}
		}
		if (callback != null)
		{
			callback(list2);
		}
	}

	// Token: 0x04001BD0 RID: 7120
	private readonly string fileExtension;

	// Token: 0x04001BD1 RID: 7121
	private SerializationContext serializationContext;

	// Token: 0x04001BD2 RID: 7122
	private DeserializationContext deserializationContext;

	// Token: 0x04001BD3 RID: 7123
	public static string encryptKey;
}
