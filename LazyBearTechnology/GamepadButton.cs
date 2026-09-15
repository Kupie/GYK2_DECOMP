using System;

namespace LazyBearTechnology
{
	// Token: 0x02000131 RID: 305
	[Serializable]
	public class GamepadButton : Enumeration
	{
		// Token: 0x060005FE RID: 1534 RVA: 0x0001EC24 File Offset: 0x0001CE24
		protected GamepadButton(int value)
			: base(value)
		{
		}

		// Token: 0x0400035F RID: 863
		public static GamepadButton None = new GamepadButton(0);

		// Token: 0x04000360 RID: 864
		public static GamepadButton B = new GamepadButton(1);

		// Token: 0x04000361 RID: 865
		public static GamepadButton A = new GamepadButton(2);

		// Token: 0x04000362 RID: 866
		public static GamepadButton X = new GamepadButton(3);

		// Token: 0x04000363 RID: 867
		public static GamepadButton Y = new GamepadButton(4);

		// Token: 0x04000364 RID: 868
		public static GamepadButton LB = new GamepadButton(5);

		// Token: 0x04000365 RID: 869
		public static GamepadButton RB = new GamepadButton(6);

		// Token: 0x04000366 RID: 870
		public static GamepadButton Back = new GamepadButton(7);

		// Token: 0x04000367 RID: 871
		public static GamepadButton Start = new GamepadButton(8);

		// Token: 0x04000368 RID: 872
		public static GamepadButton DUp = new GamepadButton(9);

		// Token: 0x04000369 RID: 873
		public static GamepadButton DDown = new GamepadButton(10);

		// Token: 0x0400036A RID: 874
		public static GamepadButton DLeft = new GamepadButton(11);

		// Token: 0x0400036B RID: 875
		public static GamepadButton DRight = new GamepadButton(12);

		// Token: 0x0400036C RID: 876
		public static GamepadButton LT = new GamepadButton(13);

		// Token: 0x0400036D RID: 877
		public static GamepadButton RT = new GamepadButton(14);

		// Token: 0x0400036E RID: 878
		public static GamepadButton RStick = new GamepadButton(15);

		// Token: 0x0400036F RID: 879
		public static GamepadButton LStick = new GamepadButton(16);
	}
}
