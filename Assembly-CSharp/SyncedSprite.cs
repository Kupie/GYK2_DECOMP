using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000528 RID: 1320
[ExecuteAlways]
public class SyncedSprite : CachedSpriteRenderer
{
	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x060021FA RID: 8698 RVA: 0x0009FA86 File Offset: 0x0009DC86
	public bool SyncStateAndPos
	{
		get
		{
			return this.syncStateAndPos;
		}
	}

	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x060021FB RID: 8699 RVA: 0x0009FA8E File Offset: 0x0009DC8E
	public bool SyncSprite
	{
		get
		{
			return this.syncSprite;
		}
	}

	// Token: 0x060021FC RID: 8700 RVA: 0x0009FA98 File Offset: 0x0009DC98
	public static void UpdateAllSyncedSprites()
	{
		foreach (SyncedSprite syncedSprite in SyncedSprite.syncedSpritesToAdd)
		{
			SyncedSprite.activeSyncedSprites.Add(syncedSprite);
		}
		foreach (SyncedSprite syncedSprite2 in SyncedSprite.syncedSpritesToRemove)
		{
			SyncedSprite.activeSyncedSprites.Remove(syncedSprite2);
		}
		if (SyncedSprite.activeSyncedSprites.Count == 0)
		{
			return;
		}
		foreach (SyncedSprite syncedSprite3 in SyncedSprite.activeSyncedSprites)
		{
			if (!(syncedSprite3 == null) && !(syncedSprite3.targetSprite == null) && !(syncedSprite3.SpriteRenderer == null))
			{
				bool flag = syncedSprite3.targetSprite.gameObject.activeInHierarchy && syncedSprite3.targetSprite.SpriteRenderer.enabled && syncedSprite3.targetSprite.SpriteRenderer.sprite != null;
				if (!flag)
				{
					syncedSprite3.SpriteRenderer.enabled = false;
				}
				else
				{
					if (syncedSprite3.syncStateAndPos)
					{
						syncedSprite3.SpriteRenderer.transform.position = syncedSprite3.targetSprite.SpriteRenderer.transform.position;
						syncedSprite3.SpriteRenderer.enabled = flag;
						syncedSprite3.SpriteRenderer.flipX = syncedSprite3.targetSprite.SpriteRenderer.flipX;
					}
					if (syncedSprite3.syncSprite)
					{
						syncedSprite3.SpriteRenderer.sprite = syncedSprite3.targetSprite.SpriteRenderer.sprite;
					}
				}
			}
		}
		SyncedSprite.syncedSpritesToAdd.Clear();
		SyncedSprite.syncedSpritesToRemove.Clear();
	}

	// Token: 0x060021FD RID: 8701 RVA: 0x0009FCC8 File Offset: 0x0009DEC8
	private void OnEnable()
	{
		if (this.targetSprite != null && !this.ignoreMe)
		{
			SyncedSprite.RegisterSyncedSprite(this);
		}
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x0009FCE6 File Offset: 0x0009DEE6
	private void OnBecameVisible()
	{
		if (this.ignoreMe)
		{
			return;
		}
		if (this.targetSprite == null)
		{
			return;
		}
		SyncedSprite.RegisterSyncedSprite(this);
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x0009FD06 File Offset: 0x0009DF06
	private void OnBecameInvisible()
	{
		if (this.ignoreMe)
		{
			return;
		}
		if (this.targetSprite == null)
		{
			return;
		}
		SyncedSprite.UnregisterSyncedSprite(this);
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x0009FD26 File Offset: 0x0009DF26
	private static void RegisterSyncedSprite(SyncedSprite syncedSprite)
	{
		if (syncedSprite == null)
		{
			return;
		}
		SyncedSprite.syncedSpritesToAdd.Add(syncedSprite);
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x0009FD3D File Offset: 0x0009DF3D
	private static void UnregisterSyncedSprite(SyncedSprite syncedSprite)
	{
		if (syncedSprite == null)
		{
			return;
		}
		SyncedSprite.syncedSpritesToRemove.Add(syncedSprite);
	}

	// Token: 0x04001E9C RID: 7836
	private static readonly HashSet<SyncedSprite> activeSyncedSprites = new HashSet<SyncedSprite>(1000);

	// Token: 0x04001E9D RID: 7837
	private static readonly List<SyncedSprite> syncedSpritesToAdd = new List<SyncedSprite>(10);

	// Token: 0x04001E9E RID: 7838
	private static readonly List<SyncedSprite> syncedSpritesToRemove = new List<SyncedSprite>(10);

	// Token: 0x04001E9F RID: 7839
	[SerializeField]
	private bool syncStateAndPos;

	// Token: 0x04001EA0 RID: 7840
	[SerializeField]
	private bool syncSprite;

	// Token: 0x04001EA1 RID: 7841
	public CachedSpriteRenderer targetSprite;

	// Token: 0x04001EA2 RID: 7842
	public bool ignoreMe;
}
