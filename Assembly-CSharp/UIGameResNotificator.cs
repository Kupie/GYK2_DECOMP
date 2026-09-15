using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200087D RID: 2173
public class UIGameResNotificator : LazyWidget<UIGameResNotificatorData>
{
	// Token: 0x06003786 RID: 14214 RVA: 0x0010BE57 File Offset: 0x0010A057
	public override void Init()
	{
		this.uiResPrefab.gameObject.SetActive(false);
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x0010BE6A File Offset: 0x0010A06A
	private void OnDisplayingCompleted(UIResElement uiResElement)
	{
		uiResElement.gameObject.SetActive(false);
		this.labelsPool.Add(uiResElement);
	}

	// Token: 0x06003788 RID: 14216 RVA: 0x0010BE84 File Offset: 0x0010A084
	private UIResElement GetNew()
	{
		if (this.labelsPool.Count == 0)
		{
			UIResElement uiresElement = this.uiResPrefab.Copy(null, false, "");
			uiresElement.Init(new Action<UIResElement>(this.OnDisplayingCompleted));
			return uiresElement;
		}
		return this.labelsPool.PopLast<UIResElement>();
	}

	// Token: 0x06003789 RID: 14217 RVA: 0x0010BEC4 File Offset: 0x0010A0C4
	private void Update()
	{
		foreach (UIResElementData uiresElementData in this.data.ResElementsWithAccumulators.Values)
		{
			if (Mathf.Abs(uiresElementData.Value).EqualsOrMore(1f, 1E-05f) && Time.time - this.lastDisplayedTime > this.delayBetweenElements)
			{
				this.lastDisplayedTime = Time.time;
				UIResElement @new = this.GetNew();
				@new.gameObject.SetActive(true);
				@new.Draw(uiresElementData);
				return;
			}
		}
		foreach (string text in this.data.ResElementsWithoutAccumulators.Keys)
		{
			if (Time.time - this.lastDisplayedTime > this.delayBetweenElements)
			{
				this.lastDisplayedTime = Time.time;
				UIResElement new2 = this.GetNew();
				new2.gameObject.SetActive(true);
				new2.Draw(this.data.ResElementsWithoutAccumulators[text]);
				this.data.ResElementsWithoutAccumulators.Remove(text);
				break;
			}
		}
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x0010C014 File Offset: 0x0010A214
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIGameResNotificatorData());
	}

	// Token: 0x04002C3A RID: 11322
	[SerializeField]
	private UIResElement uiResPrefab;

	// Token: 0x04002C3B RID: 11323
	[SerializeField]
	private float delayBetweenElements = 0.3f;

	// Token: 0x04002C3C RID: 11324
	private float lastDisplayedTime;

	// Token: 0x04002C3D RID: 11325
	private List<UIResElement> labelsPool = new List<UIResElement>();
}
