using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x020000FC RID: 252
	public class PredefinedGUIStyle
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x00017E39 File Offset: 0x00016039
		public GUIStyle Style
		{
			get
			{
				if (this.style == null)
				{
					this.style = new GUIStyle(this.baseStyleGetter());
					this.initializer(this.style);
				}
				return this.style;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00017E70 File Offset: 0x00016070
		public PredefinedGUIStyle(Func<GUIStyle> baseStyleGetter, Action<GUIStyle> initializer)
		{
			this.baseStyleGetter = baseStyleGetter;
			this.initializer = initializer;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00017E86 File Offset: 0x00016086
		public static explicit operator GUIStyle(PredefinedGUIStyle s)
		{
			return s.Style;
		}

		// Token: 0x04000230 RID: 560
		private Func<GUIStyle> baseStyleGetter;

		// Token: 0x04000231 RID: 561
		private Action<GUIStyle> initializer;

		// Token: 0x04000232 RID: 562
		private GUIStyle style;
	}
}
