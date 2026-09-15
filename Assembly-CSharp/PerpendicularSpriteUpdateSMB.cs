using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200077C RID: 1916
public class PerpendicularSpriteUpdateSMB : StateMachineBehaviour
{
	// Token: 0x060031A7 RID: 12711 RVA: 0x000EE0FC File Offset: 0x000EC2FC
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.syncedSprites == null)
		{
			this.syncedSprites = new List<SyncedSprite>();
		}
		animator.GetComponentsInChildren<SyncedSprite>(this.syncedSprites);
		if (!animator.TryGetComponent<AnimationComponent>(out this.animationComponent))
		{
			this.animationComponent = animator.GetComponentInParent<AnimationComponent>();
		}
		if (this.animationComponent != null)
		{
			this.animationComponent.OnLateUpdate -= this.CustomLateUpdate;
			this.animationComponent.OnLateUpdate += this.CustomLateUpdate;
		}
		this.lastDirection = Direction.None;
		this.newDirectionName = string.Empty;
		this.spriteCache.Clear();
		this.resolveCache.Clear();
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x000EE1A6 File Offset: 0x000EC3A6
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.animationComponent != null)
		{
			this.animationComponent.OnLateUpdate -= this.CustomLateUpdate;
		}
		this.spriteCache.Clear();
		this.resolveCache.Clear();
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x000EE1E4 File Offset: 0x000EC3E4
	public void CustomLateUpdate()
	{
		this.UpdateCachedDirection();
		if (string.IsNullOrEmpty(this.newDirectionName))
		{
			return;
		}
		foreach (SyncedSprite syncedSprite in this.syncedSprites)
		{
			if (!(syncedSprite == null) && !(syncedSprite.SpriteRenderer == null))
			{
				CachedSpriteRenderer targetSprite = syncedSprite.targetSprite;
				global::UnityEngine.Object @object;
				if (targetSprite == null)
				{
					@object = null;
				}
				else
				{
					SpriteRenderer spriteRenderer = targetSprite.SpriteRenderer;
					@object = ((spriteRenderer != null) ? spriteRenderer.sprite : null);
				}
				if (!(@object == null) && syncedSprite.targetSprite.isActiveAndEnabled && !syncedSprite.ignoreMe)
				{
					if (!syncedSprite.targetSprite.SpriteRenderer.enabled || !syncedSprite.gameObject.activeInHierarchy)
					{
						syncedSprite.SpriteRenderer.enabled = false;
					}
					else
					{
						Sprite sprite = syncedSprite.targetSprite.SpriteRenderer.sprite;
						ValueTuple<int, Direction> valueTuple = new ValueTuple<int, Direction>(sprite.GetInstanceID(), this.lastDirection);
						PerpendicularSpriteUpdateSMB.ResolvedSprite resolvedSprite;
						if (!this.resolveCache.TryGetValue(valueTuple, out resolvedSprite))
						{
							resolvedSprite = this.Resolve(sprite.name);
							this.resolveCache[valueTuple] = resolvedSprite;
						}
						if (resolvedSprite.isValid)
						{
							SpriteRenderer spriteRenderer2 = syncedSprite.SpriteRenderer;
							if (spriteRenderer2.flipX != resolvedSprite.flipX)
							{
								spriteRenderer2.flipX = resolvedSprite.flipX;
							}
							if (resolvedSprite.sprite != null && spriteRenderer2.sprite != resolvedSprite.sprite)
							{
								spriteRenderer2.sprite = resolvedSprite.sprite;
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x000EE398 File Offset: 0x000EC598
	private PerpendicularSpriteUpdateSMB.ResolvedSprite Resolve(string sourceSpriteName)
	{
		string spriteName = this.GetSpriteName(sourceSpriteName);
		string[] array;
		if (!this.TryParseSpriteName(spriteName, out array) || array.Length < 2)
		{
			return PerpendicularSpriteUpdateSMB.ResolvedSprite.Skip;
		}
		int directionIndex = this.GetDirectionIndex(spriteName, array);
		string text = array[directionIndex];
		if (text == this.newDirectionName)
		{
			return PerpendicularSpriteUpdateSMB.ResolvedSprite.Skip;
		}
		bool flag = text == "up";
		array[directionIndex] = this.newDirectionName;
		string text2 = string.Join("_", array);
		if (spriteName == text2)
		{
			return PerpendicularSpriteUpdateSMB.ResolvedSprite.FlipOnly(flag);
		}
		return new PerpendicularSpriteUpdateSMB.ResolvedSprite(true, flag, this.GetCachedSprite(text2));
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x000EE42A File Offset: 0x000EC62A
	private int GetDirectionIndex(string spriteName, string[] spriteNameParts)
	{
		if (!spriteName.Contains("static"))
		{
			return spriteNameParts.Length - 2;
		}
		if (PerpendicularSpriteUpdateSMB.IsNumericToken(spriteNameParts[spriteNameParts.Length - 1]) && spriteNameParts.Length >= 2)
		{
			return spriteNameParts.Length - 2;
		}
		return spriteNameParts.Length - 1;
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x000EE460 File Offset: 0x000EC660
	private static bool IsNumericToken(string token)
	{
		if (string.IsNullOrEmpty(token))
		{
			return false;
		}
		for (int i = 0; i < token.Length; i++)
		{
			if (token[i] < '0' || token[i] > '9')
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x000EE4A4 File Offset: 0x000EC6A4
	private void UpdateCachedDirection()
	{
		Direction direction = this.animationComponent.Animator.GetFloat(AnimationComponentBase.idDirectionAnimator).ConvertFromSignedAngle();
		if (direction != this.lastDirection)
		{
			this.lastDirection = direction;
			this.newDirectionName = this.GetDirectionNameForEnumDirection(direction);
		}
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x000EE4EC File Offset: 0x000EC6EC
	private string GetDirectionNameForEnumDirection(Direction direction)
	{
		string text;
		switch (direction)
		{
		case Direction.Right:
		case Direction.Left:
			text = "down";
			break;
		case Direction.Up:
		case Direction.Down:
			text = "left";
			break;
		default:
			text = string.Empty;
			break;
		}
		return text;
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x000EE52C File Offset: 0x000EC72C
	private string GetSpriteName(string originalName)
	{
		bool flag = originalName.IndexOf("(Clone)", StringComparison.Ordinal) >= 0;
		bool flag2 = originalName.Length > 0 && (char.IsWhiteSpace(originalName[0]) || char.IsWhiteSpace(originalName[originalName.Length - 1]));
		if (!flag && !flag2)
		{
			return originalName;
		}
		return originalName.Replace("(Clone)", "").Trim();
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x000EE598 File Offset: 0x000EC798
	private Sprite GetCachedSprite(string spriteName)
	{
		Sprite sprite;
		if (this.spriteCache.TryGetValue(spriteName, out sprite))
		{
			return sprite;
		}
		Sprite sprite2 = null;
		if (LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(spriteName))
		{
			sprite2 = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(spriteName, null);
		}
		else if (spriteName.Contains("static"))
		{
			string[] array = spriteName.Split('_', StringSplitOptions.None);
			if (array.Length >= 2 && PerpendicularSpriteUpdateSMB.IsNumericToken(array[array.Length - 1]))
			{
				string text = string.Join("_", array, 0, array.Length - 1);
				if (LazySingletonSO<EasySpritesCollection>.Instance.HasSprite(text))
				{
					sprite2 = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(text, null);
				}
			}
		}
		this.spriteCache[spriteName] = sprite2;
		return sprite2;
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x000EE63A File Offset: 0x000EC83A
	private bool TryParseSpriteName(string spriteName, out string[] parts)
	{
		parts = spriteName.Split('_', StringSplitOptions.None);
		return parts.Length >= 2;
	}

	// Token: 0x040027B0 RID: 10160
	private const string STATIC_SUFFIX = "static";

	// Token: 0x040027B1 RID: 10161
	private const string DOWN = "down";

	// Token: 0x040027B2 RID: 10162
	private const string LEFT = "left";

	// Token: 0x040027B3 RID: 10163
	private const string UP = "up";

	// Token: 0x040027B4 RID: 10164
	[SerializeField]
	private List<SyncedSprite> syncedSprites;

	// Token: 0x040027B5 RID: 10165
	private string newDirectionName = string.Empty;

	// Token: 0x040027B6 RID: 10166
	private Direction lastDirection;

	// Token: 0x040027B7 RID: 10167
	private AnimationComponent animationComponent;

	// Token: 0x040027B8 RID: 10168
	private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>(64);

	// Token: 0x040027B9 RID: 10169
	private Dictionary<ValueTuple<int, Direction>, PerpendicularSpriteUpdateSMB.ResolvedSprite> resolveCache = new Dictionary<ValueTuple<int, Direction>, PerpendicularSpriteUpdateSMB.ResolvedSprite>(128);

	// Token: 0x0200077D RID: 1917
	private readonly struct ResolvedSprite
	{
		// Token: 0x060031B3 RID: 12723 RVA: 0x000EE681 File Offset: 0x000EC881
		public ResolvedSprite(bool isValid, bool flipX, Sprite sprite)
		{
			this.isValid = isValid;
			this.flipX = flipX;
			this.sprite = sprite;
		}

		// Token: 0x060031B4 RID: 12724 RVA: 0x000EE698 File Offset: 0x000EC898
		public static PerpendicularSpriteUpdateSMB.ResolvedSprite FlipOnly(bool flipX)
		{
			return new PerpendicularSpriteUpdateSMB.ResolvedSprite(true, flipX, null);
		}

		// Token: 0x040027BA RID: 10170
		public readonly bool isValid;

		// Token: 0x040027BB RID: 10171
		public readonly bool flipX;

		// Token: 0x040027BC RID: 10172
		public readonly Sprite sprite;

		// Token: 0x040027BD RID: 10173
		public static readonly PerpendicularSpriteUpdateSMB.ResolvedSprite Skip = new PerpendicularSpriteUpdateSMB.ResolvedSprite(false, false, null);
	}
}
