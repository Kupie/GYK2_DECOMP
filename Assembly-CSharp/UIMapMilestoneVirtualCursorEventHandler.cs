using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009FC RID: 2556
[RequireComponent(typeof(Collider2D))]
public class UIMapMilestoneVirtualCursorEventHandler : BaseVirtualCursorEventHandler
{
	// Token: 0x060044E9 RID: 17641 RVA: 0x00146496 File Offset: 0x00144696
	private void Awake()
	{
		if (this.mapMilestone == null)
		{
			this.mapMilestone = base.transform.parent.GetComponent<UIMapMilestone>();
		}
	}

	// Token: 0x060044EA RID: 17642 RVA: 0x001464BC File Offset: 0x001446BC
	protected override void OnSelect(BaseVirtualCursor cursor)
	{
		if (!this.mapMilestone.IsInteractable)
		{
			return;
		}
		if (cursor != null)
		{
			MapVirtualCursor mapVirtualCursor = cursor as MapVirtualCursor;
			if (mapVirtualCursor != null)
			{
				mapVirtualCursor.DoAnimationTo(this.mapMilestone.NavigationRect);
			}
		}
		LazyUI.GetWindow<UIMapWindow>().OnEnterMapMilestone(this.mapMilestone);
	}

	// Token: 0x060044EB RID: 17643 RVA: 0x0014650C File Offset: 0x0014470C
	protected override void OnDeselect(BaseVirtualCursor cursor)
	{
		if (!this.mapMilestone.IsInteractable)
		{
			return;
		}
		if (cursor != null)
		{
			MapVirtualCursor mapVirtualCursor = cursor as MapVirtualCursor;
			if (mapVirtualCursor != null)
			{
				mapVirtualCursor.DoAnimationTo(null);
			}
		}
		LazyUI.GetWindow<UIMapWindow>().OnExitMapMilestone(this.mapMilestone);
	}

	// Token: 0x040035CB RID: 13771
	public UIMapMilestone mapMilestone;
}
