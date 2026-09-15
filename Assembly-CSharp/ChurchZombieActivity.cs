using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class ChurchZombieActivity : MonoBehaviour
{
	// Token: 0x06000980 RID: 2432 RVA: 0x00030378 File Offset: 0x0002E578
	private void Start()
	{
		this.wgoPart = base.GetComponentInParent<WgoPart>();
		if (this.wgoPart == null || this.wgoPart.Wgo == null)
		{
			return;
		}
		this.wgoPart.Wgo.Data.Inventory.OnItemsAdd += this.OnItemsChanged;
		this.wgoPart.Wgo.Data.Inventory.OnItemsRemove += this.OnItemsChanged;
		this.OnItemsChanged(null);
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00030406 File Offset: 0x0002E606
	private void OnEnable()
	{
		if (this.wgoPart == null || this.wgoPart.Wgo == null)
		{
			return;
		}
		this.OnItemsChanged(null);
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00030434 File Offset: 0x0002E634
	private void OnDestroy()
	{
		if (this.wgoPart == null || this.wgoPart.Wgo == null || this.wgoPart.Wgo.Data == null)
		{
			return;
		}
		this.wgoPart.Wgo.Data.Inventory.OnItemsAdd -= this.OnItemsChanged;
		this.wgoPart.Wgo.Data.Inventory.OnItemsRemove -= this.OnItemsChanged;
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x000304C4 File Offset: 0x0002E6C4
	private void OnItemsChanged(List<Item> items = null)
	{
		List<Item> itemsByGroupId = this.wgoPart.Wgo.Data.Inventory.GetItemsByGroupId("zombie");
		for (int i = 0; i < this.animComponents.Count; i++)
		{
			if (itemsByGroupId.Count <= i)
			{
				this.animComponents[i].gameObject.SetActive(false);
			}
			else
			{
				BodyZombieSkinSerializedItemProperty bodyZombieSkinSerializedItemProperty;
				SkinPresetGK2 skinPresetGK;
				if (itemsByGroupId[i].TryGetProperty<BodyZombieSkinSerializedItemProperty>(out bodyZombieSkinSerializedItemProperty))
				{
					switch (bodyZombieSkinSerializedItemProperty.head)
					{
					case 1050:
					case 1056:
						skinPresetGK = ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", 1001, 1050, string.Empty, bodyZombieSkinSerializedItemProperty.headLut);
						goto IL_0146;
					case 1052:
					case 1054:
						skinPresetGK = ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", 1001, 1052, string.Empty, bodyZombieSkinSerializedItemProperty.headLut);
						goto IL_0146;
					case 1058:
						skinPresetGK = ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", 1001, 1058, string.Empty, bodyZombieSkinSerializedItemProperty.headLut);
						goto IL_0146;
					}
					skinPresetGK = ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", 1001, 1051, string.Empty, bodyZombieSkinSerializedItemProperty.headLut);
				}
				else
				{
					skinPresetGK = ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", 1001, 1051, string.Empty, "hed_lut_01");
				}
				IL_0146:
				if (skinPresetGK != null)
				{
					this.animComponents[i].ChangeSkinPreset(skinPresetGK);
				}
				this.animComponents[i].gameObject.SetActive(true);
				this.animComponents[i].SetTrigger(this.GetStopActionHash());
			}
		}
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00030678 File Offset: 0x0002E878
	public void StartAction()
	{
		if (this.animComponents.IsNullOrEmpty<AnimationComponent>())
		{
			return;
		}
		foreach (AnimationComponent animationComponent in this.animComponents)
		{
			animationComponent.Animator.ResetTrigger(this.GetStopActionHash());
			animationComponent.Animator.SetTrigger(this.GetStartActionHash());
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x000306F4 File Offset: 0x0002E8F4
	public void StopAction()
	{
		if (this.animComponents.IsNullOrEmpty<AnimationComponent>())
		{
			return;
		}
		foreach (AnimationComponent animationComponent in this.animComponents)
		{
			animationComponent.Animator.ResetTrigger(this.GetStartActionHash());
			animationComponent.SetTrigger(this.GetStopActionHash());
		}
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0003076C File Offset: 0x0002E96C
	private int GetStartActionHash()
	{
		ChurchZombieActivity.ActivityType activityType = this.activityType;
		if (activityType == ChurchZombieActivity.ActivityType.Choir)
		{
			return ChurchZombieActivity.startChoirSingingHash;
		}
		if (activityType != ChurchZombieActivity.ActivityType.Organ)
		{
			return 0;
		}
		return ChurchZombieActivity.startOrganPlayingHash;
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00030798 File Offset: 0x0002E998
	private int GetStopActionHash()
	{
		ChurchZombieActivity.ActivityType activityType = this.activityType;
		if (activityType == ChurchZombieActivity.ActivityType.Choir)
		{
			return ChurchZombieActivity.stopChoirSingingHash;
		}
		if (activityType != ChurchZombieActivity.ActivityType.Organ)
		{
			return 0;
		}
		return ChurchZombieActivity.stopOrganPlayingHash;
	}

	// Token: 0x04000B20 RID: 2848
	private static readonly int startChoirSingingHash = Animator.StringToHash("choir_singing");

	// Token: 0x04000B21 RID: 2849
	private static readonly int stopChoirSingingHash = Animator.StringToHash("choir_static");

	// Token: 0x04000B22 RID: 2850
	private static readonly int startOrganPlayingHash = Animator.StringToHash("organ_playing");

	// Token: 0x04000B23 RID: 2851
	private static readonly int stopOrganPlayingHash = Animator.StringToHash("organ_static");

	// Token: 0x04000B24 RID: 2852
	[SerializeField]
	private ChurchZombieActivity.ActivityType activityType;

	// Token: 0x04000B25 RID: 2853
	[SerializeField]
	private List<AnimationComponent> animComponents;

	// Token: 0x04000B26 RID: 2854
	[SerializeField]
	private WgoPart wgoPart;

	// Token: 0x02000186 RID: 390
	private enum ActivityType
	{
		// Token: 0x04000B28 RID: 2856
		Choir,
		// Token: 0x04000B29 RID: 2857
		Organ
	}
}
