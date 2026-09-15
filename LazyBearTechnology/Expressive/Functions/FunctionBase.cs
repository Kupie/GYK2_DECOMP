using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Exceptions;
using Expressive.Expressions;

namespace Expressive.Functions
{
	// Token: 0x02000057 RID: 87
	public abstract class FunctionBase : IFunction
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x0000C0AB File Offset: 0x0000A2AB
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x0000C0B3 File Offset: 0x0000A2B3
		public IDictionary<string, object> Variables { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001AA RID: 426
		public abstract string Name { get; }

		// Token: 0x060001AB RID: 427
		public abstract object Evaluate(IExpression[] parameters, Context context);

		// Token: 0x060001AC RID: 428 RVA: 0x0000C0BC File Offset: 0x0000A2BC
		protected void ValidateParameterCount(IExpression[] parameters, int expectedCount, int minimumCount)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			if (expectedCount == 0 && (parameters.Any<IExpression>() || parameters.Length != expectedCount))
			{
				throw new ParameterCountMismatchException(this.Name + "() does not take any arguments");
			}
			if (expectedCount > 0 && (!parameters.Any<IExpression>() || parameters.Length != expectedCount))
			{
				throw new ParameterCountMismatchException(string.Format("{0}() takes only {1} argument(s)", this.Name, expectedCount));
			}
			if (minimumCount > 0 && (!parameters.Any<IExpression>() || parameters.Length < minimumCount))
			{
				throw new ParameterCountMismatchException(string.Format("{0}() expects at least {1} argument(s)", this.Name, minimumCount));
			}
		}
	}
}
