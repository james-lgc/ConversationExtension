using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Conversations.DataStructure;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[CreateAssetMenu(fileName = "StoriedConversationDataHolder", menuName = "DataHolder/Conversation/StoriedConversationDataHolder")]
	[System.Serializable]
	//Intermediary between high and low level conversation classes as IDataHoldable
	//Uses the story extension to provide a conversation
	//takes a string and returns a conversation filtered by story
	public class StoriedConversationDataHolder : ConversationDataHolder
	{
		//A story extension data holder which returns active and completed stories
		[SerializeField] protected ScriptableObject storyProgressDataHolder;

		public override Conversation GetData(string sentInfo)
		{
			if (!GetIsExtensionLoaded() || GetDataFunc == null) { return null; }
			//Get conversation from manager by sent name
			Conversation tempData = GetDataFunc(sentInfo);
			//Get current story progress from data holder
			if (storyProgressDataHolder == null || !(storyProgressDataHolder is IProvider<int[][], bool>))
			{
				Debug.Log("Incorrect story progress holder");
				return tempData;
			}
			IProvider<int[][], bool> identifierProvider = (IProvider<int[][], bool>)storyProgressDataHolder;
			int[][] activeIdentifiers = identifierProvider.GetItem(true);
			int[][] completedIdentifiers = identifierProvider.GetItem(false);
			//Get story-filtered conversation from Indexer
			Conversation newData = ConversationIndexer.GetIndexedConversation(tempData, activeIdentifiers, completedIdentifiers);
			return newData;
		}

		//seperate sub class to handle filtereing conversation by provider story progress
		private class ConversationIndexer
		{
			//Takes a conversation instance and returns a new instance containing only indexed stages.
			public static Conversation GetIndexedConversation(Conversation conv, int[][] activeIdentifiers, int[][] completedIdentifiers)
			{
				List<Stage> tempStages = GetActiveStages(conv, activeIdentifiers);
				Stage[] indexedStages;
				switch (tempStages.Count)
				{
					case 0:
						//If there are no active stages, uses the default stage.
						indexedStages = new Stage[1];
						indexedStages[0] = (Stage)conv.GetArray()[0];
						break;
					case 1:
						//Wraps the list item in an array.
						indexedStages = new Stage[1];
						indexedStages[0] = tempStages[0];
						break;
					default:
						//Sets an array of the stages with the first item as a topic question.
						indexedStages = new Stage[tempStages.Count + 1];
						indexedStages[0] = GetTopicStage(conv, tempStages);
						for (int i = 0; i < tempStages.Count; i++)
						{
							indexedStages[i + 1] = tempStages[i];
						}
						break;
				}
				//Create new conversation to avoid replacing stages in ConversationList
				Conversation indexedConversation = new Conversation(indexedStages);
				indexedConversation.Set(conv.Text, conv.ID, conv.PrintableText);
				return indexedConversation;
			}

			//Checks Conversation stages against the Active Story Indicies and returns list of active stages.
			private static List<Stage> GetActiveStages(Conversation conv, int[][] activeIdentifiers)
			{
				List<Stage> tempStages = new List<Stage>();
				for (int i = 0; i < activeIdentifiers.Length; i++)
				{
					Stage tempStage;
					if (conv.GetDictionary().TryGetValue(activeIdentifiers[i], out tempStage))
					{
						tempStages.Add(tempStage);
					}
				}
				return tempStages;
			}

			//Returns a topic stage with a topic question and replies for each available stage topic.
			private static Stage GetTopicStage(Conversation conv, List<Stage> tempStages)
			{
				//Create reply array of length 1 longer than topics length
				Reply[] topicReplies = new Reply[tempStages.Count + 1];
				for (int i = 0; i < tempStages.Count; i++)
				{
					//Create topic replies to select topic
					ReplyTag tempTag = new ReplyTag(ReplyTag.TagType.ChangeStage, i + 1);
					topicReplies[i] = new Reply(tempStages[i].Text, 0, tempTag);
				}

				//Create End Reply to exit conversation
				ReplyTag endTag = new ReplyTag(ReplyTag.TagType.End);
				topicReplies[tempStages.Count] = new Reply("End", 0, endTag);

				//Create Line and tag for topic question
				LineTag lineTag = new LineTag(LineTag.TagType.TopicQuestion);
				Line topicLine = new Line(topicReplies, conv.PrintableText, 0, false, lineTag);
				DialougeBranch topicBranch = new DialougeBranch(topicLine);
				Stage tempStage = new Stage(topicBranch);
				return tempStage;
			}

			//Returns the last stage discussed from the conversation.
			private static Stage GetLastStage(Conversation conv, Dictionary<string, List<int[]>> dict)
			{
				List<int[]> tempList;
				if (dict.TryGetValue(conv.Text, out tempList))
				{
					for (int i = 0; i < tempList.Count; i++)
					{
						Stage tempStage;
						if (conv.GetDictionary().TryGetValue(tempList[i], out tempStage))
						{
							return tempStage;
						}
					}
				}
				return null;
			}

			//Checks if any stages in use have been used in conversation already.  Calls "Use" on stage if true.
			private static void UseStages(Conversation conv, Dictionary<string, List<int[]>> dict)
			{
				List<int[]> tempList;
				if (dict.TryGetValue(conv.Text, out tempList))
				{
					foreach (Stage tempStage in conv.GetDictionary().Values)
					{
						if (tempList.Contains(tempStage.GetItem())) tempStage.MeetCondition();
					}
				}
			}
		}
	}
}