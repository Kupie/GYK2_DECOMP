using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class ModuleSlotArea : MonoBehaviour
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000824 RID: 2084 RVA: 0x000280AD File Offset: 0x000262AD
	// (set) Token: 0x06000825 RID: 2085 RVA: 0x000280B5 File Offset: 0x000262B5
	private bool IsAvailable { get; set; }

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000826 RID: 2086 RVA: 0x000280BE File Offset: 0x000262BE
	public BuildArea BuildArea
	{
		get
		{
			return this.buildArea;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x000280C6 File Offset: 0x000262C6
	public string FontIconId
	{
		get
		{
			return this.fontIconId;
		}
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x000280D0 File Offset: 0x000262D0
	public void ApplyVisibility(bool isAvailable)
	{
		if (this.meshBoundsUvToShader == null)
		{
			return;
		}
		if (this.iconTexture == null || this.gridTexture == null || this.unavailableIconTexture == null || this.unavailableGridTexture == null)
		{
			return;
		}
		this.IsAvailable = isAvailable;
		Texture2D texture2D = (isAvailable ? this.iconTexture : this.unavailableIconTexture);
		Texture2D texture2D2 = (isAvailable ? this.gridTexture : this.unavailableGridTexture);
		this.meshBoundsUvToShader.SetTextures(texture2D, texture2D2);
		this.meshBoundsUvToShader.SetCellsCount(this.cellsCount);
		this.meshBoundsUvToShader.Apply();
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x00028179 File Offset: 0x00026379
	private void Awake()
	{
		this.ApplyVisibility(this.IsAvailable);
	}

	// Token: 0x04000A13 RID: 2579
	[SerializeField]
	[CanBeNull]
	private MeshBoundsUvToShader meshBoundsUvToShader;

	// Token: 0x04000A14 RID: 2580
	[SerializeField]
	private Texture2D iconTexture;

	// Token: 0x04000A15 RID: 2581
	[SerializeField]
	private Texture2D unavailableIconTexture;

	// Token: 0x04000A16 RID: 2582
	[SerializeField]
	private Texture2D gridTexture;

	// Token: 0x04000A17 RID: 2583
	[SerializeField]
	private Texture2D unavailableGridTexture;

	// Token: 0x04000A18 RID: 2584
	[SerializeField]
	[CanBeNull]
	private BuildArea buildArea;

	// Token: 0x04000A19 RID: 2585
	[SerializeField]
	private string fontIconId;

	// Token: 0x04000A1A RID: 2586
	[SerializeField]
	private Vector2Int cellsCount = Vector2Int.one;
}
