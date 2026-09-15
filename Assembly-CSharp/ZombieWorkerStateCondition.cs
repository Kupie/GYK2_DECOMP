using System;
using UnityEngine;

// Token: 0x020003F8 RID: 1016
[Serializable]
public class ZombieWorkerStateCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06001A8E RID: 6798 RVA: 0x0007BDA5 File Offset: 0x00079FA5
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ZombieWorkerStateChanged;
		}
	}

	// Token: 0x06001A8F RID: 6799 RVA: 0x0007BDAC File Offset: 0x00079FAC
	public static Type GetStateEnumType(ZombieType zombieType)
	{
		Type type;
		if (zombieType != ZombieType.Caretaker)
		{
			if (zombieType != ZombieType.Gardener)
			{
				if (zombieType != ZombieType.ConveyorTransporter)
				{
					type = null;
				}
				else
				{
					type = typeof(ZombieWgoData.ZombieConveyorTransporterState);
				}
			}
			else
			{
				type = typeof(ZombieWgoData.ZombieGardenerState);
			}
		}
		else
		{
			type = typeof(ZombieWgoData.ZombieCaretakerState);
		}
		return type;
	}

	// Token: 0x06001A90 RID: 6800 RVA: 0x0007BDF1 File Offset: 0x00079FF1
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.ZombieWgoData != null && context.ZombieWgoData.ZombieType == this.zombieType;
	}

	// Token: 0x06001A91 RID: 6801 RVA: 0x0007BE1C File Offset: 0x0007A01C
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		ZombieWgoData zombieWgoData = context.ZombieWgoData;
		int num;
		if (zombieWgoData == null || zombieWgoData.ZombieType != this.zombieType || !this.TryGetCurrentState(zombieWgoData, out num))
		{
			return false;
		}
		bool flag = num == this.conditionType;
		if (!this.invert)
		{
			return flag;
		}
		return !flag;
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x0007BE68 File Offset: 0x0007A068
	private bool TryGetCurrentState(ZombieWgoData zombieWgoData, out int state)
	{
		ZombieType zombieType = this.zombieType;
		if (zombieType == ZombieType.Caretaker)
		{
			state = (int)zombieWgoData.CaretakerState;
			return true;
		}
		if (zombieType == ZombieType.Gardener)
		{
			state = (int)zombieWgoData.GardenerState;
			return true;
		}
		if (zombieType != ZombieType.ConveyorTransporter)
		{
			state = 0;
			return false;
		}
		state = (int)zombieWgoData.ConveyorTransporterState;
		return true;
	}

	// Token: 0x040019B7 RID: 6583
	[Tooltip("Zombie type whose state is checked")]
	public ZombieType zombieType = ZombieType.Caretaker;

	// Token: 0x040019B8 RID: 6584
	[Tooltip("Expected state of the selected zombie type")]
	public int conditionType;

	// Token: 0x040019B9 RID: 6585
	[Tooltip("Inverts the result: true when the zombie is NOT in the expected state")]
	public bool invert;
}
