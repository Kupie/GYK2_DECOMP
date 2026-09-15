using System;

namespace LazyBearTechnology
{
	// Token: 0x020000CF RID: 207
	[Serializable]
	public class AudioSettings3DType : Enumeration
	{
		// Token: 0x06000369 RID: 873 RVA: 0x00012247 File Offset: 0x00010447
		public AudioSettings3DType(int value)
			: base(value)
		{
		}

		// Token: 0x04000134 RID: 308
		public static AudioSettings3DType Default = new AudioSettings3DType(0);

		// Token: 0x04000135 RID: 309
		public static AudioSettings3DType Default3D = new AudioSettings3DType(1);

		// Token: 0x04000136 RID: 310
		public static AudioSettings3DType SoundZone3D = new AudioSettings3DType(2);

		// Token: 0x04000137 RID: 311
		public static AudioSettings3DType Fight3D3D = new AudioSettings3DType(3);

		// Token: 0x04000138 RID: 312
		public static AudioSettings3DType Workbench3D = new AudioSettings3DType(4);
	}
}
