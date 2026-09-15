using System;
using LazyBearTechnology;

// Token: 0x02000A93 RID: 2707
public static class BinaryDataSerializer
{
	// Token: 0x06004987 RID: 18823 RVA: 0x0015B6C9 File Offset: 0x001598C9
	public static byte[] SerializeData<T>(T data) where T : class, ISerializableData, new()
	{
		data.OnBeforeSerialize();
		return LazySerializer.Serialize<T>(data);
	}

	// Token: 0x06004988 RID: 18824 RVA: 0x0015B6DC File Offset: 0x001598DC
	public static T DeserializeData<T>(byte[] byteData) where T : class, ISerializableData, new()
	{
		return LazySerializer.Deserialize<T>(byteData);
	}
}
