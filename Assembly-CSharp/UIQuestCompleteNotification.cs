using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200088C RID: 2188
public class UIQuestCompleteNotification : UIBaseNotification
{
	// Token: 0x17000859 RID: 2137
	// (get) Token: 0x06003828 RID: 14376 RVA: 0x0010E6E3 File Offset: 0x0010C8E3
	// (set) Token: 0x06003829 RID: 14377 RVA: 0x0010E6EB File Offset: 0x0010C8EB
	public QuestData QuestData { get; set; }

	// Token: 0x0600382A RID: 14378 RVA: 0x0010E6F4 File Offset: 0x0010C8F4
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OpenQuestTree));
	}

	// Token: 0x0600382B RID: 14379 RVA: 0x0010E712 File Offset: 0x0010C912
	public override void Draw()
	{
		this.portrait.sprite = this.QuestData.Definition.Portrait;
		this.portrait.BlueColorReplace(this.toReplace);
	}

	// Token: 0x0600382C RID: 14380 RVA: 0x0010E740 File Offset: 0x0010C940
	private void OpenQuestTree()
	{
		QuestTreePageWidget.OpenWindowFromScratchOnSelectedQuest(this.QuestData);
	}

	// Token: 0x0600382D RID: 14381 RVA: 0x0010E74D File Offset: 0x0010C94D
	public override void ReleaseToPool()
	{
		LazyPooler.ReleaseObject<UIQuestCompleteNotification>(this);
	}

	// Token: 0x04002CBB RID: 11451
	[SerializeField]
	private LazyButton button;

	// Token: 0x04002CBC RID: 11452
	[SerializeField]
	private Image portrait;

	// Token: 0x04002CBD RID: 11453
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);
}
