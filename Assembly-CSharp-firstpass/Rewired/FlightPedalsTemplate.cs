using System;

namespace Rewired
{
	// Token: 0x0200001D RID: 29
	public sealed class FlightPedalsTemplate : ControllerTemplate, IFlightPedalsTemplate, IControllerTemplate
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000225 RID: 549 RVA: 0x00002EB2 File Offset: 0x000010B2
		IControllerTemplateAxis IFlightPedalsTemplate.leftPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(0);
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00002EBB File Offset: 0x000010BB
		IControllerTemplateAxis IFlightPedalsTemplate.rightPedal
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(1);
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00002EC4 File Offset: 0x000010C4
		IControllerTemplateAxis IFlightPedalsTemplate.slide
		{
			get
			{
				return base.GetElement<IControllerTemplateAxis>(2);
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00002E98 File Offset: 0x00001098
		public FlightPedalsTemplate(object payload)
			: base(payload)
		{
		}

		// Token: 0x04000193 RID: 403
		public static readonly Guid typeGuid = new Guid("f6fe76f8-be2a-4db2-b853-9e3652075913");

		// Token: 0x04000194 RID: 404
		public const int elementId_leftPedal = 0;

		// Token: 0x04000195 RID: 405
		public const int elementId_rightPedal = 1;

		// Token: 0x04000196 RID: 406
		public const int elementId_slide = 2;
	}
}
