using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class LineTag : DataItem, IProvider<InstructionData[]>, IEnum
	{
		public enum TagType { None, SpeakerName, TopicQuestion, ChangeBranch, ChangeStage };
		[SerializeField] private TagType tagType;

		[SerializeField] private string serializedUniqueIDPrefix = "convLineTag";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convLineTag"; return serializedUniqueIDPrefix; } }

		[SerializeField] private InstructionData[] instructions;

		public LineTag(int sentTagType = 0, string sentText = "", InstructionData[] sentInstructions = null)
		{
			tagType = (TagType)sentTagType;
			name = sentText;
			instructions = sentInstructions;
		}

		public LineTag(TagType sentTagType = TagType.None, string sentText = "", InstructionData[] sentInstructions = null)
		{
			tagType = sentTagType;
			name = sentText;
			instructions = sentInstructions;
		}

		public InstructionData[] GetItem()
		{
			return instructions;
		}

		public Enum GetEnumValue()
		{
			return tagType;
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
			throw new NotImplementedException();
		}

		public override string GetEndLabelText()
		{
			throw new NotImplementedException();
		}

		public override void SetAsNew()
		{
			name = "New LineTag";
			tagType = TagType.None;
			uniqueID = null;
			instructions = null;
			id = 0;
		}
	}
}