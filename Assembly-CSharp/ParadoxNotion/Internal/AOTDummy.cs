using System;
using FlowCanvas;
using FlowCanvas.Nodes;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using NodeCanvas.Tasks.Actions;
using NodeCanvas.Tasks.Conditions;
using UnityEngine;

namespace ParadoxNotion.Internal
{
	// Token: 0x02000C5B RID: 3163
	internal class AOTDummy
	{
		// Token: 0x060050A3 RID: 20643 RVA: 0x00002318 File Offset: 0x00000518
		private void FlowCanvas_ValueHandler_Delegate()
		{
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x0017E638 File Offset: 0x0017C838
		private void FlowCanvas_Flow_ReadParameter_1()
		{
			Flow flow = default(Flow);
			flow.ReadParameter<bool>((string)this.o);
			flow.ReadParameter<float>((string)this.o);
			flow.ReadParameter<int>((string)this.o);
			flow.ReadParameter<Vector2>((string)this.o);
			flow.ReadParameter<Vector3>((string)this.o);
			flow.ReadParameter<Vector4>((string)this.o);
			flow.ReadParameter<Quaternion>((string)this.o);
			flow.ReadParameter<Keyframe>((string)this.o);
			flow.ReadParameter<Bounds>((string)this.o);
			flow.ReadParameter<Color>((string)this.o);
			flow.ReadParameter<Rect>((string)this.o);
			flow.ReadParameter<ContactPoint>((string)this.o);
			flow.ReadParameter<ContactPoint2D>((string)this.o);
			flow.ReadParameter<RaycastHit>((string)this.o);
			flow.ReadParameter<RaycastHit2D>((string)this.o);
			flow.ReadParameter<Ray>((string)this.o);
			flow.ReadParameter<Space>((string)this.o);
			flow.ReadParameter<LayerMask>((string)this.o);
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x0017E7A4 File Offset: 0x0017C9A4
		private void FlowCanvas_Flow_WriteParameter_2()
		{
			Flow flow = default(Flow);
			flow.WriteParameter<bool>((string)this.o, (bool)this.o);
			flow.WriteParameter<float>((string)this.o, (float)this.o);
			flow.WriteParameter<int>((string)this.o, (int)this.o);
			flow.WriteParameter<Vector2>((string)this.o, (Vector2)this.o);
			flow.WriteParameter<Vector3>((string)this.o, (Vector3)this.o);
			flow.WriteParameter<Vector4>((string)this.o, (Vector4)this.o);
			flow.WriteParameter<Quaternion>((string)this.o, (Quaternion)this.o);
			flow.WriteParameter<Keyframe>((string)this.o, (Keyframe)this.o);
			flow.WriteParameter<Bounds>((string)this.o, (Bounds)this.o);
			flow.WriteParameter<Color>((string)this.o, (Color)this.o);
			flow.WriteParameter<Rect>((string)this.o, (Rect)this.o);
			flow.WriteParameter<ContactPoint>((string)this.o, (ContactPoint)this.o);
			flow.WriteParameter<ContactPoint2D>((string)this.o, (ContactPoint2D)this.o);
			flow.WriteParameter<RaycastHit>((string)this.o, (RaycastHit)this.o);
			flow.WriteParameter<RaycastHit2D>((string)this.o, (RaycastHit2D)this.o);
			flow.WriteParameter<Ray>((string)this.o, (Ray)this.o);
			flow.WriteParameter<Space>((string)this.o, (Space)this.o);
			flow.WriteParameter<LayerMask>((string)this.o, (LayerMask)this.o);
		}

		// Token: 0x060050A6 RID: 20646 RVA: 0x0017E9C4 File Offset: 0x0017CBC4
		private void FlowCanvas_FlowNode_AddValueInput_1()
		{
			object obj = null;
			obj.AddValueInput<bool>((string)this.o, (string)this.o);
			obj.AddValueInput<float>((string)this.o, (string)this.o);
			obj.AddValueInput<int>((string)this.o, (string)this.o);
			obj.AddValueInput<Vector2>((string)this.o, (string)this.o);
			obj.AddValueInput<Vector3>((string)this.o, (string)this.o);
			obj.AddValueInput<Vector4>((string)this.o, (string)this.o);
			obj.AddValueInput<Quaternion>((string)this.o, (string)this.o);
			obj.AddValueInput<Keyframe>((string)this.o, (string)this.o);
			obj.AddValueInput<Bounds>((string)this.o, (string)this.o);
			obj.AddValueInput<Color>((string)this.o, (string)this.o);
			obj.AddValueInput<Rect>((string)this.o, (string)this.o);
			obj.AddValueInput<ContactPoint>((string)this.o, (string)this.o);
			obj.AddValueInput<ContactPoint2D>((string)this.o, (string)this.o);
			obj.AddValueInput<RaycastHit>((string)this.o, (string)this.o);
			obj.AddValueInput<RaycastHit2D>((string)this.o, (string)this.o);
			obj.AddValueInput<Ray>((string)this.o, (string)this.o);
			obj.AddValueInput<Space>((string)this.o, (string)this.o);
			obj.AddValueInput<LayerMask>((string)this.o, (string)this.o);
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x0017EBDC File Offset: 0x0017CDDC
		private void FlowCanvas_FlowNode_AddValueOutput_2()
		{
			object obj = null;
			obj.AddValueOutput<bool>((string)this.o, (string)this.o, (ValueHandler<bool>)this.o);
			obj.AddValueOutput<float>((string)this.o, (string)this.o, (ValueHandler<float>)this.o);
			obj.AddValueOutput<int>((string)this.o, (string)this.o, (ValueHandler<int>)this.o);
			obj.AddValueOutput<Vector2>((string)this.o, (string)this.o, (ValueHandler<Vector2>)this.o);
			obj.AddValueOutput<Vector3>((string)this.o, (string)this.o, (ValueHandler<Vector3>)this.o);
			obj.AddValueOutput<Vector4>((string)this.o, (string)this.o, (ValueHandler<Vector4>)this.o);
			obj.AddValueOutput<Quaternion>((string)this.o, (string)this.o, (ValueHandler<Quaternion>)this.o);
			obj.AddValueOutput<Keyframe>((string)this.o, (string)this.o, (ValueHandler<Keyframe>)this.o);
			obj.AddValueOutput<Bounds>((string)this.o, (string)this.o, (ValueHandler<Bounds>)this.o);
			obj.AddValueOutput<Color>((string)this.o, (string)this.o, (ValueHandler<Color>)this.o);
			obj.AddValueOutput<Rect>((string)this.o, (string)this.o, (ValueHandler<Rect>)this.o);
			obj.AddValueOutput<ContactPoint>((string)this.o, (string)this.o, (ValueHandler<ContactPoint>)this.o);
			obj.AddValueOutput<ContactPoint2D>((string)this.o, (string)this.o, (ValueHandler<ContactPoint2D>)this.o);
			obj.AddValueOutput<RaycastHit>((string)this.o, (string)this.o, (ValueHandler<RaycastHit>)this.o);
			obj.AddValueOutput<RaycastHit2D>((string)this.o, (string)this.o, (ValueHandler<RaycastHit2D>)this.o);
			obj.AddValueOutput<Ray>((string)this.o, (string)this.o, (ValueHandler<Ray>)this.o);
			obj.AddValueOutput<Space>((string)this.o, (string)this.o, (ValueHandler<Space>)this.o);
			obj.AddValueOutput<LayerMask>((string)this.o, (string)this.o, (ValueHandler<LayerMask>)this.o);
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x0017EEBC File Offset: 0x0017D0BC
		private void FlowCanvas_FlowNode_AddValueOutput_3()
		{
			object obj = null;
			obj.AddValueOutput<bool>((string)this.o, (ValueHandler<bool>)this.o, (string)this.o);
			obj.AddValueOutput<float>((string)this.o, (ValueHandler<float>)this.o, (string)this.o);
			obj.AddValueOutput<int>((string)this.o, (ValueHandler<int>)this.o, (string)this.o);
			obj.AddValueOutput<Vector2>((string)this.o, (ValueHandler<Vector2>)this.o, (string)this.o);
			obj.AddValueOutput<Vector3>((string)this.o, (ValueHandler<Vector3>)this.o, (string)this.o);
			obj.AddValueOutput<Vector4>((string)this.o, (ValueHandler<Vector4>)this.o, (string)this.o);
			obj.AddValueOutput<Quaternion>((string)this.o, (ValueHandler<Quaternion>)this.o, (string)this.o);
			obj.AddValueOutput<Keyframe>((string)this.o, (ValueHandler<Keyframe>)this.o, (string)this.o);
			obj.AddValueOutput<Bounds>((string)this.o, (ValueHandler<Bounds>)this.o, (string)this.o);
			obj.AddValueOutput<Color>((string)this.o, (ValueHandler<Color>)this.o, (string)this.o);
			obj.AddValueOutput<Rect>((string)this.o, (ValueHandler<Rect>)this.o, (string)this.o);
			obj.AddValueOutput<ContactPoint>((string)this.o, (ValueHandler<ContactPoint>)this.o, (string)this.o);
			obj.AddValueOutput<ContactPoint2D>((string)this.o, (ValueHandler<ContactPoint2D>)this.o, (string)this.o);
			obj.AddValueOutput<RaycastHit>((string)this.o, (ValueHandler<RaycastHit>)this.o, (string)this.o);
			obj.AddValueOutput<RaycastHit2D>((string)this.o, (ValueHandler<RaycastHit2D>)this.o, (string)this.o);
			obj.AddValueOutput<Ray>((string)this.o, (ValueHandler<Ray>)this.o, (string)this.o);
			obj.AddValueOutput<Space>((string)this.o, (ValueHandler<Space>)this.o, (string)this.o);
			obj.AddValueOutput<LayerMask>((string)this.o, (ValueHandler<LayerMask>)this.o, (string)this.o);
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x0017F19C File Offset: 0x0017D39C
		private void FlowCanvas_TypeConverter_GetConverterFuncFromTo_1()
		{
			TypeConverter.GetConverterFuncFromTo<bool>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<float>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<int>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Vector2>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Vector3>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Vector4>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Quaternion>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Keyframe>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Bounds>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Color>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Rect>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<ContactPoint>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<ContactPoint2D>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<RaycastHit>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<RaycastHit2D>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Ray>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<Space>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
			TypeConverter.GetConverterFuncFromTo<LayerMask>((Type)this.o, (Type)this.o, (ValueHandler<object>)this.o);
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x0017F468 File Offset: 0x0017D668
		private void FlowCanvas_TypeConverter_QuickConvert_2()
		{
			TypeConverter.QuickConvert<bool>(this.o);
			TypeConverter.QuickConvert<float>(this.o);
			TypeConverter.QuickConvert<int>(this.o);
			TypeConverter.QuickConvert<Vector2>(this.o);
			TypeConverter.QuickConvert<Vector3>(this.o);
			TypeConverter.QuickConvert<Vector4>(this.o);
			TypeConverter.QuickConvert<Quaternion>(this.o);
			TypeConverter.QuickConvert<Keyframe>(this.o);
			TypeConverter.QuickConvert<Bounds>(this.o);
			TypeConverter.QuickConvert<Color>(this.o);
			TypeConverter.QuickConvert<Rect>(this.o);
			TypeConverter.QuickConvert<ContactPoint>(this.o);
			TypeConverter.QuickConvert<ContactPoint2D>(this.o);
			TypeConverter.QuickConvert<RaycastHit>(this.o);
			TypeConverter.QuickConvert<RaycastHit2D>(this.o);
			TypeConverter.QuickConvert<Ray>(this.o);
			TypeConverter.QuickConvert<Space>(this.o);
			TypeConverter.QuickConvert<LayerMask>(this.o);
		}

		// Token: 0x060050AB RID: 20651 RVA: 0x0017F550 File Offset: 0x0017D750
		private void FlowCanvas_ValueInput_CreateInstance_1()
		{
			ValueInput.CreateInstance<bool>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<float>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<int>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Vector2>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Vector3>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Vector4>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Quaternion>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Keyframe>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Bounds>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Color>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Rect>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<ContactPoint>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<ContactPoint2D>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<RaycastHit>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<RaycastHit2D>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Ray>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<Space>((FlowNode)this.o, (string)this.o, (string)this.o);
			ValueInput.CreateInstance<LayerMask>((FlowNode)this.o, (string)this.o, (string)this.o);
		}

		// Token: 0x060050AC RID: 20652 RVA: 0x0017F81C File Offset: 0x0017DA1C
		private void FlowCanvas_ValueOutput_CreateInstance_1()
		{
			ValueOutput.CreateInstance<bool>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<bool>)this.o);
			ValueOutput.CreateInstance<float>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<float>)this.o);
			ValueOutput.CreateInstance<int>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<int>)this.o);
			ValueOutput.CreateInstance<Vector2>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Vector2>)this.o);
			ValueOutput.CreateInstance<Vector3>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Vector3>)this.o);
			ValueOutput.CreateInstance<Vector4>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Vector4>)this.o);
			ValueOutput.CreateInstance<Quaternion>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Quaternion>)this.o);
			ValueOutput.CreateInstance<Keyframe>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Keyframe>)this.o);
			ValueOutput.CreateInstance<Bounds>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Bounds>)this.o);
			ValueOutput.CreateInstance<Color>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Color>)this.o);
			ValueOutput.CreateInstance<Rect>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Rect>)this.o);
			ValueOutput.CreateInstance<ContactPoint>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<ContactPoint>)this.o);
			ValueOutput.CreateInstance<ContactPoint2D>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<ContactPoint2D>)this.o);
			ValueOutput.CreateInstance<RaycastHit>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<RaycastHit>)this.o);
			ValueOutput.CreateInstance<RaycastHit2D>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<RaycastHit2D>)this.o);
			ValueOutput.CreateInstance<Ray>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Ray>)this.o);
			ValueOutput.CreateInstance<Space>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<Space>)this.o);
			ValueOutput.CreateInstance<LayerMask>((FlowNode)this.o, (string)this.o, (string)this.o, (ValueHandler<LayerMask>)this.o);
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x0017FBB0 File Offset: 0x0017DDB0
		private void FlowCanvas_Nodes_ReflectedDelegateEvent_Callback1_1()
		{
			object obj = null;
			obj.Callback1<bool>((bool)this.o);
			obj.Callback1<float>((float)this.o);
			obj.Callback1<int>((int)this.o);
			obj.Callback1<Vector2>((Vector2)this.o);
			obj.Callback1<Vector3>((Vector3)this.o);
			obj.Callback1<Vector4>((Vector4)this.o);
			obj.Callback1<Quaternion>((Quaternion)this.o);
			obj.Callback1<Keyframe>((Keyframe)this.o);
			obj.Callback1<Bounds>((Bounds)this.o);
			obj.Callback1<Color>((Color)this.o);
			obj.Callback1<Rect>((Rect)this.o);
			obj.Callback1<ContactPoint>((ContactPoint)this.o);
			obj.Callback1<ContactPoint2D>((ContactPoint2D)this.o);
			obj.Callback1<RaycastHit>((RaycastHit)this.o);
			obj.Callback1<RaycastHit2D>((RaycastHit2D)this.o);
			obj.Callback1<Ray>((Ray)this.o);
			obj.Callback1<Space>((Space)this.o);
			obj.Callback1<LayerMask>((LayerMask)this.o);
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x0017FCF0 File Offset: 0x0017DEF0
		private void FlowCanvas_Nodes_ReflectedUnityEvent_CallbackMethod1_1()
		{
			object obj = null;
			obj.CallbackMethod1<bool>((bool)this.o);
			obj.CallbackMethod1<float>((float)this.o);
			obj.CallbackMethod1<int>((int)this.o);
			obj.CallbackMethod1<Vector2>((Vector2)this.o);
			obj.CallbackMethod1<Vector3>((Vector3)this.o);
			obj.CallbackMethod1<Vector4>((Vector4)this.o);
			obj.CallbackMethod1<Quaternion>((Quaternion)this.o);
			obj.CallbackMethod1<Keyframe>((Keyframe)this.o);
			obj.CallbackMethod1<Bounds>((Bounds)this.o);
			obj.CallbackMethod1<Color>((Color)this.o);
			obj.CallbackMethod1<Rect>((Rect)this.o);
			obj.CallbackMethod1<ContactPoint>((ContactPoint)this.o);
			obj.CallbackMethod1<ContactPoint2D>((ContactPoint2D)this.o);
			obj.CallbackMethod1<RaycastHit>((RaycastHit)this.o);
			obj.CallbackMethod1<RaycastHit2D>((RaycastHit2D)this.o);
			obj.CallbackMethod1<Ray>((Ray)this.o);
			obj.CallbackMethod1<Space>((Space)this.o);
			obj.CallbackMethod1<LayerMask>((LayerMask)this.o);
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x0017FE30 File Offset: 0x0017E030
		private void LazyBearTechnology_CustomFlowNode_GetValueInputPort_1()
		{
			object obj = null;
			obj.GetValueInputPort<bool>((string)this.o);
			obj.GetValueInputPort<float>((string)this.o);
			obj.GetValueInputPort<int>((string)this.o);
			obj.GetValueInputPort<Vector2>((string)this.o);
			obj.GetValueInputPort<Vector3>((string)this.o);
			obj.GetValueInputPort<Vector4>((string)this.o);
			obj.GetValueInputPort<Quaternion>((string)this.o);
			obj.GetValueInputPort<Keyframe>((string)this.o);
			obj.GetValueInputPort<Bounds>((string)this.o);
			obj.GetValueInputPort<Color>((string)this.o);
			obj.GetValueInputPort<Rect>((string)this.o);
			obj.GetValueInputPort<ContactPoint>((string)this.o);
			obj.GetValueInputPort<ContactPoint2D>((string)this.o);
			obj.GetValueInputPort<RaycastHit>((string)this.o);
			obj.GetValueInputPort<RaycastHit2D>((string)this.o);
			obj.GetValueInputPort<Ray>((string)this.o);
			obj.GetValueInputPort<Space>((string)this.o);
			obj.GetValueInputPort<LayerMask>((string)this.o);
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x0017FF84 File Offset: 0x0017E184
		private void NodeCanvas_Framework_Blackboard_GetVariable_1()
		{
			object obj = null;
			obj.GetVariable<bool>((string)this.o);
			obj.GetVariable<float>((string)this.o);
			obj.GetVariable<int>((string)this.o);
			obj.GetVariable<Vector2>((string)this.o);
			obj.GetVariable<Vector3>((string)this.o);
			obj.GetVariable<Vector4>((string)this.o);
			obj.GetVariable<Quaternion>((string)this.o);
			obj.GetVariable<Keyframe>((string)this.o);
			obj.GetVariable<Bounds>((string)this.o);
			obj.GetVariable<Color>((string)this.o);
			obj.GetVariable<Rect>((string)this.o);
			obj.GetVariable<ContactPoint>((string)this.o);
			obj.GetVariable<ContactPoint2D>((string)this.o);
			obj.GetVariable<RaycastHit>((string)this.o);
			obj.GetVariable<RaycastHit2D>((string)this.o);
			obj.GetVariable<Ray>((string)this.o);
			obj.GetVariable<Space>((string)this.o);
			obj.GetVariable<LayerMask>((string)this.o);
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x001800D8 File Offset: 0x0017E2D8
		private void NodeCanvas_Framework_Blackboard_GetVariableValue_2()
		{
			object obj = null;
			obj.GetVariableValue<bool>((string)this.o);
			obj.GetVariableValue<float>((string)this.o);
			obj.GetVariableValue<int>((string)this.o);
			obj.GetVariableValue<Vector2>((string)this.o);
			obj.GetVariableValue<Vector3>((string)this.o);
			obj.GetVariableValue<Vector4>((string)this.o);
			obj.GetVariableValue<Quaternion>((string)this.o);
			obj.GetVariableValue<Keyframe>((string)this.o);
			obj.GetVariableValue<Bounds>((string)this.o);
			obj.GetVariableValue<Color>((string)this.o);
			obj.GetVariableValue<Rect>((string)this.o);
			obj.GetVariableValue<ContactPoint>((string)this.o);
			obj.GetVariableValue<ContactPoint2D>((string)this.o);
			obj.GetVariableValue<RaycastHit>((string)this.o);
			obj.GetVariableValue<RaycastHit2D>((string)this.o);
			obj.GetVariableValue<Ray>((string)this.o);
			obj.GetVariableValue<Space>((string)this.o);
			obj.GetVariableValue<LayerMask>((string)this.o);
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x0018022C File Offset: 0x0017E42C
		private void NodeCanvas_Framework_IBlackboardExtensions_AddVariable_1()
		{
			((IBlackboard)this.o).AddVariable((string)this.o, (bool)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (float)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (int)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Vector2)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Vector3)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Vector4)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Quaternion)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Keyframe)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Bounds)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Color)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Rect)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (ContactPoint)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (ContactPoint2D)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (RaycastHit)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (RaycastHit2D)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Ray)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (Space)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o, (LayerMask)this.o);
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001804F8 File Offset: 0x0017E6F8
		private void NodeCanvas_Framework_IBlackboardExtensions_AddVariable_2()
		{
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
			((IBlackboard)this.o).AddVariable((string)this.o);
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x00180700 File Offset: 0x0017E900
		private void NodeCanvas_Framework_IBlackboardExtensions_GetVariableValue_3()
		{
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
			((IBlackboard)this.o).GetVariableValue((string)this.o);
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x00180908 File Offset: 0x0017EB08
		private void NodeCanvas_Framework_IBlackboardExtensions_GetVariable_4()
		{
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
			((IBlackboard)this.o).GetVariable((string)this.o);
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x00002318 File Offset: 0x00000518
		private void CustomSpoof()
		{
		}

		// Token: 0x0400425D RID: 16989
		private object o;

		// Token: 0x0400425E RID: 16990
		private BinderConnection<bool> FlowCanvas_BinderConnection_System_Boolean;

		// Token: 0x0400425F RID: 16991
		private BinderConnection<float> FlowCanvas_BinderConnection_System_Single;

		// Token: 0x04004260 RID: 16992
		private BinderConnection<int> FlowCanvas_BinderConnection_System_Int32;

		// Token: 0x04004261 RID: 16993
		private BinderConnection<Vector2> FlowCanvas_BinderConnection_UnityEngine_Vector2;

		// Token: 0x04004262 RID: 16994
		private BinderConnection<Vector3> FlowCanvas_BinderConnection_UnityEngine_Vector3;

		// Token: 0x04004263 RID: 16995
		private BinderConnection<Vector4> FlowCanvas_BinderConnection_UnityEngine_Vector4;

		// Token: 0x04004264 RID: 16996
		private BinderConnection<Quaternion> FlowCanvas_BinderConnection_UnityEngine_Quaternion;

		// Token: 0x04004265 RID: 16997
		private BinderConnection<Keyframe> FlowCanvas_BinderConnection_UnityEngine_Keyframe;

		// Token: 0x04004266 RID: 16998
		private BinderConnection<Bounds> FlowCanvas_BinderConnection_UnityEngine_Bounds;

		// Token: 0x04004267 RID: 16999
		private BinderConnection<Color> FlowCanvas_BinderConnection_UnityEngine_Color;

		// Token: 0x04004268 RID: 17000
		private BinderConnection<Rect> FlowCanvas_BinderConnection_UnityEngine_Rect;

		// Token: 0x04004269 RID: 17001
		private BinderConnection<ContactPoint> FlowCanvas_BinderConnection_UnityEngine_ContactPoint;

		// Token: 0x0400426A RID: 17002
		private BinderConnection<ContactPoint2D> FlowCanvas_BinderConnection_UnityEngine_ContactPoint2D;

		// Token: 0x0400426B RID: 17003
		private BinderConnection<RaycastHit> FlowCanvas_BinderConnection_UnityEngine_RaycastHit;

		// Token: 0x0400426C RID: 17004
		private BinderConnection<RaycastHit2D> FlowCanvas_BinderConnection_UnityEngine_RaycastHit2D;

		// Token: 0x0400426D RID: 17005
		private BinderConnection<Ray> FlowCanvas_BinderConnection_UnityEngine_Ray;

		// Token: 0x0400426E RID: 17006
		private BinderConnection<Space> FlowCanvas_BinderConnection_UnityEngine_Space;

		// Token: 0x0400426F RID: 17007
		private BinderConnection<LayerMask> FlowCanvas_BinderConnection_UnityEngine_LayerMask;

		// Token: 0x04004270 RID: 17008
		private ValueInput<bool> FlowCanvas_ValueInput_System_Boolean;

		// Token: 0x04004271 RID: 17009
		private ValueInput<float> FlowCanvas_ValueInput_System_Single;

		// Token: 0x04004272 RID: 17010
		private ValueInput<int> FlowCanvas_ValueInput_System_Int32;

		// Token: 0x04004273 RID: 17011
		private ValueInput<Vector2> FlowCanvas_ValueInput_UnityEngine_Vector2;

		// Token: 0x04004274 RID: 17012
		private ValueInput<Vector3> FlowCanvas_ValueInput_UnityEngine_Vector3;

		// Token: 0x04004275 RID: 17013
		private ValueInput<Vector4> FlowCanvas_ValueInput_UnityEngine_Vector4;

		// Token: 0x04004276 RID: 17014
		private ValueInput<Quaternion> FlowCanvas_ValueInput_UnityEngine_Quaternion;

		// Token: 0x04004277 RID: 17015
		private ValueInput<Keyframe> FlowCanvas_ValueInput_UnityEngine_Keyframe;

		// Token: 0x04004278 RID: 17016
		private ValueInput<Bounds> FlowCanvas_ValueInput_UnityEngine_Bounds;

		// Token: 0x04004279 RID: 17017
		private ValueInput<Color> FlowCanvas_ValueInput_UnityEngine_Color;

		// Token: 0x0400427A RID: 17018
		private ValueInput<Rect> FlowCanvas_ValueInput_UnityEngine_Rect;

		// Token: 0x0400427B RID: 17019
		private ValueInput<ContactPoint> FlowCanvas_ValueInput_UnityEngine_ContactPoint;

		// Token: 0x0400427C RID: 17020
		private ValueInput<ContactPoint2D> FlowCanvas_ValueInput_UnityEngine_ContactPoint2D;

		// Token: 0x0400427D RID: 17021
		private ValueInput<RaycastHit> FlowCanvas_ValueInput_UnityEngine_RaycastHit;

		// Token: 0x0400427E RID: 17022
		private ValueInput<RaycastHit2D> FlowCanvas_ValueInput_UnityEngine_RaycastHit2D;

		// Token: 0x0400427F RID: 17023
		private ValueInput<Ray> FlowCanvas_ValueInput_UnityEngine_Ray;

		// Token: 0x04004280 RID: 17024
		private ValueInput<Space> FlowCanvas_ValueInput_UnityEngine_Space;

		// Token: 0x04004281 RID: 17025
		private ValueInput<LayerMask> FlowCanvas_ValueInput_UnityEngine_LayerMask;

		// Token: 0x04004282 RID: 17026
		private ValueOutput<bool> FlowCanvas_ValueOutput_System_Boolean;

		// Token: 0x04004283 RID: 17027
		private ValueOutput<float> FlowCanvas_ValueOutput_System_Single;

		// Token: 0x04004284 RID: 17028
		private ValueOutput<int> FlowCanvas_ValueOutput_System_Int32;

		// Token: 0x04004285 RID: 17029
		private ValueOutput<Vector2> FlowCanvas_ValueOutput_UnityEngine_Vector2;

		// Token: 0x04004286 RID: 17030
		private ValueOutput<Vector3> FlowCanvas_ValueOutput_UnityEngine_Vector3;

		// Token: 0x04004287 RID: 17031
		private ValueOutput<Vector4> FlowCanvas_ValueOutput_UnityEngine_Vector4;

		// Token: 0x04004288 RID: 17032
		private ValueOutput<Quaternion> FlowCanvas_ValueOutput_UnityEngine_Quaternion;

		// Token: 0x04004289 RID: 17033
		private ValueOutput<Keyframe> FlowCanvas_ValueOutput_UnityEngine_Keyframe;

		// Token: 0x0400428A RID: 17034
		private ValueOutput<Bounds> FlowCanvas_ValueOutput_UnityEngine_Bounds;

		// Token: 0x0400428B RID: 17035
		private ValueOutput<Color> FlowCanvas_ValueOutput_UnityEngine_Color;

		// Token: 0x0400428C RID: 17036
		private ValueOutput<Rect> FlowCanvas_ValueOutput_UnityEngine_Rect;

		// Token: 0x0400428D RID: 17037
		private ValueOutput<ContactPoint> FlowCanvas_ValueOutput_UnityEngine_ContactPoint;

		// Token: 0x0400428E RID: 17038
		private ValueOutput<ContactPoint2D> FlowCanvas_ValueOutput_UnityEngine_ContactPoint2D;

		// Token: 0x0400428F RID: 17039
		private ValueOutput<RaycastHit> FlowCanvas_ValueOutput_UnityEngine_RaycastHit;

		// Token: 0x04004290 RID: 17040
		private ValueOutput<RaycastHit2D> FlowCanvas_ValueOutput_UnityEngine_RaycastHit2D;

		// Token: 0x04004291 RID: 17041
		private ValueOutput<Ray> FlowCanvas_ValueOutput_UnityEngine_Ray;

		// Token: 0x04004292 RID: 17042
		private ValueOutput<Space> FlowCanvas_ValueOutput_UnityEngine_Space;

		// Token: 0x04004293 RID: 17043
		private ValueOutput<LayerMask> FlowCanvas_ValueOutput_UnityEngine_LayerMask;

		// Token: 0x04004294 RID: 17044
		private AddDictionaryItem<bool> FlowCanvas_Nodes_AddDictionaryItem_System_Boolean;

		// Token: 0x04004295 RID: 17045
		private AddDictionaryItem<float> FlowCanvas_Nodes_AddDictionaryItem_System_Single;

		// Token: 0x04004296 RID: 17046
		private AddDictionaryItem<int> FlowCanvas_Nodes_AddDictionaryItem_System_Int32;

		// Token: 0x04004297 RID: 17047
		private AddDictionaryItem<Vector2> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Vector2;

		// Token: 0x04004298 RID: 17048
		private AddDictionaryItem<Vector3> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Vector3;

		// Token: 0x04004299 RID: 17049
		private AddDictionaryItem<Vector4> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Vector4;

		// Token: 0x0400429A RID: 17050
		private AddDictionaryItem<Quaternion> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Quaternion;

		// Token: 0x0400429B RID: 17051
		private AddDictionaryItem<Keyframe> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Keyframe;

		// Token: 0x0400429C RID: 17052
		private AddDictionaryItem<Bounds> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Bounds;

		// Token: 0x0400429D RID: 17053
		private AddDictionaryItem<Color> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Color;

		// Token: 0x0400429E RID: 17054
		private AddDictionaryItem<Rect> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Rect;

		// Token: 0x0400429F RID: 17055
		private AddDictionaryItem<ContactPoint> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_ContactPoint;

		// Token: 0x040042A0 RID: 17056
		private AddDictionaryItem<ContactPoint2D> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_ContactPoint2D;

		// Token: 0x040042A1 RID: 17057
		private AddDictionaryItem<RaycastHit> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_RaycastHit;

		// Token: 0x040042A2 RID: 17058
		private AddDictionaryItem<RaycastHit2D> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_RaycastHit2D;

		// Token: 0x040042A3 RID: 17059
		private AddDictionaryItem<Ray> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Ray;

		// Token: 0x040042A4 RID: 17060
		private AddDictionaryItem<Space> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_Space;

		// Token: 0x040042A5 RID: 17061
		private AddDictionaryItem<LayerMask> FlowCanvas_Nodes_AddDictionaryItem_UnityEngine_LayerMask;

		// Token: 0x040042A6 RID: 17062
		private AddListItem<bool> FlowCanvas_Nodes_AddListItem_System_Boolean;

		// Token: 0x040042A7 RID: 17063
		private AddListItem<float> FlowCanvas_Nodes_AddListItem_System_Single;

		// Token: 0x040042A8 RID: 17064
		private AddListItem<int> FlowCanvas_Nodes_AddListItem_System_Int32;

		// Token: 0x040042A9 RID: 17065
		private AddListItem<Vector2> FlowCanvas_Nodes_AddListItem_UnityEngine_Vector2;

		// Token: 0x040042AA RID: 17066
		private AddListItem<Vector3> FlowCanvas_Nodes_AddListItem_UnityEngine_Vector3;

		// Token: 0x040042AB RID: 17067
		private AddListItem<Vector4> FlowCanvas_Nodes_AddListItem_UnityEngine_Vector4;

		// Token: 0x040042AC RID: 17068
		private AddListItem<Quaternion> FlowCanvas_Nodes_AddListItem_UnityEngine_Quaternion;

		// Token: 0x040042AD RID: 17069
		private AddListItem<Keyframe> FlowCanvas_Nodes_AddListItem_UnityEngine_Keyframe;

		// Token: 0x040042AE RID: 17070
		private AddListItem<Bounds> FlowCanvas_Nodes_AddListItem_UnityEngine_Bounds;

		// Token: 0x040042AF RID: 17071
		private AddListItem<Color> FlowCanvas_Nodes_AddListItem_UnityEngine_Color;

		// Token: 0x040042B0 RID: 17072
		private AddListItem<Rect> FlowCanvas_Nodes_AddListItem_UnityEngine_Rect;

		// Token: 0x040042B1 RID: 17073
		private AddListItem<ContactPoint> FlowCanvas_Nodes_AddListItem_UnityEngine_ContactPoint;

		// Token: 0x040042B2 RID: 17074
		private AddListItem<ContactPoint2D> FlowCanvas_Nodes_AddListItem_UnityEngine_ContactPoint2D;

		// Token: 0x040042B3 RID: 17075
		private AddListItem<RaycastHit> FlowCanvas_Nodes_AddListItem_UnityEngine_RaycastHit;

		// Token: 0x040042B4 RID: 17076
		private AddListItem<RaycastHit2D> FlowCanvas_Nodes_AddListItem_UnityEngine_RaycastHit2D;

		// Token: 0x040042B5 RID: 17077
		private AddListItem<Ray> FlowCanvas_Nodes_AddListItem_UnityEngine_Ray;

		// Token: 0x040042B6 RID: 17078
		private AddListItem<Space> FlowCanvas_Nodes_AddListItem_UnityEngine_Space;

		// Token: 0x040042B7 RID: 17079
		private AddListItem<LayerMask> FlowCanvas_Nodes_AddListItem_UnityEngine_LayerMask;

		// Token: 0x040042B8 RID: 17080
		private BitwiseAnd<Space> FlowCanvas_Nodes_BitwiseAnd_UnityEngine_Space;

		// Token: 0x040042B9 RID: 17081
		private BitwiseInvert<Space> FlowCanvas_Nodes_BitwiseInvert_UnityEngine_Space;

		// Token: 0x040042BA RID: 17082
		private BitwiseOr<Space> FlowCanvas_Nodes_BitwiseOr_UnityEngine_Space;

		// Token: 0x040042BB RID: 17083
		private Buffer<bool> FlowCanvas_Nodes_Buffer_System_Boolean;

		// Token: 0x040042BC RID: 17084
		private Buffer<float> FlowCanvas_Nodes_Buffer_System_Single;

		// Token: 0x040042BD RID: 17085
		private Buffer<int> FlowCanvas_Nodes_Buffer_System_Int32;

		// Token: 0x040042BE RID: 17086
		private Buffer<Vector2> FlowCanvas_Nodes_Buffer_UnityEngine_Vector2;

		// Token: 0x040042BF RID: 17087
		private Buffer<Vector3> FlowCanvas_Nodes_Buffer_UnityEngine_Vector3;

		// Token: 0x040042C0 RID: 17088
		private Buffer<Vector4> FlowCanvas_Nodes_Buffer_UnityEngine_Vector4;

		// Token: 0x040042C1 RID: 17089
		private Buffer<Quaternion> FlowCanvas_Nodes_Buffer_UnityEngine_Quaternion;

		// Token: 0x040042C2 RID: 17090
		private Buffer<Keyframe> FlowCanvas_Nodes_Buffer_UnityEngine_Keyframe;

		// Token: 0x040042C3 RID: 17091
		private Buffer<Bounds> FlowCanvas_Nodes_Buffer_UnityEngine_Bounds;

		// Token: 0x040042C4 RID: 17092
		private Buffer<Color> FlowCanvas_Nodes_Buffer_UnityEngine_Color;

		// Token: 0x040042C5 RID: 17093
		private Buffer<Rect> FlowCanvas_Nodes_Buffer_UnityEngine_Rect;

		// Token: 0x040042C6 RID: 17094
		private Buffer<ContactPoint> FlowCanvas_Nodes_Buffer_UnityEngine_ContactPoint;

		// Token: 0x040042C7 RID: 17095
		private Buffer<ContactPoint2D> FlowCanvas_Nodes_Buffer_UnityEngine_ContactPoint2D;

		// Token: 0x040042C8 RID: 17096
		private Buffer<RaycastHit> FlowCanvas_Nodes_Buffer_UnityEngine_RaycastHit;

		// Token: 0x040042C9 RID: 17097
		private Buffer<RaycastHit2D> FlowCanvas_Nodes_Buffer_UnityEngine_RaycastHit2D;

		// Token: 0x040042CA RID: 17098
		private Buffer<Ray> FlowCanvas_Nodes_Buffer_UnityEngine_Ray;

		// Token: 0x040042CB RID: 17099
		private Buffer<Space> FlowCanvas_Nodes_Buffer_UnityEngine_Space;

		// Token: 0x040042CC RID: 17100
		private Buffer<LayerMask> FlowCanvas_Nodes_Buffer_UnityEngine_LayerMask;

		// Token: 0x040042CD RID: 17101
		private Cache<bool> FlowCanvas_Nodes_Cache_System_Boolean;

		// Token: 0x040042CE RID: 17102
		private Cache<float> FlowCanvas_Nodes_Cache_System_Single;

		// Token: 0x040042CF RID: 17103
		private Cache<int> FlowCanvas_Nodes_Cache_System_Int32;

		// Token: 0x040042D0 RID: 17104
		private Cache<Vector2> FlowCanvas_Nodes_Cache_UnityEngine_Vector2;

		// Token: 0x040042D1 RID: 17105
		private Cache<Vector3> FlowCanvas_Nodes_Cache_UnityEngine_Vector3;

		// Token: 0x040042D2 RID: 17106
		private Cache<Vector4> FlowCanvas_Nodes_Cache_UnityEngine_Vector4;

		// Token: 0x040042D3 RID: 17107
		private Cache<Quaternion> FlowCanvas_Nodes_Cache_UnityEngine_Quaternion;

		// Token: 0x040042D4 RID: 17108
		private Cache<Keyframe> FlowCanvas_Nodes_Cache_UnityEngine_Keyframe;

		// Token: 0x040042D5 RID: 17109
		private Cache<Bounds> FlowCanvas_Nodes_Cache_UnityEngine_Bounds;

		// Token: 0x040042D6 RID: 17110
		private Cache<Color> FlowCanvas_Nodes_Cache_UnityEngine_Color;

		// Token: 0x040042D7 RID: 17111
		private Cache<Rect> FlowCanvas_Nodes_Cache_UnityEngine_Rect;

		// Token: 0x040042D8 RID: 17112
		private Cache<ContactPoint> FlowCanvas_Nodes_Cache_UnityEngine_ContactPoint;

		// Token: 0x040042D9 RID: 17113
		private Cache<ContactPoint2D> FlowCanvas_Nodes_Cache_UnityEngine_ContactPoint2D;

		// Token: 0x040042DA RID: 17114
		private Cache<RaycastHit> FlowCanvas_Nodes_Cache_UnityEngine_RaycastHit;

		// Token: 0x040042DB RID: 17115
		private Cache<RaycastHit2D> FlowCanvas_Nodes_Cache_UnityEngine_RaycastHit2D;

		// Token: 0x040042DC RID: 17116
		private Cache<Ray> FlowCanvas_Nodes_Cache_UnityEngine_Ray;

		// Token: 0x040042DD RID: 17117
		private Cache<Space> FlowCanvas_Nodes_Cache_UnityEngine_Space;

		// Token: 0x040042DE RID: 17118
		private Cache<LayerMask> FlowCanvas_Nodes_Cache_UnityEngine_LayerMask;

		// Token: 0x040042DF RID: 17119
		private Cast<bool> FlowCanvas_Nodes_Cast_System_Boolean;

		// Token: 0x040042E0 RID: 17120
		private Cast<float> FlowCanvas_Nodes_Cast_System_Single;

		// Token: 0x040042E1 RID: 17121
		private Cast<int> FlowCanvas_Nodes_Cast_System_Int32;

		// Token: 0x040042E2 RID: 17122
		private Cast<Vector2> FlowCanvas_Nodes_Cast_UnityEngine_Vector2;

		// Token: 0x040042E3 RID: 17123
		private Cast<Vector3> FlowCanvas_Nodes_Cast_UnityEngine_Vector3;

		// Token: 0x040042E4 RID: 17124
		private Cast<Vector4> FlowCanvas_Nodes_Cast_UnityEngine_Vector4;

		// Token: 0x040042E5 RID: 17125
		private Cast<Quaternion> FlowCanvas_Nodes_Cast_UnityEngine_Quaternion;

		// Token: 0x040042E6 RID: 17126
		private Cast<Keyframe> FlowCanvas_Nodes_Cast_UnityEngine_Keyframe;

		// Token: 0x040042E7 RID: 17127
		private Cast<Bounds> FlowCanvas_Nodes_Cast_UnityEngine_Bounds;

		// Token: 0x040042E8 RID: 17128
		private Cast<Color> FlowCanvas_Nodes_Cast_UnityEngine_Color;

		// Token: 0x040042E9 RID: 17129
		private Cast<Rect> FlowCanvas_Nodes_Cast_UnityEngine_Rect;

		// Token: 0x040042EA RID: 17130
		private Cast<ContactPoint> FlowCanvas_Nodes_Cast_UnityEngine_ContactPoint;

		// Token: 0x040042EB RID: 17131
		private Cast<ContactPoint2D> FlowCanvas_Nodes_Cast_UnityEngine_ContactPoint2D;

		// Token: 0x040042EC RID: 17132
		private Cast<RaycastHit> FlowCanvas_Nodes_Cast_UnityEngine_RaycastHit;

		// Token: 0x040042ED RID: 17133
		private Cast<RaycastHit2D> FlowCanvas_Nodes_Cast_UnityEngine_RaycastHit2D;

		// Token: 0x040042EE RID: 17134
		private Cast<Ray> FlowCanvas_Nodes_Cast_UnityEngine_Ray;

		// Token: 0x040042EF RID: 17135
		private Cast<Space> FlowCanvas_Nodes_Cast_UnityEngine_Space;

		// Token: 0x040042F0 RID: 17136
		private Cast<LayerMask> FlowCanvas_Nodes_Cast_UnityEngine_LayerMask;

		// Token: 0x040042F1 RID: 17137
		private CastTo<bool> FlowCanvas_Nodes_CastTo_System_Boolean;

		// Token: 0x040042F2 RID: 17138
		private CastTo<float> FlowCanvas_Nodes_CastTo_System_Single;

		// Token: 0x040042F3 RID: 17139
		private CastTo<int> FlowCanvas_Nodes_CastTo_System_Int32;

		// Token: 0x040042F4 RID: 17140
		private CastTo<Vector2> FlowCanvas_Nodes_CastTo_UnityEngine_Vector2;

		// Token: 0x040042F5 RID: 17141
		private CastTo<Vector3> FlowCanvas_Nodes_CastTo_UnityEngine_Vector3;

		// Token: 0x040042F6 RID: 17142
		private CastTo<Vector4> FlowCanvas_Nodes_CastTo_UnityEngine_Vector4;

		// Token: 0x040042F7 RID: 17143
		private CastTo<Quaternion> FlowCanvas_Nodes_CastTo_UnityEngine_Quaternion;

		// Token: 0x040042F8 RID: 17144
		private CastTo<Keyframe> FlowCanvas_Nodes_CastTo_UnityEngine_Keyframe;

		// Token: 0x040042F9 RID: 17145
		private CastTo<Bounds> FlowCanvas_Nodes_CastTo_UnityEngine_Bounds;

		// Token: 0x040042FA RID: 17146
		private CastTo<Color> FlowCanvas_Nodes_CastTo_UnityEngine_Color;

		// Token: 0x040042FB RID: 17147
		private CastTo<Rect> FlowCanvas_Nodes_CastTo_UnityEngine_Rect;

		// Token: 0x040042FC RID: 17148
		private CastTo<ContactPoint> FlowCanvas_Nodes_CastTo_UnityEngine_ContactPoint;

		// Token: 0x040042FD RID: 17149
		private CastTo<ContactPoint2D> FlowCanvas_Nodes_CastTo_UnityEngine_ContactPoint2D;

		// Token: 0x040042FE RID: 17150
		private CastTo<RaycastHit> FlowCanvas_Nodes_CastTo_UnityEngine_RaycastHit;

		// Token: 0x040042FF RID: 17151
		private CastTo<RaycastHit2D> FlowCanvas_Nodes_CastTo_UnityEngine_RaycastHit2D;

		// Token: 0x04004300 RID: 17152
		private CastTo<Ray> FlowCanvas_Nodes_CastTo_UnityEngine_Ray;

		// Token: 0x04004301 RID: 17153
		private CastTo<Space> FlowCanvas_Nodes_CastTo_UnityEngine_Space;

		// Token: 0x04004302 RID: 17154
		private CastTo<LayerMask> FlowCanvas_Nodes_CastTo_UnityEngine_LayerMask;

		// Token: 0x04004303 RID: 17155
		private CodeEvent<bool> FlowCanvas_Nodes_CodeEvent_System_Boolean;

		// Token: 0x04004304 RID: 17156
		private CodeEvent<float> FlowCanvas_Nodes_CodeEvent_System_Single;

		// Token: 0x04004305 RID: 17157
		private CodeEvent<int> FlowCanvas_Nodes_CodeEvent_System_Int32;

		// Token: 0x04004306 RID: 17158
		private CodeEvent<Vector2> FlowCanvas_Nodes_CodeEvent_UnityEngine_Vector2;

		// Token: 0x04004307 RID: 17159
		private CodeEvent<Vector3> FlowCanvas_Nodes_CodeEvent_UnityEngine_Vector3;

		// Token: 0x04004308 RID: 17160
		private CodeEvent<Vector4> FlowCanvas_Nodes_CodeEvent_UnityEngine_Vector4;

		// Token: 0x04004309 RID: 17161
		private CodeEvent<Quaternion> FlowCanvas_Nodes_CodeEvent_UnityEngine_Quaternion;

		// Token: 0x0400430A RID: 17162
		private CodeEvent<Keyframe> FlowCanvas_Nodes_CodeEvent_UnityEngine_Keyframe;

		// Token: 0x0400430B RID: 17163
		private CodeEvent<Bounds> FlowCanvas_Nodes_CodeEvent_UnityEngine_Bounds;

		// Token: 0x0400430C RID: 17164
		private CodeEvent<Color> FlowCanvas_Nodes_CodeEvent_UnityEngine_Color;

		// Token: 0x0400430D RID: 17165
		private CodeEvent<Rect> FlowCanvas_Nodes_CodeEvent_UnityEngine_Rect;

		// Token: 0x0400430E RID: 17166
		private CodeEvent<ContactPoint> FlowCanvas_Nodes_CodeEvent_UnityEngine_ContactPoint;

		// Token: 0x0400430F RID: 17167
		private CodeEvent<ContactPoint2D> FlowCanvas_Nodes_CodeEvent_UnityEngine_ContactPoint2D;

		// Token: 0x04004310 RID: 17168
		private CodeEvent<RaycastHit> FlowCanvas_Nodes_CodeEvent_UnityEngine_RaycastHit;

		// Token: 0x04004311 RID: 17169
		private CodeEvent<RaycastHit2D> FlowCanvas_Nodes_CodeEvent_UnityEngine_RaycastHit2D;

		// Token: 0x04004312 RID: 17170
		private CodeEvent<Ray> FlowCanvas_Nodes_CodeEvent_UnityEngine_Ray;

		// Token: 0x04004313 RID: 17171
		private CodeEvent<Space> FlowCanvas_Nodes_CodeEvent_UnityEngine_Space;

		// Token: 0x04004314 RID: 17172
		private CodeEvent<LayerMask> FlowCanvas_Nodes_CodeEvent_UnityEngine_LayerMask;

		// Token: 0x04004315 RID: 17173
		private CreateCollection<bool> FlowCanvas_Nodes_CreateCollection_System_Boolean;

		// Token: 0x04004316 RID: 17174
		private CreateCollection<float> FlowCanvas_Nodes_CreateCollection_System_Single;

		// Token: 0x04004317 RID: 17175
		private CreateCollection<int> FlowCanvas_Nodes_CreateCollection_System_Int32;

		// Token: 0x04004318 RID: 17176
		private CreateCollection<Vector2> FlowCanvas_Nodes_CreateCollection_UnityEngine_Vector2;

		// Token: 0x04004319 RID: 17177
		private CreateCollection<Vector3> FlowCanvas_Nodes_CreateCollection_UnityEngine_Vector3;

		// Token: 0x0400431A RID: 17178
		private CreateCollection<Vector4> FlowCanvas_Nodes_CreateCollection_UnityEngine_Vector4;

		// Token: 0x0400431B RID: 17179
		private CreateCollection<Quaternion> FlowCanvas_Nodes_CreateCollection_UnityEngine_Quaternion;

		// Token: 0x0400431C RID: 17180
		private CreateCollection<Keyframe> FlowCanvas_Nodes_CreateCollection_UnityEngine_Keyframe;

		// Token: 0x0400431D RID: 17181
		private CreateCollection<Bounds> FlowCanvas_Nodes_CreateCollection_UnityEngine_Bounds;

		// Token: 0x0400431E RID: 17182
		private CreateCollection<Color> FlowCanvas_Nodes_CreateCollection_UnityEngine_Color;

		// Token: 0x0400431F RID: 17183
		private CreateCollection<Rect> FlowCanvas_Nodes_CreateCollection_UnityEngine_Rect;

		// Token: 0x04004320 RID: 17184
		private CreateCollection<ContactPoint> FlowCanvas_Nodes_CreateCollection_UnityEngine_ContactPoint;

		// Token: 0x04004321 RID: 17185
		private CreateCollection<ContactPoint2D> FlowCanvas_Nodes_CreateCollection_UnityEngine_ContactPoint2D;

		// Token: 0x04004322 RID: 17186
		private CreateCollection<RaycastHit> FlowCanvas_Nodes_CreateCollection_UnityEngine_RaycastHit;

		// Token: 0x04004323 RID: 17187
		private CreateCollection<RaycastHit2D> FlowCanvas_Nodes_CreateCollection_UnityEngine_RaycastHit2D;

		// Token: 0x04004324 RID: 17188
		private CreateCollection<Ray> FlowCanvas_Nodes_CreateCollection_UnityEngine_Ray;

		// Token: 0x04004325 RID: 17189
		private CreateCollection<Space> FlowCanvas_Nodes_CreateCollection_UnityEngine_Space;

		// Token: 0x04004326 RID: 17190
		private CreateCollection<LayerMask> FlowCanvas_Nodes_CreateCollection_UnityEngine_LayerMask;

		// Token: 0x04004327 RID: 17191
		private CreateDictionary<bool> FlowCanvas_Nodes_CreateDictionary_System_Boolean;

		// Token: 0x04004328 RID: 17192
		private CreateDictionary<float> FlowCanvas_Nodes_CreateDictionary_System_Single;

		// Token: 0x04004329 RID: 17193
		private CreateDictionary<int> FlowCanvas_Nodes_CreateDictionary_System_Int32;

		// Token: 0x0400432A RID: 17194
		private CreateDictionary<Vector2> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Vector2;

		// Token: 0x0400432B RID: 17195
		private CreateDictionary<Vector3> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Vector3;

		// Token: 0x0400432C RID: 17196
		private CreateDictionary<Vector4> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Vector4;

		// Token: 0x0400432D RID: 17197
		private CreateDictionary<Quaternion> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Quaternion;

		// Token: 0x0400432E RID: 17198
		private CreateDictionary<Keyframe> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Keyframe;

		// Token: 0x0400432F RID: 17199
		private CreateDictionary<Bounds> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Bounds;

		// Token: 0x04004330 RID: 17200
		private CreateDictionary<Color> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Color;

		// Token: 0x04004331 RID: 17201
		private CreateDictionary<Rect> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Rect;

		// Token: 0x04004332 RID: 17202
		private CreateDictionary<ContactPoint> FlowCanvas_Nodes_CreateDictionary_UnityEngine_ContactPoint;

		// Token: 0x04004333 RID: 17203
		private CreateDictionary<ContactPoint2D> FlowCanvas_Nodes_CreateDictionary_UnityEngine_ContactPoint2D;

		// Token: 0x04004334 RID: 17204
		private CreateDictionary<RaycastHit> FlowCanvas_Nodes_CreateDictionary_UnityEngine_RaycastHit;

		// Token: 0x04004335 RID: 17205
		private CreateDictionary<RaycastHit2D> FlowCanvas_Nodes_CreateDictionary_UnityEngine_RaycastHit2D;

		// Token: 0x04004336 RID: 17206
		private CreateDictionary<Ray> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Ray;

		// Token: 0x04004337 RID: 17207
		private CreateDictionary<Space> FlowCanvas_Nodes_CreateDictionary_UnityEngine_Space;

		// Token: 0x04004338 RID: 17208
		private CreateDictionary<LayerMask> FlowCanvas_Nodes_CreateDictionary_UnityEngine_LayerMask;

		// Token: 0x04004339 RID: 17209
		private CustomEvent<bool> FlowCanvas_Nodes_CustomEvent_System_Boolean;

		// Token: 0x0400433A RID: 17210
		private CustomEvent<float> FlowCanvas_Nodes_CustomEvent_System_Single;

		// Token: 0x0400433B RID: 17211
		private CustomEvent<int> FlowCanvas_Nodes_CustomEvent_System_Int32;

		// Token: 0x0400433C RID: 17212
		private CustomEvent<Vector2> FlowCanvas_Nodes_CustomEvent_UnityEngine_Vector2;

		// Token: 0x0400433D RID: 17213
		private CustomEvent<Vector3> FlowCanvas_Nodes_CustomEvent_UnityEngine_Vector3;

		// Token: 0x0400433E RID: 17214
		private CustomEvent<Vector4> FlowCanvas_Nodes_CustomEvent_UnityEngine_Vector4;

		// Token: 0x0400433F RID: 17215
		private CustomEvent<Quaternion> FlowCanvas_Nodes_CustomEvent_UnityEngine_Quaternion;

		// Token: 0x04004340 RID: 17216
		private CustomEvent<Keyframe> FlowCanvas_Nodes_CustomEvent_UnityEngine_Keyframe;

		// Token: 0x04004341 RID: 17217
		private CustomEvent<Bounds> FlowCanvas_Nodes_CustomEvent_UnityEngine_Bounds;

		// Token: 0x04004342 RID: 17218
		private CustomEvent<Color> FlowCanvas_Nodes_CustomEvent_UnityEngine_Color;

		// Token: 0x04004343 RID: 17219
		private CustomEvent<Rect> FlowCanvas_Nodes_CustomEvent_UnityEngine_Rect;

		// Token: 0x04004344 RID: 17220
		private CustomEvent<ContactPoint> FlowCanvas_Nodes_CustomEvent_UnityEngine_ContactPoint;

		// Token: 0x04004345 RID: 17221
		private CustomEvent<ContactPoint2D> FlowCanvas_Nodes_CustomEvent_UnityEngine_ContactPoint2D;

		// Token: 0x04004346 RID: 17222
		private CustomEvent<RaycastHit> FlowCanvas_Nodes_CustomEvent_UnityEngine_RaycastHit;

		// Token: 0x04004347 RID: 17223
		private CustomEvent<RaycastHit2D> FlowCanvas_Nodes_CustomEvent_UnityEngine_RaycastHit2D;

		// Token: 0x04004348 RID: 17224
		private CustomEvent<Ray> FlowCanvas_Nodes_CustomEvent_UnityEngine_Ray;

		// Token: 0x04004349 RID: 17225
		private CustomEvent<Space> FlowCanvas_Nodes_CustomEvent_UnityEngine_Space;

		// Token: 0x0400434A RID: 17226
		private CustomEvent<LayerMask> FlowCanvas_Nodes_CustomEvent_UnityEngine_LayerMask;

		// Token: 0x0400434B RID: 17227
		private DictionaryContainsKey<bool> FlowCanvas_Nodes_DictionaryContainsKey_System_Boolean;

		// Token: 0x0400434C RID: 17228
		private DictionaryContainsKey<float> FlowCanvas_Nodes_DictionaryContainsKey_System_Single;

		// Token: 0x0400434D RID: 17229
		private DictionaryContainsKey<int> FlowCanvas_Nodes_DictionaryContainsKey_System_Int32;

		// Token: 0x0400434E RID: 17230
		private DictionaryContainsKey<Vector2> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Vector2;

		// Token: 0x0400434F RID: 17231
		private DictionaryContainsKey<Vector3> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Vector3;

		// Token: 0x04004350 RID: 17232
		private DictionaryContainsKey<Vector4> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Vector4;

		// Token: 0x04004351 RID: 17233
		private DictionaryContainsKey<Quaternion> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Quaternion;

		// Token: 0x04004352 RID: 17234
		private DictionaryContainsKey<Keyframe> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Keyframe;

		// Token: 0x04004353 RID: 17235
		private DictionaryContainsKey<Bounds> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Bounds;

		// Token: 0x04004354 RID: 17236
		private DictionaryContainsKey<Color> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Color;

		// Token: 0x04004355 RID: 17237
		private DictionaryContainsKey<Rect> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Rect;

		// Token: 0x04004356 RID: 17238
		private DictionaryContainsKey<ContactPoint> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_ContactPoint;

		// Token: 0x04004357 RID: 17239
		private DictionaryContainsKey<ContactPoint2D> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_ContactPoint2D;

		// Token: 0x04004358 RID: 17240
		private DictionaryContainsKey<RaycastHit> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_RaycastHit;

		// Token: 0x04004359 RID: 17241
		private DictionaryContainsKey<RaycastHit2D> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_RaycastHit2D;

		// Token: 0x0400435A RID: 17242
		private DictionaryContainsKey<Ray> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Ray;

		// Token: 0x0400435B RID: 17243
		private DictionaryContainsKey<Space> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_Space;

		// Token: 0x0400435C RID: 17244
		private DictionaryContainsKey<LayerMask> FlowCanvas_Nodes_DictionaryContainsKey_UnityEngine_LayerMask;

		// Token: 0x0400435D RID: 17245
		private ForEach<bool> FlowCanvas_Nodes_ForEach_System_Boolean;

		// Token: 0x0400435E RID: 17246
		private ForEach<float> FlowCanvas_Nodes_ForEach_System_Single;

		// Token: 0x0400435F RID: 17247
		private ForEach<int> FlowCanvas_Nodes_ForEach_System_Int32;

		// Token: 0x04004360 RID: 17248
		private ForEach<Vector2> FlowCanvas_Nodes_ForEach_UnityEngine_Vector2;

		// Token: 0x04004361 RID: 17249
		private ForEach<Vector3> FlowCanvas_Nodes_ForEach_UnityEngine_Vector3;

		// Token: 0x04004362 RID: 17250
		private ForEach<Vector4> FlowCanvas_Nodes_ForEach_UnityEngine_Vector4;

		// Token: 0x04004363 RID: 17251
		private ForEach<Quaternion> FlowCanvas_Nodes_ForEach_UnityEngine_Quaternion;

		// Token: 0x04004364 RID: 17252
		private ForEach<Keyframe> FlowCanvas_Nodes_ForEach_UnityEngine_Keyframe;

		// Token: 0x04004365 RID: 17253
		private ForEach<Bounds> FlowCanvas_Nodes_ForEach_UnityEngine_Bounds;

		// Token: 0x04004366 RID: 17254
		private ForEach<Color> FlowCanvas_Nodes_ForEach_UnityEngine_Color;

		// Token: 0x04004367 RID: 17255
		private ForEach<Rect> FlowCanvas_Nodes_ForEach_UnityEngine_Rect;

		// Token: 0x04004368 RID: 17256
		private ForEach<ContactPoint> FlowCanvas_Nodes_ForEach_UnityEngine_ContactPoint;

		// Token: 0x04004369 RID: 17257
		private ForEach<ContactPoint2D> FlowCanvas_Nodes_ForEach_UnityEngine_ContactPoint2D;

		// Token: 0x0400436A RID: 17258
		private ForEach<RaycastHit> FlowCanvas_Nodes_ForEach_UnityEngine_RaycastHit;

		// Token: 0x0400436B RID: 17259
		private ForEach<RaycastHit2D> FlowCanvas_Nodes_ForEach_UnityEngine_RaycastHit2D;

		// Token: 0x0400436C RID: 17260
		private ForEach<Ray> FlowCanvas_Nodes_ForEach_UnityEngine_Ray;

		// Token: 0x0400436D RID: 17261
		private ForEach<Space> FlowCanvas_Nodes_ForEach_UnityEngine_Space;

		// Token: 0x0400436E RID: 17262
		private ForEach<LayerMask> FlowCanvas_Nodes_ForEach_UnityEngine_LayerMask;

		// Token: 0x0400436F RID: 17263
		private GetDictionaryItem<bool> FlowCanvas_Nodes_GetDictionaryItem_System_Boolean;

		// Token: 0x04004370 RID: 17264
		private GetDictionaryItem<float> FlowCanvas_Nodes_GetDictionaryItem_System_Single;

		// Token: 0x04004371 RID: 17265
		private GetDictionaryItem<int> FlowCanvas_Nodes_GetDictionaryItem_System_Int32;

		// Token: 0x04004372 RID: 17266
		private GetDictionaryItem<Vector2> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Vector2;

		// Token: 0x04004373 RID: 17267
		private GetDictionaryItem<Vector3> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Vector3;

		// Token: 0x04004374 RID: 17268
		private GetDictionaryItem<Vector4> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Vector4;

		// Token: 0x04004375 RID: 17269
		private GetDictionaryItem<Quaternion> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Quaternion;

		// Token: 0x04004376 RID: 17270
		private GetDictionaryItem<Keyframe> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Keyframe;

		// Token: 0x04004377 RID: 17271
		private GetDictionaryItem<Bounds> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Bounds;

		// Token: 0x04004378 RID: 17272
		private GetDictionaryItem<Color> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Color;

		// Token: 0x04004379 RID: 17273
		private GetDictionaryItem<Rect> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Rect;

		// Token: 0x0400437A RID: 17274
		private GetDictionaryItem<ContactPoint> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_ContactPoint;

		// Token: 0x0400437B RID: 17275
		private GetDictionaryItem<ContactPoint2D> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_ContactPoint2D;

		// Token: 0x0400437C RID: 17276
		private GetDictionaryItem<RaycastHit> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_RaycastHit;

		// Token: 0x0400437D RID: 17277
		private GetDictionaryItem<RaycastHit2D> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_RaycastHit2D;

		// Token: 0x0400437E RID: 17278
		private GetDictionaryItem<Ray> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Ray;

		// Token: 0x0400437F RID: 17279
		private GetDictionaryItem<Space> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_Space;

		// Token: 0x04004380 RID: 17280
		private GetDictionaryItem<LayerMask> FlowCanvas_Nodes_GetDictionaryItem_UnityEngine_LayerMask;

		// Token: 0x04004381 RID: 17281
		private GetFirstListItem<bool> FlowCanvas_Nodes_GetFirstListItem_System_Boolean;

		// Token: 0x04004382 RID: 17282
		private GetFirstListItem<float> FlowCanvas_Nodes_GetFirstListItem_System_Single;

		// Token: 0x04004383 RID: 17283
		private GetFirstListItem<int> FlowCanvas_Nodes_GetFirstListItem_System_Int32;

		// Token: 0x04004384 RID: 17284
		private GetFirstListItem<Vector2> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Vector2;

		// Token: 0x04004385 RID: 17285
		private GetFirstListItem<Vector3> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Vector3;

		// Token: 0x04004386 RID: 17286
		private GetFirstListItem<Vector4> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Vector4;

		// Token: 0x04004387 RID: 17287
		private GetFirstListItem<Quaternion> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Quaternion;

		// Token: 0x04004388 RID: 17288
		private GetFirstListItem<Keyframe> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Keyframe;

		// Token: 0x04004389 RID: 17289
		private GetFirstListItem<Bounds> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Bounds;

		// Token: 0x0400438A RID: 17290
		private GetFirstListItem<Color> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Color;

		// Token: 0x0400438B RID: 17291
		private GetFirstListItem<Rect> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Rect;

		// Token: 0x0400438C RID: 17292
		private GetFirstListItem<ContactPoint> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_ContactPoint;

		// Token: 0x0400438D RID: 17293
		private GetFirstListItem<ContactPoint2D> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_ContactPoint2D;

		// Token: 0x0400438E RID: 17294
		private GetFirstListItem<RaycastHit> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_RaycastHit;

		// Token: 0x0400438F RID: 17295
		private GetFirstListItem<RaycastHit2D> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_RaycastHit2D;

		// Token: 0x04004390 RID: 17296
		private GetFirstListItem<Ray> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Ray;

		// Token: 0x04004391 RID: 17297
		private GetFirstListItem<Space> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_Space;

		// Token: 0x04004392 RID: 17298
		private GetFirstListItem<LayerMask> FlowCanvas_Nodes_GetFirstListItem_UnityEngine_LayerMask;

		// Token: 0x04004393 RID: 17299
		private GetLastListItem<bool> FlowCanvas_Nodes_GetLastListItem_System_Boolean;

		// Token: 0x04004394 RID: 17300
		private GetLastListItem<float> FlowCanvas_Nodes_GetLastListItem_System_Single;

		// Token: 0x04004395 RID: 17301
		private GetLastListItem<int> FlowCanvas_Nodes_GetLastListItem_System_Int32;

		// Token: 0x04004396 RID: 17302
		private GetLastListItem<Vector2> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Vector2;

		// Token: 0x04004397 RID: 17303
		private GetLastListItem<Vector3> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Vector3;

		// Token: 0x04004398 RID: 17304
		private GetLastListItem<Vector4> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Vector4;

		// Token: 0x04004399 RID: 17305
		private GetLastListItem<Quaternion> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Quaternion;

		// Token: 0x0400439A RID: 17306
		private GetLastListItem<Keyframe> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Keyframe;

		// Token: 0x0400439B RID: 17307
		private GetLastListItem<Bounds> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Bounds;

		// Token: 0x0400439C RID: 17308
		private GetLastListItem<Color> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Color;

		// Token: 0x0400439D RID: 17309
		private GetLastListItem<Rect> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Rect;

		// Token: 0x0400439E RID: 17310
		private GetLastListItem<ContactPoint> FlowCanvas_Nodes_GetLastListItem_UnityEngine_ContactPoint;

		// Token: 0x0400439F RID: 17311
		private GetLastListItem<ContactPoint2D> FlowCanvas_Nodes_GetLastListItem_UnityEngine_ContactPoint2D;

		// Token: 0x040043A0 RID: 17312
		private GetLastListItem<RaycastHit> FlowCanvas_Nodes_GetLastListItem_UnityEngine_RaycastHit;

		// Token: 0x040043A1 RID: 17313
		private GetLastListItem<RaycastHit2D> FlowCanvas_Nodes_GetLastListItem_UnityEngine_RaycastHit2D;

		// Token: 0x040043A2 RID: 17314
		private GetLastListItem<Ray> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Ray;

		// Token: 0x040043A3 RID: 17315
		private GetLastListItem<Space> FlowCanvas_Nodes_GetLastListItem_UnityEngine_Space;

		// Token: 0x040043A4 RID: 17316
		private GetLastListItem<LayerMask> FlowCanvas_Nodes_GetLastListItem_UnityEngine_LayerMask;

		// Token: 0x040043A5 RID: 17317
		private GetListItem<bool> FlowCanvas_Nodes_GetListItem_System_Boolean;

		// Token: 0x040043A6 RID: 17318
		private GetListItem<float> FlowCanvas_Nodes_GetListItem_System_Single;

		// Token: 0x040043A7 RID: 17319
		private GetListItem<int> FlowCanvas_Nodes_GetListItem_System_Int32;

		// Token: 0x040043A8 RID: 17320
		private GetListItem<Vector2> FlowCanvas_Nodes_GetListItem_UnityEngine_Vector2;

		// Token: 0x040043A9 RID: 17321
		private GetListItem<Vector3> FlowCanvas_Nodes_GetListItem_UnityEngine_Vector3;

		// Token: 0x040043AA RID: 17322
		private GetListItem<Vector4> FlowCanvas_Nodes_GetListItem_UnityEngine_Vector4;

		// Token: 0x040043AB RID: 17323
		private GetListItem<Quaternion> FlowCanvas_Nodes_GetListItem_UnityEngine_Quaternion;

		// Token: 0x040043AC RID: 17324
		private GetListItem<Keyframe> FlowCanvas_Nodes_GetListItem_UnityEngine_Keyframe;

		// Token: 0x040043AD RID: 17325
		private GetListItem<Bounds> FlowCanvas_Nodes_GetListItem_UnityEngine_Bounds;

		// Token: 0x040043AE RID: 17326
		private GetListItem<Color> FlowCanvas_Nodes_GetListItem_UnityEngine_Color;

		// Token: 0x040043AF RID: 17327
		private GetListItem<Rect> FlowCanvas_Nodes_GetListItem_UnityEngine_Rect;

		// Token: 0x040043B0 RID: 17328
		private GetListItem<ContactPoint> FlowCanvas_Nodes_GetListItem_UnityEngine_ContactPoint;

		// Token: 0x040043B1 RID: 17329
		private GetListItem<ContactPoint2D> FlowCanvas_Nodes_GetListItem_UnityEngine_ContactPoint2D;

		// Token: 0x040043B2 RID: 17330
		private GetListItem<RaycastHit> FlowCanvas_Nodes_GetListItem_UnityEngine_RaycastHit;

		// Token: 0x040043B3 RID: 17331
		private GetListItem<RaycastHit2D> FlowCanvas_Nodes_GetListItem_UnityEngine_RaycastHit2D;

		// Token: 0x040043B4 RID: 17332
		private GetListItem<Ray> FlowCanvas_Nodes_GetListItem_UnityEngine_Ray;

		// Token: 0x040043B5 RID: 17333
		private GetListItem<Space> FlowCanvas_Nodes_GetListItem_UnityEngine_Space;

		// Token: 0x040043B6 RID: 17334
		private GetListItem<LayerMask> FlowCanvas_Nodes_GetListItem_UnityEngine_LayerMask;

		// Token: 0x040043B7 RID: 17335
		private GetOtherVariable<bool> FlowCanvas_Nodes_GetOtherVariable_System_Boolean;

		// Token: 0x040043B8 RID: 17336
		private GetOtherVariable<float> FlowCanvas_Nodes_GetOtherVariable_System_Single;

		// Token: 0x040043B9 RID: 17337
		private GetOtherVariable<int> FlowCanvas_Nodes_GetOtherVariable_System_Int32;

		// Token: 0x040043BA RID: 17338
		private GetOtherVariable<Vector2> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Vector2;

		// Token: 0x040043BB RID: 17339
		private GetOtherVariable<Vector3> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Vector3;

		// Token: 0x040043BC RID: 17340
		private GetOtherVariable<Vector4> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Vector4;

		// Token: 0x040043BD RID: 17341
		private GetOtherVariable<Quaternion> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Quaternion;

		// Token: 0x040043BE RID: 17342
		private GetOtherVariable<Keyframe> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Keyframe;

		// Token: 0x040043BF RID: 17343
		private GetOtherVariable<Bounds> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Bounds;

		// Token: 0x040043C0 RID: 17344
		private GetOtherVariable<Color> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Color;

		// Token: 0x040043C1 RID: 17345
		private GetOtherVariable<Rect> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Rect;

		// Token: 0x040043C2 RID: 17346
		private GetOtherVariable<ContactPoint> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_ContactPoint;

		// Token: 0x040043C3 RID: 17347
		private GetOtherVariable<ContactPoint2D> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_ContactPoint2D;

		// Token: 0x040043C4 RID: 17348
		private GetOtherVariable<RaycastHit> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_RaycastHit;

		// Token: 0x040043C5 RID: 17349
		private GetOtherVariable<RaycastHit2D> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_RaycastHit2D;

		// Token: 0x040043C6 RID: 17350
		private GetOtherVariable<Ray> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Ray;

		// Token: 0x040043C7 RID: 17351
		private GetOtherVariable<Space> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_Space;

		// Token: 0x040043C8 RID: 17352
		private GetOtherVariable<LayerMask> FlowCanvas_Nodes_GetOtherVariable_UnityEngine_LayerMask;

		// Token: 0x040043C9 RID: 17353
		private GetRandomListItem<bool> FlowCanvas_Nodes_GetRandomListItem_System_Boolean;

		// Token: 0x040043CA RID: 17354
		private GetRandomListItem<float> FlowCanvas_Nodes_GetRandomListItem_System_Single;

		// Token: 0x040043CB RID: 17355
		private GetRandomListItem<int> FlowCanvas_Nodes_GetRandomListItem_System_Int32;

		// Token: 0x040043CC RID: 17356
		private GetRandomListItem<Vector2> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Vector2;

		// Token: 0x040043CD RID: 17357
		private GetRandomListItem<Vector3> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Vector3;

		// Token: 0x040043CE RID: 17358
		private GetRandomListItem<Vector4> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Vector4;

		// Token: 0x040043CF RID: 17359
		private GetRandomListItem<Quaternion> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Quaternion;

		// Token: 0x040043D0 RID: 17360
		private GetRandomListItem<Keyframe> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Keyframe;

		// Token: 0x040043D1 RID: 17361
		private GetRandomListItem<Bounds> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Bounds;

		// Token: 0x040043D2 RID: 17362
		private GetRandomListItem<Color> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Color;

		// Token: 0x040043D3 RID: 17363
		private GetRandomListItem<Rect> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Rect;

		// Token: 0x040043D4 RID: 17364
		private GetRandomListItem<ContactPoint> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_ContactPoint;

		// Token: 0x040043D5 RID: 17365
		private GetRandomListItem<ContactPoint2D> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_ContactPoint2D;

		// Token: 0x040043D6 RID: 17366
		private GetRandomListItem<RaycastHit> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_RaycastHit;

		// Token: 0x040043D7 RID: 17367
		private GetRandomListItem<RaycastHit2D> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_RaycastHit2D;

		// Token: 0x040043D8 RID: 17368
		private GetRandomListItem<Ray> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Ray;

		// Token: 0x040043D9 RID: 17369
		private GetRandomListItem<Space> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_Space;

		// Token: 0x040043DA RID: 17370
		private GetRandomListItem<LayerMask> FlowCanvas_Nodes_GetRandomListItem_UnityEngine_LayerMask;

		// Token: 0x040043DB RID: 17371
		private GetVariable<bool> FlowCanvas_Nodes_GetVariable_System_Boolean;

		// Token: 0x040043DC RID: 17372
		private GetVariable<float> FlowCanvas_Nodes_GetVariable_System_Single;

		// Token: 0x040043DD RID: 17373
		private GetVariable<int> FlowCanvas_Nodes_GetVariable_System_Int32;

		// Token: 0x040043DE RID: 17374
		private GetVariable<Vector2> FlowCanvas_Nodes_GetVariable_UnityEngine_Vector2;

		// Token: 0x040043DF RID: 17375
		private GetVariable<Vector3> FlowCanvas_Nodes_GetVariable_UnityEngine_Vector3;

		// Token: 0x040043E0 RID: 17376
		private GetVariable<Vector4> FlowCanvas_Nodes_GetVariable_UnityEngine_Vector4;

		// Token: 0x040043E1 RID: 17377
		private GetVariable<Quaternion> FlowCanvas_Nodes_GetVariable_UnityEngine_Quaternion;

		// Token: 0x040043E2 RID: 17378
		private GetVariable<Keyframe> FlowCanvas_Nodes_GetVariable_UnityEngine_Keyframe;

		// Token: 0x040043E3 RID: 17379
		private GetVariable<Bounds> FlowCanvas_Nodes_GetVariable_UnityEngine_Bounds;

		// Token: 0x040043E4 RID: 17380
		private GetVariable<Color> FlowCanvas_Nodes_GetVariable_UnityEngine_Color;

		// Token: 0x040043E5 RID: 17381
		private GetVariable<Rect> FlowCanvas_Nodes_GetVariable_UnityEngine_Rect;

		// Token: 0x040043E6 RID: 17382
		private GetVariable<ContactPoint> FlowCanvas_Nodes_GetVariable_UnityEngine_ContactPoint;

		// Token: 0x040043E7 RID: 17383
		private GetVariable<ContactPoint2D> FlowCanvas_Nodes_GetVariable_UnityEngine_ContactPoint2D;

		// Token: 0x040043E8 RID: 17384
		private GetVariable<RaycastHit> FlowCanvas_Nodes_GetVariable_UnityEngine_RaycastHit;

		// Token: 0x040043E9 RID: 17385
		private GetVariable<RaycastHit2D> FlowCanvas_Nodes_GetVariable_UnityEngine_RaycastHit2D;

		// Token: 0x040043EA RID: 17386
		private GetVariable<Ray> FlowCanvas_Nodes_GetVariable_UnityEngine_Ray;

		// Token: 0x040043EB RID: 17387
		private GetVariable<Space> FlowCanvas_Nodes_GetVariable_UnityEngine_Space;

		// Token: 0x040043EC RID: 17388
		private GetVariable<LayerMask> FlowCanvas_Nodes_GetVariable_UnityEngine_LayerMask;

		// Token: 0x040043ED RID: 17389
		private Identity<bool> FlowCanvas_Nodes_Identity_System_Boolean;

		// Token: 0x040043EE RID: 17390
		private Identity<float> FlowCanvas_Nodes_Identity_System_Single;

		// Token: 0x040043EF RID: 17391
		private Identity<int> FlowCanvas_Nodes_Identity_System_Int32;

		// Token: 0x040043F0 RID: 17392
		private Identity<Vector2> FlowCanvas_Nodes_Identity_UnityEngine_Vector2;

		// Token: 0x040043F1 RID: 17393
		private Identity<Vector3> FlowCanvas_Nodes_Identity_UnityEngine_Vector3;

		// Token: 0x040043F2 RID: 17394
		private Identity<Vector4> FlowCanvas_Nodes_Identity_UnityEngine_Vector4;

		// Token: 0x040043F3 RID: 17395
		private Identity<Quaternion> FlowCanvas_Nodes_Identity_UnityEngine_Quaternion;

		// Token: 0x040043F4 RID: 17396
		private Identity<Keyframe> FlowCanvas_Nodes_Identity_UnityEngine_Keyframe;

		// Token: 0x040043F5 RID: 17397
		private Identity<Bounds> FlowCanvas_Nodes_Identity_UnityEngine_Bounds;

		// Token: 0x040043F6 RID: 17398
		private Identity<Color> FlowCanvas_Nodes_Identity_UnityEngine_Color;

		// Token: 0x040043F7 RID: 17399
		private Identity<Rect> FlowCanvas_Nodes_Identity_UnityEngine_Rect;

		// Token: 0x040043F8 RID: 17400
		private Identity<ContactPoint> FlowCanvas_Nodes_Identity_UnityEngine_ContactPoint;

		// Token: 0x040043F9 RID: 17401
		private Identity<ContactPoint2D> FlowCanvas_Nodes_Identity_UnityEngine_ContactPoint2D;

		// Token: 0x040043FA RID: 17402
		private Identity<RaycastHit> FlowCanvas_Nodes_Identity_UnityEngine_RaycastHit;

		// Token: 0x040043FB RID: 17403
		private Identity<RaycastHit2D> FlowCanvas_Nodes_Identity_UnityEngine_RaycastHit2D;

		// Token: 0x040043FC RID: 17404
		private Identity<Ray> FlowCanvas_Nodes_Identity_UnityEngine_Ray;

		// Token: 0x040043FD RID: 17405
		private Identity<Space> FlowCanvas_Nodes_Identity_UnityEngine_Space;

		// Token: 0x040043FE RID: 17406
		private Identity<LayerMask> FlowCanvas_Nodes_Identity_UnityEngine_LayerMask;

		// Token: 0x040043FF RID: 17407
		private InsertListItem<bool> FlowCanvas_Nodes_InsertListItem_System_Boolean;

		// Token: 0x04004400 RID: 17408
		private InsertListItem<float> FlowCanvas_Nodes_InsertListItem_System_Single;

		// Token: 0x04004401 RID: 17409
		private InsertListItem<int> FlowCanvas_Nodes_InsertListItem_System_Int32;

		// Token: 0x04004402 RID: 17410
		private InsertListItem<Vector2> FlowCanvas_Nodes_InsertListItem_UnityEngine_Vector2;

		// Token: 0x04004403 RID: 17411
		private InsertListItem<Vector3> FlowCanvas_Nodes_InsertListItem_UnityEngine_Vector3;

		// Token: 0x04004404 RID: 17412
		private InsertListItem<Vector4> FlowCanvas_Nodes_InsertListItem_UnityEngine_Vector4;

		// Token: 0x04004405 RID: 17413
		private InsertListItem<Quaternion> FlowCanvas_Nodes_InsertListItem_UnityEngine_Quaternion;

		// Token: 0x04004406 RID: 17414
		private InsertListItem<Keyframe> FlowCanvas_Nodes_InsertListItem_UnityEngine_Keyframe;

		// Token: 0x04004407 RID: 17415
		private InsertListItem<Bounds> FlowCanvas_Nodes_InsertListItem_UnityEngine_Bounds;

		// Token: 0x04004408 RID: 17416
		private InsertListItem<Color> FlowCanvas_Nodes_InsertListItem_UnityEngine_Color;

		// Token: 0x04004409 RID: 17417
		private InsertListItem<Rect> FlowCanvas_Nodes_InsertListItem_UnityEngine_Rect;

		// Token: 0x0400440A RID: 17418
		private InsertListItem<ContactPoint> FlowCanvas_Nodes_InsertListItem_UnityEngine_ContactPoint;

		// Token: 0x0400440B RID: 17419
		private InsertListItem<ContactPoint2D> FlowCanvas_Nodes_InsertListItem_UnityEngine_ContactPoint2D;

		// Token: 0x0400440C RID: 17420
		private InsertListItem<RaycastHit> FlowCanvas_Nodes_InsertListItem_UnityEngine_RaycastHit;

		// Token: 0x0400440D RID: 17421
		private InsertListItem<RaycastHit2D> FlowCanvas_Nodes_InsertListItem_UnityEngine_RaycastHit2D;

		// Token: 0x0400440E RID: 17422
		private InsertListItem<Ray> FlowCanvas_Nodes_InsertListItem_UnityEngine_Ray;

		// Token: 0x0400440F RID: 17423
		private InsertListItem<Space> FlowCanvas_Nodes_InsertListItem_UnityEngine_Space;

		// Token: 0x04004410 RID: 17424
		private InsertListItem<LayerMask> FlowCanvas_Nodes_InsertListItem_UnityEngine_LayerMask;

		// Token: 0x04004411 RID: 17425
		private PickValue<bool> FlowCanvas_Nodes_PickValue_System_Boolean;

		// Token: 0x04004412 RID: 17426
		private PickValue<float> FlowCanvas_Nodes_PickValue_System_Single;

		// Token: 0x04004413 RID: 17427
		private PickValue<int> FlowCanvas_Nodes_PickValue_System_Int32;

		// Token: 0x04004414 RID: 17428
		private PickValue<Vector2> FlowCanvas_Nodes_PickValue_UnityEngine_Vector2;

		// Token: 0x04004415 RID: 17429
		private PickValue<Vector3> FlowCanvas_Nodes_PickValue_UnityEngine_Vector3;

		// Token: 0x04004416 RID: 17430
		private PickValue<Vector4> FlowCanvas_Nodes_PickValue_UnityEngine_Vector4;

		// Token: 0x04004417 RID: 17431
		private PickValue<Quaternion> FlowCanvas_Nodes_PickValue_UnityEngine_Quaternion;

		// Token: 0x04004418 RID: 17432
		private PickValue<Keyframe> FlowCanvas_Nodes_PickValue_UnityEngine_Keyframe;

		// Token: 0x04004419 RID: 17433
		private PickValue<Bounds> FlowCanvas_Nodes_PickValue_UnityEngine_Bounds;

		// Token: 0x0400441A RID: 17434
		private PickValue<Color> FlowCanvas_Nodes_PickValue_UnityEngine_Color;

		// Token: 0x0400441B RID: 17435
		private PickValue<Rect> FlowCanvas_Nodes_PickValue_UnityEngine_Rect;

		// Token: 0x0400441C RID: 17436
		private PickValue<ContactPoint> FlowCanvas_Nodes_PickValue_UnityEngine_ContactPoint;

		// Token: 0x0400441D RID: 17437
		private PickValue<ContactPoint2D> FlowCanvas_Nodes_PickValue_UnityEngine_ContactPoint2D;

		// Token: 0x0400441E RID: 17438
		private PickValue<RaycastHit> FlowCanvas_Nodes_PickValue_UnityEngine_RaycastHit;

		// Token: 0x0400441F RID: 17439
		private PickValue<RaycastHit2D> FlowCanvas_Nodes_PickValue_UnityEngine_RaycastHit2D;

		// Token: 0x04004420 RID: 17440
		private PickValue<Ray> FlowCanvas_Nodes_PickValue_UnityEngine_Ray;

		// Token: 0x04004421 RID: 17441
		private PickValue<Space> FlowCanvas_Nodes_PickValue_UnityEngine_Space;

		// Token: 0x04004422 RID: 17442
		private PickValue<LayerMask> FlowCanvas_Nodes_PickValue_UnityEngine_LayerMask;

		// Token: 0x04004423 RID: 17443
		private ReadFlowParameter<bool> FlowCanvas_Nodes_ReadFlowParameter_System_Boolean;

		// Token: 0x04004424 RID: 17444
		private ReadFlowParameter<float> FlowCanvas_Nodes_ReadFlowParameter_System_Single;

		// Token: 0x04004425 RID: 17445
		private ReadFlowParameter<int> FlowCanvas_Nodes_ReadFlowParameter_System_Int32;

		// Token: 0x04004426 RID: 17446
		private ReadFlowParameter<Vector2> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Vector2;

		// Token: 0x04004427 RID: 17447
		private ReadFlowParameter<Vector3> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Vector3;

		// Token: 0x04004428 RID: 17448
		private ReadFlowParameter<Vector4> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Vector4;

		// Token: 0x04004429 RID: 17449
		private ReadFlowParameter<Quaternion> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Quaternion;

		// Token: 0x0400442A RID: 17450
		private ReadFlowParameter<Keyframe> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Keyframe;

		// Token: 0x0400442B RID: 17451
		private ReadFlowParameter<Bounds> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Bounds;

		// Token: 0x0400442C RID: 17452
		private ReadFlowParameter<Color> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Color;

		// Token: 0x0400442D RID: 17453
		private ReadFlowParameter<Rect> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Rect;

		// Token: 0x0400442E RID: 17454
		private ReadFlowParameter<ContactPoint> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_ContactPoint;

		// Token: 0x0400442F RID: 17455
		private ReadFlowParameter<ContactPoint2D> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_ContactPoint2D;

		// Token: 0x04004430 RID: 17456
		private ReadFlowParameter<RaycastHit> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_RaycastHit;

		// Token: 0x04004431 RID: 17457
		private ReadFlowParameter<RaycastHit2D> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_RaycastHit2D;

		// Token: 0x04004432 RID: 17458
		private ReadFlowParameter<Ray> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Ray;

		// Token: 0x04004433 RID: 17459
		private ReadFlowParameter<Space> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_Space;

		// Token: 0x04004434 RID: 17460
		private ReadFlowParameter<LayerMask> FlowCanvas_Nodes_ReadFlowParameter_UnityEngine_LayerMask;

		// Token: 0x04004435 RID: 17461
		private ReflectedExtractorNodeWrapper<bool> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_System_Boolean;

		// Token: 0x04004436 RID: 17462
		private ReflectedExtractorNodeWrapper<float> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_System_Single;

		// Token: 0x04004437 RID: 17463
		private ReflectedExtractorNodeWrapper<int> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_System_Int32;

		// Token: 0x04004438 RID: 17464
		private ReflectedExtractorNodeWrapper<Vector2> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Vector2;

		// Token: 0x04004439 RID: 17465
		private ReflectedExtractorNodeWrapper<Vector3> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Vector3;

		// Token: 0x0400443A RID: 17466
		private ReflectedExtractorNodeWrapper<Vector4> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Vector4;

		// Token: 0x0400443B RID: 17467
		private ReflectedExtractorNodeWrapper<Quaternion> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Quaternion;

		// Token: 0x0400443C RID: 17468
		private ReflectedExtractorNodeWrapper<Keyframe> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Keyframe;

		// Token: 0x0400443D RID: 17469
		private ReflectedExtractorNodeWrapper<Bounds> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Bounds;

		// Token: 0x0400443E RID: 17470
		private ReflectedExtractorNodeWrapper<Color> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Color;

		// Token: 0x0400443F RID: 17471
		private ReflectedExtractorNodeWrapper<Rect> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Rect;

		// Token: 0x04004440 RID: 17472
		private ReflectedExtractorNodeWrapper<ContactPoint> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_ContactPoint;

		// Token: 0x04004441 RID: 17473
		private ReflectedExtractorNodeWrapper<ContactPoint2D> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_ContactPoint2D;

		// Token: 0x04004442 RID: 17474
		private ReflectedExtractorNodeWrapper<RaycastHit> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_RaycastHit;

		// Token: 0x04004443 RID: 17475
		private ReflectedExtractorNodeWrapper<RaycastHit2D> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_RaycastHit2D;

		// Token: 0x04004444 RID: 17476
		private ReflectedExtractorNodeWrapper<Ray> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Ray;

		// Token: 0x04004445 RID: 17477
		private ReflectedExtractorNodeWrapper<Space> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_Space;

		// Token: 0x04004446 RID: 17478
		private ReflectedExtractorNodeWrapper<LayerMask> FlowCanvas_Nodes_ReflectedExtractorNodeWrapper_UnityEngine_LayerMask;

		// Token: 0x04004447 RID: 17479
		private RelayValueInput<bool> FlowCanvas_Nodes_RelayValueInput_System_Boolean;

		// Token: 0x04004448 RID: 17480
		private RelayValueInput<float> FlowCanvas_Nodes_RelayValueInput_System_Single;

		// Token: 0x04004449 RID: 17481
		private RelayValueInput<int> FlowCanvas_Nodes_RelayValueInput_System_Int32;

		// Token: 0x0400444A RID: 17482
		private RelayValueInput<Vector2> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Vector2;

		// Token: 0x0400444B RID: 17483
		private RelayValueInput<Vector3> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Vector3;

		// Token: 0x0400444C RID: 17484
		private RelayValueInput<Vector4> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Vector4;

		// Token: 0x0400444D RID: 17485
		private RelayValueInput<Quaternion> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Quaternion;

		// Token: 0x0400444E RID: 17486
		private RelayValueInput<Keyframe> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Keyframe;

		// Token: 0x0400444F RID: 17487
		private RelayValueInput<Bounds> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Bounds;

		// Token: 0x04004450 RID: 17488
		private RelayValueInput<Color> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Color;

		// Token: 0x04004451 RID: 17489
		private RelayValueInput<Rect> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Rect;

		// Token: 0x04004452 RID: 17490
		private RelayValueInput<ContactPoint> FlowCanvas_Nodes_RelayValueInput_UnityEngine_ContactPoint;

		// Token: 0x04004453 RID: 17491
		private RelayValueInput<ContactPoint2D> FlowCanvas_Nodes_RelayValueInput_UnityEngine_ContactPoint2D;

		// Token: 0x04004454 RID: 17492
		private RelayValueInput<RaycastHit> FlowCanvas_Nodes_RelayValueInput_UnityEngine_RaycastHit;

		// Token: 0x04004455 RID: 17493
		private RelayValueInput<RaycastHit2D> FlowCanvas_Nodes_RelayValueInput_UnityEngine_RaycastHit2D;

		// Token: 0x04004456 RID: 17494
		private RelayValueInput<Ray> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Ray;

		// Token: 0x04004457 RID: 17495
		private RelayValueInput<Space> FlowCanvas_Nodes_RelayValueInput_UnityEngine_Space;

		// Token: 0x04004458 RID: 17496
		private RelayValueInput<LayerMask> FlowCanvas_Nodes_RelayValueInput_UnityEngine_LayerMask;

		// Token: 0x04004459 RID: 17497
		private RelayValueOutput<bool> FlowCanvas_Nodes_RelayValueOutput_System_Boolean;

		// Token: 0x0400445A RID: 17498
		private RelayValueOutput<float> FlowCanvas_Nodes_RelayValueOutput_System_Single;

		// Token: 0x0400445B RID: 17499
		private RelayValueOutput<int> FlowCanvas_Nodes_RelayValueOutput_System_Int32;

		// Token: 0x0400445C RID: 17500
		private RelayValueOutput<Vector2> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Vector2;

		// Token: 0x0400445D RID: 17501
		private RelayValueOutput<Vector3> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Vector3;

		// Token: 0x0400445E RID: 17502
		private RelayValueOutput<Vector4> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Vector4;

		// Token: 0x0400445F RID: 17503
		private RelayValueOutput<Quaternion> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Quaternion;

		// Token: 0x04004460 RID: 17504
		private RelayValueOutput<Keyframe> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Keyframe;

		// Token: 0x04004461 RID: 17505
		private RelayValueOutput<Bounds> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Bounds;

		// Token: 0x04004462 RID: 17506
		private RelayValueOutput<Color> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Color;

		// Token: 0x04004463 RID: 17507
		private RelayValueOutput<Rect> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Rect;

		// Token: 0x04004464 RID: 17508
		private RelayValueOutput<ContactPoint> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_ContactPoint;

		// Token: 0x04004465 RID: 17509
		private RelayValueOutput<ContactPoint2D> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_ContactPoint2D;

		// Token: 0x04004466 RID: 17510
		private RelayValueOutput<RaycastHit> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_RaycastHit;

		// Token: 0x04004467 RID: 17511
		private RelayValueOutput<RaycastHit2D> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_RaycastHit2D;

		// Token: 0x04004468 RID: 17512
		private RelayValueOutput<Ray> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Ray;

		// Token: 0x04004469 RID: 17513
		private RelayValueOutput<Space> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_Space;

		// Token: 0x0400446A RID: 17514
		private RelayValueOutput<LayerMask> FlowCanvas_Nodes_RelayValueOutput_UnityEngine_LayerMask;

		// Token: 0x0400446B RID: 17515
		private RemoveDictionaryKey<bool> FlowCanvas_Nodes_RemoveDictionaryKey_System_Boolean;

		// Token: 0x0400446C RID: 17516
		private RemoveDictionaryKey<float> FlowCanvas_Nodes_RemoveDictionaryKey_System_Single;

		// Token: 0x0400446D RID: 17517
		private RemoveDictionaryKey<int> FlowCanvas_Nodes_RemoveDictionaryKey_System_Int32;

		// Token: 0x0400446E RID: 17518
		private RemoveDictionaryKey<Vector2> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Vector2;

		// Token: 0x0400446F RID: 17519
		private RemoveDictionaryKey<Vector3> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Vector3;

		// Token: 0x04004470 RID: 17520
		private RemoveDictionaryKey<Vector4> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Vector4;

		// Token: 0x04004471 RID: 17521
		private RemoveDictionaryKey<Quaternion> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Quaternion;

		// Token: 0x04004472 RID: 17522
		private RemoveDictionaryKey<Keyframe> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Keyframe;

		// Token: 0x04004473 RID: 17523
		private RemoveDictionaryKey<Bounds> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Bounds;

		// Token: 0x04004474 RID: 17524
		private RemoveDictionaryKey<Color> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Color;

		// Token: 0x04004475 RID: 17525
		private RemoveDictionaryKey<Rect> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Rect;

		// Token: 0x04004476 RID: 17526
		private RemoveDictionaryKey<ContactPoint> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_ContactPoint;

		// Token: 0x04004477 RID: 17527
		private RemoveDictionaryKey<ContactPoint2D> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_ContactPoint2D;

		// Token: 0x04004478 RID: 17528
		private RemoveDictionaryKey<RaycastHit> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_RaycastHit;

		// Token: 0x04004479 RID: 17529
		private RemoveDictionaryKey<RaycastHit2D> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_RaycastHit2D;

		// Token: 0x0400447A RID: 17530
		private RemoveDictionaryKey<Ray> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Ray;

		// Token: 0x0400447B RID: 17531
		private RemoveDictionaryKey<Space> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_Space;

		// Token: 0x0400447C RID: 17532
		private RemoveDictionaryKey<LayerMask> FlowCanvas_Nodes_RemoveDictionaryKey_UnityEngine_LayerMask;

		// Token: 0x0400447D RID: 17533
		private RemoveListItem<bool> FlowCanvas_Nodes_RemoveListItem_System_Boolean;

		// Token: 0x0400447E RID: 17534
		private RemoveListItem<float> FlowCanvas_Nodes_RemoveListItem_System_Single;

		// Token: 0x0400447F RID: 17535
		private RemoveListItem<int> FlowCanvas_Nodes_RemoveListItem_System_Int32;

		// Token: 0x04004480 RID: 17536
		private RemoveListItem<Vector2> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Vector2;

		// Token: 0x04004481 RID: 17537
		private RemoveListItem<Vector3> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Vector3;

		// Token: 0x04004482 RID: 17538
		private RemoveListItem<Vector4> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Vector4;

		// Token: 0x04004483 RID: 17539
		private RemoveListItem<Quaternion> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Quaternion;

		// Token: 0x04004484 RID: 17540
		private RemoveListItem<Keyframe> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Keyframe;

		// Token: 0x04004485 RID: 17541
		private RemoveListItem<Bounds> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Bounds;

		// Token: 0x04004486 RID: 17542
		private RemoveListItem<Color> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Color;

		// Token: 0x04004487 RID: 17543
		private RemoveListItem<Rect> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Rect;

		// Token: 0x04004488 RID: 17544
		private RemoveListItem<ContactPoint> FlowCanvas_Nodes_RemoveListItem_UnityEngine_ContactPoint;

		// Token: 0x04004489 RID: 17545
		private RemoveListItem<ContactPoint2D> FlowCanvas_Nodes_RemoveListItem_UnityEngine_ContactPoint2D;

		// Token: 0x0400448A RID: 17546
		private RemoveListItem<RaycastHit> FlowCanvas_Nodes_RemoveListItem_UnityEngine_RaycastHit;

		// Token: 0x0400448B RID: 17547
		private RemoveListItem<RaycastHit2D> FlowCanvas_Nodes_RemoveListItem_UnityEngine_RaycastHit2D;

		// Token: 0x0400448C RID: 17548
		private RemoveListItem<Ray> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Ray;

		// Token: 0x0400448D RID: 17549
		private RemoveListItem<Space> FlowCanvas_Nodes_RemoveListItem_UnityEngine_Space;

		// Token: 0x0400448E RID: 17550
		private RemoveListItem<LayerMask> FlowCanvas_Nodes_RemoveListItem_UnityEngine_LayerMask;

		// Token: 0x0400448F RID: 17551
		private RemoveListItemAt<bool> FlowCanvas_Nodes_RemoveListItemAt_System_Boolean;

		// Token: 0x04004490 RID: 17552
		private RemoveListItemAt<float> FlowCanvas_Nodes_RemoveListItemAt_System_Single;

		// Token: 0x04004491 RID: 17553
		private RemoveListItemAt<int> FlowCanvas_Nodes_RemoveListItemAt_System_Int32;

		// Token: 0x04004492 RID: 17554
		private RemoveListItemAt<Vector2> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Vector2;

		// Token: 0x04004493 RID: 17555
		private RemoveListItemAt<Vector3> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Vector3;

		// Token: 0x04004494 RID: 17556
		private RemoveListItemAt<Vector4> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Vector4;

		// Token: 0x04004495 RID: 17557
		private RemoveListItemAt<Quaternion> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Quaternion;

		// Token: 0x04004496 RID: 17558
		private RemoveListItemAt<Keyframe> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Keyframe;

		// Token: 0x04004497 RID: 17559
		private RemoveListItemAt<Bounds> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Bounds;

		// Token: 0x04004498 RID: 17560
		private RemoveListItemAt<Color> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Color;

		// Token: 0x04004499 RID: 17561
		private RemoveListItemAt<Rect> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Rect;

		// Token: 0x0400449A RID: 17562
		private RemoveListItemAt<ContactPoint> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_ContactPoint;

		// Token: 0x0400449B RID: 17563
		private RemoveListItemAt<ContactPoint2D> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_ContactPoint2D;

		// Token: 0x0400449C RID: 17564
		private RemoveListItemAt<RaycastHit> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_RaycastHit;

		// Token: 0x0400449D RID: 17565
		private RemoveListItemAt<RaycastHit2D> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_RaycastHit2D;

		// Token: 0x0400449E RID: 17566
		private RemoveListItemAt<Ray> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Ray;

		// Token: 0x0400449F RID: 17567
		private RemoveListItemAt<Space> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_Space;

		// Token: 0x040044A0 RID: 17568
		private RemoveListItemAt<LayerMask> FlowCanvas_Nodes_RemoveListItemAt_UnityEngine_LayerMask;

		// Token: 0x040044A1 RID: 17569
		private SelectOnBool<bool> FlowCanvas_Nodes_SelectOnBool_System_Boolean;

		// Token: 0x040044A2 RID: 17570
		private SelectOnBool<float> FlowCanvas_Nodes_SelectOnBool_System_Single;

		// Token: 0x040044A3 RID: 17571
		private SelectOnBool<int> FlowCanvas_Nodes_SelectOnBool_System_Int32;

		// Token: 0x040044A4 RID: 17572
		private SelectOnBool<Vector2> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Vector2;

		// Token: 0x040044A5 RID: 17573
		private SelectOnBool<Vector3> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Vector3;

		// Token: 0x040044A6 RID: 17574
		private SelectOnBool<Vector4> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Vector4;

		// Token: 0x040044A7 RID: 17575
		private SelectOnBool<Quaternion> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Quaternion;

		// Token: 0x040044A8 RID: 17576
		private SelectOnBool<Keyframe> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Keyframe;

		// Token: 0x040044A9 RID: 17577
		private SelectOnBool<Bounds> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Bounds;

		// Token: 0x040044AA RID: 17578
		private SelectOnBool<Color> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Color;

		// Token: 0x040044AB RID: 17579
		private SelectOnBool<Rect> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Rect;

		// Token: 0x040044AC RID: 17580
		private SelectOnBool<ContactPoint> FlowCanvas_Nodes_SelectOnBool_UnityEngine_ContactPoint;

		// Token: 0x040044AD RID: 17581
		private SelectOnBool<ContactPoint2D> FlowCanvas_Nodes_SelectOnBool_UnityEngine_ContactPoint2D;

		// Token: 0x040044AE RID: 17582
		private SelectOnBool<RaycastHit> FlowCanvas_Nodes_SelectOnBool_UnityEngine_RaycastHit;

		// Token: 0x040044AF RID: 17583
		private SelectOnBool<RaycastHit2D> FlowCanvas_Nodes_SelectOnBool_UnityEngine_RaycastHit2D;

		// Token: 0x040044B0 RID: 17584
		private SelectOnBool<Ray> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Ray;

		// Token: 0x040044B1 RID: 17585
		private SelectOnBool<Space> FlowCanvas_Nodes_SelectOnBool_UnityEngine_Space;

		// Token: 0x040044B2 RID: 17586
		private SelectOnBool<LayerMask> FlowCanvas_Nodes_SelectOnBool_UnityEngine_LayerMask;

		// Token: 0x040044B3 RID: 17587
		private SelectOnEnum<bool> FlowCanvas_Nodes_SelectOnEnum_System_Boolean;

		// Token: 0x040044B4 RID: 17588
		private SelectOnEnum<float> FlowCanvas_Nodes_SelectOnEnum_System_Single;

		// Token: 0x040044B5 RID: 17589
		private SelectOnEnum<int> FlowCanvas_Nodes_SelectOnEnum_System_Int32;

		// Token: 0x040044B6 RID: 17590
		private SelectOnEnum<Vector2> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Vector2;

		// Token: 0x040044B7 RID: 17591
		private SelectOnEnum<Vector3> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Vector3;

		// Token: 0x040044B8 RID: 17592
		private SelectOnEnum<Vector4> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Vector4;

		// Token: 0x040044B9 RID: 17593
		private SelectOnEnum<Quaternion> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Quaternion;

		// Token: 0x040044BA RID: 17594
		private SelectOnEnum<Keyframe> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Keyframe;

		// Token: 0x040044BB RID: 17595
		private SelectOnEnum<Bounds> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Bounds;

		// Token: 0x040044BC RID: 17596
		private SelectOnEnum<Color> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Color;

		// Token: 0x040044BD RID: 17597
		private SelectOnEnum<Rect> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Rect;

		// Token: 0x040044BE RID: 17598
		private SelectOnEnum<ContactPoint> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_ContactPoint;

		// Token: 0x040044BF RID: 17599
		private SelectOnEnum<ContactPoint2D> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_ContactPoint2D;

		// Token: 0x040044C0 RID: 17600
		private SelectOnEnum<RaycastHit> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_RaycastHit;

		// Token: 0x040044C1 RID: 17601
		private SelectOnEnum<RaycastHit2D> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_RaycastHit2D;

		// Token: 0x040044C2 RID: 17602
		private SelectOnEnum<Ray> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Ray;

		// Token: 0x040044C3 RID: 17603
		private SelectOnEnum<Space> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_Space;

		// Token: 0x040044C4 RID: 17604
		private SelectOnEnum<LayerMask> FlowCanvas_Nodes_SelectOnEnum_UnityEngine_LayerMask;

		// Token: 0x040044C5 RID: 17605
		private SelectOnInt<bool> FlowCanvas_Nodes_SelectOnInt_System_Boolean;

		// Token: 0x040044C6 RID: 17606
		private SelectOnInt<float> FlowCanvas_Nodes_SelectOnInt_System_Single;

		// Token: 0x040044C7 RID: 17607
		private SelectOnInt<int> FlowCanvas_Nodes_SelectOnInt_System_Int32;

		// Token: 0x040044C8 RID: 17608
		private SelectOnInt<Vector2> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Vector2;

		// Token: 0x040044C9 RID: 17609
		private SelectOnInt<Vector3> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Vector3;

		// Token: 0x040044CA RID: 17610
		private SelectOnInt<Vector4> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Vector4;

		// Token: 0x040044CB RID: 17611
		private SelectOnInt<Quaternion> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Quaternion;

		// Token: 0x040044CC RID: 17612
		private SelectOnInt<Keyframe> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Keyframe;

		// Token: 0x040044CD RID: 17613
		private SelectOnInt<Bounds> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Bounds;

		// Token: 0x040044CE RID: 17614
		private SelectOnInt<Color> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Color;

		// Token: 0x040044CF RID: 17615
		private SelectOnInt<Rect> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Rect;

		// Token: 0x040044D0 RID: 17616
		private SelectOnInt<ContactPoint> FlowCanvas_Nodes_SelectOnInt_UnityEngine_ContactPoint;

		// Token: 0x040044D1 RID: 17617
		private SelectOnInt<ContactPoint2D> FlowCanvas_Nodes_SelectOnInt_UnityEngine_ContactPoint2D;

		// Token: 0x040044D2 RID: 17618
		private SelectOnInt<RaycastHit> FlowCanvas_Nodes_SelectOnInt_UnityEngine_RaycastHit;

		// Token: 0x040044D3 RID: 17619
		private SelectOnInt<RaycastHit2D> FlowCanvas_Nodes_SelectOnInt_UnityEngine_RaycastHit2D;

		// Token: 0x040044D4 RID: 17620
		private SelectOnInt<Ray> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Ray;

		// Token: 0x040044D5 RID: 17621
		private SelectOnInt<Space> FlowCanvas_Nodes_SelectOnInt_UnityEngine_Space;

		// Token: 0x040044D6 RID: 17622
		private SelectOnInt<LayerMask> FlowCanvas_Nodes_SelectOnInt_UnityEngine_LayerMask;

		// Token: 0x040044D7 RID: 17623
		private SelectOnString<bool> FlowCanvas_Nodes_SelectOnString_System_Boolean;

		// Token: 0x040044D8 RID: 17624
		private SelectOnString<float> FlowCanvas_Nodes_SelectOnString_System_Single;

		// Token: 0x040044D9 RID: 17625
		private SelectOnString<int> FlowCanvas_Nodes_SelectOnString_System_Int32;

		// Token: 0x040044DA RID: 17626
		private SelectOnString<Vector2> FlowCanvas_Nodes_SelectOnString_UnityEngine_Vector2;

		// Token: 0x040044DB RID: 17627
		private SelectOnString<Vector3> FlowCanvas_Nodes_SelectOnString_UnityEngine_Vector3;

		// Token: 0x040044DC RID: 17628
		private SelectOnString<Vector4> FlowCanvas_Nodes_SelectOnString_UnityEngine_Vector4;

		// Token: 0x040044DD RID: 17629
		private SelectOnString<Quaternion> FlowCanvas_Nodes_SelectOnString_UnityEngine_Quaternion;

		// Token: 0x040044DE RID: 17630
		private SelectOnString<Keyframe> FlowCanvas_Nodes_SelectOnString_UnityEngine_Keyframe;

		// Token: 0x040044DF RID: 17631
		private SelectOnString<Bounds> FlowCanvas_Nodes_SelectOnString_UnityEngine_Bounds;

		// Token: 0x040044E0 RID: 17632
		private SelectOnString<Color> FlowCanvas_Nodes_SelectOnString_UnityEngine_Color;

		// Token: 0x040044E1 RID: 17633
		private SelectOnString<Rect> FlowCanvas_Nodes_SelectOnString_UnityEngine_Rect;

		// Token: 0x040044E2 RID: 17634
		private SelectOnString<ContactPoint> FlowCanvas_Nodes_SelectOnString_UnityEngine_ContactPoint;

		// Token: 0x040044E3 RID: 17635
		private SelectOnString<ContactPoint2D> FlowCanvas_Nodes_SelectOnString_UnityEngine_ContactPoint2D;

		// Token: 0x040044E4 RID: 17636
		private SelectOnString<RaycastHit> FlowCanvas_Nodes_SelectOnString_UnityEngine_RaycastHit;

		// Token: 0x040044E5 RID: 17637
		private SelectOnString<RaycastHit2D> FlowCanvas_Nodes_SelectOnString_UnityEngine_RaycastHit2D;

		// Token: 0x040044E6 RID: 17638
		private SelectOnString<Ray> FlowCanvas_Nodes_SelectOnString_UnityEngine_Ray;

		// Token: 0x040044E7 RID: 17639
		private SelectOnString<Space> FlowCanvas_Nodes_SelectOnString_UnityEngine_Space;

		// Token: 0x040044E8 RID: 17640
		private SelectOnString<LayerMask> FlowCanvas_Nodes_SelectOnString_UnityEngine_LayerMask;

		// Token: 0x040044E9 RID: 17641
		private SelectOnTag<bool> FlowCanvas_Nodes_SelectOnTag_System_Boolean;

		// Token: 0x040044EA RID: 17642
		private SelectOnTag<float> FlowCanvas_Nodes_SelectOnTag_System_Single;

		// Token: 0x040044EB RID: 17643
		private SelectOnTag<int> FlowCanvas_Nodes_SelectOnTag_System_Int32;

		// Token: 0x040044EC RID: 17644
		private SelectOnTag<Vector2> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Vector2;

		// Token: 0x040044ED RID: 17645
		private SelectOnTag<Vector3> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Vector3;

		// Token: 0x040044EE RID: 17646
		private SelectOnTag<Vector4> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Vector4;

		// Token: 0x040044EF RID: 17647
		private SelectOnTag<Quaternion> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Quaternion;

		// Token: 0x040044F0 RID: 17648
		private SelectOnTag<Keyframe> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Keyframe;

		// Token: 0x040044F1 RID: 17649
		private SelectOnTag<Bounds> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Bounds;

		// Token: 0x040044F2 RID: 17650
		private SelectOnTag<Color> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Color;

		// Token: 0x040044F3 RID: 17651
		private SelectOnTag<Rect> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Rect;

		// Token: 0x040044F4 RID: 17652
		private SelectOnTag<ContactPoint> FlowCanvas_Nodes_SelectOnTag_UnityEngine_ContactPoint;

		// Token: 0x040044F5 RID: 17653
		private SelectOnTag<ContactPoint2D> FlowCanvas_Nodes_SelectOnTag_UnityEngine_ContactPoint2D;

		// Token: 0x040044F6 RID: 17654
		private SelectOnTag<RaycastHit> FlowCanvas_Nodes_SelectOnTag_UnityEngine_RaycastHit;

		// Token: 0x040044F7 RID: 17655
		private SelectOnTag<RaycastHit2D> FlowCanvas_Nodes_SelectOnTag_UnityEngine_RaycastHit2D;

		// Token: 0x040044F8 RID: 17656
		private SelectOnTag<Ray> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Ray;

		// Token: 0x040044F9 RID: 17657
		private SelectOnTag<Space> FlowCanvas_Nodes_SelectOnTag_UnityEngine_Space;

		// Token: 0x040044FA RID: 17658
		private SelectOnTag<LayerMask> FlowCanvas_Nodes_SelectOnTag_UnityEngine_LayerMask;

		// Token: 0x040044FB RID: 17659
		private global::FlowCanvas.Nodes.SendEvent<bool> FlowCanvas_Nodes_SendEvent_System_Boolean;

		// Token: 0x040044FC RID: 17660
		private global::FlowCanvas.Nodes.SendEvent<float> FlowCanvas_Nodes_SendEvent_System_Single;

		// Token: 0x040044FD RID: 17661
		private global::FlowCanvas.Nodes.SendEvent<int> FlowCanvas_Nodes_SendEvent_System_Int32;

		// Token: 0x040044FE RID: 17662
		private global::FlowCanvas.Nodes.SendEvent<Vector2> FlowCanvas_Nodes_SendEvent_UnityEngine_Vector2;

		// Token: 0x040044FF RID: 17663
		private global::FlowCanvas.Nodes.SendEvent<Vector3> FlowCanvas_Nodes_SendEvent_UnityEngine_Vector3;

		// Token: 0x04004500 RID: 17664
		private global::FlowCanvas.Nodes.SendEvent<Vector4> FlowCanvas_Nodes_SendEvent_UnityEngine_Vector4;

		// Token: 0x04004501 RID: 17665
		private global::FlowCanvas.Nodes.SendEvent<Quaternion> FlowCanvas_Nodes_SendEvent_UnityEngine_Quaternion;

		// Token: 0x04004502 RID: 17666
		private global::FlowCanvas.Nodes.SendEvent<Keyframe> FlowCanvas_Nodes_SendEvent_UnityEngine_Keyframe;

		// Token: 0x04004503 RID: 17667
		private global::FlowCanvas.Nodes.SendEvent<Bounds> FlowCanvas_Nodes_SendEvent_UnityEngine_Bounds;

		// Token: 0x04004504 RID: 17668
		private global::FlowCanvas.Nodes.SendEvent<Color> FlowCanvas_Nodes_SendEvent_UnityEngine_Color;

		// Token: 0x04004505 RID: 17669
		private global::FlowCanvas.Nodes.SendEvent<Rect> FlowCanvas_Nodes_SendEvent_UnityEngine_Rect;

		// Token: 0x04004506 RID: 17670
		private global::FlowCanvas.Nodes.SendEvent<ContactPoint> FlowCanvas_Nodes_SendEvent_UnityEngine_ContactPoint;

		// Token: 0x04004507 RID: 17671
		private global::FlowCanvas.Nodes.SendEvent<ContactPoint2D> FlowCanvas_Nodes_SendEvent_UnityEngine_ContactPoint2D;

		// Token: 0x04004508 RID: 17672
		private global::FlowCanvas.Nodes.SendEvent<RaycastHit> FlowCanvas_Nodes_SendEvent_UnityEngine_RaycastHit;

		// Token: 0x04004509 RID: 17673
		private global::FlowCanvas.Nodes.SendEvent<RaycastHit2D> FlowCanvas_Nodes_SendEvent_UnityEngine_RaycastHit2D;

		// Token: 0x0400450A RID: 17674
		private global::FlowCanvas.Nodes.SendEvent<Ray> FlowCanvas_Nodes_SendEvent_UnityEngine_Ray;

		// Token: 0x0400450B RID: 17675
		private global::FlowCanvas.Nodes.SendEvent<Space> FlowCanvas_Nodes_SendEvent_UnityEngine_Space;

		// Token: 0x0400450C RID: 17676
		private global::FlowCanvas.Nodes.SendEvent<LayerMask> FlowCanvas_Nodes_SendEvent_UnityEngine_LayerMask;

		// Token: 0x0400450D RID: 17677
		private SendGlobalEvent<bool> FlowCanvas_Nodes_SendGlobalEvent_System_Boolean;

		// Token: 0x0400450E RID: 17678
		private SendGlobalEvent<float> FlowCanvas_Nodes_SendGlobalEvent_System_Single;

		// Token: 0x0400450F RID: 17679
		private SendGlobalEvent<int> FlowCanvas_Nodes_SendGlobalEvent_System_Int32;

		// Token: 0x04004510 RID: 17680
		private SendGlobalEvent<Vector2> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Vector2;

		// Token: 0x04004511 RID: 17681
		private SendGlobalEvent<Vector3> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Vector3;

		// Token: 0x04004512 RID: 17682
		private SendGlobalEvent<Vector4> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Vector4;

		// Token: 0x04004513 RID: 17683
		private SendGlobalEvent<Quaternion> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Quaternion;

		// Token: 0x04004514 RID: 17684
		private SendGlobalEvent<Keyframe> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Keyframe;

		// Token: 0x04004515 RID: 17685
		private SendGlobalEvent<Bounds> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Bounds;

		// Token: 0x04004516 RID: 17686
		private SendGlobalEvent<Color> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Color;

		// Token: 0x04004517 RID: 17687
		private SendGlobalEvent<Rect> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Rect;

		// Token: 0x04004518 RID: 17688
		private SendGlobalEvent<ContactPoint> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_ContactPoint;

		// Token: 0x04004519 RID: 17689
		private SendGlobalEvent<ContactPoint2D> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_ContactPoint2D;

		// Token: 0x0400451A RID: 17690
		private SendGlobalEvent<RaycastHit> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_RaycastHit;

		// Token: 0x0400451B RID: 17691
		private SendGlobalEvent<RaycastHit2D> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_RaycastHit2D;

		// Token: 0x0400451C RID: 17692
		private SendGlobalEvent<Ray> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Ray;

		// Token: 0x0400451D RID: 17693
		private SendGlobalEvent<Space> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_Space;

		// Token: 0x0400451E RID: 17694
		private SendGlobalEvent<LayerMask> FlowCanvas_Nodes_SendGlobalEvent_UnityEngine_LayerMask;

		// Token: 0x0400451F RID: 17695
		private SetListItem<bool> FlowCanvas_Nodes_SetListItem_System_Boolean;

		// Token: 0x04004520 RID: 17696
		private SetListItem<float> FlowCanvas_Nodes_SetListItem_System_Single;

		// Token: 0x04004521 RID: 17697
		private SetListItem<int> FlowCanvas_Nodes_SetListItem_System_Int32;

		// Token: 0x04004522 RID: 17698
		private SetListItem<Vector2> FlowCanvas_Nodes_SetListItem_UnityEngine_Vector2;

		// Token: 0x04004523 RID: 17699
		private SetListItem<Vector3> FlowCanvas_Nodes_SetListItem_UnityEngine_Vector3;

		// Token: 0x04004524 RID: 17700
		private SetListItem<Vector4> FlowCanvas_Nodes_SetListItem_UnityEngine_Vector4;

		// Token: 0x04004525 RID: 17701
		private SetListItem<Quaternion> FlowCanvas_Nodes_SetListItem_UnityEngine_Quaternion;

		// Token: 0x04004526 RID: 17702
		private SetListItem<Keyframe> FlowCanvas_Nodes_SetListItem_UnityEngine_Keyframe;

		// Token: 0x04004527 RID: 17703
		private SetListItem<Bounds> FlowCanvas_Nodes_SetListItem_UnityEngine_Bounds;

		// Token: 0x04004528 RID: 17704
		private SetListItem<Color> FlowCanvas_Nodes_SetListItem_UnityEngine_Color;

		// Token: 0x04004529 RID: 17705
		private SetListItem<Rect> FlowCanvas_Nodes_SetListItem_UnityEngine_Rect;

		// Token: 0x0400452A RID: 17706
		private SetListItem<ContactPoint> FlowCanvas_Nodes_SetListItem_UnityEngine_ContactPoint;

		// Token: 0x0400452B RID: 17707
		private SetListItem<ContactPoint2D> FlowCanvas_Nodes_SetListItem_UnityEngine_ContactPoint2D;

		// Token: 0x0400452C RID: 17708
		private SetListItem<RaycastHit> FlowCanvas_Nodes_SetListItem_UnityEngine_RaycastHit;

		// Token: 0x0400452D RID: 17709
		private SetListItem<RaycastHit2D> FlowCanvas_Nodes_SetListItem_UnityEngine_RaycastHit2D;

		// Token: 0x0400452E RID: 17710
		private SetListItem<Ray> FlowCanvas_Nodes_SetListItem_UnityEngine_Ray;

		// Token: 0x0400452F RID: 17711
		private SetListItem<Space> FlowCanvas_Nodes_SetListItem_UnityEngine_Space;

		// Token: 0x04004530 RID: 17712
		private SetListItem<LayerMask> FlowCanvas_Nodes_SetListItem_UnityEngine_LayerMask;

		// Token: 0x04004531 RID: 17713
		private SetOtherVariable<bool> FlowCanvas_Nodes_SetOtherVariable_System_Boolean;

		// Token: 0x04004532 RID: 17714
		private SetOtherVariable<float> FlowCanvas_Nodes_SetOtherVariable_System_Single;

		// Token: 0x04004533 RID: 17715
		private SetOtherVariable<int> FlowCanvas_Nodes_SetOtherVariable_System_Int32;

		// Token: 0x04004534 RID: 17716
		private SetOtherVariable<Vector2> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Vector2;

		// Token: 0x04004535 RID: 17717
		private SetOtherVariable<Vector3> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Vector3;

		// Token: 0x04004536 RID: 17718
		private SetOtherVariable<Vector4> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Vector4;

		// Token: 0x04004537 RID: 17719
		private SetOtherVariable<Quaternion> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Quaternion;

		// Token: 0x04004538 RID: 17720
		private SetOtherVariable<Keyframe> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Keyframe;

		// Token: 0x04004539 RID: 17721
		private SetOtherVariable<Bounds> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Bounds;

		// Token: 0x0400453A RID: 17722
		private SetOtherVariable<Color> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Color;

		// Token: 0x0400453B RID: 17723
		private SetOtherVariable<Rect> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Rect;

		// Token: 0x0400453C RID: 17724
		private SetOtherVariable<ContactPoint> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_ContactPoint;

		// Token: 0x0400453D RID: 17725
		private SetOtherVariable<ContactPoint2D> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_ContactPoint2D;

		// Token: 0x0400453E RID: 17726
		private SetOtherVariable<RaycastHit> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_RaycastHit;

		// Token: 0x0400453F RID: 17727
		private SetOtherVariable<RaycastHit2D> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_RaycastHit2D;

		// Token: 0x04004540 RID: 17728
		private SetOtherVariable<Ray> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Ray;

		// Token: 0x04004541 RID: 17729
		private SetOtherVariable<Space> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_Space;

		// Token: 0x04004542 RID: 17730
		private SetOtherVariable<LayerMask> FlowCanvas_Nodes_SetOtherVariable_UnityEngine_LayerMask;

		// Token: 0x04004543 RID: 17731
		private global::FlowCanvas.Nodes.SetVariable<bool> FlowCanvas_Nodes_SetVariable_System_Boolean;

		// Token: 0x04004544 RID: 17732
		private global::FlowCanvas.Nodes.SetVariable<float> FlowCanvas_Nodes_SetVariable_System_Single;

		// Token: 0x04004545 RID: 17733
		private global::FlowCanvas.Nodes.SetVariable<int> FlowCanvas_Nodes_SetVariable_System_Int32;

		// Token: 0x04004546 RID: 17734
		private global::FlowCanvas.Nodes.SetVariable<Vector2> FlowCanvas_Nodes_SetVariable_UnityEngine_Vector2;

		// Token: 0x04004547 RID: 17735
		private global::FlowCanvas.Nodes.SetVariable<Vector3> FlowCanvas_Nodes_SetVariable_UnityEngine_Vector3;

		// Token: 0x04004548 RID: 17736
		private global::FlowCanvas.Nodes.SetVariable<Vector4> FlowCanvas_Nodes_SetVariable_UnityEngine_Vector4;

		// Token: 0x04004549 RID: 17737
		private global::FlowCanvas.Nodes.SetVariable<Quaternion> FlowCanvas_Nodes_SetVariable_UnityEngine_Quaternion;

		// Token: 0x0400454A RID: 17738
		private global::FlowCanvas.Nodes.SetVariable<Keyframe> FlowCanvas_Nodes_SetVariable_UnityEngine_Keyframe;

		// Token: 0x0400454B RID: 17739
		private global::FlowCanvas.Nodes.SetVariable<Bounds> FlowCanvas_Nodes_SetVariable_UnityEngine_Bounds;

		// Token: 0x0400454C RID: 17740
		private global::FlowCanvas.Nodes.SetVariable<Color> FlowCanvas_Nodes_SetVariable_UnityEngine_Color;

		// Token: 0x0400454D RID: 17741
		private global::FlowCanvas.Nodes.SetVariable<Rect> FlowCanvas_Nodes_SetVariable_UnityEngine_Rect;

		// Token: 0x0400454E RID: 17742
		private global::FlowCanvas.Nodes.SetVariable<ContactPoint> FlowCanvas_Nodes_SetVariable_UnityEngine_ContactPoint;

		// Token: 0x0400454F RID: 17743
		private global::FlowCanvas.Nodes.SetVariable<ContactPoint2D> FlowCanvas_Nodes_SetVariable_UnityEngine_ContactPoint2D;

		// Token: 0x04004550 RID: 17744
		private global::FlowCanvas.Nodes.SetVariable<RaycastHit> FlowCanvas_Nodes_SetVariable_UnityEngine_RaycastHit;

		// Token: 0x04004551 RID: 17745
		private global::FlowCanvas.Nodes.SetVariable<RaycastHit2D> FlowCanvas_Nodes_SetVariable_UnityEngine_RaycastHit2D;

		// Token: 0x04004552 RID: 17746
		private global::FlowCanvas.Nodes.SetVariable<Ray> FlowCanvas_Nodes_SetVariable_UnityEngine_Ray;

		// Token: 0x04004553 RID: 17747
		private global::FlowCanvas.Nodes.SetVariable<Space> FlowCanvas_Nodes_SetVariable_UnityEngine_Space;

		// Token: 0x04004554 RID: 17748
		private global::FlowCanvas.Nodes.SetVariable<LayerMask> FlowCanvas_Nodes_SetVariable_UnityEngine_LayerMask;

		// Token: 0x04004555 RID: 17749
		private ShuffleList<bool> FlowCanvas_Nodes_ShuffleList_System_Boolean;

		// Token: 0x04004556 RID: 17750
		private ShuffleList<float> FlowCanvas_Nodes_ShuffleList_System_Single;

		// Token: 0x04004557 RID: 17751
		private ShuffleList<int> FlowCanvas_Nodes_ShuffleList_System_Int32;

		// Token: 0x04004558 RID: 17752
		private ShuffleList<Vector2> FlowCanvas_Nodes_ShuffleList_UnityEngine_Vector2;

		// Token: 0x04004559 RID: 17753
		private ShuffleList<Vector3> FlowCanvas_Nodes_ShuffleList_UnityEngine_Vector3;

		// Token: 0x0400455A RID: 17754
		private ShuffleList<Vector4> FlowCanvas_Nodes_ShuffleList_UnityEngine_Vector4;

		// Token: 0x0400455B RID: 17755
		private ShuffleList<Quaternion> FlowCanvas_Nodes_ShuffleList_UnityEngine_Quaternion;

		// Token: 0x0400455C RID: 17756
		private ShuffleList<Keyframe> FlowCanvas_Nodes_ShuffleList_UnityEngine_Keyframe;

		// Token: 0x0400455D RID: 17757
		private ShuffleList<Bounds> FlowCanvas_Nodes_ShuffleList_UnityEngine_Bounds;

		// Token: 0x0400455E RID: 17758
		private ShuffleList<Color> FlowCanvas_Nodes_ShuffleList_UnityEngine_Color;

		// Token: 0x0400455F RID: 17759
		private ShuffleList<Rect> FlowCanvas_Nodes_ShuffleList_UnityEngine_Rect;

		// Token: 0x04004560 RID: 17760
		private ShuffleList<ContactPoint> FlowCanvas_Nodes_ShuffleList_UnityEngine_ContactPoint;

		// Token: 0x04004561 RID: 17761
		private ShuffleList<ContactPoint2D> FlowCanvas_Nodes_ShuffleList_UnityEngine_ContactPoint2D;

		// Token: 0x04004562 RID: 17762
		private ShuffleList<RaycastHit> FlowCanvas_Nodes_ShuffleList_UnityEngine_RaycastHit;

		// Token: 0x04004563 RID: 17763
		private ShuffleList<RaycastHit2D> FlowCanvas_Nodes_ShuffleList_UnityEngine_RaycastHit2D;

		// Token: 0x04004564 RID: 17764
		private ShuffleList<Ray> FlowCanvas_Nodes_ShuffleList_UnityEngine_Ray;

		// Token: 0x04004565 RID: 17765
		private ShuffleList<Space> FlowCanvas_Nodes_ShuffleList_UnityEngine_Space;

		// Token: 0x04004566 RID: 17766
		private ShuffleList<LayerMask> FlowCanvas_Nodes_ShuffleList_UnityEngine_LayerMask;

		// Token: 0x04004567 RID: 17767
		private StaticCodeEvent<bool> FlowCanvas_Nodes_StaticCodeEvent_System_Boolean;

		// Token: 0x04004568 RID: 17768
		private StaticCodeEvent<float> FlowCanvas_Nodes_StaticCodeEvent_System_Single;

		// Token: 0x04004569 RID: 17769
		private StaticCodeEvent<int> FlowCanvas_Nodes_StaticCodeEvent_System_Int32;

		// Token: 0x0400456A RID: 17770
		private StaticCodeEvent<Vector2> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Vector2;

		// Token: 0x0400456B RID: 17771
		private StaticCodeEvent<Vector3> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Vector3;

		// Token: 0x0400456C RID: 17772
		private StaticCodeEvent<Vector4> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Vector4;

		// Token: 0x0400456D RID: 17773
		private StaticCodeEvent<Quaternion> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Quaternion;

		// Token: 0x0400456E RID: 17774
		private StaticCodeEvent<Keyframe> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Keyframe;

		// Token: 0x0400456F RID: 17775
		private StaticCodeEvent<Bounds> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Bounds;

		// Token: 0x04004570 RID: 17776
		private StaticCodeEvent<Color> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Color;

		// Token: 0x04004571 RID: 17777
		private StaticCodeEvent<Rect> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Rect;

		// Token: 0x04004572 RID: 17778
		private StaticCodeEvent<ContactPoint> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_ContactPoint;

		// Token: 0x04004573 RID: 17779
		private StaticCodeEvent<ContactPoint2D> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_ContactPoint2D;

		// Token: 0x04004574 RID: 17780
		private StaticCodeEvent<RaycastHit> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_RaycastHit;

		// Token: 0x04004575 RID: 17781
		private StaticCodeEvent<RaycastHit2D> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_RaycastHit2D;

		// Token: 0x04004576 RID: 17782
		private StaticCodeEvent<Ray> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Ray;

		// Token: 0x04004577 RID: 17783
		private StaticCodeEvent<Space> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_Space;

		// Token: 0x04004578 RID: 17784
		private StaticCodeEvent<LayerMask> FlowCanvas_Nodes_StaticCodeEvent_UnityEngine_LayerMask;

		// Token: 0x04004579 RID: 17785
		private SwitchValue<bool> FlowCanvas_Nodes_SwitchValue_System_Boolean;

		// Token: 0x0400457A RID: 17786
		private SwitchValue<float> FlowCanvas_Nodes_SwitchValue_System_Single;

		// Token: 0x0400457B RID: 17787
		private SwitchValue<int> FlowCanvas_Nodes_SwitchValue_System_Int32;

		// Token: 0x0400457C RID: 17788
		private SwitchValue<Vector2> FlowCanvas_Nodes_SwitchValue_UnityEngine_Vector2;

		// Token: 0x0400457D RID: 17789
		private SwitchValue<Vector3> FlowCanvas_Nodes_SwitchValue_UnityEngine_Vector3;

		// Token: 0x0400457E RID: 17790
		private SwitchValue<Vector4> FlowCanvas_Nodes_SwitchValue_UnityEngine_Vector4;

		// Token: 0x0400457F RID: 17791
		private SwitchValue<Quaternion> FlowCanvas_Nodes_SwitchValue_UnityEngine_Quaternion;

		// Token: 0x04004580 RID: 17792
		private SwitchValue<Keyframe> FlowCanvas_Nodes_SwitchValue_UnityEngine_Keyframe;

		// Token: 0x04004581 RID: 17793
		private SwitchValue<Bounds> FlowCanvas_Nodes_SwitchValue_UnityEngine_Bounds;

		// Token: 0x04004582 RID: 17794
		private SwitchValue<Color> FlowCanvas_Nodes_SwitchValue_UnityEngine_Color;

		// Token: 0x04004583 RID: 17795
		private SwitchValue<Rect> FlowCanvas_Nodes_SwitchValue_UnityEngine_Rect;

		// Token: 0x04004584 RID: 17796
		private SwitchValue<ContactPoint> FlowCanvas_Nodes_SwitchValue_UnityEngine_ContactPoint;

		// Token: 0x04004585 RID: 17797
		private SwitchValue<ContactPoint2D> FlowCanvas_Nodes_SwitchValue_UnityEngine_ContactPoint2D;

		// Token: 0x04004586 RID: 17798
		private SwitchValue<RaycastHit> FlowCanvas_Nodes_SwitchValue_UnityEngine_RaycastHit;

		// Token: 0x04004587 RID: 17799
		private SwitchValue<RaycastHit2D> FlowCanvas_Nodes_SwitchValue_UnityEngine_RaycastHit2D;

		// Token: 0x04004588 RID: 17800
		private SwitchValue<Ray> FlowCanvas_Nodes_SwitchValue_UnityEngine_Ray;

		// Token: 0x04004589 RID: 17801
		private SwitchValue<Space> FlowCanvas_Nodes_SwitchValue_UnityEngine_Space;

		// Token: 0x0400458A RID: 17802
		private SwitchValue<LayerMask> FlowCanvas_Nodes_SwitchValue_UnityEngine_LayerMask;

		// Token: 0x0400458B RID: 17803
		private ToArray<bool> FlowCanvas_Nodes_ToArray_System_Boolean;

		// Token: 0x0400458C RID: 17804
		private ToArray<float> FlowCanvas_Nodes_ToArray_System_Single;

		// Token: 0x0400458D RID: 17805
		private ToArray<int> FlowCanvas_Nodes_ToArray_System_Int32;

		// Token: 0x0400458E RID: 17806
		private ToArray<Vector2> FlowCanvas_Nodes_ToArray_UnityEngine_Vector2;

		// Token: 0x0400458F RID: 17807
		private ToArray<Vector3> FlowCanvas_Nodes_ToArray_UnityEngine_Vector3;

		// Token: 0x04004590 RID: 17808
		private ToArray<Vector4> FlowCanvas_Nodes_ToArray_UnityEngine_Vector4;

		// Token: 0x04004591 RID: 17809
		private ToArray<Quaternion> FlowCanvas_Nodes_ToArray_UnityEngine_Quaternion;

		// Token: 0x04004592 RID: 17810
		private ToArray<Keyframe> FlowCanvas_Nodes_ToArray_UnityEngine_Keyframe;

		// Token: 0x04004593 RID: 17811
		private ToArray<Bounds> FlowCanvas_Nodes_ToArray_UnityEngine_Bounds;

		// Token: 0x04004594 RID: 17812
		private ToArray<Color> FlowCanvas_Nodes_ToArray_UnityEngine_Color;

		// Token: 0x04004595 RID: 17813
		private ToArray<Rect> FlowCanvas_Nodes_ToArray_UnityEngine_Rect;

		// Token: 0x04004596 RID: 17814
		private ToArray<ContactPoint> FlowCanvas_Nodes_ToArray_UnityEngine_ContactPoint;

		// Token: 0x04004597 RID: 17815
		private ToArray<ContactPoint2D> FlowCanvas_Nodes_ToArray_UnityEngine_ContactPoint2D;

		// Token: 0x04004598 RID: 17816
		private ToArray<RaycastHit> FlowCanvas_Nodes_ToArray_UnityEngine_RaycastHit;

		// Token: 0x04004599 RID: 17817
		private ToArray<RaycastHit2D> FlowCanvas_Nodes_ToArray_UnityEngine_RaycastHit2D;

		// Token: 0x0400459A RID: 17818
		private ToArray<Ray> FlowCanvas_Nodes_ToArray_UnityEngine_Ray;

		// Token: 0x0400459B RID: 17819
		private ToArray<Space> FlowCanvas_Nodes_ToArray_UnityEngine_Space;

		// Token: 0x0400459C RID: 17820
		private ToArray<LayerMask> FlowCanvas_Nodes_ToArray_UnityEngine_LayerMask;

		// Token: 0x0400459D RID: 17821
		private ToList<bool> FlowCanvas_Nodes_ToList_System_Boolean;

		// Token: 0x0400459E RID: 17822
		private ToList<float> FlowCanvas_Nodes_ToList_System_Single;

		// Token: 0x0400459F RID: 17823
		private ToList<int> FlowCanvas_Nodes_ToList_System_Int32;

		// Token: 0x040045A0 RID: 17824
		private ToList<Vector2> FlowCanvas_Nodes_ToList_UnityEngine_Vector2;

		// Token: 0x040045A1 RID: 17825
		private ToList<Vector3> FlowCanvas_Nodes_ToList_UnityEngine_Vector3;

		// Token: 0x040045A2 RID: 17826
		private ToList<Vector4> FlowCanvas_Nodes_ToList_UnityEngine_Vector4;

		// Token: 0x040045A3 RID: 17827
		private ToList<Quaternion> FlowCanvas_Nodes_ToList_UnityEngine_Quaternion;

		// Token: 0x040045A4 RID: 17828
		private ToList<Keyframe> FlowCanvas_Nodes_ToList_UnityEngine_Keyframe;

		// Token: 0x040045A5 RID: 17829
		private ToList<Bounds> FlowCanvas_Nodes_ToList_UnityEngine_Bounds;

		// Token: 0x040045A6 RID: 17830
		private ToList<Color> FlowCanvas_Nodes_ToList_UnityEngine_Color;

		// Token: 0x040045A7 RID: 17831
		private ToList<Rect> FlowCanvas_Nodes_ToList_UnityEngine_Rect;

		// Token: 0x040045A8 RID: 17832
		private ToList<ContactPoint> FlowCanvas_Nodes_ToList_UnityEngine_ContactPoint;

		// Token: 0x040045A9 RID: 17833
		private ToList<ContactPoint2D> FlowCanvas_Nodes_ToList_UnityEngine_ContactPoint2D;

		// Token: 0x040045AA RID: 17834
		private ToList<RaycastHit> FlowCanvas_Nodes_ToList_UnityEngine_RaycastHit;

		// Token: 0x040045AB RID: 17835
		private ToList<RaycastHit2D> FlowCanvas_Nodes_ToList_UnityEngine_RaycastHit2D;

		// Token: 0x040045AC RID: 17836
		private ToList<Ray> FlowCanvas_Nodes_ToList_UnityEngine_Ray;

		// Token: 0x040045AD RID: 17837
		private ToList<Space> FlowCanvas_Nodes_ToList_UnityEngine_Space;

		// Token: 0x040045AE RID: 17838
		private ToList<LayerMask> FlowCanvas_Nodes_ToList_UnityEngine_LayerMask;

		// Token: 0x040045AF RID: 17839
		private global::FlowCanvas.Nodes.TryGetValue<bool> FlowCanvas_Nodes_TryGetValue_System_Boolean;

		// Token: 0x040045B0 RID: 17840
		private global::FlowCanvas.Nodes.TryGetValue<float> FlowCanvas_Nodes_TryGetValue_System_Single;

		// Token: 0x040045B1 RID: 17841
		private global::FlowCanvas.Nodes.TryGetValue<int> FlowCanvas_Nodes_TryGetValue_System_Int32;

		// Token: 0x040045B2 RID: 17842
		private global::FlowCanvas.Nodes.TryGetValue<Vector2> FlowCanvas_Nodes_TryGetValue_UnityEngine_Vector2;

		// Token: 0x040045B3 RID: 17843
		private global::FlowCanvas.Nodes.TryGetValue<Vector3> FlowCanvas_Nodes_TryGetValue_UnityEngine_Vector3;

		// Token: 0x040045B4 RID: 17844
		private global::FlowCanvas.Nodes.TryGetValue<Vector4> FlowCanvas_Nodes_TryGetValue_UnityEngine_Vector4;

		// Token: 0x040045B5 RID: 17845
		private global::FlowCanvas.Nodes.TryGetValue<Quaternion> FlowCanvas_Nodes_TryGetValue_UnityEngine_Quaternion;

		// Token: 0x040045B6 RID: 17846
		private global::FlowCanvas.Nodes.TryGetValue<Keyframe> FlowCanvas_Nodes_TryGetValue_UnityEngine_Keyframe;

		// Token: 0x040045B7 RID: 17847
		private global::FlowCanvas.Nodes.TryGetValue<Bounds> FlowCanvas_Nodes_TryGetValue_UnityEngine_Bounds;

		// Token: 0x040045B8 RID: 17848
		private global::FlowCanvas.Nodes.TryGetValue<Color> FlowCanvas_Nodes_TryGetValue_UnityEngine_Color;

		// Token: 0x040045B9 RID: 17849
		private global::FlowCanvas.Nodes.TryGetValue<Rect> FlowCanvas_Nodes_TryGetValue_UnityEngine_Rect;

		// Token: 0x040045BA RID: 17850
		private global::FlowCanvas.Nodes.TryGetValue<ContactPoint> FlowCanvas_Nodes_TryGetValue_UnityEngine_ContactPoint;

		// Token: 0x040045BB RID: 17851
		private global::FlowCanvas.Nodes.TryGetValue<ContactPoint2D> FlowCanvas_Nodes_TryGetValue_UnityEngine_ContactPoint2D;

		// Token: 0x040045BC RID: 17852
		private global::FlowCanvas.Nodes.TryGetValue<RaycastHit> FlowCanvas_Nodes_TryGetValue_UnityEngine_RaycastHit;

		// Token: 0x040045BD RID: 17853
		private global::FlowCanvas.Nodes.TryGetValue<RaycastHit2D> FlowCanvas_Nodes_TryGetValue_UnityEngine_RaycastHit2D;

		// Token: 0x040045BE RID: 17854
		private global::FlowCanvas.Nodes.TryGetValue<Ray> FlowCanvas_Nodes_TryGetValue_UnityEngine_Ray;

		// Token: 0x040045BF RID: 17855
		private global::FlowCanvas.Nodes.TryGetValue<Space> FlowCanvas_Nodes_TryGetValue_UnityEngine_Space;

		// Token: 0x040045C0 RID: 17856
		private global::FlowCanvas.Nodes.TryGetValue<LayerMask> FlowCanvas_Nodes_TryGetValue_UnityEngine_LayerMask;

		// Token: 0x040045C1 RID: 17857
		private WriteFlowParameter<bool> FlowCanvas_Nodes_WriteFlowParameter_System_Boolean;

		// Token: 0x040045C2 RID: 17858
		private WriteFlowParameter<float> FlowCanvas_Nodes_WriteFlowParameter_System_Single;

		// Token: 0x040045C3 RID: 17859
		private WriteFlowParameter<int> FlowCanvas_Nodes_WriteFlowParameter_System_Int32;

		// Token: 0x040045C4 RID: 17860
		private WriteFlowParameter<Vector2> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Vector2;

		// Token: 0x040045C5 RID: 17861
		private WriteFlowParameter<Vector3> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Vector3;

		// Token: 0x040045C6 RID: 17862
		private WriteFlowParameter<Vector4> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Vector4;

		// Token: 0x040045C7 RID: 17863
		private WriteFlowParameter<Quaternion> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Quaternion;

		// Token: 0x040045C8 RID: 17864
		private WriteFlowParameter<Keyframe> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Keyframe;

		// Token: 0x040045C9 RID: 17865
		private WriteFlowParameter<Bounds> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Bounds;

		// Token: 0x040045CA RID: 17866
		private WriteFlowParameter<Color> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Color;

		// Token: 0x040045CB RID: 17867
		private WriteFlowParameter<Rect> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Rect;

		// Token: 0x040045CC RID: 17868
		private WriteFlowParameter<ContactPoint> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_ContactPoint;

		// Token: 0x040045CD RID: 17869
		private WriteFlowParameter<ContactPoint2D> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_ContactPoint2D;

		// Token: 0x040045CE RID: 17870
		private WriteFlowParameter<RaycastHit> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_RaycastHit;

		// Token: 0x040045CF RID: 17871
		private WriteFlowParameter<RaycastHit2D> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_RaycastHit2D;

		// Token: 0x040045D0 RID: 17872
		private WriteFlowParameter<Ray> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Ray;

		// Token: 0x040045D1 RID: 17873
		private WriteFlowParameter<Space> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_Space;

		// Token: 0x040045D2 RID: 17874
		private WriteFlowParameter<LayerMask> FlowCanvas_Nodes_WriteFlowParameter_UnityEngine_LayerMask;

		// Token: 0x040045D3 RID: 17875
		private BBParameter<bool> NodeCanvas_Framework_BBParameter_System_Boolean;

		// Token: 0x040045D4 RID: 17876
		private BBParameter<float> NodeCanvas_Framework_BBParameter_System_Single;

		// Token: 0x040045D5 RID: 17877
		private BBParameter<int> NodeCanvas_Framework_BBParameter_System_Int32;

		// Token: 0x040045D6 RID: 17878
		private BBParameter<Vector2> NodeCanvas_Framework_BBParameter_UnityEngine_Vector2;

		// Token: 0x040045D7 RID: 17879
		private BBParameter<Vector3> NodeCanvas_Framework_BBParameter_UnityEngine_Vector3;

		// Token: 0x040045D8 RID: 17880
		private BBParameter<Vector4> NodeCanvas_Framework_BBParameter_UnityEngine_Vector4;

		// Token: 0x040045D9 RID: 17881
		private BBParameter<Quaternion> NodeCanvas_Framework_BBParameter_UnityEngine_Quaternion;

		// Token: 0x040045DA RID: 17882
		private BBParameter<Keyframe> NodeCanvas_Framework_BBParameter_UnityEngine_Keyframe;

		// Token: 0x040045DB RID: 17883
		private BBParameter<Bounds> NodeCanvas_Framework_BBParameter_UnityEngine_Bounds;

		// Token: 0x040045DC RID: 17884
		private BBParameter<Color> NodeCanvas_Framework_BBParameter_UnityEngine_Color;

		// Token: 0x040045DD RID: 17885
		private BBParameter<Rect> NodeCanvas_Framework_BBParameter_UnityEngine_Rect;

		// Token: 0x040045DE RID: 17886
		private BBParameter<ContactPoint> NodeCanvas_Framework_BBParameter_UnityEngine_ContactPoint;

		// Token: 0x040045DF RID: 17887
		private BBParameter<ContactPoint2D> NodeCanvas_Framework_BBParameter_UnityEngine_ContactPoint2D;

		// Token: 0x040045E0 RID: 17888
		private BBParameter<RaycastHit> NodeCanvas_Framework_BBParameter_UnityEngine_RaycastHit;

		// Token: 0x040045E1 RID: 17889
		private BBParameter<RaycastHit2D> NodeCanvas_Framework_BBParameter_UnityEngine_RaycastHit2D;

		// Token: 0x040045E2 RID: 17890
		private BBParameter<Ray> NodeCanvas_Framework_BBParameter_UnityEngine_Ray;

		// Token: 0x040045E3 RID: 17891
		private BBParameter<Space> NodeCanvas_Framework_BBParameter_UnityEngine_Space;

		// Token: 0x040045E4 RID: 17892
		private BBParameter<LayerMask> NodeCanvas_Framework_BBParameter_UnityEngine_LayerMask;

		// Token: 0x040045E5 RID: 17893
		private ExposedParameter<bool> NodeCanvas_Framework_ExposedParameter_System_Boolean;

		// Token: 0x040045E6 RID: 17894
		private ExposedParameter<float> NodeCanvas_Framework_ExposedParameter_System_Single;

		// Token: 0x040045E7 RID: 17895
		private ExposedParameter<int> NodeCanvas_Framework_ExposedParameter_System_Int32;

		// Token: 0x040045E8 RID: 17896
		private ExposedParameter<Vector2> NodeCanvas_Framework_ExposedParameter_UnityEngine_Vector2;

		// Token: 0x040045E9 RID: 17897
		private ExposedParameter<Vector3> NodeCanvas_Framework_ExposedParameter_UnityEngine_Vector3;

		// Token: 0x040045EA RID: 17898
		private ExposedParameter<Vector4> NodeCanvas_Framework_ExposedParameter_UnityEngine_Vector4;

		// Token: 0x040045EB RID: 17899
		private ExposedParameter<Quaternion> NodeCanvas_Framework_ExposedParameter_UnityEngine_Quaternion;

		// Token: 0x040045EC RID: 17900
		private ExposedParameter<Keyframe> NodeCanvas_Framework_ExposedParameter_UnityEngine_Keyframe;

		// Token: 0x040045ED RID: 17901
		private ExposedParameter<Bounds> NodeCanvas_Framework_ExposedParameter_UnityEngine_Bounds;

		// Token: 0x040045EE RID: 17902
		private ExposedParameter<Color> NodeCanvas_Framework_ExposedParameter_UnityEngine_Color;

		// Token: 0x040045EF RID: 17903
		private ExposedParameter<Rect> NodeCanvas_Framework_ExposedParameter_UnityEngine_Rect;

		// Token: 0x040045F0 RID: 17904
		private ExposedParameter<ContactPoint> NodeCanvas_Framework_ExposedParameter_UnityEngine_ContactPoint;

		// Token: 0x040045F1 RID: 17905
		private ExposedParameter<ContactPoint2D> NodeCanvas_Framework_ExposedParameter_UnityEngine_ContactPoint2D;

		// Token: 0x040045F2 RID: 17906
		private ExposedParameter<RaycastHit> NodeCanvas_Framework_ExposedParameter_UnityEngine_RaycastHit;

		// Token: 0x040045F3 RID: 17907
		private ExposedParameter<RaycastHit2D> NodeCanvas_Framework_ExposedParameter_UnityEngine_RaycastHit2D;

		// Token: 0x040045F4 RID: 17908
		private ExposedParameter<Ray> NodeCanvas_Framework_ExposedParameter_UnityEngine_Ray;

		// Token: 0x040045F5 RID: 17909
		private ExposedParameter<Space> NodeCanvas_Framework_ExposedParameter_UnityEngine_Space;

		// Token: 0x040045F6 RID: 17910
		private ExposedParameter<LayerMask> NodeCanvas_Framework_ExposedParameter_UnityEngine_LayerMask;

		// Token: 0x040045F7 RID: 17911
		private Variable<bool> NodeCanvas_Framework_Variable_System_Boolean;

		// Token: 0x040045F8 RID: 17912
		private Variable<float> NodeCanvas_Framework_Variable_System_Single;

		// Token: 0x040045F9 RID: 17913
		private Variable<int> NodeCanvas_Framework_Variable_System_Int32;

		// Token: 0x040045FA RID: 17914
		private Variable<Vector2> NodeCanvas_Framework_Variable_UnityEngine_Vector2;

		// Token: 0x040045FB RID: 17915
		private Variable<Vector3> NodeCanvas_Framework_Variable_UnityEngine_Vector3;

		// Token: 0x040045FC RID: 17916
		private Variable<Vector4> NodeCanvas_Framework_Variable_UnityEngine_Vector4;

		// Token: 0x040045FD RID: 17917
		private Variable<Quaternion> NodeCanvas_Framework_Variable_UnityEngine_Quaternion;

		// Token: 0x040045FE RID: 17918
		private Variable<Keyframe> NodeCanvas_Framework_Variable_UnityEngine_Keyframe;

		// Token: 0x040045FF RID: 17919
		private Variable<Bounds> NodeCanvas_Framework_Variable_UnityEngine_Bounds;

		// Token: 0x04004600 RID: 17920
		private Variable<Color> NodeCanvas_Framework_Variable_UnityEngine_Color;

		// Token: 0x04004601 RID: 17921
		private Variable<Rect> NodeCanvas_Framework_Variable_UnityEngine_Rect;

		// Token: 0x04004602 RID: 17922
		private Variable<ContactPoint> NodeCanvas_Framework_Variable_UnityEngine_ContactPoint;

		// Token: 0x04004603 RID: 17923
		private Variable<ContactPoint2D> NodeCanvas_Framework_Variable_UnityEngine_ContactPoint2D;

		// Token: 0x04004604 RID: 17924
		private Variable<RaycastHit> NodeCanvas_Framework_Variable_UnityEngine_RaycastHit;

		// Token: 0x04004605 RID: 17925
		private Variable<RaycastHit2D> NodeCanvas_Framework_Variable_UnityEngine_RaycastHit2D;

		// Token: 0x04004606 RID: 17926
		private Variable<Ray> NodeCanvas_Framework_Variable_UnityEngine_Ray;

		// Token: 0x04004607 RID: 17927
		private Variable<Space> NodeCanvas_Framework_Variable_UnityEngine_Space;

		// Token: 0x04004608 RID: 17928
		private Variable<LayerMask> NodeCanvas_Framework_Variable_UnityEngine_LayerMask;

		// Token: 0x04004609 RID: 17929
		private ReflectedAction<bool> NodeCanvas_Framework_Internal_ReflectedAction_System_Boolean;

		// Token: 0x0400460A RID: 17930
		private ReflectedAction<float> NodeCanvas_Framework_Internal_ReflectedAction_System_Single;

		// Token: 0x0400460B RID: 17931
		private ReflectedAction<int> NodeCanvas_Framework_Internal_ReflectedAction_System_Int32;

		// Token: 0x0400460C RID: 17932
		private ReflectedAction<Vector2> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Vector2;

		// Token: 0x0400460D RID: 17933
		private ReflectedAction<Vector3> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Vector3;

		// Token: 0x0400460E RID: 17934
		private ReflectedAction<Vector4> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Vector4;

		// Token: 0x0400460F RID: 17935
		private ReflectedAction<Quaternion> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Quaternion;

		// Token: 0x04004610 RID: 17936
		private ReflectedAction<Keyframe> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Keyframe;

		// Token: 0x04004611 RID: 17937
		private ReflectedAction<Bounds> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Bounds;

		// Token: 0x04004612 RID: 17938
		private ReflectedAction<Color> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Color;

		// Token: 0x04004613 RID: 17939
		private ReflectedAction<Rect> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Rect;

		// Token: 0x04004614 RID: 17940
		private ReflectedAction<ContactPoint> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_ContactPoint;

		// Token: 0x04004615 RID: 17941
		private ReflectedAction<ContactPoint2D> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_ContactPoint2D;

		// Token: 0x04004616 RID: 17942
		private ReflectedAction<RaycastHit> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_RaycastHit;

		// Token: 0x04004617 RID: 17943
		private ReflectedAction<RaycastHit2D> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_RaycastHit2D;

		// Token: 0x04004618 RID: 17944
		private ReflectedAction<Ray> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Ray;

		// Token: 0x04004619 RID: 17945
		private ReflectedAction<Space> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_Space;

		// Token: 0x0400461A RID: 17946
		private ReflectedAction<LayerMask> NodeCanvas_Framework_Internal_ReflectedAction_UnityEngine_LayerMask;

		// Token: 0x0400461B RID: 17947
		private ReflectedFunction<bool> NodeCanvas_Framework_Internal_ReflectedFunction_System_Boolean;

		// Token: 0x0400461C RID: 17948
		private ReflectedFunction<float> NodeCanvas_Framework_Internal_ReflectedFunction_System_Single;

		// Token: 0x0400461D RID: 17949
		private ReflectedFunction<int> NodeCanvas_Framework_Internal_ReflectedFunction_System_Int32;

		// Token: 0x0400461E RID: 17950
		private ReflectedFunction<Vector2> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Vector2;

		// Token: 0x0400461F RID: 17951
		private ReflectedFunction<Vector3> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Vector3;

		// Token: 0x04004620 RID: 17952
		private ReflectedFunction<Vector4> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Vector4;

		// Token: 0x04004621 RID: 17953
		private ReflectedFunction<Quaternion> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Quaternion;

		// Token: 0x04004622 RID: 17954
		private ReflectedFunction<Keyframe> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Keyframe;

		// Token: 0x04004623 RID: 17955
		private ReflectedFunction<Bounds> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Bounds;

		// Token: 0x04004624 RID: 17956
		private ReflectedFunction<Color> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Color;

		// Token: 0x04004625 RID: 17957
		private ReflectedFunction<Rect> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Rect;

		// Token: 0x04004626 RID: 17958
		private ReflectedFunction<ContactPoint> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_ContactPoint;

		// Token: 0x04004627 RID: 17959
		private ReflectedFunction<ContactPoint2D> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_ContactPoint2D;

		// Token: 0x04004628 RID: 17960
		private ReflectedFunction<RaycastHit> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_RaycastHit;

		// Token: 0x04004629 RID: 17961
		private ReflectedFunction<RaycastHit2D> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_RaycastHit2D;

		// Token: 0x0400462A RID: 17962
		private ReflectedFunction<Ray> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Ray;

		// Token: 0x0400462B RID: 17963
		private ReflectedFunction<Space> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_Space;

		// Token: 0x0400462C RID: 17964
		private ReflectedFunction<LayerMask> NodeCanvas_Framework_Internal_ReflectedFunction_UnityEngine_LayerMask;

		// Token: 0x0400462D RID: 17965
		private AddElementToDictionary<bool> NodeCanvas_Tasks_Actions_AddElementToDictionary_System_Boolean;

		// Token: 0x0400462E RID: 17966
		private AddElementToDictionary<float> NodeCanvas_Tasks_Actions_AddElementToDictionary_System_Single;

		// Token: 0x0400462F RID: 17967
		private AddElementToDictionary<int> NodeCanvas_Tasks_Actions_AddElementToDictionary_System_Int32;

		// Token: 0x04004630 RID: 17968
		private AddElementToDictionary<Vector2> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Vector2;

		// Token: 0x04004631 RID: 17969
		private AddElementToDictionary<Vector3> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Vector3;

		// Token: 0x04004632 RID: 17970
		private AddElementToDictionary<Vector4> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Vector4;

		// Token: 0x04004633 RID: 17971
		private AddElementToDictionary<Quaternion> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Quaternion;

		// Token: 0x04004634 RID: 17972
		private AddElementToDictionary<Keyframe> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Keyframe;

		// Token: 0x04004635 RID: 17973
		private AddElementToDictionary<Bounds> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Bounds;

		// Token: 0x04004636 RID: 17974
		private AddElementToDictionary<Color> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Color;

		// Token: 0x04004637 RID: 17975
		private AddElementToDictionary<Rect> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Rect;

		// Token: 0x04004638 RID: 17976
		private AddElementToDictionary<ContactPoint> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_ContactPoint;

		// Token: 0x04004639 RID: 17977
		private AddElementToDictionary<ContactPoint2D> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_ContactPoint2D;

		// Token: 0x0400463A RID: 17978
		private AddElementToDictionary<RaycastHit> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_RaycastHit;

		// Token: 0x0400463B RID: 17979
		private AddElementToDictionary<RaycastHit2D> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_RaycastHit2D;

		// Token: 0x0400463C RID: 17980
		private AddElementToDictionary<Ray> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Ray;

		// Token: 0x0400463D RID: 17981
		private AddElementToDictionary<Space> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_Space;

		// Token: 0x0400463E RID: 17982
		private AddElementToDictionary<LayerMask> NodeCanvas_Tasks_Actions_AddElementToDictionary_UnityEngine_LayerMask;

		// Token: 0x0400463F RID: 17983
		private AddElementToList<bool> NodeCanvas_Tasks_Actions_AddElementToList_System_Boolean;

		// Token: 0x04004640 RID: 17984
		private AddElementToList<float> NodeCanvas_Tasks_Actions_AddElementToList_System_Single;

		// Token: 0x04004641 RID: 17985
		private AddElementToList<int> NodeCanvas_Tasks_Actions_AddElementToList_System_Int32;

		// Token: 0x04004642 RID: 17986
		private AddElementToList<Vector2> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Vector2;

		// Token: 0x04004643 RID: 17987
		private AddElementToList<Vector3> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Vector3;

		// Token: 0x04004644 RID: 17988
		private AddElementToList<Vector4> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Vector4;

		// Token: 0x04004645 RID: 17989
		private AddElementToList<Quaternion> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Quaternion;

		// Token: 0x04004646 RID: 17990
		private AddElementToList<Keyframe> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Keyframe;

		// Token: 0x04004647 RID: 17991
		private AddElementToList<Bounds> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Bounds;

		// Token: 0x04004648 RID: 17992
		private AddElementToList<Color> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Color;

		// Token: 0x04004649 RID: 17993
		private AddElementToList<Rect> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Rect;

		// Token: 0x0400464A RID: 17994
		private AddElementToList<ContactPoint> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_ContactPoint;

		// Token: 0x0400464B RID: 17995
		private AddElementToList<ContactPoint2D> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_ContactPoint2D;

		// Token: 0x0400464C RID: 17996
		private AddElementToList<RaycastHit> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_RaycastHit;

		// Token: 0x0400464D RID: 17997
		private AddElementToList<RaycastHit2D> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_RaycastHit2D;

		// Token: 0x0400464E RID: 17998
		private AddElementToList<Ray> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Ray;

		// Token: 0x0400464F RID: 17999
		private AddElementToList<Space> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_Space;

		// Token: 0x04004650 RID: 18000
		private AddElementToList<LayerMask> NodeCanvas_Tasks_Actions_AddElementToList_UnityEngine_LayerMask;

		// Token: 0x04004651 RID: 18001
		private GetDictionaryElement<bool> NodeCanvas_Tasks_Actions_GetDictionaryElement_System_Boolean;

		// Token: 0x04004652 RID: 18002
		private GetDictionaryElement<float> NodeCanvas_Tasks_Actions_GetDictionaryElement_System_Single;

		// Token: 0x04004653 RID: 18003
		private GetDictionaryElement<int> NodeCanvas_Tasks_Actions_GetDictionaryElement_System_Int32;

		// Token: 0x04004654 RID: 18004
		private GetDictionaryElement<Vector2> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Vector2;

		// Token: 0x04004655 RID: 18005
		private GetDictionaryElement<Vector3> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Vector3;

		// Token: 0x04004656 RID: 18006
		private GetDictionaryElement<Vector4> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Vector4;

		// Token: 0x04004657 RID: 18007
		private GetDictionaryElement<Quaternion> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Quaternion;

		// Token: 0x04004658 RID: 18008
		private GetDictionaryElement<Keyframe> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Keyframe;

		// Token: 0x04004659 RID: 18009
		private GetDictionaryElement<Bounds> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Bounds;

		// Token: 0x0400465A RID: 18010
		private GetDictionaryElement<Color> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Color;

		// Token: 0x0400465B RID: 18011
		private GetDictionaryElement<Rect> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Rect;

		// Token: 0x0400465C RID: 18012
		private GetDictionaryElement<ContactPoint> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_ContactPoint;

		// Token: 0x0400465D RID: 18013
		private GetDictionaryElement<ContactPoint2D> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_ContactPoint2D;

		// Token: 0x0400465E RID: 18014
		private GetDictionaryElement<RaycastHit> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_RaycastHit;

		// Token: 0x0400465F RID: 18015
		private GetDictionaryElement<RaycastHit2D> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_RaycastHit2D;

		// Token: 0x04004660 RID: 18016
		private GetDictionaryElement<Ray> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Ray;

		// Token: 0x04004661 RID: 18017
		private GetDictionaryElement<Space> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_Space;

		// Token: 0x04004662 RID: 18018
		private GetDictionaryElement<LayerMask> NodeCanvas_Tasks_Actions_GetDictionaryElement_UnityEngine_LayerMask;

		// Token: 0x04004663 RID: 18019
		private GetIndexOfElement<bool> NodeCanvas_Tasks_Actions_GetIndexOfElement_System_Boolean;

		// Token: 0x04004664 RID: 18020
		private GetIndexOfElement<float> NodeCanvas_Tasks_Actions_GetIndexOfElement_System_Single;

		// Token: 0x04004665 RID: 18021
		private GetIndexOfElement<int> NodeCanvas_Tasks_Actions_GetIndexOfElement_System_Int32;

		// Token: 0x04004666 RID: 18022
		private GetIndexOfElement<Vector2> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Vector2;

		// Token: 0x04004667 RID: 18023
		private GetIndexOfElement<Vector3> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Vector3;

		// Token: 0x04004668 RID: 18024
		private GetIndexOfElement<Vector4> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Vector4;

		// Token: 0x04004669 RID: 18025
		private GetIndexOfElement<Quaternion> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Quaternion;

		// Token: 0x0400466A RID: 18026
		private GetIndexOfElement<Keyframe> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Keyframe;

		// Token: 0x0400466B RID: 18027
		private GetIndexOfElement<Bounds> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Bounds;

		// Token: 0x0400466C RID: 18028
		private GetIndexOfElement<Color> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Color;

		// Token: 0x0400466D RID: 18029
		private GetIndexOfElement<Rect> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Rect;

		// Token: 0x0400466E RID: 18030
		private GetIndexOfElement<ContactPoint> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_ContactPoint;

		// Token: 0x0400466F RID: 18031
		private GetIndexOfElement<ContactPoint2D> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_ContactPoint2D;

		// Token: 0x04004670 RID: 18032
		private GetIndexOfElement<RaycastHit> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_RaycastHit;

		// Token: 0x04004671 RID: 18033
		private GetIndexOfElement<RaycastHit2D> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_RaycastHit2D;

		// Token: 0x04004672 RID: 18034
		private GetIndexOfElement<Ray> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Ray;

		// Token: 0x04004673 RID: 18035
		private GetIndexOfElement<Space> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_Space;

		// Token: 0x04004674 RID: 18036
		private GetIndexOfElement<LayerMask> NodeCanvas_Tasks_Actions_GetIndexOfElement_UnityEngine_LayerMask;

		// Token: 0x04004675 RID: 18037
		private InsertElementToList<bool> NodeCanvas_Tasks_Actions_InsertElementToList_System_Boolean;

		// Token: 0x04004676 RID: 18038
		private InsertElementToList<float> NodeCanvas_Tasks_Actions_InsertElementToList_System_Single;

		// Token: 0x04004677 RID: 18039
		private InsertElementToList<int> NodeCanvas_Tasks_Actions_InsertElementToList_System_Int32;

		// Token: 0x04004678 RID: 18040
		private InsertElementToList<Vector2> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Vector2;

		// Token: 0x04004679 RID: 18041
		private InsertElementToList<Vector3> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Vector3;

		// Token: 0x0400467A RID: 18042
		private InsertElementToList<Vector4> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Vector4;

		// Token: 0x0400467B RID: 18043
		private InsertElementToList<Quaternion> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Quaternion;

		// Token: 0x0400467C RID: 18044
		private InsertElementToList<Keyframe> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Keyframe;

		// Token: 0x0400467D RID: 18045
		private InsertElementToList<Bounds> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Bounds;

		// Token: 0x0400467E RID: 18046
		private InsertElementToList<Color> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Color;

		// Token: 0x0400467F RID: 18047
		private InsertElementToList<Rect> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Rect;

		// Token: 0x04004680 RID: 18048
		private InsertElementToList<ContactPoint> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_ContactPoint;

		// Token: 0x04004681 RID: 18049
		private InsertElementToList<ContactPoint2D> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_ContactPoint2D;

		// Token: 0x04004682 RID: 18050
		private InsertElementToList<RaycastHit> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_RaycastHit;

		// Token: 0x04004683 RID: 18051
		private InsertElementToList<RaycastHit2D> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_RaycastHit2D;

		// Token: 0x04004684 RID: 18052
		private InsertElementToList<Ray> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Ray;

		// Token: 0x04004685 RID: 18053
		private InsertElementToList<Space> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_Space;

		// Token: 0x04004686 RID: 18054
		private InsertElementToList<LayerMask> NodeCanvas_Tasks_Actions_InsertElementToList_UnityEngine_LayerMask;

		// Token: 0x04004687 RID: 18055
		private PickListElement<bool> NodeCanvas_Tasks_Actions_PickListElement_System_Boolean;

		// Token: 0x04004688 RID: 18056
		private PickListElement<float> NodeCanvas_Tasks_Actions_PickListElement_System_Single;

		// Token: 0x04004689 RID: 18057
		private PickListElement<int> NodeCanvas_Tasks_Actions_PickListElement_System_Int32;

		// Token: 0x0400468A RID: 18058
		private PickListElement<Vector2> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Vector2;

		// Token: 0x0400468B RID: 18059
		private PickListElement<Vector3> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Vector3;

		// Token: 0x0400468C RID: 18060
		private PickListElement<Vector4> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Vector4;

		// Token: 0x0400468D RID: 18061
		private PickListElement<Quaternion> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Quaternion;

		// Token: 0x0400468E RID: 18062
		private PickListElement<Keyframe> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Keyframe;

		// Token: 0x0400468F RID: 18063
		private PickListElement<Bounds> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Bounds;

		// Token: 0x04004690 RID: 18064
		private PickListElement<Color> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Color;

		// Token: 0x04004691 RID: 18065
		private PickListElement<Rect> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Rect;

		// Token: 0x04004692 RID: 18066
		private PickListElement<ContactPoint> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_ContactPoint;

		// Token: 0x04004693 RID: 18067
		private PickListElement<ContactPoint2D> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_ContactPoint2D;

		// Token: 0x04004694 RID: 18068
		private PickListElement<RaycastHit> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_RaycastHit;

		// Token: 0x04004695 RID: 18069
		private PickListElement<RaycastHit2D> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_RaycastHit2D;

		// Token: 0x04004696 RID: 18070
		private PickListElement<Ray> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Ray;

		// Token: 0x04004697 RID: 18071
		private PickListElement<Space> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_Space;

		// Token: 0x04004698 RID: 18072
		private PickListElement<LayerMask> NodeCanvas_Tasks_Actions_PickListElement_UnityEngine_LayerMask;

		// Token: 0x04004699 RID: 18073
		private PickRandomListElement<bool> NodeCanvas_Tasks_Actions_PickRandomListElement_System_Boolean;

		// Token: 0x0400469A RID: 18074
		private PickRandomListElement<float> NodeCanvas_Tasks_Actions_PickRandomListElement_System_Single;

		// Token: 0x0400469B RID: 18075
		private PickRandomListElement<int> NodeCanvas_Tasks_Actions_PickRandomListElement_System_Int32;

		// Token: 0x0400469C RID: 18076
		private PickRandomListElement<Vector2> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Vector2;

		// Token: 0x0400469D RID: 18077
		private PickRandomListElement<Vector3> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Vector3;

		// Token: 0x0400469E RID: 18078
		private PickRandomListElement<Vector4> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Vector4;

		// Token: 0x0400469F RID: 18079
		private PickRandomListElement<Quaternion> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Quaternion;

		// Token: 0x040046A0 RID: 18080
		private PickRandomListElement<Keyframe> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Keyframe;

		// Token: 0x040046A1 RID: 18081
		private PickRandomListElement<Bounds> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Bounds;

		// Token: 0x040046A2 RID: 18082
		private PickRandomListElement<Color> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Color;

		// Token: 0x040046A3 RID: 18083
		private PickRandomListElement<Rect> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Rect;

		// Token: 0x040046A4 RID: 18084
		private PickRandomListElement<ContactPoint> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_ContactPoint;

		// Token: 0x040046A5 RID: 18085
		private PickRandomListElement<ContactPoint2D> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_ContactPoint2D;

		// Token: 0x040046A6 RID: 18086
		private PickRandomListElement<RaycastHit> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_RaycastHit;

		// Token: 0x040046A7 RID: 18087
		private PickRandomListElement<RaycastHit2D> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_RaycastHit2D;

		// Token: 0x040046A8 RID: 18088
		private PickRandomListElement<Ray> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Ray;

		// Token: 0x040046A9 RID: 18089
		private PickRandomListElement<Space> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_Space;

		// Token: 0x040046AA RID: 18090
		private PickRandomListElement<LayerMask> NodeCanvas_Tasks_Actions_PickRandomListElement_UnityEngine_LayerMask;

		// Token: 0x040046AB RID: 18091
		private RemoveElementFromList<bool> NodeCanvas_Tasks_Actions_RemoveElementFromList_System_Boolean;

		// Token: 0x040046AC RID: 18092
		private RemoveElementFromList<float> NodeCanvas_Tasks_Actions_RemoveElementFromList_System_Single;

		// Token: 0x040046AD RID: 18093
		private RemoveElementFromList<int> NodeCanvas_Tasks_Actions_RemoveElementFromList_System_Int32;

		// Token: 0x040046AE RID: 18094
		private RemoveElementFromList<Vector2> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Vector2;

		// Token: 0x040046AF RID: 18095
		private RemoveElementFromList<Vector3> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Vector3;

		// Token: 0x040046B0 RID: 18096
		private RemoveElementFromList<Vector4> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Vector4;

		// Token: 0x040046B1 RID: 18097
		private RemoveElementFromList<Quaternion> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Quaternion;

		// Token: 0x040046B2 RID: 18098
		private RemoveElementFromList<Keyframe> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Keyframe;

		// Token: 0x040046B3 RID: 18099
		private RemoveElementFromList<Bounds> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Bounds;

		// Token: 0x040046B4 RID: 18100
		private RemoveElementFromList<Color> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Color;

		// Token: 0x040046B5 RID: 18101
		private RemoveElementFromList<Rect> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Rect;

		// Token: 0x040046B6 RID: 18102
		private RemoveElementFromList<ContactPoint> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_ContactPoint;

		// Token: 0x040046B7 RID: 18103
		private RemoveElementFromList<ContactPoint2D> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_ContactPoint2D;

		// Token: 0x040046B8 RID: 18104
		private RemoveElementFromList<RaycastHit> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_RaycastHit;

		// Token: 0x040046B9 RID: 18105
		private RemoveElementFromList<RaycastHit2D> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_RaycastHit2D;

		// Token: 0x040046BA RID: 18106
		private RemoveElementFromList<Ray> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Ray;

		// Token: 0x040046BB RID: 18107
		private RemoveElementFromList<Space> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_Space;

		// Token: 0x040046BC RID: 18108
		private RemoveElementFromList<LayerMask> NodeCanvas_Tasks_Actions_RemoveElementFromList_UnityEngine_LayerMask;

		// Token: 0x040046BD RID: 18109
		private NodeCanvas.Tasks.Actions.SendEvent<bool> NodeCanvas_Tasks_Actions_SendEvent_System_Boolean;

		// Token: 0x040046BE RID: 18110
		private NodeCanvas.Tasks.Actions.SendEvent<float> NodeCanvas_Tasks_Actions_SendEvent_System_Single;

		// Token: 0x040046BF RID: 18111
		private NodeCanvas.Tasks.Actions.SendEvent<int> NodeCanvas_Tasks_Actions_SendEvent_System_Int32;

		// Token: 0x040046C0 RID: 18112
		private NodeCanvas.Tasks.Actions.SendEvent<Vector2> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Vector2;

		// Token: 0x040046C1 RID: 18113
		private NodeCanvas.Tasks.Actions.SendEvent<Vector3> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Vector3;

		// Token: 0x040046C2 RID: 18114
		private NodeCanvas.Tasks.Actions.SendEvent<Vector4> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Vector4;

		// Token: 0x040046C3 RID: 18115
		private NodeCanvas.Tasks.Actions.SendEvent<Quaternion> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Quaternion;

		// Token: 0x040046C4 RID: 18116
		private NodeCanvas.Tasks.Actions.SendEvent<Keyframe> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Keyframe;

		// Token: 0x040046C5 RID: 18117
		private NodeCanvas.Tasks.Actions.SendEvent<Bounds> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Bounds;

		// Token: 0x040046C6 RID: 18118
		private NodeCanvas.Tasks.Actions.SendEvent<Color> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Color;

		// Token: 0x040046C7 RID: 18119
		private NodeCanvas.Tasks.Actions.SendEvent<Rect> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Rect;

		// Token: 0x040046C8 RID: 18120
		private NodeCanvas.Tasks.Actions.SendEvent<ContactPoint> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_ContactPoint;

		// Token: 0x040046C9 RID: 18121
		private NodeCanvas.Tasks.Actions.SendEvent<ContactPoint2D> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_ContactPoint2D;

		// Token: 0x040046CA RID: 18122
		private NodeCanvas.Tasks.Actions.SendEvent<RaycastHit> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_RaycastHit;

		// Token: 0x040046CB RID: 18123
		private NodeCanvas.Tasks.Actions.SendEvent<RaycastHit2D> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_RaycastHit2D;

		// Token: 0x040046CC RID: 18124
		private NodeCanvas.Tasks.Actions.SendEvent<Ray> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Ray;

		// Token: 0x040046CD RID: 18125
		private NodeCanvas.Tasks.Actions.SendEvent<Space> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_Space;

		// Token: 0x040046CE RID: 18126
		private NodeCanvas.Tasks.Actions.SendEvent<LayerMask> NodeCanvas_Tasks_Actions_SendEvent_UnityEngine_LayerMask;

		// Token: 0x040046CF RID: 18127
		private SendEventToObjects<bool> NodeCanvas_Tasks_Actions_SendEventToObjects_System_Boolean;

		// Token: 0x040046D0 RID: 18128
		private SendEventToObjects<float> NodeCanvas_Tasks_Actions_SendEventToObjects_System_Single;

		// Token: 0x040046D1 RID: 18129
		private SendEventToObjects<int> NodeCanvas_Tasks_Actions_SendEventToObjects_System_Int32;

		// Token: 0x040046D2 RID: 18130
		private SendEventToObjects<Vector2> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Vector2;

		// Token: 0x040046D3 RID: 18131
		private SendEventToObjects<Vector3> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Vector3;

		// Token: 0x040046D4 RID: 18132
		private SendEventToObjects<Vector4> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Vector4;

		// Token: 0x040046D5 RID: 18133
		private SendEventToObjects<Quaternion> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Quaternion;

		// Token: 0x040046D6 RID: 18134
		private SendEventToObjects<Keyframe> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Keyframe;

		// Token: 0x040046D7 RID: 18135
		private SendEventToObjects<Bounds> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Bounds;

		// Token: 0x040046D8 RID: 18136
		private SendEventToObjects<Color> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Color;

		// Token: 0x040046D9 RID: 18137
		private SendEventToObjects<Rect> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Rect;

		// Token: 0x040046DA RID: 18138
		private SendEventToObjects<ContactPoint> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_ContactPoint;

		// Token: 0x040046DB RID: 18139
		private SendEventToObjects<ContactPoint2D> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_ContactPoint2D;

		// Token: 0x040046DC RID: 18140
		private SendEventToObjects<RaycastHit> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_RaycastHit;

		// Token: 0x040046DD RID: 18141
		private SendEventToObjects<RaycastHit2D> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_RaycastHit2D;

		// Token: 0x040046DE RID: 18142
		private SendEventToObjects<Ray> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Ray;

		// Token: 0x040046DF RID: 18143
		private SendEventToObjects<Space> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_Space;

		// Token: 0x040046E0 RID: 18144
		private SendEventToObjects<LayerMask> NodeCanvas_Tasks_Actions_SendEventToObjects_UnityEngine_LayerMask;

		// Token: 0x040046E1 RID: 18145
		private SendMessage<bool> NodeCanvas_Tasks_Actions_SendMessage_System_Boolean;

		// Token: 0x040046E2 RID: 18146
		private SendMessage<float> NodeCanvas_Tasks_Actions_SendMessage_System_Single;

		// Token: 0x040046E3 RID: 18147
		private SendMessage<int> NodeCanvas_Tasks_Actions_SendMessage_System_Int32;

		// Token: 0x040046E4 RID: 18148
		private SendMessage<Vector2> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Vector2;

		// Token: 0x040046E5 RID: 18149
		private SendMessage<Vector3> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Vector3;

		// Token: 0x040046E6 RID: 18150
		private SendMessage<Vector4> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Vector4;

		// Token: 0x040046E7 RID: 18151
		private SendMessage<Quaternion> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Quaternion;

		// Token: 0x040046E8 RID: 18152
		private SendMessage<Keyframe> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Keyframe;

		// Token: 0x040046E9 RID: 18153
		private SendMessage<Bounds> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Bounds;

		// Token: 0x040046EA RID: 18154
		private SendMessage<Color> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Color;

		// Token: 0x040046EB RID: 18155
		private SendMessage<Rect> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Rect;

		// Token: 0x040046EC RID: 18156
		private SendMessage<ContactPoint> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_ContactPoint;

		// Token: 0x040046ED RID: 18157
		private SendMessage<ContactPoint2D> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_ContactPoint2D;

		// Token: 0x040046EE RID: 18158
		private SendMessage<RaycastHit> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_RaycastHit;

		// Token: 0x040046EF RID: 18159
		private SendMessage<RaycastHit2D> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_RaycastHit2D;

		// Token: 0x040046F0 RID: 18160
		private SendMessage<Ray> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Ray;

		// Token: 0x040046F1 RID: 18161
		private SendMessage<Space> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_Space;

		// Token: 0x040046F2 RID: 18162
		private SendMessage<LayerMask> NodeCanvas_Tasks_Actions_SendMessage_UnityEngine_LayerMask;

		// Token: 0x040046F3 RID: 18163
		private SetListElement<bool> NodeCanvas_Tasks_Actions_SetListElement_System_Boolean;

		// Token: 0x040046F4 RID: 18164
		private SetListElement<float> NodeCanvas_Tasks_Actions_SetListElement_System_Single;

		// Token: 0x040046F5 RID: 18165
		private SetListElement<int> NodeCanvas_Tasks_Actions_SetListElement_System_Int32;

		// Token: 0x040046F6 RID: 18166
		private SetListElement<Vector2> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Vector2;

		// Token: 0x040046F7 RID: 18167
		private SetListElement<Vector3> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Vector3;

		// Token: 0x040046F8 RID: 18168
		private SetListElement<Vector4> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Vector4;

		// Token: 0x040046F9 RID: 18169
		private SetListElement<Quaternion> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Quaternion;

		// Token: 0x040046FA RID: 18170
		private SetListElement<Keyframe> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Keyframe;

		// Token: 0x040046FB RID: 18171
		private SetListElement<Bounds> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Bounds;

		// Token: 0x040046FC RID: 18172
		private SetListElement<Color> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Color;

		// Token: 0x040046FD RID: 18173
		private SetListElement<Rect> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Rect;

		// Token: 0x040046FE RID: 18174
		private SetListElement<ContactPoint> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_ContactPoint;

		// Token: 0x040046FF RID: 18175
		private SetListElement<ContactPoint2D> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_ContactPoint2D;

		// Token: 0x04004700 RID: 18176
		private SetListElement<RaycastHit> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_RaycastHit;

		// Token: 0x04004701 RID: 18177
		private SetListElement<RaycastHit2D> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_RaycastHit2D;

		// Token: 0x04004702 RID: 18178
		private SetListElement<Ray> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Ray;

		// Token: 0x04004703 RID: 18179
		private SetListElement<Space> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_Space;

		// Token: 0x04004704 RID: 18180
		private SetListElement<LayerMask> NodeCanvas_Tasks_Actions_SetListElement_UnityEngine_LayerMask;

		// Token: 0x04004705 RID: 18181
		private NodeCanvas.Tasks.Actions.SetVariable<bool> NodeCanvas_Tasks_Actions_SetVariable_System_Boolean;

		// Token: 0x04004706 RID: 18182
		private NodeCanvas.Tasks.Actions.SetVariable<float> NodeCanvas_Tasks_Actions_SetVariable_System_Single;

		// Token: 0x04004707 RID: 18183
		private NodeCanvas.Tasks.Actions.SetVariable<int> NodeCanvas_Tasks_Actions_SetVariable_System_Int32;

		// Token: 0x04004708 RID: 18184
		private NodeCanvas.Tasks.Actions.SetVariable<Vector2> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Vector2;

		// Token: 0x04004709 RID: 18185
		private NodeCanvas.Tasks.Actions.SetVariable<Vector3> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Vector3;

		// Token: 0x0400470A RID: 18186
		private NodeCanvas.Tasks.Actions.SetVariable<Vector4> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Vector4;

		// Token: 0x0400470B RID: 18187
		private NodeCanvas.Tasks.Actions.SetVariable<Quaternion> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Quaternion;

		// Token: 0x0400470C RID: 18188
		private NodeCanvas.Tasks.Actions.SetVariable<Keyframe> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Keyframe;

		// Token: 0x0400470D RID: 18189
		private NodeCanvas.Tasks.Actions.SetVariable<Bounds> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Bounds;

		// Token: 0x0400470E RID: 18190
		private NodeCanvas.Tasks.Actions.SetVariable<Color> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Color;

		// Token: 0x0400470F RID: 18191
		private NodeCanvas.Tasks.Actions.SetVariable<Rect> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Rect;

		// Token: 0x04004710 RID: 18192
		private NodeCanvas.Tasks.Actions.SetVariable<ContactPoint> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_ContactPoint;

		// Token: 0x04004711 RID: 18193
		private NodeCanvas.Tasks.Actions.SetVariable<ContactPoint2D> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_ContactPoint2D;

		// Token: 0x04004712 RID: 18194
		private NodeCanvas.Tasks.Actions.SetVariable<RaycastHit> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_RaycastHit;

		// Token: 0x04004713 RID: 18195
		private NodeCanvas.Tasks.Actions.SetVariable<RaycastHit2D> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_RaycastHit2D;

		// Token: 0x04004714 RID: 18196
		private NodeCanvas.Tasks.Actions.SetVariable<Ray> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Ray;

		// Token: 0x04004715 RID: 18197
		private NodeCanvas.Tasks.Actions.SetVariable<Space> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_Space;

		// Token: 0x04004716 RID: 18198
		private NodeCanvas.Tasks.Actions.SetVariable<LayerMask> NodeCanvas_Tasks_Actions_SetVariable_UnityEngine_LayerMask;

		// Token: 0x04004717 RID: 18199
		private CheckCSharpEvent<bool> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_System_Boolean;

		// Token: 0x04004718 RID: 18200
		private CheckCSharpEvent<float> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_System_Single;

		// Token: 0x04004719 RID: 18201
		private CheckCSharpEvent<int> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_System_Int32;

		// Token: 0x0400471A RID: 18202
		private CheckCSharpEvent<Vector2> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Vector2;

		// Token: 0x0400471B RID: 18203
		private CheckCSharpEvent<Vector3> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Vector3;

		// Token: 0x0400471C RID: 18204
		private CheckCSharpEvent<Vector4> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Vector4;

		// Token: 0x0400471D RID: 18205
		private CheckCSharpEvent<Quaternion> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Quaternion;

		// Token: 0x0400471E RID: 18206
		private CheckCSharpEvent<Keyframe> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Keyframe;

		// Token: 0x0400471F RID: 18207
		private CheckCSharpEvent<Bounds> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Bounds;

		// Token: 0x04004720 RID: 18208
		private CheckCSharpEvent<Color> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Color;

		// Token: 0x04004721 RID: 18209
		private CheckCSharpEvent<Rect> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Rect;

		// Token: 0x04004722 RID: 18210
		private CheckCSharpEvent<ContactPoint> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_ContactPoint;

		// Token: 0x04004723 RID: 18211
		private CheckCSharpEvent<ContactPoint2D> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_ContactPoint2D;

		// Token: 0x04004724 RID: 18212
		private CheckCSharpEvent<RaycastHit> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_RaycastHit;

		// Token: 0x04004725 RID: 18213
		private CheckCSharpEvent<RaycastHit2D> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_RaycastHit2D;

		// Token: 0x04004726 RID: 18214
		private CheckCSharpEvent<Ray> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Ray;

		// Token: 0x04004727 RID: 18215
		private CheckCSharpEvent<Space> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_Space;

		// Token: 0x04004728 RID: 18216
		private CheckCSharpEvent<LayerMask> NodeCanvas_Tasks_Conditions_CheckCSharpEvent_UnityEngine_LayerMask;

		// Token: 0x04004729 RID: 18217
		private CheckCSharpEventValue<bool> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_System_Boolean;

		// Token: 0x0400472A RID: 18218
		private CheckCSharpEventValue<float> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_System_Single;

		// Token: 0x0400472B RID: 18219
		private CheckCSharpEventValue<int> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_System_Int32;

		// Token: 0x0400472C RID: 18220
		private CheckCSharpEventValue<Vector2> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Vector2;

		// Token: 0x0400472D RID: 18221
		private CheckCSharpEventValue<Vector3> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Vector3;

		// Token: 0x0400472E RID: 18222
		private CheckCSharpEventValue<Vector4> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Vector4;

		// Token: 0x0400472F RID: 18223
		private CheckCSharpEventValue<Quaternion> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Quaternion;

		// Token: 0x04004730 RID: 18224
		private CheckCSharpEventValue<Keyframe> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Keyframe;

		// Token: 0x04004731 RID: 18225
		private CheckCSharpEventValue<Bounds> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Bounds;

		// Token: 0x04004732 RID: 18226
		private CheckCSharpEventValue<Color> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Color;

		// Token: 0x04004733 RID: 18227
		private CheckCSharpEventValue<Rect> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Rect;

		// Token: 0x04004734 RID: 18228
		private CheckCSharpEventValue<ContactPoint> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_ContactPoint;

		// Token: 0x04004735 RID: 18229
		private CheckCSharpEventValue<ContactPoint2D> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_ContactPoint2D;

		// Token: 0x04004736 RID: 18230
		private CheckCSharpEventValue<RaycastHit> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_RaycastHit;

		// Token: 0x04004737 RID: 18231
		private CheckCSharpEventValue<RaycastHit2D> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_RaycastHit2D;

		// Token: 0x04004738 RID: 18232
		private CheckCSharpEventValue<Ray> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Ray;

		// Token: 0x04004739 RID: 18233
		private CheckCSharpEventValue<Space> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_Space;

		// Token: 0x0400473A RID: 18234
		private CheckCSharpEventValue<LayerMask> NodeCanvas_Tasks_Conditions_CheckCSharpEventValue_UnityEngine_LayerMask;

		// Token: 0x0400473B RID: 18235
		private CheckEvent<bool> NodeCanvas_Tasks_Conditions_CheckEvent_System_Boolean;

		// Token: 0x0400473C RID: 18236
		private CheckEvent<float> NodeCanvas_Tasks_Conditions_CheckEvent_System_Single;

		// Token: 0x0400473D RID: 18237
		private CheckEvent<int> NodeCanvas_Tasks_Conditions_CheckEvent_System_Int32;

		// Token: 0x0400473E RID: 18238
		private CheckEvent<Vector2> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Vector2;

		// Token: 0x0400473F RID: 18239
		private CheckEvent<Vector3> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Vector3;

		// Token: 0x04004740 RID: 18240
		private CheckEvent<Vector4> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Vector4;

		// Token: 0x04004741 RID: 18241
		private CheckEvent<Quaternion> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Quaternion;

		// Token: 0x04004742 RID: 18242
		private CheckEvent<Keyframe> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Keyframe;

		// Token: 0x04004743 RID: 18243
		private CheckEvent<Bounds> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Bounds;

		// Token: 0x04004744 RID: 18244
		private CheckEvent<Color> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Color;

		// Token: 0x04004745 RID: 18245
		private CheckEvent<Rect> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Rect;

		// Token: 0x04004746 RID: 18246
		private CheckEvent<ContactPoint> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_ContactPoint;

		// Token: 0x04004747 RID: 18247
		private CheckEvent<ContactPoint2D> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_ContactPoint2D;

		// Token: 0x04004748 RID: 18248
		private CheckEvent<RaycastHit> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_RaycastHit;

		// Token: 0x04004749 RID: 18249
		private CheckEvent<RaycastHit2D> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_RaycastHit2D;

		// Token: 0x0400474A RID: 18250
		private CheckEvent<Ray> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Ray;

		// Token: 0x0400474B RID: 18251
		private CheckEvent<Space> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_Space;

		// Token: 0x0400474C RID: 18252
		private CheckEvent<LayerMask> NodeCanvas_Tasks_Conditions_CheckEvent_UnityEngine_LayerMask;

		// Token: 0x0400474D RID: 18253
		private CheckEventValue<bool> NodeCanvas_Tasks_Conditions_CheckEventValue_System_Boolean;

		// Token: 0x0400474E RID: 18254
		private CheckEventValue<float> NodeCanvas_Tasks_Conditions_CheckEventValue_System_Single;

		// Token: 0x0400474F RID: 18255
		private CheckEventValue<int> NodeCanvas_Tasks_Conditions_CheckEventValue_System_Int32;

		// Token: 0x04004750 RID: 18256
		private CheckEventValue<Vector2> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Vector2;

		// Token: 0x04004751 RID: 18257
		private CheckEventValue<Vector3> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Vector3;

		// Token: 0x04004752 RID: 18258
		private CheckEventValue<Vector4> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Vector4;

		// Token: 0x04004753 RID: 18259
		private CheckEventValue<Quaternion> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Quaternion;

		// Token: 0x04004754 RID: 18260
		private CheckEventValue<Keyframe> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Keyframe;

		// Token: 0x04004755 RID: 18261
		private CheckEventValue<Bounds> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Bounds;

		// Token: 0x04004756 RID: 18262
		private CheckEventValue<Color> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Color;

		// Token: 0x04004757 RID: 18263
		private CheckEventValue<Rect> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Rect;

		// Token: 0x04004758 RID: 18264
		private CheckEventValue<ContactPoint> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_ContactPoint;

		// Token: 0x04004759 RID: 18265
		private CheckEventValue<ContactPoint2D> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_ContactPoint2D;

		// Token: 0x0400475A RID: 18266
		private CheckEventValue<RaycastHit> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_RaycastHit;

		// Token: 0x0400475B RID: 18267
		private CheckEventValue<RaycastHit2D> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_RaycastHit2D;

		// Token: 0x0400475C RID: 18268
		private CheckEventValue<Ray> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Ray;

		// Token: 0x0400475D RID: 18269
		private CheckEventValue<Space> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_Space;

		// Token: 0x0400475E RID: 18270
		private CheckEventValue<LayerMask> NodeCanvas_Tasks_Conditions_CheckEventValue_UnityEngine_LayerMask;

		// Token: 0x0400475F RID: 18271
		private CheckUnityEvent<bool> NodeCanvas_Tasks_Conditions_CheckUnityEvent_System_Boolean;

		// Token: 0x04004760 RID: 18272
		private CheckUnityEvent<float> NodeCanvas_Tasks_Conditions_CheckUnityEvent_System_Single;

		// Token: 0x04004761 RID: 18273
		private CheckUnityEvent<int> NodeCanvas_Tasks_Conditions_CheckUnityEvent_System_Int32;

		// Token: 0x04004762 RID: 18274
		private CheckUnityEvent<Vector2> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Vector2;

		// Token: 0x04004763 RID: 18275
		private CheckUnityEvent<Vector3> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Vector3;

		// Token: 0x04004764 RID: 18276
		private CheckUnityEvent<Vector4> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Vector4;

		// Token: 0x04004765 RID: 18277
		private CheckUnityEvent<Quaternion> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Quaternion;

		// Token: 0x04004766 RID: 18278
		private CheckUnityEvent<Keyframe> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Keyframe;

		// Token: 0x04004767 RID: 18279
		private CheckUnityEvent<Bounds> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Bounds;

		// Token: 0x04004768 RID: 18280
		private CheckUnityEvent<Color> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Color;

		// Token: 0x04004769 RID: 18281
		private CheckUnityEvent<Rect> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Rect;

		// Token: 0x0400476A RID: 18282
		private CheckUnityEvent<ContactPoint> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_ContactPoint;

		// Token: 0x0400476B RID: 18283
		private CheckUnityEvent<ContactPoint2D> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_ContactPoint2D;

		// Token: 0x0400476C RID: 18284
		private CheckUnityEvent<RaycastHit> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_RaycastHit;

		// Token: 0x0400476D RID: 18285
		private CheckUnityEvent<RaycastHit2D> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_RaycastHit2D;

		// Token: 0x0400476E RID: 18286
		private CheckUnityEvent<Ray> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Ray;

		// Token: 0x0400476F RID: 18287
		private CheckUnityEvent<Space> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_Space;

		// Token: 0x04004770 RID: 18288
		private CheckUnityEvent<LayerMask> NodeCanvas_Tasks_Conditions_CheckUnityEvent_UnityEngine_LayerMask;

		// Token: 0x04004771 RID: 18289
		private CheckUnityEventValue<bool> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_System_Boolean;

		// Token: 0x04004772 RID: 18290
		private CheckUnityEventValue<float> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_System_Single;

		// Token: 0x04004773 RID: 18291
		private CheckUnityEventValue<int> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_System_Int32;

		// Token: 0x04004774 RID: 18292
		private CheckUnityEventValue<Vector2> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Vector2;

		// Token: 0x04004775 RID: 18293
		private CheckUnityEventValue<Vector3> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Vector3;

		// Token: 0x04004776 RID: 18294
		private CheckUnityEventValue<Vector4> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Vector4;

		// Token: 0x04004777 RID: 18295
		private CheckUnityEventValue<Quaternion> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Quaternion;

		// Token: 0x04004778 RID: 18296
		private CheckUnityEventValue<Keyframe> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Keyframe;

		// Token: 0x04004779 RID: 18297
		private CheckUnityEventValue<Bounds> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Bounds;

		// Token: 0x0400477A RID: 18298
		private CheckUnityEventValue<Color> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Color;

		// Token: 0x0400477B RID: 18299
		private CheckUnityEventValue<Rect> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Rect;

		// Token: 0x0400477C RID: 18300
		private CheckUnityEventValue<ContactPoint> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_ContactPoint;

		// Token: 0x0400477D RID: 18301
		private CheckUnityEventValue<ContactPoint2D> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_ContactPoint2D;

		// Token: 0x0400477E RID: 18302
		private CheckUnityEventValue<RaycastHit> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_RaycastHit;

		// Token: 0x0400477F RID: 18303
		private CheckUnityEventValue<RaycastHit2D> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_RaycastHit2D;

		// Token: 0x04004780 RID: 18304
		private CheckUnityEventValue<Ray> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Ray;

		// Token: 0x04004781 RID: 18305
		private CheckUnityEventValue<Space> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_Space;

		// Token: 0x04004782 RID: 18306
		private CheckUnityEventValue<LayerMask> NodeCanvas_Tasks_Conditions_CheckUnityEventValue_UnityEngine_LayerMask;

		// Token: 0x04004783 RID: 18307
		private CheckVariable<bool> NodeCanvas_Tasks_Conditions_CheckVariable_System_Boolean;

		// Token: 0x04004784 RID: 18308
		private CheckVariable<float> NodeCanvas_Tasks_Conditions_CheckVariable_System_Single;

		// Token: 0x04004785 RID: 18309
		private CheckVariable<int> NodeCanvas_Tasks_Conditions_CheckVariable_System_Int32;

		// Token: 0x04004786 RID: 18310
		private CheckVariable<Vector2> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Vector2;

		// Token: 0x04004787 RID: 18311
		private CheckVariable<Vector3> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Vector3;

		// Token: 0x04004788 RID: 18312
		private CheckVariable<Vector4> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Vector4;

		// Token: 0x04004789 RID: 18313
		private CheckVariable<Quaternion> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Quaternion;

		// Token: 0x0400478A RID: 18314
		private CheckVariable<Keyframe> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Keyframe;

		// Token: 0x0400478B RID: 18315
		private CheckVariable<Bounds> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Bounds;

		// Token: 0x0400478C RID: 18316
		private CheckVariable<Color> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Color;

		// Token: 0x0400478D RID: 18317
		private CheckVariable<Rect> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Rect;

		// Token: 0x0400478E RID: 18318
		private CheckVariable<ContactPoint> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_ContactPoint;

		// Token: 0x0400478F RID: 18319
		private CheckVariable<ContactPoint2D> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_ContactPoint2D;

		// Token: 0x04004790 RID: 18320
		private CheckVariable<RaycastHit> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_RaycastHit;

		// Token: 0x04004791 RID: 18321
		private CheckVariable<RaycastHit2D> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_RaycastHit2D;

		// Token: 0x04004792 RID: 18322
		private CheckVariable<Ray> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Ray;

		// Token: 0x04004793 RID: 18323
		private CheckVariable<Space> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_Space;

		// Token: 0x04004794 RID: 18324
		private CheckVariable<LayerMask> NodeCanvas_Tasks_Conditions_CheckVariable_UnityEngine_LayerMask;

		// Token: 0x04004795 RID: 18325
		private ListContainsElement<bool> NodeCanvas_Tasks_Conditions_ListContainsElement_System_Boolean;

		// Token: 0x04004796 RID: 18326
		private ListContainsElement<float> NodeCanvas_Tasks_Conditions_ListContainsElement_System_Single;

		// Token: 0x04004797 RID: 18327
		private ListContainsElement<int> NodeCanvas_Tasks_Conditions_ListContainsElement_System_Int32;

		// Token: 0x04004798 RID: 18328
		private ListContainsElement<Vector2> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Vector2;

		// Token: 0x04004799 RID: 18329
		private ListContainsElement<Vector3> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Vector3;

		// Token: 0x0400479A RID: 18330
		private ListContainsElement<Vector4> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Vector4;

		// Token: 0x0400479B RID: 18331
		private ListContainsElement<Quaternion> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Quaternion;

		// Token: 0x0400479C RID: 18332
		private ListContainsElement<Keyframe> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Keyframe;

		// Token: 0x0400479D RID: 18333
		private ListContainsElement<Bounds> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Bounds;

		// Token: 0x0400479E RID: 18334
		private ListContainsElement<Color> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Color;

		// Token: 0x0400479F RID: 18335
		private ListContainsElement<Rect> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Rect;

		// Token: 0x040047A0 RID: 18336
		private ListContainsElement<ContactPoint> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_ContactPoint;

		// Token: 0x040047A1 RID: 18337
		private ListContainsElement<ContactPoint2D> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_ContactPoint2D;

		// Token: 0x040047A2 RID: 18338
		private ListContainsElement<RaycastHit> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_RaycastHit;

		// Token: 0x040047A3 RID: 18339
		private ListContainsElement<RaycastHit2D> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_RaycastHit2D;

		// Token: 0x040047A4 RID: 18340
		private ListContainsElement<Ray> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Ray;

		// Token: 0x040047A5 RID: 18341
		private ListContainsElement<Space> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_Space;

		// Token: 0x040047A6 RID: 18342
		private ListContainsElement<LayerMask> NodeCanvas_Tasks_Conditions_ListContainsElement_UnityEngine_LayerMask;

		// Token: 0x040047A7 RID: 18343
		private NodeCanvas.Tasks.Conditions.TryGetValue<bool> NodeCanvas_Tasks_Conditions_TryGetValue_System_Boolean;

		// Token: 0x040047A8 RID: 18344
		private NodeCanvas.Tasks.Conditions.TryGetValue<float> NodeCanvas_Tasks_Conditions_TryGetValue_System_Single;

		// Token: 0x040047A9 RID: 18345
		private NodeCanvas.Tasks.Conditions.TryGetValue<int> NodeCanvas_Tasks_Conditions_TryGetValue_System_Int32;

		// Token: 0x040047AA RID: 18346
		private NodeCanvas.Tasks.Conditions.TryGetValue<Vector2> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Vector2;

		// Token: 0x040047AB RID: 18347
		private NodeCanvas.Tasks.Conditions.TryGetValue<Vector3> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Vector3;

		// Token: 0x040047AC RID: 18348
		private NodeCanvas.Tasks.Conditions.TryGetValue<Vector4> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Vector4;

		// Token: 0x040047AD RID: 18349
		private NodeCanvas.Tasks.Conditions.TryGetValue<Quaternion> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Quaternion;

		// Token: 0x040047AE RID: 18350
		private NodeCanvas.Tasks.Conditions.TryGetValue<Keyframe> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Keyframe;

		// Token: 0x040047AF RID: 18351
		private NodeCanvas.Tasks.Conditions.TryGetValue<Bounds> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Bounds;

		// Token: 0x040047B0 RID: 18352
		private NodeCanvas.Tasks.Conditions.TryGetValue<Color> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Color;

		// Token: 0x040047B1 RID: 18353
		private NodeCanvas.Tasks.Conditions.TryGetValue<Rect> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Rect;

		// Token: 0x040047B2 RID: 18354
		private NodeCanvas.Tasks.Conditions.TryGetValue<ContactPoint> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_ContactPoint;

		// Token: 0x040047B3 RID: 18355
		private NodeCanvas.Tasks.Conditions.TryGetValue<ContactPoint2D> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_ContactPoint2D;

		// Token: 0x040047B4 RID: 18356
		private NodeCanvas.Tasks.Conditions.TryGetValue<RaycastHit> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_RaycastHit;

		// Token: 0x040047B5 RID: 18357
		private NodeCanvas.Tasks.Conditions.TryGetValue<RaycastHit2D> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_RaycastHit2D;

		// Token: 0x040047B6 RID: 18358
		private NodeCanvas.Tasks.Conditions.TryGetValue<Ray> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Ray;

		// Token: 0x040047B7 RID: 18359
		private NodeCanvas.Tasks.Conditions.TryGetValue<Space> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_Space;

		// Token: 0x040047B8 RID: 18360
		private NodeCanvas.Tasks.Conditions.TryGetValue<LayerMask> NodeCanvas_Tasks_Conditions_TryGetValue_UnityEngine_LayerMask;
	}
}
