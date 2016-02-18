using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Injection;
using System.Reflection;

public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	[Inject]
	IEntityManager entityManager = null;
	public int Iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{
		entityManager.CreateEntity(Entity);
	}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}

	void OnDrawGizmos()
	{
		Debug.DrawRay(Vector3.zero, CachedTransform.TransformVector(new Vector3(10f, 0f, 0f)));
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

namespace Pseudo
{
	public partial class EntityGroups
	{
		public static readonly EntityGroups Food1 = new EntityGroups(1);
		public static readonly EntityGroups Food2 = new EntityGroups(2);
		public static readonly EntityGroups Food3 = new EntityGroups(3);
	}
}