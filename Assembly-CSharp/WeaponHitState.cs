using System;
using System.Collections.Generic;

// Token: 0x02000354 RID: 852
public class WeaponHitState
{
	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06001662 RID: 5730 RVA: 0x0006BBE1 File Offset: 0x00069DE1
	// (set) Token: 0x06001663 RID: 5731 RVA: 0x0006BBE9 File Offset: 0x00069DE9
	public bool IsInsideProtectedZone { get; private set; }

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06001664 RID: 5732 RVA: 0x0006BBF2 File Offset: 0x00069DF2
	// (set) Token: 0x06001665 RID: 5733 RVA: 0x0006BBFA File Offset: 0x00069DFA
	public bool DamageDealt { get; private set; }

	// Token: 0x06001666 RID: 5734 RVA: 0x0006BC03 File Offset: 0x00069E03
	public void MarkDamageDealt()
	{
		this.DamageDealt = true;
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x0006BC0C File Offset: 0x00069E0C
	public void MarkZonePassed(HitZone zone)
	{
		this.passedZones.Add(zone.GetInstanceID());
		if (zone.ZoneType == HitZoneType.DoorFrontArea)
		{
			this.IsInsideProtectedZone = true;
		}
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x0006BC30 File Offset: 0x00069E30
	public bool HasPassedZone(HitZone zone)
	{
		return this.passedZones.Contains(zone.GetInstanceID());
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x0006BC43 File Offset: 0x00069E43
	public void Reset()
	{
		this.passedZones.Clear();
		this.IsInsideProtectedZone = false;
		this.DamageDealt = false;
	}

	// Token: 0x040016BB RID: 5819
	private readonly HashSet<int> passedZones = new HashSet<int>();
}
