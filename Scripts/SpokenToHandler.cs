using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Conversations.DataStructure;

namespace DSA.Extensions.Conversations
{
	public class SpokenToHandler
	{

		public static Conversation GetSpokenTo(Conversation sentConversation, Conversation originalConversation, Dictionary<string, List<int[]>> spokenToDictionary)
		{
			Conversation filteredConversation = sentConversation;
			if (sentConversation.GetArray().Length == 1)
			{
				Stage tempStage = GetLastStage(originalConversation, spokenToDictionary);
				if (tempStage != null)
				{
					Stage[] tempStageArray = new Stage[] { tempStage };
					filteredConversation = new Conversation(tempStageArray, sentConversation.Text, sentConversation.ID, sentConversation.PrintableText);
				}
			}
			UseStages(filteredConversation, spokenToDictionary);
			return filteredConversation;
		}

		//Returns the last stage discussed from the conversation.
		private static Stage GetLastStage(Conversation conv, Dictionary<string, List<int[]>> dict)
		{
			List<int[]> tempList;
			if (dict == null)
			{
				dict = new Dictionary<string, List<int[]>>();
				return null;
			}
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
			if (dict == null)
			{
				dict = new Dictionary<string, List<int[]>>();
				return;
			}
			if (dict.TryGetValue(conv.Text, out tempList))
			{
				foreach (Stage tempStage in conv.GetDictionary().Values)
				{
					if (tempList.Contains(tempStage.GetItem())) tempStage.MeetCondition();
				}
			}
		}

		public static void SetSpokenToDictionary(Conversation sentConversation, Dictionary<string, List<int[]>> spokenToDictionary)
		{
			for (int i = 0; i < sentConversation.GetArray().Length; i++)
			{
				Stage stage = (Stage)sentConversation.GetArray()[i];
				if (stage.GetIsConditionMet() == true)
				{
					List<int[]> tempList;
					if (!spokenToDictionary.TryGetValue(sentConversation.Text, out tempList))
					{
						tempList = new List<int[]>();
						spokenToDictionary.Add(sentConversation.Text, tempList);
					}
					tempList.Add(stage.GetItem());
				}
			}
		}

	}
}