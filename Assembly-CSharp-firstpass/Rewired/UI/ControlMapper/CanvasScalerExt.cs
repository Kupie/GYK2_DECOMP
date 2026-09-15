using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000092 RID: 146
	[AddComponentMenu("")]
	public class CanvasScalerExt : CanvasScaler
	{
		// Token: 0x06000741 RID: 1857 RVA: 0x000129A8 File Offset: 0x00010BA8
		public void ForceRefresh()
		{
			this.Handle();
		}
	}
}
