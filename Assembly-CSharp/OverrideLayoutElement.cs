using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000833 RID: 2099
[RequireComponent(typeof(RectTransform))]
[Serializable]
public class OverrideLayoutElement : LayoutElement
{
	// Token: 0x17000803 RID: 2051
	// (get) Token: 0x060035B1 RID: 13745 RVA: 0x001022C0 File Offset: 0x001004C0
	// (set) Token: 0x060035B2 RID: 13746 RVA: 0x001022D2 File Offset: 0x001004D2
	public override int layoutPriority
	{
		get
		{
			if (!this.ignoreOnGettingPreferedSize)
			{
				return base.layoutPriority;
			}
			return -1;
		}
		set
		{
			base.layoutPriority = value;
		}
	}

	// Token: 0x17000804 RID: 2052
	// (get) Token: 0x060035B3 RID: 13747 RVA: 0x001022DC File Offset: 0x001004DC
	// (set) Token: 0x060035B4 RID: 13748 RVA: 0x00102363 File Offset: 0x00100563
	public override float preferredHeight
	{
		get
		{
			if (!this.useMaxHeight)
			{
				if (!this.useHeightSnapping)
				{
					return base.preferredHeight;
				}
				return this.GetSnappedValue(base.preferredHeight, this.heightSnappingType);
			}
			else
			{
				bool flag = this.ignoreOnGettingPreferedSize;
				this.ignoreOnGettingPreferedSize = true;
				float preferredHeight = LayoutUtility.GetPreferredHeight(base.transform as RectTransform);
				this.ignoreOnGettingPreferedSize = flag;
				float num = ((preferredHeight > this.maxHeight) ? this.maxHeight : preferredHeight);
				if (!this.useHeightSnapping)
				{
					return num;
				}
				return this.GetSnappedValue(num, this.heightSnappingType);
			}
		}
		set
		{
			base.preferredHeight = value;
		}
	}

	// Token: 0x17000805 RID: 2053
	// (get) Token: 0x060035B5 RID: 13749 RVA: 0x0010236C File Offset: 0x0010056C
	// (set) Token: 0x060035B6 RID: 13750 RVA: 0x001023BF File Offset: 0x001005BF
	public override float preferredWidth
	{
		get
		{
			if (!this.useMaxWidth)
			{
				return base.preferredWidth;
			}
			bool flag = this.ignoreOnGettingPreferedSize;
			this.ignoreOnGettingPreferedSize = true;
			float preferredWidth = LayoutUtility.GetPreferredWidth(base.transform as RectTransform);
			this.ignoreOnGettingPreferedSize = flag;
			if (preferredWidth <= this.maxWidth)
			{
				return preferredWidth;
			}
			return this.maxWidth;
		}
		set
		{
			base.preferredWidth = value;
		}
	}

	// Token: 0x17000806 RID: 2054
	// (get) Token: 0x060035B7 RID: 13751 RVA: 0x001023C8 File Offset: 0x001005C8
	public override float minHeight
	{
		get
		{
			if (!this.useHeightSnapping)
			{
				return base.minHeight;
			}
			return this.GetSnappedValue(base.minHeight, this.heightSnappingType);
		}
	}

	// Token: 0x060035B8 RID: 13752 RVA: 0x001023EC File Offset: 0x001005EC
	private float GetSnappedValue(float actualSize, OverrideLayoutElement.SizeSnappingType snappingType)
	{
		int num = Mathf.RoundToInt(actualSize);
		float num2 = actualSize;
		if (snappingType != OverrideLayoutElement.SizeSnappingType.Even)
		{
			if (snappingType == OverrideLayoutElement.SizeSnappingType.Odd)
			{
				if (num % 2 == 0)
				{
					num2 = actualSize + 1f;
				}
			}
		}
		else if (num % 2 == 1)
		{
			num2 = (float)(num + 1);
		}
		return num2;
	}

	// Token: 0x04002B02 RID: 11010
	public float maxHeight;

	// Token: 0x04002B03 RID: 11011
	public float maxWidth;

	// Token: 0x04002B04 RID: 11012
	public bool useMaxWidth;

	// Token: 0x04002B05 RID: 11013
	public bool useMaxHeight;

	// Token: 0x04002B06 RID: 11014
	public bool useHeightSnapping;

	// Token: 0x04002B07 RID: 11015
	public OverrideLayoutElement.SizeSnappingType heightSnappingType;

	// Token: 0x04002B08 RID: 11016
	private bool ignoreOnGettingPreferedSize;

	// Token: 0x02000834 RID: 2100
	public enum SizeSnappingType
	{
		// Token: 0x04002B0A RID: 11018
		Even,
		// Token: 0x04002B0B RID: 11019
		Odd
	}
}
