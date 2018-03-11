using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DSA.Extensions.Base;

namespace DSA.Extensions.Conversations.DataStructure
{
	[System.Serializable]
	public class LineTag : DataItem, IProvider<StoryInstruction>, IEnum
	{
		public enum TagType { None, SpeakerName, TopicQuestion, StoryInstruct, ChangeBranch, ChangeStage };
		[SerializeField] private TagType tagType;
		[SerializeField] private int id;
		public override int ID { get { return ID; } }
		[SerializeField] private string text;
		public override string Text { get { return text; } }

		[SerializeField] private string serializedUniqueIDPrefix = "convLineTag";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convLineTag"; return serializedUniqueIDPrefix; } }

		[SerializeField] private StoryInstruction storyInstruction;

		public LineTag(int sentTagType = 0, string sentText = "", StoryInstruction sentInstruction = default(StoryInstruction))
		{
			tagType = (TagType)sentTagType;
			text = sentText;
			storyInstruction = sentInstruction;
		}

		public LineTag(TagType sentTagType = TagType.None, string sentText = "", StoryInstruction sentInstruction = default(StoryInstruction))
		{
			tagType = sentTagType;
			text = sentText;
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
	}
}