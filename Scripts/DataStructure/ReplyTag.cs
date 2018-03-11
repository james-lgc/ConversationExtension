using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DSA.Extensions.Base;

[System.Serializable]
public class ReplyTag : IOrderable, IProvider<int>, IEnum
{
	public enum TagType { None, End, ChangeStage, ChangeBranch };
	[SerializeField] private TagType tagType;
	public int ID { get { return (int)tagType; } }
	[SerializeField] private int id;

	public ReplyTag(int sentTagType = 0, int sentID = 0)
	{
		tagType = (TagType)sentTagType;
		id = sentID;
	}

	public ReplyTag(TagType sentTagType, int sentID = 0)
	{
		tagType = sentTagType;
		id = sentID;
	}

	public int GetItem()
	{
		return id;
	}

	public Enum GetEnumValue()
	{
		return tagType;
	}
}
