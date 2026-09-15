using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020007FE RID: 2046
public struct PhraseData
{
	// Token: 0x0600347C RID: 13436 RVA: 0x000FC4E8 File Offset: 0x000FA6E8
	public PhraseData(bool isPlayer, WgoData npcWgoData, string text, Action onFinished, SpeechBubblePreset preset, SpeechBubbleType speechType = SpeechBubbleType.Talk, UIBasicBubble.ForceCornerPosition cornerPosition = UIBasicBubble.ForceCornerPosition.Auto, float fixedShowTimeValue = 0f, bool isOverBlackout = false)
	{
		this.isPlayer = isPlayer;
		this.npcWgoData = npcWgoData;
		this.text = text;
		this.onFinished = onFinished;
		this.preset = preset;
		this.speechType = speechType;
		this.cornerPosition = cornerPosition;
		this.fixedShowTimeValue = fixedShowTimeValue;
		this.isOverBlackout = isOverBlackout;
	}

	// Token: 0x0600347D RID: 13437 RVA: 0x000FC53C File Offset: 0x000FA73C
	public Vector3 GetTargetPos()
	{
		if (this.isPlayer || this.npcWgoData.Definition.usePortraitInDialogues)
		{
			return MainGame.PlayerController.BubblePoint.position;
		}
		if (this.npcWgoData.id == "player_wisp")
		{
			return MainGame.PlayerController.WispController.BubblePoint.position;
		}
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.npcWgoData.UniqueId);
		if (wgoViewGlobal != null)
		{
			return wgoViewGlobal.BubbleDrawablePosition;
		}
		return this.npcWgoData.BubblePos;
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x000FC5CC File Offset: 0x000FA7CC
	public Direction GetDirection()
	{
		if (this.isPlayer || this.npcWgoData.Definition.usePortraitInDialogues || this.npcWgoData.id == "player_wisp")
		{
			return MainGame.PlayerController.Direction;
		}
		return this.npcWgoData.Direction;
	}

	// Token: 0x040029F0 RID: 10736
	public bool isPlayer;

	// Token: 0x040029F1 RID: 10737
	public WgoData npcWgoData;

	// Token: 0x040029F2 RID: 10738
	public string text;

	// Token: 0x040029F3 RID: 10739
	public Action onFinished;

	// Token: 0x040029F4 RID: 10740
	public UIBasicBubble.ForceCornerPosition cornerPosition;

	// Token: 0x040029F5 RID: 10741
	public SpeechBubblePreset preset;

	// Token: 0x040029F6 RID: 10742
	public SpeechBubbleType speechType;

	// Token: 0x040029F7 RID: 10743
	public float fixedShowTimeValue;

	// Token: 0x040029F8 RID: 10744
	public bool isOverBlackout;
}
