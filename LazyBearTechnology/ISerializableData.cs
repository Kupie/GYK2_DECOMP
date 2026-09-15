using System;

namespace LazyBearTechnology
{
	// Token: 0x02000138 RID: 312
	public interface ISerializableData
	{
		// Token: 0x06000641 RID: 1601
		void OnBeforeSerialize();

		// Token: 0x06000642 RID: 1602
		void OnAfterSerialize();
	}
}
