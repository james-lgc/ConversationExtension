using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DSA.Extensions.Conversations.DataStructure;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	//The displayable UI for Conversations.
	public class ConversationCanvas : ClickableCanvas
	{
		public override ExtensionEnum.Extension Extension { get { return ExtensionEnum.Extension.Conversation; } }
		private Conversation conversation;
		private ConversationProgressTracker progressTracker;

		public override void Initialize()
		{
			base.Initialize();
			SetSendActions<DataItem>(ReceiveReply, 1);
			IReceivable<StoryInstruction>[] storyTraits = GetReceivableArray<StoryInstruction>();
			for (int i = 0; i < storyTraits.Length; i++)
			{
				storyTraits[i].ReceiveFunction = GetLineInstruction;
			}
		}

		public void SetConversation(Conversation sentConversation)
		{
			conversation = sentConversation;
		}

		protected override void Show()
		{
			base.Show();
			progressTracker = new ConversationProgressTracker();
			progressTracker.activeBranchIndex = 0;
			progressTracker.useSelectedIndex = false;
			progressTracker.activeStageIndex = 0;
			progressTracker.iCounter = 0;
		}

		protected override void Hide()
		{
			base.Hide();
		}

		public override void DisplayData()
		{
			Clear();
			Stage activeStage = (Stage)conversation.GetArray()[progressTracker.activeStageIndex];
			DialougeBranch activeBranch = (DialougeBranch)activeStage.GetArray()[progressTracker.activeBranchIndex];
			Line[] lines = activeBranch.GetArray() as Line[];
			progressTracker.currentLine = lines[progressTracker.iCounter];
			DataItem[] data = new DataItem[2];
			data[0] = conversation;
			data[1] = progressTracker.currentLine;
			WriteText(data, progressTracker.currentLine.GetArray());
			CallInSequence(0);
			SelectButton();
		}

		//Writes data to displayable UI Elements.
		public void WriteText(DataItem[] lineData, DataItem[] replyData)
		{
			panels[0].SetData<DataItem>(lineData);
			base.SetDisplayableArrayData<DataItem>(replyData, 1);
		}

		private void SelectButton()
		{
			if (progressTracker.useSelectedIndex == true)
			{
				SetSelectedDisplayable(1, progressTracker.selectedIndex);
				progressTracker.useSelectedIndex = false;
			}
			else
			{
				SetSelectedDisplayable(1, 0);
			}
		}

		public void ReceiveReply(DataItem sentData)
		{
			progressTracker.useSelectedIndex = false;
			if (!(sentData is Reply)) { return; }
			Reply reply = (Reply)sentData;
			ProcessLine(reply);
			ProcessReply(reply);
		}

		private void ProcessLine(Reply reply)
		{
			if (progressTracker.currentLine.GetItem() == null) { return; }
			if (ConversationChecker.GetIsTagType(progressTracker.currentLine.GetItem().GetEnumValue(), LineTag.TagType.TopicQuestion))
			{
				if (ConversationChecker.GetIsTagType(reply.GetItem().GetEnumValue(), ReplyTag.TagType.End))
				{
					EndConversation();
					return;
				}
				for (int i = 0; i < progressTracker.currentLine.GetArray().Length; i++)
				{
					if (reply == progressTracker.currentLine.GetArray()[i])
					{
						progressTracker.selectedIndex = i;
						break;
					}
				}
			}
		}

		private void ProcessReply(Reply reply)
		{
			progressTracker.activeReply = reply;
			progressTracker.iCounter++;
			if (reply.GetItem().GetEnumValue().ToString() != "None")
			{
				switch (reply.GetItem().GetEnumValue().ToString())
				{
					case "End":
						if (conversation.GetArray().Length <= 1)
						{
							IConditional stage = (IConditional)conversation.GetArray()[progressTracker.activeStageIndex];
							stage.MeetCondition();
							EndConversation();
							return;
						}
						progressTracker.activeStageIndex = 0;
						progressTracker.iCounter = 0;
						progressTracker.useSelectedIndex = true;
						break;
					case "ChangeStage":
						progressTracker.activeStageIndex = reply.GetItem().ID;
						progressTracker.iCounter = 0;
						break;
					case "ChangeBranch":
						progressTracker.activeBranchIndex = reply.GetItem().ID;
						progressTracker.iCounter = 0;
						break;
				}
			}
			DisplayData();
		}

		public StoryInstruction GetLineInstruction()
		{
			if (progressTracker.currentLine.GetItem().GetEnumValue().ToString() == "StoryInstruct")
			{
				return progressTracker.currentLine.GetItem().GetItem();
			}
			return default(StoryInstruction);
		}

		public void EndConversation()
		{
			conversation = null;
			EndAction();
		}
	}
}