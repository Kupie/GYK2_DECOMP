using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class BannerView : MonoBehaviour
{
	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06000765 RID: 1893 RVA: 0x00023119 File Offset: 0x00021319
	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00023126 File Offset: 0x00021326
	public string GetCurVariationId()
	{
		return this.variations.Find((BannerView.BannerVariation x) => x.gameObject.activeSelf).id;
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00023158 File Offset: 0x00021358
	public void Show(bool isEnabled, string id = "")
	{
		if (!isEnabled)
		{
			base.gameObject.SetActive(false);
			this.currentVariation = null;
			return;
		}
		foreach (BannerView.BannerVariation bannerVariation in this.variations)
		{
			if (bannerVariation.id == id)
			{
				bannerVariation.gameObject.SetActive(true);
				this.currentVariation = bannerVariation;
			}
			else
			{
				bannerVariation.gameObject.SetActive(false);
			}
		}
		base.gameObject.SetActive(true);
	}

	// Token: 0x06000768 RID: 1896 RVA: 0x000231F8 File Offset: 0x000213F8
	public void SetEnabledClothFading(bool isEnabled)
	{
		if (this.currentVariation != null)
		{
			this.currentVariation.Cloth.SetEnabledFading(isEnabled);
		}
	}

	// Token: 0x04000955 RID: 2389
	[SerializeField]
	private List<BannerView.BannerVariation> variations = new List<BannerView.BannerVariation>();

	// Token: 0x04000956 RID: 2390
	private BannerView.BannerVariation currentVariation;

	// Token: 0x0200013A RID: 314
	[Serializable]
	private class BannerVariation
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00023228 File Offset: 0x00021428
		public Cloth Cloth
		{
			get
			{
				Cloth cloth;
				if ((cloth = this.cloth) == null)
				{
					cloth = (this.cloth = this.gameObject.GetComponentInChildren<Cloth>());
				}
				return cloth;
			}
		}

		// Token: 0x04000957 RID: 2391
		[SerializeField]
		public string id = "";

		// Token: 0x04000958 RID: 2392
		[SerializeField]
		public GameObject gameObject;

		// Token: 0x04000959 RID: 2393
		private Cloth cloth;
	}
}
