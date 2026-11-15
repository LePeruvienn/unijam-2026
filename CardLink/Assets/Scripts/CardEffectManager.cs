using UnityEngine;
using System;

public class CardEffectManager : MonoBehaviour
{
	[SerializeField] private float voyanteTimeToSee = 2f;

	[SerializeField] private CardLinker carLinkder;
	[SerializeField] private CardSelector cardSelector;
	[SerializeField] private LinkRevealer linkRevealer;

	// Effects Handlers

	public void HandleOnPlayEffects(PlayedCard playedCard, Card cardModel, Action onEffectComplete)
	{
		if (cardModel == null)
		{
			Debug.LogError("Failed to get Card Model when handling effect");
			onEffectComplete?.Invoke();
			return;
		}

		// 1. Effet Amoureux
		if (cardModel.aAmoureux)
		{
			Debug.Log($"Effet déclenché : Amoureux pour {cardModel.nom}");
			carLinkder.StartSingleTargetLink(playedCard, true, true, () =>
			{
				onEffectComplete?.Invoke();
			});
			return;
		}

		// 2. Effet Cupidon
		if (cardModel.aCupidon)
		{
			Debug.Log($"Effet déclenché : Cupidon pour {cardModel.nom}");
			carLinkder.StartDoubleTargetLink(true, true, () =>
			{
				onEffectComplete?.Invoke();
			});
			return;
		}

		// 3. Effet Sorcière
		if(cardModel.aBriseLien)
		{
			Debug.Log($"Effet déclenché : Delete links pour {cardModel.nom}");
			cardSelector.StartSelection((PlayedCard selectedCard) => 
			{
				if(selectedCard != null)
					selectedCard.RemoveAllLinks();

				onEffectComplete?.Invoke();
			}, 2);
			return;
		}

		// 4. Effet Voyante
		if(cardModel.aVoyance)
		{
			Debug.Log($"Effet déclenché : Voyance pour {cardModel.nom}");
			cardSelector.StartSelection((PlayedCard selectedCard) => 
			{
				if(selectedCard != null)
				{
					linkRevealer.RevealLink(selectedCard, voyanteTimeToSee);
				}
				onEffectComplete?.Invoke();
			}, 2);
			return;
		}

		onEffectComplete?.Invoke();
	}

	public void HandleOnDeathEffects(PlayedCard playedCard, Action onEffectComplete)
	{
		Card cardModel = playedCard.GetModel();

		if (cardModel == null)
		{
			Debug.LogError("Failed to get Card Model when handling effect");
			onEffectComplete?.Invoke();
			return;
		}

		onEffectComplete?.Invoke();
	}
}
