using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;
using Pseudo.Internal.Entity;

public class SceneInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		BindManagers();
	}

	void BindManagers()
	{
		Container.BindAllInterfacesToSingle<SystemManager>();
		Container.Bind<IEntityManager>().ToSingle<EntityManager>();
		Container.BindIFactory<ByteFlag, IEntity>().ToFactory<Entity>();
		Container.Bind<IEventManager>().ToSingle<EventManager>();
	}
}
