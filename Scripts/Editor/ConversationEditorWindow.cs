using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DSA.Extensions.Base.Editor;
using DSA.Extensions.Conversations;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations.DataStructure.Editor
{
	//an editor window class to edit conversation json data
	public class ConversationEditorWindow : DataEditorWindow<ConversationList>
	{
		protected override int maxNestedButtons { get { return 5; } }

		//Menu item initiailisation to load the top level conversation list
		[MenuItem("Window/Conversations")]
		public static void Init()
		{
			ConversationEditorWindow window = (ConversationEditorWindow)EditorWindow.GetWindow(typeof(ConversationEditorWindow));
			window.Set();
			window.Show();
		}

		//initialisation method with a sent property to display
		public static void Init(SerializedProperty sentProperty)
		{
			ConversationEditorWindow window = (ConversationEditorWindow)EditorWindow.GetWindow(typeof(ConversationEditorWindow));
			window.Set();
			if (window.propertyList == null) { window.propertyList = new List<SerializedProperty>(); }
			window.propertyList.Add(sentProperty);
			window.Show();
		}

		protected void OnFocus()
		{
			if (propertyList == null)
			{
				Init();
			}
		}

		protected void Set()
		{
			writer = (JsonWriter<ConversationList>)Resources.Load("ConversationWriter");
			base.Set(writer, "conversationList");
		}
	}
}