using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class ItemFromInventoryDrawer : MonoBehaviour
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x060016FB RID: 5883 RVA: 0x0006D693 File Offset: 0x0006B893
	private Inventory Inventory
	{
		get
		{
			return this.wgoPart.Wgo.Data.Inventory;
		}
	}

	// Token: 0x060016FC RID: 5884 RVA: 0x0006D6AC File Offset: 0x0006B8AC
	private void OnEnable()
	{
		WgoPart wgoPart = this.wgoPart;
		bool flag;
		if (wgoPart == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = wgoPart.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag)
		{
			return;
		}
		if (!this.isSubscribed)
		{
			this.Inventory.OnItemsAdd += this.OnItemsChanged;
			this.Inventory.OnItemsRemove += this.OnItemsChanged;
			this.isSubscribed = true;
		}
		this.Redraw();
	}

	// Token: 0x060016FD RID: 5885 RVA: 0x0006D720 File Offset: 0x0006B920
	private void OnDisable()
	{
		WgoPart wgoPart = this.wgoPart;
		bool flag;
		if (wgoPart == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = wgoPart.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag)
		{
			return;
		}
		if (this.isSubscribed)
		{
			this.Inventory.OnItemsAdd -= this.OnItemsChanged;
			this.Inventory.OnItemsRemove -= this.OnItemsChanged;
			this.isSubscribed = false;
		}
	}

	// Token: 0x060016FE RID: 5886 RVA: 0x0006D78C File Offset: 0x0006B98C
	private void OnDestroy()
	{
		WgoPart wgoPart = this.wgoPart;
		bool flag;
		if (wgoPart == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = wgoPart.Wgo;
			flag = ((wgo != null) ? wgo.Data : null) != null;
		}
		if (!flag)
		{
			return;
		}
		if (this.isSubscribed)
		{
			this.Inventory.OnItemsAdd -= this.OnItemsChanged;
			this.Inventory.OnItemsRemove -= this.OnItemsChanged;
			this.isSubscribed = false;
		}
	}

	// Token: 0x060016FF RID: 5887 RVA: 0x0006D7F7 File Offset: 0x0006B9F7
	private void OnItemsChanged(List<Item> items)
	{
		this.Redraw();
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x0006D800 File Offset: 0x0006BA00
	private void Redraw()
	{
		if (this.Inventory.Data.InventoryFillSize <= 0 || this.Inventory.Data.Inventory[0].IsEmpty)
		{
			this.itemObject.SetActive(false);
			return;
		}
		this.itemObject.SetActive(true);
		this.spriteRenderer.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.Inventory.Data.Inventory[0].Definition.iconId, null);
	}

	// Token: 0x040016FA RID: 5882
	[SerializeField]
	private WgoPart wgoPart;

	// Token: 0x040016FB RID: 5883
	[SerializeField]
	private GameObject itemObject;

	// Token: 0x040016FC RID: 5884
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x040016FD RID: 5885
	private bool isSubscribed;
}
