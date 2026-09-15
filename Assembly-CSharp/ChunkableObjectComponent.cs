using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
[DisallowMultipleComponent]
public class ChunkableObjectComponent : MonoBehaviour, IChunkableObject
{
	// Token: 0x1700074B RID: 1867
	// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000E055D File Offset: 0x000DE75D
	// (set) Token: 0x06002EF0 RID: 12016 RVA: 0x000E0565 File Offset: 0x000DE765
	public bool CustomVisibilityDisabled
	{
		get
		{
			return this.customVisibilityDisabled;
		}
		set
		{
			if (this.customVisibilityDisabled == value)
			{
				return;
			}
			this.customVisibilityDisabled = value;
			this.ApplyVisibility();
		}
	}

	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x000E057E File Offset: 0x000DE77E
	protected bool ShouldBeActive
	{
		get
		{
			return this.isVisible && !this.customVisibilityDisabled;
		}
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000E0593 File Offset: 0x000DE793
	public virtual void CalculateChunkBounds()
	{
		this.initialBoundsPosition = base.transform.position;
		this.bounds = ChunkSizeCalculator.CalculateChunkBounds(base.gameObject);
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x000337EF File Offset: 0x000319EF
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		this.DrawChunkGizmos();
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x000E05B8 File Offset: 0x000DE7B8
	public BurstableBounds GetChunkableData()
	{
		if (!this.chunkBoundsCalculated)
		{
			this.chunkBounds = new BurstableBounds(this.bounds.GetBounds().center + base.transform.position - this.initialBoundsPosition, this.bounds.GetBounds().size);
			this.chunkBoundsCalculated = true;
			this.isVisible = base.gameObject.activeSelf;
		}
		return this.chunkBounds;
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x06002EF5 RID: 12021 RVA: 0x000E0641 File Offset: 0x000DE841
	// (set) Token: 0x06002EF6 RID: 12022 RVA: 0x000E0649 File Offset: 0x000DE849
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x06002EF7 RID: 12023 RVA: 0x000E0652 File Offset: 0x000DE852
	public virtual void UpdateChunkVisibility(bool isVisible)
	{
		this.isVisible = isVisible;
		this.ApplyVisibility();
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x000E0664 File Offset: 0x000DE864
	protected virtual void ApplyVisibility()
	{
		bool shouldBeActive = this.ShouldBeActive;
		if (base.gameObject.activeSelf == shouldBeActive)
		{
			return;
		}
		base.gameObject.SetActive(shouldBeActive);
	}

	// Token: 0x040025E0 RID: 9696
	[SerializeField]
	protected ChunkBoundsPair bounds;

	// Token: 0x040025E1 RID: 9697
	[SerializeField]
	[HideInInspector]
	private Vector3 initialBoundsPosition;

	// Token: 0x040025E2 RID: 9698
	protected bool isVisible = true;

	// Token: 0x040025E3 RID: 9699
	private bool chunkBoundsCalculated;

	// Token: 0x040025E4 RID: 9700
	protected BurstableBounds chunkBounds;

	// Token: 0x040025E5 RID: 9701
	private bool customVisibilityDisabled;
}
