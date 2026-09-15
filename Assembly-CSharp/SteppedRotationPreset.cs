using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class SteppedRotationPreset
{
	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0004E1FA File Offset: 0x0004C3FA
	protected virtual List<Sector> PossibleSectors { get; } = new List<Sector>
	{
		new Sector(-180f, -90f),
		new Sector(-90f, 0f),
		new Sector(0f, 180f)
	};

	// Token: 0x06000F1F RID: 3871 RVA: 0x0004E202 File Offset: 0x0004C402
	public float ComputeAngle(Vector2 direction)
	{
		return this.GetPossibleAngleByGiven(direction);
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x0004E20C File Offset: 0x0004C40C
	public Vector2 ComputeDirection(Vector2 direction)
	{
		float possibleAngleByGiven = this.GetPossibleAngleByGiven(direction);
		return new Vector2(Mathf.Cos(possibleAngleByGiven * 0.017453292f), Mathf.Sin(possibleAngleByGiven * 0.017453292f));
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0004E240 File Offset: 0x0004C440
	public Direction GetDirectionFromAngle(float angle)
	{
		if (angle.Equals(180f) || angle.Equals(-180f))
		{
			List<Sector> possibleSectors = this.PossibleSectors;
			return possibleSectors[possibleSectors.Count - 1].Direction;
		}
		for (int i = 0; i < this.PossibleSectors.Count; i++)
		{
			if (angle >= this.PossibleSectors[i].AngleL && angle < this.PossibleSectors[i].AngleG)
			{
				return this.PossibleSectors[i].Direction;
			}
		}
		return Direction.None;
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0004E2DF File Offset: 0x0004C4DF
	public float ComputeAngleSmooth(Vector2 direction)
	{
		return this.ComputePossibleAnglesWithSmooth(direction);
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0004E2E8 File Offset: 0x0004C4E8
	public Vector2 ComputeDirectionSmooth(Vector2 direction)
	{
		float num = this.ComputePossibleAnglesWithSmooth(direction);
		return new Vector2(Mathf.Cos(num * 0.017453292f), Mathf.Sin(num * 0.017453292f));
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x0004E31A File Offset: 0x0004C51A
	public virtual float GetSteppedDirection(Direction direction)
	{
		return this.GetPossibleAngleByGiven(direction.ConvertToVector2XZ());
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0004E328 File Offset: 0x0004C528
	protected virtual float ComputePossibleAnglesWithSmooth(Vector2 direction)
	{
		float num = Vector2.SignedAngle(Vector2.right, direction);
		if (num.Equals(this.PossibleSectors[0].AngleL) || num.Equals(this.PossibleSectors[this.PossibleSectors.Count - 1].AngleG))
		{
			return this.currentAngle = num;
		}
		if (num >= this.currentSubSector.AngleL && num < this.currentSubSector.AngleG)
		{
			this.angleAccumThreshold = 0f;
		}
		else
		{
			float num2 = Mathf.Min(Mathf.Abs(num - this.currentSubSector.AngleL), Mathf.Abs(num - this.currentSubSector.AngleG));
			this.angleAccumThreshold += num2;
		}
		if (this.angleAccumThreshold >= this.angleThreshold)
		{
			this.angleAccumThreshold = 0f;
			return this.currentAngle = this.GetPossibleAngleByGiven(direction);
		}
		return this.currentAngle;
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0004E424 File Offset: 0x0004C624
	protected float GetPossibleAngleByGiven(Vector2 direction)
	{
		float num = Vector2.SignedAngle(Vector2.right, direction);
		foreach (Sector sector in this.PossibleSectors)
		{
			if (num >= sector.AngleL && num < sector.AngleG)
			{
				this.currentSector = sector;
				this.currentAngle = this.currentSector.GetClosestAngleByGiven(num);
				float num2 = (this.currentSector.AngleG + this.currentSector.AngleL) / 2f;
				this.currentSubSector = ((this.currentAngle == this.currentSector.AngleL) ? new Sector(this.currentSector.AngleL, num2) : new Sector(num2, this.currentSector.AngleG));
				return this.currentAngle;
			}
		}
		this.currentSector = this.PossibleSectors[0];
		return this.currentAngle = this.currentSector.AngleL;
	}

	// Token: 0x040011FC RID: 4604
	protected static readonly Sector nullSector = new Sector(0f, 0f, Sector.RoundType.GetLower, Direction.None);

	// Token: 0x040011FD RID: 4605
	private Sector currentSector = SteppedRotationPreset.nullSector;

	// Token: 0x040011FE RID: 4606
	private Sector currentSubSector = SteppedRotationPreset.nullSector;

	// Token: 0x040011FF RID: 4607
	private float angleAccumThreshold;

	// Token: 0x04001200 RID: 4608
	private float angleThreshold = 10f;

	// Token: 0x04001201 RID: 4609
	private float currentAngle;
}
