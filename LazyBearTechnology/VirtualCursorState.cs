using System;

namespace LazyBearTechnology
{
	// Token: 0x0200015F RID: 351
	[Serializable]
	public class VirtualCursorState : Enumeration
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x00026CC9 File Offset: 0x00024EC9
		public VirtualCursorState(int value)
			: base(value)
		{
		}

		// Token: 0x040004AC RID: 1196
		public static VirtualCursorState Default = new VirtualCursorState(0);

		// Token: 0x040004AD RID: 1197
		public static VirtualCursorState OverObject = new VirtualCursorState(1);
	}
}
