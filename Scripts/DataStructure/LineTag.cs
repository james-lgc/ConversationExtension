using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations
{
	[System.Serializable]
	public class LineTag : DataItem, IProvider<StoryInstruction>, IEnum
	{
		public enum TagType { None, SpeakerName, TopicQuestion, StoryInstruct, ChangeBranch, ChangeStage };
		[SerializeField] private TagType tagType;

		[SerializeField] private string serializedUniqueIDPrefix = "convLineTag";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convLineTag"; return serializedUniqueIDPrefix; } }

		[SerializeField] private StoryInstruction storyInstruction;

		public LineTag(int sentTagType = 0, string sentText = "", StoryInstruction sentInstruction = default(StoryInstruction))
		{
			tagType = (TagType)sentTagType;
			name = sentText;
			storyInstruction = sentInstruction;
		}

		public LineTag(TagType sentTagType = TagType.None, string sentText = "", StoryInstruction sentInstruction = default(StoryInstruction))
		{
			tagType = sentTagType;
			name = sentText;
			storyInstruction = sentInstruction;
		}

		public StoryInstruction GetItem()
		{
			return storyInstruction;
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
			storyInstruction = default(StoryInstruction);
			id = 0;
		}
	}
}