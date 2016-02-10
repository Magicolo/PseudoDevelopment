using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;

public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	[Inject]
	IEntityManager entityManager = null;
	public const int iterations = 1000;

	[SerializeField, PropertyField]
	float f;
	public float F
	{
		get { return f; }
		set { f = value; }
	}

	[Button]
	public bool test;
	void Test()
	{
		entityManager.CreateEntity(Entity);
	}

	void Update()
	{
		if (SpawnMany)
		{
			for (int i = 0; i < 100; i++)
				entityManager.CreateEntity(Entity);
		}
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

public class MessagesComparer : IEqualityComparer<Messages>
{
	public bool Equals(Messages x, Messages y)
	{
		return x == y;
	}

	public int GetHashCode(Messages obj)
	{
		return (int)obj;
	}
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