using System;
using TMPro;
using UnityEngine;

// Token: 0x02000A5A RID: 2650
public class UICreditsWindowElement : MonoBehaviour
{
	// Token: 0x06004782 RID: 18306 RVA: 0x0015384C File Offset: 0x00151A4C
	public void DrawCenter(string text)
	{
		TextMeshProUGUI textMeshProUGUI = ((this.centerLabel != null) ? this.centerLabel : this.leftLabel);
		UICreditsWindowElement.SetLabel(this.leftLabel, string.Empty, false);
		this.SetRightLabel(string.Empty, false);
		UICreditsWindowElement.SetLabel(this.centerLabel, string.Empty, false);
		UICreditsWindowElement.SetLabel(textMeshProUGUI, text, true);
	}

	// Token: 0x06004783 RID: 18307 RVA: 0x001538AA File Offset: 0x00151AAA
	public void DrawRow(string leftText, string rightText)
	{
		UICreditsWindowElement.SetLabel(this.leftLabel, leftText, true);
		this.SetRightLabel(rightText, true);
		UICreditsWindowElement.SetLabel(this.centerLabel, string.Empty, false);
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x001538D4 File Offset: 0x00151AD4
	private void SetRightLabel(string text, bool isActive)
	{
		if (this.rightLabel == null)
		{
			return;
		}
		((this.rightLabel.transform.parent != null) ? this.rightLabel.transform.parent.gameObject : this.rightLabel.gameObject).SetActive(isActive);
		this.rightLabel.text = text;
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x0015393C File Offset: 0x00151B3C
	private static void SetLabel(TextMeshProUGUI label, string text, bool isActive)
	{
		if (label == null)
		{
			return;
		}
		label.gameObject.SetActive(isActive);
		label.text = text;
	}

	// Token: 0x040037D1 RID: 14289
	public TextMeshProUGUI leftLabel;

	// Token: 0x040037D2 RID: 14290
	public TextMeshProUGUI rightLabel;

	// Token: 0x040037D3 RID: 14291
	public TextMeshProUGUI centerLabel;
}
