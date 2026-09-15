using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Expressive;
using Expressive.Exceptions;
using Expressive.Expressions;
using LazyBearTechnology;
using Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000234 RID: 564
[Serializable]
public class LazyExpression : LazyExpressionBase
{
	// Token: 0x06000D3D RID: 3389 RVA: 0x0004429B File Offset: 0x0004249B
	public LazyExpression()
	{
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x000442A3 File Offset: 0x000424A3
	public LazyExpression(string expression)
	{
		base.FromString(expression);
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x000442B2 File Offset: 0x000424B2
	protected override void CheckExpressionInit()
	{
		if (this.expression != null || base.HasPureValue)
		{
			return;
		}
		this.expression = new Expression(this.expressionString, LazyExpression.SharedContext);
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x000442DC File Offset: 0x000424DC
	protected override string ParseRegex(string input)
	{
		foreach (string text in LazyExpression.equatings.Keys)
		{
			input = Regex.Replace(input, text, LazyExpression.equatings[text] + "(\"$1\", $2)");
		}
		input = Regex.Replace(input, "\\$(\\w*)", "PPar(\"$1\")");
		input = Regex.Replace(input, "@(\\w*)", "WGOPar(\"$1\")");
		input = Regex.Replace(input, "\\%(\\w*)", "WorldPar(\"$1\")");
		input = Regex.Replace(input, "#(\\w*)", "WorkerPar(\"$1\")");
		return input;
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x00044394 File Offset: 0x00042594
	public float EvaluateFloat(LazyExpressionContext ctx)
	{
		this.context = ctx;
		return this.EvaluateFloatInternal();
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x000443A3 File Offset: 0x000425A3
	public int EvaluateInt(LazyExpressionContext ctx)
	{
		return (int)this.EvaluateFloat(ctx);
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x000443AD File Offset: 0x000425AD
	public bool EvaluateBool(LazyExpressionContext ctx)
	{
		this.context = ctx;
		return this.EvaluateBoolInternal();
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x000443BC File Offset: 0x000425BC
	public string Evaluate(LazyExpressionContext ctx)
	{
		this.context = ctx;
		return this.EvaluateStringInternal();
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x000443CB File Offset: 0x000425CB
	public Item EvaluateItem(LazyExpressionContext ctx)
	{
		this.context = ctx;
		return this.EvaluateItemInternal();
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x000443DA File Offset: 0x000425DA
	public float EvaluateFloat()
	{
		this.context = LazyExpressionContext.Empty;
		return this.EvaluateFloatInternal();
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x000443ED File Offset: 0x000425ED
	public int EvaluateInt()
	{
		return (int)this.EvaluateFloat();
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x000443F6 File Offset: 0x000425F6
	public bool EvaluateBool()
	{
		this.context = LazyExpressionContext.Empty;
		return this.EvaluateBoolInternal();
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x00044409 File Offset: 0x00042609
	public string Evaluate()
	{
		this.context = LazyExpressionContext.Empty;
		return this.EvaluateStringInternal();
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x0004441C File Offset: 0x0004261C
	public int EvaluateInt(WgoData wgoData)
	{
		return this.EvaluateInt(LazyExpressionContext.From(wgoData));
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0004442A File Offset: 0x0004262A
	public int EvaluateInt(ICombatEntity combatEntity)
	{
		return this.EvaluateInt(LazyExpressionContext.From(combatEntity));
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x00044438 File Offset: 0x00042638
	public int EvaluateInt(Item item)
	{
		return this.EvaluateInt(LazyExpressionContext.From(item));
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x00044446 File Offset: 0x00042646
	public int EvaluateInt(ICraftable craftable)
	{
		return this.EvaluateInt(LazyExpressionContext.From(craftable));
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x00044454 File Offset: 0x00042654
	public float EvaluateFloat(WgoData wgoData)
	{
		return this.EvaluateFloat(LazyExpressionContext.From(wgoData));
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00044462 File Offset: 0x00042662
	public float EvaluateFloat(ICraftable craftable)
	{
		return this.EvaluateFloat(LazyExpressionContext.From(craftable));
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x00044470 File Offset: 0x00042670
	public float EvaluateFloat(ICombatEntity combatEntity)
	{
		return this.EvaluateFloat(LazyExpressionContext.From(combatEntity));
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x0004447E File Offset: 0x0004267E
	public float EvaluateFloat(Item item)
	{
		return this.EvaluateFloat(LazyExpressionContext.From(item));
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x0004448C File Offset: 0x0004268C
	public float EvaluateFloat(WorldZoneData worldZoneData)
	{
		return this.EvaluateFloat(LazyExpressionContext.From(worldZoneData));
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0004449A File Offset: 0x0004269A
	public bool EvaluateBool(WgoData wgoData)
	{
		return this.EvaluateBool(LazyExpressionContext.From(wgoData));
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x000444A8 File Offset: 0x000426A8
	public string Evaluate(WgoData wgoData)
	{
		return this.Evaluate(LazyExpressionContext.From(wgoData));
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x000444B6 File Offset: 0x000426B6
	public string Evaluate(Item item)
	{
		return this.Evaluate(LazyExpressionContext.From(item));
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x000444C4 File Offset: 0x000426C4
	public string EvaluateWithAttackerAttacksMe(ICombatEntity combatEntity)
	{
		return this.Evaluate(LazyExpressionContext.WithAttackerAttacksMe(combatEntity));
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x000444D2 File Offset: 0x000426D2
	public string EvaluateValueDelta(int valueDelta)
	{
		return this.Evaluate(LazyExpressionContext.ValueDelta(valueDelta));
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x000444E0 File Offset: 0x000426E0
	public Item EvaluateItem(WgoData wgoData)
	{
		return this.EvaluateItem(LazyExpressionContext.From(wgoData));
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x000444F0 File Offset: 0x000426F0
	private object EvaluateInScope(Action customCallback = null)
	{
		LazyExpressionEvaluationScope.Begin(this.context, customCallback);
		object obj;
		try
		{
			obj = this.expression.Evaluate(null);
		}
		finally
		{
			LazyExpressionEvaluationScope.End();
		}
		return obj;
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x00044530 File Offset: 0x00042730
	private float EvaluateFloatInternal()
	{
		if (!base.HasExpression)
		{
			return this.defaultFloatValue;
		}
		if (base.HasPureValue && this.pureValueType == PureValueType.Float)
		{
			return this.pureValueFloat;
		}
		this.CheckExpressionInit();
		try
		{
			return (float)Convert.ToDecimal(this.EvaluateInScope(null));
		}
		catch (ExpressiveException ex)
		{
			this.HandleEvaluateError(ex, null);
		}
		return this.defaultFloatValue;
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x000445A4 File Offset: 0x000427A4
	private bool EvaluateBoolInternal()
	{
		if (!base.HasExpression)
		{
			return false;
		}
		if (base.HasPureValue && this.pureValueType == PureValueType.Bool)
		{
			return this.pureValueBool;
		}
		if (base.HasPureValue && this.pureValueType == PureValueType.Float)
		{
			return this.pureValueFloat > 0f;
		}
		this.CheckExpressionInit();
		try
		{
			return Convert.ToBoolean(this.EvaluateInScope(null));
		}
		catch (ExpressiveException ex)
		{
			this.HandleEvaluateError(ex, null);
		}
		return false;
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x00044628 File Offset: 0x00042828
	private string EvaluateStringInternal()
	{
		if (!base.HasExpression)
		{
			return string.Empty;
		}
		if (base.HasPureValue && this.pureValueType == PureValueType.String)
		{
			return this.expressionString;
		}
		this.CheckExpressionInit();
		try
		{
			return Convert.ToString(this.EvaluateInScope(null));
		}
		catch (ExpressiveException ex)
		{
			this.HandleEvaluateError(ex, null);
		}
		return string.Empty;
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x00044694 File Offset: 0x00042894
	private Item EvaluateItemInternal()
	{
		if (!base.HasExpression)
		{
			return null;
		}
		this.CheckExpressionInit();
		try
		{
			return this.EvaluateInScope(null) as Item;
		}
		catch (ExpressiveException ex)
		{
			this.HandleEvaluateError(ex, null);
		}
		return null;
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x000446E0 File Offset: 0x000428E0
	public bool EvaluateChance()
	{
		float num = this.EvaluateFloat();
		return num >= 0.01f && num > global::UnityEngine.Random.Range(0f, 1f);
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x00044710 File Offset: 0x00042910
	public bool EvaluateWithCallback(Action callback)
	{
		if (!base.HasExpression)
		{
			return false;
		}
		this.CheckExpressionInit();
		try
		{
			return this.EvaluateInScope(callback) is LazyExpression.CallbackAvailableMarker;
		}
		catch (ExpressiveException ex)
		{
			this.HandleEvaluateError(ex, null);
		}
		return false;
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x00044760 File Offset: 0x00042960
	protected override bool HasExpressionSymptom(string strToCheck)
	{
		return !string.IsNullOrEmpty(strToCheck) && (base.HasExpressionSymptom(strToCheck) || strToCheck.Contains('$') || strToCheck.Contains('@'));
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000D61 RID: 3425 RVA: 0x00044789 File Offset: 0x00042989
	private static Context SharedContext
	{
		get
		{
			if (LazyExpression.s_sharedContext == null)
			{
				LazyExpression.s_sharedContext = new Context(ExpressiveOptions.None);
				LazyExpression.RegisterCustomFunctions(LazyExpression.s_sharedContext);
			}
			return LazyExpression.s_sharedContext;
		}
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x000447AC File Offset: 0x000429AC
	private static void RegisterCustomFunctions(Context ctx)
	{
		ctx.RegisterFunction("If2", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (!pars[0].EvaluateAsBoolean(values))
			{
				return false;
			}
			return pars[1].Evaluate(values);
		}, false);
		ctx.RegisterFunction("True", (IExpression[] pars, IDictionary<string, object> values) => true, false);
		ctx.RegisterFunction("False", (IExpression[] pars, IDictionary<string, object> values) => false, false);
		ctx.RegisterFunction("PPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text = pars[0].EvaluateAsString(values);
			return MainGame.PlayerData.GetRes(text, 0f);
		}, false);
		ctx.RegisterFunction("AddPPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text2 = pars[0].EvaluateAsString(values);
			float num = pars[1].EvaluateAsFloat(values);
			MainGame.PlayerData.AddRes(text2, num);
			return MainGame.PlayerData.GetRes(text2, 0f);
		}, false);
		ctx.RegisterFunction("DecPPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text3 = pars[0].EvaluateAsString(values);
			float num2 = pars[1].EvaluateAsFloat(values);
			MainGame.PlayerData.AddRes(text3, -num2);
			return MainGame.PlayerData.GetRes(text3, 0f);
		}, false);
		ctx.RegisterFunction("SetPPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text4 = pars[0].EvaluateAsString(values);
			float num3 = pars[1].EvaluateAsFloat(values);
			MainGame.PlayerData.SetRes(text4, num3);
			return MainGame.PlayerData.GetRes(text4, 0f);
		}, false);
		ctx.RegisterFunction("WorldPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text5 = pars[0].EvaluateAsString(values);
			return MainGame.Instance.GameSave.WorldData.GetGameRes(text5);
		}, false);
		ctx.RegisterFunction("AddWorldPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text6 = pars[0].EvaluateAsString(values);
			float num4 = pars[1].EvaluateAsFloat(values);
			MainGame.Instance.GameSave.WorldData.AddGameRes(text6, num4);
			return MainGame.Instance.GameSave.WorldData.GetGameRes(text6);
		}, false);
		ctx.RegisterFunction("DecWorldPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text7 = pars[0].EvaluateAsString(values);
			float num5 = pars[1].EvaluateAsFloat(values);
			MainGame.Instance.GameSave.WorldData.AddGameRes(text7, -num5);
			return MainGame.Instance.GameSave.WorldData.GetGameRes(text7);
		}, false);
		ctx.RegisterFunction("MultiplyWorldPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text8 = pars[0].EvaluateAsString(values);
			float num6 = pars[1].EvaluateAsFloat(values);
			MainGame.Instance.GameSave.WorldData.MultiplyGameRes(text8, num6);
			return MainGame.Instance.GameSave.WorldData.GetGameRes(text8);
		}, false);
		ctx.RegisterFunction("DivideWorldPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text9 = pars[0].EvaluateAsString(values);
			float num7 = pars[1].EvaluateAsFloat(values);
			MainGame.Instance.GameSave.WorldData.MultiplyGameRes(text9, 1f / num7);
			return MainGame.Instance.GameSave.WorldData.GetGameRes(text9);
		}, false);
		ctx.RegisterFunction("SetWorldPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text10 = pars[0].EvaluateAsString(values);
			float num8 = pars[1].EvaluateAsFloat(values);
			MainGame.Instance.GameSave.WorldData.SetGameRes(text10, num8);
			return MainGame.Instance.GameSave.WorldData.GetGameRes(text10);
		}, false);
		ctx.RegisterFunction("GetDeltaValue", (IExpression[] pars, IDictionary<string, object> values) => LazyExpressionEvaluationScope.DeltaValue, false);
		ctx.RegisterFunction("AttackerPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			ICombatEntity combatEntity = LazyExpressionEvaluationScope.CombatEntity;
			if (combatEntity != null)
			{
				global::UnityEngine.Object @object = combatEntity as global::UnityEngine.Object;
				if (@object == null || !(@object == null))
				{
					string text11 = pars[0].EvaluateAsString(values);
					return combatEntity.GetCombatEntityGameRes(text11);
				}
			}
			return 0;
		}, false);
		ctx.RegisterFunction("AddInspiration", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text12 = pars[0].EvaluateAsString(values);
			int num9 = pars[1].EvaluateAsInt(values);
			MainGame.Instance.GameSave.talentSystemData.AddToInspiration(text12, num9);
			return true;
		}, false);
		ctx.RegisterFunction("AddTalentValue", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text13 = pars[0].EvaluateAsString(values);
			int num10 = pars[1].EvaluateAsInt(values);
			return MainGame.Instance.GameSave.talentSystemData.AddTalentValue(text13, num10);
		}, false);
		ctx.RegisterFunction("TeleportTo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text14 = pars[0].EvaluateAsString(values);
			string text15 = ((pars.Length > 1) ? pars[1].EvaluateAsString(values) : string.Empty);
			string text16 = ((pars.Length > 2) ? pars[2].EvaluateAsString(values) : string.Empty);
			return PlayerController.Teleport(new WgoTeleportData(text14, text15, text16, false, null, false, 0.3f));
		}, false);
		ctx.RegisterFunction("TeleportToGD", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text17 = pars[0].EvaluateAsString(values);
			string text18 = ((pars.Length > 1) ? pars[1].EvaluateAsString(values) : string.Empty);
			string text19 = ((pars.Length > 2) ? pars[2].EvaluateAsString(values) : string.Empty);
			return PlayerController.Teleport(new GDPointTeleportData(MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(text17), text18, text19, null, false, 0.3f));
		}, false);
		ctx.RegisterFunction("AddPerk", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text20 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.perkSystemData.AddPerk(text20);
			return true;
		}, false);
		ctx.RegisterFunction("RemovePerk", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text21 = pars[0].EvaluateAsString(values);
			bool flag = pars[1].EvaluateAsBoolean(values);
			MainGame.Instance.GameSave.perkSystemData.RemovePerk(text21, flag);
			return true;
		}, false);
		ctx.RegisterFunction("DropBurialRewards", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			Item item = null;
			foreach (Item item2 in LazyExpressionEvaluationScope.WgoData.Inventory.Data.Inventory)
			{
				if (item2.Definition.itemGroupIds.Contains("body"))
				{
					item = item2;
					break;
				}
			}
			if (item == null)
			{
				return false;
			}
			List<Item> list = new List<Item>();
			foreach (Item item3 in item.Inventory)
			{
				if (item3.Definition.itemGroupIds.Contains("burial_reward"))
				{
					list.Add(item3);
				}
			}
			foreach (Item item4 in list)
			{
				MainGame.Instance.dropSystem.DropItem(new Item(item4.id, item4.Count), LazyExpressionEvaluationScope.WgoData.WorldId, LazyExpressionEvaluationScope.WgoData.GetDropPos(item4), null);
			}
			return true;
		}, false);
		ctx.RegisterFunction("GSRun", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			GlobalScriptsManager.RunFlowScript(pars[0].EvaluateAsString(values), null, FlowScriptLoadMode.DeserializeOnInit);
			return true;
		}, false);
		ctx.RegisterFunction("GSFireEvent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text22 = pars[0].EvaluateAsString(values);
			string text23 = pars[1].EvaluateAsString(values);
			GlobalScriptsManager.FireEvent(text22, text23, null);
			return true;
		}, false);
		ctx.RegisterFunction("WGOFireEvent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text24 = pars[0].EvaluateAsString(values);
			string text25 = pars[1].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(text24);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't trigger fire event on wgo with tag:[" + text24 + "]. There is no such wgo!");
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.FireEvent(text25);
			return true;
		}, false);
		ctx.RegisterFunction("FireEventOnWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text26 = pars[0].EvaluateAsString(values);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't trigger fire event on wgo. LazyExpressionEvaluationScope.WgoData is not set!");
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.FireEvent(text26);
			return true;
		}, false);
		ctx.RegisterFunction("AddInteractionEvent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text27 = pars[0].EvaluateAsString(values);
			string text28 = pars[1].EvaluateAsString(values);
			bool flag2 = pars.Length > 2 && pars[2].EvaluateAsBoolean(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(text27);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't add interaction event to wgo with tag:[" + text27 + "]. There is no such wgo!");
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.AddInteractionEvent(text28, flag2);
			return true;
		}, false);
		ctx.RegisterFunction("RemoveInteractionEvent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text29 = pars[0].EvaluateAsString(values);
			string text30 = pars[1].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(text29);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't remove interaction event from wgo with tag:[" + text29 + "]. There is no such wgo!");
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.RemoveInteractionEvent(text30);
			return true;
		}, false);
		ctx.RegisterFunction("SetTimeWithoutSleep", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			float num11 = pars[0].EvaluateAsFloat(values);
			MainGame.Instance.GameSave.playerData.energySystem.timeWithoutSleep = num11;
			return true;
		}, false);
		ctx.RegisterFunction("HasPlayerOvrhdItemByGrp", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string overheadItemGroup = pars[0].EvaluateAsString(values);
			return LazyExpressionEvaluationScope.AnyOverheadItem((Item item) => item.Definition.itemGroupIds.Contains(overheadItemGroup));
		}, false);
		ctx.RegisterFunction("PlayerOvrhdItemIsZombie()", (IExpression[] pars, IDictionary<string, object> values) => LazyExpressionEvaluationScope.AnyOverheadItem((Item item) => MainGame.ZombieSystemData.Cache.ContainsKey(item.UniqueId.Guid)), false);
		ctx.RegisterFunction("HasPlayerOvrhdItem", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string overheadItemId = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(overheadItemId))
			{
				return false;
			}
			return LazyExpressionEvaluationScope.AnyOverheadItem((Item item) => item.id == overheadItemId);
		}, false);
		ctx.RegisterFunction("PlayerHasOvrhdAndCanInsert", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			return LazyExpressionEvaluationScope.AnyOverheadItem((Item item) => LazyExpressionEvaluationScope.WgoData.Inventory.CanAddItemToInventory(item));
		}, false);
		ctx.RegisterFunction("AddItemToWgoInv", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text31 = pars[0].EvaluateAsString(values);
			int num12 = pars[1].EvaluateAsInt(values);
			LazyExpressionEvaluationScope.WgoData.Inventory.Data.AddItemToInventory(new Item(text31, num12), false);
			return true;
		}, false);
		ctx.RegisterFunction("NoOvhdItem", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			PlayerData playerData = MainGame.PlayerData;
			return playerData == null || playerData.HasFreeOverheadSlot;
		}, false);
		ctx.RegisterFunction("HasOverheadWithSkulls", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string skullsName = pars[0].EvaluateAsString(values);
			int skullsCount = pars[1].EvaluateAsInt(values);
			return LazyExpressionEvaluationScope.AnyOverheadItem(delegate(Item item)
			{
				int num13 = 0;
				int num14 = 0;
				foreach (Item item6 in item.Inventory)
				{
					num13 += item6.Definition.redSkulls * item6.Count;
					num14 += item6.Definition.whiteSkulls * item6.Count;
				}
				num13 = Mathf.Clamp(num13, 0, 999);
				num14 = Mathf.Clamp(num14, 0, 999);
				return (skullsName == "red" && num13 >= skullsCount) || (skullsName == "white" && num14 >= skullsCount);
			});
		}, false);
		ctx.RegisterFunction("PlayerHasIntrctSeed", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item interactingItem = MainGame.PlayerData.interactingItem;
			if (interactingItem != null && !string.IsNullOrEmpty(interactingItem.id) && interactingItem.IsSeed)
			{
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("CanPlayerFertilize", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null || LazyExpressionEvaluationScope.WgoData.CraftComponent.IsStarted)
			{
				return false;
			}
			Item interactingItem2 = MainGame.PlayerData.interactingItem;
			if (interactingItem2 == null || !interactingItem2.IsFertilizer)
			{
				return false;
			}
			CraftDef craftDef = null;
			foreach (CraftDef craftDef2 in GameBalance.Me.gardenCraftsPerItemCache[interactingItem2.Definition])
			{
				if (craftDef2.craftsIn.Contains(LazyExpressionEvaluationScope.WgoData.Definition.id))
				{
					craftDef = craftDef2;
					break;
				}
			}
			if (craftDef == null)
			{
				return false;
			}
			foreach (LazyExpression lazyExpression in craftDef.onCraftEndExpressions)
			{
				if (lazyExpression.HasExpression && lazyExpression.expressionString.Contains("AddPerkWgoSelf"))
				{
					string text32 = lazyExpression.expressionString.Replace("AddPerkWgoSelf", "").Trim(new char[] { ')', '(', '"' });
					if (LazyExpressionEvaluationScope.WgoData.HasPerk(text32))
					{
						return false;
					}
				}
			}
			return GardenInteractionHandler.HasFreeFertilizerPerkSlot(LazyExpressionEvaluationScope.WgoData);
		}, false);
		ctx.RegisterFunction("HasPlayerItemInInv", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text33 = pars[0].EvaluateAsString(values);
			int num15 = pars[1].EvaluateAsInt(values);
			return MainGame.PlayerData.Inventory.Data.HasItemQuantityInInventory(text33, num15) || MainGame.PlayerData.toolBeltInventory.Data.HasItemQuantityInInventory(text33, num15);
		}, false);
		ctx.RegisterFunction("HasPlayerItemEquip", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text34 = pars[0].EvaluateAsString(values);
			int num16 = pars[1].EvaluateAsInt(values);
			return MainGame.PlayerData.toolBeltInventory.Data.HasItemQuantityInInventory(text34, num16);
		}, false);
		ctx.RegisterFunction("GetEquippedShovelIcon", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			PlayerData playerData2 = MainGame.PlayerData;
			Item item7;
			if (playerData2 == null)
			{
				item7 = null;
			}
			else
			{
				Inventory toolBeltInventory = playerData2.toolBeltInventory;
				item7 = ((toolBeltInventory != null) ? toolBeltInventory.GetItemByType(ItemType.Shovel) : null);
			}
			Item item8 = item7;
			if (item8 == null || item8.IsEmpty)
			{
				return "i_shovel_0";
			}
			return item8.Definition.iconId;
		}, false);
		ctx.RegisterFunction("IsPlayerControlsEnabled", (IExpression[] pars, IDictionary<string, object> values) => MainGame.PlayerController.IsControlsEnabled, false);
		ctx.RegisterFunction("SetPlayerDamageImmunity", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			bool flag3 = pars[0].EvaluateAsBoolean(values);
			MainGame.PlayerData.hpComponent.IsImmuneToDamage = flag3;
			return true;
		}, false);
		ctx.RegisterFunction("UnlockPhrase", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text35 = pars[0].EvaluateAsString(values);
			if (!string.IsNullOrEmpty(text35))
			{
				MainGame.Instance.GameSave.knowledgeSystem.UnlockPhrase(text35);
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("UnlockHudDaySprite", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text36 = pars[0].EvaluateAsString(values);
			if (!string.IsNullOrEmpty(text36) && !MainGame.Instance.GameSave.knowledgeSystem.unlockedCustomHudDaySprites.Contains(text36))
			{
				MainGame.Instance.GameSave.knowledgeSystem.unlockedCustomHudDaySprites.Add(text36);
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("UnlockCustomizationPart", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text37 = pars[0].EvaluateAsString(values);
			int num17 = pars[1].EvaluateAsInt(values);
			return MainGame.PlayerData.customization.UnlockCustomizationPart(text37, (CustomizablePartType)num17);
		}, false);
		ctx.RegisterFunction("UnlockCustomizationColor", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num18 = pars[0].EvaluateAsInt(values);
			string text38 = pars[1].EvaluateAsString(values);
			int num19 = pars[2].EvaluateAsInt(values);
			return MainGame.PlayerData.customization.UnlockCustomizationColor((PlayerColorCustomizationType)num18, text38, num19);
		}, false);
		ctx.RegisterFunction("SuperUnlockBodyCustomization", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text39 = pars[0].EvaluateAsString(values);
			return MainGame.PlayerData.customization.SuperUnlockBodyCustomization(text39, PlayerSkinHelper.CharacterCustomizationData, LazyExpressionEvaluationScope.Item);
		}, false);
		ctx.RegisterFunction("BlackListPhrase", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text40 = pars[0].EvaluateAsString(values);
			if (!string.IsNullOrEmpty(text40))
			{
				MainGame.Instance.GameSave.knowledgeSystem.AddPhraseToBlackList(text40);
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("RemovePhraseFromBlackList", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text41 = pars[0].EvaluateAsString(values);
			if (!string.IsNullOrEmpty(text41))
			{
				MainGame.Instance.GameSave.knowledgeSystem.RemovePhraseFromBlackList(text41);
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("IncreasePlayerInventory", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num20 = pars[0].EvaluateAsInt(values);
			if (num20 <= 0)
			{
				Debug.LogError("Increase inventory value can not be less or equal to 0");
				return false;
			}
			MainGame.PlayerData.IncreaseInventorySize(num20);
			return false;
		}, false);
		ctx.RegisterFunction("IncreaseOverheadStackLimit", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num21 = pars[0].EvaluateAsInt(values);
			if (num21 <= 0)
			{
				Debug.LogError("Increase overhead stack limit value can not be less or equal to 0");
				return false;
			}
			MainGame.PlayerData.AddRes("extra_overhead", (float)num21);
			return true;
		}, false);
		ctx.RegisterFunction("SetOverheadStackLimit", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num22 = pars[0].EvaluateAsInt(values);
			if (num22 < 1)
			{
				Debug.LogError("Overhead stack limit can not be less than 1");
				return false;
			}
			MainGame.PlayerData.SetRes("extra_overhead", (float)(num22 - 1));
			return true;
		}, false);
		ctx.RegisterFunction("ReducePlayerInventory", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num23 = pars[0].EvaluateAsInt(values);
			if (num23 <= 0)
			{
				Debug.LogError("Reduce inventory value can not be less or equal to 0");
				return false;
			}
			MainGame.PlayerData.ReduceInventorySize(num23);
			return false;
		}, false);
		ctx.RegisterFunction("AddPlayerHP", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num24 = pars[0].EvaluateAsInt(values);
			HPComponent hpComponent = MainGame.PlayerData.hpComponent;
			if (num24 > 0)
			{
				hpComponent.AddHp(num24);
				return true;
			}
			if (num24 >= 0)
			{
				return false;
			}
			hpComponent.ApplyDamage(Mathf.Abs(num24));
			return true;
		}, false);
		ctx.RegisterFunction("InsertOvrhdItem", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			Item overheadItemForMutation = LazyExpressionEvaluationScope.GetOverheadItemForMutation();
			if (overheadItemForMutation == null)
			{
				return false;
			}
			MainGame.PlayerData.InsertOverheadItemTo(LazyExpressionEvaluationScope.WgoData, overheadItemForMutation);
			return true;
		}, false);
		ctx.RegisterFunction("InsertOvrhdItemWithSkulls", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			Item overheadItemForMutation2 = LazyExpressionEvaluationScope.GetOverheadItemForMutation();
			if (overheadItemForMutation2 == null)
			{
				return false;
			}
			MainGame.PlayerData.InsertOverheadItemTo(LazyExpressionEvaluationScope.WgoData, overheadItemForMutation2);
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerInsertBodyWithSkulls, LazyExpressionEvaluationScope.WgoData.id + ":" + overheadItemForMutation2.id);
			return true;
		}, false);
		ctx.RegisterFunction("TakeOvrhdItem", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			if (!MainGame.PlayerData.HasFreeOverheadSlot)
			{
				return false;
			}
			string text42 = pars[0].EvaluateAsString(values);
			List<Item> list2 = LazyExpressionEvaluationScope.WgoData.Inventory.RemoveItemById(text42, 1, null, null, false);
			if (list2.Count > 0)
			{
				return MainGame.PlayerData.TryAddOverheadItemNoReplace(list2[0]);
			}
			return false;
		}, false);
		ctx.RegisterFunction("TakeOvrhdItemByGroup", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			if (!MainGame.PlayerData.HasFreeOverheadSlot)
			{
				return false;
			}
			string text43 = pars[0].EvaluateAsString(values);
			List<Item> list3 = LazyExpressionEvaluationScope.WgoData.Inventory.RemoveItemByGroup(text43, 1);
			if (list3.Count > 0)
			{
				return MainGame.PlayerData.TryAddOverheadItemNoReplace(list3[0]);
			}
			return false;
		}, false);
		ctx.RegisterFunction("HasItemInInvByGroup", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text44 = pars[0].EvaluateAsString(values);
			using (List<Item>.Enumerator enumerator5 = LazyExpressionEvaluationScope.WgoData.Inventory.Data.Inventory.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					if (enumerator5.Current.Definition.itemGroupIds.Contains(text44))
					{
						return true;
					}
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("RemoveItemByGroup", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			WgoData wgoData = LazyExpressionEvaluationScope.WgoData;
			if (wgoData == null)
			{
				return false;
			}
			string text45 = pars[0].EvaluateAsString(values);
			int num25 = pars[1].EvaluateAsInt(values);
			Item item9;
			if (wgoData.Inventory.Data.TryGetItemInInventoryByGroupId(text45, out item9) && item9.Count >= num25)
			{
				wgoData.Inventory.RemoveItemFromInventoryByUID(item9, num25);
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("HasItemInInv", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text46 = pars[0].EvaluateAsString(values);
			int num26 = pars[1].EvaluateAsInt(values);
			return LazyExpressionEvaluationScope.WgoData.Inventory.Data.HasItemQuantityInInventory(text46, num26);
		}, false);
		ctx.RegisterFunction("DropItem", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text47 = pars[0].EvaluateAsString(values);
			int num27 = pars[1].EvaluateAsInt(values);
			PlayerData playerData3 = MainGame.PlayerData;
			MainGame.Instance.dropSystem.DropItem(new Item(text47, num27), MainGame.PlayerData.currentGameSceneId, playerData3.position.Value + new Vector3(playerData3.Direction.x, 0f, playerData3.Direction.y), null);
			return true;
		}, false);
		ctx.RegisterFunction("DropItemByGroup", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text48 = pars[0].EvaluateAsString(values);
			Item item10 = null;
			foreach (Item item11 in LazyExpressionEvaluationScope.WgoData.Inventory.Data.Inventory)
			{
				if (item11.Definition.itemGroupIds.Contains(text48))
				{
					item10 = item11;
					break;
				}
			}
			if (item10 == null)
			{
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.Inventory.RemoveItemFromInventoryByUID(item10, -1);
			Vector3 normalized = (MainGame.PlayerData.position.Value - LazyExpressionEvaluationScope.WgoData.Position).normalized;
			MainGame.Instance.dropSystem.DropItem(item10, LazyExpressionEvaluationScope.WgoData.WorldId, LazyExpressionEvaluationScope.WgoData.Position + normalized, null);
			return true;
		}, false);
		ctx.RegisterFunction("DropHappiness", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num28 = pars[0].EvaluateAsInt(values);
			TechPointsSpawner.CreateSpawner(MainGame.PlayerData.position.Value, 0, 0, 0, num28);
			return true;
		}, false);
		ctx.RegisterFunction("UseItem", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text49 = pars[0].EvaluateAsString(values);
			PlayerData playerData4 = MainGame.PlayerData;
			Item itemById = playerData4.inventory.GetItemById(text49);
			playerData4.UseItem(itemById);
			return true;
		}, false);
		ctx.RegisterFunction("RemoveItem", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text50 = pars[0].EvaluateAsString(values);
			PlayerData playerData5 = MainGame.PlayerData;
			Item itemById2 = playerData5.inventory.GetItemById(text50);
			playerData5.RemoveItem(itemById2);
			return true;
		}, false);
		ctx.RegisterFunction("ChangeWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text51 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.WorldData.ChangeWgoData(LazyExpressionEvaluationScope.WgoData, text51);
			return true;
		}, false);
		ctx.RegisterFunction("SpawnWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text52 = pars[0].EvaluateAsString(values);
			float num29 = pars[1].EvaluateAsFloat(values);
			float num30 = pars[2].EvaluateAsFloat(values);
			float num31 = pars[3].EvaluateAsFloat(values);
			string text53 = pars[4].EvaluateAsString(values);
			string text54 = pars[5].EvaluateAsString(values);
			WgoData wgoData2;
			MainGame.Instance.GameSave.WorldData.AddWgoData(text52, new Vector3(num29, num30, num31), text53, text54, out wgoData2, false);
			return true;
		}, false);
		ctx.RegisterFunction("SpawnWgoOnGDPoint", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text55 = pars[0].EvaluateAsString(values);
			string text56 = pars[1].EvaluateAsString(values);
			string text57 = pars[2].EvaluateAsString(values);
			string text58 = pars[3].EvaluateAsString(values);
			GDPointData gdpointDataById = MainGame.Instance.GameSave.WorldData.gdPointsData.GetGDPointDataById(text56);
			if (gdpointDataById == null)
			{
				Debug.LogError("No gd point:[" + text56 + "] can't spawn wgo");
				return false;
			}
			WgoData wgoData3;
			MainGame.Instance.GameSave.WorldData.AddWgoData(text55, gdpointDataById.Position, text57, text58, out wgoData3, false);
			return true;
		}, false);
		ctx.RegisterFunction("SpawnConveyorWgoOnGDPoint", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text59 = pars[0].EvaluateAsString(values);
			string text60 = pars[1].EvaluateAsString(values);
			string text61 = pars[2].EvaluateAsString(values);
			string text62 = pars[3].EvaluateAsString(values);
			GDPointData gdpointDataById2 = MainGame.Instance.GameSave.WorldData.gdPointsData.GetGDPointDataById(text60);
			if (gdpointDataById2 == null)
			{
				Debug.LogError("No gd point:[" + text60 + "] can't spawn wgo");
				return false;
			}
			WgoData wgoData4;
			MainGame.Instance.GameSave.WorldData.AddWgoData(text59, gdpointDataById2.Position, text61, text62, out wgoData4, false);
			ConveyorWgoData conveyorWgoData = wgoData4 as ConveyorWgoData;
			if (conveyorWgoData != null)
			{
				Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(conveyorWgoData.UniqueId);
				if (wgoViewGlobal != null)
				{
					ConveyorBuildPointer.TryMakeConnectionsOutsideBuildSystem(wgoViewGlobal);
				}
				else
				{
					MainGame.Instance.conveyorSystem.AddConveyorObject(conveyorWgoData.ConveyorComponent);
				}
			}
			return true;
		}, false);
		ctx.RegisterFunction("SpawnConveyorWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text63 = pars[0].EvaluateAsString(values);
			float num32 = pars[1].EvaluateAsFloat(values);
			float num33 = pars[2].EvaluateAsFloat(values);
			float num34 = pars[3].EvaluateAsFloat(values);
			string text64 = pars[4].EvaluateAsString(values);
			string text65 = pars[5].EvaluateAsString(values);
			WgoData wgoData5;
			MainGame.Instance.GameSave.WorldData.AddWgoData(text63, new Vector3(num32, num33, num34), text64, text65, out wgoData5, false);
			ConveyorWgoData conveyorWgoData2 = wgoData5 as ConveyorWgoData;
			if (conveyorWgoData2 != null)
			{
				Wgo wgoViewGlobal2 = GameScene.GetWgoViewGlobal(conveyorWgoData2.UniqueId);
				if (wgoViewGlobal2 != null)
				{
					ConveyorBuildPointer.TryMakeConnectionsOutsideBuildSystem(wgoViewGlobal2);
				}
				else
				{
					MainGame.Instance.conveyorSystem.AddConveyorObject(conveyorWgoData2.ConveyorComponent);
				}
			}
			return true;
		}, false);
		ctx.RegisterFunction("SpawnConveyorWgoOnThis", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text66 = pars[0].EvaluateAsString(values);
			bool flag4 = false;
			if (pars.Length > 1)
			{
				flag4 = pars[1].EvaluateAsBoolean(values);
			}
			WgoData wgoData6 = LazyExpressionEvaluationScope.WgoData;
			if (wgoData6 == null)
			{
				Debug.LogError("Current WgoData is null. Can not spawn ConveyorWgo [" + text66 + "]");
				return false;
			}
			WgoData wgoData7;
			MainGame.Instance.GameSave.WorldData.AddWgoData(text66, new Vector3(wgoData6.Position.x, wgoData6.Position.y, wgoData6.Position.z), wgoData6.WorldId, "", out wgoData7, false);
			if (flag4)
			{
				wgoData7.ApplyWgoPartState(wgoData6.MainWgoPartData.variationId, wgoData6.MainWgoPartData.rotationIndex);
			}
			ConveyorWgoData conveyorWgoData3 = wgoData7 as ConveyorWgoData;
			if (conveyorWgoData3 != null)
			{
				Wgo wgoViewGlobal3 = GameScene.GetWgoViewGlobal(conveyorWgoData3.UniqueId);
				if (wgoViewGlobal3 != null)
				{
					ConveyorBuildPointer.TryMakeConnectionsOutsideBuildSystem(wgoViewGlobal3);
				}
				else
				{
					MainGame.Instance.conveyorSystem.AddConveyorObject(conveyorWgoData3.ConveyorComponent);
				}
			}
			MainGame.WorldData.RemoveWgoDataFromGameScene(wgoData6, true);
			return true;
		}, false);
		ctx.RegisterFunction("ReplaceWgoByIds", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text67 = pars[0].EvaluateAsString(values);
			string text68 = pars[1].EvaluateAsString(values);
			MainGame.Instance.GameSave.WorldData.ChangeWgoData(MainGame.Instance.GameSave.WorldData.GetWgoData(text67), text68);
			return true;
		}, false);
		ctx.RegisterFunction("SetWgoHiddenState", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text69 = pars[0].EvaluateAsString(values);
			bool flag5 = pars[1].EvaluateAsBoolean(values);
			WgoData wgoData8 = MainGame.Instance.GameSave.WorldData.GetWgoData(text69);
			if (wgoData8 == null)
			{
				Debug.LogError(string.Format("Can not set wgo [{0}] hidden state [{1}]. Wgo is null", text69, flag5));
				return false;
			}
			wgoData8.IsHidden = flag5;
			return true;
		}, false);
		ctx.RegisterFunction("SetWgoHiddenStateByTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text70 = pars[0].EvaluateAsString(values);
			bool flag6 = pars[1].EvaluateAsBoolean(values);
			WgoData wgoDataByCustomTag = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text70);
			if (wgoDataByCustomTag == null)
			{
				Debug.LogError(string.Format("Can not set wgo with tag [{0}] hidden state [{1}]. Wgo is null", text70, flag6));
				return false;
			}
			wgoDataByCustomTag.IsHidden = flag6;
			return true;
		}, false);
		ctx.RegisterFunction("CanAddItemToWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text71 = pars[0].EvaluateAsString(values);
			int num35 = pars[1].EvaluateAsInt(values);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			return LazyExpressionEvaluationScope.WgoData.Inventory.CanAddItemToInventory(text71, num35);
		}, false);
		ctx.RegisterFunction("AddWgoPart", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text72 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData.AddWgoPart(text72, "", -1);
			return true;
		}, false);
		ctx.RegisterFunction("RemoveWgoPart", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text73 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData.RemoveWgoPart(text73);
			return true;
		}, false);
		ctx.RegisterFunction("RemoveAllWgoParts", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.RemoveAllWgoParts();
			return true;
		}, false);
		ctx.RegisterFunction("SetWgoPartStateAll", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazyExpressionEvaluationScope.WgoData.ApplyWgoPartState(pars[0].EvaluateAsString(values), (pars.Length > 1) ? pars[1].EvaluateAsInt(values) : LazyExpressionEvaluationScope.WgoData.MainWgoPartData.rotationIndex);
			return true;
		}, false);
		ctx.RegisterFunction("SetWgoVariation", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text74 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.worldData.GetWgoData(text74);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't set variation for wgo with id:[" + text74 + "]. There is no such wgo!");
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.ApplyWgoPartState(pars[1].EvaluateAsString(values), (pars.Length > 2) ? pars[2].EvaluateAsInt(values) : LazyExpressionEvaluationScope.WgoData.MainWgoPartData.rotationIndex);
			return true;
		}, false);
		ctx.RegisterFunction("GetItemByGrp", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text75 = pars[0].EvaluateAsString(values);
			foreach (Item item12 in LazyExpressionEvaluationScope.WgoData.Inventory.Data.Inventory)
			{
				if (item12.Definition.itemGroupIds.Contains(text75))
				{
					return item12;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("GetItemById", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text76 = pars[0].EvaluateAsString(values);
			foreach (Item item13 in LazyExpressionEvaluationScope.WgoData.Inventory.Data.Inventory)
			{
				if (item13.id == text76)
				{
					return item13;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("OpenAutopsyWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			LazyExpression.OpenAutopsyWindow(LazyExpressionEvaluationScope.WgoData, null);
			return true;
		}, false);
		ctx.RegisterFunction("OpenAutopsyWindow_CB", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.worldData.GetWgoData("autopsy_table_1");
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return null;
			}
			bool flag7 = LazyExpressionEvaluationScope.CustomCallback != null;
			Action callback = (flag7 ? LazyExpressionEvaluationScope.CustomCallback : null);
			LazyExpression.OpenAutopsyWindow(LazyExpressionEvaluationScope.WgoData, delegate(UIAutopsyWindowData _)
			{
				Action callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2();
			});
			LazyExpressionEvaluationScope.CustomCallback = null;
			return new LazyExpression.CallbackAvailableMarker();
		}, false);
		ctx.RegisterFunction("OpenResurrectionWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			LazyUI.GetWindow<UIResurrectionWindow>().Open(new UIResurrectionWindowData(LazyExpressionEvaluationScope.WgoData));
			return true;
		}, false);
		ctx.RegisterFunction("OpenDemoEndWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazyUI.GetWindow<UIDemoEndWindow>().Open(null);
			return true;
		}, false);
		ctx.RegisterFunction("OpenCustomizationWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazyExpression.<>c__DisplayClass43_4 CS$<>8__locals5 = new LazyExpression.<>c__DisplayClass43_4();
			UICustomizationWindowData uicustomizationWindowData = new UICustomizationWindowData();
			uicustomizationWindowData.CurrentData = PlayerCustomizationData.Copy(MainGame.PlayerData.customization);
			CS$<>8__locals5.customizationWindow = LazyUI.GetWindow<UICustomizationWindow>();
			CS$<>8__locals5.customizationWindow.Open(uicustomizationWindowData);
			CS$<>8__locals5.customizationWindow.OnCustomizationApplied += CS$<>8__locals5.<RegisterCustomFunctions>g__OnCustomizationApplied|243;
			return true;
		}, false);
		ctx.RegisterFunction("OpenNotesWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazyWindow<UINotesWindowData> window = LazyUI.GetWindow<UINotesWindow>();
			string text77 = pars[0].EvaluateAsString(values);
			UINotesWindowData uinotesWindowData = new UINotesWindowData(GameBalance.Me.GetData<ItemDef>(text77));
			window.Open(uinotesWindowData);
			return true;
		}, false);
		ctx.RegisterFunction("UnlockTechTab", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num36 = pars[0].EvaluateAsInt(values);
			MainGame.Instance.GameSave.knowledgeSystem.UnlockTechTab((TechTreeTab)num36);
			return true;
		}, false);
		ctx.RegisterFunction("LockTechTab", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num37 = pars[0].EvaluateAsInt(values);
			MainGame.Instance.GameSave.knowledgeSystem.LockTechTab((TechTreeTab)num37);
			return true;
		}, false);
		ctx.RegisterFunction("RevealTech", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text78 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.knowledgeSystem.RevealTech(text78);
			return true;
		}, false);
		ctx.RegisterFunction("AddTech", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text79 = pars[0].EvaluateAsString(values);
			GameBalance.Me.GetData<TechDef>(text79).Unlock(true);
			return true;
		}, false);
		ctx.RegisterFunction("RemoveTech", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text80 = pars[0].EvaluateAsString(values);
			GameBalance.Me.GetData<TechDef>(text80).RemoveTech();
			return true;
		}, false);
		ctx.RegisterFunction("IsTechUnlocked", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text81 = pars[0].EvaluateAsString(values);
			return !string.IsNullOrEmpty(text81) && MainGame.Instance.GameSave.knowledgeSystem.unlockedTechs.Contains(text81);
		}, false);
		ctx.RegisterFunction("UnlockCraft", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text82 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.knowledgeSystem.UnlockCraft(text82);
			return true;
		}, false);
		ctx.RegisterFunction("UnlockOrgan", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num38 = pars[0].EvaluateAsInt(values);
			MainGame.Instance.GameSave.knowledgeSystem.UnlockOrgan((ItemType)num38);
			return true;
		}, false);
		ctx.RegisterFunction("UnlockTownBuilding", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text83 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.knowledgeSystem.UnlockTownBuilding(text83);
			return true;
		}, false);
		ctx.RegisterFunction("LockTownBuilding", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text84 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.knowledgeSystem.LockTownBuilding(text84);
			return true;
		}, false);
		ctx.RegisterFunction("UnlockMapFightIcon", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text85 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.knowledgeSystem.UnlockMapFightIcon(text85);
			return true;
		}, false);
		ctx.RegisterFunction("LockMapFightIcon", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text86 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.knowledgeSystem.LockMapFightIcon(text86);
			return true;
		}, false);
		ctx.RegisterFunction("RevealTalentLevelUp", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text87 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.knowledgeSystem.RevealTalentLevelUp(text87);
			return true;
		}, false);
		ctx.RegisterFunction("IsNotCraftingNow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			return !LazyExpressionEvaluationScope.WgoData.CraftComponent.IsStarted;
		}, false);
		ctx.RegisterFunction("AddMoneyToVendor", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text88 = pars[0].EvaluateAsString(values);
			int num39 = pars[1].EvaluateAsInt(values);
			MainGame.Instance.GameSave.vendorSystem.AddMoneyToVendor(text88, num39);
			return true;
		}, false);
		ctx.RegisterFunction("LvlUpVendor", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text89 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.vendorSystem.ForceLevelUpVendor(text89);
			return true;
		}, false);
		ctx.RegisterFunction("ChangeCharTabLockedState", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num40 = pars[0].EvaluateAsInt(values);
			if (pars[1].EvaluateAsBoolean(values))
			{
				MainGame.Instance.GameSave.knowledgeSystem.LockCharTab((CharacterWindowData.CharPage)num40);
			}
			else
			{
				MainGame.Instance.GameSave.knowledgeSystem.UnlockCharTab((CharacterWindowData.CharPage)num40);
			}
			return true;
		}, false);
		ctx.RegisterFunction("AddRepWGO", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text90 = pars[0].EvaluateAsString(values);
			int num41 = pars[1].EvaluateAsInt(values);
			WGODef data = GameBalance.Me.GetData<WGODef>(text90);
			MainGame.Instance.GameSave.playerData.AddNPCRep(data.repResName, num41);
			return true;
		}, false);
		ctx.RegisterFunction("SetRepWGO", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text91 = pars[0].EvaluateAsString(values);
			int num42 = pars[1].EvaluateAsInt(values);
			WGODef data2 = GameBalance.Me.GetData<WGODef>(text91);
			MainGame.Instance.GameSave.playerData.SetNPCRep(data2.repResName, num42);
			return true;
		}, false);
		ctx.RegisterFunction("AddRep", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text92 = pars[0].EvaluateAsString(values);
			int num43 = pars[1].EvaluateAsInt(values);
			MainGame.Instance.GameSave.playerData.AddNPCRep(text92, num43);
			return true;
		}, false);
		ctx.RegisterFunction("SetRep", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text93 = pars[0].EvaluateAsString(values);
			int num44 = pars[1].EvaluateAsInt(values);
			MainGame.Instance.GameSave.playerData.SetNPCRep(text93, num44);
			return true;
		}, false);
		ctx.RegisterFunction("HasCraft", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			return LazyExpressionEvaluationScope.WgoData.CraftComponent.CurrentCraftElement != null;
		}, false);
		ctx.RegisterFunction("StartCraft", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text94 = pars[0].EvaluateAsString(values);
			if (GameBalance.GetCraftDef(text94) == null)
			{
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.CraftComponent.AddToQueue(new CraftElement(text94, 1, new CraftParamsData(text94, LazyExpressionEvaluationScope.WgoData, CraftParamsData.CraftParamsType.Common, -1)), false, -1);
			return true;
		}, false);
		ctx.RegisterFunction("StartCraftOnWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text95 = pars[0].EvaluateAsString(values);
			string text96 = pars[1].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text95) || string.IsNullOrEmpty(text96))
			{
				Debug.LogError("wgoId or craftId is null. wgoId: " + text95 + ", craftId: " + text96);
				return false;
			}
			WgoData wgoData9 = MainGame.Instance.GameSave.WorldData.GetWgoData(text95);
			wgoData9.CraftComponent.AddToQueue(new CraftElement(text96, 1, new CraftParamsData(text96, wgoData9, CraftParamsData.CraftParamsType.Common, -1)), false, -1);
			return true;
		}, false);
		ctx.RegisterFunction("CrIn_SummQualGrd10", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			CraftElementBase craftElementBase = LazyExpressionEvaluationScope.WgoData.CraftComponent.CraftElementsQueue[LazyExpressionEvaluationScope.WgoData.CraftComponent.CraftElementsQueue.Count - 1];
			if (craftElementBase == null)
			{
				return 0;
			}
			string text97 = pars[0].EvaluateAsString(values);
			float num45 = 0f;
			foreach (NeedItemData needItemData in craftElementBase.Requirements)
			{
				if (!needItemData.IsGroup && needItemData.ItemDef != null && needItemData.ItemDef.itemGroupIds.Contains(text97))
				{
					num45 += (float)needItemData.ItemDef.quality;
				}
			}
			return num45 / 10f;
		}, false);
		ctx.RegisterFunction("CrIn_SummQualGrd", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			CraftElementBase craftElementBase2 = LazyExpressionEvaluationScope.WgoData.CraftComponent.CraftElementsQueue[LazyExpressionEvaluationScope.WgoData.CraftComponent.CraftElementsQueue.Count - 1];
			if (craftElementBase2 == null)
			{
				return 0;
			}
			string text98 = pars[0].EvaluateAsString(values);
			float num46 = 0f;
			foreach (NeedItemData needItemData2 in craftElementBase2.Requirements)
			{
				if (!needItemData2.IsGroup && needItemData2.ItemDef != null && needItemData2.ItemDef.itemGroupIds.Contains(text98))
				{
					num46 += (float)needItemData2.ItemDef.quality;
				}
			}
			return num46;
		}, false);
		ctx.RegisterFunction("OpenGardenWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			LazyWindow<UIGardenBedWindowData> window2 = LazyUI.GetWindow<UIGardenBedWindow>();
			UIGardenBedWindowData uigardenBedWindowData = new UIGardenBedWindowData(LazyExpressionEvaluationScope.WgoData);
			window2.Open(uigardenBedWindowData);
			return true;
		}, false);
		ctx.RegisterFunction("OpenSurveyResultWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text99 = pars[0].EvaluateAsString(values);
			SurveyDef surveyDefOrNull = GameBalance.GetSurveyDefOrNull("surv:" + text99);
			CraftElementSurvey craftElementSurvey = LazyExpressionEvaluationScope.WgoData.CraftComponent.CurrentCraftElement as CraftElementSurvey;
			if (craftElementSurvey != null && !string.IsNullOrEmpty(craftElementSurvey.SelectedSurveyItemId))
			{
				text99 = craftElementSurvey.SelectedSurveyItemId;
			}
			else if (surveyDefOrNull != null)
			{
				List<ItemDef> surveyedItemDefs = surveyDefOrNull.GetSurveyedItemDefs();
				if (surveyedItemDefs.Count > 0)
				{
					text99 = surveyedItemDefs[0].id;
				}
			}
			LazyUI.GetWindow<UISurveyResultWindow>().Open(new UISurveyResultWindowData(GameBalance.Me.GetData<ItemDef>(text99)));
			return true;
		}, false);
		ctx.RegisterFunction("TryOpenTechTree", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (!MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.TechTree))
			{
				LazyUI.GetWindow<CharacterWindow>().Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.TechTree, null));
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("TryOpenQuestTree", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (!MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(CharacterWindowData.CharPage.QuestTree))
			{
				LazyUI.GetWindow<CharacterWindow>().Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.QuestTree, null));
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("ShowTechUnlock", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text100 = pars[0].EvaluateAsString(values);
			TechDef data3 = GameBalance.Me.GetData<TechDef>(text100);
			if (!MainGame.Instance.GameSave.knowledgeSystem.IsTechUnlocked(data3.id))
			{
				Debug.LogError("Tech:[" + text100 + "] is not unlocked!!!");
				return false;
			}
			if (data3.techDefType != TechDefType.Common)
			{
				Debug.LogError("Tech:[" + text100 + "] is not available to show unlock window!!!");
				return false;
			}
			LazyWindow<UITechTreeElementWindowData> window3 = LazyUI.GetWindow<UITechTreeElementWindow>();
			UIDialogWindowData.ButtonData buttonData = new UIDialogWindowData.ButtonData(new Action(LazyUI.GetWindow<UITechTreeElementWindow>().Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
			window3.Open(new UITechTreeElementWindowData(data3, new List<UIDialogWindowData.ButtonData> { buttonData }, null, "ui_new_tech_unlocked"));
			return true;
		}, false);
		ctx.RegisterFunction("AddWGOPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text101 = pars[0].EvaluateAsString(values);
			float num47 = pars[1].EvaluateAsFloat(values);
			LazyExpressionEvaluationScope.WgoData.AddGameRes(text101, num47);
			return true;
		}, false);
		ctx.RegisterFunction("DecWGOPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text102 = pars[0].EvaluateAsString(values);
			float num48 = pars[1].EvaluateAsFloat(values);
			LazyExpressionEvaluationScope.WgoData.AddGameRes(text102, -num48);
			return true;
		}, false);
		ctx.RegisterFunction("MultiplyWGOPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text103 = pars[0].EvaluateAsString(values);
			float num49 = pars[1].EvaluateAsFloat(values);
			LazyExpressionEvaluationScope.WgoData.MultiplyGameRes(text103, num49);
			return true;
		}, false);
		ctx.RegisterFunction("DivideWGOPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text104 = pars[0].EvaluateAsString(values);
			float num50 = pars[1].EvaluateAsFloat(values);
			LazyExpressionEvaluationScope.WgoData.MultiplyGameRes(text104, 1f / num50);
			return true;
		}, false);
		ctx.RegisterFunction("SetWGOPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text105 = pars[0].EvaluateAsString(values);
			float num51 = pars[1].EvaluateAsFloat(values);
			LazyExpressionEvaluationScope.WgoData.SetGameRes(text105, num51);
			return true;
		}, false);
		ctx.RegisterFunction("WGOPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			string text106 = pars[0].EvaluateAsString(values);
			return LazyExpressionEvaluationScope.WgoData.GetGameRes(text106);
		}, false);
		ctx.RegisterFunction("SetWGOParByTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text107 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text107);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't set par to wgo with tag:[" + text107 + "]. There is no such wgo!");
				return false;
			}
			string text108 = pars[1].EvaluateAsString(values);
			float num52 = pars[2].EvaluateAsFloat(values);
			LazyExpressionEvaluationScope.WgoData.SetGameRes(text108, num52);
			return true;
		}, false);
		ctx.RegisterFunction("Spawning", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			string craftId = LazyExpressionEvaluationScope.WgoData.CraftComponent.CurrentCraftElement.CraftId;
			SpawnStage spawnStage = LazyExpressionEvaluationScope.WgoData.SpawnWGOComponent.FindStageById(craftId);
			if (spawnStage == null)
			{
				Debug.LogError("Spawning null stage for id:[" + craftId + "]!!!");
				return false;
			}
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.worldData.ReplaceWgoData(LazyExpressionEvaluationScope.WgoData, spawnStage.variations.GetRandom<SpawnVariation>().wgoPartId);
			if (!string.IsNullOrEmpty(spawnStage.craftId))
			{
				if (GameBalance.GetCraftDef(spawnStage.craftId) == null)
				{
					return false;
				}
				LazyExpressionEvaluationScope.WgoData.CraftComponent.AddToQueue(new CraftElement(spawnStage.craftId, 1, new CraftParamsData(spawnStage.craftId, LazyExpressionEvaluationScope.WgoData, CraftParamsData.CraftParamsType.Common, -1)), false, -1);
			}
			return true;
		}, false);
		ctx.RegisterFunction("SetRandomSpawnWgoStages", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			List<string> list4 = new List<string>();
			foreach (IExpression expression in pars)
			{
				list4.Add(expression.EvaluateAsString(values));
			}
			string random = list4.GetRandom<string>();
			AsyncOperationHandle<SpawnConfiguration> asyncOperationHandle = Addressables.LoadAssetAsync<SpawnConfiguration>("Assets/AddressableAssets/SpawnerConfigurations/" + random + ".asset");
			SpawnConfiguration spawnConfiguration = asyncOperationHandle.WaitForCompletion();
			if (spawnConfiguration == null)
			{
				Debug.LogError("Can not find SpawnConfiguration with id " + random);
				return false;
			}
			List<SpawnStage> list5 = new List<SpawnStage>();
			list5.AddRange(spawnConfiguration.SpawnStages);
			LazyExpressionEvaluationScope.WgoData.SpawnWGOComponent.SpawnStages = list5;
			asyncOperationHandle.Release();
			return true;
		}, false);
		ctx.RegisterFunction("SetRandomVariation", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			LazyExpressionEvaluationScope.WgoData.ApplyRandomState();
			return true;
		}, false);
		ctx.RegisterFunction("AddPerkWgoSelf", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			string text109 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData.AddPerk(text109);
			return true;
		}, false);
		ctx.RegisterFunction("RemovePerkWgoSelf", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			string text110 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData.RemovePerk(text110);
			return true;
		}, false);
		ctx.RegisterFunction("RemoveAllPerksWgoSelf", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			LazyExpressionEvaluationScope.WgoData.RemoveAllPerks();
			return true;
		}, false);
		ctx.RegisterFunction("AddPerkToWgoByTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text111 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text111);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't add perk to wgo with tag:[" + text111 + "]. There is no such wgo!");
				return false;
			}
			string text112 = pars[1].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData.AddPerk(text112);
			return true;
		}, false);
		ctx.RegisterFunction("RemovePerkFromWgoByTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text113 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text113);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't remove perk from wgo with tag:[" + text113 + "]. There is no such wgo!");
				return false;
			}
			string text114 = pars[1].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData.RemovePerk(text114);
			return true;
		}, false);
		ctx.RegisterFunction("SetWgoInteractionState", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text115 = pars[0].EvaluateAsString(values);
			bool flag8 = pars[1].EvaluateAsBoolean(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text115);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError(string.Format("Can't set interactableState with tag: [{0}] to: [{1}]. There is no such wgo!", text115, flag8));
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.IsInteractable = flag8;
			return true;
		}, false);
		ctx.RegisterFunction("SetWgoGameResInt", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text116 = pars[0].EvaluateAsString(values);
			string text117 = pars[1].EvaluateAsString(values);
			int num53 = pars[2].EvaluateAsInt(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text116);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't set res with tag: [" + text116 + "]. There is no such wgo!");
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.SetGameRes(text117, num53);
			return true;
		}, false);
		ctx.RegisterFunction("DestroyWgoData", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text118 = pars[0].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text118);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Can't destroy wgo data with tag: [" + text118 + "]. There is no such wgo!");
				return false;
			}
			MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(LazyExpressionEvaluationScope.WgoData, true);
			return true;
		}, false);
		ctx.RegisterFunction("CreateTownBuilding", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			MainGame.Instance.GameSave.townSystem.CreateTownBuildingOnWgo(LazyExpressionEvaluationScope.WgoData);
			return true;
		}, false);
		ctx.RegisterFunction("DestroySignboard", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			Debug.Log(string.Format("Destroying signboard for wgo:[{0}]", LazyExpressionEvaluationScope.WgoData.UniqueId));
			MainGame.WorldData.RemoveWgoDataFromGameScene(LazyExpressionEvaluationScope.WgoData.UniqueId);
			return true;
		}, false);
		ctx.RegisterFunction("RepairHousesById", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text119 = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text119))
			{
				Debug.LogError("[RepairHousesById] WSO ID is null or empty");
				return false;
			}
			int num54 = ((pars.Length != 0) ? pars[1].EvaluateAsInt(values) : (-1));
			List<WsoData> list6;
			if (MainGame.Instance.GameSave.worldData.Cache.wsoDataByIdCache.TryGetValue(text119, out list6))
			{
				foreach (WsoData wsoData in list6)
				{
					TownUtils.RepairHouse(wsoData, num54);
				}
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("RepairHousesByTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text120 = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text120))
			{
				Debug.LogError("[RepairHousesById] WSO tag is null or empty");
				return false;
			}
			int num55 = ((pars.Length != 0) ? pars[1].EvaluateAsInt(values) : (-1));
			List<WsoData> list7;
			if (MainGame.Instance.GameSave.worldData.Cache.wsoDataByCustomTagsCache.TryGetValue(text120, out list7))
			{
				foreach (WsoData wsoData2 in list7)
				{
					TownUtils.RepairHouse(wsoData2, num55);
				}
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("SetResWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text121 = pars[0].EvaluateAsString(values);
			string text122 = pars[1].EvaluateAsString(values);
			float num56 = pars[2].EvaluateAsFloat(values);
			if (string.IsNullOrEmpty(text121))
			{
				Debug.LogError("WGO ID is null or empty");
				return false;
			}
			List<WgoData> list8;
			if (MainGame.Instance.GameSave.worldData.Cache.wgoDataByIdsCache.TryGetValue(text121, out list8))
			{
				list8[0].SetGameRes(text122, num56);
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("LazyExpressionOnWgo", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text123 = pars[0].EvaluateAsString(values);
			string text124 = pars[1].EvaluateAsString(values);
			LazyExpressionEvaluationScope.WgoData = MainGame.Instance.GameSave.WorldData.GetWgoDataByCustomTag(text123);
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				Debug.LogError("Unable to call LE on wgo data with custom tag: [" + text123 + "]. There is no such wgo!");
				return false;
			}
			return new LazyExpression(text124).EvaluateBool(LazyExpressionEvaluationScope.WgoData);
		}, false);
		ctx.RegisterFunction("TryPlacePlantOrder", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			return GardenInteractionHandler.TryPlacePlantOrder(LazyExpressionEvaluationScope.WgoData);
		}, false);
		ctx.RegisterFunction("TryPlaceGatherOrder", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			return GardenInteractionHandler.TryPlaceGatherOrder(LazyExpressionEvaluationScope.WgoData);
		}, false);
		ctx.RegisterFunction("TryFinishPanicReductionMachineCraft", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			WgoData wgoDataByCustomTag2 = MainGame.WorldData.GetWgoDataByCustomTag("panic_reduction_machine");
			if (wgoDataByCustomTag2 == null)
			{
				Debug.LogWarning("Can wgo with tag [panic_reduction_machine] to finish it craft");
				return false;
			}
			if (wgoDataByCustomTag2.CraftComponent.CurrentCraftElement != null)
			{
				wgoDataByCustomTag2.CraftComponent.TryFinishCurCraft();
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("KillerIsPlayer", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.CombatEntityAttacksMe == null)
			{
				return 0;
			}
			ICombatEntity combatEntityAttacksMe = LazyExpressionEvaluationScope.CombatEntityAttacksMe;
			if (combatEntityAttacksMe != null)
			{
				global::UnityEngine.Object object2 = combatEntityAttacksMe as global::UnityEngine.Object;
				if (object2 == null || !(object2 == null))
				{
					return (combatEntityAttacksMe is PlayerPhysicalBody) ? 1 : 0;
				}
			}
			return 0;
		}, false);
		ctx.RegisterFunction("GetChurchObjectQuality", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			WgoData wgoData10 = LazyExpressionEvaluationScope.WgoData;
			if (wgoData10 == null)
			{
				return 0;
			}
			int num57 = 0;
			string id = wgoData10.id;
			if (!(id == "zmb_choir_place"))
			{
				if (id == "zmb_organ_place")
				{
					num57 += ConstDef.Get("church_organ_default_quality").IntValue;
				}
			}
			else
			{
				num57 += ConstDef.Get("church_choir_default_quality").IntValue;
			}
			foreach (Item item14 in wgoData10.Inventory.GetItemsByGroupId("zombie"))
			{
				ZombieWgoData zombie = MainGame.Instance.GameSave.zombieSystemData.GetZombie(item14.UniqueId);
				if (zombie != null)
				{
					if (zombie.GetGameResInt("zperk_musician_master") > 0)
					{
						num57 += 7;
					}
					else if (zombie.GetGameResInt("zperk_musician_advanced") > 0)
					{
						num57 += 4;
					}
					else if (zombie.GetGameResInt("zperk_musician_basic") > 0)
					{
						num57 += 2;
					}
					else
					{
						num57++;
					}
				}
			}
			return num57;
		}, false);
		ctx.RegisterFunction("AddCustomRectToWZ", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text125 = pars[0].EvaluateAsString(values);
			int num58 = pars[1].EvaluateAsInt(values);
			int num59 = pars[2].EvaluateAsInt(values);
			WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.worldData.GetWorldZoneDataById(text125);
			if (worldZoneDataById == null)
			{
				return false;
			}
			Vector2 vector = new Vector2(LazyExpressionEvaluationScope.WgoData.Position.x, LazyExpressionEvaluationScope.WgoData.Position.z);
			Rect rect = new Rect(vector, Vector2.Scale(BuildConsts.BUILD_GRID_SIZE_WORLD_UNIT, new Vector2((float)num58, (float)num59)));
			worldZoneDataById.AddCustomQualityRect(rect);
			return true;
		}, false);
		ctx.RegisterFunction("RemoveCustomRectFromWZ", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			string text126 = pars[0].EvaluateAsString(values);
			int num60 = pars[1].EvaluateAsInt(values);
			int num61 = pars[2].EvaluateAsInt(values);
			WorldZoneData worldZoneDataById2 = MainGame.Instance.GameSave.worldData.GetWorldZoneDataById(text126);
			if (worldZoneDataById2 == null)
			{
				return false;
			}
			Vector2 vector2 = new Vector2(LazyExpressionEvaluationScope.WgoData.Position.x, LazyExpressionEvaluationScope.WgoData.Position.z);
			Rect rect2 = new Rect(vector2, Vector2.Scale(BuildConsts.BUILD_GRID_SIZE_WORLD_UNIT, new Vector2((float)num60, (float)num61)));
			return worldZoneDataById2.RemoveCustomQualityRect(rect2);
		}, false);
		ctx.RegisterFunction("WZQual", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text127 = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text127))
			{
				Debug.LogError("WorldZone id is null or empty");
				return 0f;
			}
			WorldZoneData worldZoneDataById3 = MainGame.Instance.GameSave.worldData.GetWorldZoneDataById(text127);
			if (worldZoneDataById3 == null)
			{
				Debug.LogError("WorldZone by id [" + text127 + "] not found");
				return 0f;
			}
			return worldZoneDataById3.GetTotalQuality();
		}, false);
		ctx.RegisterFunction("AddTownQuality", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int num62 = pars[0].EvaluateAsInt(values);
			MainGame.Instance.GameSave.townSystem.Quality += num62;
			return true;
		}, false);
		ctx.RegisterFunction("WZQualPowerSource", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WorldZoneData == null)
			{
				return 0;
			}
			return LazyExpressionEvaluationScope.WorldZoneData.GetTotalQuality(WorldZoneWgoQualityType.ConveyorPowerSource);
		}, false);
		ctx.RegisterFunction("WZQualConveyorCells", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WorldZoneData == null)
			{
				return 0;
			}
			return LazyExpressionEvaluationScope.WorldZoneData.GetTotalQuality(WorldZoneWgoQualityType.ConveyorCells);
		}, false);
		ctx.RegisterFunction("WZAddAdditionalQuality", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text128 = pars[0].EvaluateAsString(values);
			int num63 = pars[1].EvaluateAsInt(values);
			if (string.IsNullOrEmpty(text128))
			{
				Debug.LogError("WorldZone id is null or empty");
				return 0f;
			}
			WorldZoneData worldZoneDataById4 = MainGame.Instance.GameSave.worldData.GetWorldZoneDataById(text128);
			if (worldZoneDataById4 == null)
			{
				Debug.LogError("WorldZone by id [" + text128 + "] not found");
				return 0f;
			}
			worldZoneDataById4.AdditionalQuality += num63;
			return true;
		}, false);
		ctx.RegisterFunction("IsBuildPlaceUnlocked", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text129 = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text129))
			{
				return true;
			}
			if (MainGame.Instance.GameSave.knowledgeSystem.unlockedBuildings.Contains(text129))
			{
				return true;
			}
			return false;
		}, false);
		ctx.RegisterFunction("IsBuildingLocked", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text130 = pars[0].EvaluateAsString(values);
			return !string.IsNullOrEmpty(text130) && MainGame.Instance.GameSave.knowledgeSystem.lockedBuildings.Contains(text130);
		}, false);
		ctx.RegisterFunction("UnlockBuilding", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text131 = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text131))
			{
				Debug.LogError("Can't unlock building " + text131 + ", it's value a null or empty");
				return false;
			}
			MainGame.Instance.GameSave.knowledgeSystem.UnlockBuilding(text131);
			return true;
		}, false);
		ctx.RegisterFunction("LockBuilding", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text132 = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text132))
			{
				Debug.LogError("Can't lock building " + text132 + ", it's value a null or empty");
				return false;
			}
			MainGame.Instance.GameSave.knowledgeSystem.LockBuilding(text132);
			return true;
		}, false);
		ctx.RegisterFunction("AddToKnownMapZones", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text133 = pars[0].EvaluateAsString(values);
			if (string.IsNullOrEmpty(text133))
			{
				return false;
			}
			if (MainGame.Instance.GameSave.knowledgeSystem.knownMapZones.Contains(text133))
			{
				return false;
			}
			MainGame.Instance.GameSave.knowledgeSystem.knownMapZones.Add(text133);
			return true;
		}, false);
		ctx.RegisterFunction("StartQuest", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text134 = pars[0].EvaluateAsString(values);
			float num64 = 0f;
			if (pars.Length > 1)
			{
				num64 = pars[1].EvaluateAsFloat(values);
			}
			MainGame.Instance.GameSave.questSystemData.StartQuest(text134, num64);
			return true;
		}, false);
		ctx.RegisterFunction("StartQuestDemoDependent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text135 = pars[0].EvaluateAsString(values);
			pars[1].EvaluateAsString(values);
			float num65 = 0f;
			if (pars.Length > 2)
			{
				num65 = pars[2].EvaluateAsFloat(values);
			}
			MainGame.Instance.GameSave.questSystemData.StartQuest(text135, num65);
			return true;
		}, false);
		ctx.RegisterFunction("AwaitQuest", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text136 = pars[0].EvaluateAsString(values);
			float num66 = 0f;
			if (pars.Length > 1)
			{
				num66 = pars[1].EvaluateAsFloat(values);
			}
			MainGame.Instance.GameSave.questSystemData.AwaitQuest(text136, num66);
			return true;
		}, false);
		ctx.RegisterFunction("AwaitQuestDemoDependent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text137 = pars[0].EvaluateAsString(values);
			pars[1].EvaluateAsString(values);
			float num67 = 0f;
			if (pars.Length > 2)
			{
				num67 = pars[2].EvaluateAsFloat(values);
			}
			MainGame.Instance.GameSave.questSystemData.AwaitQuest(text137, num67);
			return true;
		}, false);
		ctx.RegisterFunction("CompleteQuest", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text138 = pars[0].EvaluateAsString(values);
			float num68 = 0f;
			if (pars.Length > 1)
			{
				num68 = pars[1].EvaluateAsFloat(values);
			}
			MainGame.Instance.GameSave.questSystemData.CompleteQuest(text138, num68);
			return true;
		}, false);
		ctx.RegisterFunction("CompleteQuestDemoDependent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text139 = pars[0].EvaluateAsString(values);
			pars[1].EvaluateAsString(values);
			float num69 = 0f;
			if (pars.Length > 2)
			{
				num69 = pars[2].EvaluateAsFloat(values);
			}
			MainGame.Instance.GameSave.questSystemData.CompleteQuest(text139, num69);
			return true;
		}, false);
		ctx.RegisterFunction("CancelQuest", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text140 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.questSystemData.CancelQuest(text140);
			return true;
		}, false);
		ctx.RegisterFunction("ChangeQuestHiddenState", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text141 = pars[0].EvaluateAsString(values);
			bool flag9 = pars[1].EvaluateAsBoolean(values);
			MainGame.Instance.GameSave.questSystemData.ChangeQuestHiddenState(text141, !flag9);
			return true;
		}, false);
		ctx.RegisterFunction("ChangeQuestUnknownState", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text142 = pars[0].EvaluateAsString(values);
			bool flag10 = pars[1].EvaluateAsBoolean(values);
			MainGame.Instance.GameSave.questSystemData.ChangeQuestUnknownState(text142, !flag10);
			return true;
		}, false);
		ctx.RegisterFunction("IsQuestCompleted", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text143 = pars[0].EvaluateAsString(values);
			return MainGame.Instance.GameSave.questSystemData.IsQuestInStatus(text143, QuestStatus.Completed);
		}, false);
		ctx.RegisterFunction("ShowTutorialWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			UITutorialWindowData uitutorialWindowData = new UITutorialWindowData(pars[0].EvaluateAsString(values), null, false);
			LazyUI.GetWindow<UITutorialWindow>().Open(uitutorialWindowData);
			return true;
		}, false);
		ctx.RegisterFunction("ShowTutorialWindowWithCallback", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text144 = pars[0].EvaluateAsString(values);
			string exprOnClose = pars[1].EvaluateAsString(values);
			UITutorialWindowData uitutorialWindowData2 = new UITutorialWindowData(text144, null, false);
			uitutorialWindowData2.OnCompleteCallback = delegate
			{
				new LazyExpression(exprOnClose).Evaluate();
			};
			LazyUI.GetWindow<UITutorialWindow>().Open(uitutorialWindowData2);
			return true;
		}, false);
		ctx.RegisterFunction("AttachArrowTutorial", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text145 = pars[0].EvaluateAsString(values);
			LazySingleton<UITutorialArrow>.Instance.Attach(text145);
			return true;
		}, false);
		ctx.RegisterFunction("UnAttachArrowTutorial", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazySingleton<UITutorialArrow>.Instance.UnAttach();
			return true;
		}, false);
		ctx.RegisterFunction("EnableTutorialMode", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			List<string> list9 = new List<string>();
			for (int j = 0; j < pars.Length; j++)
			{
				string text146 = pars[j].EvaluateAsString(values);
				if (!string.IsNullOrEmpty(text146))
				{
					WgoData wgoDataByCustomTag3 = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(text146);
					if (wgoDataByCustomTag3 != null)
					{
						list9.Add(wgoDataByCustomTag3.UniqueId.Id);
					}
				}
			}
			MainGame.PlayerData.SetTutorialModeState(true);
			MainGame.PlayerData.AddToTutorialModeExcludedList(list9);
			return true;
		}, false);
		ctx.RegisterFunction("DisableTutorialMode", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			MainGame.PlayerData.SetTutorialModeState(false);
			return true;
		}, false);
		ctx.RegisterFunction("TutorialModeRemoveFromExclude", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			List<string> list10 = new List<string>();
			for (int k = 0; k < pars.Length; k++)
			{
				string text147 = pars[k].EvaluateAsString(values);
				if (!string.IsNullOrEmpty(text147))
				{
					WgoData wgoDataByCustomTag4 = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(text147);
					if (wgoDataByCustomTag4 != null)
					{
						list10.Add(wgoDataByCustomTag4.UniqueId.Id);
					}
				}
			}
			MainGame.PlayerData.RemoveFromTutorialModeExcludedList(list10);
			return true;
		}, false);
		ctx.RegisterFunction("TutorialModeAddToExclude", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			List<string> list11 = new List<string>();
			for (int l = 0; l < pars.Length; l++)
			{
				string text148 = pars[l].EvaluateAsString(values);
				if (!string.IsNullOrEmpty(text148))
				{
					WgoData wgoDataByCustomTag5 = MainGame.Instance.GameSave.worldData.GetWgoDataByCustomTag(text148);
					if (wgoDataByCustomTag5 != null)
					{
						list11.Add(wgoDataByCustomTag5.UniqueId.Id);
					}
				}
			}
			MainGame.PlayerData.AddToTutorialModeExcludedList(list11);
			return true;
		}, false);
		ctx.RegisterFunction("TrySetOrRemoveResurrectionDebuff", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			int resInt = MainGame.PlayerData.GetResInt("cur_zombies_count");
			int num70 = (int)MainGame.WorldData.GetWorldZoneDataById("resurrection").GetTotalQuality();
			if (resInt <= num70)
			{
				MainGame.Instance.GameSave.perkSystemData.RemovePerk("debuff_excessive_zombie", false);
			}
			else
			{
				MainGame.Instance.GameSave.perkSystemData.AddPerk("debuff_excessive_zombie");
			}
			return true;
		}, false);
		ctx.RegisterFunction("SetWeatherState", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text149 = pars[0].EvaluateAsString(values);
			WeatherSystem.Instance.SetWeatherState(text149, true);
			return true;
		}, false);
		ctx.RegisterFunction("ResetWeatherState", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			WeatherSystem.Instance.ResetWeatherState();
			return true;
		}, false);
		ctx.RegisterFunction("SetWeatherComponent", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text150 = pars[0].EvaluateAsString(values);
			bool flag11 = pars[1].EvaluateAsBoolean(values);
			WeatherSystem.Instance.SetWeatherComponent(text150, flag11);
			return true;
		}, false);
		ctx.RegisterFunction("SetPostProcessProfile", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (pars.Length < 1)
			{
				return false;
			}
			string text151 = pars[0].EvaluateAsString(values);
			CameraSystem.Instance.MainCamera.SetPostProcessProfile(text151);
			return true;
		}, false);
		ctx.RegisterFunction("ResetPostProcessProfile", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			CameraSystem.Instance.MainCamera.ResetPostProcessProfile();
			return true;
		}, false);
		ctx.RegisterFunction("SetLightEnvironmentOverride", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (pars.Length < 1)
			{
				return false;
			}
			string text152 = pars[0].EvaluateAsString(values);
			EnvironmentEngine.Instance.ApplyOverridePreset(text152, 1f);
			return true;
		}, false);
		ctx.RegisterFunction("ResetLightEnvironmentOverride", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			EnvironmentEngine.Instance.ApplyOverridePreset(null, 0f);
			return true;
		}, false);
		ctx.RegisterFunction("CloseUIWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			IUIWindowCustomOperable windowByInterface = LazyUI.GetWindowByInterface<IUIWindowCustomOperable>(pars[0].EvaluateAsString(values));
			if (windowByInterface == null)
			{
				return false;
			}
			windowByInterface.Close();
			return true;
		}, false);
		ctx.RegisterFunction("IsShownUIWindow", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			IUIWindowCustomOperable windowByInterface2 = LazyUI.GetWindowByInterface<IUIWindowCustomOperable>(pars[0].EvaluateAsString(values));
			if (windowByInterface2 == null)
			{
				return false;
			}
			return windowByInterface2.IsShown;
		}, false);
		ctx.RegisterFunction("ShowTextNotification", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text153 = pars[0].EvaluateAsString(values);
			LazySingleton<UINotificator>.Instance.ShowSimpleTextNotification(text153);
			return true;
		}, false);
		ctx.RegisterFunction("ResetVendors", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			MainGame.Instance.GameSave.townSystem.ResetVendors();
			return true;
		}, false);
		ctx.RegisterFunction("ClearTownPalettes", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			MainGame.Instance.GameSave.townSystem.ClearTownPalettes();
			return true;
		}, false);
		ctx.RegisterFunction("AddOrderSlot", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			MainGame.Instance.GameSave.vendorSystem.currentOrders.Add(SGuid.Empty);
			return true;
		}, false);
		ctx.RegisterFunction("RepairTownCluster", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			LazyExpressionEvaluationScope.WgoData.TownClusterRepairWgoComponent.DoRepairLogic();
			return true;
		}, false);
		ctx.RegisterFunction("EnableGDPointById", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text154 = pars[0].EvaluateAsString(values);
			foreach (GDPointData gdpointData in MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointsDataById(text154))
			{
				gdpointData.Enabled = true;
			}
			return true;
		}, false);
		ctx.RegisterFunction("EnableGDPointByTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text155 = pars[0].EvaluateAsString(values);
			foreach (GDPointData gdpointData2 in MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointsDataByCustomTag(text155))
			{
				gdpointData2.Enabled = true;
			}
			return true;
		}, false);
		ctx.RegisterFunction("DisableGDPointById", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text156 = pars[0].EvaluateAsString(values);
			foreach (GDPointData gdpointData3 in MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointsDataById(text156))
			{
				gdpointData3.Enabled = false;
			}
			return true;
		}, false);
		ctx.RegisterFunction("DisableGDPointByTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text157 = pars[0].EvaluateAsString(values);
			foreach (GDPointData gdpointData4 in MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointsDataByCustomTag(text157))
			{
				gdpointData4.Enabled = false;
			}
			return true;
		}, false);
		ctx.RegisterFunction("GetWeapon", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return new ItemDef();
			}
			ZombieWgoData zombieWgoData = LazyExpressionEvaluationScope.WgoData as ZombieWgoData;
			if (zombieWgoData != null)
			{
				return zombieWgoData.WorkerToolInventory.GetItemByTypes(new ItemType[]
				{
					ItemType.Bow,
					ItemType.Pike,
					ItemType.Sword
				});
			}
			return LazyExpressionEvaluationScope.WgoData.Inventory.GetItemByTypes(new ItemType[]
			{
				ItemType.Bow,
				ItemType.Pike,
				ItemType.Sword
			});
		}, false);
		ctx.RegisterFunction("GetArmor", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return new ItemDef();
			}
			ZombieWgoData zombieWgoData2 = LazyExpressionEvaluationScope.WgoData as ZombieWgoData;
			if (zombieWgoData2 != null)
			{
				return zombieWgoData2.WorkerToolInventory.GetItemByType(ItemType.BodyArmor);
			}
			return LazyExpressionEvaluationScope.WgoData.Inventory.GetItemByType(ItemType.BodyArmor);
		}, false);
		ctx.RegisterFunction("Item_Dmg", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item item15 = pars[0].Evaluate(values) as Item;
			if (item15 == null || item15.IsEmpty)
			{
				item15 = LazyExpressionEvaluationScope.Item;
			}
			if (item15 != null && !item15.IsEmpty)
			{
				return item15.Definition.damage.EvaluateInt(LazyExpressionEvaluationScope.CombatEntity);
			}
			return 0;
		}, false);
		ctx.RegisterFunction("Item_Range", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item item16 = pars[0].Evaluate(values) as Item;
			if (item16 == null || item16.IsEmpty)
			{
				item16 = LazyExpressionEvaluationScope.Item;
			}
			if (item16 != null && !item16.IsEmpty)
			{
				return item16.Definition.atkRange;
			}
			return 0f;
		}, false);
		ctx.RegisterFunction("Item_AtkPause", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item item17 = pars[0].Evaluate(values) as Item;
			if (item17 == null || item17.IsEmpty)
			{
				item17 = LazyExpressionEvaluationScope.Item;
			}
			if (item17 != null && !item17.IsEmpty)
			{
				return item17.Definition.atkPause;
			}
			return 1f;
		}, false);
		ctx.RegisterFunction("Item_Armor", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item item18 = pars[0].Evaluate(values) as Item;
			if (item18 == null || item18.IsEmpty)
			{
				item18 = LazyExpressionEvaluationScope.Item;
			}
			if (item18 != null && !item18.IsEmpty)
			{
				return item18.Definition.quality;
			}
			return 0;
		}, false);
		ctx.RegisterFunction("Item_KnForce", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item item19 = pars[0].Evaluate(values) as Item;
			if (item19 == null || item19.IsEmpty)
			{
				item19 = LazyExpressionEvaluationScope.Item;
			}
			if (item19 != null && !item19.IsEmpty)
			{
				return item19.Definition.knockbackForce;
			}
			return 0;
		}, false);
		ctx.RegisterFunction("Item_DPTag", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item item20 = pars[0].Evaluate(values) as Item;
			if (item20 == null || item20.IsEmpty)
			{
				item20 = LazyExpressionEvaluationScope.Item;
			}
			if (item20 != null && !item20.IsEmpty)
			{
				return item20.Definition.dockPointTag;
			}
			return DockPointTag.None;
		}, false);
		ctx.RegisterFunction("CollectedCount", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			Item item21 = LazyExpressionEvaluationScope.Item;
			if (item21 == null || item21.IsEmpty)
			{
				return 0;
			}
			return item21.Count;
		}, false);
		ctx.RegisterFunction("Spawn_FightingLvl", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text158 = pars[0].EvaluateAsString(values);
			string text159 = ((pars.Length > 1) ? pars[1].EvaluateAsString(values) : "RuinedTemple");
			GameSceneData gameSceneDataById = MainGame.WorldData.GetGameSceneDataById(text159);
			if (gameSceneDataById == null)
			{
				Debug.LogError("Scene with name [" + text159 + "] not found");
				return false;
			}
			gameSceneDataById.AddFightingLevelData(text158);
			return true;
		}, false);
		ctx.RegisterFunction("Despawn_FightingLvl", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text160 = pars[0].EvaluateAsString(values);
			string text161 = ((pars.Length > 1) ? pars[1].EvaluateAsString(values) : "RuinedTemple");
			GameSceneData gameSceneDataById2 = MainGame.WorldData.GetGameSceneDataById(text161);
			if (gameSceneDataById2 == null)
			{
				Debug.LogError("Scene with name [" + text161 + "] not found");
				return false;
			}
			gameSceneDataById2.RemoveFightingLevelData(text160);
			return true;
		}, false);
		ctx.RegisterFunction("SetStage_FightingLvl", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text162 = pars[0].EvaluateAsString(values);
			int num71 = pars[1].EvaluateAsInt(values);
			string text163 = ((pars.Length > 2) ? pars[2].EvaluateAsString(values) : "RuinedTemple");
			GameSceneData gameSceneDataById3 = MainGame.WorldData.GetGameSceneDataById(text163);
			if (gameSceneDataById3 == null)
			{
				Debug.LogError("Scene with name [" + text163 + "] not found");
				return false;
			}
			gameSceneDataById3.ApplyStageForFightingLevel(text162, num71);
			return true;
		}, false);
		ctx.RegisterFunction("Replace_To_Broken_Barricade", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			int rotationIndex = LazyExpressionEvaluationScope.WgoData.MainWgoPartData.rotationIndex;
			WgoData wgoData11 = new WgoData(LazyExpressionEvaluationScope.WgoData.id + "_broken", LazyExpressionEvaluationScope.WgoData.Position, LazyExpressionEvaluationScope.WgoData.WorldId);
			MainGame.Instance.GameSave.worldData.AddWgoData(wgoData11);
			wgoData11.ApplyWgoPartState(wgoData11.MainWgoPartData.variationId, rotationIndex);
			LazySingleton<FightingGameController>.Instance.AddTemporaryWgoData(wgoData11);
			return true;
		}, false);
		ctx.RegisterFunction("SetMercenariesPaymentId", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text164 = pars[0].EvaluateAsString(values);
			MainGame.Instance.GameSave.militaryBaseData.MercenariesPaymentId = text164;
			return true;
		}, false);
		ctx.RegisterFunction("IsFightActive", (IExpression[] pars, IDictionary<string, object> values) => LazySingleton<FightingGameController>.Instance.CurrentFightState > FightState.Disabled, false);
		ctx.RegisterFunction("AddWorkerPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			IWorker workerForCraftExpressions = LazyExpressionEvaluationScope.WgoData.GetWorkerForCraftExpressions();
			if (workerForCraftExpressions != null)
			{
				global::UnityEngine.Object object3 = workerForCraftExpressions as global::UnityEngine.Object;
				if (object3 == null || !(object3 == null))
				{
					string text165 = pars[0].EvaluateAsString(values);
					float num72 = pars[1].EvaluateAsFloat(values);
					workerForCraftExpressions.AddRes(text165, num72);
					return true;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("DecWorkerPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			IWorker workerForCraftExpressions2 = LazyExpressionEvaluationScope.WgoData.GetWorkerForCraftExpressions();
			if (workerForCraftExpressions2 != null)
			{
				global::UnityEngine.Object object4 = workerForCraftExpressions2 as global::UnityEngine.Object;
				if (object4 == null || !(object4 == null))
				{
					string text166 = pars[0].EvaluateAsString(values);
					float num73 = pars[1].EvaluateAsFloat(values);
					workerForCraftExpressions2.AddRes(text166, -num73);
					return true;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("MultiplyWorkerPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			IWorker workerForCraftExpressions3 = LazyExpressionEvaluationScope.WgoData.GetWorkerForCraftExpressions();
			if (workerForCraftExpressions3 != null)
			{
				global::UnityEngine.Object object5 = workerForCraftExpressions3 as global::UnityEngine.Object;
				if (object5 == null || !(object5 == null))
				{
					string text167 = pars[0].EvaluateAsString(values);
					float num74 = pars[1].EvaluateAsFloat(values);
					workerForCraftExpressions3.MultiplyRes(text167, num74);
					return true;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("DivideWorkerPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			IWorker workerForCraftExpressions4 = LazyExpressionEvaluationScope.WgoData.GetWorkerForCraftExpressions();
			if (workerForCraftExpressions4 != null)
			{
				global::UnityEngine.Object object6 = workerForCraftExpressions4 as global::UnityEngine.Object;
				if (object6 == null || !(object6 == null))
				{
					string text168 = pars[0].EvaluateAsString(values);
					float num75 = pars[1].EvaluateAsFloat(values);
					workerForCraftExpressions4.MultiplyRes(text168, 1f / num75);
					return true;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("SetWorkerPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return false;
			}
			IWorker workerForCraftExpressions5 = LazyExpressionEvaluationScope.WgoData.GetWorkerForCraftExpressions();
			if (workerForCraftExpressions5 != null)
			{
				global::UnityEngine.Object object7 = workerForCraftExpressions5 as global::UnityEngine.Object;
				if (object7 == null || !(object7 == null))
				{
					string text169 = pars[0].EvaluateAsString(values);
					float num76 = pars[1].EvaluateAsFloat(values);
					workerForCraftExpressions5.SetRes(text169, num76);
					return true;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("WorkerPar", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			IWorker workerForCraftExpressions6 = LazyExpressionEvaluationScope.WgoData.GetWorkerForCraftExpressions();
			if (workerForCraftExpressions6 != null)
			{
				global::UnityEngine.Object object8 = workerForCraftExpressions6 as global::UnityEngine.Object;
				if (object8 == null || !(object8 == null))
				{
					string text170 = pars[0].EvaluateAsString(values);
					return workerForCraftExpressions6.GetRes(text170, 0f);
				}
			}
			return 0;
		}, false);
		ctx.RegisterFunction("WorkerIsPlayer", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			if (LazyExpressionEvaluationScope.WgoData == null)
			{
				return 0;
			}
			IWorker workerForCraftExpressions7 = LazyExpressionEvaluationScope.WgoData.GetWorkerForCraftExpressions();
			if (workerForCraftExpressions7 != null)
			{
				global::UnityEngine.Object object9 = workerForCraftExpressions7 as global::UnityEngine.Object;
				if (object9 == null || !(object9 == null))
				{
					return (workerForCraftExpressions7 is PlayerController) ? 1 : 0;
				}
			}
			return 0;
		}, false);
		ctx.RegisterFunction("NPCSim_AddWgoToGroupFromBalance", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text171 = pars[0].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.AddWgoToGroupFromBalance(text171);
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_AddWgoToGroup", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text172 = pars[0].EvaluateAsString(values);
			string text173 = pars[1].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.AddWgoToGroup(text172, text173);
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_RemoveWgoFromGroup", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text174 = pars[0].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.RemoveWgoFromGroup(text174);
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_UnlockPointOfInterest", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text175 = pars[0].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.UnlockPointOfInterest(text175);
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_LockPointOfInterest", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text176 = pars[0].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.LockPointOfInterest(text176);
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_SendGroupHome", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text177 = pars[0].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.SendGroupHome(text177);
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_SendAllGroupsHome", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			MainGame.Instance.npcLifeSimulator.SendAllGroupsHome();
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_RollActivityForGroup", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text178 = pars[0].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.RollActivityForGroup(text178);
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_RollActivityForAllGroups", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			MainGame.Instance.npcLifeSimulator.RollActivityForAllGroups();
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_ForceAllGroupsTeleportHome", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			MainGame.Instance.npcLifeSimulator.ForceAllGroupsTeleportHome();
			return true;
		}, false);
		ctx.RegisterFunction("NPCSim_ForceGroupTeleportHome", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text179 = pars[0].EvaluateAsString(values);
			MainGame.Instance.npcLifeSimulator.ForceGroupTeleportHome(text179);
			return true;
		}, false);
		ctx.RegisterFunction("ScanGraph", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazyConsts.Navigation.Graph graph;
			if (Enum.TryParse<LazyConsts.Navigation.Graph>(pars[0].EvaluateAsString(values), out graph))
			{
				RecastGraph recastGraph = AstarPath.active.graphs[(int)graph] as RecastGraph;
				if (recastGraph != null)
				{
					recastGraph.Scan();
					return true;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("Debug_ScanGraphWithDelay", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			LazyConsts.Navigation.Graph graph2;
			if (Enum.TryParse<LazyConsts.Navigation.Graph>(pars[0].EvaluateAsString(values), out graph2))
			{
				RecastGraph recastGraph2 = AstarPath.active.graphs[(int)graph2] as RecastGraph;
				if (recastGraph2 != null)
				{
					LazyTimer.AddTimer(1f, new Action(recastGraph2.Scan), null);
					return true;
				}
			}
			return false;
		}, false);
		ctx.RegisterFunction("SpawnFxOnGDPoint", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text180 = pars[0].EvaluateAsString(values);
			string text181 = pars[1].EvaluateAsString(values);
			GDPointData gdpointDataById3 = MainGame.Instance.GameSave.WorldData.gdPointsData.GetGDPointDataById(text180);
			if (gdpointDataById3 == null)
			{
				Debug.LogError("No gd point:[" + text180 + "] can't spawn fx");
				return false;
			}
			WorldFX.Spawn(gdpointDataById3.Position, text181, null, default(Vector3));
			return true;
		}, false);
		ctx.RegisterFunction("AchUnlock", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text182 = pars[0].EvaluateAsString(values);
			AchievementsSystem.Instance.Unlock(text182);
			return true;
		}, false);
		ctx.RegisterFunction("AchTriggerCountable", delegate(IExpression[] pars, IDictionary<string, object> values)
		{
			string text183 = pars[0].EvaluateAsString(values);
			int num77 = pars[1].EvaluateAsInt(values);
			AchievementsSystem.Instance.TriggerCountable(text183, num77);
			return true;
		}, false);
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00046F88 File Offset: 0x00045188
	private static void OpenAutopsyWindow(WgoData wgoData, Action<UIAutopsyWindowData> onClose = null)
	{
		LazyWindow<UIAutopsyWindowData> window = LazyUI.GetWindow<UIAutopsyWindow>();
		UIAutopsyWindowData uiautopsyWindowData = new UIAutopsyWindowData(wgoData);
		window.Open(uiautopsyWindowData, onClose);
	}

	// Token: 0x04001065 RID: 4197
	private static Dictionary<string, string> equatings = new Dictionary<string, string>
	{
		{ "\\$(\\w*) *\\+= *(.*)", "AddPPar" },
		{ "\\$(\\w*) *\\-= *(.*)", "DecPPar" },
		{ "\\$(\\w*) *\\*= *(.*)", "MultiplyPPar" },
		{ "\\$(\\w*) *\\/= *(.*)", "DividePPar" },
		{ "\\$(\\w*) *\\= *([^=].*)", "SetPPar" },
		{ "\\@(\\w*) *\\+= *(.*)", "AddWGOPar" },
		{ "\\@(\\w*) *\\-= *(.*)", "DecWGOPar" },
		{ "\\@(\\w*) *\\*= *(.*)", "MultiplyWGOPar" },
		{ "\\@(\\w*) *\\/= *(.*)", "DivideWGOPar" },
		{ "\\@(\\w*) *\\= *([^=].*)", "SetWGOPar" },
		{ "\\%(\\w*) *\\+= *(.*)", "AddWorldPar" },
		{ "\\%(\\w*) *\\-= *(.*)", "DecWorldPar" },
		{ "\\%(\\w*) *\\*= *(.*)", "MultiplyWorldPar" },
		{ "\\%(\\w*) *\\/= *(.*)", "DivideWorldPar" },
		{ "\\%(\\w*) *\\= *([^=].*)", "SetWorldPar" },
		{ "\\#(\\w*) *\\+= *(.*)", "AddWorkerPar" },
		{ "\\#(\\w*) *\\-= *(.*)", "DecWorkerPar" },
		{ "\\#(\\w*) *\\*= *(.*)", "MultiplyWorkerPar" },
		{ "\\#(\\w*) *\\/= *(.*)", "DivideWorkerPar" },
		{ "\\#(\\w*) *\\= *([^=].*)", "SetWorkerPar" }
	};

	// Token: 0x04001066 RID: 4198
	private readonly float defaultFloatValue;

	// Token: 0x04001067 RID: 4199
	[NonSerialized]
	private LazyExpressionContext context;

	// Token: 0x04001068 RID: 4200
	private static Context s_sharedContext;

	// Token: 0x02000235 RID: 565
	private class CallbackAvailableMarker
	{
	}
}
