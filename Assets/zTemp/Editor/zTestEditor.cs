using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Pseudo;
using UnityEditor;
using Pseudo.Internal.Editor;

[CustomEditor(typeof(zTest))]
public class zTestEditor : PEditor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var test = (zTest)target;

		if (GUILayout.Button("Schema Test"))
		{
			var schema = Schema.Create("Assets/zTemp/Data/", "BobaFett", typeof(zTest));
			var block = schema.CreateFunctionDeclaration("Update");
			var function1 = schema.CreateInstanceFunction("Method1");
			var function2 = schema.CreateInstanceFunction("Method2");
			var variable1 = schema.CreateInstanceVariable("Property1");
			var variable2 = schema.CreateInstanceVariable("Property2");
			function1.SetParameter(variable2, 0);
			function2.SetParameter(schema.CreateThis(), 0);
			function2.SetParameter(variable1, 1);
			block.AddFunction(function1);
			block.AddFunction(function2);
			block.AddFunction(schema.CloneNode(function1));

			if (schema.IsValid())
				schema.Compile();
			else
				PDebug.Log("Was not valid.");
		}

		if (GUILayout.Button("Schema Complie"))
			((Schema)test.Schema).Compile();

		if (GUILayout.Button("Schema Destroy All"))
			((Schema)test.Schema).DestroyNodes();
	}
}
