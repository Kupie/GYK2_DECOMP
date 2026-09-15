using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LazyBearTechnology
{
	// Token: 0x02000139 RID: 313
	public class LazySaveSystem
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x0001F9A0 File Offset: 0x0001DBA0
		public LazySaveSystem(params ValueTuple<Type, IDataSerializer>[] dataSerializersArray)
		{
			this.dataSaverDictionary = new Dictionary<Type, IDataSerializer>();
			Array.ForEach<ValueTuple<Type, IDataSerializer>>(dataSerializersArray, delegate(ValueTuple<Type, IDataSerializer> x)
			{
				this.dataSaverDictionary.Add(x.Item1, x.Item2);
			});
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001F9C8 File Offset: 0x0001DBC8
		public bool Save<T>(T data, string directory, string saveFilename, Action callback) where T : class, ISerializableData, new()
		{
			IDataSerializer dataSerializer;
			if (!this.dataSaverDictionary.TryGetValue(typeof(T), out dataSerializer))
			{
				throw new Exception(string.Format("{0} for type {1} not defined.", typeof(IDataSerializer), typeof(T)));
			}
			return dataSerializer.SerializeAndSave<T>(data, directory, saveFilename, callback);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001FA20 File Offset: 0x0001DC20
		public void Load<T>(string directory, string saveFilename, Action<T> callback) where T : class, ISerializableData, new()
		{
			IDataSerializer dataSerializer;
			if (!this.dataSaverDictionary.TryGetValue(typeof(T), out dataSerializer))
			{
				throw new Exception(string.Format("{0} for type {1} not defined.", typeof(IDataSerializer), typeof(T)));
			}
			dataSerializer.LoadAndDeserialize<T>(directory, saveFilename, callback);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001FA74 File Offset: 0x0001DC74
		public void LoadAll<T>(string directory, [TupleElementNames(new string[] { "data", "fileName" })] Action<List<ValueTuple<T, string>>> callback) where T : class, ISerializableData, new()
		{
			IDataSerializer dataSerializer;
			if (!this.dataSaverDictionary.TryGetValue(typeof(T), out dataSerializer))
			{
				throw new Exception(string.Format("{0} for type {1} not defined.", typeof(IDataSerializer), typeof(T)));
			}
			dataSerializer.LoadAndDeserializeAll<T>(directory, callback);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
		public bool Remove<T>(string directory, string fileName, Action callback)
		{
			IDataSerializer dataSerializer;
			if (!this.dataSaverDictionary.TryGetValue(typeof(T), out dataSerializer))
			{
				throw new Exception(string.Format("{0} for type {1} not defined.", typeof(IDataSerializer), typeof(T)));
			}
			return dataSerializer.Remove(directory, fileName, callback);
		}

		// Token: 0x04000399 RID: 921
		private Dictionary<Type, IDataSerializer> dataSaverDictionary;
	}
}
