using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x0200087E RID: 2174
public class UIGameResNotificatorData : LazyWidgetDataBase
{
	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x0600378C RID: 14220 RVA: 0x0010C03F File Offset: 0x0010A23F
	// (set) Token: 0x0600378D RID: 14221 RVA: 0x0010C047 File Offset: 0x0010A247
	public Dictionary<string, UIResElementData> ResElementsWithAccumulators { get; private set; }

	// Token: 0x17000840 RID: 2112
	// (get) Token: 0x0600378E RID: 14222 RVA: 0x0010C050 File Offset: 0x0010A250
	// (set) Token: 0x0600378F RID: 14223 RVA: 0x0010C058 File Offset: 0x0010A258
	public Dictionary<string, UIResElementData> ResElementsWithoutAccumulators { get; private set; }

	// Token: 0x06003790 RID: 14224 RVA: 0x0010C064 File Offset: 0x0010A264
	public UIGameResNotificatorData()
	{
		this.ResElementsWithAccumulators = new Dictionary<string, UIResElementData>();
		this.ResElementsWithoutAccumulators = new Dictionary<string, UIResElementData>();
		PlayerEnergyGameResSystem system = PlayerEnergyGameResSystem.GetSystem();
		system.onValueDeltaChanged = (Action<float>)Delegate.Combine(system.onValueDeltaChanged, new Action<float>(this.OnEnergyDeltaChanged));
		PlayerInsanityGameResSystem system2 = PlayerInsanityGameResSystem.GetSystem();
		system2.onValueDeltaChanged = (Action<float>)Delegate.Combine(system2.onValueDeltaChanged, new Action<float>(this.OnInstanityDeltaChanged));
		PlayerHPActivity.OnNotEnoughResOccurred += this.OnNotEnoughResOccurred;
		PlayerCraftActivity.OnNotEnoughResOccurred += this.OnNotEnoughResOccurred;
		ZombieWgoData.OnTechPointsAddedToZombie += this.OnZombieTechPointsAdded;
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x0010C10C File Offset: 0x0010A30C
	private void OnInspirationCompleted(string id)
	{
		UIResElementData uiresElementData;
		if (this.ResElementsWithAccumulators.TryGetValue(id, out uiresElementData))
		{
			uiresElementData.AddValue(1f);
			return;
		}
		UIResElementData uiresElementData2 = new UIResElementData(id, UIGameResDisplayingType.AppearOverTargetType, "hint_inspiration", 1f, MainGame.PlayerController.PhysicalBody.PlayerView.BubblePoint, false, null);
		this.ResElementsWithAccumulators.Add(id, uiresElementData2);
	}

	// Token: 0x06003792 RID: 14226 RVA: 0x0010C16C File Offset: 0x0010A36C
	private void OnZombieTechPointsAdded(WgoData wgoData, ZombieWgoData zombieWgoData, string type, int value)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		if (wgoViewGlobal != null && wgoViewGlobal.MainWgoPart != null)
		{
			this.ResElementsWithAccumulators[type] = new UIResElementData(type, UIGameResDisplayingType.AppearOverTargetType, type, (float)value, wgoViewGlobal.MainWgoPart.BubblePoint, false, null);
		}
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x0010C1C0 File Offset: 0x0010A3C0
	private void OnEnergyDeltaChanged(float value)
	{
		this.TryAddFromAccumulator("energy", "energy", value);
	}

	// Token: 0x06003794 RID: 14228 RVA: 0x0010C1D3 File Offset: 0x0010A3D3
	private void OnInstanityDeltaChanged(float value)
	{
		this.TryAddFromAccumulator("insanity", "insanity", value);
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x0010C1E6 File Offset: 0x0010A3E6
	private void OnNotEnoughResOccurred(string resId)
	{
		if (resId == "energy")
		{
			this.AddIfNotShowed(resId, "no_energy");
			return;
		}
		if (!(resId == "insanity"))
		{
			return;
		}
		this.AddIfNotShowed(resId, "no_insanity");
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x0010C21C File Offset: 0x0010A41C
	private void TryAddFromAccumulator(string id, string iconId, float value)
	{
		if (!this.ResElementsWithAccumulators.ContainsKey(id))
		{
			UIResElementData uiresElementData = new UIResElementData(id, UIGameResDisplayingType.AppearOverTargetType, iconId, value, MainGame.PlayerController.PhysicalBody.PlayerView.BubblePoint, false, null);
			this.ResElementsWithAccumulators.Add(id, uiresElementData);
			return;
		}
		this.ResElementsWithAccumulators[id].AddValue(value);
	}

	// Token: 0x06003797 RID: 14231 RVA: 0x0010C278 File Offset: 0x0010A478
	private void AddIfNotShowed(string id, string iconId)
	{
		if (this.ResElementsWithoutAccumulators.ContainsKey(id))
		{
			return;
		}
		UIResElementData uiresElementData = new UIResElementData(id, UIGameResDisplayingType.AppearOverTargetType, iconId, 0f, MainGame.PlayerController.PhysicalBody.PlayerView.BubblePoint, false, null);
		this.ResElementsWithoutAccumulators.Add(id, uiresElementData);
	}

	// Token: 0x04002C3E RID: 11326
	public Action<UIResElementData> onResAdd;

	// Token: 0x04002C3F RID: 11327
	private bool subscribedEvents;
}
