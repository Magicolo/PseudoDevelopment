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
		public bool hasPositionComponentTest;
		void HasPositionComponentTest()
		{
			PDebug.Log(Entity.HasComponent<DataComponent>());
		}

		[Button]
		public bool findMatchingEntities;
		void FindMatchingEntities()
		{
			PDebug.Log(EntityManager.GetEntityGroup(Match).Entities);
		}

		[Button]
		public bool poolEntity;
		void PoolEntity()
		{
			StartCoroutine(RecycleAfterDelay(PrefabPoolManager.Create(Entity), 2f));
		}

		//void Update()
		//{
		//	InstantiationTest();
		//}

		void InstantiationTest()
		{
			for (int i = 0; i < 100; i++)
				PrefabPoolManager.Recycle(PrefabPoolManager.Create(Entity));
		}

		IEnumerator RecycleAfterDelay(PEntity entity, float delay)
		{
			for (float counter = 0; counter < delay; counter += Time.deltaTime)
				yield return null;

			PrefabPoolManager.Recycle(entity);
		}
	}
}