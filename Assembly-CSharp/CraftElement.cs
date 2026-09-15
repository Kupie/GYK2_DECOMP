using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000269 RID: 617
[Serializable]
public class CraftElement : CraftElementT<CraftDef>
{
	// Token: 0x06000FD8 RID: 4056 RVA: 0x00050F1C File Offset: 0x0004F11C
	public CraftElement(string craftId, int count, CraftParamsData craftParamsData)
		: this(craftId, count, new List<NeedItemData>(), craftParamsData)
	{
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x00050F2C File Offset: 0x0004F12C
	public CraftElement(string craftId, int count, List<NeedItemData> requirements, CraftParamsData paramsData)
		: base(craftId, count, requirements, paramsData)
	{
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x00050F39 File Offset: 0x0004F139
	public CraftElement(CraftDefBase definition)
		: base(definition)
	{
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x00050F42 File Offset: 0x0004F142
	public CraftElement(CraftDefBase definition, CraftParamsData craftParamsData)
		: base(definition, craftParamsData)
	{
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x00050F4C File Offset: 0x0004F14C
	protected CraftElement(CraftElement other, int count = 1)
		: base(other, count)
	{
		this.durabilityUse = other.durabilityUse;
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x00050F62 File Offset: 0x0004F162
	public override CraftElementBase Clone(int count = 1)
	{
		return new CraftElement(this, count);
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x00050F6C File Offset: 0x0004F16C
	public override CraftStatus CanStartCraft(ICraftable craftableObject, MultiInventory multiInventory = null)
	{
		CraftDef definition = base.Definition;
		IWorker craftableAttachedWorker = craftableObject.CraftableAttachedWorker;
		Inventory inventory = ((craftableAttachedWorker != null) ? craftableAttachedWorker.WorkerInventory : null);
		if (definition.hasDurabilityUseItem && inventory != null && !inventory.Data.HasItemWithEnoughDurability(definition.durabilityUseItem.Id, definition.needItemsDurabilityUse))
		{
			return CraftStatus.DoesntHaveItemWithEnoughDurability;
		}
		return base.CanStartCraft(craftableObject, multiInventory);
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x00050FC8 File Offset: 0x0004F1C8
	public override void Finish()
	{
		base.Finish();
		DurabilitySerializedItemProperty durabilitySerializedItemProperty;
		if (this.durabilityUse != null && this.durabilityUse.TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty))
		{
			durabilitySerializedItemProperty.Durability -= base.Definition.needItemsDurabilityUse;
		}
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x0005100C File Offset: 0x0004F20C
	public override void RemoveCraftRequirements(ICraftable craftable)
	{
		base.RemoveCraftRequirements(craftable);
		IWorker craftableAttachedWorker = craftable.CraftableAttachedWorker;
		Inventory inventory = ((craftableAttachedWorker != null) ? craftableAttachedWorker.WorkerInventory : null);
		CraftDef definition = base.Definition;
		if (definition.hasDurabilityUseItem && inventory != null)
		{
			this.durabilityUse = inventory.Data.GetAndRemoveItemWithEnoughDurability(definition.durabilityUseItem.Id, definition.needItemsDurabilityUse);
		}
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x00051068 File Offset: 0x0004F268
	public override int GetCurrentQuality()
	{
		if (!base.Def.isStarCraft)
		{
			return -1;
		}
		if (base.Definition.goldLevel == 0 && base.Definition.silverLevel == 0 && base.Definition.bronzeLevel == 0)
		{
			return -1;
		}
		int num = 0;
		if (this.succeededProgressTicks >= base.Definition.goldLevel)
		{
			num = 3;
		}
		else if (this.succeededProgressTicks >= base.Definition.silverLevel)
		{
			num = 2;
		}
		else if (this.succeededProgressTicks >= base.Definition.bronzeLevel)
		{
			num = 1;
		}
		return num;
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x000510F3 File Offset: 0x0004F2F3
	public override CraftStatus CheckWorkerDependentValues(IWorker worker, float deltaTime = 1f, bool skipEnergyCheck = false, bool skipInsanityCheck = false)
	{
		if (this.paramsData.customRes.GetInt("do_not_check_worker_dependent_values") == 1)
		{
			return CraftStatus.OK;
		}
		if (worker != null)
		{
			return worker.CheckWorkerDependentValues(this, deltaTime, skipEnergyCheck, skipInsanityCheck);
		}
		return MainGame.PlayerController.CheckWorkerDependentValues(this, deltaTime, skipEnergyCheck, skipInsanityCheck);
	}

	// Token: 0x04001276 RID: 4726
	[SerializeField]
	private Item durabilityUse;
}
