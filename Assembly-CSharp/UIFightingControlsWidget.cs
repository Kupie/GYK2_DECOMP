using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009C3 RID: 2499
public class UIFightingControlsWidget : LazyWidget<UIFightingControlsWidgetData>
{
	// Token: 0x06004288 RID: 17032 RVA: 0x0013BAEE File Offset: 0x00139CEE
	public override void Redraw()
	{
		base.Redraw();
		this.RedrawWeapons();
		this.RedrawPose();
	}

	// Token: 0x06004289 RID: 17033 RVA: 0x0013BB04 File Offset: 0x00139D04
	private void RedrawWeapons()
	{
		bool flag = MainGame.PlayerController.Sword.id != "empty";
		bool flag2 = MainGame.PlayerController.Bow.id != "empty";
		this.canChangeWeapon = flag && flag2;
		this.changeWeaponObj.SetActive(this.canChangeWeapon);
		AttackComponent attackComponent = MainGame.PlayerController.AttackComponent;
		if (!attackComponent.HasEquippedWeapon)
		{
			return;
		}
		ItemDef itemDef = attackComponent.weapon.ItemDef;
		this.activeWeaponType = itemDef.type;
		this.activeWeaponIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(itemDef.iconId, null);
		this.activeWeaponIcon.BlueColorReplace(this.activeCol);
		if (this.canChangeWeapon)
		{
			ItemDef itemDef2 = ((this.activeWeaponType == ItemType.Sword) ? MainGame.PlayerController.Bow.Definition : MainGame.PlayerController.Sword.Definition);
			this.inactiveWeaponIcon.sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(itemDef2.iconId, null);
			this.inactiveWeaponIcon.BlueColorReplace(this.inactiveCol);
		}
	}

	// Token: 0x0600428A RID: 17034 RVA: 0x0013BC1C File Offset: 0x00139E1C
	private void UpdateCombatFlags()
	{
		this.isPlayerInFocus = this.IsAttackFocusInputActive();
		this.focusedDirection = Direction.None;
		SSMState curState = MainGame.PlayerController.Ssm.CurState;
		AttackSwordFocusedPlayerState attackSwordFocusedPlayerState = curState as AttackSwordFocusedPlayerState;
		if (attackSwordFocusedPlayerState != null && attackSwordFocusedPlayerState.IsActive)
		{
			this.isPlayerInFocus = true;
			this.focusedDirection = attackSwordFocusedPlayerState.FocusedDirection;
		}
		else
		{
			AttackBowFocusedPlayerState attackBowFocusedPlayerState = curState as AttackBowFocusedPlayerState;
			if (attackBowFocusedPlayerState != null && attackBowFocusedPlayerState.IsActive)
			{
				this.isPlayerInFocus = true;
				this.focusedDirection = attackBowFocusedPlayerState.FocusedDirection;
			}
			else if (this.isPlayerInFocus)
			{
				this.focusedDirection = MainGame.PlayerController.PlayerData.Direction.ConvertFromVector2();
			}
		}
		AttackSwordPlayerState attackSwordPlayerState = curState as AttackSwordPlayerState;
		bool flag;
		if (attackSwordPlayerState != null)
		{
			if (attackSwordPlayerState.IsActive)
			{
				flag = true;
				goto IL_0137;
			}
		}
		else
		{
			AttackSwordDefaultPlayerState attackSwordDefaultPlayerState = curState as AttackSwordDefaultPlayerState;
			if (attackSwordDefaultPlayerState != null)
			{
				if (attackSwordDefaultPlayerState.IsActive)
				{
					flag = true;
					goto IL_0137;
				}
			}
			else
			{
				AttackSwordContinuousPlayerState attackSwordContinuousPlayerState = curState as AttackSwordContinuousPlayerState;
				if (attackSwordContinuousPlayerState != null)
				{
					if (attackSwordContinuousPlayerState.IsActive)
					{
						flag = true;
						goto IL_0137;
					}
				}
				else
				{
					AttackBowDefaultPlayerState attackBowDefaultPlayerState = curState as AttackBowDefaultPlayerState;
					if (attackBowDefaultPlayerState != null)
					{
						if (attackBowDefaultPlayerState.IsActive)
						{
							flag = true;
							goto IL_0137;
						}
					}
					else
					{
						AttackBowAutoPlayerState attackBowAutoPlayerState = curState as AttackBowAutoPlayerState;
						if (attackBowAutoPlayerState != null)
						{
							if (attackBowAutoPlayerState.IsActive)
							{
								flag = true;
								goto IL_0137;
							}
						}
					}
				}
			}
		}
		flag = MainGame.PlayerController.View.PlayerAnimation.GetLayerWeight(AnimationComponent.Layers.WeaponHitBox) > 0f;
		IL_0137:
		this.isAttackAnimPlaying = flag;
	}

	// Token: 0x0600428B RID: 17035 RVA: 0x0013BD68 File Offset: 0x00139F68
	private bool IsAttackFocusInputActive()
	{
		PlayerController playerController = MainGame.PlayerController;
		if (!playerController.IsControlsEnabled || !PlayerInputHandler.IsAttackFocusHeld())
		{
			return false;
		}
		AttackComponent attackComponent = playerController.AttackComponent;
		if (!attackComponent.HasEquippedWeapon)
		{
			return false;
		}
		PlayerInputHandler playerInputHandler = playerController.PlayerInputHandler;
		if (!attackComponent.IsRangedWeapon)
		{
			return playerInputHandler.meleeMode == MeleeMode.WithFocus;
		}
		return playerInputHandler.rangedMode == RangedMode.WithFocus;
	}

	// Token: 0x0600428C RID: 17036 RVA: 0x0013BDC0 File Offset: 0x00139FC0
	private void RedrawPose()
	{
		this.UpdateCombatFlags();
		this.arrowUp.SetActive(false);
		this.arrowDown.SetActive(false);
		this.arrowLeft.SetActive(false);
		this.arrowRight.SetActive(false);
		if (this.isPlayerInFocus)
		{
			this.poseImage.sprite = this.poseLocked;
			if (this.focusedDirection == Direction.Left)
			{
				this.poseImage.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				this.poseImage.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			switch (this.focusedDirection)
			{
			case Direction.None:
				break;
			case Direction.Right:
				this.arrowRight.SetActive(true);
				break;
			case Direction.Up:
				this.arrowUp.SetActive(true);
				break;
			case Direction.Left:
				this.arrowLeft.SetActive(true);
				break;
			case Direction.Down:
				this.arrowDown.SetActive(true);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			this.lockedPosGlow.SetActive(true);
			this.lockedPosSelection.SetActive(true);
			return;
		}
		this.lockedPosGlow.SetActive(false);
		this.lockedPosSelection.SetActive(false);
		this.poseImage.sprite = this.poseFree;
		this.poseImage.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	// Token: 0x0600428D RID: 17037 RVA: 0x0013BF3C File Offset: 0x0013A13C
	public void DrawGamepadTips()
	{
		this.poseGamepadTip.text = ControllerIconLibrary.GetIconId(GameKey.AttackFocus, null, true);
		this.attackGamepadTip.text = ControllerIconLibrary.GetIconId(GameKey.Attack, null, true);
		this.changeWeaponGamepadTip.text = ControllerIconLibrary.GetIconId(GameKey.ChangeWeapon, null, true);
	}

	// Token: 0x0600428E RID: 17038 RVA: 0x0013BF90 File Offset: 0x0013A190
	public override void CustomUpdate()
	{
		base.CustomUpdate();
		this.UpdateCombatFlags();
		if (LazyInput.GetKeyDown(GameKey.ChangeWeapon))
		{
			this.RedrawWeapons();
			if (this.canChangeWeapon && !this.isAttackAnimPlaying && !this.isPlayerInFocus)
			{
				if (this.activeWeaponType == ItemType.Sword)
				{
					MainGame.PlayerController.AttackComponent.EquipWeapon(MainGame.PlayerController.Bow.Definition);
				}
				else
				{
					MainGame.PlayerController.AttackComponent.EquipWeapon(MainGame.PlayerController.Sword.Definition);
				}
			}
		}
		this.RedrawWeapons();
		this.RedrawPose();
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040033DA RID: 13274
	[SerializeField]
	private GameObject changeWeaponObj;

	// Token: 0x040033DB RID: 13275
	[SerializeField]
	private Image poseImage;

	// Token: 0x040033DC RID: 13276
	[SerializeField]
	private Sprite poseLocked;

	// Token: 0x040033DD RID: 13277
	[SerializeField]
	private Sprite poseFree;

	// Token: 0x040033DE RID: 13278
	[SerializeField]
	private GameObject arrowDown;

	// Token: 0x040033DF RID: 13279
	[SerializeField]
	private GameObject arrowUp;

	// Token: 0x040033E0 RID: 13280
	[SerializeField]
	private GameObject arrowLeft;

	// Token: 0x040033E1 RID: 13281
	[SerializeField]
	private GameObject arrowRight;

	// Token: 0x040033E2 RID: 13282
	[SerializeField]
	private GameObject lockedPosGlow;

	// Token: 0x040033E3 RID: 13283
	[SerializeField]
	private GameObject lockedPosSelection;

	// Token: 0x040033E4 RID: 13284
	[SerializeField]
	private Color activeCol;

	// Token: 0x040033E5 RID: 13285
	[SerializeField]
	private Color inactiveCol;

	// Token: 0x040033E6 RID: 13286
	[SerializeField]
	private Image activeWeaponIcon;

	// Token: 0x040033E7 RID: 13287
	[SerializeField]
	private Image inactiveWeaponIcon;

	// Token: 0x040033E8 RID: 13288
	[SerializeField]
	private TextMeshProUGUI poseGamepadTip;

	// Token: 0x040033E9 RID: 13289
	[SerializeField]
	private TextMeshProUGUI attackGamepadTip;

	// Token: 0x040033EA RID: 13290
	[SerializeField]
	private TextMeshProUGUI changeWeaponGamepadTip;

	// Token: 0x040033EB RID: 13291
	private bool canChangeWeapon;

	// Token: 0x040033EC RID: 13292
	private bool isPlayerInFocus;

	// Token: 0x040033ED RID: 13293
	private bool isAttackAnimPlaying;

	// Token: 0x040033EE RID: 13294
	private Direction focusedDirection;

	// Token: 0x040033EF RID: 13295
	private ItemType activeWeaponType;
}
