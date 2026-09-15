using System;
using UnityEngine;

// Token: 0x02000196 RID: 406
public abstract class DropViewAtomBase : MonoBehaviour
{
	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x06000A4C RID: 2636 RVA: 0x00034296 File Offset: 0x00032496
	public DropView ParentDropGameObject
	{
		get
		{
			return this.parentDropGameObject;
		}
	}

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0003429E File Offset: 0x0003249E
	public string IconId
	{
		get
		{
			return this.iconId;
		}
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x000342A6 File Offset: 0x000324A6
	public virtual void Activate(string iconId)
	{
		base.gameObject.SetActive(true);
		this.iconId = iconId;
	}

	// Token: 0x06000A4F RID: 2639
	public abstract SpriteText GetSpriteText();

	// Token: 0x06000A50 RID: 2640 RVA: 0x000342BB File Offset: 0x000324BB
	public virtual void Deactivate()
	{
		base.gameObject.SetActive(false);
		this.iconId = null;
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void SetInteractionState(bool isUnderInteraction)
	{
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x000342D0 File Offset: 0x000324D0
	private void Awake()
	{
		this.parentDropGameObject = base.GetComponentInParent<DropView>();
	}

	// Token: 0x04000BB9 RID: 3001
	private DropView parentDropGameObject;

	// Token: 0x04000BBA RID: 3002
	private string iconId;
}
