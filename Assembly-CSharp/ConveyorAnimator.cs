using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020005FC RID: 1532
[ExecuteAlways]
public abstract class ConveyorAnimator : ConveyorSystemAnimator
{
	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x06002948 RID: 10568
	public abstract ConveyorSystemAnimatorType Type { get; }

	// Token: 0x06002949 RID: 10569 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void TryRegister()
	{
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Unregister()
	{
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000C2974 File Offset: 0x000C0B74
	public override void CustomUpdate()
	{
		if (this.sprite == null)
		{
			return;
		}
		if (this.animationsData.Count == 0 && this.itemsToAnimate.Count == 0)
		{
			return;
		}
		Sprite[] orBuildTargets = this.GetOrBuildTargets(this.sprite);
		for (int i = 0; i < this.animationsData.Count; i++)
		{
			Sprite sprite = orBuildTargets[i];
			if (!(sprite == null))
			{
				foreach (GenericSprite genericSprite in this.animationsData[i].spritesToAnimate)
				{
					if (genericSprite.SpriteRenderer.sprite != sprite)
					{
						genericSprite.SpriteRenderer.sprite = sprite;
					}
				}
			}
		}
		foreach (ConveyorAnimatableItem conveyorAnimatableItem in this.itemsToAnimate)
		{
			conveyorAnimatableItem.transform.localPosition = this.displacement;
			conveyorAnimatableItem.transform.localEulerAngles = this.itemRotation;
			conveyorAnimatableItem.transform.localScale = this.itemScale;
		}
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000C2AB4 File Offset: 0x000C0CB4
	public void AddAnimatable(ConveyorAnimatable animatable)
	{
		ConveyorAnimatableSprite conveyorAnimatableSprite = animatable as ConveyorAnimatableSprite;
		if (conveyorAnimatableSprite != null)
		{
			ConveyorAnimator.SpriteAnimationData spriteAnimationData;
			if (this.animationDataDict.TryGetValue(conveyorAnimatableSprite.spriteNameWithoutIdx, out spriteAnimationData))
			{
				spriteAnimationData.spritesToAnimate.Add(conveyorAnimatableSprite.sprite);
			}
			else
			{
				spriteAnimationData = new ConveyorAnimator.SpriteAnimationData
				{
					spriteNameWithoutIdx = conveyorAnimatableSprite.spriteNameWithoutIdx
				};
				this.animationDataDict.Add(conveyorAnimatableSprite.spriteNameWithoutIdx, spriteAnimationData);
				this.animationsData.Add(spriteAnimationData);
			}
			spriteAnimationData.spritesToAnimate.Add(conveyorAnimatableSprite.sprite);
		}
		else
		{
			ConveyorAnimatableItem conveyorAnimatableItem = animatable as ConveyorAnimatableItem;
			if (conveyorAnimatableItem != null)
			{
				this.itemsToAnimate.Add(conveyorAnimatableItem);
			}
		}
		animatable.CreateCache();
		this.InvalidateDrivingSpriteCache();
		if (this.sprite != null)
		{
			this.GetOrBuildTargets(this.sprite);
		}
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x000C2B74 File Offset: 0x000C0D74
	public void RemoveAnimatable(ConveyorAnimatable animatable)
	{
		ConveyorAnimatableSprite conveyorAnimatableSprite = animatable as ConveyorAnimatableSprite;
		if (conveyorAnimatableSprite != null)
		{
			ConveyorAnimator.SpriteAnimationData spriteAnimationData;
			if (this.animationDataDict.TryGetValue(conveyorAnimatableSprite.spriteNameWithoutIdx, out spriteAnimationData))
			{
				spriteAnimationData.spritesToAnimate.Remove(conveyorAnimatableSprite.sprite);
			}
			if (spriteAnimationData == null || spriteAnimationData.spritesToAnimate.Count == 0)
			{
				this.animationDataDict.Remove(conveyorAnimatableSprite.spriteNameWithoutIdx);
				this.animationsData.Remove(spriteAnimationData);
			}
		}
		else
		{
			ConveyorAnimatableItem conveyorAnimatableItem = animatable as ConveyorAnimatableItem;
			if (conveyorAnimatableItem != null)
			{
				this.itemsToAnimate.Remove(conveyorAnimatableItem);
			}
		}
		animatable.RestoreCache();
		this.InvalidateDrivingSpriteCache();
		if (this.sprite != null)
		{
			this.GetOrBuildTargets(this.sprite);
		}
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x000C2C24 File Offset: 0x000C0E24
	private Sprite[] GetOrBuildTargets(Sprite drivingSprite)
	{
		if (drivingSprite == this.currentDrivingSprite)
		{
			return this.currentTargets;
		}
		this.currentDrivingSprite = drivingSprite;
		if (!this.drivingSpriteToTargets.TryGetValue(drivingSprite, out this.currentTargets))
		{
			this.currentTargets = this.BuildTargetsForDrivingSprite(drivingSprite);
			this.drivingSpriteToTargets[drivingSprite] = this.currentTargets;
		}
		return this.currentTargets;
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x000C2C88 File Offset: 0x000C0E88
	private Sprite[] BuildTargetsForDrivingSprite(Sprite drivingSprite)
	{
		Sprite[] array = new Sprite[this.animationsData.Count];
		string idxFromSpriteName = ConveyorAnimator.GetIdxFromSpriteName(drivingSprite.name);
		for (int i = 0; i < this.animationsData.Count; i++)
		{
			string text = this.animationsData[i].spriteNameWithoutIdx + "_" + idxFromSpriteName;
			array[i] = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, null);
		}
		return array;
	}

	// Token: 0x06002950 RID: 10576 RVA: 0x000C2CF8 File Offset: 0x000C0EF8
	private static string GetIdxFromSpriteName(string sourceName)
	{
		int num = sourceName.LastIndexOf('_');
		if (num < 0)
		{
			return sourceName;
		}
		return sourceName.Substring(num + 1);
	}

	// Token: 0x06002951 RID: 10577 RVA: 0x000C2D1D File Offset: 0x000C0F1D
	private void InvalidateDrivingSpriteCache()
	{
		this.drivingSpriteToTargets.Clear();
		this.currentDrivingSprite = null;
		this.currentTargets = null;
	}

	// Token: 0x04002245 RID: 8773
	[SerializeField]
	private Sprite sprite;

	// Token: 0x04002246 RID: 8774
	public Vector3 displacement = Vector3.zero;

	// Token: 0x04002247 RID: 8775
	public Vector3 itemRotation = Vector3.zero;

	// Token: 0x04002248 RID: 8776
	public Vector3 itemScale = Vector3.one;

	// Token: 0x04002249 RID: 8777
	[SerializeField]
	private List<ConveyorAnimator.SpriteAnimationData> animationsData = new List<ConveyorAnimator.SpriteAnimationData>();

	// Token: 0x0400224A RID: 8778
	[SerializeField]
	protected List<ConveyorAnimatableItem> itemsToAnimate = new List<ConveyorAnimatableItem>();

	// Token: 0x0400224B RID: 8779
	private Dictionary<string, ConveyorAnimator.SpriteAnimationData> animationDataDict = new Dictionary<string, ConveyorAnimator.SpriteAnimationData>();

	// Token: 0x0400224C RID: 8780
	private Dictionary<Sprite, Sprite[]> drivingSpriteToTargets = new Dictionary<Sprite, Sprite[]>();

	// Token: 0x0400224D RID: 8781
	private Sprite currentDrivingSprite;

	// Token: 0x0400224E RID: 8782
	private Sprite[] currentTargets;

	// Token: 0x020005FD RID: 1533
	[Serializable]
	private class SpriteAnimationData
	{
		// Token: 0x0400224F RID: 8783
		public string spriteNameWithoutIdx = string.Empty;

		// Token: 0x04002250 RID: 8784
		public List<GenericSprite> spritesToAnimate = new List<GenericSprite>();
	}
}
