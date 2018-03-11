using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations.DataStructure
{
	[System.Serializable]
	public class ConversationList : NestedBaseData<Conversation>, ICatalogued<string, Conversation>, IDefault
	{
		[SerializeField] private string name;
		[SerializeField] private int id;
		[SerializeField] private ConversationDefualts conversationDefaults;
		[SerializeField] private string continueText;
		[SerializeField] private string endText;
		[SerializeField] private List<Conversation> conversations;
		private Dictionary<string, Conversation> dictionary;

		public override int ID { get { return id; } }

		public override string Text { get { return name; } }

		[SerializeField] private string serializedUniqueIDPrefix = "convList";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convList"; return serializedUniqueIDPrefix; } }

		public Dictionary<string, Conversation> GetDictionary()
		{
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, Conversation>();
				for (int i = 0; i < conversations.Count; i++)
				{
					dictionary.Add(conversations[i].Text, conversations[i]);
				}
			}
			return dictionary;
		}

		public void SetDefault()
		{
			id = -1;
			name = "Conversation List";
			for (int i = 0; i < conversations.Count; i++)
			{
				conversations[i].SetDefault(conversationDefaults, i);
			}
			List<string> uniqueIDs = GetUniqueIDs();
		}

		public override DataItem[] GetArray()
		{
			return conversations.ToArray();
		}

		protected override void SetArray(Conversation[] sentData)
		{
			throw new NotImplementedException();
		}

		public ConversationList(Conversation[] sentArray) : base(sentArray)
		{
			conversations = sentArray.ToList();
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetChildUniqueIDs<Conversation>(conversations);
			tempList.Add(uniqueID);
			return tempList;
		}

		public override void SetUniqueID(IProvider<string, string, string> sentProvider)
		{
			uniqueID = sentProvider.GetItem(uniqueID, uniqueIDPrefix);
			SetChildUnqueIDs(sentProvider);
		}
	}
}
