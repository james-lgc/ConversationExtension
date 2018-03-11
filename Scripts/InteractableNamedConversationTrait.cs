using UnityEngine;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[AddComponentMenu("Trait/Conversation/(Interactable) Named Conversation Trait")]
	[System.Serializable]
	//Interactable concrete class to initiate conversation from string
	public class InteractableNamedConversationTrait : NamedConversationTrait, IInteractCallable
	{
		//Called from Traited objects Interact method
		public void CallInteract()
		{
			//call the base method to start a conversation
			InitiateConversation();
		}
	}
}