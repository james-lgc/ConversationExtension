using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Conversations.DataStructure;
using UnityEditor;
using System;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.DataStructure.Editor
{
	[CustomPropertyDrawer(typeof(Stage))]
	//Overrides how Stage class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	//Inherits from BasePropertyDrawer to access custom defaults
	public class StageDrawer : DataItemDrawer
	{
		private UnityEditorInternal.ReorderableList firstBranchesList;
		private UnityEditorInternal.ReorderableList secondBranchesList;
		private SerializedProperty topic;
		private SerializedProperty identifier;
		private SerializedProperty firstBranches;
		private SerializedProperty secondBranches;

		//Ensure all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			//set child properties
			topic = sentProperty.FindPropertyRelative("name");
			firstBranches = sentProperty.FindPropertyRelative("firstBranches");
			secondBranches = sentProperty.FindPropertyRelative("secondBranches");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			identifier = sentProperty.FindPropertyRelative("identifier");
			//create action for edit button in list
			//action opens property in conversation window
			System.Action<SerializedProperty> editAction = DSA.Extensions.Conversations.DataStructure.Editor.ConversationEditorWindow.Init;
			//method to return a string showing number of child elements in list item
			System.Func<SerializedProperty, string> endTextFunc = (SerializedProperty arrayProperty) =>
			{
				return GetArrayCountString(arrayProperty, "lines", "Line", "Lines");
			};
			//action overrides copied values and generates unique id
			System.Action<UnityEditorInternal.ReorderableList> addAction = (UnityEditorInternal.ReorderableList list) =>
			{
				int index = list.serializedProperty.arraySize;
				OnAddElement(list);
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("lines").arraySize = 0;
			};
			//create lists
			firstBranchesList = GetDefaultEditButtonList(firstBranches, "First Branches", editAction, endTextFunc, addAction);
			secondBranchesList = GetDefaultEditButtonList(secondBranches, "Second Branches", editAction, endTextFunc, addAction);
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw name field
			newPosition = DrawTextField(newPosition, topic, "Topic");
			//draw identifier
			newPosition = EditorTool.DrawArray(newPosition, identifier, "Identifier");
			//newPosition = new Rect(newPosition.x, newPosition.y + lineHeight, newPosition.width, 50F);
			//draw branches
			newPosition = DrawReorderableList(newPosition, firstBranchesList, "Branches");
			newPosition = DrawReorderableList(newPosition, secondBranchesList, null);
		}

		//Calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = initialVerticalPaddingHeight;
			//add topic label
			totalHeight = totalHeight + GetAddedHeight(lineHeight);
			//add topic field
			totalHeight = totalHeight + GetAddedHeight(lineHeight);
			//add identifier
			float identifierHeight = GetAddedHeight(lineHeight);
			for (int i = 0; i < identifier.arraySize; i++)
			{
				identifierHeight += GetAddedHeight(GetHeight(identifier.GetArrayElementAtIndex(i)));
			}
			totalHeight += identifierHeight;
			//add branches label
			totalHeight = totalHeight + GetAddedHeight(lineHeight);

			//add first branch
			totalHeight = totalHeight + GetAddedHeight(firstBranchesList.GetHeight());
			//add second branch
			totalHeight = totalHeight + GetAddedHeight(secondBranchesList.GetHeight());
			//add final line height for padding
			totalHeight = totalHeight + GetAddedHeight(lineHeight);
			return totalHeight;
		}
	}
}