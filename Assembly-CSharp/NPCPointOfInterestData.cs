using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001BA RID: 442
[Serializable]
public class NPCPointOfInterestData
{
	// Token: 0x06000B30 RID: 2864 RVA: 0x000382BF File Offset: 0x000364BF
	public NPCPointOfInterestData(NPCPointOfInterestConfiguration configuration)
	{
		this.id = configuration.Id;
		this.enabled = configuration.EnabledByDefault;
		this.occupiedBy = SGuid.Empty;
		this.weight = configuration.StartWeight;
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06000B31 RID: 2865 RVA: 0x000382F6 File Offset: 0x000364F6
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06000B32 RID: 2866 RVA: 0x000382FE File Offset: 0x000364FE
	public bool IsOccupied
	{
		get
		{
			return !this.occupiedBy.IsEmpty;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0003830E File Offset: 0x0003650E
	public NPCPointOfInterestConfiguration Configuration
	{
		get
		{
			return LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.GetPointById(this.Id);
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00038320 File Offset: 0x00036520
	public float Weight
	{
		get
		{
			return this.weight;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00038328 File Offset: 0x00036528
	// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00038330 File Offset: 0x00036530
	public bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			this.enabled = value;
			GDPointData gdpointData = this.GDPointData;
			if (gdpointData != null)
			{
				gdpointData.Enabled = this.enabled;
			}
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000B37 RID: 2871 RVA: 0x0003835A File Offset: 0x0003655A
	public GDPointData GDPointData
	{
		get
		{
			GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.Id);
			if (gdpointDataById == null)
			{
				Debug.LogError("NPCPointOfInterestConfiguration:[" + this.Id + "] no linked gd point!!!");
			}
			return gdpointDataById;
		}
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x00038398 File Offset: 0x00036598
	public void Occupy(SGuid sGuid)
	{
		this.occupiedBy = sGuid;
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x000383A1 File Offset: 0x000365A1
	public void Deoccupy()
	{
		this.occupiedBy = SGuid.Empty;
	}

	// Token: 0x04000C75 RID: 3189
	[SerializeField]
	private string id;

	// Token: 0x04000C76 RID: 3190
	[SerializeField]
	private bool enabled;

	// Token: 0x04000C77 RID: 3191
	[SerializeField]
	private SGuid occupiedBy;

	// Token: 0x04000C78 RID: 3192
	[SerializeField]
	private float weight;
}
