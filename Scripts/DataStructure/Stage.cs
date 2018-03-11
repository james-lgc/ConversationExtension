using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DSA.Extensions.Base;

namespace
DSA.Extensions.Conversations.DataStructure
{
	[System.Serializable]
	public class Stage : NestedBaseData<DialougeBranch>, ISettable<string, int, bool>, IProvider<int[]>, IConditional, IDefault<ConversationDefualts, int>
	{
		[SerializeField] private string name;
		public override string Text { get { return name; } }
		[SerializeField] private int[] identifier;
		[HideInInspector] [SerializeField] private int id;
		public override int ID { get { return id; } }
		[HideInInspector] [SerializeField] private bool isUsed;
		public bool IsUsed { get { return isUsed; } }

		[SerializeField] private string serializedUniqueIDPrefix = "convStage";
		protected override string uniqueIDPrefix { get { serializedUniqueIDPrefix = "convStage"; return serializedUniqueIDPrefix; } }

		[Header("Branches")] [SerializeField] private DialougeBranch[] firstBranches;
		[SerializeField] private DialougeBranch[] secondBranches;
		public DialougeBranch[] GetBranches()
		{
			if (!isUsed)
			{
				return firstBranches;
			}
			return secondBranches;
		}

		public Stage(DialougeBranch[] sentArray, DialougeBranch[] sentSecondBranch = null, string sentTopic = null, int sentID = -1, bool sentIsUsed = false) : base(sentArray)
		{
			Set(sentTopic, sentID, sentIsUsed);
		}
		public Stage(DialougeBranch sentData, DialougeBranch[] sentSecondBranch = null, string sentTopic = null, int sentID = -1, bool sentIsUsed = false) : base(sentData)
		{
			Set(sentTopic, sentID, sentIsUsed);
		}

		public override DataItem[] GetArray()
		{
			if (!isUsed) { return firstBranches; }
			return secondBranches;
		}

		protected override void SetArray(DialougeBranch[] sentData)
		{
			firstBranches = sentData;
		}

		public void Set(string sentItem1 = null, int sentItem2 = -1, bool sentItem3 = false)
		{
			name = sentItem1;
			id = sentItem2;
			isUsed = sentItem3;
		}

		public bool GetIsConditionMet()
		{
			return isUsed;
		}

		public int[] GetItem()
		{
			return identifier;
		}

		public void SetDefault(ConversationDefualts sentItem1, int sentItem2)
		{
			id = sentItem2;
			SetDefaultBranches(firstBranches, sentItem1, 0);
			SetDefaultBranches(secondBranches, sentItem1, 1);
		}

		private void SetDefaultBranches(DialougeBranch[] sentBranches, ConversationDefualts sentItem, int sentItem2)
		{
			for (int i = 0; i < sentBranches.Length; i++)
			{
				sentBranches[i].SetDefault(sentItem, i);
			}
		}

		public void MeetCondition()
		{
			isUsed = true;
		}

		public override List<string> GetUniqueIDs()
		{
			List<string> tempList = GetChildUniqueIDs(firstBranches);
			tempList = tempList.Concat(GetChildUniqueIDs(secondBranches)).ToList();
			tempList.Add(uniqueID);
			return tempList;
		}

		protected List<string> GetBranchesIDs(DialougeBranch[] sentBranches)
		{
			List<string> childIDs = GetNewStringList();
			for (int i = 0; i < sentBranches.Length; i++)
			{
				childIDs.Add(sentBranches[i].UniqueID);
			}
			return childIDs;
		}

		public override void SetUniqueID(IProvider<string, string, string> sentProvider)
		{
			uniqueID = sentProvider.GetItem(uniqueID, uniqueIDPrefix);
			SetArrayUniqueIDs(firstBranches, sentProvider);
			SetArrayUniqueIDs(secondBranches, sentProvider);
		}
	}
}