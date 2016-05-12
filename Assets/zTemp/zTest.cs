using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal;
using UnityEngine.SceneManagement;
using Pseudo.Injection;
using Pseudo.EntityFramework;
using Pseudo.Communication;
using UnityEngine.Scripting;
using Pseudo.Injection.Internal;
using UnityEngine.Assertions;
using System.Reflection;
using Pseudo.Reflection;
using Pseudo.Reflection.Internal;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Pseudo.Pooling2;
using System.ComponentModel;

public class zTest : PMonoBehaviour
{
	/*
	 * Initialization problem:
	 * null -> null = nothing
	 * class -> null = direct assign
	 * class -> class = recursive assign if same type else ???
	 * null -> class = ???
	 * 
	 * struct -> default = direct assign
	 * struct -> struct = direct assign if pure else recursive assign
	 * default -> struct = direct assign if pure else ???
	 * 
	* IInitializer (initialize instances to their reference state
	* ITypeAnalyzer (analyzes a type and produces a corresponding ITypeInfo)
	* ITypeInfo (holds the info and accessors required for initialization)
	* IUpdater *find better name* (updates the IInitializer and feeds it the instances to be initialized)
	* IPool / IPool<T>
	* IPoolManager *find better name* (centralizes the access to the pools)
	*/
	// Solve the Additional arguments implementation
	// Test build with assembly references
	// Check enum flags for correct implementation of Contains

	public PType Type;
	public PAssembly[] Assemblies;
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	public int Iterations = 100000;

	[Inject]
	IEntityManager entityManager = null;
	[Multiline(15)]
	public string Data;

	[Button]
	public bool test;
	void Test()
	{
		PDebug.Log(typeof(List<>).Is(typeof(List<>)));
		PDebug.Log(typeof(List<>).Is(typeof(IList<>)));
		PDebug.Log(typeof(IList<>).Is(typeof(IList<>)));
		//var instance = A.Pool.Create();
		//PDebug.Log(instance.Value);
		//A.Pool.Recycle(instance);
	}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}

	void OnGUI()
	{
		GUILayout.TextArea(Convert.ToString(Type.Type), GUILayout.Width(Screen.width));
		GUILayout.TextArea(PDebug.ToString(Assemblies), GUILayout.Width(Screen.width));
	}

	void OnCreate() { }
	void OnRecycle() { }
}

[MessageEnum]
public enum Messages : byte
{
	Zero,
	One,
	Two,
	Three,
}

public class A
{
	public static readonly IPool<A> Pool = new Pool<A>(() => new A());

	public int Value
	{
		get { return value; }
		set { this.value = value; }
	}
	int value;

	class Initializer : Initializer<A>
	{
		public override void OnCreate(A instance)
		{
			instance.value = PRandom.Range(0, 1000);
		}

		public override void OnRecycle(A instance) { }
	}
}

//public interface IFlags<T>
//{
//	T Add(byte flag);
//	T Add(T flags);
//	T Remove(byte flag);
//	T Remove(T flags);
//	T And(T flags);
//	T Or(T flags);
//	T Xor(T flags);
//	T Not();
//	bool Has(byte flag);
//	bool HasAll(T flags);
//	bool HasAny(T flags);
//	bool HasNone(T flags);
//}

//public static class PEnumUtility
//{
//	public static IEnumerable<T> GetValues<T>() where T : IEnum
//	{
//		return GetValueFields<T>()
//			.Select(f => (T)f.GetValue(null));
//	}

//	public static IEnumerable<string> GetValueNames<T>() where T : IEnum
//	{
//		return GetValues<T>()
//			.Select(v => v.Name);
//	}

//	public static IEnumerable<FieldInfo> GetValueFields<T>() where T : IEnum
//	{
//		return typeof(T)
//			.GetFields(BindingFlags.Public | BindingFlags.Static)
//			.Where(f => f.FieldType.Is<T>() && f.IsInitOnly);
//	}
//}

//public class SomeEnum2 : EnumBase<SomeEnum2, int>
//{
//	public static readonly ZEnum A = Create(1);
//}

//public abstract class EnumBase<TEnum, TValue>
//{
//	static readonly IEqualityComparer<ZEnum> comparer = new ZEnumComparer<TValue>();

//	protected static ZEnum Create(TValue value)
//	{
//		return new ZEnum(value, typeof(TEnum), comparer);
//	}
//}

//public class ZEnumComparer<TValue> : PEqualityComparer<ZEnum>
//{
//	static readonly IEqualityComparer<TValue> valueComparer = PEqualityComparer<TValue>.Default;

//	public override bool Equals(ZEnum x, ZEnum y)
//	{
//		return x.Type == y.Type && valueComparer.Equals((TValue)x.Value, (TValue)y.Value);
//	}

//	public override int GetHashCode(ZEnum obj)
//	{
//		return obj.Value.GetHashCode();
//	}
//}

//public struct ZEnumFlag : IFlag<ZEnumFlag>
//{
//	public ByteFlag Value
//	{
//		get { return value; }
//	}
//	public Type Type
//	{
//		get { return type; }
//	}

//	readonly ByteFlag value;
//	readonly Type type;
//	readonly IEqualityComparer<ZEnumFlag> comparer;

//	public ZEnumFlag(ByteFlag value, Type type, IEqualityComparer<ZEnumFlag> comparer)
//	{
//		this.value = value;
//		this.type = type;
//		this.comparer = comparer;
//	}

//	public bool Equals(ZEnumFlag other)
//	{
//		return comparer.Equals(this, other);
//	}

//	public override bool Equals(object obj)
//	{
//		if (obj is ZEnum)
//			return Equals((ZEnum)obj);

//		return false;
//	}

//	public override int GetHashCode()
//	{
//		return comparer.GetHashCode();
//	}

//	ZEnumFlag IFlag<ZEnumFlag>.Add(ZEnumFlag flags)
//	{
//		throw new NotImplementedException();
//	}

//	ZEnumFlag IFlag<ZEnumFlag>.Subtract(ZEnumFlag flags)
//	{
//		throw new NotImplementedException();
//	}

//	bool IFlag<ZEnumFlag>.HasAll(ZEnumFlag flags)
//	{
//		throw new NotImplementedException();
//	}

//	bool IFlag<ZEnumFlag>.HasAny(ZEnumFlag flags)
//	{
//		throw new NotImplementedException();
//	}

//	bool IFlag<ZEnumFlag>.HasNone(ZEnumFlag flags)
//	{
//		throw new NotImplementedException();
//	}

//	ZEnumFlag IFlag<ZEnumFlag>.Not()
//	{
//		throw new NotImplementedException();
//	}

//	ZEnumFlag IFlag<ZEnumFlag>.And(ZEnumFlag other)
//	{
//		throw new NotImplementedException();
//	}

//	ZEnumFlag IFlag<ZEnumFlag>.Or(ZEnumFlag other)
//	{
//		throw new NotImplementedException();
//	}

//	ZEnumFlag IFlag<ZEnumFlag>.Xor(ZEnumFlag other)
//	{
//		throw new NotImplementedException();
//	}

//	public static bool operator ==(ZEnumFlag a, ZEnumFlag b)
//	{
//		return a.comparer.Equals(a, b);
//	}

//	public static bool operator !=(ZEnumFlag a, ZEnumFlag b)
//	{
//		return !a.comparer.Equals(a, b);
//	}
//}

//public struct ZEnum : IEquatable<ZEnum>
//{
//	public object Value
//	{
//		get { return value; }
//	}
//	public Type Type
//	{
//		get { return type; }
//	}

//	readonly object value;
//	readonly Type type;
//	readonly IEqualityComparer<ZEnum> comparer;

//	public ZEnum(object value, Type type, IEqualityComparer<ZEnum> comparer)
//	{
//		this.value = value;
//		this.type = type;
//		this.comparer = comparer;
//	}

//	public bool Equals(ZEnum other)
//	{
//		return this == other;
//	}

//	public override bool Equals(object obj)
//	{
//		if (obj is ZEnum)
//			return Equals((ZEnum)obj);

//		return false;
//	}

//	public override int GetHashCode()
//	{
//		return comparer.GetHashCode();
//	}

//	public static bool operator ==(ZEnum a, ZEnum b)
//	{
//		return a.comparer.Equals(a, b);
//	}

//	public static bool operator !=(ZEnum a, ZEnum b)
//	{
//		return !a.comparer.Equals(a, b);
//	}
//}



//public partial struct SomeEnum
//{
//	public static readonly SomeEnum A = new SomeEnum(1, 2, 3);
//	public static readonly SomeEnum B = new SomeEnum(2, 6, 8);
//	public static readonly SomeEnum C = new SomeEnum(3, 9, 12);
//}

//public partial struct SomeEnum : IEnum<ByteFlag>, IFlag<ByteFlag, byte>, IEquatable<SomeEnum>
//{
//	public ByteFlag Value
//	{
//		get { return value; }
//	}
//	public string Name
//	{
//		get { return name; }
//	}

//	object IEnum.Value
//	{
//		get { return value; }
//	}
//	Type IEnum.Type
//	{
//		get { return typeof(int); }
//	}

//	ByteFlag value;
//	string name;

//	static SomeEnum()
//	{
//		foreach (var field in PEnumUtility.GetValueFields<SomeEnum>())
//		{
//			var value = (SomeEnum)field.GetValue(null);
//			value.name = field.Name;
//			field.SetValue(null, value);
//		}
//	}

//	SomeEnum(params byte[] values) : this(new ByteFlag(values)) { }

//	SomeEnum(ByteFlag value) : this()
//	{
//		this.value = value;
//	}

//	public bool Equals(SomeEnum other)
//	{
//		return this == other;
//	}

//	public bool Equals(ByteFlag other)
//	{
//		return this == other;
//	}

//	public override bool Equals(object obj)
//	{
//		if (obj is SomeEnum)
//			return Equals((SomeEnum)obj);
//		else if (obj is IEnum<ByteFlag>)
//			return Equals((IEnum<ByteFlag>)obj);
//		else if (obj is ByteFlag)
//			return Equals((ByteFlag)obj);

//		return false;
//	}

//	public override int GetHashCode()
//	{
//		return value.GetHashCode();
//	}

//	public override string ToString()
//	{
//		return string.Format("{0}({1}, {2})", GetType().Name, name, value);
//	}

//	public ByteFlag Add(byte flag)
//	{
//		return value + flag;
//	}

//	public ByteFlag Subtract(byte flag)
//	{
//		return value - flag;
//	}

//	public bool Has(byte flag)
//	{
//		return value[flag];
//	}

//	public ByteFlag Add(ByteFlag flags)
//	{
//		return value + flags;
//	}

//	public ByteFlag Subtract(ByteFlag flags)
//	{
//		return value - flags;
//	}

//	public bool HasAll(ByteFlag flags)
//	{
//		return ((IFlag<ByteFlag, byte>)value).HasAll(flags);
//	}

//	public bool HasAny(ByteFlag flags)
//	{
//		return ((IFlag<ByteFlag, byte>)value).HasAny(flags);
//	}

//	public bool HasNone(ByteFlag flags)
//	{
//		return ((IFlag<ByteFlag, byte>)value).HasNone(flags);
//	}

//	public ByteFlag Not()
//	{
//		return ((IFlag<ByteFlag, byte>)value).Not();
//	}

//	public ByteFlag And(ByteFlag other)
//	{
//		return ((IFlag<ByteFlag, byte>)value).And(other);
//	}

//	public ByteFlag Or(ByteFlag other)
//	{
//		return ((IFlag<ByteFlag, byte>)value).Or(other);
//	}

//	public ByteFlag Xor(ByteFlag other)
//	{
//		return ((IFlag<ByteFlag, byte>)value).Xor(other);
//	}

//	public static bool operator ==(SomeEnum a, SomeEnum b)
//	{
//		return a.value == b.value;
//	}

//	public static bool operator !=(SomeEnum a, SomeEnum b)
//	{
//		return a.value != b.value;
//	}

//	public static bool operator ==(SomeEnum a, ByteFlag b)
//	{
//		return a.value == b;
//	}

//	public static bool operator !=(SomeEnum a, ByteFlag b)
//	{
//		return a.value != b;
//	}

//	public static bool operator ==(ByteFlag a, SomeEnum b)
//	{
//		return a == b.value;
//	}

//	public static bool operator !=(ByteFlag a, SomeEnum b)
//	{
//		return a != b.value;
//	}

//	public static implicit operator ByteFlag(SomeEnum value)
//	{
//		return value.value;
//	}

//	public static implicit operator SomeEnum(ByteFlag value)
//	{
//		return new SomeEnum(value);
//	}
//}

//public interface IEnum<TValue> : IEnum, IEquatable<TValue>
//{
//	new TValue Value { get; }
//}

//public interface IEnum
//{
//	object Value { get; }
//	string Name { get; }
//	Type Type { get; }
//}