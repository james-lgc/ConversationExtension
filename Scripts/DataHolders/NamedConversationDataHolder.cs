using UnityEngine;
using DSA.Extensions.Conversations.DataStructure;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[CreateAssetMenu(fileName = "NamedConversationDataHolder", menuName = "DataHolder/Conversation/NamedConversationDataHolder")]
	[System.Serializable]
	//Intermediary between high and low level conversation classes as IDataHoldable
	//The single-extension conversation provider
	//takes a string and returns a conversation from the manager
	public class NamedConversationDataHolder : ConversationDataHolder, IDataHoldable<string, Conversation>
	{
		public override Conversation GetData(string sentInfo)
		{
			if (!GetIsExtensionLoaded() || GetDataFunc == null) { return null; }
			return GetDataFunc(sentInfo);
		}
	}
}