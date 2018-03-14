using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class Conversation : NestedBaseData<Stage>, ISettable<string, int, string>, IPrintable, ICatalogued<int[], Stage>, IDefault<ConversationDefualts, int>
	{
		[SerializeField] private string topicQuestion;
		public string PrintableText { get { return topicQuestion; } }

		[SerializeField] private string serializedUniqueIDPrefix = "conv";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "conv"; return serializedUniqueIDPrefix; } }

		public Dictionary<int[], Stage> GetDictionary()
		{
			Dictionary<int[], Stage> stageDictionary = new Dictionary<int[], Stage>();
			for (int i = 0; i < dataArray.Length; i++)
			{
				stageDictionary.Add(dataArray[i].GetItem(), dataArray[i]);
			}
			return stageDictionary;
		}

		public Conversation(Stage[] sentArray, string sentName = null, int sentID = -1, string sentTopicQuestion = null) : base(sentArray)
		{
			Set(sentName, sentID, sentTopicQuestion);
		}

		public Conversation(Stage sentData, string sentName = null, int sentID = -1, string sentTopicQuestion = null) : base(sentData)
		{
			Set(sentName, sentID, sentTopicQuestion);
		}

		public Conversation() : base()
		{
			name = "New Conversation";
		}

		public void Set(string sentItem1, int sentItem2, string sentItem3)
		{
			name = sentItem1;
			id = sentItem2;
			topicQuestion = sentItem3;
		}

		public override DataItem[] GetArray()
		{
			return dataArray;
		}

		protected override void SetArray(Stage[] sentData)
		{
			dataArray = sentData;
		}

		public void SetDefault(ConversationDefualts sentItem, int sentItem2)
		{
			id = sentItem2;
			for (int i = 0; i < dataArray.Length; i++)
			{
				dataArray[i].SetDefault(sentItem, i);
			}
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetChildUniqueIDs(dataArray);
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
			string unitText = "Stages";
			if (GetArray().Length == 1) { unitText = "Stage"; }
			return "[" + GetArray().Length + " " + unitText + "]";
		}

		public override void SetAsNew()
		{
			name = "New Conversation";
			dataArray = new Stage[0];
			uniqueID = null;
		}
	}
}
