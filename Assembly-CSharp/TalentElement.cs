using System;
using TMPro;
using UnityEngine;

// Token: 0x020008F2 RID: 2290
public class TalentElement : MonoBehaviour
{
	// Token: 0x06003BE7 RID: 15335 RVA: 0x0011E018 File Offset: 0x0011C218
	public void Draw(string fontIcon, int value = -1)
	{
		string text = ((value > -1) ? value.ToString() : "");
		this.talentValue.text = fontIcon + text;
	}

	// Token: 0x04002F2B RID: 12075
	[SerializeField]
	private TextMeshProUGUI talentValue;
}
