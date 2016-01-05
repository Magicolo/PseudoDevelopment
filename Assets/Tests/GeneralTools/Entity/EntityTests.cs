using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	public class EntityTests : PMonoBehaviour
	{
		[EntityRequires(typeof(DataComponent))]
		public PEntity Entity;
		public EntityMatch Match;

		[Button]
		public bool hasDataComponentTest;
		void HasDataComponentTest()
		{
			PDebug.Log(Entity.HasComponent<DataComponent>());
		}

		[Button]
		public bool findMatchingEntitiesTest;
		void FindMatchingEntitiesTest()
		{
			PDebug.Log(EntityManager.GetEntityGroup(Match).Entities);
		}

		[Button]
		public bool sendMessageTest;
		void SendMessageTest()
		{
			Entity.SendMessage(EntityMessages.Spawn);
		}

		[Button]
		public bool instantiateEntityTest;
		void InstantiateEntityTest()
		{
			var entity = Instantiate(Entity);
			StartCoroutine(DelayedAction(() => entity.GameObject.Destroy(), 3f));
		}

		[Button]
		public bool poolEntityTest;
		void PoolEntityTest()
		{
			var entity = PrefabPoolManager.Create(Entity);
			StartCoroutine(DelayedAction(() => PrefabPoolManager.Recycle(entity), 3f));
		}

		[Button]
		public bool assembleEntityTest;
		void AssembleEntityTest()
		{
			var entity = new GameObject("Assembled Entity").AddComponent<PEntity>();
			entity.AddComponent<DataComponent>().BYTE = 32;
			entity.AddComponent<TimeComponent>().Channel = TimeManager.TimeChannels.UI;

			StartCoroutine(DelayedAction(() => entity.GameObject.Destroy(), 3f));
		}

		[Button]
		public bool assemblePooledEntityTest;
		void AssemblePooledEntityTest()
		{
			var entity = TypePoolManager.Create<PEntity>();
			entity.AddComponent<DataComponent>().BYTE = 66;
			entity.AddComponent<TimeComponent>().Channel = TimeManager.TimeChannels.Enemy;

			StartCoroutine(DelayedAction(() => TypePoolManager.Recycle(entity), 3f));
		}

		void Update()
		{
			//UnityInstantiateTest();
			//PoolCreateTest();
		}

		void UnityInstantiateTest()
		{
			for (int i = 0; i < 1000; i++)
				Instantiate(Entity).GameObject.Destroy();
		}

		void PoolCreateTest()
		{
			for (int i = 0; i < 1000; i++)
				PrefabPoolManager.Recycle(PrefabPoolManager.Create(Entity));
		}

		IEnumerator DelayedAction(Action action, float delay)
		{
			for (float counter = 0; counter < delay; counter += Time.deltaTime)
				yield return null;

			action();
		}
	}

	[Serializable]
	public class EntityGroups : EntityGroupDefinition
	{
		public static readonly EntityGroups Character_All = new EntityGroups(0, 1);
		public static readonly EntityGroups Character_Player = new EntityGroups(0);
		public static readonly EntityGroups Character_Enemy = new EntityGroups(1);

		public static readonly EntityGroups Item_All = new EntityGroups(100, 101, 200, 201);
		public static readonly EntityGroups Item_Weapon_Sword = new EntityGroups(100);
		public static readonly EntityGroups Item_Weapon_Mace = new EntityGroups(101);
		public static readonly EntityGroups Item_Armor_Gold = new EntityGroups(200);
		public static readonly EntityGroups Item_Armor_Adamantium = new EntityGroups(201);

		public EntityGroups(params byte[] groupIds) : base(groupIds) { }
	}

	public class ComponentGroups : ComponentGroupDefinition
	{
		public static readonly ComponentGroups Moveable = new ComponentGroups(typeof(MotionComponent));

		public ComponentGroups(params Type[] componentTypes) : base(componentTypes) { }
	}
}