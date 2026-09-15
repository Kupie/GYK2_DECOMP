using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200086F RID: 2159
public class UIBuffsDisplay : LazyWidget<UIBuffsDisplayData>
{
	// Token: 0x17000838 RID: 2104
	// (get) Token: 0x06003744 RID: 14148 RVA: 0x0010ACC4 File Offset: 0x00108EC4
	public RectTransform RectTransform
	{
		get
		{
			return base.transform as RectTransform;
		}
	}

	// Token: 0x17000839 RID: 2105
	// (get) Token: 0x06003745 RID: 14149 RVA: 0x0010ACD1 File Offset: 0x00108ED1
	public RectTransform Content
	{
		get
		{
			return this.content;
		}
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x0010ACD9 File Offset: 0x00108ED9
	public override void Init()
	{
		this.uiBuffElementPrefab.gameObject.SetActive(false);
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x0010ACEC File Offset: 0x00108EEC
	protected override void SetData(UIBuffsDisplayData data)
	{
		base.SetData(data);
		data.OnBuffAdded += this.AddBuffToDisplay;
		data.OnBuffRemoved += this.RemoveBuffFromDisplay;
		data.OnBuffUpdated += this.UpdateBuffOnDisplay;
		data.OnBuffFxRequested += this.PlayAppliedFx;
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x0010AD48 File Offset: 0x00108F48
	public override void Redraw()
	{
		base.Redraw();
		for (int i = 0; i < this.data.UIBuffElementsData.Count; i++)
		{
			this.AddBuffToDisplay(this.data.UIBuffElementsData[i], false);
		}
		this.content.sizeDelta = ((GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Big) ? this.sizeBig : this.sizeSmall);
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x0010ADB4 File Offset: 0x00108FB4
	public override void Hide()
	{
		if (this.data != null)
		{
			this.data.OnBuffAdded -= this.AddBuffToDisplay;
			this.data.OnBuffRemoved -= this.RemoveBuffFromDisplay;
			this.data.OnBuffUpdated -= this.UpdateBuffOnDisplay;
			this.data.OnBuffFxRequested -= this.PlayAppliedFx;
		}
		for (int i = this.activeElements.Count - 1; i >= 0; i--)
		{
			this.activeElements[i].gameObject.SetActive(false);
			this.elementsPool.Add(this.activeElements[i]);
			this.activeElements.Remove(this.activeElements[i]);
		}
		if (this.activeElements.Count == 0)
		{
			this.content.gameObject.SetActive(false);
		}
		base.Hide();
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x0010AEA7 File Offset: 0x001090A7
	private void AddBuffToDisplay(UIBuffElementData elementData)
	{
		this.AddBuffToDisplay(elementData, true);
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x0010AEB4 File Offset: 0x001090B4
	private void AddBuffToDisplay(UIBuffElementData elementData, bool playAppliedFx)
	{
		UIBuffElement @new = this.GetNew();
		@new.gameObject.SetActive(true);
		@new.Draw(elementData);
		if (playAppliedFx)
		{
			this.PlayAppliedFx(@new, elementData.PerkData.Definition);
		}
		this.activeElements.Add(@new);
		if (!this.content.gameObject.activeSelf)
		{
			this.content.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x0010AF20 File Offset: 0x00109120
	private void PlayAppliedFx(string buffId)
	{
		UIBuffElement uibuffElement = this.activeElements.Find((UIBuffElement x) => x.Id == buffId);
		if (uibuffElement == null)
		{
			return;
		}
		UIBuffElementData uibuffElementData = this.data.UIBuffElementsData.Find((UIBuffElementData x) => x.PerkData.id == buffId);
		this.PlayAppliedFx(uibuffElement, (uibuffElementData != null) ? uibuffElementData.PerkData.Definition : null);
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x0010AF94 File Offset: 0x00109194
	private void PlayAppliedFx(UIBuffElement element, PerkDef def)
	{
		element.PlayHudFx();
		if (def == null || string.IsNullOrEmpty(def.worldFxPrefabId))
		{
			return;
		}
		PlayerController playerController = MainGame.PlayerController;
		PlayerView playerView = ((playerController != null) ? playerController.View : null);
		if (playerView == null)
		{
			WorldFX.Spawn(MainGame.PlayerData.position.Value, def.worldFxPrefabId, null, default(Vector3));
			return;
		}
		WorldFX worldFX = WorldFX.Spawn(playerView.transform.position, def.worldFxPrefabId, null, default(Vector3));
		if (worldFX == null)
		{
			return;
		}
		worldFX.Follow(playerView.transform);
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x0010B030 File Offset: 0x00109230
	private void RemoveBuffFromDisplay(string buffId)
	{
		UIBuffElement uibuffElement = this.activeElements.Find((UIBuffElement x) => x.Id == buffId);
		if (uibuffElement != null)
		{
			uibuffElement.gameObject.SetActive(false);
			this.activeElements.Remove(uibuffElement);
			this.elementsPool.Add(uibuffElement);
		}
		if (this.activeElements.Count == 0)
		{
			this.content.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x0010B0B0 File Offset: 0x001092B0
	private void UpdateBuffOnDisplay(string buffId)
	{
		UIBuffElement uibuffElement = this.activeElements.Find((UIBuffElement x) => x.Id == buffId);
		if (uibuffElement == null)
		{
			return;
		}
		uibuffElement.Redraw();
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x0010B0EB File Offset: 0x001092EB
	private UIBuffElement GetNew()
	{
		if (this.elementsPool.Count == 0)
		{
			return this.uiBuffElementPrefab.Copy(null, false, "");
		}
		return this.elementsPool.PopLast<UIBuffElement>();
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x0010B118 File Offset: 0x00109318
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIBuffsDisplayData(new List<PerkType> { PerkType.Buff })
		{
			UIBuffElementsData = new List<UIBuffElementData>
			{
				new UIBuffElementData(new PerkData("test1"), 20f, false, false),
				new UIBuffElementData(new PerkData("test2"), 100f, false, false),
				new UIBuffElementData(new PerkData("test3"), 70f, true, false),
				new UIBuffElementData(new PerkData("test3"), -1f, true, true)
			}
		});
	}

	// Token: 0x04002C05 RID: 11269
	[SerializeField]
	private UIBuffElement uiBuffElementPrefab;

	// Token: 0x04002C06 RID: 11270
	[SerializeField]
	private RectTransform content;

	// Token: 0x04002C07 RID: 11271
	[SerializeField]
	private Vector2 sizeBig;

	// Token: 0x04002C08 RID: 11272
	[SerializeField]
	private Vector2 sizeSmall;

	// Token: 0x04002C09 RID: 11273
	private List<UIBuffElement> activeElements = new List<UIBuffElement>();

	// Token: 0x04002C0A RID: 11274
	private List<UIBuffElement> elementsPool = new List<UIBuffElement>();
}
