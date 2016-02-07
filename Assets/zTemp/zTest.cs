using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;
using UnityEngine.Assertions;
using Pseudo.Internal.Entity;
using UnityEngine.Events;
using System.Reflection;
using Pseudo.Internal;
using UnityEngine.SceneManagement;

public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	[Inject]
	IEntityManager entityManager = null;
	public const int iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{
		//entityManager.CreateEntity(Entity);
	}

	void Update()
	{
		var array1 = new FragmentArray<string>(8);
		array1.Dispose();

		if (SpawnMany)
		{
			for (int i = 0; i < 100; i++)
				entityManager.CreateEntity(Entity);
		}
	}

	public class ToInject
	{
		[Inject]
		public IEntityManager EntityManager { get; set; }
	}

	public TElement GetSomtin<TList, TElement>(TList list) where TList : IList<TElement>
	{
		return list.First();
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
	public partial class Events
	{
		public static readonly Events OnClash = new Events(1);
		public static readonly Events OnSplash = new Events(2);
	}

	public partial class EntityGroups
	{
		public static readonly EntityGroups Food1 = new EntityGroups(1);
		public static readonly EntityGroups Food2 = new EntityGroups(2);
		public static readonly EntityGroups Food3 = new EntityGroups(3);
	}
}