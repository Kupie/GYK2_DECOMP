using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
[Serializable]
public class BakedChunkableObjectComponentData : IChunkableObject
{
	// Token: 0x06002EA9 RID: 11945 RVA: 0x000DF62B File Offset: 0x000DD82B
	public BurstableBounds GetChunkableData()
	{
		return this.chunkBounds.GetBounds();
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06002EAA RID: 11946 RVA: 0x000DF638 File Offset: 0x000DD838
	// (set) Token: 0x06002EAB RID: 11947 RVA: 0x000DF640 File Offset: 0x000DD840
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06002EAC RID: 11948 RVA: 0x00028294 File Offset: 0x00026494
	public bool IgnoreChunkVisibility
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002EAD RID: 11949 RVA: 0x000DF649 File Offset: 0x000DD849
	public void UpdateChunkVisibility(bool isVisible)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.isVisible = isVisible;
		this.RefreshViewVisibility();
		if (!isVisible)
		{
			this.UnsubscribeFromParentGdPoint();
		}
	}

	// Token: 0x06002EAE RID: 11950 RVA: 0x000DF66C File Offset: 0x000DD86C
	private void RefreshViewVisibility()
	{
		if (!this.isVisible || this.IsDisabledByParentGdPoint())
		{
			if (this.view != null)
			{
				BakedChunkableObjectPool.Release(this.pathToObject, this.view);
				this.view = null;
			}
			return;
		}
		if (this.view != null)
		{
			return;
		}
		this.view = BakedChunkableObjectPool.Get(this.pathToObject);
		if (this.view == null)
		{
			return;
		}
		this.view.SetData(this);
		this.view.ApplyData();
	}

	// Token: 0x06002EAF RID: 11951 RVA: 0x000DF6FC File Offset: 0x000DD8FC
	private bool IsDisabledByParentGdPoint()
	{
		if (!string.IsNullOrEmpty(this.parentGdPointId))
		{
			if (this.parentGdPointData == null)
			{
				this.parentGdPointData = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.parentGdPointId);
				if (this.parentGdPointData != null)
				{
					this.parentGdPointData.OnActiveStateChanged += this.OnGdPointStateChanged;
					MainGame.OnGoToMainMenu = (Action)Delegate.Combine(MainGame.OnGoToMainMenu, new Action(this.OnGoToMainMenu));
				}
			}
			if (this.parentGdPointData != null && !this.parentGdPointData.Enabled)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x000DF79D File Offset: 0x000DD99D
	private void OnGdPointStateChanged(bool enabled)
	{
		this.RefreshViewVisibility();
	}

	// Token: 0x06002EB1 RID: 11953 RVA: 0x000DF7A5 File Offset: 0x000DD9A5
	private void OnGoToMainMenu()
	{
		this.UnsubscribeFromParentGdPoint();
	}

	// Token: 0x06002EB2 RID: 11954 RVA: 0x000DF7B0 File Offset: 0x000DD9B0
	private void UnsubscribeFromParentGdPoint()
	{
		if (this.parentGdPointData != null)
		{
			this.parentGdPointData.OnActiveStateChanged -= this.OnGdPointStateChanged;
			MainGame.OnGoToMainMenu = (Action)Delegate.Remove(MainGame.OnGoToMainMenu, new Action(this.OnGoToMainMenu));
			this.parentGdPointData = null;
		}
	}

	// Token: 0x040025B4 RID: 9652
	public string pathToObject;

	// Token: 0x040025B5 RID: 9653
	[HideInInspector]
	public Vector3 worldPos;

	// Token: 0x040025B6 RID: 9654
	[HideInInspector]
	public Vector3 lossyScale;

	// Token: 0x040025B7 RID: 9655
	[HideInInspector]
	public Quaternion rotation;

	// Token: 0x040025B8 RID: 9656
	[HideInInspector]
	public BurstableChunkBoundsPair chunkBounds;

	// Token: 0x040025B9 RID: 9657
	[HideInInspector]
	public Vector3 gndLocalPos;

	// Token: 0x040025BA RID: 9658
	[HideInInspector]
	public string parentGdPointId;

	// Token: 0x040025BB RID: 9659
	private bool isVisible;

	// Token: 0x040025BC RID: 9660
	private BakedChunkableObjectComponent view;

	// Token: 0x040025BD RID: 9661
	private GDPointData parentGdPointData;
}
