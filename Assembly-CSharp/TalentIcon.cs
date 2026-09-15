using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000922 RID: 2338
public class TalentIcon : MonoBehaviour
{
	// Token: 0x06003DAA RID: 15786 RVA: 0x001267C2 File Offset: 0x001249C2
	public void Draw(string talentId, bool isActive = true)
	{
		this.label.text = (isActive ? talentId.FontIcon() : (talentId + "-inactive").FontIcon());
	}

	// Token: 0x0400306E RID: 12398
	[SerializeField]
	private TextMeshProUGUI label;
}
