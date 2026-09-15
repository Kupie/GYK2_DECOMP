using System;
using System.Collections.Generic;

// Token: 0x02000250 RID: 592
public class DonkeySteppedRotationPreset : SteppedRotationPreset
{
	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0004E0A1 File Offset: 0x0004C2A1
	public static DonkeySteppedRotationPreset Instance
	{
		get
		{
			DonkeySteppedRotationPreset donkeySteppedRotationPreset;
			if ((donkeySteppedRotationPreset = DonkeySteppedRotationPreset.instance) == null)
			{
				donkeySteppedRotationPreset = (DonkeySteppedRotationPreset.instance = new DonkeySteppedRotationPreset());
			}
			return donkeySteppedRotationPreset;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0004E0B7 File Offset: 0x0004C2B7
	protected override List<Sector> PossibleSectors
	{
		get
		{
			return this.possibleSectors;
		}
	}

	// Token: 0x040011F3 RID: 4595
	private static DonkeySteppedRotationPreset instance;

	// Token: 0x040011F4 RID: 4596
	private List<Sector> possibleSectors = new List<Sector>
	{
		new Sector(-180f, 0f, Sector.RoundType.GetLower, Direction.Left),
		new Sector(0f, 180f, Sector.RoundType.GetGreater, Direction.Right)
	};
}
