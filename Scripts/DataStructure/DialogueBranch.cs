using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class DialougeBranch : NestedBaseData<Line>, ISettable<string, int>, IDefault<ConversationDefualts, int>
	{
		[SerializeField] private string serializedUniqueIDPrefix = "convBranch";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convBranch"; return serializedUniqueIDPrefix; } }

		public DialougeBranch(Line[] sentArray, string sentName = null, int sentID = -1) : base(sentArray)
		{
			Set(sentName, sentID);
		}
		public DialougeBranch(Line sentData, string sentName = null, int sentID = -1) : base(sentData)
		{
			Set(sentName, sentID);
		}

		public override DataItem[] GetArray()
		{
			return dataArray;
		}

		protected override void SetArray(Line[] sentData)
		{
			dataArray = sentData;
		}

		public void Set(string sentItem1, int sentItem2)
		{
			name = sentItem1;
			id = sentItem2;
		}

		public void SetDefault(ConversationDefualts sentItem1, int sentItem2)
		{
			id = sentItem2;
			for (int i = 0; i < dataArray.Length; i++)
			{
				if (dataArray[i].GetIsConditionMet())
				{
					Reply[] tempReplies;
					if (i < dataArray.Length - 1) { tempReplies = GetDefaultReply(sentItem1); }
					else
					{
						string tagType = dataArray[i].GetItem().GetEnumValue().ToString();
						if (tagType == "ChangeBranch" || tagType == "ChangeStage")
						{
							tempReplies = GetDefaultReply(sentItem1);
						}
						else
						{
							tempReplies = GetDefaultFinalReply(sentItem1);
						}
					}
					dataArray[i].SetDefault(tempReplies, i);
				}
			}
		}

		private Reply[] GetDefaultReply(ConversationDefualts sentDefaults)
		{
			return new Reply[] { (new Reply(sentDefaults.continueText, 0)) };
		}

		private Reply[] GetDefaultFinalReply(ConversationDefualts sentDefaults)
		{
			ReplyTag tempTag = new ReplyTag(ReplyTag.TagType.End);
			return new Reply[] { new Reply(sentDefaults.endText, 0, tempTag) };
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetChildUniqueIDs(dataArray);
			tempList.Add(uniqueID);
			return tempList;
		}

		public override void SetUniqueID(IProvider<string, string, string> sentProvider)
		{
			uniqueID = sentProvider.GetItem(uniqueID, uniqueIDPrefix);
			SetChildUnqueIDs(sentProvider);
		}

		public override string GetEndLabelText()
		{
			string unitText = "Lines";
			if (GetArray().Length == 1) { unitText = "Line"; }
			return "[" + GetArray().Length + " " + unitText + "]";
		}

		public override void SetAsNew()
		{
			name = "New DialogueBranch";
			dataArray = new Line[0];
			uniqueID = null;
		}
	}
}