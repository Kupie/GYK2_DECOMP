using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000145 RID: 325
	public class LazyButtonTipStrTester : MonoBehaviour
	{
		// Token: 0x060006A9 RID: 1705 RVA: 0x0002232C File Offset: 0x0002052C
		private void TestPlatformTips()
		{
			Platform.ForceDebugPlatform(this.testPlatformType);
			LazyInput.ForceGamepadActivityState(true);
			LazyInput.ForceDebugGamepadType(this.testGamepadType);
			LazySingletonSO<ControllerIconLibrary>.Instance.Init();
			List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
			foreach (GameKey gameKey in this.keysToTest)
			{
				list.Add(new LazyGameKeyTip(gameKey, "Test Active", true, false, false));
				list.Add(new LazyGameKeyTip(gameKey, "Test Inactive", false, false, false));
			}
			this.lazyButtonTips.Print(list, "\n\n");
			Platform.ClearForcedPlatform();
			LazyInput.ClearForcedGamepadType();
			LazyInput.ClearGamepadActivityState();
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000223EC File Offset: 0x000205EC
		private void ForcePlatformAndGamepadType()
		{
			Platform.ForceDebugPlatform(this.testPlatformType);
			LazyInput.ForceDebugGamepadType(this.testGamepadType);
			LazySingletonSO<ControllerIconLibrary>.Instance.Init();
		}

		// Token: 0x040003F5 RID: 1013
		[SerializeField]
		private PlatformType testPlatformType;

		// Token: 0x040003F6 RID: 1014
		[SerializeField]
		private GamepadType testGamepadType;

		// Token: 0x040003F7 RID: 1015
		[SerializeField]
		private LazyButtonTipsStr lazyButtonTips;

		// Token: 0x040003F8 RID: 1016
		[SerializeField]
		private List<GameKey> keysToTest;
	}
}
