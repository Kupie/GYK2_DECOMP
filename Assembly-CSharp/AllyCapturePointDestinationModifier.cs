using System;

// Token: 0x020002DF RID: 735
public class AllyCapturePointDestinationModifier : ControlPointDestinationModifier
{
	// Token: 0x0600133B RID: 4923 RVA: 0x0005E24C File Offset: 0x0005C44C
	public AllyCapturePointDestinationModifier(FightingCapturePoint capturePoint)
		: base(capturePoint)
	{
	}

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x0600133C RID: 4924 RVA: 0x00028294 File Offset: 0x00026494
	public override bool ShouldAnchorOnArrival
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x0600133D RID: 4925 RVA: 0x00028294 File Offset: 0x00026494
	protected override bool AcceptWaitingSlot
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x0600133E RID: 4926 RVA: 0x0005E255 File Offset: 0x0005C455
	public override bool IsValid
	{
		get
		{
			return base.TargetControlPoint && !base.TargetControlPoint.IsOnCapturePoint(base.Wgo.Data.Position);
		}
	}
}
