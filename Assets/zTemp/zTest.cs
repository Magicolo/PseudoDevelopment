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

public class zTest : PMonoBehaviour
{
	public PType Type;
	public PAssembly Assembly;
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
		// Test build with assembly references
		// Check enum flags for correct implementation of Contains
	}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}

	void OnGUI()
	{
		GUILayout.TextArea(Convert.ToString(Type.Type), GUILayout.Width(Screen.width));
		GUILayout.TextArea(Convert.ToString(Assembly.Assembly), GUILayout.Width(Screen.width));
	}

	void OnInjectMember(InjectionContext context, IInjectableMember member)
	{
		member.Inject(context);
	}

	object OnResolveParameter(InjectionContext context, IInjectableParameter parameter)
	{
		return parameter.Inject(context);
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

public class Thing<T>
{
	public readonly T Value;

	public Thing(T value)
	{
		Value = value;
	}
}