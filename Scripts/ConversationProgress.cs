using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConversationProgress
{
	[SerializeField] private Dictionary<string, List<int[]>> spokenToDictionary;
	public Dictionary<string, List<int[]>> SpokenToDictionary { get { return spokenToDictionary; } }

	public ConversationProgress(Dictionary<string, List<int[]>> sentDict)
	{
		spokenToDictionary = sentDict;
	}
}
