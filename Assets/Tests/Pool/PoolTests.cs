using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	public class PoolTests : PMonoBehaviour
	{
		public PoolableTest PrefabTest;

		[Button]
		public bool createPoolableInstance;
		[Button]
		public bool createRecycle1000PoolableInstance;
		[Button]
		public bool instantiateDestroy1000Instances;

		void CreatePoolableItemTest()
		{
			PMonoBehaviour instance = PrefabPoolManager.Create(PrefabTest);
			StartCoroutine(RecycleAfterDelay(instance, 1f));
		}

		void CreateRecycle1000PoolableItemTest()
		{
			for (int i = 0; i < 1000; i++)
				PrefabPoolManager.Recycle(PrefabPoolManager.Create(PrefabTest));
		}

		void InstantiateDestroy1000InstancesTest()
		{
			for (int i = 0; i < 1000; i++)
				Instantiate(PrefabTest).CachedGameObject.Destroy();
		}

		void Awake()
		{
		}

		void Update()
		{
			if (createPoolableInstance)
				CreatePoolableItemTest();
			else if (createRecycle1000PoolableInstance)
				CreateRecycle1000PoolableItemTest();
			else if (instantiateDestroy1000Instances)
				InstantiateDestroy1000InstancesTest();
		}

		IEnumerator RecycleAfterDelay(PMonoBehaviour instance, float delay)
		{
			for (float counter = 0f; counter < delay; counter += Time.deltaTime)
				yield return null;

			PrefabPoolManager.Recycle(instance);
		}
	}
}