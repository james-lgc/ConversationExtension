using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Conversations;
using UnityEditor;
using System;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(Stage))]
	//Overrides how Stage class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	public class StageDrawer : BaseConversationDrawer
	{
		private UnityEditorInternal.ReorderableList firstBranchesList;
		private UnityEditorInternal.ReorderableList secondBranchesList;
		private SerializedProperty topic;
		private SerializedProperty identifier;
		private SerializedProperty secondDataArray;

		//Ensure all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			//set child properties
			topic = sentProperty.FindPropertyRelative("name");
			secondDataArray = sentProperty.FindPropertyRelative("secondDataArray");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			identifier = sentProperty.FindPropertyRelative("identifier");
			//action overrides copied values and generates unique id
			System.Action<UnityEditorInternal.ReorderableList> addAction = (UnityEditorInternal.ReorderableList list) =>
			{
				int index = list.serializedProperty.arraySize;
				OnAddElement(list);
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("lines").arraySize = 0;
			};
			//create lists
			firstBranchesList = GetDefaultEditButtonList(dataArray, "First Branches");
			secondBranchesList = GetDefaultEditButtonList(secondDataArray, "Second Branches");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw name field
			newPosition = EditorTool.DrawTextField(newPosition, topic, "Topic");
			//draw identifier
			newPosition = EditorTool.DrawArray(newPosition, identifier, "Identifier");
			//newPosition = new Rect(newPosition.x, newPosition.y + lineHeight, newPosition.width, 50F);
			//draw branches
			newPosition = EditorTool.DrawReorderableList(newPosition, firstBranchesList, "Branches");
			newPosition = EditorTool.DrawReorderableList(newPosition, secondBranchesList, null);
		}

		//Calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = EditorTool.InitialVerticalPadding;
			//add topic label
			totalHeight = totalHeight + EditorTool.AddedLineHeight;
			//add topic field
			totalHeight = totalHeight + EditorTool.AddedLineHeight;
			//add identifier
			float identifierHeight = EditorTool.AddedLineHeight;
			for (int i = 0; i < identifier.arraySize; i++)
			{
				identifierHeight += EditorTool.GetAddedHeight(EditorTool.GetHeight(identifier.GetArrayElementAtIndex(i)));
			}
			totalHeight += identifierHeight;
			//add branches label
			totalHeight = totalHeight + EditorTool.AddedLineHeight;

			//add first branch
			totalHeight += EditorTool.GetAddedHeight(firstBranchesList.GetHeight());
			//add second branch
			totalHeight += EditorTool.GetAddedHeight(secondBranchesList.GetHeight());
			//add final line height for padding
			totalHeight = totalHeight + EditorTool.AddedLineHeight;
			return totalHeight;
		}
	}
}