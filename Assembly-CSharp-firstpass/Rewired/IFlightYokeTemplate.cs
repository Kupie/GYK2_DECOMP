using System;

namespace Rewired
{
	// Token: 0x02000016 RID: 22
	public interface IFlightYokeTemplate : IControllerTemplate
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060000F0 RID: 240
		IControllerTemplateButton leftPaddle { get; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060000F1 RID: 241
		IControllerTemplateButton rightPaddle { get; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060000F2 RID: 242
		IControllerTemplateButton leftGripButton1 { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060000F3 RID: 243
		IControllerTemplateButton leftGripButton2 { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060000F4 RID: 244
		IControllerTemplateButton leftGripButton3 { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060000F5 RID: 245
		IControllerTemplateButton leftGripButton4 { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060000F6 RID: 246
		IControllerTemplateButton leftGripButton5 { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060000F7 RID: 247
		IControllerTemplateButton leftGripButton6 { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060000F8 RID: 248
		IControllerTemplateButton rightGripButton1 { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060000F9 RID: 249
		IControllerTemplateButton rightGripButton2 { get; }

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060000FA RID: 250
		IControllerTemplateButton rightGripButton3 { get; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060000FB RID: 251
		IControllerTemplateButton rightGripButton4 { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060000FC RID: 252
		IControllerTemplateButton rightGripButton5 { get; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060000FD RID: 253
		IControllerTemplateButton rightGripButton6 { get; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060000FE RID: 254
		IControllerTemplateButton centerButton1 { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060000FF RID: 255
		IControllerTemplateButton centerButton2 { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000100 RID: 256
		IControllerTemplateButton centerButton3 { get; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000101 RID: 257
		IControllerTemplateButton centerButton4 { get; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000102 RID: 258
		IControllerTemplateButton centerButton5 { get; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000103 RID: 259
		IControllerTemplateButton centerButton6 { get; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000104 RID: 260
		IControllerTemplateButton centerButton7 { get; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000105 RID: 261
		IControllerTemplateButton centerButton8 { get; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000106 RID: 262
		IControllerTemplateButton wheel1Up { get; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000107 RID: 263
		IControllerTemplateButton wheel1Down { get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000108 RID: 264
		IControllerTemplateButton wheel1Press { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000109 RID: 265
		IControllerTemplateButton wheel2Up { get; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600010A RID: 266
		IControllerTemplateButton wheel2Down { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600010B RID: 267
		IControllerTemplateButton wheel2Press { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600010C RID: 268
		IControllerTemplateButton consoleButton1 { get; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600010D RID: 269
		IControllerTemplateButton consoleButton2 { get; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600010E RID: 270
		IControllerTemplateButton consoleButton3 { get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600010F RID: 271
		IControllerTemplateButton consoleButton4 { get; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000110 RID: 272
		IControllerTemplateButton consoleButton5 { get; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000111 RID: 273
		IControllerTemplateButton consoleButton6 { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000112 RID: 274
		IControllerTemplateButton consoleButton7 { get; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000113 RID: 275
		IControllerTemplateButton consoleButton8 { get; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000114 RID: 276
		IControllerTemplateButton consoleButton9 { get; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000115 RID: 277
		IControllerTemplateButton consoleButton10 { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000116 RID: 278
		IControllerTemplateButton mode1 { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000117 RID: 279
		IControllerTemplateButton mode2 { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000118 RID: 280
		IControllerTemplateButton mode3 { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000119 RID: 281
		IControllerTemplateYoke yoke { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600011A RID: 282
		IControllerTemplateThrottle lever1 { get; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600011B RID: 283
		IControllerTemplateThrottle lever2 { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600011C RID: 284
		IControllerTemplateThrottle lever3 { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600011D RID: 285
		IControllerTemplateThrottle lever4 { get; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600011E RID: 286
		IControllerTemplateThrottle lever5 { get; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600011F RID: 287
		IControllerTemplateHat leftGripHat { get; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000120 RID: 288
		IControllerTemplateHat rightGripHat { get; }
	}
}
