using UnityEngine;
using System.Collections;
using DSA.Extensions.Conversations.DataStructure;
using System;

namespace DSA.Extensions.Conversations
{
	//Checks tag types for lines and replies.
	public class ConversationChecker
	{
		public static bool GetIsTagType(Enum tagValue, Enum sentTagType)
		{
			if (tagValue == null) { return false; }
			if (tagValue == sentTagType)
			{
				return true;
			}
			return false;
		}
	}
}