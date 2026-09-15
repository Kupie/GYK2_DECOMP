using System;
using Rewired.Platforms.Custom;
using UnityEngine;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x0200011C RID: 284
	public sealed class CustomPlatformManager : MonoBehaviour, ICustomPlatformInitializer
	{
		// Token: 0x06000D40 RID: 3392 RVA: 0x000273D0 File Offset: 0x000255D0
		public CustomPlatformInitOptions GetCustomPlatformInitOptions()
		{
			CustomPlatformInitOptions customPlatformInitOptions = new CustomPlatformInitOptions();
			customPlatformInitOptions.platformId = 0;
			customPlatformInitOptions.platformIdentifierString = "MyPlatform";
			customPlatformInitOptions.hardwareJoystickMapCustomPlatformMapProvider = this.mapProvider;
			CustomPlatformConfigVars customPlatformConfigVars = new CustomPlatformConfigVars
			{
				ignoreInputWhenAppNotInFocus = true,
				useNativeKeyboard = true,
				useNativeMouse = true
			};
			customPlatformInitOptions.inputSource = new MyPlatformInputSource(customPlatformConfigVars);
			return customPlatformInitOptions;
		}

		// Token: 0x04000711 RID: 1809
		public CustomPlatformHardwareJoystickMapProvider mapProvider;
	}
}
