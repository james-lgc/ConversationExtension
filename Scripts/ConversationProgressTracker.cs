using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSA.Extensions.Conversations;
using DSA.Extensions.Conversations.DataStructure;

[System.Serializable]
public class ConversationProgressTracker
{
	public int activeStageIndex;
	public Line currentLine;
	public Reply activeReply;
	public int activeBranchIndex;
	public int iCounter;
	public int selectedIndex;
	public bool useSelectedIndex;
}
