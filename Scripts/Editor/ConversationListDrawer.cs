using UnityEngine;
using UnityEditor;
using System.Reflection;
using DSA.Extensions.Base.Editor;

namespace DSA.Extensions.Conversations.Editor
{
	[CustomPropertyDrawer(typeof(ConversationList))]
	//Overrides how ConversationList class is displayed in Unity Editor
	//Adds a reorderable list with edit buttons to show nested data
	public class ConversationListDrawer : BaseConversationDrawer
	{
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
			uniqueID = sentProperty.FindPropertyRelative("uniqueID");
			defaultRepy = conversationDefaults.FindPropertyRelative("continueText");
			defaultEndReply = conversationDefaults.FindPropertyRelative("endText");
			//action overrides copied values and generates unique id
			System.Action<UnityEditorInternal.ReorderableList> addAction = (UnityEditorInternal.ReorderableList list) =>
			{
				int index = list.serializedProperty.arraySize;
				OnAddElement(list);
				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				element.FindPropertyRelative("stages").arraySize = 0;
			};
			//create list
			reorderableList = GetDefaultEditButtonList(dataArray, "Conversations");
		}

		//called from base OnGUI, handles child property drawing
		protected override void DrawChildProperties(Rect position, SerializedProperty property)
		{
			//draw label
			Rect newPosition = DrawTopLabel(position);
			//draw unique id
			newPosition = DrawUniqueID(newPosition);
			//draw defaults
			newPosition = EditorTool.DrawTextField(newPosition, defaultRepy, "Default Reply");
			newPosition = EditorTool.DrawTextField(newPosition, defaultEndReply, "Default End Reply");
			//draw conversations
			newPosition = EditorTool.DrawReorderableList(newPosition, reorderableList, "Conversations");
		}

		//calculate the height of this property
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SetProperties(property);
			//additional start padding
			float totalHeight = EditorTool.InitialVerticalPadding;
			//unique id
			totalHeight += EditorTool.AddedLineHeight;
			//defaults
			totalHeight += EditorTool.AddedLineHeight;
			totalHeight += EditorTool.AddedLineHeight;
			//conversations label
			totalHeight += EditorTool.AddedLineHeight;
			//conversations
			totalHeight += EditorTool.GetAddedHeight(reorderableList.GetHeight());
			//end padding
			totalHeight += EditorTool.AddedLineHeight;
			return totalHeight;
		}
	}
}