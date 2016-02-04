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

public class zTest : PMonoBehaviour, IMessageable
{
	public MessageEnum E;
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	[Inject]
	IEntityManager entityManager = null;
	public const int iterations = 10000;

	[Button]
	public bool test;
	void Test()
	{
		var m = new MessageManager();
		m.Send(this, E.Value);
		//entityManager.CreateEntity(Entity);
		//SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 2);
	}

	[Message(Messages.One)]
	void Callback()
	{
		PDebug.LogMethod();
	}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}

	public void OnMessage<TId>(TId message)
	{
		PDebug.LogMethod(message);
	}
}

[MessageEnum]
public enum Messages
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
















public enum AllEvents
{
	OnFire,
	OnMove,
	OnStart,
	OnFall
}

public class AComponent : ComponentBehaviour
{
	[Message(AllEvents.OnFire)]
	void OnFire()
	{
		// Do something
	}

	[Message(AllEvents.OnMove)]
	void OnMove()
	{
		// Do something
	}
}