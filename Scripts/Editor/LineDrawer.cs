using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(Line))]
	//Overrides how Line class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	public class LineDrawer : BaseConversationDrawer
	{
		private UnityEditorInternal.ReorderableList repliesList;
		private SerializedProperty tag;
		private SerializedProperty useDefaultReply;

		private SerializedProperty tagType;
		private SerializedProperty tagName;
		private SerializedProperty storyInstruction;
		private SerializedProperty instructions;
		private UnityEditorInternal.ReorderableList instructionsList;


		bool isDetailExpaned;

		//ensures all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			//set line properties
			name = sentProperty.FindPropertyRelative("name");
			tag = sentProperty.FindPropertyRelative("tag");
			useDefaultReply = sentProperty.FindPropertyRelative("useDefaultReply");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			//set tag properties
			tagType = tag.FindPropertyRelative("tagType");
			tagName = tag.FindPropertyRelative("name");
			id = tag.FindPropertyRelative("id");
			storyInstruction = tag.FindPropertyRelative("storyInstruction");
			instructions = tag.FindPropertyRelative("instructions");
			System.Action<SerializedProperty> editAction = ConversationEditorWindow.Init;
			repliesList = GetDefaultEditButtonList(dataArray, "Replies");
			instructionsList = EditorTool.GetDefaultFullDisplayList(instructions, "Instructions");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//Draw name
			newPosition = EditorTool.DrawTextArea(newPosition, name, "Text");
			//tag header
			newPosition = EditorTool.DrawLabel(newPosition, "Tag");
			//tag
			newPosition = DrawTag(newPosition);
			//remove indent
			newPosition = EditorTool.GetIndentedPosition(newPosition, false);
			//replies header
			newPosition = EditorTool.DrawLabel(newPosition, "Replies");
			//Draw useDefaultReply
			newPosition = EditorTool.DrawBoolField(newPosition, useDefaultReply, "Use Default Replies");
			isDetailExpaned = useDefaultReply.boolValue;
			//Draw replies if not default
			if (isDetailExpaned == false)
			{
				newPosition = EditorTool.DrawReorderableList(newPosition, repliesList);
			}
		}

		private Rect DrawTag(Rect position)
		{
			//Draw tagType
			Rect newPosition = EditorTool.DrawPropertyField(position, tagType);
			float height = newPosition.height;
			string tagTypeName = tagType.enumNames[tagType.enumValueIndex];
			//Draw necissary fields based on tag type
			switch (tagTypeName)
			{
				case "SpeakerName":
					//Draw text
					newPosition = EditorTool.DrawTextField(newPosition, tagName, "Speaker Name");
					height += newPosition.height;
					break;
				case "ChangeBranch":
					newPosition = EditorTool.DrawIntField(newPosition, id, "ID");
					height += newPosition.height;
					break;
				case "ChangeStage":
					//draw id
					newPosition = EditorTool.DrawIntField(newPosition, id, "ID");
					height += newPosition.height;
					break;
			}
			newPosition = EditorTool.DrawReorderableList(newPosition, instructionsList, "Instructions");
			height += newPosition.height;
			newPosition = new Rect(newPosition.x, newPosition.y, newPosition.width, height);
			//EditorGUI.DrawRect(newPosition, Color.blue);
			return newPosition;
		}

		//Calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = EditorTool.InitialVerticalPadding;
			//Add Text header height
			totalHeight += EditorTool.AddedLineHeight;
			//Add unique ID
			totalHeight += EditorTool.AddedLineHeight;
			//Add Text height
			totalHeight += EditorTool.GetAddedHeight(EditorTool.GetHeight(name));
			//tag header
			totalHeight += EditorTool.AddedLineHeight;
			//Add tag height
			totalHeight += EditorTool.GetAddedHeight(GetTagHeight(property));
			//replies label
			totalHeight += EditorTool.AddedLineHeight;
			//Add default replies toggle height
			totalHeight += EditorTool.GetAddedHeight(EditorTool.GetHeight(useDefaultReply));
			if (isDetailExpaned == false)
			{
				//Add Replies height
				totalHeight += EditorTool.GetAddedHeight(repliesList.GetHeight());
			}
			//added padding
			totalHeight += EditorTool.AddedLineHeight;
			return totalHeight;
		}

		//Calculate the tag height
		private float GetTagHeight(SerializedProperty property)
		{
			float totalHeight = 0f;
			//add tagtype
			totalHeight += EditorTool.GetAddedHeight(EditorTool.GetHeight(tagType));
			string tagTypeName = tagType.enumNames[tagType.enumValueIndex];
			switch (tagTypeName)
			{
				case "SpeakerName":
					//add text height
					totalHeight += EditorTool.AddedLineHeight;
					break;
				case "ChangeBranch":
					//add id height
					totalHeight += EditorTool.AddedLineHeight;
					break;
				case "ChangeStage":
					//add id height
					totalHeight += EditorTool.AddedLineHeight;
					break;
			}
			//instructions label
			totalHeight += EditorTool.AddedLineHeight;
			//instructions
			totalHeight += EditorTool.GetAddedHeight(instructionsList.GetHeight());
			return totalHeight;
		}
	}
}