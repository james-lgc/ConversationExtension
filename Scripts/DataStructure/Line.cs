using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class Line : NestedBaseData<Reply>, ISettable<string, int, bool, LineTag>, IProvider<LineTag>, IConditional, IDefault<Reply[], int>
	{
		[SerializeField] private LineTag tag;
		[SerializeField] private bool useDefaultReply;

		[SerializeField] private string serializedUniqueIDPrefix = "convLine";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convLine"; return serializedUniqueIDPrefix; } }

		public Line(Reply[] sentArray, string sentText = "", int sentID = -1, bool sentUseDefaultReply = true, LineTag sentTag = default(LineTag)) : base(sentArray)
		{
			Set(sentText, sentID, sentUseDefaultReply, tag);
		}

		public Line(Reply sentData, string sentText = "", int sentID = -1, bool sentUseDefaultReply = true, LineTag sentTag = default(LineTag)) : base(sentData)
		{
			Set(sentText, sentID, sentUseDefaultReply, tag);
		}

		public void Set(string sentItem1, int sentItem2, bool sentItem3, LineTag sentItem4)
		{
			name = sentItem1;
			id = sentItem2;
			useDefaultReply = sentItem3;
			tag = sentItem4;
		}

		public LineTag GetItem()
		{
			return tag;
		}

		public void SetDefault(Reply[] sentItem, int sentItem2)
		{
			id = sentItem2;
			SetArray(sentItem);
		}

		public bool GetIsConditionMet()
		{
			return useDefaultReply;
		}

		public void MeetCondition()
		{
			useDefaultReply = true;
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetChildUniqueIDs(dataArray);
			tempList.Add(uniqueID);
			tempList.Add(tag.UniqueID);
			return tempList;
		}

		public override void SetUniqueID(IProvider<string, string, string> sentProvider)
		{
			uniqueID = sentProvider.GetItem(uniqueID, uniqueIDPrefix);
			SetChildUnqueIDs(sentProvider);
			tag.SetUniqueID(sentProvider);
		}

		public override string GetEndLabelText()
		{
			string unitText = "Replies";
			if (dataArray.Length == 1) { unitText = "Reply"; }
			return "[" + dataArray.Length + " " + unitText + "]";
		}
		public override void SetAsNew()
		{
			name = "New Line";
			dataArray = new Reply[0];
			uniqueID = null;
			tag = new LineTag(LineTag.TagType.None);
		}
	}
}