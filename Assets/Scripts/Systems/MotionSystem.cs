using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;

public class MotionSystem : SystemBase, IUpdateable
{
	IEntityGroup entities;

	public override void OnInitialize()
	{
		base.OnInitialize();

		entities = EntityManager.Entities.Filter(new[]
		{
			typeof(MotionComponent),
			typeof(TimeComponent)
		});
	}

	public void Update()
	{
		for (int i = 0; i < entities.Count; i++)
		{
			var entity = entities[i];
			var time = entity.GetComponent<TimeComponent>();
			var motion = entity.GetComponent<MotionComponent>();

			motion.CachedTransform.position += motion.Motion.ToVector3() * time.DeltaTime;

			if (time.Time % 3 > 2.9f)
				EventManager.Trigger(Events.OnEquip, entity);
		}
	}
}