using System;
using Rewired.Platforms.Custom;
using UnityEngine;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x02000127 RID: 295
	public class MyPlatformUnifiedKeyboardSource : CustomPlatformUnifiedKeyboardSource
	{
		// Token: 0x06000D6A RID: 3434 RVA: 0x00027864 File Offset: 0x00025A64
		protected override void OnInitialize()
		{
			base.OnInitialize();
			CustomPlatformUnifiedKeyboardSource.KeyPropertyMap keyPropertyMap = new CustomPlatformUnifiedKeyboardSource.KeyPropertyMap();
			keyPropertyMap.Set(new CustomPlatformUnifiedKeyboardSource.KeyPropertyMap.Key
			{
				keyCode = KeyboardKeyCode.A,
				label = "[A]"
			});
			keyPropertyMap.Set(new CustomPlatformUnifiedKeyboardSource.KeyPropertyMap.Key[]
			{
				new CustomPlatformUnifiedKeyboardSource.KeyPropertyMap.Key
				{
					keyCode = KeyboardKeyCode.B,
					label = "[B]"
				},
				new CustomPlatformUnifiedKeyboardSource.KeyPropertyMap.Key
				{
					keyCode = KeyboardKeyCode.C,
					label = "[C]"
				},
				new CustomPlatformUnifiedKeyboardSource.KeyPropertyMap.Key
				{
					keyCode = KeyboardKeyCode.D,
					label = "[D]"
				}
			});
			base.keyPropertyMap = keyPropertyMap;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00027924 File Offset: 0x00025B24
		protected override void Update()
		{
			for (int i = 0; i < MyPlatformUnifiedKeyboardSource.keyCodes.Length; i++)
			{
				base.SetKeyValue(MyPlatformUnifiedKeyboardSource.keyCodes[i], Input.GetKey((KeyCode)MyPlatformUnifiedKeyboardSource.keyCodes[i]));
			}
		}

		// Token: 0x0400071F RID: 1823
		private static readonly KeyboardKeyCode[] keyCodes = (KeyboardKeyCode[])Enum.GetValues(typeof(KeyboardKeyCode));
	}
}
