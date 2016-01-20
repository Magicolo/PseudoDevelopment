using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;
using UnityEngine.Assertions;
using Pseudo.Internal.Entity;

public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	[Inject]
	IEntityManager entityManager = null;
	public const int iterations = 100000;

	[Button]
	public bool test;
	void Test()
	{
		entityManager.CreateEntity(Entity);
		//SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 2);
	}

	void Update()
	{
		entityManager.CreateEntity(Entity);
	}
}

namespace Pseudo
{
	public partial class Events
	{
		public static readonly Events OnAll = new Events(1, 2, 3, 4);
		public static readonly Events OnEquip = new Events(1);
		public static readonly Events OnUnequip = new Events(2);
		public static readonly Events OnBuy = new Events(3);
		public static readonly Events OnDie = new Events(4);
	}
}