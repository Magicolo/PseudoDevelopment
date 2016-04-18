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
		// Solve the Additionnal arguments implementation
		// Test build with assembly references
		// Check enum flags for correct implementation of Contains
		var method = typeof(zTest).GetMethod("Method");
		var parameter = method.GetParameters()[0];

		PDebug.Log(method, parameter);
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

	public void Method(int test = 123) { }
}

[MessageEnum]
public enum Messages : byte
{
	Zero,
	One,
	Two,
	Three,
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class InjectAllAttribute : PreserveAttribute
{

}