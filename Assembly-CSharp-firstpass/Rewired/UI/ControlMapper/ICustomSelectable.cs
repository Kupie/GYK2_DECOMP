using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000BD RID: 189
	public interface ICustomSelectable : ICancelHandler, IEventSystemHandler
	{
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000A29 RID: 2601
		// (set) Token: 0x06000A2A RID: 2602
		Sprite disabledHighlightedSprite { get; set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000A2B RID: 2603
		// (set) Token: 0x06000A2C RID: 2604
		Color disabledHighlightedColor { get; set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000A2D RID: 2605
		// (set) Token: 0x06000A2E RID: 2606
		string disabledHighlightedTrigger { get; set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000A2F RID: 2607
		// (set) Token: 0x06000A30 RID: 2608
		bool autoNavUp { get; set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000A31 RID: 2609
		// (set) Token: 0x06000A32 RID: 2610
		bool autoNavDown { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000A33 RID: 2611
		// (set) Token: 0x06000A34 RID: 2612
		bool autoNavLeft { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000A35 RID: 2613
		// (set) Token: 0x06000A36 RID: 2614
		bool autoNavRight { get; set; }

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000A37 RID: 2615
		// (remove) Token: 0x06000A38 RID: 2616
		event UnityAction CancelEvent;
	}
}
