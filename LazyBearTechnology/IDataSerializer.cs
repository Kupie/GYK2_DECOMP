using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;

// Token: 0x0200000E RID: 14
public interface IDataSerializer
{
	// Token: 0x06000028 RID: 40
	bool SerializeAndSave<T>(T data, string directory, string filename, Action callback) where T : class, ISerializableData, new();

	// Token: 0x06000029 RID: 41
	void LoadAndDeserialize<T>(string directory, string filename, Action<T> callback) where T : class, ISerializableData, new();

	// Token: 0x0600002A RID: 42
	void LoadAndDeserializeAll<T>(string directory, [TupleElementNames(new string[] { "data", "fileName" })] Action<List<ValueTuple<T, string>>> callback) where T : class, ISerializableData, new();

	// Token: 0x0600002B RID: 43
	bool Remove(string directory, string fileName, Action callback);
}
