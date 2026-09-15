using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020007D4 RID: 2004
public class CinematicsTextWidget : LazyWidget<CinematicsTextWidgetData>
{
	// Token: 0x06003386 RID: 13190 RVA: 0x000F8E28 File Offset: 0x000F7028
	public override void Init()
	{
		base.Init();
		if (this.rectTransform == null)
		{
			this.rectTransform = base.transform as RectTransform;
		}
		if (this.label == null)
		{
			this.label = base.GetComponentInChildren<TextMeshProUGUI>(true);
		}
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x000F8E75 File Offset: 0x000F7075
	protected override void SetData(CinematicsTextWidgetData data)
	{
		this.UnsubscribeFromData();
		base.SetData(data);
		this.SubscribeToData();
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x000F8E8A File Offset: 0x000F708A
	public override void Hide()
	{
		this.UnsubscribeFromData();
		base.Hide();
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x000F8E98 File Offset: 0x000F7098
	public override void Redraw()
	{
		if (this.data == null)
		{
			return;
		}
		this.ApplyWorldLayout();
		this.ApplyBackgroundVisibility();
		this.ApplyColor(this.data.Color);
	}

	// Token: 0x0600338A RID: 13194 RVA: 0x000F8EC0 File Offset: 0x000F70C0
	private void LateUpdate()
	{
		if (this.data == null || !base.isActiveAndEnabled)
		{
			return;
		}
		this.ApplyWorldLayout();
		this.ApplyBackgroundVisibility();
		this.ApplyColor(this.data.Color);
	}

	// Token: 0x0600338B RID: 13195 RVA: 0x000F8EF0 File Offset: 0x000F70F0
	private void OnDestroy()
	{
		this.UnsubscribeFromData();
	}

	// Token: 0x0600338C RID: 13196 RVA: 0x000F8EF8 File Offset: 0x000F70F8
	private void ApplyWorldLayout()
	{
		if (this.rectTransform == null || CameraSystem.Instance == null)
		{
			return;
		}
		this.rectTransform.pivot = this.data.Pivot;
		this.rectTransform.position = CameraSystem.WorldToScreenPoint(new Vector3(this.data.Position.x, this.data.Position.y, 0f));
		bool showBackground = this.data.ShowBackground;
		if (this.contentSizeFitter != null)
		{
			this.contentSizeFitter.enabled = showBackground;
		}
		if (!showBackground)
		{
			this.rectTransform.sizeDelta = this.GetMappedWorldSize();
		}
	}

	// Token: 0x0600338D RID: 13197 RVA: 0x000F8FAC File Offset: 0x000F71AC
	private Vector2 GetMappedWorldSize()
	{
		Vector2 pivot = this.data.Pivot;
		Vector3 vector = new Vector3(this.data.Position.x - this.data.Size.x * pivot.x, this.data.Position.y - this.data.Size.y * pivot.y, 0f);
		Vector3 vector2 = new Vector3(this.data.Position.x + this.data.Size.x * (1f - pivot.x), this.data.Position.y + this.data.Size.y * (1f - pivot.y), 0f);
		Vector3 vector3 = CameraSystem.WorldToScreenPoint(vector);
		Vector3 vector4 = CameraSystem.WorldToScreenPoint(vector2);
		float num = LazyUI.ScaleFactor;
		if (num <= 0f)
		{
			num = (float)Mathf.Max(1, ResolutionConfig.PixelSize);
		}
		return new Vector2(Mathf.Abs(vector4.x - vector3.x) / num, Mathf.Abs(vector4.y - vector3.y) / num);
	}

	// Token: 0x0600338E RID: 13198 RVA: 0x000F90E4 File Offset: 0x000F72E4
	private void SubscribeToData()
	{
		if (this.data == null)
		{
			return;
		}
		CinematicsTextWidgetData data = this.data;
		data.OnTextChanged = (Action<string>)Delegate.Combine(data.OnTextChanged, new Action<string>(this.OnTextChanged));
		CinematicsTextWidgetData data2 = this.data;
		data2.OnVisibleChanged = (Action<bool>)Delegate.Combine(data2.OnVisibleChanged, new Action<bool>(this.OnVisibleChanged));
		CinematicsTextWidgetData data3 = this.data;
		data3.OnColorChanged = (Action<Color>)Delegate.Combine(data3.OnColorChanged, new Action<Color>(this.OnColorChanged));
	}

	// Token: 0x0600338F RID: 13199 RVA: 0x000F9170 File Offset: 0x000F7370
	private void UnsubscribeFromData()
	{
		if (this.data == null)
		{
			return;
		}
		CinematicsTextWidgetData data = this.data;
		data.OnTextChanged = (Action<string>)Delegate.Remove(data.OnTextChanged, new Action<string>(this.OnTextChanged));
		CinematicsTextWidgetData data2 = this.data;
		data2.OnVisibleChanged = (Action<bool>)Delegate.Remove(data2.OnVisibleChanged, new Action<bool>(this.OnVisibleChanged));
		CinematicsTextWidgetData data3 = this.data;
		data3.OnColorChanged = (Action<Color>)Delegate.Remove(data3.OnColorChanged, new Action<Color>(this.OnColorChanged));
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x000F91FB File Offset: 0x000F73FB
	private void OnTextChanged(string text)
	{
		if (this.label != null)
		{
			this.label.text = text;
		}
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x000F9217 File Offset: 0x000F7417
	private void OnVisibleChanged(bool visible)
	{
		base.gameObject.SetActive(visible);
	}

	// Token: 0x06003392 RID: 13202 RVA: 0x000F9225 File Offset: 0x000F7425
	private void OnColorChanged(Color color)
	{
		this.ApplyColor(color);
	}

	// Token: 0x06003393 RID: 13203 RVA: 0x000F922E File Offset: 0x000F742E
	private void ApplyBackgroundVisibility()
	{
		if (this.backgroundImage == null)
		{
			return;
		}
		this.backgroundImage.gameObject.SetActive(this.data != null && this.data.ShowBackground);
	}

	// Token: 0x06003394 RID: 13204 RVA: 0x000F9268 File Offset: 0x000F7468
	private void ApplyColor(Color color)
	{
		if (this.label != null)
		{
			this.label.color = color;
		}
		if (this.backgroundImage == null || !this.backgroundImage.gameObject.activeSelf)
		{
			return;
		}
		Color color2 = this.backgroundImage.color;
		color2.a = color.a;
		this.backgroundImage.color = color2;
	}

	// Token: 0x06003395 RID: 13205 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x0400292E RID: 10542
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x0400292F RID: 10543
	[SerializeField]
	private RectTransform rectTransform;

	// Token: 0x04002930 RID: 10544
	[SerializeField]
	private Image backgroundImage;

	// Token: 0x04002931 RID: 10545
	[SerializeField]
	private ContentSizeFitter contentSizeFitter;
}
