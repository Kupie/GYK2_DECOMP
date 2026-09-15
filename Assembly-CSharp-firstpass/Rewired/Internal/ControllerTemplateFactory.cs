using System;

namespace Rewired.Internal
{
	// Token: 0x0200004C RID: 76
	public static class ControllerTemplateFactory
	{
		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00009ED5 File Offset: 0x000080D5
		public static Type[] templateTypes
		{
			get
			{
				return ControllerTemplateFactory._defaultTemplateTypes;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00009EDC File Offset: 0x000080DC
		public static Type[] templateInterfaceTypes
		{
			get
			{
				return ControllerTemplateFactory._defaultTemplateInterfaceTypes;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00009EE4 File Offset: 0x000080E4
		public static IControllerTemplate Create(Guid typeGuid, object payload)
		{
			if (typeGuid == GamepadTemplate.typeGuid)
			{
				return new GamepadTemplate(payload);
			}
			if (typeGuid == RacingWheelTemplate.typeGuid)
			{
				return new RacingWheelTemplate(payload);
			}
			if (typeGuid == HOTASTemplate.typeGuid)
			{
				return new HOTASTemplate(payload);
			}
			if (typeGuid == FlightYokeTemplate.typeGuid)
			{
				return new FlightYokeTemplate(payload);
			}
			if (typeGuid == FlightPedalsTemplate.typeGuid)
			{
				return new FlightPedalsTemplate(payload);
			}
			if (typeGuid == SixDofControllerTemplate.typeGuid)
			{
				return new SixDofControllerTemplate(payload);
			}
			return null;
		}

		// Token: 0x040002A1 RID: 673
		private static readonly Type[] _defaultTemplateTypes = new Type[]
		{
			typeof(GamepadTemplate),
			typeof(RacingWheelTemplate),
			typeof(HOTASTemplate),
			typeof(FlightYokeTemplate),
			typeof(FlightPedalsTemplate),
			typeof(SixDofControllerTemplate)
		};

		// Token: 0x040002A2 RID: 674
		private static readonly Type[] _defaultTemplateInterfaceTypes = new Type[]
		{
			typeof(IGamepadTemplate),
			typeof(IRacingWheelTemplate),
			typeof(IHOTASTemplate),
			typeof(IFlightYokeTemplate),
			typeof(IFlightPedalsTemplate),
			typeof(ISixDofControllerTemplate)
		};
	}
}
