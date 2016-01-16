using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Zenject;
using Pseudo.Internal.Entity;
using ModestTree.Util;

public class SceneInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		BindManagers();
	}

	void BindManagers()
	{
		Container.Bind<IEntityManager>().ToSingle<EntityManager>();
		Container.BindAllInterfacesToSingle<SystemManager>();
		Container.BindAllInterfacesToSingle<EventManager>();
		Container.BindLateTickablePriority<EventManager>(100);
	}
}