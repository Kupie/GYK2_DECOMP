using System;

namespace Expressive.Operators
{
	// Token: 0x0200003C RID: 60
	public enum OperatorPrecedence
	{
		// Token: 0x04000099 RID: 153
		Minimum,
		// Token: 0x0400009A RID: 154
		Or,
		// Token: 0x0400009B RID: 155
		And,
		// Token: 0x0400009C RID: 156
		Equal,
		// Token: 0x0400009D RID: 157
		NotEqual,
		// Token: 0x0400009E RID: 158
		LessThan,
		// Token: 0x0400009F RID: 159
		GreaterThan,
		// Token: 0x040000A0 RID: 160
		LessThanOrEqual,
		// Token: 0x040000A1 RID: 161
		GreaterThanOrEqual,
		// Token: 0x040000A2 RID: 162
		Not,
		// Token: 0x040000A3 RID: 163
		BitwiseOr,
		// Token: 0x040000A4 RID: 164
		BitwiseXOr,
		// Token: 0x040000A5 RID: 165
		BitwiseAnd,
		// Token: 0x040000A6 RID: 166
		LeftShift,
		// Token: 0x040000A7 RID: 167
		RightShift,
		// Token: 0x040000A8 RID: 168
		Add,
		// Token: 0x040000A9 RID: 169
		Subtract,
		// Token: 0x040000AA RID: 170
		Multiply,
		// Token: 0x040000AB RID: 171
		Modulus,
		// Token: 0x040000AC RID: 172
		Divide,
		// Token: 0x040000AD RID: 173
		NullCoalescing,
		// Token: 0x040000AE RID: 174
		UnaryPlus,
		// Token: 0x040000AF RID: 175
		UnaryMinus,
		// Token: 0x040000B0 RID: 176
		ParenthesisOpen,
		// Token: 0x040000B1 RID: 177
		ParenthesisClose
	}
}
