using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000DC RID: 220
	[AddComponentMenu("")]
	public abstract class UIElementInfo : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000B6E RID: 2926 RVA: 0x0001F08C File Offset: 0x0001D28C
		// (remove) Token: 0x06000B6F RID: 2927 RVA: 0x0001F0C4 File Offset: 0x0001D2C4
		public event Action<GameObject> OnSelectedEvent;

		// Token: 0x06000B70 RID: 2928 RVA: 0x0001F0F9 File Offset: 0x0001D2F9
		public void OnSelect(BaseEventData eventData)
		{
			if (this.OnSelectedEvent != null)
			{
				this.OnSelectedEvent(base.gameObject);
			}
		}

		// Token: 0x040005B4 RID: 1460
		public string identifier;

		// Token: 0x040005B5 RID: 1461
		public int intData;

		// Token: 0x040005B6 RID: 1462
		public TMP_Text text;
	}
}
