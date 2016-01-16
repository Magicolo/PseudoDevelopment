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
		public EntityMatchOld Match;

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
			PDebug.Log(EntityManagerOld.GetEntityGroup(Match).Entities);
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

			StartCoroutine(DelayedAction(() => entity.GameObject.Destroy(), 3f));
		}

		[Button]
		public bool assemblePooledEntityTest;
		void AssemblePooledEntityTest()
		{
			var entity = TypePoolManager.Create<PEntity>();
			entity.AddComponent<DataComponent>().BYTE = 66;

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

	[EntityGroups]
	public static class EntityGroups
	{
		public static readonly ByteFlag Character_Player = new ByteFlag(1);
		public static readonly ByteFlag Character_Enemy = new ByteFlag(2);
		public static readonly ByteFlag Item_Weapon_All = new ByteFlag(3, 4);
		public static readonly ByteFlag Item_Weapon_Axe = new ByteFlag(3);
		public static readonly ByteFlag Item_Weapon_Mace = new ByteFlag(4);
	}
}