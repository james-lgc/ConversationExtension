using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConversationProgress
{
	[SerializeField] private Dictionary<string, List<StoryIndex>> spokenToDictionary;
	public Dictionary<string, List<StoryIndex>> SpokenToDictionary { get { return spokenToDictionary; } }

	public ConversationProgress(Dictionary<string, List<StoryIndex>> sentDict)
	{
		spokenToDictionary = sentDict;
	}
}
