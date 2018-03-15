using UnityEngine;
using DSA.Extensions.Conversations;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	//abstract base trait to initiate a conversation by name
	public abstract class NamedConversationTrait : TraitBase, ISendable<Conversation>, IDataRetrievable<string, Conversation>
	{
		public override ExtensionEnum Extension { get { return ExtensionEnum.Conversation; } }

		//name sent to manager to retrieve conversation
		[SerializeField] private string conversorName;
		//DataHolder to communicate with manager
		public IDataHoldable<string, Conversation> DataHolder { protected get; set; }
		//method to initiate conversation
		public Action<Conversation> SendAction { get; set; }

		private Conversation data;

		protected void InitiateConversation()
		{
			if (!GetIsExtensionLoaded() || SendAction == null) { return; }
			//get conversation from data holder
			data = DataHolder.GetData(conversorName);
			if (data == null) { return; }
			//Initiate conversation
			SendAction(data);
		}
	}
}