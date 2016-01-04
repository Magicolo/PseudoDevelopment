using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.EventSystems;
using System.Threading;
using System.Reflection;
using Pseudo.Internal.Audio;
using System.Runtime.Serialization;
using Pseudo.Internal;

namespace Pseudo.Internal.Tests
{
	[Serializable, ComponentCategory("Data")]
	public class DataComponent : ComponentBase
	{
		public DataComponent() { }

		public sbyte SBYTE;
		public byte BYTE;
		public short SHORT;
		public ushort USHORT;
		public int INT;
		public uint UINT;
		public long LONG;
		public ulong ULONG;
		public float FLOAT;
		public double DOUBLE;
		public decimal DECIMAL;
		public Vector2 VECTOR2;
		public Vector3 VECTOR3;
		public Vector4 VECTOR4;
		public Quaternion QUATERNION;
		public Color COLOR;
		public Rect RECT;
		public Bounds BOUNDS;
		public string STRING;
		public char CHAR;
		public AnimationCurve ANIMATIONCURVE;
		public AudioClipLoadType ENUM;
		public EntityMatch MATCH;
	}

	[Serializable, EntityRequires(typeof(DataComponent))]
	public class SmallComponent : ComponentBase
	{
		[Min]
		public int INT;
		public EntityMatch MATCH;
		[Min]
		public int[] INTS;
		public EntityMatch[] MATCHES;
		public EntityGroups Group;

		void Spawn()
		{
			PDebug.LogMethod(GetHashCode());
		}
	}

	[Serializable, ComponentCategory("Reference"), EntityRequires(typeof(DataComponent))]
	public class ReferencezComponent : ComponentBase, IUpdateable
	{
		[InitializeValue]
		public GameObject GAMEOBJECT;
		public string TRANSFORM;
		[InitializeValue]
		public PEntity[] ENTITY;

		float IUpdateable.UpdateRate { get { return 0f; } }

		void IUpdateable.Update()
		{

		}
	}

	[Serializable, EntityRequires(typeof(TimeComponent)), ComponentCategory("Motion")]
	public class MotionComponent : ComponentBase, IFixedUpdateable
	{
		public IEntityGroup PlayerGroup = EntityManager.GetEntityGroup(EntityGroups.Character_All).Filter(typeof(ReferencezComponent));
		public Rigidbody2D Rigidbody;
		[Polar]
		public Vector2 Direction;

		void IFixedUpdateable.FixedUpdate()
		{
			Rigidbody.Translate(Direction * Entity.GetComponent<TimeComponent>().DeltaTime);
		}
	}
}