using System;

namespace LazyBearTechnology
{
	// Token: 0x020000DD RID: 221
	[Serializable]
	public class LazyBuildType : Enumeration
	{
		// Token: 0x060003CA RID: 970 RVA: 0x00014ACE File Offset: 0x00012CCE
		public LazyBuildType(int value)
			: base(value)
		{
		}

		// Token: 0x040001CB RID: 459
		public static LazyBuildType Release = new LazyBuildType(0);

		// Token: 0x040001CC RID: 460
		public static LazyBuildType Demo = new LazyBuildType(1);

		// Token: 0x040001CD RID: 461
		public static LazyBuildType PS4_Release_US = new LazyBuildType(2);

		// Token: 0x040001CE RID: 462
		public static LazyBuildType PS4_Release_EU = new LazyBuildType(3);

		// Token: 0x040001CF RID: 463
		public static LazyBuildType PS4_Patch_US = new LazyBuildType(4);

		// Token: 0x040001D0 RID: 464
		public static LazyBuildType PS4_Patch_EU = new LazyBuildType(5);

		// Token: 0x040001D1 RID: 465
		public static LazyBuildType PS5_Release = new LazyBuildType(6);

		// Token: 0x040001D2 RID: 466
		public static LazyBuildType PS4_Release_JP = new LazyBuildType(7);

		// Token: 0x040001D3 RID: 467
		public static LazyBuildType PS4_Release_ASIA = new LazyBuildType(8);

		// Token: 0x040001D4 RID: 468
		public static LazyBuildType PS4_Patch_JP = new LazyBuildType(9);

		// Token: 0x040001D5 RID: 469
		public static LazyBuildType PS4_Patch_ASIA = new LazyBuildType(10);

		// Token: 0x040001D6 RID: 470
		public static LazyBuildType PS5_Demo = new LazyBuildType(11);

		// Token: 0x040001D7 RID: 471
		public static LazyBuildType PS4_Demo_US = new LazyBuildType(12);

		// Token: 0x040001D8 RID: 472
		public static LazyBuildType PS4_Demo_Patch_US = new LazyBuildType(13);
	}
}
