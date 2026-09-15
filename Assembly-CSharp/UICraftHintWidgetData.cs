using System;
using LazyBearTechnology;

// Token: 0x020007DA RID: 2010
public class UICraftHintWidgetData : LazyWidgetDataBase
{
	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x060033DD RID: 13277 RVA: 0x000FA8D6 File Offset: 0x000F8AD6
	// (set) Token: 0x060033DE RID: 13278 RVA: 0x000FA8DE File Offset: 0x000F8ADE
	public CraftComponent CraftComponent { get; private set; }

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x060033DF RID: 13279 RVA: 0x000FA8E7 File Offset: 0x000F8AE7
	public CraftElementBase CraftElement
	{
		get
		{
			return this.CraftComponent.CurrentCraftElement;
		}
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x060033E0 RID: 13280 RVA: 0x000FA8F4 File Offset: 0x000F8AF4
	public IWorker Worker
	{
		get
		{
			return this.CraftComponent.CraftableObject.CraftableAttachedWorker;
		}
	}

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x060033E1 RID: 13281 RVA: 0x000FA906 File Offset: 0x000F8B06
	// (set) Token: 0x060033E2 RID: 13282 RVA: 0x000FA90E File Offset: 0x000F8B0E
	public bool IsPlantingCraft { get; private set; }

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x060033E3 RID: 13283 RVA: 0x000FA918 File Offset: 0x000F8B18
	public bool IsWgoUnderInteraction
	{
		get
		{
			Wgo wgoUnderInteraction = MainGame.PlayerController.PlayerInteractionComponent.WgoUnderInteraction;
			if (!(((wgoUnderInteraction != null) ? wgoUnderInteraction.Data.UniqueId : null) == this.wgoUniqueId))
			{
				Wgo wgo = MainGame.PlayerController.PlayerWorkComponent.Wgo;
				SGuid sguid;
				if (wgo == null)
				{
					sguid = null;
				}
				else
				{
					WgoData data = wgo.Data;
					sguid = ((data != null) ? data.UniqueId : null);
				}
				return sguid == this.wgoUniqueId;
			}
			return true;
		}
	}

	// Token: 0x060033E4 RID: 13284 RVA: 0x000FA986 File Offset: 0x000F8B86
	public UICraftHintWidgetData(CraftComponent craftComponent)
	{
		this.CraftComponent = craftComponent;
		this.IsPlantingCraft = this.CraftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.GardenPlanting;
	}

	// Token: 0x0400295F RID: 10591
	public SGuid wgoUniqueId;
}
