using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000613 RID: 1555
[CreateAssetMenu(fileName = "PlayerColorCustomizationElementData", menuName = "GK2/PlayerColorCustomizationElementData")]
public class PlayerColorCustomizationElementData : ScriptableObject
{
	// Token: 0x170006AC RID: 1708
	// (get) Token: 0x06002998 RID: 10648 RVA: 0x000C462D File Offset: 0x000C282D
	public IReadOnlyList<PlayerColorCustomizationSkinElementData> SkinElements
	{
		get
		{
			return this.skinElements;
		}
	}

	// Token: 0x06002999 RID: 10649 RVA: 0x000C4635 File Offset: 0x000C2835
	public int GetPalettesCountForSkin(int skin)
	{
		return this.GetSkinElement(skin).palettes.Count;
	}

	// Token: 0x0600299A RID: 10650 RVA: 0x000C4648 File Offset: 0x000C2848
	public int GetMaxPaletteCount()
	{
		int num = 0;
		for (int i = 0; i < this.skinElements.Count; i++)
		{
			if (this.skinElements[i].palettes.Count > num)
			{
				num = this.skinElements[i].palettes.Count;
			}
		}
		return num;
	}

	// Token: 0x0600299B RID: 10651 RVA: 0x000C46A0 File Offset: 0x000C28A0
	public Texture2D GetPaletteByIndex(int index, int skin)
	{
		Texture2D texture2D = null;
		if (this.skinElements.Count == 1)
		{
			texture2D = this.skinElements[0].GetPaletteByIndex(index);
		}
		for (int i = 0; i < this.skinElements.Count; i++)
		{
			if (this.skinElements[i].skinId == skin)
			{
				texture2D = this.skinElements[i].GetPaletteByIndex(index);
				break;
			}
		}
		if (texture2D == null)
		{
			texture2D = this.skinElements[0].GetPaletteByIndex(index);
		}
		if (texture2D == null)
		{
			texture2D = this.emptyTexture;
		}
		return texture2D;
	}

	// Token: 0x0600299C RID: 10652 RVA: 0x000C473C File Offset: 0x000C293C
	public PlayerColorCustomizationSkinElementData GetSkinElement(int skin)
	{
		if (this.skinElements.Count == 1)
		{
			return this.skinElements[0];
		}
		for (int i = 0; i < this.skinElements.Count; i++)
		{
			if (this.skinElements[i].skinId == skin)
			{
				return this.skinElements[i];
			}
		}
		Debug.LogWarning(string.Format("No skin element for id:[{0}]", skin));
		return null;
	}

	// Token: 0x0400229E RID: 8862
	public PlayerColorCustomizationType playerColorCustomizationType;

	// Token: 0x0400229F RID: 8863
	[SerializeField]
	private List<PlayerColorCustomizationSkinElementData> skinElements;

	// Token: 0x040022A0 RID: 8864
	[SerializeField]
	private Texture2D emptyTexture;
}
