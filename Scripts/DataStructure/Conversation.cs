using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations.DataStructure
{
	[System.Serializable]
	public class Conversation : NestedBaseData<Stage>, ISettable<string, int, string>, IPrintable, ICatalogued<int[], Stage>, IDefault<ConversationDefualts, int>, IProvider<Stage>
	{
		[SerializeField] private string name;
		public override string Text { get { return name; } }
		[HideInInspector] [SerializeField] private int id;
		public override int ID { get { return id; } }
		[SerializeField] private string topicQuestion;
		public string PrintableText { get { return topicQuestion; } }

		[SerializeField] private string serializedUniqueIDPrefix = "conv";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "conv"; return serializedUniqueIDPrefix; } }

		[SerializeField] private Stage defaultStage;

		[SerializeField] private Stage[] stages;

		public Dictionary<int[], Stage> GetDictionary()
		{
			Dictionary<int[], Stage> stageDictionary = new Dictionary<int[], Stage>();
			for (int i = 0; i < stages.Length; i++)
			{
				stageDictionary.Add(stages[i].GetItem(), stages[i]);
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

		private void SetAsNew()
		{
			name = "NewConversation";
			stages = new Stage[0];
			defaultStage = null;
			uniqueID = null;
		}

		public override DataItem[] GetArray()
		{
			return stages;
		}

		protected override void SetArray(Stage[] sentData)
		{
			stages = sentData;
		}

		public void SetDefault(ConversationDefualts sentItem, int sentItem2)
		{
			id = sentItem2;
			for (int i = 0; i < stages.Length; i++)
			{
				stages[i].SetDefault(sentItem, i);
			}
		}

		public Stage GetItem()
		{
			return defaultStage;
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetChildUniqueIDs(stages);
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
