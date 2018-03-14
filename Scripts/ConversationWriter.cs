using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DSA.Extensions.Conversations;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[CreateAssetMenu(menuName = "Writers/ConversationWriter", fileName = "ConversationWriter")]
	[System.Serializable]
	//Edits the Conversation Json file in the inspector.
	public class ConversationWriter : JsonWriter<ConversationList>, IProvider<ConversationList>
	{
		[HideInInspector] [SerializeField] private ConversationList conversationList;
		public ConversationList Conversations { get { return conversationList; } }

		public override void Process()
		{
			conversationList.SetDefault();
			conversationList.SetUniqueID(this);
			uniqueIDs = conversationList.GetUniqueIDs();
			uniqueIDs.Sort();
			WriteToJson(conversationList);
		}

		public ConversationList GetItem()
		{
			return conversationList;
		}

		private void Awake()
		{
			conversationList = default(ConversationList);
		}

		public override void Set()
		{
			conversationList = base.ReadTFromJson();
		}
	}
}
