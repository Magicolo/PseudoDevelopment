using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Pseudo.Editor.Internal;
using System.IO;
using Microsoft.CSharp;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Ast;
using ICSharpCode.NRefactory.Visitors;
using ICSharpCode.NRefactory.PrettyPrinter;
using ICSharpCode.NRefactory.Parser;
using System.Reflection;
using Pseudo.Internal;
using UnityEditorInternal;
using Pseudo;

[CustomEditor(typeof(zTest))]
public class zTestEditor : PEditor
{
	const string directory = "Assets/zTemp/Data/";
	const string tempDirectory = "Assets/zTemp/Data/.Temp/";
	const string scriptFileName = "temp.cs";
	const string scriptPath = directory + scriptFileName;
	const string scriptTempPath = tempDirectory + scriptFileName;
	public static TypeData[] Types = new TypeData[0];

	CompilationUnit unit;

	readonly Dictionary<string, ReorderableList> identifierToList = new Dictionary<string, ReorderableList>();
	readonly Queue<INode> toRemove = new Queue<INode>();

	public override void OnEnable()
	{
		base.OnEnable();

		Reset();
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		//ShowCompilationUnit(unit);
		//Separator();
		//ShowSaveButton();
		//ShowResetButton();
		//ShowCode();
		//CommitRemovals();
	}

	void ShowCompilationUnit(CompilationUnit unit)
	{
		for (int i = 0; i < unit.Children.Count; i++)
		{
			var child = unit.Children[i];
			ShowNode(GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, GetHeight(child)), child);
		}
	}

	void ShowNode(Rect rect, INode node)
	{
		if (node is TypeDeclaration)
			ShowType(rect, (TypeDeclaration)node);
		else if (node is FieldDeclaration)
			ShowField(rect, (FieldDeclaration)node);
		else if (node is MethodDeclaration)
			ShowMethod(rect, (MethodDeclaration)node);

		var style = new GUIStyle("MiniToolbarButtonLeft")
		{
			clipping = TextClipping.Overflow,
			contentOffset = new Vector2(0, -1f),
			fontStyle = FontStyle.Bold,
			fontSize = 12
		};

		if (GUI.Button(new Rect(rect) { x = rect.x + rect.width - 22f, y = rect.y + 2f, width = 18f, height = 18f }, "−".ToGUIContent(), style))
			toRemove.Enqueue(node);
	}

	void ShowUnit(Rect rect, CompilationUnit unit)
	{
		var list = GetList(unit);
		list.draggable = false;
		list.DoList(rect);
	}

	void ShowType(Rect rect, TypeDeclaration type)
	{
		var list = GetList(type);
		list.DoList(rect);
	}

	void ShowField(Rect rect, FieldDeclaration field)
	{
		EditorGUI.LabelField(rect, GetLabel(field));
	}

	void ShowMethod(Rect rect, MethodDeclaration method)
	{
		var label = GetLabel(method);
		Type type;
		MethodInfo methodInfo;

		if (method.Parent == null ||
			(type = TypeUtility.GetType(((TypeDeclaration)method.Parent).GetFullTypeName())) == null ||
			(methodInfo = type.GetMethod(method.Name, method.GetParameterTypes().ToArray())) == null)
		{
			EditorGUI.LabelField(rect, label);
		}
		else
		{
			var buttonStyle = new GUIStyle(EditorStyles.miniButton)
			{
				alignment = TextAnchor.MiddleLeft
			};

			rect.y += 1f;
			rect.width -= 32f;
			rect.height -= 4f;
			if (GUI.Button(rect, label, buttonStyle))
			{
				if (methodInfo.IsStatic)
					methodInfo.Invoke(null, methodInfo.GetDefaultParameters());
				else
				{
					var instance = Activator.CreateInstance(type);
					methodInfo.Invoke(instance, methodInfo.GetDefaultParameters());
				}
			}
		}
	}

	void ShowSaveButton()
	{
		if (GUILayout.Button("Save"))
		{
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			unit.GenerateCode(scriptPath);
			AssetDatabase.Refresh();
		}
	}

	void ShowResetButton()
	{
		if (GUILayout.Button("Reset"))
		{
			unit = GenerateNRefactory();
			Generate();
			Reset();
		}
	}

	void ShowCode()
	{
		var code = unit.GenerateCode();
		EditorGUILayout.TextArea(code, GUILayout.Height(code.Count(c => c == '\n') * 16f));
	}

	ReorderableList GetList(INode node)
	{
		ReorderableList list;
		var key = node.ToString();

		if (!identifierToList.TryGetValue(key, out list))
		{
			list = CreateList(node);
			identifierToList[key] = list;
		}

		return list;
	}

	ReorderableList CreateList(INode node)
	{
		bool hasChildren = node.Children.Count > 0;

		return new ReorderableList(node.Children, typeof(INode), true, true, true, false)
		{
			drawHeaderCallback = rect => EditorGUI.LabelField(rect, GetLabel(node), EditorStyles.boldLabel),
			drawElementCallback = (rect, index, active, focused) => ShowNode(rect, node.Children[index]),
			drawElementBackgroundCallback = (rect, index, active, focused) =>
			{
				if (Event.current.type != EventType.Repaint || !hasChildren)
					return;

				var style = new GUIStyle("RL Element");
				style.Draw(new Rect(rect) { height = GetHeight(node.Children[index]) - 2f }, false, active, active, focused);
			},
			elementHeightCallback = index => GetHeight(node.Children[index]),
			onAddDropdownCallback = (rect, list) =>
			{
				var menu = new GenericMenu();

				menu.DropDown(rect);
			},
			onReorderCallback = list => Generate(),
			onAddCallback = list => { Generate(); Reset(); },
			onRemoveCallback = list => { list.list.RemoveAt(list.index); Generate(); Reset(); },
		};
	}

	string GetLabel(INode node)
	{
		if (node is TypeDeclaration)
		{
			var type = (TypeDeclaration)node;
			return string.Format("{0}", type.Name);
		}
		else if (node is FieldDeclaration)
		{
			var field = (FieldDeclaration)node;
			return string.Format("{0} : {1}", string.Join(", ", field.Fields.Select(f => f.Name).ToArray()), field.TypeReference.GenerateCode());
		}
		else if (node is MethodDeclaration)
		{
			var method = (MethodDeclaration)node;
			return string.Format("{0} ({1})", method.Name, string.Join(", ", method.Parameters.Select(p => p.TypeReference.GenerateCode() + " " + p.ParameterName).ToArray()));
		}
		else
			return node.ToString();
	}

	float GetHeight(INode node)
	{
		bool hasChildren = node.Children.Count > 0;
		float height;

		if (hasChildren)
			height = 44f;
		else if (node is TypeDeclaration)
			height = 60f;
		else height = 18f;

		foreach (var child in node.Children)
			height += GetHeight(child);

		return height;
	}

	CompilationUnit GetUnit()
	{
		if (File.Exists(scriptTempPath))
		{
			using (var reader = new StringReader(File.ReadAllText(scriptTempPath)))
			{
				var parser = ParserFactory.CreateParser(SupportedLanguage.CSharp, reader);
				parser.Parse();

				return parser.CompilationUnit;
			}
		}
		else
		{
			unit = GenerateNRefactory();
			Generate();
			Reset();

			return unit;
		}
	}

	CompilationUnit GenerateNRefactory()
	{
		return new CompilationUnit().AddChildren(
			NRefactoryUtility.CreateTypeDeclaration("CLASS", ClassType.Class, Modifiers.Public).AddChildren(
				NRefactoryUtility.CreateFieldDeclaration("VARIABLE", typeof(int[]), Modifiers.Protected),
				new MethodDeclaration
				{
					Name = "FUNCTION",
					TypeReference = NRefactoryUtility.CreateTypeReference("void"),
					Modifier = Modifiers.Public,
					Parameters = new List<ParameterDeclarationExpression>
					{
						new ParameterDeclarationExpression(NRefactoryUtility.CreateTypeReference(typeof(List<string>)), "PARAMETER")
					},
					Body = new BlockStatement().AddChildren(
						new ExpressionStatement(
							new InvocationExpression(
								new MemberReferenceExpression(
									new TypeReferenceExpression(
										NRefactoryUtility.CreateTypeReference(typeof(Debug))), "Log"),
								new List<Expression>
								{
									new PrimitiveExpression("TEST")
								})))
				}));
	}

	void Generate()
	{
		if (!Directory.Exists(directory))
			Directory.CreateDirectory(directory);

		if (!Directory.Exists(tempDirectory))
			Directory.CreateDirectory(tempDirectory);

		unit.GenerateCode(scriptTempPath);
	}

	void Reset()
	{
		unit = GetUnit();
		identifierToList.Clear();
	}

	void CommitRemovals()
	{
		bool didRemove = false;

		while (toRemove.Count > 0)
		{
			var child = toRemove.Dequeue();

			if (child.Parent != null)
				child.Parent.Children.Remove(child);

			didRemove = true;
		}

		if (didRemove)
		{
			Generate();
			Reset();
		}
	}

	[UnityEditor.Callbacks.DidReloadScripts]
	static void OnScriptReload()
	{
		Types = TypeUtility.AllTypes
			.Where(t => t.IsClass && t.IsPublic && !t.IsSpecialName)
			.OrderBy(t => t.GUID)
			.Select(t => new TypeData(t))
			.ToArray();
	}

	public class TypeData
	{
		public readonly Type Type;
		public readonly string DisplayName;
		public readonly FieldInfo[] Fields;
		public readonly PropertyInfo[] Properties;
		public readonly MethodInfo[] Methods;

		public TypeData(Type type)
		{
			Type = type;
			DisplayName = type.FullName.Replace('.', '/');
			Fields = type.GetFields();
			Properties = type.GetProperties();
			Methods = type.GetMethods();
		}
	}
}
