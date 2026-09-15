using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000A20 RID: 2592
public class QuestInfoElement : MonoBehaviour
{
	// Token: 0x17000AA3 RID: 2723
	// (get) Token: 0x060045A5 RID: 17829 RVA: 0x001493F1 File Offset: 0x001475F1
	public QuestData QuestData
	{
		get
		{
			return this.questData;
		}
	}

	// Token: 0x060045A6 RID: 17830 RVA: 0x001493FC File Offset: 0x001475FC
	public void Draw(QuestData questData, List<LinkedEntityWidget> linkedEntityWidgets)
	{
		this.label.text = questData.Description;
		for (int i = 0; i < linkedEntityWidgets.Count; i++)
		{
			linkedEntityWidgets[i].transform.SetParent(this.linkedParent);
		}
		this.anyLinked = linkedEntityWidgets.Count > 0;
		if (this.anyLinked)
		{
			this.label.rectTransform.sizeDelta = this.sizeDeltaForLabelIfAnyLinked;
			this.linkedParent.parent.gameObject.SetActive(true);
		}
		else
		{
			this.label.rectTransform.sizeDelta = this.sizeDeltaForLabelIfNoLinked;
			this.linkedParent.parent.gameObject.SetActive(false);
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x0400367A RID: 13946
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x0400367B RID: 13947
	[SerializeField]
	private Vector2 sizeDeltaForLabelIfAnyLinked;

	// Token: 0x0400367C RID: 13948
	[SerializeField]
	private Vector2 sizeDeltaForLabelIfNoLinked;

	// Token: 0x0400367D RID: 13949
	[SerializeField]
	private RectTransform linkedParent;

	// Token: 0x0400367E RID: 13950
	[HideInInspector]
	public bool anyLinked;

	// Token: 0x0400367F RID: 13951
	private QuestData questData;
}
