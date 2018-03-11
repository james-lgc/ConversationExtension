using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DSA.Extensions.Conversations.DataStructure
{
	[System.Serializable]
	public struct ConversationDefualts
	{
		public string continueText;
		public string endText;

		public string conversationListPrefix;
		public string conversationPrefix;
		public string stagePrefix;
		public string branchPrefix;
		public string linePrefix;
		public string lineTagPrefix;
		public string replyPrefix;
	}
}