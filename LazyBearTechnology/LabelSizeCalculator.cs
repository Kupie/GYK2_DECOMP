using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000140 RID: 320
	[RequireComponent(typeof(LayoutElement))]
	[RequireComponent(typeof(ContentSizeFitter))]
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class LabelSizeCalculator : MonoBehaviour
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x0002145A File Offset: 0x0001F65A
		private void Awake()
		{
			LabelSizeCalculator.instance = this;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00021464 File Offset: 0x0001F664
		private Vector2 CalcRenderedTextValues(TextMeshProUGUI refLabel, string text, float maxWidth)
		{
			this.layout.preferredWidth = maxWidth;
			this.label.text = text;
			this.label.font = refLabel.font;
			this.label.fontSize = refLabel.fontSize;
			this.label.isRightToLeftText = refLabel.isRightToLeftText;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
			this.label.ForceMeshUpdate(false, false);
			Vector2 renderedValues = this.label.GetRenderedValues();
			if (renderedValues.x <= maxWidth)
			{
				return renderedValues;
			}
			return new Vector2(maxWidth, renderedValues.y);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000214F7 File Offset: 0x0001F6F7
		public static float CalculateFitWidth(TextMeshProUGUI label, string text, float maxWidth)
		{
			return LabelSizeCalculator.CalculateFitVector(label, text, maxWidth).x;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00021506 File Offset: 0x0001F706
		public static Vector2 CalculateFitVector(TextMeshProUGUI label, string text, float maxWidth)
		{
			return LabelSizeCalculator.instance.CalcRenderedTextValues(label, text, maxWidth);
		}

		// Token: 0x040003B4 RID: 948
		[SerializeField]
		private TextMeshProUGUI label;

		// Token: 0x040003B5 RID: 949
		[SerializeField]
		private RectTransform rectTransform;

		// Token: 0x040003B6 RID: 950
		[SerializeField]
		private LayoutElement layout;

		// Token: 0x040003B7 RID: 951
		private static LabelSizeCalculator instance;
	}
}
