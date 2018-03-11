using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[AddComponentMenu("Trait/Conversation/(Sequenced) Named Conversation Trait")]
	[System.Serializable]
	//Sequenceable concrete class to initiate conversation from string
	public class SequencedNamedConversationTrait : NamedConversationTrait, ISequenceable
	{
		//this objects order, defined in the Editor
		[SerializeField] private int sequenceOrder;
		public int SequenceOrder { get { return sequenceOrder; } }

		//Start a conversation if correct sequence stage is call
		public void CallInSequence(int sequenceID)
		{
			if (sequenceID != sequenceOrder) { return; }
			InitiateConversation();
		}
	}
}