using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

namespace Pseudo.Internal.Tests
{
	public class PoolableTest : SubPoolableTest
	{
		[DoNotInitialize]
		public GameObject AGameObject;
		public Transform ATransform;
		public Rect Rect;
		public Bounds Bounds;
		[DoNotInitialize]
		public float Popy { get; set; }

		public override void OnCreate()
		{
			base.OnCreate();

			Float = PRandom.Range(1f, 100f);
			Vector = UnityEngine.Random.insideUnitCircle;
			ATransform = null;
			Rect = new Rect(1, 23, 4, 5);
			Bounds = new Bounds(Vector, Vector);
			Zone.LocalCircle = new Circle(Vector, Float);
			Popy = Float;
		}
	}

	public class SubPoolableTest : PMonoBehaviour
	{
		public float Float;
		public Vector2 Vector;
		[InitializeContent]
		public CircleZone Zone;
	}
}