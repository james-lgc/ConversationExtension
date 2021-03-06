﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DSA.Extensions.Conversations;
using UnityEditor;
using DSA.Extensions.Base;

[RequireComponent(typeof(TraitedMonoBehaviour))]
[System.Serializable]
public class ConversationTrait : TraitBase, ISendable<Conversation>
{
	public override ExtensionEnum Extension { get { return ExtensionEnum.Conversation; } }

	public Action<Conversation> SendAction { get; set; }

	[SerializeField] private Conversation data;
	public Conversation Data { get { return data; } protected set { data = value; } }

	protected void Use()
	{
		if (!GetIsExtensionLoaded() || SendAction == null || Data == null) { return; }
		SendAction(Data);
	}
}
