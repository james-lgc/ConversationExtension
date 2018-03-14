using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DSA.Extensions.Conversations;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(Reply))]
	//Overrides how Reply class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	public class ReplyDrawer : BaseConversationDrawer
	{
		private SerializedProperty tag;

		private SerializedProperty tagType;

		//ensures all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			name = sentProperty.FindPropertyRelative("name");
			tag = sentProperty.FindPropertyRelative("tag");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");

			tagType = tag.FindPropertyRelative("tagType");
			id = tag.FindPropertyRelative("id");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//Draw name
			newPosition = EditorTool.DrawTextField(newPosition, name, "Text");
			//Draw tag
			newPosition = DrawTag(newPosition);
		}

		protected Rect DrawTag(Rect position)
		{
			//Draw tagType
			Rect newPosition = EditorTool.DrawPropertyField(position, tagType);
			//add indent
			newPosition = EditorTool.GetIndentedPosition(newPosition);
			//Draw necissary fields based on tag type
			switch (tagType.intValue)
			{
				case 2:
					//Draw id
					newPosition = EditorTool.DrawIntField(newPosition, id, "ID");
					break;
				case 3:
					//Draw id
					newPosition = EditorTool.DrawIntField(newPosition, id, "ID");
					break;
			}
			return newPosition;
		}

		//Calculate the tag height
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Ensure all properties are set
			SetProperties(property);
			//add initial padding
			float totalHeight = EditorTool.InitialVerticalPadding;
			//add label
			totalHeight += EditorTool.AddedLineHeight;
			//add unique id
			totalHeight += EditorTool.AddedLineHeight;
			//Add Text height
			totalHeight += EditorTool.GetAddedHeight(EditorTool.GetHeight(name));
			//Add tag height
			totalHeight += EditorTool.GetAddedHeight(GetTagHeight(property));
			//add padding
			totalHeight += EditorTool.AddedLineHeight;
			return totalHeight;
		}

		//Calculate the height of this property
		protected float GetTagHeight(SerializedProperty property)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = 0F;
			// add tag type
			totalHeight += EditorTool.GetAddedHeight(EditorTool.GetHeight(tagType));
			switch (tagType.intValue)
			{
				case 2:
					//add text height
					totalHeight += EditorTool.AddedLineHeight;
					break;
				case 3:
					//add text height
					totalHeight += EditorTool.AddedLineHeight;
					break;
			}
			return totalHeight;
		}
	}
}
