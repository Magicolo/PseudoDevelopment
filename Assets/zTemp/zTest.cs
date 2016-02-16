using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Injection;

public class zTest : PMonoBehaviour
{
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	IEntityManager entityManager = null;
	public int Iterations = 1000;
	IBinder binder;

	[Inject(Optional = true)]
	readonly IDummy dummy = null;

	[Button]
	public bool test;
	void Test()
	{
		//entityManager.CreateEntity(Entity);
	}

	void Update()
	{
		if (binder == null)
		{
			binder = new Binder();
			binder.Bind<IDummy>().ToSingle<Dummy>();
		}

		binder.Injector.Inject(this);

		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}
}

public interface IDummy { }
public class Dummy : IDummy { }

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