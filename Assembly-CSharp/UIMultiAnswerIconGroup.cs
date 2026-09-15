using System;
using System.Collections.Generic;
using LinqTools;
using Sirenix.Utilities;
using UnityEngine;

// Token: 0x0200080E RID: 2062
public class UIMultiAnswerIconGroup : MonoBehaviour
{
	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x060034EA RID: 13546 RVA: 0x000FEA1D File Offset: 0x000FCC1D
	public List<UIMultiAnswerIcon> Icons
	{
		get
		{
			return this.icons;
		}
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x000FEA25 File Offset: 0x000FCC25
	private void Start()
	{
		this.icons = base.GetComponentsInChildren<UIMultiAnswerIcon>(true).ToList<UIMultiAnswerIcon>();
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x000FEA3C File Offset: 0x000FCC3C
	public void Show(int number)
	{
		if (this.icons.IsNullOrEmpty<UIMultiAnswerIcon>())
		{
			this.icons = base.GetComponentsInChildren<UIMultiAnswerIcon>(true).ToList<UIMultiAnswerIcon>();
		}
		if (number > 2)
		{
			Debug.LogError("Trying to display more than max icon count");
			number = 2;
		}
		if (number == 0)
		{
			return;
		}
		this.HideAll();
		base.gameObject.SetActive(true);
		for (int i = 0; i < number; i++)
		{
			this.icons[i].Show();
		}
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x000FEAAC File Offset: 0x000FCCAC
	public void HideAll()
	{
		foreach (UIMultiAnswerIcon uimultiAnswerIcon in this.icons)
		{
			uimultiAnswerIcon.Hide();
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002A5E RID: 10846
	[SerializeField]
	private List<UIMultiAnswerIcon> icons;
}
