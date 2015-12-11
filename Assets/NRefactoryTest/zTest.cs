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

public class zTest : PMonoBehaviour
{
	const int iterations = 1000;
	Data D = new Data();

	[Button]
	public bool test;
	void Test()
	{
		//this.SetValueToFieldAtPath("D.Vs.0.x", PRandom.Range(0f, 1000f));
		//this.SetValueToFieldAtPath("D.Vs.1", new Vector2(PRandom.Range(0f, 1000f), 42f));
		//PDebug.Log(this.GetValueFromFieldAtPath("D.Vs.0.x"));
		//PDebug.Log(this.GetValueFromFieldAtPath("D.Vs.1"));
		//PDebug.LogTest("Reflection Get", () => this.GetValueFromFieldAtPath("D.Vs.0.x"), 1000);
		//PDebug.LogTest("Reflection Set", () => this.SetValueToFieldAtPath("D.Vs.0.x", 100f), 1000);
	}
}

[Serializable]
public class Data
{
	public object[] Vs =
	{
		new FloatHolder(),
		Vector2.right
	};

	public GameObject GO;
	public GameObject[] GOs;
}

public class FloatHolder
{
	public float x;
}