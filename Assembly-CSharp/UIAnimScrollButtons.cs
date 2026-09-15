using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009AB RID: 2475
public class UIAnimScrollButtons : MonoBehaviour
{
	// Token: 0x0600420D RID: 16909 RVA: 0x0013A514 File Offset: 0x00138714
	public void Activate(List<string> idsList, Action<string> clickedCallback)
	{
		if (base.gameObject.activeSelf)
		{
			return;
		}
		base.gameObject.SetActive(true);
		for (int i = 0; i < idsList.Count; i++)
		{
			UIAnimSimpleButton button = this.GetButton();
			string id = idsList[i];
			button.Activate(id, delegate
			{
				Action<string> clickedCallback2 = clickedCallback;
				if (clickedCallback2 != null)
				{
					clickedCallback2(id);
				}
				this.Deactivate();
			});
			this.activeBtnsList.Add(button);
		}
	}

	// Token: 0x0600420E RID: 16910 RVA: 0x0013A5A5 File Offset: 0x001387A5
	public void Deactivate()
	{
		this.ResetToPool();
		base.gameObject.SetActive(false);
	}

	// Token: 0x0600420F RID: 16911 RVA: 0x0013A5B9 File Offset: 0x001387B9
	private UIAnimSimpleButton GetButton()
	{
		if (this.btnsPool.Count == 0)
		{
			this.btnsPool.Add(this.buttonPrefab.Copy(null, true, ""));
		}
		return this.btnsPool.PopFirst<UIAnimSimpleButton>();
	}

	// Token: 0x06004210 RID: 16912 RVA: 0x0013A5F0 File Offset: 0x001387F0
	private void ResetToPool()
	{
		this.activeBtnsList.ForEach(delegate(UIAnimSimpleButton btn)
		{
			btn.Deactivate();
			this.btnsPool.Add(btn);
		});
		this.activeBtnsList.Clear();
	}

	// Token: 0x04003393 RID: 13203
	[SerializeField]
	private UIAnimSimpleButton buttonPrefab;

	// Token: 0x04003394 RID: 13204
	private List<UIAnimSimpleButton> activeBtnsList = new List<UIAnimSimpleButton>();

	// Token: 0x04003395 RID: 13205
	private List<UIAnimSimpleButton> btnsPool = new List<UIAnimSimpleButton>();

	// Token: 0x04003396 RID: 13206
	private bool isActivated;
}
