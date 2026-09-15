using System;
using TMPro;
using UnityEngine;

// Token: 0x02000704 RID: 1796
public class CinematicsCommonObject : MonoBehaviour
{
	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000E344C File Offset: 0x000E164C
	// (set) Token: 0x06002F5D RID: 12125 RVA: 0x000E3454 File Offset: 0x000E1654
	public bool UseOverImageText { get; private set; }

	// Token: 0x06002F5E RID: 12126 RVA: 0x000E345D File Offset: 0x000E165D
	public Transform GetCenter(bool isSwitch1)
	{
		if (!isSwitch1)
		{
			return this.center;
		}
		return this.centerForFullScreen;
	}

	// Token: 0x06002F5F RID: 12127 RVA: 0x000E346F File Offset: 0x000E166F
	public bool IsTargetLabel(TextMeshPro bound)
	{
		if (bound == null)
		{
			return false;
		}
		if (bound == this.labelOverImage)
		{
			return this.UseOverImageText;
		}
		return !(bound == this.label) || !this.UseOverImageText;
	}

	// Token: 0x06002F60 RID: 12128 RVA: 0x000E34AC File Offset: 0x000E16AC
	public void SetSwitchVariant(bool isSwitch1)
	{
		this.UseOverImageText = isSwitch1;
		CinematicsCommonObject.HideWorldLabel(this.label);
		CinematicsCommonObject.HideWorldLabel(this.labelOverImage);
		TextMeshPro textMeshPro = (isSwitch1 ? this.labelOverImage : this.label);
		if (textMeshPro != null)
		{
			textMeshPro.gameObject.SetActive(true);
		}
		if (this.center != null)
		{
			this.center.gameObject.SetActive(!isSwitch1);
		}
		if (this.centerForFullScreen != null)
		{
			this.centerForFullScreen.gameObject.SetActive(isSwitch1);
		}
	}

	// Token: 0x06002F61 RID: 12129 RVA: 0x000E3540 File Offset: 0x000E1740
	public static void HideWorldLabel(TextMeshPro tmp)
	{
		if (tmp == null)
		{
			return;
		}
		tmp.enabled = false;
		Renderer[] componentsInChildren = tmp.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
	}

	// Token: 0x04002644 RID: 9796
	public TextMeshPro label;

	// Token: 0x04002645 RID: 9797
	public TextMeshPro labelOverImage;

	// Token: 0x04002646 RID: 9798
	public Transform center;

	// Token: 0x04002647 RID: 9799
	public Transform centerForFullScreen;
}
