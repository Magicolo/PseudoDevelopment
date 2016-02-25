using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using Pseudo.Internal.Injection;
using System.Reflection;
using System.Runtime.InteropServices;

public class zTest : PMonoBehaviour, ISerializationCallbackReceiver
{
	public EntityBehaviour Entity;
	public bool SpawnMany = true;
	[Inject]
	IEntityManager entityManager = null;
	public int Iterations = 1000;

	public object Data;
	[SerializeField]
	string dataType;
	[SerializeField]
	string data;

	[Button]
	public bool test;
	void Test()
	{
		//entityManager.CreateEntity(Entity);
	}

	void Update()
	{
		if (SpawnMany)
			entityManager.CreateEntity(Entity);
	}

	void OnDrawGizmos()
	{
		Debug.DrawRay(Vector3.zero, CachedTransform.TransformVector(new Vector3(10f, 0f, 0f)));
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
		if (Data == null)
		{
			data = null;
			return;
		}

		dataType = Data.GetType().AssemblyQualifiedName;
		data = JsonUtility.ToJson(Data);
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(dataType))
			return;

		var type = TypeUtility.GetType(dataType);
		Data = JsonUtility.FromJson(data, type);
	}
}

public class Something
{
	public float F;
}

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