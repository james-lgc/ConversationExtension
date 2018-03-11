using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DSA.Extensions.Conversations.DataStructure;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(Reply))]
	//Overrides how Reply class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	//Inherits from BasePropertyDrawer to access custom defaults
	public class ReplyDrawer : DataItemDrawer
	{
		private SerializedProperty name;
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
			newPosition = DrawTextField(newPosition, name, "Text");
			//Draw tag
			newPosition = DrawTag(newPosition);
		}

		protected Rect DrawTag(Rect position)
		{
			//Draw tagType
			Rect newPosition = DrawPropertyField(position, tagType);
			//add indent
			newPosition = GetIndentedPosition(newPosition);
			//Draw necissary fields based on tag type
			switch (tagType.intValue)
			{
				case 2:
					//Draw id
					newPosition = DrawIntField(newPosition, id, "ID");
					break;
				case 3:
					//Draw id
					newPosition = DrawIntField(newPosition, id, "ID");
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
			float totalHeight = initialVerticalPaddingHeight;
			//add label
			totalHeight += GetAddedHeight(lineHeight);
			//add unique id
			totalHeight += GetAddedHeight(lineHeight);
			//Add Text height
			totalHeight += GetAddedHeight(GetHeight(name));
			//Add tag height
			totalHeight += GetAddedHeight(GetTagHeight(property));
			//add padding
			totalHeight += GetAddedHeight(lineHeight);
			return totalHeight;
		}

		//Calculate the height of this property
		protected float GetTagHeight(SerializedProperty property)
		{
			//Ensure all properties are set
			SetProperties(property);
			float totalHeight = 0F;
			// add tag type
			totalHeight += GetAddedHeight(GetHeight(tagType));
			switch (tagType.intValue)
			{
				case 2:
					//add text height
					totalHeight += GetAddedHeight(lineHeight);
					break;
				case 3:
					//add text height
					totalHeight += GetAddedHeight(lineHeight);
					break;
			}
			return totalHeight;
		}
	}
}
