using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;  

// NOTE: ça devrais s'appeler Hand Manager plutot ?
// Je vois ce script comme un Vue qui affiche des cartes, il ne devrais pas avoir d'autres resposabilité
public class InputScript : MonoBehaviour
{
	[SerializeField] private GameObject cardVisualPrefab;
	[SerializeField] private float rotationCorrection = 90f;
	
    public bool persoPlay;
    public bool player1;
    [SerializeField] int cardNumber; //temp

	// WARNING: C'est pas les cartes de J1 mais les carte acutellement afficher !!!
	// Rien à voir avec J1 ou J2!!!
    [SerializeField] List<Transform> p1cards = new List<Transform>();

	// Stockage interne des modèles de cartes pour chaque élément affiché
	private List<Card> p1CardModels = new List<Card>();

    Camera cam;

    bool mouseClick; //selectionner
    bool mouseClickSt; //premiere frame ou mouseclick

    Vector2 mousePosScreen; //position de la souris, en pixels
    Vector2 mousePos; //position de la souris, entre (-x et x) et (0 et 1)

    List<Vector2> cardPos = new List<Vector2>();
    int selectedCard;
    
    [SerializeField] float distanceForwardToCamera;
    [SerializeField] float distanceBelowToHideFromCamera;
    [SerializeField] float posBelowPutDown;
    [SerializeField] float posBelowPutUp;
    [SerializeField] float posBelowPutSelected;
    [SerializeField] float distanceBetweenCards;
    [SerializeField] float screenProportionToPlay;

    [SerializeField] BoardTerrain p1BoardTerrain;
    [SerializeField] BoardTerrain p2BoardTerrain;
    [SerializeField] TurnController turnController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        mouseClick = false;
        mouseClickSt = false;
        mousePosScreen = Vector2.zero;
        selectedCard = -1;

        // Synchronise la taille des positions selon la liste initiale
        UpdateCardPosListSize();
    }

    // Update is called once per frame
    void Update()
    {
        // Sécurité pour garder la liste des positions synchronisée si la main change
        if (cardPos.Count != p1cards.Count)
        {
            UpdateCardPosListSize();
        }

        int currentCount = p1cards.Count;

        if(persoPlay){
            for (int i = 0; i < currentCount; i++)
            {
                cardPos[i] = new Vector2(distanceBetweenCards * (i - (currentCount - 1) / 2f), posBelowPutDown);
            }
            if(mousePos.y <= screenProportionToPlay){
                int newSelectedCard = -1;
                if(mousePos.x > -0.4f && mousePos.x < -0.15f && currentCount > 0){
                    newSelectedCard = 0;
                }
                if(mousePos.x > -0.15f && mousePos.x < 0.15f && currentCount > 1){
                    newSelectedCard = 1;
                }
                if(mousePos.x > 0.15f && mousePos.x < 0.4f && currentCount > 2){
                    newSelectedCard = 2;
                }
                if(newSelectedCard != -1 && newSelectedCard < currentCount){
                    cardPos[newSelectedCard] = new Vector2(distanceBetweenCards * (newSelectedCard - (currentCount - 1) / 2f), posBelowPutUp);
                    if(mouseClickSt){
                        if(selectedCard == newSelectedCard){
                            selectedCard = -1;
                        }else{
                            selectedCard = newSelectedCard;
                        }
                    }
                }
            }
            if(selectedCard != -1 && selectedCard < currentCount){
                cardPos[selectedCard] = new Vector2(distanceBetweenCards * (selectedCard - (currentCount - 1) / 2f), posBelowPutSelected);
            }


        }else{
            for (int i = 0; i < currentCount; i++)
            {
                cardPos[i] = -distanceBelowToHideFromCamera * Vector2.up;
            }
        }










        for(int i = 0; i < p1cards.Count; i ++){
            if (p1cards[i] != null)
            {
                p1cards[i].position = Vector3.Lerp(p1cards[i].position, cam.transform.rotation * new Vector3(cardPos[i].x, cardPos[i].y, distanceForwardToCamera) + cam.transform.position, 0.1f);

				// AElazealdsqlkdjq c'est hciant
				Quaternion baseRotation = cam.transform.rotation;
				Quaternion flatCorrection = Quaternion.Euler(rotationCorrection, 0f, 0f);
				// Quaternion playerFlip = player1 ? Quaternion.identity : Quaternion.Euler(0f, 180f, 0f);
				Quaternion playerFlip = Quaternion.identity;
				Quaternion targetRotation = baseRotation * playerFlip * flatCorrection;
				p1cards[i].rotation = Quaternion.Slerp(p1cards[i].rotation, targetRotation, 0.1f);
            }
        }







        mouseClickSt = false;
    }

    // Petite méthode utilitaire pour garder les tailles de tableaux alignées proprement
    void UpdateCardPosListSize()
    {
        cardPos.Clear();
        for (int i = 0; i < p1cards.Count; i++)
        {
            cardPos.Add(Vector2.zero);
        }
        cardNumber = p1cards.Count; // Garde ton champ temp à jour
    }

    public void SetCards(List<Transform> newCards){
        selectedCard = -1;
        p1cards = new List<Transform>(newCards);
        UpdateCardPosListSize();
        Debug.Log("Set cards");
    }

    // Nouvelle méthode SetCards prenant en compte une liste de modèles (Card)
    public void SetCards(List<Card> newCardModels)
    {
        selectedCard = -1;

        // Nettoyage des anciennes vues instanciées dans la scène
        foreach (Transform cardTransform in p1cards)
        {
            if (cardTransform != null)
            {
                Destroy(cardTransform.gameObject);
            }
        }
        p1cards.Clear();
        p1CardModels.Clear();

        if (newCardModels == null) return;

        // Instanciation et initialisation des nouveaux visuels
        foreach (Card cardModel in newCardModels)
        {
            if (cardModel != null && cardVisualPrefab != null)
            {
                GameObject newCardObj = Instantiate(cardVisualPrefab, transform);
                CardVisual cardVisual = newCardObj.GetComponent<CardVisual>();
                if (cardVisual != null)
                {
                    cardVisual.SetFromModel(cardModel);
                }
                p1cards.Add(newCardObj.transform);
                p1CardModels.Add(cardModel);
            }
        }

        UpdateCardPosListSize();
        Debug.Log("Set cards from List<Card>");
    }

    public void PlayCard(UnityEngine.InputSystem.InputAction.CallbackContext context){
        if(context.ReadValue<float>() == 0){ //temp
            return;
        }
        
        if(persoPlay && selectedCard != -1){
            persoPlay = false;
            if(player1){
                p1BoardTerrain.AddNewCard(null, p1cards[selectedCard].position);
            }else{
                p2BoardTerrain.AddNewCard(null, p1cards[selectedCard].position);
            }
            turnController.Next();
        }
    }

    public void AttackCard(){
        if((player1 && p1BoardTerrain.ApplySelection()) || (!player1 && p2BoardTerrain.ApplySelection())){
            turnController.Next();
        }
    }

    public void MouseMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        mousePosScreen = context.ReadValue<Vector2>();
        mousePos = (mousePosScreen - 0.5f * Screen.width * Vector2.right) / Screen.height;
    }
    public void MouseClick(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        mouseClick = context.ReadValue<float>() == 1;
        mouseClickSt = context.ReadValue<float>() == 1;
    }

    public BoardTerrain GetCurrentBoardTerrain()
    {
        return (player1) ? p1BoardTerrain : p2BoardTerrain;
    }

    public int GetSelectedCard()
    {
        return selectedCard;
    }

    public Vector3 GetCardPosition(int index)
    {
        if (index < 0 || index >= p1cards.Count)
        {
            Debug.LogError("FIIIN ... Frérot tu es cringe");
            return Vector3.zero;
        }

        return p1cards[index].position;
    }

    public Card GetCardModel(int index)
    {
        if (index < 0 || index >= p1CardModels.Count)
        {
            Debug.LogError("Index de carte invalide pour GetCardAtIndex !");
            return null;
        }
        return p1CardModels[index];
    }

	public void AddCard(Card cardModel)
	{
		if (cardModel == null || cardVisualPrefab == null)
		{
			Debug.LogError("Card Visual Prefab or model is null");
			return;
		}

		GameObject newCardObj = Instantiate(cardVisualPrefab, transform);
		
		CardVisual cardVisual = newCardObj.GetComponent<CardVisual>();
		if (cardVisual != null)
		{
			cardVisual.SetFromModel(cardModel);
		}

		p1cards.Add(newCardObj.transform);
		p1CardModels.Add(cardModel);
		UpdateCardPosListSize();
	}

	public void RemoveCard(int index)
	{
		if (index >= 0 && index < p1cards.Count)
		{
			if (p1cards[index] != null)
			{
				Destroy(p1cards[index].gameObject);
			}

			p1cards.RemoveAt(index);
			if (index < p1CardModels.Count)
			{
				p1CardModels.RemoveAt(index);
			}
			
			if (selectedCard == index)
			{
				selectedCard = -1;
			}
			else if (selectedCard > index)
			{
				selectedCard--;
			}

			UpdateCardPosListSize();
		}
		else
		{
			Debug.LogWarning("Index invalide pour supprimer la carte.");
		}
	}

	public int GetCardCount()
	{
		return p1cards.Count;
	}
}
