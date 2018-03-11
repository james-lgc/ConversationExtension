using UnityEngine;
using UnityEditor;
using System.Reflection;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.DataStructure.Editor
{
	[CustomPropertyDrawer(typeof(ConversationList))]
	//Overrides how ConversationList class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	//Inherits from BasePropertyDrawer to access custom defaults
	public class ConversationListDrawer : DataItemDrawer
	{
		private SerializedProperty conversations;
		private UnityEditorInternal.ReorderableList reorderableList;
		private SerializedProperty conversationDefaults;

		private SerializedProperty defaultRepy;
		private SerializedProperty defaultEndReply;

		//Ensure all properties are set
		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			//Find child properties in this property
			conversationDefaults = sentProperty.FindPropertyRelative("conversationDefaults");
			conversations = sentProperty.FindPropertyRelative("conversations");
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			defaultRepy = conversationDefaults.FindPropertyRelative("continueText");
			defaultEndReply = conversationDefaults.FindPropertyRelative("endText");
			//create action for edit button in list
			//action opens property in conversation window
			System.Action<SerializedProperty> editAction = DSA.Extensions.Conversations.DataStructure.Editor.ConversationEditorWindow.Init;
			//method to return a string showing number of child elements in list item
			System.Func<SerializedProperty, string> endTextFunc = (SerializedProperty arrayProperty) =>
			{
				return GetArrayCountString(arrayProperty, "stages", "Stage", "Stages");
			};
			//action overrides copied values and generates unique id
			System.Action<UnityEditorInternal.ReorderableList> addAction = (UnityEditorInternal.ReorderableList list) =>
			{
				int index = list.serializedProperty.arraySize;
				OnAddElement(list);
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("stages").arraySize = 0;
			};
			//create list
			reorderableList = GetDefaultEditButtonList(conversations, "Conversations", editAction, endTextFunc, addAction);
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw defaults
			newPosition = DrawTextField(newPosition, defaultRepy, "Default Reply");
			newPosition = DrawTextField(newPosition, defaultEndReply, "Default End Reply");
			//draw conversations
			newPosition = DrawReorderableList(newPosition, reorderableList, "Conversations");
		}

		//calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SetProperties(property);
			//additional start padding
			float totalHeight = initialVerticalPaddingHeight;
			//unique id
			totalHeight += GetAddedHeight(lineHeight);
			//defaults
			totalHeight += GetAddedHeight(lineHeight);
			totalHeight += GetAddedHeight(lineHeight);
			//conversations label
			totalHeight += GetAddedHeight(lineHeight);
			//conversations
			totalHeight += GetAddedHeight(reorderableList.GetHeight());
			//end padding
			totalHeight += GetAddedHeight(lineHeight);
			return totalHeight;
		}
	}
}