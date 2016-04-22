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

public class zTest : PMonoBehaviour
{
	/*
	* IInitializer (initialize instances to their reference state
	* ITypeAnalyzer (analyzes a type and produces a corresponding ITypeInfo)
	* ITypeInfo (holds the info and accessors required for initialization)
	* IUpdater *find better name* (updates the IInitializer and feeds it the instances to be initialized)
	* IPool / IPool<T>
	* IPoolManager *find better name* (centralises the access to the pools)
	*/
	// Solve the Additionnal arguments implementation
	// Test build with assembly references
	// Check enum flags for correct implementation of Contains

	public PType Type;
	public PAssembly[] Assemblies;
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	public int Iterations = 100000;

	[Inject]
	readonly IEntityManager entityManager = null;
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
