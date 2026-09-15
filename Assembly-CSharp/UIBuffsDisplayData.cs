using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000873 RID: 2163
public class UIBuffsDisplayData : LazyWidgetDataBase
{
	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x0600375A RID: 14170 RVA: 0x0010B228 File Offset: 0x00109428
	// (remove) Token: 0x0600375B RID: 14171 RVA: 0x0010B260 File Offset: 0x00109460
	public event Action<UIBuffElementData> OnBuffAdded;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x0600375C RID: 14172 RVA: 0x0010B298 File Offset: 0x00109498
	// (remove) Token: 0x0600375D RID: 14173 RVA: 0x0010B2D0 File Offset: 0x001094D0
	public event Action<string> OnBuffRemoved;

	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x0600375E RID: 14174 RVA: 0x0010B308 File Offset: 0x00109508
	// (remove) Token: 0x0600375F RID: 14175 RVA: 0x0010B340 File Offset: 0x00109540
	public event Action<string> OnBuffUpdated;

	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x06003760 RID: 14176 RVA: 0x0010B378 File Offset: 0x00109578
	// (remove) Token: 0x06003761 RID: 14177 RVA: 0x0010B3B0 File Offset: 0x001095B0
	public event Action<string> OnBuffFxRequested;

	// Token: 0x1700083A RID: 2106
	// (get) Token: 0x06003762 RID: 14178 RVA: 0x0010B3E5 File Offset: 0x001095E5
	// (set) Token: 0x06003763 RID: 14179 RVA: 0x0010B3ED File Offset: 0x001095ED
	public List<UIBuffElementData> UIBuffElementsData { get; set; }

	// Token: 0x06003764 RID: 14180 RVA: 0x0010B3F8 File Offset: 0x001095F8
	public UIBuffsDisplayData(List<PerkType> drawingTypes)
	{
		this.drawingTypes = drawingTypes;
		this.UIBuffElementsData = new List<UIBuffElementData>();
		for (int i = 0; i < MainGame.Instance.GameSave.perkSystemData.activePerks.Count; i++)
		{
			PerkData perkData = MainGame.Instance.GameSave.perkSystemData.activePerks[i];
			if (drawingTypes.Contains(perkData.Definition.perkType))
			{
				UIBuffElementData uibuffElementData = new UIBuffElementData(perkData, perkData.currentDuration, perkData.Definition.hiddenTimer, perkData.Definition.duration < 0f);
				this.UIBuffElementsData.Add(uibuffElementData);
			}
		}
		MainGame.Instance.GameSave.perkSystemData.OnPerkAdded += this.AddPerkElementData;
		MainGame.Instance.GameSave.perkSystemData.OnPerksUpdated += this.UpdatePerkElementsData;
		MainGame.Instance.GameSave.perkSystemData.OnPerkRemoved += this.RemovePerkElementData;
		MainGame.Instance.GameSave.perkSystemData.OnPerkReapplied += this.RequestBuffFx;
	}

	// Token: 0x06003765 RID: 14181 RVA: 0x0010B528 File Offset: 0x00109728
	private void AddPerkElementData(PerkData perkData)
	{
		if (perkData.Definition.isHidden)
		{
			return;
		}
		if (!this.drawingTypes.Contains(perkData.Definition.perkType))
		{
			return;
		}
		if (this.UIBuffElementsData.Find((UIBuffElementData x) => x.PerkData.id == perkData.id) != null)
		{
			Debug.LogWarning("[UIBuffsDisplayData]: buff [" + perkData.id + "] has been already added");
			return;
		}
		UIBuffElementData uibuffElementData = new UIBuffElementData(perkData, perkData.currentDuration, perkData.Definition.hiddenTimer, perkData.Definition.duration < 0f);
		this.UIBuffElementsData.Add(uibuffElementData);
		Action<UIBuffElementData> onBuffAdded = this.OnBuffAdded;
		if (onBuffAdded == null)
		{
			return;
		}
		onBuffAdded(uibuffElementData);
	}

	// Token: 0x06003766 RID: 14182 RVA: 0x0010B608 File Offset: 0x00109808
	private void RequestBuffFx(PerkData perkData)
	{
		if (perkData.Definition.isHidden)
		{
			return;
		}
		if (!this.drawingTypes.Contains(perkData.Definition.perkType))
		{
			return;
		}
		if (this.UIBuffElementsData.Find((UIBuffElementData x) => x.PerkData.id == perkData.id) == null)
		{
			return;
		}
		Action<string> onBuffFxRequested = this.OnBuffFxRequested;
		if (onBuffFxRequested == null)
		{
			return;
		}
		onBuffFxRequested(perkData.id);
	}

	// Token: 0x06003767 RID: 14183 RVA: 0x0010B688 File Offset: 0x00109888
	private void RemovePerkElementData(PerkData perkData)
	{
		if (perkData.Definition.isHidden)
		{
			return;
		}
		if (!this.drawingTypes.Contains(perkData.Definition.perkType))
		{
			return;
		}
		UIBuffElementData uibuffElementData = this.UIBuffElementsData.Find((UIBuffElementData x) => x.PerkData.id == perkData.id);
		if (uibuffElementData == null)
		{
			Debug.LogWarning("[UIBuffsDisplayData]: buff [" + perkData.id + "] has been already removed");
			return;
		}
		this.UIBuffElementsData.Remove(uibuffElementData);
		Action<string> onBuffRemoved = this.OnBuffRemoved;
		if (onBuffRemoved == null)
		{
			return;
		}
		onBuffRemoved(uibuffElementData.PerkData.id);
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x0010B738 File Offset: 0x00109938
	private void UpdatePerkElementsData(List<PerkData> buffsData)
	{
		using (List<PerkData>.Enumerator enumerator = buffsData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PerkData buffData = enumerator.Current;
				if (!buffData.Definition.isHidden && this.drawingTypes.Contains(buffData.Definition.perkType))
				{
					UIBuffElementData uibuffElementData = this.UIBuffElementsData.Find((UIBuffElementData x) => x.PerkData.id == buffData.id);
					if (uibuffElementData == null)
					{
						Debug.LogWarning("[UIBuffsDisplayData]: buff [" + buffData.id + "] tries to update data but it has been removed");
					}
					else
					{
						uibuffElementData.UpdateData(buffData.currentDuration);
						Action<string> onBuffUpdated = this.OnBuffUpdated;
						if (onBuffUpdated != null)
						{
							onBuffUpdated(uibuffElementData.PerkData.id);
						}
					}
				}
			}
		}
	}

	// Token: 0x04002C13 RID: 11283
	private List<PerkType> drawingTypes;
}
