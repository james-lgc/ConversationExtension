using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class Reply : DataItem, IProvider<ReplyTag>
	{
		[SerializeField] private ReplyTag tag;

		[SerializeField] private string serializedUniqueIDPrefix = "convReply";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convReply"; return serializedUniqueIDPrefix; } }

		public Reply(string sentText, int sentID = -1, ReplyTag sentTag = default(ReplyTag))
		{
			name = sentText;
			id = sentID;
			tag = sentTag;
		}

		public ReplyTag GetItem()
		{
			return tag;
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetNewStringList();
			tempList.Add(uniqueID);
			return tempList;
		}

		public override void SetUniqueID(IProvider<string, string, string> sentProvider)
		{
			uniqueID = sentProvider.GetItem(uniqueID, uniqueIDPrefix);
		}

		public override string GetLabelText()
		{
			return name;
		}

		public override string GetEndLabelText()
		{
			return null;
		}

		public override void SetAsNew()
		{
			name = "New Reply";
			tag = new ReplyTag(ReplyTag.TagType.None);
			uniqueID = null;
		}

	}
}