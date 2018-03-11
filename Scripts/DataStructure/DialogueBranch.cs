using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations.DataStructure
{
	[System.Serializable]
	public class DialougeBranch : NestedBaseData<Line>, ISettable<string, int>, IDefault<ConversationDefualts, int>
	{
		[SerializeField] private string name;
		public override string Text { get { return name; } }
		[HideInInspector] [SerializeField] private int id;
		public override int ID { get { return id; } }

		[SerializeField] private string serializedUniqueIDPrefix = "convBranch";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convBranch"; return serializedUniqueIDPrefix; } }

		[SerializeField] private Line[] lines;

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
			return lines;
		}

		protected override void SetArray(Line[] sentData)
		{
			lines = sentData;
		}

		public void Set(string sentItem1, int sentItem2)
		{
			name = sentItem1;
			id = sentItem2;
		}

		public void SetDefault(ConversationDefualts sentItem1, int sentItem2)
		{
			id = sentItem2;
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].GetIsConditionMet())
				{
					Reply[] tempReplies;
					if (i < lines.Length - 1) { tempReplies = GetDefaultReply(sentItem1); }
					else
					{
						string tagType = lines[i].GetItem().GetEnumValue().ToString();
						if (tagType == "ChangeBranch" || tagType == "ChangeStage")
						{
							tempReplies = GetDefaultReply(sentItem1);
						}
						else
						{
							tempReplies = GetDefaultFinalReply(sentItem1);
						}
					}
					lines[i].SetDefault(tempReplies, i);
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
			List<string> tempList = GetChildUniqueIDs(lines);
			tempList.Add(uniqueID);
			return tempList;
		}

		public override void SetUniqueID(IProvider<string, string, string> sentProvider)
		{
			uniqueID = sentProvider.GetItem(uniqueID, uniqueIDPrefix);
			SetChildUnqueIDs(sentProvider);
		}
	}
}