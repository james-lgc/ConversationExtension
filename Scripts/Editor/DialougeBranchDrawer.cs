using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DSA.Extensions.Conversations;
using System;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(DialougeBranch))]
	//Overrides how DialougeBranch class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	public class DialougeBranchDrawer : BaseConversationDrawer
	{
		private UnityEditorInternal.ReorderableList linesList;

		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			id = sentProperty.FindPropertyRelative("id");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			//action overrides copied values and generates unique id
			System.Action<UnityEditorInternal.ReorderableList> addAction = (UnityEditorInternal.ReorderableList list) =>
			{
				int index = list.serializedProperty.arraySize;
				OnAddElement(list);
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("replies").arraySize = 0;
			};
			//create list
			linesList = GetDefaultEditButtonList(dataArray, "Lines");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw name field
			newPosition = EditorTool.DrawTextField(newPosition, name, "Name");
			//draw lines
			newPosition = EditorTool.DrawReorderableList(newPosition, linesList, "Lines");
		}

		//Calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = EditorTool.InitialVerticalPadding;
			//name label
			totalHeight += EditorTool.AddedLineHeight;
			//unique id
			totalHeight += EditorTool.AddedLineHeight;
			//name field
			totalHeight += EditorTool.AddedLineHeight;
			//lines label
			totalHeight += EditorTool.AddedLineHeight;
			//lines
			totalHeight += EditorTool.GetAddedHeight(linesList.GetHeight());
			return totalHeight;
		}
	}
}