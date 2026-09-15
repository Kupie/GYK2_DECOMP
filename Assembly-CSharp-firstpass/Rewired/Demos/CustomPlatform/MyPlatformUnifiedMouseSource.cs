using System;
using Rewired.Platforms.Custom;
using UnityEngine;

namespace Rewired.Demos.CustomPlatform
{
	// Token: 0x02000128 RID: 296
	public class MyPlatformUnifiedMouseSource : CustomPlatformUnifiedMouseSource
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0002797F File Offset: 0x00025B7F
		public override Vector2 mousePosition
		{
			get
			{
				return Input.mousePosition;
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x0002798C File Offset: 0x00025B8C
		protected override void Update()
		{
			base.SetAxisValue(0, Input.GetAxis("MouseAxis1"));
			base.SetAxisValue(1, Input.GetAxis("MouseAxis2"));
			base.SetAxisValue(2, Input.GetAxis("MouseAxis3"));
			base.SetButtonValue(0, Input.GetButton("MouseButton0"));
			base.SetButtonValue(1, Input.GetButton("MouseButton1"));
			base.SetButtonValue(2, Input.GetButton("MouseButton2"));
			base.SetButtonValue(3, Input.GetButton("MouseButton3"));
			base.SetButtonValue(4, Input.GetButton("MouseButton4"));
			base.SetButtonValue(5, Input.GetButton("MouseButton5"));
			base.SetButtonValue(6, Input.GetButton("MouseButton6"));
		}
	}
}
