using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200088D RID: 2189
public class UIQuestStartNotification : UIBaseNotification
{
	// Token: 0x1700085A RID: 2138
	// (get) Token: 0x0600382F RID: 14383 RVA: 0x0010E77C File Offset: 0x0010C97C
	// (set) Token: 0x06003830 RID: 14384 RVA: 0x0010E784 File Offset: 0x0010C984
	public QuestData QuestData { get; set; }

	// Token: 0x06003831 RID: 14385 RVA: 0x0010E78D File Offset: 0x0010C98D
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OpenQuestTree));
	}

	// Token: 0x06003832 RID: 14386 RVA: 0x0010E7AB File Offset: 0x0010C9AB
	public override void Draw()
	{
		this.portrait.sprite = this.QuestData.Definition.Portrait;
		this.portrait.BlueColorReplace(this.toReplace);
	}

	// Token: 0x06003833 RID: 14387 RVA: 0x0010E7D9 File Offset: 0x0010C9D9
	private void OpenQuestTree()
	{
		QuestTreePageWidget.OpenWindowFromScratchOnSelectedQuest(this.QuestData);
	}

	// Token: 0x06003834 RID: 14388 RVA: 0x0010E7E6 File Offset: 0x0010C9E6
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UIQuestStartNotification>(this);
	}

	// Token: 0x04002CBF RID: 11455
	[SerializeField]
	private LazyButton button;

	// Token: 0x04002CC0 RID: 11456
	[SerializeField]
	private Image portrait;

	// Token: 0x04002CC1 RID: 11457
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}
