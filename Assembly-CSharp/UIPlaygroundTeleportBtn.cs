using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009B1 RID: 2481
public class UIPlaygroundTeleportBtn : MonoBehaviour
{
	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x06004228 RID: 16936 RVA: 0x0013A9DF File Offset: 0x00138BDF
	// (set) Token: 0x06004229 RID: 16937 RVA: 0x0013A9E7 File Offset: 0x00138BE7
	public string TaskId { get; private set; }

	// Token: 0x0600422A RID: 16938 RVA: 0x0013A9F0 File Offset: 0x00138BF0
	public void Init(string name, UnityAction callback)
	{
		base.gameObject.SetActive(true);
		this.button.onClick.AddListener(callback);
		Match match = new Regex("GK2?-\\d+").Match(name);
		if (match.Success)
		{
			this.TaskId = match.Value;
			string text = "<color=#808080>" + name.Replace(this.TaskId, "") + "</color>";
			this.label.text = this.TaskId + text;
			return;
		}
		this.label.text = name;
	}

	// Token: 0x040033A5 RID: 13221
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x040033A6 RID: 13222
	[SerializeField]
	private Button button;
}
