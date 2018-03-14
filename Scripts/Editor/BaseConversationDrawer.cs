using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Base.Editor;
using UnityEditor;

namespace DSA.Extensions.Conversations.Editor
{
	public abstract class BaseConversationDrawer : DataItemDrawer
	{
		protected SerializedProperty dataArray;

		protected override System.Action<SerializedProperty> editAction
		{
			get
			{
				System.Action<SerializedProperty> tempAction = (SerializedProperty sentProperty) =>
				{
					DSA.Extensions.Conversations.Editor.ConversationEditorWindow.Init(sentProperty);
				};
				return tempAction;
			}
		}

		protected override void SetProperties(SerializedProperty sentProperty)
		{
			if (GetIsCurrentProperty(sentProperty)) { return; }
			base.SetProperties(sentProperty);
			try { dataArray = sentProperty.FindPropertyRelative("dataArray"); }
			catch (System.Exception e) { Debug.Log("DataArray property not found.\n" + e.ToString()); }
		}
	}
}