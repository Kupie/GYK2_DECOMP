using System;

namespace Rewired
{
	// Token: 0x02000013 RID: 19
	public interface IGamepadTemplate : IControllerTemplate
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000051 RID: 81
		IControllerTemplateButton actionBottomRow1 { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000052 RID: 82
		IControllerTemplateButton a { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000053 RID: 83
		IControllerTemplateButton actionBottomRow2 { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000054 RID: 84
		IControllerTemplateButton b { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000055 RID: 85
		IControllerTemplateButton actionBottomRow3 { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000056 RID: 86
		IControllerTemplateButton c { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000057 RID: 87
		IControllerTemplateButton actionTopRow1 { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000058 RID: 88
		IControllerTemplateButton x { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000059 RID: 89
		IControllerTemplateButton actionTopRow2 { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005A RID: 90
		IControllerTemplateButton y { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005B RID: 91
		IControllerTemplateButton actionTopRow3 { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600005C RID: 92
		IControllerTemplateButton z { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005D RID: 93
		IControllerTemplateButton leftShoulder1 { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005E RID: 94
		IControllerTemplateButton leftBumper { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005F RID: 95
		IControllerTemplateAxis leftShoulder2 { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000060 RID: 96
		IControllerTemplateAxis leftTrigger { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000061 RID: 97
		IControllerTemplateButton rightShoulder1 { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000062 RID: 98
		IControllerTemplateButton rightBumper { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000063 RID: 99
		IControllerTemplateAxis rightShoulder2 { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000064 RID: 100
		IControllerTemplateAxis rightTrigger { get; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000065 RID: 101
		IControllerTemplateButton center1 { get; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000066 RID: 102
		IControllerTemplateButton back { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000067 RID: 103
		IControllerTemplateButton center2 { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000068 RID: 104
		IControllerTemplateButton start { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000069 RID: 105
		IControllerTemplateButton center3 { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006A RID: 106
		IControllerTemplateButton guide { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006B RID: 107
		IControllerTemplateThumbStick leftStick { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600006C RID: 108
		IControllerTemplateThumbStick rightStick { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600006D RID: 109
		IControllerTemplateDPad dPad { get; }
	}
}
