﻿using System;
using DSA.Extensions.Conversations.DataStructure;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	//Base class for an Intermediary between high and low level conversation classes as IDataHoldable
	//Base class for an Intermediary between extensions
	//Takes a string and returns a conversation from the manager
	[System.Serializable]
	public abstract class ConversationDataHolder : ExtendedScriptableObject, IDataHoldable<string, Conversation>
	{
		public override ExtensionEnum.Extension Extension { get { return ExtensionEnum.Extension.Conversation; } }

		public Func<string, Conversation> GetDataFunc { protected get; set; }

		public abstract Conversation GetData(string sentInfo);
	}
}