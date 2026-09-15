using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TheraBytes.BetterUi
{
	// Token: 0x02000B47 RID: 2887
	[ExecuteAlways]
	[HelpURL("https://documentation.therabytes.de/better-ui/BetterTextMeshPro-InputField.html")]
	[AddComponentMenu("Better UI/TextMeshPro/Better TextMeshPro - Input Field", 30)]
	public class BetterTextMeshProInputField : TMP_InputField, IBetterTransitionUiElement, IResolutionDependency
	{
		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004C9D RID: 19613 RVA: 0x0016994E File Offset: 0x00167B4E
		public List<Transitions> BetterTransitions
		{
			get
			{
				return this.betterTransitions;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06004C9E RID: 19614 RVA: 0x00169956 File Offset: 0x00167B56
		public List<Graphic> AdditionalPlaceholders
		{
			get
			{
				return this.additionalPlaceholders;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004C9F RID: 19615 RVA: 0x0016995E File Offset: 0x00167B5E
		public FloatSizeModifier PointSizeScaler
		{
			get
			{
				return this.pointSizeScaler;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06004CA0 RID: 19616 RVA: 0x00169966 File Offset: 0x00167B66
		// (set) Token: 0x06004CA1 RID: 19617 RVA: 0x0016996E File Offset: 0x00167B6E
		public bool OverridePointSizeSettings
		{
			get
			{
				return this.overridePointSize;
			}
			set
			{
				this.overridePointSize = value;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x00169977 File Offset: 0x00167B77
		// (set) Token: 0x06004CA3 RID: 19619 RVA: 0x0016997F File Offset: 0x00167B7F
		public new float pointSize
		{
			get
			{
				return base.pointSize;
			}
			set
			{
				Config.Set<float>(value, delegate(float o)
				{
					base.pointSize = o;
				}, delegate(float o)
				{
					this.PointSizeScaler.SetSize(this, o);
				});
			}
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x001699A0 File Offset: 0x00167BA0
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			foreach (Transitions transitions in this.betterTransitions)
			{
				transitions.SetState(state.ToString(), instant);
			}
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x00169A14 File Offset: 0x00167C14
		public override void OnUpdateSelected(BaseEventData eventData)
		{
			base.OnUpdateSelected(eventData);
			this.DisplayPlaceholders(base.text);
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x00169A2C File Offset: 0x00167C2C
		private void DisplayPlaceholders(string input)
		{
			bool flag = string.IsNullOrEmpty(input);
			if (Application.isPlaying)
			{
				foreach (Graphic graphic in this.additionalPlaceholders)
				{
					graphic.enabled = flag;
				}
			}
		}

		// Token: 0x06004CA7 RID: 19623 RVA: 0x00169A8C File Offset: 0x00167C8C
		protected override void OnEnable()
		{
			this.CalculateSize();
			base.OnEnable();
		}

		// Token: 0x06004CA8 RID: 19624 RVA: 0x00169A9A File Offset: 0x00167C9A
		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			this.CalculateSize();
		}

		// Token: 0x06004CA9 RID: 19625 RVA: 0x00169AA8 File Offset: 0x00167CA8
		public void OnResolutionChanged()
		{
			this.CalculateSize();
		}

		// Token: 0x06004CAA RID: 19626 RVA: 0x00169AB0 File Offset: 0x00167CB0
		public void CalculateSize()
		{
			if (this.overridePointSize)
			{
				base.pointSize = this.pointSizeScaler.CalculateSize(this, "pointSizeScaler");
			}
			this.OverrideBetterTextMeshSize(this.m_Placeholder as BetterTextMeshProUGUI, this.pointSize);
			this.OverrideBetterTextMeshSize(this.m_TextComponent as BetterTextMeshProUGUI, this.pointSize);
			foreach (Graphic graphic in this.additionalPlaceholders)
			{
				this.OverrideBetterTextMeshSize(graphic as BetterTextMeshProUGUI, this.pointSize);
			}
		}

		// Token: 0x06004CAB RID: 19627 RVA: 0x00169B5C File Offset: 0x00167D5C
		private void OverrideBetterTextMeshSize(BetterTextMeshProUGUI better, float size)
		{
			if (better == null)
			{
				return;
			}
			better.IgnoreFontSizerOptions = this.overridePointSize;
			if (this.overridePointSize)
			{
				better.FontSizer.OverrideLastCalculatedSize(size);
				better.fontSize = size;
				return;
			}
			better.FontSizer.CalculateSize(this, "FontSizer");
		}

		// Token: 0x04003DB8 RID: 15800
		[SerializeField]
		[DefaultTransitionStates]
		private List<Transitions> betterTransitions = new List<Transitions>();

		// Token: 0x04003DB9 RID: 15801
		[SerializeField]
		private List<Graphic> additionalPlaceholders = new List<Graphic>();

		// Token: 0x04003DBA RID: 15802
		[SerializeField]
		private FloatSizeModifier pointSizeScaler = new FloatSizeModifier(36f, 10f, 500f);

		// Token: 0x04003DBB RID: 15803
		[SerializeField]
		private bool overridePointSize;
	}
}
