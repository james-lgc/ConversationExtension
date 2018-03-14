
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DSA.Extensions.Conversations;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class ConversationManager : CanvasedManagerBase<ConversationCanvas>
	{
		public override ExtensionEnum.Extension Extension { get { return ExtensionEnum.Extension.Conversation; } }

		[SerializeField] ConversationWriter writer;

		[SerializeField] private ConversationDataHolder conversationDataHolder;

		private ConversationList hardConversationList;
		private Conversation conversation;
		[SerializeField] [HideInInspector] private Dictionary<string, List<int[]>> spokenToDictionary;
		public Dictionary<string, List<int[]>> SpokenToDictionary { get { if (spokenToDictionary == null) { spokenToDictionary = new Dictionary<string, List<int[]>>(); } return spokenToDictionary; } }

		public override void Initialize()
		{
			base.Initialize();
			conversationDataHolder.GetDataFunc = GetConversationByName;
			if (spokenToDictionary == null) spokenToDictionary = new Dictionary<string, List<int[]>>();
		}

		public override void LoadAtGameStart()
		{
			base.Load();
			hardConversationList = writer.ReadTFromJson();
			base.LoadAtGameStart();
		}

		public override void PassDelegatesToTraits(TraitedMonoBehaviour sentObj)
		{
			SetTraitActions<ConversationTrait, Conversation>(sentObj, ReceiveConversation);
			SetTraitActions<NamedConversationTrait, Conversation>(sentObj, ReceiveConversation);
			SetDataHolder<NamedConversationTrait, string, Conversation>(sentObj, conversationDataHolder);
		}

		public void ReceiveConversation(Conversation sentConversation)
		{
			conversation = sentConversation;
			canvas.SetConversation(sentConversation);
			QueueProecess();
		}

		public Conversation GetConversationByName(string sentName)
		{
			Conversation conv = null;
			hardConversationList.GetDictionary().TryGetValue(sentName, out conv);
			return conv;
		}

		protected override void StartProcess()
		{
			base.StartProcess();
			Conversation originalConversation = hardConversationList.GetDictionary()[conversation.Text];
			Conversation filteredConversation = SpokenToHandler.GetSpokenTo(conversation, originalConversation, SpokenToDictionary);
			Debug.Log("Conversation Entered: " + conversation.Text);
			canvas.DisplayData();
		}

		public override void EndProcess()
		{
			SpokenToHandler.SetSpokenToDictionary(conversation, SpokenToDictionary);
			base.EndProcess();
		}

		public override void AddDataToArrayList(ArrayList sentArrayList)
		{
			ConversationProgress progress = new ConversationProgress(spokenToDictionary);
			sentArrayList.Add(progress);
		}

		public override void ProcessArrayList(ArrayList sentArrayList)
		{
			for (int i = 0; i < sentArrayList.Count; i++)
			{
				if (sentArrayList[i] is ConversationProgress)
				{
					ConversationProgress progress = (ConversationProgress)sentArrayList[i];
					spokenToDictionary = progress.SpokenToDictionary;
				}
			}
		}
	}
}
