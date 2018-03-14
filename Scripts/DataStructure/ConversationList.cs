using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class ConversationList : NestedBaseData<Conversation>, ICatalogued<string, Conversation>, IDefault
	{
		[SerializeField] private ConversationDefualts conversationDefaults;
		private Dictionary<string, Conversation> dictionary;

		[SerializeField] private string serializedUniqueIDPrefix = "convList";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convList"; return serializedUniqueIDPrefix; } }

		public Dictionary<string, Conversation> GetDictionary()
		{
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, Conversation>();
				for (int i = 0; i < dataArray.Length; i++)
				{
					dictionary.Add(dataArray[i].Text, dataArray[i]);
				}
			}
			return dictionary;
		}

		public void SetDefault()
		{
			id = -1;
			name = "Conversation List";
			for (int i = 0; i < dataArray.Length; i++)
			{
				dataArray[i].SetDefault(conversationDefaults, i);
			}
			List<string> uniqueIDs = GetUniqueIDs();
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetChildUniqueIDs<Conversation>(dataArray.ToList());
			tempList.Add(uniqueID);
			return tempList;
		}

		public override void SetUniqueID(IProvider<string, string, string> sentProvider)
		{
			uniqueID = sentProvider.GetItem(uniqueID, uniqueIDPrefix);
			SetChildUnqueIDs(sentProvider);
		}

		public override string GetEndLabelText()
		{
			string unitText = "Conversations";
			if (dataArray.Length == 1) { unitText = "Conversation"; }
			return "[" + dataArray.Length + " " + unitText + "]";
		}

		public override void SetAsNew()
		{
			name = "New Conversation List";
			dataArray = new Conversation[0];
			uniqueID = null;
		}
	}
}
