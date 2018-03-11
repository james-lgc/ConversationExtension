using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using DSA.Extensions.Conversations.DataStructure;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(ConversationDefualts))]
	//Overrides how Reply.Tag class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	//Inherits from BasePropertyDrawer to access custom defaults
	public class ConversationDefaultsDrawer : BasePropertyDrawer
	{
		private static SerializedProperty continueText;
		private static SerializedProperty endText;

		//ensures all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			continueText = sentProperty.FindPropertyRelative("continueText");
			endText = sentProperty.FindPropertyRelative("endText");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//ensure all properties set
			SetProperties(property);

			//draw defaults label
			Rect newPostion = DrawTopLabel(position, "Defaults");
			//draw continue text
			newPostion = DrawTextField(newPostion, continueText, "Reply");
			//draw end text
			newPostion = DrawTextField(newPostion, endText, "Last Reply");
		}

		//Calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float totalHeight = initialVerticalPaddingHeight;
			//label
			totalHeight += GetAddedHeight(lineHeight);
			//continue text
			totalHeight += GetAddedHeight(lineHeight);
			//end text
			totalHeight += GetAddedHeight(lineHeight);
			return totalHeight;
		}
	}
}