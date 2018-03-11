using UnityEngine;
using System.Collections;
using UnityEditor;
using DSA.Extensions.Conversations;
using DSA.Extensions.Conversations.DataStructure;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.DataStructure.Editor
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
			serializedObject.Update();
			DrawDefaultInspector();
			if (GUILayout.Button("Edit"))
			{
				ConversationEditorWindow.Init();
			}
			serializedObject.ApplyModifiedProperties();
			return;
		}
	}
}