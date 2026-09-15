using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000823 RID: 2083
[RequireComponent(typeof(Canvas))]
public class UISleepFade : UIBasicFade
{
	// Token: 0x06003548 RID: 13640 RVA: 0x0010070C File Offset: 0x000FE90C
	public override void Init()
	{
		base.Init();
		this.canvas = base.GetComponent<Canvas>();
		this.canvas.overrideSorting = true;
		this.hrs.material = new Material(this.hrs.material);
		this.brd.material = new Material(this.brd.material);
		this.canvas.sortingOrder = 49;
	}

	// Token: 0x06003549 RID: 13641 RVA: 0x0010077C File Offset: 0x000FE97C
	public void FadeIn(Action onComplete, FadeFlag fadeFlag = FadeFlag.Common, bool blockInterceptions = false, SleepAnimType sleepAnimType = SleepAnimType.None, bool instant = false)
	{
		this.sleepAnimType = sleepAnimType;
		this.ApplyPlayerSkin();
		if (instant)
		{
			base.FadeInInstant(fadeFlag);
			if (onComplete != null)
			{
				onComplete();
			}
		}
		else
		{
			base.FadeIn(onComplete, fadeFlag, blockInterceptions);
		}
		if (sleepAnimType == SleepAnimType.None)
		{
			this.bed.SetActive(false);
			return;
		}
		this.bed.SetActive(true);
		this.animator.SetInteger(UISleepFade.SleepAnim, (int)sleepAnimType);
	}

	// Token: 0x0600354A RID: 13642 RVA: 0x001007E5 File Offset: 0x000FE9E5
	public void FadeOut(Action onComplete, bool instant = false)
	{
		if (this.sleepAnimType != SleepAnimType.None)
		{
			this.bed.SetActive(false);
		}
		if (instant)
		{
			base.FadeOutInstant(FadeFlag.Common);
			if (onComplete != null)
			{
				onComplete();
				return;
			}
		}
		else
		{
			base.FadeOut(onComplete, FadeFlag.Common);
		}
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x00100818 File Offset: 0x000FEA18
	private void ApplyPlayerSkin()
	{
		SkinPresetGK2 skinPreset = MainGame.PlayerController.View.PlayerAnimation.SkinPreset;
		UISleepFade.ApplySkinPart(this.brd, string.Format("{0}{1}", skinPreset.beard.id, "_brd_static_down"), skinPreset, skinPreset.beard);
		UISleepFade.ApplySkinPart(this.hrs, string.Format("{0}{1}", skinPreset.hairstyle.id, "_hrs_static_down"), skinPreset, skinPreset.hairstyle);
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x0010089C File Offset: 0x000FEA9C
	private static void ApplySkinPart(Image image, string spriteName, SkinPresetGK2 skinPreset, SkinPresetPartGK2 part)
	{
		bool flag = LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(spriteName);
		image.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		image.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(spriteName, null);
		skinPreset.TryToApply(image, image.name, part);
	}

	// Token: 0x04002AB7 RID: 10935
	private const string BEARD_SPRITE_PART_WITHOUT_ID = "_brd_static_down";

	// Token: 0x04002AB8 RID: 10936
	private const string HAIRSTYLE_SPRITE_PART_WITHOUT_ID = "_hrs_static_down";

	// Token: 0x04002AB9 RID: 10937
	private static readonly int SleepAnim = Animator.StringToHash("SleepAnimType");

	// Token: 0x04002ABA RID: 10938
	[SerializeField]
	private Animator animator;

	// Token: 0x04002ABB RID: 10939
	[SerializeField]
	private GameObject bed;

	// Token: 0x04002ABC RID: 10940
	[SerializeField]
	private Image hrs;

	// Token: 0x04002ABD RID: 10941
	[SerializeField]
	private Image brd;

	// Token: 0x04002ABE RID: 10942
	private Canvas canvas;

	// Token: 0x04002ABF RID: 10943
	private SleepAnimType sleepAnimType;
}
