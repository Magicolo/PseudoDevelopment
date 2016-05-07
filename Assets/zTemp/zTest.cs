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
