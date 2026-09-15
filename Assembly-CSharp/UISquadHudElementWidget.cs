using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009CF RID: 2511
public class UISquadHudElementWidget : LazyWidget<UISquadHudElementWidgetData>
{
	// Token: 0x0600430D RID: 17165 RVA: 0x0013EA9E File Offset: 0x0013CC9E
	public override void Hide()
	{
		this.UnsubscribeFromHp();
		base.Hide();
	}

	// Token: 0x0600430E RID: 17166 RVA: 0x0013EAAC File Offset: 0x0013CCAC
	public override void Redraw()
	{
		base.Redraw();
		this.UnsubscribeFromHp();
		UISquadHudElementWidgetData data = this.data;
		HPComponent hpcomponent;
		if (data == null)
		{
			hpcomponent = null;
		}
		else
		{
			WgoData fighter = data.Fighter;
			hpcomponent = ((fighter != null) ? fighter.HpComponent : null);
		}
		this.hpComponent = hpcomponent;
		if (this.hpComponent != null)
		{
			this.hpComponent.OnHpChanged += this.HandleHpChanged;
		}
		this.UpdateHealthBar();
		this.UpdateWeaponIcon();
	}

	// Token: 0x0600430F RID: 17167 RVA: 0x0013EB14 File Offset: 0x0013CD14
	private void UpdateHealthBar()
	{
		if (this.healthBar == null)
		{
			return;
		}
		if (this.hpComponent == null)
		{
			this.healthBar.value = 0f;
			return;
		}
		this.healthBar.maxValue = (float)this.hpComponent.MaxHpValue;
		this.healthBar.value = (float)this.hpComponent.Hp;
	}

	// Token: 0x06004310 RID: 17168 RVA: 0x0013EB78 File Offset: 0x0013CD78
	private void UpdateWeaponIcon()
	{
		if (this.weaponIcon == null)
		{
			return;
		}
		Item currentWeapon = this.GetCurrentWeapon();
		Sprite sprite = this.GetWeaponIcon(currentWeapon);
		if (sprite == null)
		{
			this.weaponIcon.gameObject.SetActive(false);
			return;
		}
		this.weaponIcon.sprite = sprite;
		this.weaponIcon.gameObject.SetActive(true);
	}

	// Token: 0x06004311 RID: 17169 RVA: 0x0013EBDC File Offset: 0x0013CDDC
	private Item GetCurrentWeapon()
	{
		UISquadHudElementWidgetData data = this.data;
		ZombieWgoData zombieWgoData = ((data != null) ? data.Fighter : null) as ZombieWgoData;
		if (zombieWgoData != null)
		{
			return zombieWgoData.Hand;
		}
		UISquadHudElementWidgetData data2 = this.data;
		if (data2 == null)
		{
			return null;
		}
		WgoData fighter = data2.Fighter;
		if (fighter == null)
		{
			return null;
		}
		Inventory inventory = fighter.Inventory;
		if (inventory == null)
		{
			return null;
		}
		return inventory.GetItemByGroupId("weapon");
	}

	// Token: 0x06004312 RID: 17170 RVA: 0x0013EC38 File Offset: 0x0013CE38
	private Sprite GetWeaponIcon(Item weapon)
	{
		if (weapon == null || weapon.IsEmpty || weapon.Definition == null)
		{
			return null;
		}
		ItemType type = weapon.Definition.type;
		Sprite sprite;
		if (type != ItemType.Sword)
		{
			if (type != ItemType.Bow)
			{
				if (type != ItemType.Pike)
				{
					sprite = null;
				}
				else
				{
					sprite = this.spearIcon;
				}
			}
			else
			{
				sprite = this.bowIcon;
			}
		}
		else
		{
			sprite = this.swordIcon;
		}
		return sprite;
	}

	// Token: 0x06004313 RID: 17171 RVA: 0x0013EC95 File Offset: 0x0013CE95
	private void HandleHpChanged(HPComponent component)
	{
		this.UpdateHealthBar();
	}

	// Token: 0x06004314 RID: 17172 RVA: 0x0013EC9D File Offset: 0x0013CE9D
	private void UnsubscribeFromHp()
	{
		if (this.hpComponent != null)
		{
			this.hpComponent.OnHpChanged -= this.HandleHpChanged;
			this.hpComponent = null;
		}
	}

	// Token: 0x06004315 RID: 17173 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003450 RID: 13392
	[SerializeField]
	private Image weaponIcon;

	// Token: 0x04003451 RID: 13393
	[SerializeField]
	private Slider healthBar;

	// Token: 0x04003452 RID: 13394
	[SerializeField]
	private Sprite bowIcon;

	// Token: 0x04003453 RID: 13395
	[SerializeField]
	private Sprite spearIcon;

	// Token: 0x04003454 RID: 13396
	[SerializeField]
	private Sprite swordIcon;

	// Token: 0x04003455 RID: 13397
	private HPComponent hpComponent;
}
