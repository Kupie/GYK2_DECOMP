using System;
using TMPro;
using UnityEngine;

// Token: 0x020008BB RID: 2235
public class UICraftSelectionOutputItemCell : MonoBehaviour
{
	// Token: 0x170008B3 RID: 2227
	// (get) Token: 0x06003A18 RID: 14872 RVA: 0x001161E5 File Offset: 0x001143E5
	public UIItemCell UIItemCell
	{
		get
		{
			return this.itemCell;
		}
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x001161ED File Offset: 0x001143ED
	public void Draw(OutputPreview outputPreview, int queueCount = 0)
	{
		this.itemCell.DrawCraftOutput(outputPreview, -1, CraftStatus.OK, ItemType.None, 1, ItemRelatedWidgetState.NotSet, false);
		this.itemCell.ShowMouseSelectionFrame = false;
		this.UpdateQueueCount(queueCount);
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x00116214 File Offset: 0x00114414
	public void UpdateQueueCount(int count)
	{
		this.queueCount.text = count.ToString();
		this.queueCount.gameObject.SetActive(count > 0);
	}

	// Token: 0x04002DCE RID: 11726
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x04002DCF RID: 11727
	[SerializeField]
	private TextMeshProUGUI queueCount;
}
