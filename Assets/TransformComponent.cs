using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo.Internal.Entity3;
using Pseudo;

public class TransformComponent : PMonoBehaviour, ITransformComponent
{
	public Transform Transform
	{
		get { return CachedTransform; }
	}
}

public interface ITransformComponent : Pseudo.Internal.Entity3.IComponent
{
	Transform Transform { get; }
}