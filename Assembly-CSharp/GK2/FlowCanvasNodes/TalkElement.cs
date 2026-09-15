using System;
using LazyBearTechnology;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD7 RID: 3031
	[Serializable]
	public class TalkElement : Element
	{
		// Token: 0x06004E8E RID: 20110 RVA: 0x0017235A File Offset: 0x0017055A
		public TalkElement(string wgoId, string text)
		{
			this.wgoId = wgoId;
			this.text = text;
		}

		// Token: 0x04003FB9 RID: 16313
		public string text;

		// Token: 0x04003FBA RID: 16314
		public UIBasicBubble.ForceCornerPosition forceCornerPosition;
	}
}
