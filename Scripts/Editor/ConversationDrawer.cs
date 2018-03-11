using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DSA.Extensions.Conversations.DataStructure;
using System;
using System.Reflection;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.DataStructure.Editor
{
	[CustomPropertyDrawer(typeof(Conversation))]
	//Overrides how Conversation class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	//Inherits from BasePropertyDrawer to access custom defaults
	public class ConversationDrawer : DataItemDrawer
	{
		private UnityEditorInternal.ReorderableList stageList;
		private SerializedProperty topicQuestion;
		private SerializedProperty stages;
		private SerializedProperty defaultStage;

		//Ensure all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			name = sentProperty.FindPropertyRelative("name");
			topicQuestion = sentProperty.FindPropertyRelative("topicQuestion");
			stages = sentProperty.FindPropertyRelative("stages");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			//create action for edit button in list
			//action opens property in conversation window
			System.Action<SerializedProperty> editAction = DSA.Extensions.Conversations.DataStructure.Editor.ConversationEditorWindow.Init;
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
			stageList = GetDefaultEditButtonList(stages, "Stages", editAction, GetStageCountString, addAction);
		}

		//sent as a func to list draw element call back
		//shows story index of the stage
		private string GetStageCountString(SerializedProperty sentStage)
		{
			SerializedProperty storyIndex = sentStage.FindPropertyRelative("storyIndex");
			SerializedProperty mythID = storyIndex.FindPropertyRelative("mythID");
			SerializedProperty storyID = storyIndex.FindPropertyRelative("storyID");
			SerializedProperty threadID = storyIndex.FindPropertyRelative("threadID");
			SerializedProperty stageID = storyIndex.FindPropertyRelative("stageID");
			//create single string to represent story index
			return "[" + mythID.intValue.ToString() + "." + storyID.intValue.ToString() + "." + threadID.intValue.ToString() + "." + stageID.intValue.ToString() + "]";
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect sentPosition, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(sentPosition);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw name field
			newPosition = DrawTextField(newPosition, name, "Name");
			//draw topic question field
			newPosition = DrawTextField(newPosition, topicQuestion, "Topic Question");
			//draw stages
			newPosition = DrawReorderableList(newPosition, stageList, "Stages");
		}

		//calculate the height of this property
		//some unnecissary duplicate code, but this layout keeps it easy to track height
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			//initial padding height
			float totalHeight = initialVerticalPaddingHeight;
			//add name label
			totalHeight += GetAddedHeight(lineHeight);
			//add name field
			totalHeight += GetAddedHeight(lineHeight);
			//add topic question
			totalHeight += GetAddedHeight(lineHeight);
			//add default stage
			totalHeight += GetAddedHeight(lineHeight);
			//add stages label
			totalHeight += GetAddedHeight(lineHeight);
			//add stages
			totalHeight += GetAddedHeight(stageList.GetHeight());
			//end padding
			totalHeight += GetAddedHeight(lineHeight);
			return totalHeight;
		}
	}
}