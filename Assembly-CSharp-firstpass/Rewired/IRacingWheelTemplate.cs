using System;

namespace Rewired
{
	// Token: 0x02000014 RID: 20
	public interface IRacingWheelTemplate : IControllerTemplate
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600006E RID: 110
		IControllerTemplateAxis wheel { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006F RID: 111
		IControllerTemplateAxis accelerator { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000070 RID: 112
		IControllerTemplateAxis brake { get; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000071 RID: 113
		IControllerTemplateAxis clutch { get; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000072 RID: 114
		IControllerTemplateButton shiftDown { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000073 RID: 115
		IControllerTemplateButton shiftUp { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000074 RID: 116
		IControllerTemplateButton wheelButton1 { get; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000075 RID: 117
		IControllerTemplateButton wheelButton2 { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000076 RID: 118
		IControllerTemplateButton wheelButton3 { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000077 RID: 119
		IControllerTemplateButton wheelButton4 { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000078 RID: 120
		IControllerTemplateButton wheelButton5 { get; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000079 RID: 121
		IControllerTemplateButton wheelButton6 { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600007A RID: 122
		IControllerTemplateButton wheelButton7 { get; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007B RID: 123
		IControllerTemplateButton wheelButton8 { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007C RID: 124
		IControllerTemplateButton wheelButton9 { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007D RID: 125
		IControllerTemplateButton wheelButton10 { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600007E RID: 126
		IControllerTemplateButton consoleButton1 { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600007F RID: 127
		IControllerTemplateButton consoleButton2 { get; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000080 RID: 128
		IControllerTemplateButton consoleButton3 { get; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000081 RID: 129
		IControllerTemplateButton consoleButton4 { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000082 RID: 130
		IControllerTemplateButton consoleButton5 { get; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000083 RID: 131
		IControllerTemplateButton consoleButton6 { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000084 RID: 132
		IControllerTemplateButton consoleButton7 { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000085 RID: 133
		IControllerTemplateButton consoleButton8 { get; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000086 RID: 134
		IControllerTemplateButton consoleButton9 { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000087 RID: 135
		IControllerTemplateButton consoleButton10 { get; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000088 RID: 136
		IControllerTemplateButton shifter1 { get; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000089 RID: 137
		IControllerTemplateButton shifter2 { get; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600008A RID: 138
		IControllerTemplateButton shifter3 { get; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600008B RID: 139
		IControllerTemplateButton shifter4 { get; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600008C RID: 140
		IControllerTemplateButton shifter5 { get; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600008D RID: 141
		IControllerTemplateButton shifter6 { get; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600008E RID: 142
		IControllerTemplateButton shifter7 { get; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600008F RID: 143
		IControllerTemplateButton shifter8 { get; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000090 RID: 144
		IControllerTemplateButton shifter9 { get; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000091 RID: 145
		IControllerTemplateButton shifter10 { get; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000092 RID: 146
		IControllerTemplateButton reverseGear { get; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000093 RID: 147
		IControllerTemplateButton select { get; }

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000094 RID: 148
		IControllerTemplateButton start { get; }

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000095 RID: 149
		IControllerTemplateButton systemButton { get; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000096 RID: 150
		IControllerTemplateButton horn { get; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000097 RID: 151
		IControllerTemplateDPad dPad { get; }
	}
}
