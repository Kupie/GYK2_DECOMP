using System;

namespace Rewired
{
	// Token: 0x02000017 RID: 23
	public interface IFlightPedalsTemplate : IControllerTemplate
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000121 RID: 289
		IControllerTemplateAxis leftPedal { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000122 RID: 290
		IControllerTemplateAxis rightPedal { get; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000123 RID: 291
		IControllerTemplateAxis slide { get; }
	}
}
