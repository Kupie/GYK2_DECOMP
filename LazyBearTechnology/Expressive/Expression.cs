using System;
using System.Collections.Generic;
using System.Threading;
using Expressive.Exceptions;
using Expressive.Expressions;
using Expressive.Functions;
using Expressive.Operators;

namespace Expressive
{
	// Token: 0x0200002D RID: 45
	public sealed class Expression : IExpression
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000624C File Offset: 0x0000444C
		public IReadOnlyCollection<string> ReferencedVariables
		{
			get
			{
				this.CompileExpression();
				return this.referencedVariables;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000625A File Offset: 0x0000445A
		public Expression(string expression, ExpressiveOptions options = ExpressiveOptions.None)
			: this(expression, new Context(options))
		{
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006269 File Offset: 0x00004469
		public Expression(string expression, Context context)
		{
			this.originalExpression = expression;
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			this.context = context;
			this.parser = new ExpressionParser(this.context);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000062A0 File Offset: 0x000044A0
		public object Evaluate(IDictionary<string, object> variables = null)
		{
			object obj;
			try
			{
				this.CompileExpression();
				IExpression expression = this.compiledExpression;
				obj = ((expression != null) ? expression.Evaluate(Expression.ApplyStringComparerSettings(variables, this.context.ParsingStringComparer)) : null);
			}
			catch (Exception ex)
			{
				throw new ExpressiveException(ex);
			}
			return obj;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000062F0 File Offset: 0x000044F0
		public T Evaluate<T>(IDictionary<string, object> variables = null)
		{
			T t;
			try
			{
				t = (T)((object)this.Evaluate(variables));
			}
			catch (ExpressiveException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ExpressiveException(ex);
			}
			return t;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006334 File Offset: 0x00004534
		public object Evaluate(IVariableProvider variableProvider)
		{
			if (variableProvider == null)
			{
				throw new ArgumentNullException("variableProvider");
			}
			return this.Evaluate(new VariableProviderDictionary(variableProvider));
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006350 File Offset: 0x00004550
		public T Evaluate<T>(IVariableProvider variableProvider)
		{
			if (variableProvider == null)
			{
				throw new ArgumentNullException("variableProvider");
			}
			T t;
			try
			{
				t = (T)((object)this.Evaluate(variableProvider));
			}
			catch (ExpressiveException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ExpressiveException(ex);
			}
			return t;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000063A4 File Offset: 0x000045A4
		public void EvaluateAsync(Action<string, object> callback, IDictionary<string, object> variables = null)
		{
			this.EvaluateAsync<object>(callback, variables);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000063AE File Offset: 0x000045AE
		public void EvaluateAsync<T>(Action<string, T> callback, IDictionary<string, object> variables = null)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			ThreadPool.QueueUserWorkItem(delegate(object o)
			{
				T t = default(T);
				string text = null;
				try
				{
					t = this.Evaluate<T>(variables);
				}
				catch (ExpressiveException ex)
				{
					text = ex.Message;
				}
				callback(text, t);
			});
		}

		// Token: 0x060000EF RID: 239 RVA: 0x000063EE File Offset: 0x000045EE
		public void RegisterFunction(string functionName, Func<IExpression[], IDictionary<string, object>, object> function)
		{
			this.context.RegisterFunction(functionName, function, false);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000063FE File Offset: 0x000045FE
		public void RegisterFunction(IFunction function)
		{
			this.context.RegisterFunction(function, false);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000640D File Offset: 0x0000460D
		public void RegisterOperator(IOperator op, bool force = false)
		{
			this.context.RegisterOperator(op, force);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000641C File Offset: 0x0000461C
		public void UnregisterFunction(string functionName)
		{
			this.context.UnregisterFunction(functionName);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000642A File Offset: 0x0000462A
		public void UnregisterOperator(string tag)
		{
			this.context.UnregisterOperator(tag);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00006438 File Offset: 0x00004638
		private void CompileExpression()
		{
			if (this.compiledExpression != null && !this.context.Options.HasFlag(ExpressiveOptions.NoCache))
			{
				return;
			}
			List<string> list = new List<string>();
			this.compiledExpression = this.parser.CompileExpression(this.originalExpression, list);
			this.referencedVariables = list.ToArray();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006498 File Offset: 0x00004698
		private static IDictionary<string, object> ApplyStringComparerSettings(IDictionary<string, object> variables, IEqualityComparer<string> desiredStringComparer)
		{
			if (variables != null)
			{
				Dictionary<string, object> dictionary = variables as Dictionary<string, object>;
				if (dictionary == null)
				{
					if (variables is VariableProviderDictionary)
					{
						return variables;
					}
				}
				else if (dictionary.Comparer.Equals(desiredStringComparer))
				{
					return dictionary;
				}
				return new Dictionary<string, object>(variables, desiredStringComparer);
			}
			return null;
		}

		// Token: 0x0400007E RID: 126
		private IExpression compiledExpression;

		// Token: 0x0400007F RID: 127
		private readonly Context context;

		// Token: 0x04000080 RID: 128
		private readonly string originalExpression;

		// Token: 0x04000081 RID: 129
		private readonly ExpressionParser parser;

		// Token: 0x04000082 RID: 130
		private string[] referencedVariables;
	}
}
