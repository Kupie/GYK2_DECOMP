using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x0200004E RID: 78
	[AddComponentMenu("Rewired/Rewired Event System")]
	public class RewiredEventSystem : EventSystem
	{
		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000A19C File Offset: 0x0000839C
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0000A1A4 File Offset: 0x000083A4
		public bool alwaysUpdate
		{
			get
			{
				return this._alwaysUpdate;
			}
			set
			{
				this._alwaysUpdate = value;
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000A1B0 File Offset: 0x000083B0
		protected override void Update()
		{
			if (this.alwaysUpdate)
			{
				EventSystem current = EventSystem.current;
				if (current != this)
				{
					EventSystem.current = this;
				}
				try
				{
					base.Update();
					return;
				}
				finally
				{
					if (current != this)
					{
						EventSystem.current = current;
					}
				}
			}
			base.Update();
		}

		// Token: 0x040002A9 RID: 681
		[Tooltip("If enabled, the Event System will be updated every frame even if other Event Systems are enabled. Otherwise, only EventSystem.current will be updated.")]
		[SerializeField]
		private bool _alwaysUpdate;
	}
}
