using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020009AE RID: 2478
public class UIAnimSimpleButton : MonoBehaviour
{
	// Token: 0x06004216 RID: 16918 RVA: 0x0013A674 File Offset: 0x00138874
	public void Activate(string name, UnityAction callback)
	{
		base.gameObject.SetActive(true);
		base.transform.SetAsFirstSibling();
		this.button.onClick.AddListener(callback);
		this.label.text = name;
	}

	// Token: 0x06004217 RID: 16919 RVA: 0x0013A6AA File Offset: 0x001388AA
	public void Select()
	{
		this.button.image.color = Color.green;
	}

	// Token: 0x06004218 RID: 16920 RVA: 0x0013A6C1 File Offset: 0x001388C1
	public void Deselect()
	{
		this.button.image.color = Color.white;
	}

	// Token: 0x06004219 RID: 16921 RVA: 0x0013A6D8 File Offset: 0x001388D8
	public void Deactivate()
	{
		this.Deselect();
		this.button.onClick.RemoveAllListeners();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400339B RID: 13211
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x0400339C RID: 13212
	[SerializeField]
	private Button button;
}
