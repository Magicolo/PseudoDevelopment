using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pseudo;
using UnityEngine.EventSystems;
using System.Threading;
using System.Reflection;

public class zTest : PMonoBehaviour
{
	public zTest Integer;
	FieldInfo field = typeof(zTest).GetField("Integer");
	const int iterations = 1000;

	[Button]
	public bool test;
	void Test()
	{
		//PDebug.LogTest("Get", () => field.GetValue(this), 1000000);
		//PDebug.LogTest("Set", () => field.SetValue(this, i), 1000000);
	}

	void GetTest()
	{
		for (int i = 0; i < iterations; i++)
			field.GetValue(this);
	}

	void SetTest()
	{
		for (int i = 0; i < iterations; i++)
			field.SetValue(this, null);
	}

	void Update()
	{
		if (test)
		{
			GetTest();
			SetTest();
		}
	}
}