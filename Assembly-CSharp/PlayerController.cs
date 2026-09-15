using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LazyBearTechnology;
using Pathfinding;
using UnityEngine;

// Token: 0x0200037A RID: 890
public class PlayerController : MonoBehaviour, IMovable, IWorker, ISoundZoneRecognizable
{
	// Token: 0x1400003A RID: 58
	// (add) Token: 0x060017A1 RID: 6049 RVA: 0x0006FE6C File Offset: 0x0006E06C
	// (remove) Token: 0x060017A2 RID: 6050 RVA: 0x0006FEA4 File Offset: 0x0006E0A4
	public event Action OnControlStateChanged;

	// Token: 0x1400003B RID: 59
	// (add) Token: 0x060017A3 RID: 6051 RVA: 0x0006FEDC File Offset: 0x0006E0DC
	// (remove) Token: 0x060017A4 RID: 6052 RVA: 0x0006FF14 File Offset: 0x0006E114
	public event Action<bool> OnActiveStateChanged;

	// Token: 0x1400003C RID: 60
	// (add) Token: 0x060017A5 RID: 6053 RVA: 0x0006FF4C File Offset: 0x0006E14C
	// (remove) Token: 0x060017A6 RID: 6054 RVA: 0x0006FF84 File Offset: 0x0006E184
	public event Action OnDeathAnimFinished;

	// Token: 0x1400003D RID: 61
	// (add) Token: 0x060017A7 RID: 6055 RVA: 0x0006FFBC File Offset: 0x0006E1BC
	// (remove) Token: 0x060017A8 RID: 6056 RVA: 0x0006FFF0 File Offset: 0x0006E1F0
	public static event Action OnPlayerTeleported;

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x060017A9 RID: 6057 RVA: 0x00070023 File Offset: 0x0006E223
	public PlayerData PlayerData
	{
		get
		{
			return this.playerData;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x060017AA RID: 6058 RVA: 0x0007002B File Offset: 0x0006E22B
	public bool IsControlsEnabled
	{
		get
		{
			return this.playerTakenControlMultiFlag.ResultFlag;
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x060017AB RID: 6059 RVA: 0x00070038 File Offset: 0x0006E238
	public bool IsControlsEnabledForInteractionHints
	{
		get
		{
			return this.IsControlsEnabledExcept(new TakenControlType[]
			{
				TakenControlType.ByWork,
				TakenControlType.ByLadder,
				TakenControlType.ByAttack
			});
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x060017AC RID: 6060 RVA: 0x0007004E File Offset: 0x0006E24E
	public PlayerPhysicalBody PhysicalBody
	{
		get
		{
			return this.playerPhysicalBody;
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x060017AD RID: 6061 RVA: 0x00070056 File Offset: 0x0006E256
	public PlayerView View
	{
		get
		{
			return this.playerView;
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x060017AE RID: 6062 RVA: 0x0007005E File Offset: 0x0006E25E
	public Transform BubblePoint
	{
		get
		{
			return this.playerView.BubblePoint;
		}
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x060017AF RID: 6063 RVA: 0x0007006B File Offset: 0x0006E26B
	public Transform LarryInPocketBubblePoint
	{
		get
		{
			return this.playerView.LarryInPocketBubblePoint;
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x060017B0 RID: 6064 RVA: 0x00070078 File Offset: 0x0006E278
	public PlayerInteractionComponent PlayerInteractionComponent
	{
		get
		{
			return this.playerInteractionComponent;
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x060017B1 RID: 6065 RVA: 0x00070080 File Offset: 0x0006E280
	public PlayerLocalAreaMovement PlayerLocalAreaMovement
	{
		get
		{
			return this.playerLocalAreaMovement;
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x060017B2 RID: 6066 RVA: 0x00070088 File Offset: 0x0006E288
	public PlayerInputHandler PlayerInputHandler
	{
		get
		{
			return this.playerInputHandler;
		}
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x060017B3 RID: 6067 RVA: 0x00070090 File Offset: 0x0006E290
	public PlayerWorkComponent PlayerWorkComponent
	{
		get
		{
			return this.playerWorkComponent;
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x060017B4 RID: 6068 RVA: 0x00070098 File Offset: 0x0006E298
	public PlayerFishingComponent FishingComponent
	{
		get
		{
			return this.fishingComponent;
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x060017B5 RID: 6069 RVA: 0x000700A0 File Offset: 0x0006E2A0
	public AttackComponent AttackComponent
	{
		get
		{
			return this.attackComponent;
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x060017B6 RID: 6070 RVA: 0x000700A8 File Offset: 0x0006E2A8
	public MovementComponent MovementComponent
	{
		get
		{
			return this.movementComponent;
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x060017B7 RID: 6071 RVA: 0x000700B0 File Offset: 0x0006E2B0
	public PlayerColorCustomizationData CharacterCustomizationData
	{
		get
		{
			return this.characterCustomizationData;
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x060017B8 RID: 6072 RVA: 0x000700B8 File Offset: 0x0006E2B8
	public SSM Ssm
	{
		get
		{
			return this.ssm;
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x060017B9 RID: 6073 RVA: 0x000700C0 File Offset: 0x0006E2C0
	public LadderClimbController LadderClimbController
	{
		get
		{
			return this.ladderClimbController;
		}
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x060017BA RID: 6074 RVA: 0x000700C8 File Offset: 0x0006E2C8
	public WispController WispController
	{
		get
		{
			return this.playerView.WispController;
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x060017BB RID: 6075 RVA: 0x000700D5 File Offset: 0x0006E2D5
	public Item Sword
	{
		get
		{
			return this.PlayerData.toolBeltInventory.Data.GetItemByType(ItemType.Sword);
		}
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x060017BC RID: 6076 RVA: 0x000700EE File Offset: 0x0006E2EE
	public Item Bow
	{
		get
		{
			return this.PlayerData.toolBeltInventory.Data.GetItemByType(ItemType.Bow);
		}
	}

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x060017BD RID: 6077 RVA: 0x00070107 File Offset: 0x0006E307
	// (set) Token: 0x060017BE RID: 6078 RVA: 0x00070129 File Offset: 0x0006E329
	[CanBeNull]
	public GameScene CurrentGameScene
	{
		get
		{
			if (this.currentGameScene == null)
			{
				Debug.LogWarning("[PlayerController]: no active game scene found");
				return null;
			}
			return this.currentGameScene;
		}
		set
		{
			this.TrySetCurrentGameScene(value, null);
		}
	}

	// Token: 0x060017BF RID: 6079 RVA: 0x00070134 File Offset: 0x0006E334
	public bool TryGetCurrentGameScene(out GameScene gameScene)
	{
		gameScene = this.currentGameScene;
		return gameScene != null;
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x00070148 File Offset: 0x0006E348
	public bool TrySetCurrentGameScene([CanBeNull] GameScene gameScene, string sceneId = null)
	{
		if (gameScene == null)
		{
			Debug.LogError("[PlayerController]: failed to set CurrentGameScene" + (string.IsNullOrEmpty(sceneId) ? "" : (" for scene [" + sceneId + "]")));
			return false;
		}
		this.currentGameScene = gameScene;
		List<int> recastGraphIndexByWorldId = GraphHelper.Instance.SceneGraphsData.GetRecastGraphIndexByWorldId(this.currentGameScene.Id);
		if (recastGraphIndexByWorldId.Count > 0)
		{
			this.SceneRecastGraph = AstarPath.active.data.graphs[recastGraphIndexByWorldId[0]] as RecastGraph;
		}
		return true;
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x000701DC File Offset: 0x0006E3DC
	public void ClearCurrentGameScene()
	{
		this.currentGameScene = null;
		this.SceneRecastGraph = null;
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x060017C2 RID: 6082 RVA: 0x000701EC File Offset: 0x0006E3EC
	// (set) Token: 0x060017C3 RID: 6083 RVA: 0x000701F4 File Offset: 0x0006E3F4
	[CanBeNull]
	public RecastGraph SceneRecastGraph { get; private set; }

	// Token: 0x060017C4 RID: 6084 RVA: 0x00070200 File Offset: 0x0006E400
	public void Initialize()
	{
		if (!this.isInitialized)
		{
			if (PlayerController.controlsEnumArray == null)
			{
				PlayerController.controlsEnumArray = Enum.GetValues(typeof(TakenControlType));
			}
			if (PlayerController.activeEnumArray == null)
			{
				PlayerController.activeEnumArray = Enum.GetValues(typeof(DisabledStateType));
			}
			this.playerTakenControlMultiFlag.Init(new Action<bool>(this.HandleControlChange), true);
			this.playerDisabledStateMultiFlag.Init(new Action<bool>(this.HandleActiveStateChangedChange), true);
			this.playerInputHandler = new PlayerInputHandler(this);
			this.playerPhysicalBody.Init();
			this.playerInteractionComponent.Init();
			this.toolComponent = new ToolComponent(this.playerView.PlayerAnimation);
			this.ladderClimbController.Init(this.playerView.PlayerAnimation);
			this.playerWorkComponent = base.GetComponent<PlayerWorkComponent>();
			this.attackComponent = base.GetComponent<AttackComponent>();
			this.fishingComponent = base.GetComponent<PlayerFishingComponent>();
			this.playerWorkComponent.Init(this, this.toolComponent, this.playerLocalAreaMovement);
			this.movementComponent.Init(this);
			this.playerLocalAreaMovement.Init(this.playerPhysicalBody);
			this.ssm = new SSM(new FreePlayerState(this));
			this.ssm.AddState(new WorkPlayerState(this));
			this.ssm.AddState(new LadderPlayerState(this, this.ladderClimbController));
			this.ssm.AddState(new BuildPlayerState(this));
			this.ssm.AddState(new PlantingPlayerState(this));
			this.ssm.AddState(new AttackSwordPlayerState(this));
			this.ssm.AddState(new AttackSwordDefaultPlayerState(this));
			this.ssm.AddState(new AttackSwordFocusedPlayerState(this));
			this.ssm.AddState(new AttackSwordContinuousPlayerState(this));
			this.ssm.AddState(new AttackBowFocusedPlayerState(this));
			this.ssm.AddState(new AttackBowDefaultPlayerState(this));
			this.ssm.AddState(new AttackBowAutoPlayerState(this));
			this.isInitialized = true;
			this.attackComponent.Init(this.PhysicalBody, null, null);
			AttackComponent attackComponent = this.attackComponent;
			attackComponent.OnWeaponChanged = (Action)Delegate.Combine(attackComponent.OnWeaponChanged, new Action(this.UpdateArmorLayers));
		}
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x00070438 File Offset: 0x0006E638
	public void PreparePlayerForGame(PlayerData playerData)
	{
		this.SetPlayerData(playerData);
		playerData.hpComponent.SetCustomHpValue(GameBalance.Me.GetData<ConstDef>("player_hp").IntValue, true);
		this.ResetControlState();
		playerData.hpComponent.OnHpChanged += this.HandleHpChanged;
		this.HandleHpChanged(playerData.hpComponent);
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x00070498 File Offset: 0x0006E698
	public void UnPreparePlayerFromGame()
	{
		this.attackComponent.UnequipWeapon();
		this.playerView.PlayerAnimation.SetDirection(Direction.Down);
		this.playerView.PlayerAnimation.SetState(global::AnimationState.Idle);
		this.playerView.PlayerAnimation.Animator.Rebind();
		this.SetArmorView(false, null, true);
		this.RemoveTheFlag();
		this.RemoveOverheadItem();
		this.playerPhysicalBody.UnPrepareFromGame();
		this.playerData.hpComponent.OnHpChanged -= this.HandleHpChanged;
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x0007052C File Offset: 0x0006E72C
	public void SetPlayerData(PlayerData playerData)
	{
		this.playerData = playerData;
		this.playerPhysicalBody.PrepareForGame(playerData);
		this.playerInteractionComponent.SetPlayerData(playerData);
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x00070550 File Offset: 0x0006E750
	public void SetPosition(Vector3 position, bool updateWispWgoData = true, bool instantCameraUpdate = true)
	{
		this.playerPhysicalBody.SetPosition(position);
		if (instantCameraUpdate && CameraSystem.Instance.ActiveCameraController.Target == this.playerPhysicalBody.PlayerView.transform)
		{
			CameraSystem.Instance.ActiveCameraController.UpdateTargetPosInstant();
		}
		if (this.playerView.ControllingWispView)
		{
			if (this.playerView.WispController.TargetTransform == null)
			{
				this.playerView.UpdateWispDirection();
			}
			this.playerView.WispController.TeleportToTarget(updateWispWgoData);
		}
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x000705E4 File Offset: 0x0006E7E4
	public void SetControlTakenType(TakenControlType t, bool isEnabled)
	{
		Debug.Log(string.Format("Player SetControlTakenType:[{0}] isEnabled:[{1}]", t, isEnabled));
		this.playerTakenControlMultiFlag.UpdateFlag(t, isEnabled);
		if (t == TakenControlType.ByFlow || t == TakenControlType.ByBuilding)
		{
			this.playerPhysicalBody.SetNonKinematicFlag(PlayerDynamicType.ByFlowPlayerControl, isEnabled);
		}
		if (t != TakenControlType.BySelf && !isEnabled)
		{
			this.playerData.RemoveInteractingItem();
		}
		Action onControlStateChanged = this.OnControlStateChanged;
		if (onControlStateChanged == null)
		{
			return;
		}
		onControlStateChanged();
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x00070652 File Offset: 0x0006E852
	public void SetDisabledStateType(DisabledStateType t, bool isEnabled)
	{
		Debug.Log(string.Format("Player SetDisabledStateType:[{0}] isEnabled:[{1}]", t, isEnabled));
		this.playerDisabledStateMultiFlag.UpdateFlag(t, isEnabled);
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x0007067C File Offset: 0x0006E87C
	public bool IsControlsEnabledExcept(params TakenControlType[] controls)
	{
		return this.playerTakenControlMultiFlag.GetResultFlagExceptFlagTypesNonAlloc(controls);
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x0007068A File Offset: 0x0006E88A
	public bool IsControlEnabledByType(TakenControlType t)
	{
		return this.playerTakenControlMultiFlag.GetFlag(t);
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x00070698 File Offset: 0x0006E898
	public void ResetControlState()
	{
		foreach (object obj in PlayerController.controlsEnumArray)
		{
			TakenControlType takenControlType = (TakenControlType)obj;
			this.playerTakenControlMultiFlag.UpdateFlag(takenControlType, true);
		}
		Action onControlStateChanged = this.OnControlStateChanged;
		if (onControlStateChanged == null)
		{
			return;
		}
		onControlStateChanged();
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x00070708 File Offset: 0x0006E908
	private void Update()
	{
		this.ssm.CustomUpdate();
		if (this.attachedWgo)
		{
			this.attachedWgo.Data.Position = this.playerData.position.Value;
		}
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x00070742 File Offset: 0x0006E942
	private void FixedUpdate()
	{
		this.ssm.CustomFixedUpdate();
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x00070750 File Offset: 0x0006E950
	private void HandleControlChange(bool isEnabled)
	{
		if (!isEnabled)
		{
			if (this.playerData != null && this.playerData.charState.Value == global::AnimationState.Walk)
			{
				this.playerPhysicalBody.StopMoving();
			}
			if (this.playerView.PlayerAnimation.GetState() == global::AnimationState.Walk)
			{
				this.playerView.PlayerAnimation.SetState(global::AnimationState.Idle);
			}
		}
		this.playerView.PlayerAnimation.EyesBlinkingMultiflag.UpdateFlag(PlayerAnimation.EyesBlinkingReason.ControlValue, isEnabled);
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x000707C1 File Offset: 0x0006E9C1
	private void HandleActiveStateChangedChange(bool isEnabled)
	{
		Action<bool> onActiveStateChanged = this.OnActiveStateChanged;
		if (onActiveStateChanged != null)
		{
			onActiveStateChanged(isEnabled);
		}
		if (isEnabled)
		{
			this.Enable();
		}
		else
		{
			this.Disable();
		}
		this.playerView.PlayerAnimation.EyesBlinkingMultiflag.UpdateFlag(PlayerAnimation.EyesBlinkingReason.EnabledState, isEnabled);
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x000707FD File Offset: 0x0006E9FD
	private void Enable()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x00027874 File Offset: 0x00025A74
	private void Disable()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x0007080C File Offset: 0x0006EA0C
	private void HandleHpChanged(HPComponent hpComponent)
	{
		this.playerView.UpdateHpBarState(hpComponent);
		Dictionary<string, GlobalEventsSystem.Event> dictionary;
		if (GlobalEventsSystem.Me.GetEvents(GlobalEventsSystem.Event.Type.PlayerHpValueReached, out dictionary))
		{
			List<string> list = null;
			foreach (KeyValuePair<string, GlobalEventsSystem.Event> keyValuePair in dictionary)
			{
				int num;
				if (int.TryParse(keyValuePair.Key, out num) && num < hpComponent.prevHp && num >= hpComponent.Hp)
				{
					List<string> list2;
					if ((list2 = list) == null)
					{
						list2 = (list = new List<string>());
					}
					list2.Add(keyValuePair.Key);
				}
			}
			if (list != null)
			{
				foreach (string text in list)
				{
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerHpValueReached, text);
				}
			}
		}
		if (hpComponent.Hp == 0)
		{
			this.SetControlTakenType(TakenControlType.ByDeath, false);
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerDead, "");
			this.playerView.PlayerAnimation.SetState(global::AnimationState.Death);
		}
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x00070924 File Offset: 0x0006EB24
	public void SetOverheadItem(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			this.RemoveOverheadItem();
			return;
		}
		this.SetOverheadItems(new Item[] { item });
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x00070948 File Offset: 0x0006EB48
	public void SetOverheadItems(IReadOnlyList<Item> items)
	{
		this.playerView.PlayerAnimation.SetLayerWeight(3, 1f);
		this.playerView.PlayerAnimation.SetOverheadItems(items);
		this.isOverheadActive = true;
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x00070978 File Offset: 0x0006EB78
	public void RemoveOverheadItem()
	{
		this.playerView.PlayerAnimation.SetLayerWeight(3, 0f);
		this.playerView.PlayerAnimation.RemoveOverheadItem();
		this.isOverheadActive = false;
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x000709A8 File Offset: 0x0006EBA8
	public void SetInteractingItem(Item item, int totalCount)
	{
		if (this.playerData.HasMultipleOverheadItems)
		{
			return;
		}
		if (this.playerData.HasOverheadItem)
		{
			this.playerData.DropOverheadItem();
		}
		this.playerView.PlayerAnimation.SetLayerWeight(5, 1f);
		if (item != null)
		{
			this.playerView.SetInteractingItem(item, totalCount);
		}
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00070A01 File Offset: 0x0006EC01
	public void RemoveInteractingItem()
	{
		if (!this.isOverheadActive)
		{
			this.playerView.PlayerAnimation.SetLayerWeight(5, 0f);
		}
		this.playerView.RemoveInteractingItem();
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x00070A2C File Offset: 0x0006EC2C
	public void UpdateArmorLayers()
	{
		if (!this.isArmorViewActive)
		{
			return;
		}
		AnimationComponent.Layers layers = (this.armorViewUsesHelmet ? AnimationComponent.Layers.Armor : AnimationComponent.Layers.ArmorNoHelmet);
		if (this.armorViewUsesHelmet)
		{
			AttackComponent attackComponent = this.attackComponent;
			if (((attackComponent != null) ? attackComponent.weapon : null) != null)
			{
				layers = (this.attackComponent.IsRangedWeapon ? AnimationComponent.Layers.ArmorWithBow : AnimationComponent.Layers.ArmorWithSword);
			}
		}
		this.View.PlayerAnimation.ResetArmorLayers();
		this.View.PlayerAnimation.SetLayerWeight(layers, 1f);
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x00070AAC File Offset: 0x0006ECAC
	private void TryEquipArmorFromInventory()
	{
		if (!MainGame.PlayerData.toolBeltInventory.GetItemByType(ItemType.BodyArmor).IsEmpty)
		{
			return;
		}
		Item itemByType = MainGame.PlayerData.inventory.GetItemByType(ItemType.BodyArmor);
		if (!itemByType.IsEmpty)
		{
			MainGame.PlayerData.EquipItem(itemByType);
			return;
		}
		Debug.LogWarning("[PlayerController]: No armor in inventory");
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x00070B04 File Offset: 0x0006ED04
	public void SetArmorView(bool isActive, int? armorIndex = null, bool withHelmet = true)
	{
		this.isArmorViewActive = isActive;
		if (isActive)
		{
			this.TryEquipArmorFromInventory();
			this.armorViewUsesHelmet = withHelmet;
			int num = armorIndex ?? this.GetEquippedArmorColorIndex();
			SkinPresetGK2 skinPresetGK = (withHelmet ? PlayerSkinHelper.ArmorPreset : PlayerSkinHelper.GetArmorNoHelmetPreset());
			this.playerView.SetPlayerPreset(skinPresetGK, false);
			PlayerSkinHelper.ApplyArmorColorsByIndex(num, withHelmet);
			this.UpdateArmorLayers();
		}
		else
		{
			this.armorViewUsesHelmet = true;
			this.View.SetPlayerPreset(PlayerSkinHelper.CurrentPreset, false);
			this.View.PlayerAnimation.ResetArmorLayers();
			this.View.PlayerAnimation.UseAdditionalStepSound = false;
		}
		this.playerView.PlayerAnimation.EyesBlinkingMultiflag.UpdateFlag(PlayerAnimation.EyesBlinkingReason.ArmorEquippedEquippedState, !isActive || !withHelmet);
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x00070BC8 File Offset: 0x0006EDC8
	private int GetEquippedArmorColorIndex()
	{
		Item itemByType = this.PlayerData.toolBeltInventory.GetItemByType(ItemType.BodyArmor);
		if (itemByType == null || itemByType.IsEmpty)
		{
			return 0;
		}
		int num;
		if (!PlayerController.TryParseIndexFromItemId(itemByType.id, out num))
		{
			return 0;
		}
		return num;
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x00070C08 File Offset: 0x0006EE08
	private static bool TryParseIndexFromItemId(string itemId, out int index)
	{
		index = 0;
		if (string.IsNullOrEmpty(itemId))
		{
			return false;
		}
		int num = itemId.LastIndexOf('_');
		return num >= 0 && num < itemId.Length - 1 && int.TryParse(itemId.Substring(num + 1), out index);
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x00070C4B File Offset: 0x0006EE4B
	public void DisableWispControl()
	{
		this.playerView.ControllingWispView = false;
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x00070C59 File Offset: 0x0006EE59
	public void EnableWispControl()
	{
		this.playerView.ControllingWispView = true;
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x00070C68 File Offset: 0x0006EE68
	public static bool Teleport(TeleportDataBase teleportData)
	{
		PlayerController.<>c__DisplayClass122_0 CS$<>8__locals1 = new PlayerController.<>c__DisplayClass122_0();
		CS$<>8__locals1.teleportData = teleportData;
		string text;
		if (!CS$<>8__locals1.teleportData.CanTeleport(out text))
		{
			Debug.LogError(text);
			return false;
		}
		if (!string.IsNullOrEmpty(CS$<>8__locals1.teleportData.soundOnTeleport))
		{
			LazyAudio.Play(CS$<>8__locals1.teleportData.soundOnTeleport, false);
		}
		CS$<>8__locals1.uiFade = LazyUI.Get<UIFade>();
		CS$<>8__locals1.teleportData.environmentPreset = (string.IsNullOrEmpty(CS$<>8__locals1.teleportData.environmentPreset) ? "outdoor" : CS$<>8__locals1.teleportData.environmentPreset);
		CS$<>8__locals1.tpLogic = delegate(bool dontFade)
		{
			PlayerController.<>c__DisplayClass122_1 CS$<>8__locals2 = new PlayerController.<>c__DisplayClass122_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.dontFade = dontFade;
			string currentGameSceneId = MainGame.PlayerData.currentGameSceneId;
			if (CS$<>8__locals1.teleportData.GetDestinationSceneData() == null || !(currentGameSceneId != CS$<>8__locals1.teleportData.GetDestinationSceneData().id))
			{
				base.<Teleport>g__SetPosAndUnFadeAsync|2(CS$<>8__locals2.dontFade, false).Forget();
				return;
			}
			if (LazySingleton<GameSceneManager>.Instance.DisabledUnloadOnTeleport(currentGameSceneId))
			{
				CS$<>8__locals2.<Teleport>g__OnSceneUnloaded|4();
				return;
			}
			LazySingleton<GameSceneManager>.Instance.UnloadScene(currentGameSceneId, delegate
			{
				base.<Teleport>g__OnSceneUnloaded|4();
			});
		};
		MainGame.PlayerController.View.Banner.SetEnabledClothFading(false);
		MainGame.PlayerController.SetControlTakenType(TakenControlType.ByTeleport, false);
		if (!CS$<>8__locals1.teleportData.donNotFade)
		{
			CS$<>8__locals1.uiFade.FadeIn(delegate
			{
				PlayerController.<>c__DisplayClass122_0.<<Teleport>b__1>d <<Teleport>b__1>d;
				<<Teleport>b__1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<Teleport>b__1>d.<>4__this = CS$<>8__locals1;
				<<Teleport>b__1>d.<>1__state = -1;
				<<Teleport>b__1>d.<>t__builder.Start<PlayerController.<>c__DisplayClass122_0.<<Teleport>b__1>d>(ref <<Teleport>b__1>d);
			}, FadeFlag.Common, false);
		}
		else
		{
			PlayerController.SetNotDirectlyInGame(true);
			CS$<>8__locals1.tpLogic(CS$<>8__locals1.teleportData.donNotFade);
		}
		return true;
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x00070D6F File Offset: 0x0006EF6F
	private static void SetNotDirectlyInGame(bool active)
	{
		if (WeatherSystem.Instance == null)
		{
			return;
		}
		WeatherSystem.Instance.AudioMixerStateController.SetLayerActive(AudioMixerSnapshotLayer.NotDirectlyInGame, active);
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x00070D90 File Offset: 0x0006EF90
	public void TryTeleportPlayerToAnyFreePlace()
	{
		base.StartCoroutine(this.TryTeleportPlayerToAnyFreePlaceCoroutine());
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x00070D9F File Offset: 0x0006EF9F
	private IEnumerator TryTeleportPlayerToAnyFreePlaceCoroutine()
	{
		yield return new WaitForFixedUpdate();
		if (PlayerController.IsOverlappingSomething(this.PlayerData.position.Value, Vector3.zero, this.overlapRadiusForSearch))
		{
			Vector3 vector = this.<TryTeleportPlayerToAnyFreePlaceCoroutine>g__RandPos|125_0();
			bool flag = PlayerController.IsOverlappingSomething(this.PlayerData.position.Value + vector, Vector3.zero, this.overlapRadiusForSearch);
			int num = 15;
			if (flag)
			{
				while (flag && num > 0)
				{
					num--;
					vector = this.<TryTeleportPlayerToAnyFreePlaceCoroutine>g__RandPos|125_0();
					flag = PlayerController.IsOverlappingSomething(this.PlayerData.position.Value + vector, Vector3.zero, this.overlapRadiusForSearch);
				}
			}
			if (!flag)
			{
				this.SetPosition(this.PlayerData.position.Value + vector, true, false);
			}
		}
		yield break;
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x00070DB0 File Offset: 0x0006EFB0
	private static bool IsOverlappingSomething(Vector3 pos, Vector3 dir, float radius)
	{
		Collider[] array = Physics.OverlapSphere(pos + dir / 2f, radius, 257);
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].isTrigger)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x00070DF8 File Offset: 0x0006EFF8
	public void AttachTheFlag(Wgo flagWgo)
	{
		this.View.Banner.Show(true, flagWgo.Data.MainWgoPartData.variationId);
		this.attachedWgo = flagWgo;
		flagWgo.SetInteractableCollidersState(false);
		flagWgo.SetLayerToAllColliders(26);
		this.attachedWgo.MainWgoPart.ApplyWgoPartState("empty", 0);
		flagWgo.UpdateFlag(ChunkingIgnoreType.Fighting, true);
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x00070E5C File Offset: 0x0006F05C
	public Wgo RemoveTheFlag()
	{
		if (!this.attachedWgo)
		{
			this.View.Banner.Show(false, "");
			return null;
		}
		this.View.Banner.Show(false, "");
		Wgo wgo = this.attachedWgo;
		this.attachedWgo = null;
		wgo.Data.ApplyWgoPartState(this.View.Banner.GetCurVariationId(), 0);
		return wgo;
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x060017E8 RID: 6120 RVA: 0x00070ECD File Offset: 0x0006F0CD
	// (set) Token: 0x060017E9 RID: 6121 RVA: 0x00070EDF File Offset: 0x0006F0DF
	public Vector3 MovablePosition
	{
		get
		{
			return this.playerData.position.Value;
		}
		set
		{
			this.playerPhysicalBody.MoveByPosition(value, this.MovableDirection, true);
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x060017EA RID: 6122 RVA: 0x00070ECD File Offset: 0x0006F0CD
	// (set) Token: 0x060017EB RID: 6123 RVA: 0x00070EF4 File Offset: 0x0006F0F4
	public Vector3 MovablePositionWithoutDirectionChange
	{
		get
		{
			return this.playerData.position.Value;
		}
		set
		{
			this.playerPhysicalBody.MoveByPosition(value, this.MovableDirection, false);
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x060017EC RID: 6124 RVA: 0x00070F09 File Offset: 0x0006F109
	// (set) Token: 0x060017ED RID: 6125 RVA: 0x00070F16 File Offset: 0x0006F116
	public Vector2 MovableDirection
	{
		get
		{
			return this.playerData.Direction;
		}
		set
		{
			this.playerData.Direction = value;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x060017EE RID: 6126 RVA: 0x00070F24 File Offset: 0x0006F124
	public Direction Direction
	{
		get
		{
			return this.MovableDirection.ConvertFromVector2();
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x060017EF RID: 6127 RVA: 0x00070F31 File Offset: 0x0006F131
	public string MovableObjectId
	{
		get
		{
			return "Player";
		}
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x00070F38 File Offset: 0x0006F138
	public void OnPathStart()
	{
		this.playerPhysicalBody.SetNonKinematicFlag(PlayerDynamicType.ByMovementComponent, false);
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x00070F47 File Offset: 0x0006F147
	public void OnPathComplete(MovementComponent component)
	{
		this.playerPhysicalBody.SetNonKinematicFlag(PlayerDynamicType.ByMovementComponent, true);
		this.playerPhysicalBody.StopMoving();
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x060017F2 RID: 6130 RVA: 0x00070F61 File Offset: 0x0006F161
	public SGuid Id
	{
		get
		{
			return this.playerData.Guid;
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x060017F3 RID: 6131 RVA: 0x00070F6E File Offset: 0x0006F16E
	public IWorkActivity WorkerActivity
	{
		get
		{
			return this.curWorkActivity;
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x060017F4 RID: 6132 RVA: 0x00070F76 File Offset: 0x0006F176
	public MultiInventory WorkerMultiInventory
	{
		get
		{
			return new MultiInventory(this.playerData, true);
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x060017F5 RID: 6133 RVA: 0x00070F84 File Offset: 0x0006F184
	public Inventory WorkerInventory
	{
		get
		{
			return this.playerData.inventory;
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x060017F6 RID: 6134 RVA: 0x00070F91 File Offset: 0x0006F191
	public Inventory WorkerToolInventory
	{
		get
		{
			return this.playerData.toolBeltInventory;
		}
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x00070FA0 File Offset: 0x0006F1A0
	public CraftStatus CheckWorkerDependentValues(CraftElement craftElement, float deltaTime = 1f, bool skipEnergyCheck = false, bool skipInsanityCheck = false)
	{
		CraftDef definition = craftElement.Definition;
		if (definition.insanityLock.HasExpression && !PlayerInsanityGameResSystem.GetSystem().IsEnoughValue(definition.insanityLock.EvaluateFloat()))
		{
			return CraftStatus.NotEnoughInsanity;
		}
		if (!skipInsanityCheck && definition.insanityPerTick.HasExpression && !PlayerInsanityGameResSystem.GetSystem().CanChangeInsanity(definition.insanityPerTick.EvaluateFloat() * deltaTime))
		{
			return CraftStatus.NotEnoughInsanity;
		}
		if (!craftElement.ParamsData.HasRequiredTool)
		{
			return CraftStatus.DoesntHaveRequiredTool;
		}
		if (!skipEnergyCheck && definition.energyPerTick.HasExpression && !PlayerEnergyGameResSystem.GetSystem().IsEnoughValue(definition.energyPerTick.EvaluateFloat() * deltaTime))
		{
			return CraftStatus.NotEnoughEnergy;
		}
		if (!definition.isStarCraft && !definition.isAutopsyCraft && craftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.Common && craftElement.ParamsData.MasteryValue < craftElement.ParamsData.MasteryLock)
		{
			return CraftStatus.NotEnoughMastery;
		}
		if ((definition.isStarCraft || definition.isAutopsyCraft) && craftElement.ParamsData.MasteryValue <= 0)
		{
			return CraftStatus.NotEnoughMastery;
		}
		if (craftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.GardenPlanting && craftElement.ParamsData.MasteryValue <= 0)
		{
			return CraftStatus.NotEnoughMastery;
		}
		return CraftStatus.OK;
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x000710B9 File Offset: 0x0006F2B9
	public void AddRes(string type, float value)
	{
		this.playerData.AddRes(type, value);
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x000710C8 File Offset: 0x0006F2C8
	public void MultiplyRes(string type, float value)
	{
		this.playerData.MultiplyRes(type, value);
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x000710D7 File Offset: 0x0006F2D7
	public void SetRes(string stype, float value)
	{
		this.playerData.SetRes(stype, value);
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x000710E6 File Offset: 0x0006F2E6
	public float GetRes(string stype, float defaultValue = 0f)
	{
		return this.playerData.GetRes(stype, defaultValue);
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x000710F8 File Offset: 0x0006F2F8
	public int GetMasteryLevelForTalentBranch(string talentId, CraftDefBase craftDef = null)
	{
		int perksCraftMasteryBonusValue = this.GetPerksCraftMasteryBonusValue(craftDef);
		if (string.IsNullOrEmpty(talentId))
		{
			return 1 + perksCraftMasteryBonusValue;
		}
		int num = 0;
		foreach (Item item in this.WorkerToolInventory.Data.Inventory)
		{
			if (item.Definition.talentIds.Contains(talentId))
			{
				num += item.Definition.talentBonus;
			}
		}
		return MainGame.Instance.GameSave.talentSystemData.GetTalentBranch(talentId).curTalentValue + num + perksCraftMasteryBonusValue;
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x000711A4 File Offset: 0x0006F3A4
	public bool HasToolForWork(WgoData wgoData, CraftDefBase craftDef)
	{
		ItemType itemType = ItemType.None;
		CraftDef craftDef2 = craftDef as CraftDef;
		if (craftDef2 != null && craftDef2.customItemTypeAction != ItemType.None && !craftDef.isAuto)
		{
			itemType = craftDef2.customItemTypeAction;
		}
		if (itemType == ItemType.None)
		{
			itemType = wgoData.Definition.toolAction.actionableTool;
		}
		return itemType == ItemType.None || !this.WorkerToolInventory.Data.GetItemByType(itemType).IsEmpty;
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x00071208 File Offset: 0x0006F408
	public bool HasToolForWork(WgoData wgoData, out ItemType possibleTool)
	{
		possibleTool = ItemType.None;
		foreach (CraftDefBase craftDefBase in wgoData.CraftComponent.AvailableCrafts)
		{
			CraftDef craftDef = craftDefBase as CraftDef;
			if (craftDef != null && craftDef.customItemTypeAction != ItemType.None && !craftDefBase.isAuto)
			{
				possibleTool = craftDef.customItemTypeAction;
				break;
			}
		}
		if (possibleTool == ItemType.None)
		{
			possibleTool = wgoData.Definition.toolAction.actionableTool;
		}
		return possibleTool == ItemType.None || !this.WorkerToolInventory.Data.GetItemByType(possibleTool).IsEmpty;
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x000712B8 File Offset: 0x0006F4B8
	public int GetPerksCraftMasteryBonusValue(CraftDefBase craftDef)
	{
		int num = 0;
		if (craftDef == null)
		{
			return 0;
		}
		foreach (string text in craftDef.linkedPerks)
		{
			if (MainGame.Instance.GameSave.perkSystemData.HasPerk(text))
			{
				num += GameBalance.Me.GetData<PerkDef>(text).craftMasteryBonus;
			}
		}
		return num;
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x00071338 File Offset: 0x0006F538
	public int GetPerksCraftStartTicksBonusValue(CraftDefBase craftDef)
	{
		int num = 0;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = MainGame.Instance.GameSave.perkSystemData.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.craftStartTicks;
				}
			}
		}
		return num;
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x000713CC File Offset: 0x0006F5CC
	public int GetPerksCraftAddTotalProgressTicksValue(CraftDefBase craftDef)
	{
		int num = 0;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = MainGame.Instance.GameSave.perkSystemData.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.craftTotalProgressTicksBonus;
				}
			}
		}
		return num;
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x00071460 File Offset: 0x0006F660
	public float GetPerksEnergyBonusValue(CraftDefBase craftDef)
	{
		float num = 0f;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = MainGame.Instance.GameSave.perkSystemData.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.energyAdd;
				}
			}
		}
		return num;
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x000714F8 File Offset: 0x0006F6F8
	public float GetPerksInsanityBonusValue(CraftDefBase craftDef)
	{
		float num = 0f;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = MainGame.Instance.GameSave.perkSystemData.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.insanityAdd;
				}
			}
		}
		return num;
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x00071590 File Offset: 0x0006F790
	public Item GetToolForWorkOnCraft(WgoData wgoData, CraftDefBase craftDef)
	{
		ItemType itemType = ItemType.None;
		CraftDef craftDef2 = craftDef as CraftDef;
		if (craftDef2 != null && craftDef2.customItemTypeAction != ItemType.None && !craftDef.isAuto)
		{
			itemType = craftDef2.customItemTypeAction;
		}
		if (itemType == ItemType.None)
		{
			itemType = wgoData.Definition.toolAction.actionableTool;
		}
		foreach (Item item in this.WorkerToolInventory.Data.Inventory)
		{
			if (item.Definition.type == itemType)
			{
				return item;
			}
		}
		return Item.Empty;
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x00071638 File Offset: 0x0006F838
	public void SetHPActivity(WgoData wgoData)
	{
		if (this.curWorkActivity != null)
		{
			this.curWorkActivity.WgoData.ClearWorker();
		}
		this.curWorkActivity = new PlayerHPActivity(this.playerData, wgoData);
		wgoData.TrySetWorker(this, null);
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x0007166D File Offset: 0x0006F86D
	public void SetCraftActivity(WgoData wgoData)
	{
		if (this.curWorkActivity != null)
		{
			this.curWorkActivity.WgoData.ClearWorker();
		}
		this.curWorkActivity = new PlayerCraftActivity(this.playerData, wgoData);
		wgoData.TrySetWorker(this, null);
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x000716A2 File Offset: 0x0006F8A2
	public void ClearWorkActivity()
	{
		if (this.curWorkActivity == null)
		{
			return;
		}
		this.curWorkActivity.WgoData.ClearWorker();
		this.curWorkActivity = null;
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x0007172E File Offset: 0x0006F92E
	[CompilerGenerated]
	private Vector3 <TryTeleportPlayerToAnyFreePlaceCoroutine>g__RandPos|125_0()
	{
		return new Vector3(global::UnityEngine.Random.Range(-this.randPosRadiusForOverlapSearch, this.randPosRadiusForOverlapSearch), 0f, global::UnityEngine.Random.Range(-this.randPosRadiusForOverlapSearch, this.randPosRadiusForOverlapSearch)) * this.positionRandomizationForOverlapSearch;
	}

	// Token: 0x0400176E RID: 5998
	private const int OVERLAP_COLLIDERS_MAX_COUNT = 50;

	// Token: 0x0400176F RID: 5999
	private const string PLAYER_HP_CONSTS_ID = "player_hp";

	// Token: 0x04001770 RID: 6000
	public Action OnWeaponEquiped;

	// Token: 0x04001775 RID: 6005
	private static Collider[] overlapColliders = new Collider[50];

	// Token: 0x04001776 RID: 6006
	[SerializeField]
	private PlayerInteractionComponent playerInteractionComponent;

	// Token: 0x04001777 RID: 6007
	[SerializeField]
	private PlayerPhysicalBody playerPhysicalBody;

	// Token: 0x04001778 RID: 6008
	[SerializeField]
	private PlayerView playerView;

	// Token: 0x04001779 RID: 6009
	[SerializeField]
	private PlayerLocalAreaMovement playerLocalAreaMovement;

	// Token: 0x0400177A RID: 6010
	[SerializeField]
	private PlayerColorCustomizationData characterCustomizationData;

	// Token: 0x0400177B RID: 6011
	[SerializeField]
	private LadderClimbController ladderClimbController;

	// Token: 0x0400177C RID: 6012
	[SerializeField]
	private float randPosRadiusForOverlapSearch = 1.4f;

	// Token: 0x0400177D RID: 6013
	[SerializeField]
	private float positionRandomizationForOverlapSearch = 0.3f;

	// Token: 0x0400177E RID: 6014
	[SerializeField]
	private float overlapRadiusForSearch = 0.22f;

	// Token: 0x0400177F RID: 6015
	private MultiFlagAND<TakenControlType> playerTakenControlMultiFlag = new MultiFlagAND<TakenControlType>();

	// Token: 0x04001780 RID: 6016
	private MultiFlagAND<DisabledStateType> playerDisabledStateMultiFlag = new MultiFlagAND<DisabledStateType>();

	// Token: 0x04001781 RID: 6017
	private PlayerData playerData;

	// Token: 0x04001782 RID: 6018
	private GameScene currentGameScene;

	// Token: 0x04001783 RID: 6019
	private ToolComponent toolComponent;

	// Token: 0x04001784 RID: 6020
	private PlayerWorkComponent playerWorkComponent;

	// Token: 0x04001785 RID: 6021
	private PlayerFishingComponent fishingComponent;

	// Token: 0x04001786 RID: 6022
	private AttackComponent attackComponent;

	// Token: 0x04001787 RID: 6023
	private MovementComponent movementComponent = new MovementComponent();

	// Token: 0x04001788 RID: 6024
	private PlayerInputHandler playerInputHandler;

	// Token: 0x04001789 RID: 6025
	public Wgo attachedWgo;

	// Token: 0x0400178A RID: 6026
	private SSM ssm;

	// Token: 0x0400178B RID: 6027
	private bool isInitialized;

	// Token: 0x0400178C RID: 6028
	private static Array controlsEnumArray;

	// Token: 0x0400178D RID: 6029
	private static Array activeEnumArray;

	// Token: 0x0400178E RID: 6030
	private bool isOverheadActive;

	// Token: 0x0400178F RID: 6031
	private bool isArmorViewActive;

	// Token: 0x04001790 RID: 6032
	private bool armorViewUsesHelmet = true;

	// Token: 0x04001792 RID: 6034
	private PlayerActivity curWorkActivity;
}
