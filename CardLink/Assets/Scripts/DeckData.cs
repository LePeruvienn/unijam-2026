
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardEntry
{
	public Card card;
	[Min(1)] public int quantity = 1;
}

[CreateAssetMenu(fileName = "DeckData", menuName = "Cards/Deck Data")]
public class DeckData : ScriptableObject
{
	public List<CardEntry> cards = new List<CardEntry>();
}

