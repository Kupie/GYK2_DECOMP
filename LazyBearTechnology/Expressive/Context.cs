using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Expressive.Exceptions;
using Expressive.Expressions;
using Expressive.Functions;
using Expressive.Functions.Conversion;
using Expressive.Functions.Date;
using Expressive.Functions.Logical;
using Expressive.Functions.Mathematical;
using Expressive.Functions.Relational;
using Expressive.Functions.Statistical;
using Expressive.Functions.String;
using Expressive.Operators;
using Expressive.Operators.Additive;
using Expressive.Operators.Bitwise;
using Expressive.Operators.Conditional;
using Expressive.Operators.Grouping;
using Expressive.Operators.Logical;
using Expressive.Operators.Multiplicative;
using Expressive.Operators.Relational;

namespace Expressive
{
	// Token: 0x0200002C RID: 44
	public class Context
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005AA9 File Offset: 0x00003CA9
		internal ExpressiveOptions Options { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00005AB1 File Offset: 0x00003CB1
		internal CultureInfo CurrentCulture { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005AB9 File Offset: 0x00003CB9
		internal CultureInfo DecimalCurrentCulture { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005AC1 File Offset: 0x00003CC1
		internal char DecimalSeparator { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00005AC9 File Offset: 0x00003CC9
		internal IEnumerable<string> FunctionNames
		{
			get
			{
				return this.registeredFunctions.Keys.OrderByDescending((string k) => k.Length);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00005AFA File Offset: 0x00003CFA
		internal IEnumerable<string> OperatorNames
		{
			get
			{
				return this.registeredOperators.Keys.OrderByDescending((string k) => k.Length);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005B2B File Offset: 0x00003D2B
		private bool IsCaseInsensitiveEqualityEnabled
		{
			get
			{
				return this.Options.HasFlag(ExpressiveOptions.IgnoreCase) || this.Options.HasFlag(ExpressiveOptions.IgnoreCaseForEquality);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005B5E File Offset: 0x00003D5E
		internal StringComparison EqualityStringComparison
		{
			get
			{
				if (!this.IsCaseInsensitiveEqualityEnabled)
				{
					return StringComparison.Ordinal;
				}
				return StringComparison.OrdinalIgnoreCase;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00005B6B File Offset: 0x00003D6B
		internal bool IsCaseInsensitiveParsingEnabled
		{
			get
			{
				return this.Options.HasFlag(ExpressiveOptions.IgnoreCase) || this.Options.HasFlag(ExpressiveOptions.IgnoreCaseForParsing);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005BA0 File Offset: 0x00003DA0
		internal IEqualityComparer<string> ParsingStringComparer
		{
			get
			{
				if (!this.IsCaseInsensitiveParsingEnabled)
				{
					return EqualityComparer<string>.Default;
				}
				return StringComparer.OrdinalIgnoreCase;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005BC4 File Offset: 0x00003DC4
		internal StringComparison ParsingStringComparison
		{
			get
			{
				if (!this.IsCaseInsensitiveParsingEnabled)
				{
					return StringComparison.Ordinal;
				}
				return StringComparison.OrdinalIgnoreCase;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005BD1 File Offset: 0x00003DD1
		public Context(ExpressiveOptions options)
			: this(options, CultureInfo.CurrentCulture, CultureInfo.InvariantCulture)
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005BE4 File Offset: 0x00003DE4
		public Context(ExpressiveOptions options, CultureInfo mainCurrentCulture, CultureInfo decimalCurrentCulture)
		{
			this.Options = options;
			if (mainCurrentCulture == null)
			{
				throw new ArgumentNullException("mainCurrentCulture");
			}
			this.CurrentCulture = mainCurrentCulture;
			if (decimalCurrentCulture == null)
			{
				throw new ArgumentNullException("decimalCurrentCulture");
			}
			this.DecimalCurrentCulture = decimalCurrentCulture;
			this.DecimalSeparator = Convert.ToChar(this.DecimalCurrentCulture.NumberFormat.NumberDecimalSeparator, this.DecimalCurrentCulture);
			this.registeredFunctions = new Dictionary<string, Func<IExpression[], IDictionary<string, object>, object>>(this.ParsingStringComparer);
			this.registeredOperators = new Dictionary<string, IOperator>(this.ParsingStringComparer);
			this.RegisterOperator(new PlusOperator(), false);
			this.RegisterOperator(new SubtractOperator(), false);
			this.RegisterOperator(new BitwiseAndOperator(), false);
			this.RegisterOperator(new BitwiseOrOperator(), false);
			this.RegisterOperator(new BitwiseExclusiveOrOperator(), false);
			this.RegisterOperator(new LeftShiftOperator(), false);
			this.RegisterOperator(new RightShiftOperator(), false);
			this.RegisterOperator(new NullCoalescingOperator(), false);
			this.RegisterOperator(new ParenthesisCloseOperator(), false);
			this.RegisterOperator(new ParenthesisOpenOperator(), false);
			this.RegisterOperator(new AndOperator(), false);
			this.RegisterOperator(new NotOperator(), false);
			this.RegisterOperator(new OrOperator(), false);
			this.RegisterOperator(new DivideOperator(), false);
			this.RegisterOperator(new ModulusOperator(), false);
			this.RegisterOperator(new MultiplyOperator(), false);
			this.RegisterOperator(new EqualOperator(), false);
			this.RegisterOperator(new GreaterThanOperator(), false);
			this.RegisterOperator(new GreaterThanOrEqualOperator(), false);
			this.RegisterOperator(new LessThanOperator(), false);
			this.RegisterOperator(new LessThanOrEqualOperator(), false);
			this.RegisterOperator(new NotEqualOperator(), false);
			this.RegisterFunction(new DateFunction(), false);
			this.RegisterFunction(new DecimalFunction(), false);
			this.RegisterFunction(new DoubleFunction(), false);
			this.RegisterFunction(new IntegerFunction(), false);
			this.RegisterFunction(new LongFunction(), false);
			this.RegisterFunction(new StringFunction(), false);
			this.RegisterFunction(new AddDaysFunction(), false);
			this.RegisterFunction(new AddHoursFunction(), false);
			this.RegisterFunction(new AddMillisecondsFunction(), false);
			this.RegisterFunction(new AddMinutesFunction(), false);
			this.RegisterFunction(new AddMonthsFunction(), false);
			this.RegisterFunction(new AddSecondsFunction(), false);
			this.RegisterFunction(new AddYearsFunction(), false);
			this.RegisterFunction(new DayOfFunction(), false);
			this.RegisterFunction(new DaysBetweenFunction(), false);
			this.RegisterFunction(new HourOfFunction(), false);
			this.RegisterFunction(new HoursBetweenFunction(), false);
			this.RegisterFunction(new MillisecondOfFunction(), false);
			this.RegisterFunction(new MillisecondsBetweenFunction(), false);
			this.RegisterFunction(new MinuteOfFunction(), false);
			this.RegisterFunction(new MinutesBetweenFunction(), false);
			this.RegisterFunction(new MonthOfFunction(), false);
			this.RegisterFunction(new SecondOfFunction(), false);
			this.RegisterFunction(new SecondsBetweenFunction(), false);
			this.RegisterFunction(new YearOfFunction(), false);
			this.RegisterFunction(new AbsFunction(), false);
			this.RegisterFunction(new AcosFunction(), false);
			this.RegisterFunction(new AsinFunction(), false);
			this.RegisterFunction(new AtanFunction(), false);
			this.RegisterFunction(new CeilingFunction(), false);
			this.RegisterFunction(new CosFunction(), false);
			this.RegisterFunction(new CountFunction(), false);
			this.RegisterFunction(new ExpFunction(), false);
			this.RegisterFunction(new FloorFunction(), false);
			this.RegisterFunction(new IEEERemainderFunction(), false);
			this.RegisterFunction(new Log10Function(), false);
			this.RegisterFunction(new LogFunction(), false);
			this.RegisterFunction(new PowFunction(), false);
			this.RegisterFunction(new RandomFunction(), false);
			this.RegisterFunction(new RoundFunction(), false);
			this.RegisterFunction(new SignFunction(), false);
			this.RegisterFunction(new SinFunction(), false);
			this.RegisterFunction(new SqrtFunction(), false);
			this.RegisterFunction(new SumFunction(), false);
			this.RegisterFunction(new TanFunction(), false);
			this.RegisterFunction(new TruncateFunction(), false);
			this.RegisterFunction(new EFunction(), false);
			this.RegisterFunction(new PIFunction(), false);
			this.RegisterFunction(new IfFunction(), false);
			this.RegisterFunction(new InFunction(), false);
			this.RegisterFunction(new MaxFunction(), false);
			this.RegisterFunction(new MinFunction(), false);
			this.RegisterFunction(new AverageFunction(), false);
			this.RegisterFunction(new MeanFunction(), false);
			this.RegisterFunction(new MedianFunction(), false);
			this.RegisterFunction(new ModeFunction(), false);
			this.RegisterFunction(new ContainsFunction(), false);
			this.RegisterFunction(new EndsWithFunction(), false);
			this.RegisterFunction(new LengthFunction(), false);
			this.RegisterFunction(new PadLeftFunction(), false);
			this.RegisterFunction(new PadRightFunction(), false);
			this.RegisterFunction(new RegexFunction(), false);
			this.RegisterFunction(new StartsWithFunction(), false);
			this.RegisterFunction(new SubstringFunction(), false);
			this.RegisterFunction(new ConcatFunction(), false);
			this.RegisterFunction(new IndexOfFunction(), false);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00006090 File Offset: 0x00004290
		public Context(Context context)
		{
			this.registeredFunctions = new Dictionary<string, Func<IExpression[], IDictionary<string, object>, object>>(context.registeredFunctions);
			this.registeredOperators = new Dictionary<string, IOperator>(context.registeredOperators);
			this.Options = context.Options;
			this.CurrentCulture = context.CurrentCulture;
			this.DecimalCurrentCulture = context.DecimalCurrentCulture;
			this.DecimalSeparator = context.DecimalSeparator;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000060F5 File Offset: 0x000042F5
		public void RegisterFunction(string functionName, Func<IExpression[], IDictionary<string, object>, object> function, bool force = false)
		{
			this.CheckForExistingFunctionName(functionName, force);
			this.registeredFunctions[functionName] = function;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000610C File Offset: 0x0000430C
		public void RegisterFunction(IFunction function, bool force = false)
		{
			if (function == null)
			{
				throw new ArgumentNullException("function");
			}
			this.RegisterFunction(function.Name, delegate(IExpression[] p, IDictionary<string, object> a)
			{
				function.Variables = a;
				return function.Evaluate(p, this);
			}, force);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00006160 File Offset: 0x00004360
		public void RegisterOperator(IOperator op, bool force = false)
		{
			if (op == null)
			{
				throw new ArgumentNullException("op");
			}
			foreach (string text in op.Tags)
			{
				if (!force && this.registeredOperators.ContainsKey(text))
				{
					throw new OperatorNameAlreadyRegisteredException(text);
				}
				this.registeredOperators[text] = op;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000061DC File Offset: 0x000043DC
		public void UnregisterFunction(string functionName)
		{
			this.registeredFunctions.ContainsKey(functionName);
			this.registeredFunctions.Remove(functionName);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000061F8 File Offset: 0x000043F8
		public void UnregisterOperator(string tag)
		{
			this.registeredOperators.ContainsKey(tag);
			this.registeredOperators.Remove(tag);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006214 File Offset: 0x00004414
		public virtual bool TryGetFunction(string functionName, out Func<IExpression[], IDictionary<string, object>, object> value)
		{
			return this.registeredFunctions.TryGetValue(functionName, out value);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006223 File Offset: 0x00004423
		internal bool TryGetOperator(string operatorName, out IOperator value)
		{
			return this.registeredOperators.TryGetValue(operatorName, out value);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006232 File Offset: 0x00004432
		private void CheckForExistingFunctionName(string functionName, bool force)
		{
			if (!force && this.registeredFunctions.ContainsKey(functionName))
			{
				throw new FunctionNameAlreadyRegisteredException(functionName);
			}
		}

		// Token: 0x04000076 RID: 118
		internal const char DateSeparator = '#';

		// Token: 0x04000077 RID: 119
		internal const char ParameterSeparator = ',';

		// Token: 0x04000078 RID: 120
		private readonly IDictionary<string, Func<IExpression[], IDictionary<string, object>, object>> registeredFunctions;

		// Token: 0x04000079 RID: 121
		private readonly IDictionary<string, IOperator> registeredOperators;
	}
}
