using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.EventSystems;
using System.Threading;
using System.Reflection;
using Pseudo.Internal.Audio;
using System.Runtime.Serialization;
using Pseudo.Internal;

public class zTest : PMonoBehaviour
{
	const int iterations = 1000;

	Dictionary<Type, object> dict = new Dictionary<Type, object>();

	[Button]
	public bool test;
	void Test()
	{
		PDebug.LogTest("Dict", () => GetValue(typeof(Dummy)), 1000000);
		PDebug.LogTest("Static", () => GetValue<Dummy>(), 1000000);
	}

	object GetValue(Type type)
	{
		object value;

		if (!dict.TryGetValue(type, out value))
		{
			value = Activator.CreateInstance(type);
			dict[type] = value;
		}

		return value;
	}

	T GetValue<T>()
	{
		return ObjectHolder<T>.Obj;
	}

	void Start()
	{
	}

	void Update()
	{

	}
}

public static class ObjectHolder<T>
{
	public static T Obj = Activator.CreateInstance<T>();
}

public class Dummy
{

}