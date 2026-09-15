using System;
using System.Collections.Generic;

// Token: 0x0200024F RID: 591
public class BasicNpcSteppedRotationPreset : SteppedRotationPreset
{
	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0004DFEF File Offset: 0x0004C1EF
	public static BasicNpcSteppedRotationPreset Instance
	{
		get
		{
			BasicNpcSteppedRotationPreset basicNpcSteppedRotationPreset;
			if ((basicNpcSteppedRotationPreset = BasicNpcSteppedRotationPreset.instance) == null)
			{
				basicNpcSteppedRotationPreset = (BasicNpcSteppedRotationPreset.instance = new BasicNpcSteppedRotationPreset());
			}
			return basicNpcSteppedRotationPreset;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0004E005 File Offset: 0x0004C205
	protected override List<Sector> PossibleSectors
	{
		get
		{
			return this.possibleSectors;
		}
	}

	// Token: 0x040011F1 RID: 4593
	private static BasicNpcSteppedRotationPreset instance;

	// Token: 0x040011F2 RID: 4594
	private List<Sector> possibleSectors = new List<Sector>
	{
		new Sector(-180f, -90f, Sector.RoundType.GetLower, Direction.Left),
		new Sector(-90f, 0f, Sector.RoundType.GetGreater, Direction.Down),
		new Sector(0f, 90f, Sector.RoundType.GetLower, Direction.Right),
		new Sector(90f, 180f, Sector.RoundType.GetGreater, Direction.Up),
		new Sector(180f, 180f, Sector.RoundType.GetGreater, Direction.Left)
	};
}
