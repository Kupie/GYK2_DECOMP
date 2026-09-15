using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using TMPro;
using UnityEngine;

// Token: 0x020009FF RID: 2559
public class UIMapZone : MonoBehaviour
{
	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x060044FA RID: 17658 RVA: 0x0014674B File Offset: 0x0014494B
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x060044FB RID: 17659 RVA: 0x00146753 File Offset: 0x00144953
	public List<UISortComponent> SortComponents
	{
		get
		{
			return this.sortComponents;
		}
	}

	// Token: 0x060044FC RID: 17660 RVA: 0x0014675C File Offset: 0x0014495C
	public void Init()
	{
		this.sortComponents = base.GetComponentsInChildren<UISortComponent>(true).ToList<UISortComponent>();
		this.worldZones = base.GetComponentsInChildren<UIMapWorldZone>(true).ToList<UIMapWorldZone>();
		base.gameObject.SetActive(false);
		if (this.cloudObject)
		{
			this.cloudObject.SetActive(true);
		}
	}

	// Token: 0x060044FD RID: 17661 RVA: 0x001467B4 File Offset: 0x001449B4
	public void Draw(bool isHidden)
	{
		this.isHidden = isHidden;
		if (this.alwaysVisible)
		{
			isHidden = false;
		}
		base.gameObject.SetActive(true);
		if (this.label != null)
		{
			this.label.text = LLBase.L("wz_" + this.id);
			this.label.gameObject.SetActive(!isHidden);
		}
		foreach (UISortComponent uisortComponent in this.SortComponents)
		{
			uisortComponent.gameObject.SetActive(isHidden);
		}
		foreach (UIMapWorldZone uimapWorldZone in this.worldZones)
		{
			bool flag = MainGame.Instance.GameSave.knowledgeSystem.visitedWorldZones.Contains(uimapWorldZone.WorldZoneId);
			uimapWorldZone.gameObject.SetActive(!isHidden && flag);
			uimapWorldZone.Label.text = LLBase.L("wz_" + uimapWorldZone.WorldZoneId);
		}
	}

	// Token: 0x040035D1 RID: 13777
	[SerializeField]
	private string id;

	// Token: 0x040035D2 RID: 13778
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x040035D3 RID: 13779
	[SerializeField]
	private GameObject cloudObject;

	// Token: 0x040035D4 RID: 13780
	[SerializeField]
	private bool alwaysVisible;

	// Token: 0x040035D5 RID: 13781
	private List<UIMapWorldZone> worldZones;

	// Token: 0x040035D6 RID: 13782
	private List<UISortComponent> sortComponents;

	// Token: 0x040035D7 RID: 13783
	private bool isHidden;
}
