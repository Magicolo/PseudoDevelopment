using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class InputMotionSystem : SystemBase, IUpdateable
{
	IEntityGroup entities;

	public override void OnInitialize()
	{
		base.OnInitialize();

		entities = EntityManager.Entities.Filter(new[]
		{
			typeof(TimeComponent),
			typeof(InputMotionComponent)
		});
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
		{
			var entity = entities[i];
			var time = entity.GetComponent<TimeComponent>();
			var motion = entity.GetComponent<InputMotionComponent>();

			motion.CachedTransform.Translate(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * time.DeltaTime * motion.Speed, Axes.XY);
		}
	}
}
