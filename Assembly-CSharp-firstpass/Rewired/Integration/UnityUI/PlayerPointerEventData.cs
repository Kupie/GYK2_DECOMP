using System;
using System.Text;
using Rewired.UI;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x0200004D RID: 77
	public class PlayerPointerEventData : PointerEventData
	{
		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x0000A02B File Offset: 0x0000822B
		// (set) Token: 0x0600048C RID: 1164 RVA: 0x0000A033 File Offset: 0x00008233
		public int playerId { get; set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x0000A03C File Offset: 0x0000823C
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x0000A044 File Offset: 0x00008244
		public int inputSourceIndex { get; set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0000A04D File Offset: 0x0000824D
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x0000A055 File Offset: 0x00008255
		public IMouseInputSource mouseSource { get; set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x0000A05E File Offset: 0x0000825E
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x0000A066 File Offset: 0x00008266
		public ITouchInputSource touchSource { get; set; }

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0000A06F File Offset: 0x0000826F
		// (set) Token: 0x06000494 RID: 1172 RVA: 0x0000A077 File Offset: 0x00008277
		public PointerEventType sourceType { get; set; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0000A080 File Offset: 0x00008280
		// (set) Token: 0x06000496 RID: 1174 RVA: 0x0000A088 File Offset: 0x00008288
		public int buttonIndex { get; set; }

		// Token: 0x06000497 RID: 1175 RVA: 0x0000A091 File Offset: 0x00008291
		public PlayerPointerEventData(EventSystem eventSystem)
			: base(eventSystem)
		{
			this.playerId = -1;
			this.inputSourceIndex = -1;
			this.buttonIndex = -1;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000A0B0 File Offset: 0x000082B0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<b>Player Id</b>: " + this.playerId.ToString());
			string text = "<b>Mouse Source</b>: ";
			IMouseInputSource mouseSource = this.mouseSource;
			stringBuilder.AppendLine(text + ((mouseSource != null) ? mouseSource.ToString() : null));
			stringBuilder.AppendLine("<b>Input Source Index</b>: " + this.inputSourceIndex.ToString());
			string text2 = "<b>Touch Source/b>: ";
			ITouchInputSource touchSource = this.touchSource;
			stringBuilder.AppendLine(text2 + ((touchSource != null) ? touchSource.ToString() : null));
			stringBuilder.AppendLine("<b>Source Type</b>: " + this.sourceType.ToString());
			stringBuilder.AppendLine("<b>Button Index</b>: " + this.buttonIndex.ToString());
			stringBuilder.Append(base.ToString());
			return stringBuilder.ToString();
		}
	}
}
