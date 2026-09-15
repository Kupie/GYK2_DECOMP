using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200086D RID: 2157
public class UIBuffElement : LazyWidget<UIBuffElementData>
{
	// Token: 0x17000833 RID: 2099
	// (get) Token: 0x06003730 RID: 14128 RVA: 0x0010AA40 File Offset: 0x00108C40
	// (set) Token: 0x06003731 RID: 14129 RVA: 0x0010AA48 File Offset: 0x00108C48
	public string Id { get; private set; }

	// Token: 0x06003732 RID: 14130 RVA: 0x0010AA51 File Offset: 0x00108C51
	protected override void SetData(UIBuffElementData data)
	{
		this.Id = data.PerkData.id;
		base.SetData(data);
	}

	// Token: 0x06003733 RID: 14131 RVA: 0x0010AA6C File Offset: 0x00108C6C
	public void PlayHudFx()
	{
		this.ClearHudFx();
		UIBuffElementData data = this.data;
		string text;
		if (data == null)
		{
			text = null;
		}
		else
		{
			PerkData perkData = data.PerkData;
			if (perkData == null)
			{
				text = null;
			}
			else
			{
				PerkDef definition = perkData.Definition;
				text = ((definition != null) ? definition.hudFxPrefabId : null);
			}
		}
		string text2 = text;
		if (string.IsNullOrEmpty(text2))
		{
			return;
		}
		this.hudFxInstance = HudFX.Spawn(this.icon.rectTransform, text2);
	}

	// Token: 0x06003734 RID: 14132 RVA: 0x0010AACC File Offset: 0x00108CCC
	public override void Redraw()
	{
		base.Redraw();
		this.icon.sprite = this.data.PerkData.Definition.Icon;
		this.icon.SetNativeSize();
		this.buffDurationLabel.gameObject.SetActive(!this.data.HasHiddenTimer);
		this.buffDurationLabel.text = PerkSystemData.GetFormattedDuration(this.data.Duration);
		Vector2 vector = (this.buffDurationLabel.gameObject.activeSelf ? Vector2.zero : new Vector2(0f, 8f));
		GameObject gameObject = base.gameObject;
		Action action = new Action(this.ShowBuffTooltip);
		bool flag = true;
		bool flag2 = false;
		Vector2 vector2 = vector;
		UIMouseTooltip.AttachCustom(gameObject, action, flag, flag2, default(UIMouseTooltipEdges), vector2);
	}

	// Token: 0x06003735 RID: 14133 RVA: 0x0010AB92 File Offset: 0x00108D92
	private void OnDisable()
	{
		this.ClearHudFx();
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x0010AB9A File Offset: 0x00108D9A
	private void ClearHudFx()
	{
		if (this.hudFxInstance == null)
		{
			return;
		}
		global::UnityEngine.Object.Destroy(this.hudFxInstance);
		this.hudFxInstance = null;
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x0010ABC0 File Offset: 0x00108DC0
	private void ShowBuffTooltip()
	{
		UIBuffElementData data = this.data;
		bool flag;
		if (data == null)
		{
			flag = null != null;
		}
		else
		{
			PerkData perkData = data.PerkData;
			flag = ((perkData != null) ? perkData.Definition : null) != null;
		}
		if (!flag)
		{
			return;
		}
		Vector2 vector = Vector2.zero;
		UIMouseTooltip component = base.GetComponent<UIMouseTooltip>();
		if (component != null)
		{
			vector = component.GetResolvedAppearOffset();
		}
		UITooltip.ShowPerk(this.data.PerkData.Definition, base.transform as RectTransform, vector);
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x0010AC2C File Offset: 0x00108E2C
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIBuffElementData(new PerkData("test1"), 20f, false, false));
	}

	// Token: 0x04002BFD RID: 11261
	[SerializeField]
	private Image icon;

	// Token: 0x04002BFE RID: 11262
	[SerializeField]
	private TextMeshProUGUI buffDurationLabel;

	// Token: 0x04002C00 RID: 11264
	private GameObject hudFxInstance;
}
