using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200000F RID: 15
public abstract class BaseSerializer : IDataSerializer
{
	// Token: 0x0600002C RID: 44 RVA: 0x00002B8C File Offset: 0x00000D8C
	public virtual bool SerializeAndSave<T>(T data, string directory, string filename, Action callback) where T : class, ISerializableData, new()
	{
		Debug.LogError(string.Format("{0}: method {1} not implemented.", base.GetType(), "SerializeAndSave"));
		return true;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002BA9 File Offset: 0x00000DA9
	public virtual void LoadAndDeserialize<T>(string directory, string filename, Action<T> callback) where T : class, ISerializableData, new()
	{
		Debug.LogError(string.Format("{0}: method {1} not implemented.", base.GetType(), "LoadAndDeserialize"));
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00002BC5 File Offset: 0x00000DC5
	public virtual void LoadAndDeserializeAll<T>(string directory, [TupleElementNames(new string[] { "data", "fileName" })] Action<List<ValueTuple<T, string>>> callback) where T : class, ISerializableData, new()
	{
		Debug.LogError(string.Format("{0}: method {1} not implemented.", base.GetType(), "LoadAndDeserializeAll"));
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002BE1 File Offset: 0x00000DE1
	public virtual bool Remove(string directory, string fileName, Action callback)
	{
		Debug.LogError(string.Format("{0}: method {1} not implemented.", base.GetType(), "Remove"));
		return true;
	}
}
