using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002B5 RID: 693
public class DamageEffectComponent : MonoBehaviour
{
	// Token: 0x170002DF RID: 735
	// (get) Token: 0x060011B0 RID: 4528 RVA: 0x00058CEE File Offset: 0x00056EEE
	public DamageEffectSettings Settings
	{
		get
		{
			return this.settings;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x060011B1 RID: 4529 RVA: 0x00058CF6 File Offset: 0x00056EF6
	// (set) Token: 0x060011B2 RID: 4530 RVA: 0x00058CFE File Offset: 0x00056EFE
	private float EvaluationTime { get; set; }

	// Token: 0x060011B3 RID: 4531 RVA: 0x00058D08 File Offset: 0x00056F08
	public static void TryPlayEffect(SGuid guid, Vector3 position, Vector3 attackDirection, DamageEffectSettings effectOverride = null)
	{
		DamageEffectComponent damageEffectComponent;
		if (!DamageEffectComponent.cache.TryGetValue(guid, out damageEffectComponent))
		{
			return;
		}
		DamageEffectSettings damageEffectSettings = effectOverride ?? damageEffectComponent.settings;
		if (!damageEffectSettings)
		{
			return;
		}
		if (!string.IsNullOrEmpty(damageEffectSettings.fxName))
		{
			WorldFX.Spawn(position, damageEffectSettings.fxName, null, default(Vector3));
		}
		if (damageEffectSettings.spawnBloodPaddle && global::UnityEngine.Random.value < LazySingletonSO<GlobalResources>.Instance.fighting.bloodDecalSpawnProbability)
		{
			float num = global::UnityEngine.Random.Range(-15f, 15f);
			Quaternion quaternion = Quaternion.Euler(0f, num, 0f);
			Vector3 vector = position + (quaternion * attackDirection).normalized * global::UnityEngine.Random.Range(0.8f, 1.2f);
			LazySingleton<FightingGameController>.Instance.FightEffectsManager.bloodDecalsCollection.SpawnDecal(vector, Direction.None, "");
		}
		damageEffectComponent.PlayColorBlink(damageEffectSettings);
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x00058DF0 File Offset: 0x00056FF0
	public static void TryPlayEffect(ICombatEntity target, AttackContext context)
	{
		DamageEffectComponent.TryPlayEffect(target.CombatEntityUID, context.hitPosition, context.direction, context.damageEffectOverride);
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x00058E10 File Offset: 0x00057010
	public void Init(ICombatEntity entity = null)
	{
		ICombatEntity combatEntity = entity ?? base.GetComponentInParent<ICombatEntity>();
		if (combatEntity == null)
		{
			Debug.LogError("DamageEffectComponent: combatEntity is null");
			return;
		}
		DamageEffectComponent.cache[combatEntity.CombatEntityUID] = this;
		this.attachedSguid = combatEntity.CombatEntityUID;
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x00058E54 File Offset: 0x00057054
	private void Start()
	{
		if (this.initManually)
		{
			return;
		}
		this.Init(null);
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00058E68 File Offset: 0x00057068
	private void OnDestroy()
	{
		if (SGuid.IsNullOrEmpty(this.attachedSguid))
		{
			return;
		}
		DamageEffectComponent damageEffectComponent;
		if (DamageEffectComponent.cache.TryGetValue(this.attachedSguid, out damageEffectComponent) && damageEffectComponent == this)
		{
			DamageEffectComponent.cache.Remove(this.attachedSguid);
		}
		this.attachedSguid = null;
		if (this.damageTween != null)
		{
			this.damageTween.Kill(true);
			this.damageTween = null;
		}
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x00058ED4 File Offset: 0x000570D4
	private void PlayColorBlink(DamageEffectSettings blinkSettings)
	{
		if (!blinkSettings)
		{
			return;
		}
		int colorProperty = (blinkSettings.useAdditiveTintColor ? DamageEffectComponent.AdditiveTintColor : DamageEffectComponent.TintColor);
		if (this.damageTween == null)
		{
			this.EvaluationTime = 0f;
			if (this.matPropertyBlock == null)
			{
				this.matPropertyBlock = new MaterialPropertyBlock();
			}
			this.damageTween = DOTween.To(() => this.EvaluationTime, delegate(float t)
			{
				this.EvaluationTime = t;
				Color color = blinkSettings.colorGradient.Evaluate(t);
				foreach (GenericSprite genericSprite in this.sprites)
				{
					if ((genericSprite != null) ? genericSprite.SpriteRenderer : null)
					{
						genericSprite.SpriteRenderer.GetPropertyBlock(this.matPropertyBlock);
						this.matPropertyBlock.SetColor(colorProperty, color);
						genericSprite.SpriteRenderer.SetPropertyBlock(this.matPropertyBlock);
					}
				}
			}, 1f, blinkSettings.blinkDuration).SetEase(blinkSettings.blinkEase).OnComplete(delegate
			{
				this.damageTween = null;
			});
			return;
		}
		this.damageTween.Restart(true, -1f);
		this.EvaluationTime = 0.4f;
		this.damageTween.Goto(this.EvaluationTime * blinkSettings.blinkDuration, true);
	}

	// Token: 0x04001391 RID: 5009
	private static Dictionary<SGuid, DamageEffectComponent> cache = new Dictionary<SGuid, DamageEffectComponent>();

	// Token: 0x04001392 RID: 5010
	private static readonly int TintColor = Shader.PropertyToID("_TintColor");

	// Token: 0x04001393 RID: 5011
	private static readonly int AdditiveTintColor = Shader.PropertyToID("_AdditiveTintColor");

	// Token: 0x04001394 RID: 5012
	[SerializeField]
	private DamageEffectSettings settings;

	// Token: 0x04001395 RID: 5013
	[SerializeField]
	private List<GenericSprite> sprites = new List<GenericSprite>();

	// Token: 0x04001396 RID: 5014
	[SerializeField]
	private bool initManually;

	// Token: 0x04001397 RID: 5015
	private SGuid attachedSguid;

	// Token: 0x04001398 RID: 5016
	private Tween damageTween;

	// Token: 0x04001399 RID: 5017
	private MaterialPropertyBlock matPropertyBlock;
}
