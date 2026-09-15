using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000845 RID: 2117
public class UIMouseTooltipCollider : MonoBehaviour
{
	// Token: 0x1700080E RID: 2062
	// (get) Token: 0x06003616 RID: 13846 RVA: 0x00103BB4 File Offset: 0x00101DB4
	private RectTransform Target
	{
		get
		{
			if (!(this.overrideTarget != null))
			{
				return base.transform as RectTransform;
			}
			return this.overrideTarget;
		}
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x00103BD8 File Offset: 0x00101DD8
	public static UIMouseTooltipCollider Attach(Collider2D collider, string lngId, RectTransform overrideTarget = null)
	{
		if (collider == null)
		{
			return null;
		}
		GameObject gameObject = collider.gameObject;
		UIMouseTooltipCollider uimouseTooltipCollider = gameObject.GetComponent<UIMouseTooltipCollider>();
		if (uimouseTooltipCollider == null)
		{
			uimouseTooltipCollider = gameObject.AddComponent<UIMouseTooltipCollider>();
		}
		uimouseTooltipCollider.collisionCollider = collider;
		uimouseTooltipCollider.lngId = lngId;
		uimouseTooltipCollider.overrideTarget = overrideTarget;
		return uimouseTooltipCollider;
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x00103C24 File Offset: 0x00101E24
	public void SetLocalizationId(string lngId)
	{
		if (this.lngId == lngId)
		{
			return;
		}
		this.HideTooltip(true);
		this.lngId = lngId;
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x00103C43 File Offset: 0x00101E43
	private void Awake()
	{
		if (this.collisionCollider == null)
		{
			this.collisionCollider = base.GetComponent<Collider2D>();
		}
	}

	// Token: 0x0600361A RID: 13850 RVA: 0x00103C5F File Offset: 0x00101E5F
	private void OnEnable()
	{
		LazyInput.OnInputChanged += this.OnInputChanged;
		GameSettings.OnLanguageChanged += this.OnLanguageChanged;
	}

	// Token: 0x0600361B RID: 13851 RVA: 0x00103C83 File Offset: 0x00101E83
	private void OnDisable()
	{
		LazyInput.OnInputChanged -= this.OnInputChanged;
		GameSettings.OnLanguageChanged -= this.OnLanguageChanged;
		this.HideTooltip(true);
		this.entered = false;
	}

	// Token: 0x0600361C RID: 13852 RVA: 0x00103CB8 File Offset: 0x00101EB8
	private void Update()
	{
		if (this.collisionCollider == null || !UIMouseTooltip.IsAvailable)
		{
			if (this.entered)
			{
				this.HideTooltip(true);
				this.entered = false;
			}
			return;
		}
		bool flag = this.IsMouseOvered();
		if (!this.entered)
		{
			if (flag)
			{
				this.entered = true;
				UIMouseTooltip.TryShow(this.Target, this.lngId, default(Vector2), null);
				return;
			}
		}
		else if (!flag)
		{
			this.entered = false;
			this.HideTooltip(false);
		}
	}

	// Token: 0x0600361D RID: 13853 RVA: 0x00103D38 File Offset: 0x00101F38
	private void OnInputChanged()
	{
		if (UIMouseTooltip.IsAvailable)
		{
			return;
		}
		this.HideTooltip(true);
		this.entered = false;
	}

	// Token: 0x0600361E RID: 13854 RVA: 0x00103D50 File Offset: 0x00101F50
	private void OnLanguageChanged()
	{
		this.HideTooltip(true);
		this.entered = false;
	}

	// Token: 0x0600361F RID: 13855 RVA: 0x00103D60 File Offset: 0x00101F60
	private void HideTooltip(bool immediately)
	{
		UIMouseTooltip.HideIfShowingAt(this.Target, immediately);
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x00103D70 File Offset: 0x00101F70
	private bool IsMouseOvered()
	{
		Vector2 vector = Input.mousePosition;
		if (this.collisionCollider.OverlapPoint(vector))
		{
			return true;
		}
		Camera main = Camera.main;
		if (main == null)
		{
			return false;
		}
		Vector3 vector2 = vector;
		vector2.z = this.collisionCollider.transform.position.z - main.transform.position.z;
		return this.collisionCollider.OverlapPoint(main.ScreenToWorldPoint(vector2));
	}

	// Token: 0x04002B4C RID: 11084
	[SerializeField]
	private string lngId;

	// Token: 0x04002B4D RID: 11085
	[SerializeField]
	private Collider2D collisionCollider;

	// Token: 0x04002B4E RID: 11086
	[SerializeField]
	private RectTransform overrideTarget;

	// Token: 0x04002B4F RID: 11087
	private bool entered;
}
