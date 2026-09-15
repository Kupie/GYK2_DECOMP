using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000410 RID: 1040
public class FishUnderwaterGfx : MonoBehaviour
{
	// Token: 0x170004CD RID: 1229
	// (get) Token: 0x06001B31 RID: 6961 RVA: 0x0007E6A2 File Offset: 0x0007C8A2
	// (set) Token: 0x06001B32 RID: 6962 RVA: 0x0007E6AA File Offset: 0x0007C8AA
	public FishUnderwaterGfx.FishGfxState FishState { get; private set; }

	// Token: 0x06001B33 RID: 6963 RVA: 0x0007E6B4 File Offset: 0x0007C8B4
	public void UpdateGfx(FishGfxUnderwaterType type = FishGfxUnderwaterType.None)
	{
		this.fishGfxType = type;
		this.fishGfxObjects.ForEach(delegate(GameObject obj)
		{
			obj.SetActive(false);
		});
		int num = -1;
		switch (type)
		{
		case FishGfxUnderwaterType.Small:
			num = 0;
			break;
		case FishGfxUnderwaterType.Big:
			num = 1;
			break;
		case FishGfxUnderwaterType.Snake:
			num = 2;
			break;
		case FishGfxUnderwaterType.Squid:
			num = 3;
			break;
		case FishGfxUnderwaterType.Frog:
			num = 4;
			break;
		}
		if (num != -1)
		{
			GameObject gameObject = this.fishGfxObjects[num];
			gameObject.SetActive(true);
			Animator animator;
			if (gameObject.TryGetComponent<Animator>(out animator))
			{
				this.curFishAnimator = animator;
				return;
			}
		}
		else
		{
			this.curFishAnimator = null;
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x0007E753 File Offset: 0x0007C953
	public void SetFishState(FishUnderwaterGfx.FishGfxState state)
	{
		if (this.curFishAnimator)
		{
			this.curFishAnimator.SetInteger(FishUnderwaterGfx.fishStateAnimator, (int)state);
		}
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x0007E773 File Offset: 0x0007C973
	private void Update()
	{
		this.bobGameObject.transform.position = base.transform.position;
	}

	// Token: 0x04001A51 RID: 6737
	[SerializeField]
	private GameObject bobGameObject;

	// Token: 0x04001A52 RID: 6738
	[SerializeField]
	private List<GameObject> fishGfxObjects = new List<GameObject>();

	// Token: 0x04001A53 RID: 6739
	[SerializeField]
	private FishGfxUnderwaterType fishGfxType;

	// Token: 0x04001A54 RID: 6740
	private Animator curFishAnimator;

	// Token: 0x04001A56 RID: 6742
	private static readonly int fishStateAnimator = Animator.StringToHash("fish_state");

	// Token: 0x02000411 RID: 1041
	public enum FishGfxState
	{
		// Token: 0x04001A58 RID: 6744
		None,
		// Token: 0x04001A59 RID: 6745
		Losing,
		// Token: 0x04001A5A RID: 6746
		Pulling
	}
}
