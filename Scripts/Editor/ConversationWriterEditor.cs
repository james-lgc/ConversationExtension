using UnityEngine;
using System.Collections;
using UnityEditor;
using DSA.Extensions.Conversations;
using DSA.Extensions.Base.Editor;
using System.Reflection;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomEditor(typeof(ConversationWriter))]
	public class ConversationWriterEditor : UnityEditor.Editor
	{
		//Read conversations from Json file when enabled in inspector 
		private void OnEnable()
		{
			ConversationWriter conversationWriter = (ConversationWriter)target;
			conversationWriter.Set();
		}

		//GUI Button to serialize conversation to Json file after edited in inspector
		public override void OnInspectorGUI()
		{
			ConversationWriter conversationWriter = (ConversationWriter)target;
			serializedObject.Update();
			DrawDefaultInspector();
			System.Type type = conversationWriter.GetType();
			BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			FieldInfo fi = type.GetField("conversationList", flags);
			ConversationList list = (ConversationList)fi.GetValue(conversationWriter);
			type = fi.FieldType;
			fi = type.GetField("conversations", flags);
			System.Collections.IList array = (System.Collections.IList)fi.GetValue(list);
			Conversation conv = (Conversation)array[1];
			EditorGUILayout.TextField(conv.Text);
			if (GUILayout.Button("Edit"))
			{
				ConversationEditorWindow.Init();
			}
			serializedObject.ApplyModifiedProperties();
			return;
		}
	}
}