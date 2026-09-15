using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020009FA RID: 2554
public class UIMapMilestone : MonoBehaviour
{
	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x060044D9 RID: 17625 RVA: 0x00146388 File Offset: 0x00144588
	public RectTransform RectTransform
	{
		get
		{
			return this.rectTransform;
		}
	}

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x060044DA RID: 17626 RVA: 0x00146390 File Offset: 0x00144590
	public RectTransform NavigationRect
	{
		get
		{
			return this.navigationRect;
		}
	}

	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x060044DB RID: 17627 RVA: 0x00146398 File Offset: 0x00144598
	// (set) Token: 0x060044DC RID: 17628 RVA: 0x001463A0 File Offset: 0x001445A0
	public UIMapMilestoneData UIMapMilestoneData { get; private set; }

	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x060044DD RID: 17629 RVA: 0x001463A9 File Offset: 0x001445A9
	// (set) Token: 0x060044DE RID: 17630 RVA: 0x001463B1 File Offset: 0x001445B1
	public WgoData WgoData { get; private set; }

	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x060044DF RID: 17631 RVA: 0x001463BA File Offset: 0x001445BA
	// (set) Token: 0x060044E0 RID: 17632 RVA: 0x001463C2 File Offset: 0x001445C2
	public bool IsInteractable { get; private set; }

	// Token: 0x060044E1 RID: 17633 RVA: 0x001463CB File Offset: 0x001445CB
	public void DrawAsActivated(UIMapMilestoneData milestoneData, WgoData wgoData, bool isInteractable, Action<UIMapMilestone> onClick)
	{
		this.activated.SetActive(true);
		this.notActivated.SetActive(false);
		this.UIMapMilestoneData = milestoneData;
		this.WgoData = wgoData;
		this.IsInteractable = isInteractable;
		this.onClick = onClick;
	}

	// Token: 0x060044E2 RID: 17634 RVA: 0x00146402 File Offset: 0x00144602
	public void DrawAsNotActivated(UIMapMilestoneData milestoneData, WgoData wgoData)
	{
		this.activated.SetActive(false);
		this.notActivated.SetActive(true);
		this.UIMapMilestoneData = milestoneData;
		this.WgoData = wgoData;
		this.IsInteractable = false;
		this.onClick = null;
	}

	// Token: 0x060044E3 RID: 17635 RVA: 0x00146438 File Offset: 0x00144638
	public void OnClick()
	{
		if (!this.IsInteractable)
		{
			return;
		}
		Action<UIMapMilestone> action = this.onClick;
		if (action != null)
		{
			action(this);
		}
		LazyAudio.PlayAndForget("gui_click");
	}

	// Token: 0x060044E4 RID: 17636 RVA: 0x0014645F File Offset: 0x0014465F
	public void OnEnter()
	{
		if (!this.IsInteractable)
		{
			return;
		}
		this.selection.SetActive(true);
		LazyAudio.PlayAndForget("gui_hover_light");
	}

	// Token: 0x060044E5 RID: 17637 RVA: 0x00146480 File Offset: 0x00144680
	public void OnExit()
	{
		this.selection.SetActive(false);
	}

	// Token: 0x060044E6 RID: 17638 RVA: 0x0014648E File Offset: 0x0014468E
	private void OnDisable()
	{
		this.OnExit();
	}

	// Token: 0x040035BC RID: 13756
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x040035BD RID: 13757
	[SerializeField]
	private GameObject activated;

	// Token: 0x040035BE RID: 13758
	[SerializeField]
	private GameObject notActivated;

	// Token: 0x040035BF RID: 13759
	[SerializeField]
	private GameObject selection;

	// Token: 0x040035C0 RID: 13760
	[SerializeField]
	private RectTransform navigationRect;

	// Token: 0x040035C1 RID: 13761
	private Action<UIMapMilestone> onClick;
}
