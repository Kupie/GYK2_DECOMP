using System;
using System.Collections.Generic;
using System.Text;
using LazyBearTechnology;
using LinqTools;
using TMPro;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class LazyButtonTipsStr : MonoBehaviour
{
	// Token: 0x0600004B RID: 75 RVA: 0x00003701 File Offset: 0x00001901
	private void Awake()
	{
		if (this.label == null)
		{
			this.label = base.GetComponent<TextMeshProUGUI>();
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x0000371D File Offset: 0x0000191D
	public void Clear()
	{
		this.hasCachedTips = false;
		this.cachedTips = null;
		this.label.text = string.Empty;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x0000373D File Offset: 0x0000193D
	public void Print(params LazyGameKeyTip[] tips)
	{
		this.Print(tips.ToList<LazyGameKeyTip>(), "  ");
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003750 File Offset: 0x00001950
	public void Print(string separator, params LazyGameKeyTip[] tips)
	{
		this.Print(tips.ToList<LazyGameKeyTip>(), separator);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0000375F File Offset: 0x0000195F
	public void Print(List<LazyGameKeyTip> tips, string separator = "  ")
	{
		this.CacheTips(tips, separator);
		this.ApplyCachedTips();
	}

	// Token: 0x06000050 RID: 80 RVA: 0x0000376F File Offset: 0x0000196F
	public void Print(LazyGameKeyTip tip)
	{
		this.CacheTips(new List<LazyGameKeyTip> { tip }, "  ");
		this.ApplyCachedTips();
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00003790 File Offset: 0x00001990
	public static void RefreshAll()
	{
		LazyButtonTipsStr[] array = global::UnityEngine.Object.FindObjectsByType<LazyButtonTipsStr>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].RefreshFromCache();
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000037BB File Offset: 0x000019BB
	private void RefreshFromCache()
	{
		if (!this.hasCachedTips || this.cachedTips == null || this.label == null)
		{
			return;
		}
		this.ApplyCachedTips();
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000037E2 File Offset: 0x000019E2
	private void CacheTips(List<LazyGameKeyTip> tips, string separator)
	{
		this.cachedTips = new List<LazyGameKeyTip>(tips);
		this.cachedSeparator = separator;
		this.hasCachedTips = true;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003800 File Offset: 0x00001A00
	private void ApplyCachedTips()
	{
		if (!this.hasCachedTips || this.cachedTips == null || this.label == null)
		{
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < this.cachedTips.Count; i++)
		{
			string text = this.cachedTips[i].ToString();
			stringBuilder.Append(text);
			if (text.Length > 0 && i < this.cachedTips.Count - 1)
			{
				stringBuilder.Append(this.cachedSeparator);
			}
		}
		this.label.text = stringBuilder.ToString();
	}

	// Token: 0x06000055 RID: 85 RVA: 0x0000389C File Offset: 0x00001A9C
	public void ApplyStyle(TextStyle style)
	{
		style.ApplyStyle(this.label, false, null, null, null);
	}

	// Token: 0x04000045 RID: 69
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04000046 RID: 70
	private List<LazyGameKeyTip> cachedTips;

	// Token: 0x04000047 RID: 71
	private string cachedSeparator = "  ";

	// Token: 0x04000048 RID: 72
	private bool hasCachedTips;
}
