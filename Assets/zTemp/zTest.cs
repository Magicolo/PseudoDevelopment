using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Injection;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using Pseudo.Internal.References;
using UnityEngine.Events;
using UnityEngine.Scripting;
using Pseudo.Internal;
using Pseudo.Internal.Entity;
using System.Text;

public class zTest : PMonoBehaviour
{
	[Serializable]
	public class ComponentProperty : PropertyReference<IComponent> { }
	public ScriptableObject Schema;

	public PType Type;
	public ComponentProperty Component;
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	[Inject]
	IEntityManager entityManager = null;
	public int Iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{

	}

	protected override void Start()
	{
		base.Start();

		Test();
	}

	void Update()
	{
		if (SpawnMany)
		{
			entityManager.CreateEntity(Entity);
		}
	}

	float property1 = 2;
	float property2 = 1.1f;

	public float Property1
	{
		get { return property1; }
		set { property1 = value; }
	}

	public float Property2
	{
		get { return property2; }
		set { property2 = value; }
	}

	public float Method1(float a)
	{
		return a + 1f;
	}

	public void Method2(zTest test, float d, float b) { }
}

[MessageEnum]
public enum Messages : byte
{
	Zero,
	One,
	Two,
	Three,
}

namespace Pseudo
{
	public partial class EntityGroups
	{
		public static readonly EntityGroups Food1 = new EntityGroups(1);
		public static readonly EntityGroups Food2 = new EntityGroups(2);
		public static readonly EntityGroups Food3 = new EntityGroups(3);
	}
}