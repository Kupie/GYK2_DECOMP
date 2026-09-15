using System;
using System.Reflection;
using LazyBearTechnology;
using Sirenix.Serialization;

// Token: 0x02000AEC RID: 2796
public class LazySerializationPolicy : CustomSerializationPolicy
{
	// Token: 0x06004ACC RID: 19148 RVA: 0x00160F06 File Offset: 0x0015F106
	private LazySerializationPolicy(string id, bool allowNonSerializableTypes, Func<MemberInfo, bool> shouldSerializeFunc)
		: base(id, allowNonSerializableTypes, shouldSerializeFunc)
	{
	}

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x06004ACD RID: 19149 RVA: 0x00160F11 File Offset: 0x0015F111
	public static LazySerializationPolicy Instance
	{
		get
		{
			return LazySerializationPolicy.instance;
		}
	}

	// Token: 0x06004ACE RID: 19150 RVA: 0x00160F18 File Offset: 0x0015F118
	public void TestSerialization()
	{
		SerializationContext serializationContext = new SerializationContext();
		DeserializationContext deserializationContext = new DeserializationContext();
		serializationContext.Config.SerializationPolicy = LazySerializationPolicy.Instance;
		serializationContext.IndexReferenceResolver = new UnityReferenceResolver();
		deserializationContext.Config.SerializationPolicy = LazySerializationPolicy.Instance;
		deserializationContext.IndexReferenceResolver = new UnityReferenceResolver();
		SerializationUtility.SerializeValue<MainGame>(new MainGame(), DataFormat.Binary, serializationContext);
	}

	// Token: 0x04003C6A RID: 15466
	private static readonly LazySerializationPolicy instance = new LazySerializationPolicy("LazySerializationPolicy", true, delegate(MemberInfo member)
	{
		FieldInfo fieldInfo = member as FieldInfo;
		return (fieldInfo != null && Attribute.IsDefined(fieldInfo, typeof(LazySerialize))) || SerializationPolicies.Unity.ShouldSerializeMember(member);
	});
}
