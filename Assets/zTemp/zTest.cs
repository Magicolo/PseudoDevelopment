using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Injection;
using Pseudo.EntityFramework;

public class zTest : PMonoBehaviour
{
	/*
	 * EntityFramework vs Unity
	 * -More flexibility for sending messages (implementable in Unity)
	 * 
	 * 
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
	public EntityBehaviour Entityz;
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
		// Make a pool behaviour for prefabs
		// Use attributes to tag default modules for Initializer, Copier, Caster, Cloner, PEqualityComparer, PComparer, etc...
		// Use attributes to define the types that can be handled by the modules		
		// Go over Generic components to fix pooling and remove dependency on EntityFramework
		// Check AudioManager to fix pooling
		// Check oscillation to fix pooling
		// Check ParticleManager to fix pooling
		// Kill EntityFramework
	}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entityz);
	}

	void OnGUI()
	{
		GUILayout.TextArea(Convert.ToString(Type.Type), GUILayout.Width(Screen.width));
		GUILayout.TextArea(PDebug.ToString(Assemblies), GUILayout.Width(Screen.width));
	}
}