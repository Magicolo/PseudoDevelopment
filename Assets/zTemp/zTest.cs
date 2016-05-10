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
using Pseudo.Initialization;
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
		var flags = new ByteFlag(1, 2, 3);
		PDebug.Log(flags);
		var b = flags + 4;
		PDebug.Log(flags, b);
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
}

[MessageEnum]
public enum Messages : byte
{
	Zero,
	One,
	Two,
	Three,
}

public interface IFlags<T>
{
	T Add(byte flag);
	T Add(T flags);
	T Remove(byte flag);
	T Remove(T flags);
	T And(T flags);
	T Or(T flags);
	T Xor(T flags);
	T Not();
	bool Has(byte flag);
	bool HasAll(T flags);
	bool HasAny(T flags);
	bool HasNone(T flags);
}