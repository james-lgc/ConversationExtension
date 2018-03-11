using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.DataStructure.Editor
{
	[CustomPropertyDrawer(typeof(Line))]
	//Overrides how Line class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	//Inherits from BasePropertyDrawer to access custom defaults
	public class LineDrawer : DataItemDrawer
	{
		private UnityEditorInternal.ReorderableList repliesList;
		private SerializedProperty tag;
		private SerializedProperty replies;
		private SerializedProperty useDefaultReply;

		SerializedProperty tagType;
		SerializedProperty text;
		SerializedProperty storyInstruction;

		bool isDetailExpaned;

		//ensures all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			//set line properties
			name = sentProperty.FindPropertyRelative("name");
			replies = sentProperty.FindPropertyRelative("replies");
			tag = sentProperty.FindPropertyRelative("tag");
			useDefaultReply = sentProperty.FindPropertyRelative("useDefaultReply");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			//set tag properties
			tagType = tag.FindPropertyRelative("tagType");
			text = tag.FindPropertyRelative("text");
			id = tag.FindPropertyRelative("id");
			storyInstruction = tag.FindPropertyRelative("storyInstruction");
			System.Action<SerializedProperty> editAction = ConversationEditorWindow.Init;
			repliesList = GetDefaultEditButtonList(replies, "Replies", editAction);
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//Draw name
			newPosition = DrawTextArea(newPosition, name, "Text");
			//tag header
			newPosition = DrawLabel(newPosition, "Tag");
			//tag
			newPosition = DrawTag(newPosition);
			//remove indent
			newPosition = GetIndentedPosition(newPosition, false);
			//replies header
			newPosition = DrawLabel(newPosition, "Replies");
			//Draw useDefaultReply
			newPosition = DrawBoolField(newPosition, useDefaultReply, "Use Default Replies");
			isDetailExpaned = useDefaultReply.boolValue;
			//Draw replies if not default
			if (isDetailExpaned == false)
			{
				newPosition = DrawReorderableList(newPosition, repliesList, "Replies");
			}
		}

		private Rect DrawTag(Rect position)
		{
			//Draw tagType
			Rect newPosition = DrawPropertyField(position, tagType);
			float height = newPosition.height;
			//Draw necissary fields based on tag type
			switch (tagType.intValue)
			{
				case 1:
					//Draw text
					newPosition = DrawTextField(newPosition, text, "Speaker Name");
					break;
				case 3:
					//Draw Story Instruction
					newPosition = DrawPropertyField(newPosition, storyInstruction, usePadding: false);
					break;
				case 4:
					//draw id
					newPosition = DrawIntField(newPosition, id, "ID");
					break;
				case 5:
					//draw id
					newPosition = DrawIntField(newPosition, id, "ID");
					break;
			}
			height += newPosition.height;
			newPosition = new Rect(newPosition.x, newPosition.y, newPosition.width, height);
			return newPosition;
		}

		//Calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = initialVerticalPaddingHeight;
			//Add Text header height
			totalHeight += GetAddedHeight(lineHeight);
			//Add unique ID
			totalHeight += GetAddedHeight(lineHeight);
			//Add Text height
			totalHeight += GetAddedHeight(GetHeight(name));
			//Add tag height
			totalHeight += GetAddedHeight(GetTagHeight(property));

			//Add default replies toggle height
			totalHeight += GetAddedHeight(GetHeight(useDefaultReply));
			if (isDetailExpaned == false)
			{
				//Add Replies height
				totalHeight += GetAddedHeight(repliesList.GetHeight());
			}
			return totalHeight;
		}

		//Calculate the tag height
		private float GetTagHeight(SerializedProperty property)
		{
			float totalHeight = 0f;
			//add tagtype
			totalHeight += GetAddedHeight(GetHeight(tagType));
			switch (tagType.intValue)
			{
				case 1:
					//add text height
					totalHeight += GetAddedHeight(lineHeight);
					break;
				case 3:
					//add storyInstruction height
					totalHeight += GetAddedHeight(GetHeight(storyInstruction));
					break;
				case 4:
					//add id height
					totalHeight += GetAddedHeight(lineHeight);
					break;
				case 5:
					//add id height
					totalHeight += GetAddedHeight(lineHeight);
					break;
			}
			return totalHeight;
		}
	}
}