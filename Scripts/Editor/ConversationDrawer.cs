using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DSA.Extensions.Conversations;
using System;
using System.Reflection;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(Conversation))]
	//Overrides how Conversation class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	public class ConversationDrawer : BaseConversationDrawer
	{
		private UnityEditorInternal.ReorderableList stageList;
		private SerializedProperty topicQuestion;

		//Ensure all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			name = sentProperty.FindPropertyRelative("name");
			topicQuestion = sentProperty.FindPropertyRelative("topicQuestion");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			//action overrides copied values and generates unique id
			System.Action<UnityEditorInternal.ReorderableList> addAction = (UnityEditorInternal.ReorderableList list) =>
			{
				int index = list.serializedProperty.arraySize;
				OnAddElement(list);
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("firstBranches").arraySize = 0;
				element.FindPropertyRelative("secondBranches").arraySize = 0;
			};
			//create list
			stageList = GetDefaultEditButtonList(dataArray, "Stages");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect sentPosition, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(sentPosition);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw name field
			newPosition = EditorTool.DrawTextField(newPosition, name, "Name");
			//draw topic question field
			newPosition = EditorTool.DrawTextField(newPosition, topicQuestion, "Topic Question");
			//draw stages
			newPosition = EditorTool.DrawReorderableList(newPosition, stageList, "Stages");
		}

		//calculate the height of this property
		//some unnecissary duplicate code, but this layout keeps it easy to track height
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			//initial padding height
			float totalHeight = EditorTool.InitialVerticalPadding;
			//add name label
			totalHeight += EditorTool.AddedLineHeight;
			//add name field
			totalHeight += EditorTool.AddedLineHeight;
			//add topic question
			totalHeight += EditorTool.AddedLineHeight;
			//add stages label
			totalHeight += EditorTool.AddedLineHeight;
			//add stages
			totalHeight += EditorTool.GetAddedHeight(stageList.GetHeight());
			//end padding
			totalHeight += EditorTool.AddedLineHeight;
			return totalHeight;
		}
	}
}