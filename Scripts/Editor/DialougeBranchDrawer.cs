using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DSA.Extensions.Conversations.DataStructure;
using System;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(DialougeBranch))]
	//Overrides how DialougeBranch class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	//Inherits from BasePropertyDrawer to access custom defaults
	public class DialougeBranchDrawer : DataItemDrawer
	{
		private UnityEditorInternal.ReorderableList linesList;
		private SerializedProperty lines;

		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			lines = sentProperty.FindPropertyRelative("lines");
			id = sentProperty.FindPropertyRelative("id");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			//create action for edit button in list
			//action opens property in conversation window
			System.Action<SerializedProperty> editAction = DSA.Extensions.Conversations.DataStructure.Editor.ConversationEditorWindow.Init;
			//method to return a string showing number of child elements in list item
			System.Func<SerializedProperty, string> endTextFunc = (SerializedProperty arrayProperty) =>
			{
				return GetArrayCountString(arrayProperty, "replies", "Reply", "Replies");
			};
			//action overrides copied values and generates unique id
			System.Action<UnityEditorInternal.ReorderableList> addAction = (UnityEditorInternal.ReorderableList list) =>
			{
				int index = list.serializedProperty.arraySize;
				OnAddElement(list);
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("replies").arraySize = 0;
			};
			//create list
			linesList = GetDefaultEditButtonList(lines, "Lines", editAction, GetReplyInfo, addAction);
		}

		protected string GetReplyInfo(SerializedProperty sentLine)
		{
			SerializedProperty useDefaultReply = sentLine.FindPropertyRelative("useDefaultReply");
			if (useDefaultReply.boolValue) { return "[Default]"; }
			return GetArrayCountString(sentLine, "replies", "Reply", "Replies");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw name field
			newPosition = DrawTextField(newPosition, name, "Name");
			//draw lines
			newPosition = DrawReorderableList(newPosition, linesList, "Lines");
		}

		//Calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = initialVerticalPaddingHeight;
			//name label
			totalHeight += GetAddedHeight(lineHeight);
			//unique id
			totalHeight += GetAddedHeight(lineHeight);
			//name field
			totalHeight += GetAddedHeight(lineHeight);
			//lines label
			totalHeight += GetAddedHeight(lineHeight);
			//lines
			totalHeight += GetAddedHeight(linesList.GetHeight());
			return totalHeight;
		}
	}
}