using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;

// Token: 0x02000677 RID: 1655
public class PlayerView : MonoBehaviour, IBubbleDrawable
{
	// Token: 0x1400009A RID: 154
	// (add) Token: 0x06002BB1 RID: 11185 RVA: 0x000CF3A8 File Offset: 0x000CD5A8
	// (remove) Token: 0x06002BB2 RID: 11186 RVA: 0x000CF3E0 File Offset: 0x000CD5E0
	public event Action OnPlantingAnimationEvent;

	// Token: 0x1400009B RID: 155
	// (add) Token: 0x06002BB3 RID: 11187 RVA: 0x000CF418 File Offset: 0x000CD618
	// (remove) Token: 0x06002BB4 RID: 11188 RVA: 0x000CF450 File Offset: 0x000CD650
	public event Action<ToolComponent> OnWorkActionAnimationEvent;

	// Token: 0x170006C8 RID: 1736
	// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x000CF485 File Offset: 0x000CD685
	// (set) Token: 0x06002BB6 RID: 11190 RVA: 0x000CF48D File Offset: 0x000CD68D
	public bool ControllingWispView
	{
		get
		{
			return this.controllingWispView;
		}
		set
		{
			this.controllingWispView = value;
		}
	}

	// Token: 0x170006C9 RID: 1737
	// (get) Token: 0x06002BB7 RID: 11191 RVA: 0x000CF496 File Offset: 0x000CD696
	public WispController WispController
	{
		get
		{
			return this.playerWisp;
		}
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06002BB8 RID: 11192 RVA: 0x000CF49E File Offset: 0x000CD69E
	public PlayerAnimation PlayerAnimation
	{
		get
		{
			return this.playerAnimation;
		}
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06002BB9 RID: 11193 RVA: 0x000CF4A6 File Offset: 0x000CD6A6
	public PlayerAnimation CustomizationCharacter
	{
		get
		{
			return this.customizationCharacter;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06002BBA RID: 11194 RVA: 0x000CF4AE File Offset: 0x000CD6AE
	public Transform BubblePoint
	{
		get
		{
			return this.bubblePoint;
		}
	}

	// Token: 0x170006CD RID: 1741
	// (get) Token: 0x06002BBB RID: 11195 RVA: 0x000CF4B6 File Offset: 0x000CD6B6
	public BannerView Banner
	{
		get
		{
			return this.banner;
		}
	}

	// Token: 0x170006CE RID: 1742
	// (get) Token: 0x06002BBC RID: 11196 RVA: 0x000CF4BE File Offset: 0x000CD6BE
	public FishingContainer FishingContainer
	{
		get
		{
			return this.fishingContainer;
		}
	}

	// Token: 0x170006CF RID: 1743
	// (get) Token: 0x06002BBD RID: 11197 RVA: 0x000CF4C6 File Offset: 0x000CD6C6
	public Transform LarryInPocketBubblePoint
	{
		get
		{
			return this.larryInPocketBubblePoint;
		}
	}

	// Token: 0x170006D0 RID: 1744
	// (get) Token: 0x06002BBE RID: 11198 RVA: 0x000CF4CE File Offset: 0x000CD6CE
	private PlayerController PlayerController
	{
		get
		{
			return MainGame.PlayerController;
		}
	}

	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06002BBF RID: 11199 RVA: 0x000CF4D5 File Offset: 0x000CD6D5
	public Vector3 RoundedPosition
	{
		get
		{
			return VisualConsts.GetRoundedPosXYZ(base.transform.parent.position, this.pixelSizeForPosRounding, 1);
		}
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x000CF4F4 File Offset: 0x000CD6F4
	public void Init()
	{
		if (this.isInitialized)
		{
			return;
		}
		this.isInitialized = true;
		if (!base.TryGetComponent<PlayerAnimation>(out this.playerAnimation))
		{
			Debug.LogError("Player Animation Component doesn't found");
		}
		this.InitPlayerAnimation();
		this.OnWorkActionAnimationEvent += this.PlayerAnimation.OnUseToolAnimation;
		this.OnPlantingAnimationEvent += this.PlayerAnimation.OnPlantingAnimation;
		if (this.cylinderShadowcaster)
		{
			this.cylinderShadowCasterScale = this.cylinderShadowcaster.localScale;
		}
		this.HandleMainCameraRenderTypeChanged(CameraSystem.Instance.MainCamera.GetRenderType());
		MainCamera.OnRenderModeChanged += this.HandleMainCameraRenderTypeChanged;
		this.InitPlatformDependentElements();
		LazySingleton<global::Microphone>.Instance.SetTarget(base.transform);
	}

	// Token: 0x06002BC1 RID: 11201 RVA: 0x000CF5B7 File Offset: 0x000CD7B7
	public void PrepareForGame(PlayerData playerData)
	{
		this.movementAdjustComponent.Init(this.PlayerController);
		LazySingleton<FightingGameController>.Instance.OnFightStateChanged += this.HandleFightStateChanged;
		this.RefreshFightBubbleWidgets();
	}

	// Token: 0x06002BC2 RID: 11202 RVA: 0x000CF5E6 File Offset: 0x000CD7E6
	public void UnPrepareFromGame()
	{
		this.movementAdjustComponent.DeInit();
		LazySingleton<FightingGameController>.Instance.OnFightStateChanged -= this.HandleFightStateChanged;
		this.PlayerAnimation.TryPlayResetClip();
	}

	// Token: 0x06002BC3 RID: 11203 RVA: 0x000CF614 File Offset: 0x000CD814
	public void UpdatePosition(Vector3 position)
	{
		base.transform.position = position;
	}

	// Token: 0x06002BC4 RID: 11204 RVA: 0x000CF622 File Offset: 0x000CD822
	public void UpdateAnimationDirection(Vector2 direction)
	{
		this.cachedDirectionAngle = this.PlayerAnimation.SetDirection(direction);
		this.UpdateWispDirection(this.cachedDirectionAngle);
	}

	// Token: 0x06002BC5 RID: 11205 RVA: 0x000CF642 File Offset: 0x000CD842
	public void UpdateWispDirection()
	{
		this.UpdateWispDirection(this.cachedDirectionAngle);
	}

	// Token: 0x06002BC6 RID: 11206 RVA: 0x000CF650 File Offset: 0x000CD850
	private void UpdateWispDirection(float angle)
	{
		if (this.controllingWispView)
		{
			switch (angle.ConvertFromSignedAngle())
			{
			case Direction.None:
				this.playerWisp.SetTargetTransform(this.wispTargetDownDirection);
				return;
			case Direction.Right:
				this.playerWisp.SetTargetTransform(this.wispTargetRightDirection);
				return;
			case Direction.Up:
				this.playerWisp.SetTargetTransform(this.wispTargetUpDirection);
				return;
			case Direction.Left:
				this.playerWisp.SetTargetTransform(this.wispTargetLeftDirection);
				return;
			case Direction.Down:
				this.playerWisp.SetTargetTransform(this.wispTargetDownDirection);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06002BC7 RID: 11207 RVA: 0x000CF6E0 File Offset: 0x000CD8E0
	public Vector2 GetAnimationDirection()
	{
		return this.PlayerAnimation.GetDirection();
	}

	// Token: 0x06002BC8 RID: 11208 RVA: 0x000CF6ED File Offset: 0x000CD8ED
	public void UpdateHpBarState(HPComponent hpComponent)
	{
		this.RefreshFightBubbleWidgets();
	}

	// Token: 0x06002BC9 RID: 11209 RVA: 0x000CF6ED File Offset: 0x000CD8ED
	public void UpdateStaminaBarState()
	{
		this.RefreshFightBubbleWidgets();
	}

	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06002BCA RID: 11210 RVA: 0x000CF6F5 File Offset: 0x000CD8F5
	private Object3DTransparencyOccluder transparencyOccluder
	{
		get
		{
			return Object3DTransparencyOccluder.Shared;
		}
	}

	// Token: 0x06002BCB RID: 11211 RVA: 0x000CF6FC File Offset: 0x000CD8FC
	public void Update()
	{
		if (BuildController.Instance != null && BuildController.Instance.IsBuildModeActive)
		{
			return;
		}
		this.transparencyOccluder.BeginFrame();
		this.transparencyOccluder.AddOccludersFromCapsule(base.transform.position, base.transform.position + Vector3.up * 1.4f, 0.24f, 10f);
		this.transparencyOccluder.EndFrame();
	}

	// Token: 0x06002BCC RID: 11212 RVA: 0x000CF778 File Offset: 0x000CD978
	private void LateUpdate()
	{
		Vector3 roundedPosition = this.RoundedPosition;
		base.transform.position = roundedPosition;
	}

	// Token: 0x06002BCD RID: 11213 RVA: 0x000CF798 File Offset: 0x000CD998
	private void OnDestroy()
	{
		this.OnWorkActionAnimationEvent -= this.PlayerAnimation.OnUseToolAnimation;
		this.OnPlantingAnimationEvent -= this.PlayerAnimation.OnPlantingAnimation;
		MainCamera.OnRenderModeChanged -= this.HandleMainCameraRenderTypeChanged;
		this.transparencyOccluder.Clear();
	}

	// Token: 0x06002BCE RID: 11214 RVA: 0x000CF7EF File Offset: 0x000CD9EF
	public void SendWorkActionAnimationEvent()
	{
		Action<ToolComponent> onWorkActionAnimationEvent = this.OnWorkActionAnimationEvent;
		if (onWorkActionAnimationEvent != null)
		{
			onWorkActionAnimationEvent(this.PlayerController.PlayerWorkComponent.ToolComponent);
		}
		this.PlayerController.PlayerWorkComponent.ToolComponent.OnUseToolActionAnimationEvent();
	}

	// Token: 0x06002BCF RID: 11215 RVA: 0x000CF827 File Offset: 0x000CDA27
	public void SendPlantingActionAnimationEvent()
	{
		Action onPlantingAnimationEvent = this.OnPlantingAnimationEvent;
		if (onPlantingAnimationEvent == null)
		{
			return;
		}
		onPlantingAnimationEvent();
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x000CF839 File Offset: 0x000CDA39
	public void SetPlayerPreset(SkinPresetGK2 skinPreset, bool onlyForCustomizationCharacter)
	{
		if (!onlyForCustomizationCharacter)
		{
			this.playerAnimation.ChangeSkinPreset(skinPreset);
		}
		this.customizationCharacter.ChangeSkinPreset(skinPreset);
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x000CF856 File Offset: 0x000CDA56
	public void ApplyPlayerColors(Texture2D palette, List<CustomizablePartType> affectedPartTypes, bool onlyForCustomizationCharacter)
	{
		if (!onlyForCustomizationCharacter)
		{
			this.playerAnimation.ApplyPlayerColors(palette, affectedPartTypes, null);
		}
		this.customizationCharacter.ApplyPlayerColors(palette, affectedPartTypes, null);
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x000CF877 File Offset: 0x000CDA77
	private void InitPlayerAnimation()
	{
		this.playerAnimation.Init(PlayerSkinHelper.CurrentPreset);
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x000CF88C File Offset: 0x000CDA8C
	public void SetInteractingItem(Item item, int totalCount)
	{
		if (this.interactingItem != null)
		{
			this.interactingItem.DisableBubble();
		}
		if (item != null)
		{
			this.interactingItem = UIInteractingItem.ShowInteractingItem(new Item(item.id, totalCount), this.interactionItemPoint);
			this.playerAnimation.DisableDropView();
		}
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x000CF8DD File Offset: 0x000CDADD
	public void RemoveInteractingItem()
	{
		this.playerAnimation.EnableDropView();
		if (this.interactingItem != null)
		{
			this.interactingItem.DisableBubble();
		}
	}

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x000CF903 File Offset: 0x000CDB03
	public SGuid BubbleDrawableUniqueId
	{
		get
		{
			return MainGame.PlayerController.PlayerData.Guid;
		}
	}

	// Token: 0x170006D4 RID: 1748
	// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x000CF914 File Offset: 0x000CDB14
	public List<LazyWidgetDataBase> BubbleDrawableWidgets
	{
		get
		{
			List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
			bool flag = LazySingleton<FightingGameController>.Instance.CurrentFightState > FightState.Disabled;
			if (!this.PlayerController.PlayerData.hpComponent.HasFullHp || flag)
			{
				list.Add(new HpBarPlayerWidgetData(MainGame.PlayerController.PlayerData.hpComponent, -1f, -1f));
			}
			if (flag)
			{
				list.Add(new StaminaBarPlayerWidgetData());
			}
			return list;
		}
	}

	// Token: 0x170006D5 RID: 1749
	// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x000CF984 File Offset: 0x000CDB84
	public Vector3 BubbleDrawablePosition
	{
		get
		{
			return this.bubblePoint.position;
		}
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x000CF991 File Offset: 0x000CDB91
	private void HandleMainCameraRenderTypeChanged(MainCamera.RenderMode renderType)
	{
		this.pixelSizeForPosRounding = ((renderType == MainCamera.RenderMode.Native) ? 1 : 2);
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x000CF9A0 File Offset: 0x000CDBA0
	public void DisplayFishingRope()
	{
		this.fishingContainer.ResetColor();
		this.fishingContainer.gameObject.SetActive(true);
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000CF9BE File Offset: 0x000CDBBE
	public void SetSermonContainerActive(bool isActive)
	{
		this.sermonContainer.gameObject.SetActive(isActive);
		LazyAudio.PlayAndForget(MainGame.PlayerData.currentSermon.success ? "sermon_success" : "sermon_fail");
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x000CF9F4 File Offset: 0x000CDBF4
	public void SetSermonIcon(Sprite sprite)
	{
		this.sermonContainer.GetComponentsInChildren<SpriteRenderer>(true).ToList<SpriteRenderer>().ForEach(delegate(SpriteRenderer s)
		{
			s.sprite = sprite;
		});
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x000CFA30 File Offset: 0x000CDC30
	public void FinishSermon(bool isSuccess, Action onAnimationFinished = null)
	{
		Animator component = this.sermonContainer.GetComponent<Animator>();
		if (component == null)
		{
			Debug.LogWarning("[PlayerView.FinishSermon]: sermonAnimator is null");
			return;
		}
		component.SetTrigger(isSuccess ? "success" : "fail");
		base.StartCoroutine(this.FinishSermonEnumerator(component, onAnimationFinished));
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x000CFA81 File Offset: 0x000CDC81
	private IEnumerator FinishSermonEnumerator(Animator animator, Action onAnimationFinished = null)
	{
		yield return null;
		yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
		this.sermonContainer.gameObject.SetActive(false);
		if (onAnimationFinished != null)
		{
			onAnimationFinished();
		}
		yield break;
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x000CF6ED File Offset: 0x000CD8ED
	private void HandleFightStateChanged(FightState fightState)
	{
		this.RefreshFightBubbleWidgets();
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x000CFA9E File Offset: 0x000CDC9E
	private void RefreshFightBubbleWidgets()
	{
		UIObjectBubbleManager.Instance.Display(this);
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x000CFAAC File Offset: 0x000CDCAC
	private void InitPlatformDependentElements()
	{
		foreach (LazyPlatformDependentElement lazyPlatformDependentElement in base.GetComponentsInChildren<LazyPlatformDependentElement>(true))
		{
			if (!lazyPlatformDependentElement.UseAwakeForInit)
			{
				lazyPlatformDependentElement.Init();
			}
		}
	}

	// Token: 0x0400236B RID: 9067
	[SerializeField]
	private Transform bubblePoint;

	// Token: 0x0400236C RID: 9068
	[SerializeField]
	private Transform larryInPocketBubblePoint;

	// Token: 0x0400236D RID: 9069
	[SerializeField]
	private Transform interactionItemPoint;

	// Token: 0x0400236E RID: 9070
	[Space]
	[Header("Banner")]
	[SerializeField]
	private BannerView banner;

	// Token: 0x0400236F RID: 9071
	[Space]
	[Header("Wisp")]
	[SerializeField]
	private WispController playerWisp;

	// Token: 0x04002370 RID: 9072
	[SerializeField]
	private Transform wispTargetRightDirection;

	// Token: 0x04002371 RID: 9073
	[SerializeField]
	private Transform wispTargetLeftDirection;

	// Token: 0x04002372 RID: 9074
	[SerializeField]
	private Transform wispTargetUpDirection;

	// Token: 0x04002373 RID: 9075
	[SerializeField]
	private Transform wispTargetDownDirection;

	// Token: 0x04002374 RID: 9076
	[Space]
	[Header("Fishing line")]
	[SerializeField]
	private FishingContainer fishingContainer;

	// Token: 0x04002375 RID: 9077
	[Space]
	[Header("Sermon")]
	[SerializeField]
	private Transform sermonContainer;

	// Token: 0x04002376 RID: 9078
	[Space]
	[SerializeField]
	public Transform cylinderShadowcaster;

	// Token: 0x04002377 RID: 9079
	[SerializeField]
	private PlayerAnimation customizationCharacter;

	// Token: 0x04002378 RID: 9080
	[NonSerialized]
	public Vector3 cylinderShadowCasterScale;

	// Token: 0x04002379 RID: 9081
	[SerializeField]
	private PlayerMovementAdjustComponent movementAdjustComponent;

	// Token: 0x0400237A RID: 9082
	private int pixelSizeForPosRounding = 1;

	// Token: 0x0400237B RID: 9083
	private UIInteractingItem interactingItem;

	// Token: 0x0400237C RID: 9084
	private PlayerAnimation playerAnimation;

	// Token: 0x0400237D RID: 9085
	private float cachedDirectionAngle;

	// Token: 0x0400237E RID: 9086
	[SerializeField]
	private bool controllingWispView = true;

	// Token: 0x0400237F RID: 9087
	private bool isInitialized;
}
