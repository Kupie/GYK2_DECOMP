using System;

namespace LazyBearTechnology
{
	// Token: 0x02000132 RID: 306
	[Serializable]
	public class GamepadType : Enumeration
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x0001ED00 File Offset: 0x0001CF00
		public GamepadType(int value)
			: base(value)
		{
		}

		// Token: 0x04000370 RID: 880
		public static GamepadType Xbox_XboxController = new GamepadType(0);

		// Token: 0x04000371 RID: 881
		public static GamepadType Sony_DualShock = new GamepadType(1);

		// Token: 0x04000372 RID: 882
		public static GamepadType Sony_DualSense = new GamepadType(2);

		// Token: 0x04000373 RID: 883
		public static GamepadType Switch_JoyCon_Dual = new GamepadType(3);

		// Token: 0x04000374 RID: 884
		public static GamepadType Switch_JoyCon_Left = new GamepadType(4);

		// Token: 0x04000375 RID: 885
		public static GamepadType Switch_JoyCon_Right = new GamepadType(5);

		// Token: 0x04000376 RID: 886
		public static GamepadType Switch_Pro = new GamepadType(6);

		// Token: 0x04000377 RID: 887
		public static GamepadType Switch_Handheld = new GamepadType(7);
	}
}
