using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;  

public class InputScript : MonoBehaviour
{
    [SerializeField] bool persoPlay; //temp
    [SerializeField] int cardNumber; //temp
    [SerializeField] Transform[] p1cards;

    Camera cam;

    bool mouseClick; //selectionner
    bool mouseClickSt; //premiere frame ou mouseclick

    Vector2 mousePosScreen; //position de la souris, en pixels
    Vector2 mousePos; //position de la souris, entre (-x et x) et (0 et 1)

    Vector2[] cardPos;
    int selectedCard;
    
    [SerializeField] float distanceForwardToCamera;
    [SerializeField] float distanceBelowToHideFromCamera;
    [SerializeField] float posBelowPutDown;
    [SerializeField] float posBelowPutUp;
    [SerializeField] float posBelowPutSelected;
    [SerializeField] float distanceBetweenCards;
    [SerializeField] float screenProportionToPlay;

    [SerializeField] BoardTerrain BoardTerrain;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        mouseClick = false;
        mouseClickSt = false;
        mousePosScreen = Vector2.zero;
        selectedCard = -1;

        cardPos = new Vector2[3];
        for (int i = 0; i < 3; i++)
        {
            cardPos[i] = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(persoPlay){
            for (int i = 0; i < 3; i++)
            {
                cardPos[i] = new Vector2(distanceBetweenCards * (i -1), posBelowPutDown);
            }
            if(mousePos.y <= screenProportionToPlay){
                int newSelectedCard = -1;
                if(mousePos.x > -0.4f && mousePos.x < -0.15f && cardNumber > 0){
                    newSelectedCard = 0;
                }
                if(mousePos.x > -0.15f && mousePos.x < 0.15f && cardNumber > 1){
                    newSelectedCard = 1;
                }
                if(mousePos.x > 0.15f && mousePos.x < 0.4f && cardNumber > 2){
                    newSelectedCard = 2;
                }
                if(newSelectedCard != -1){
                    cardPos[newSelectedCard] = new Vector2(distanceBetweenCards * (newSelectedCard -1), posBelowPutUp);
                    if(mouseClickSt){
                        if(selectedCard == newSelectedCard){
                            selectedCard = -1;
                        }else{
                            selectedCard = newSelectedCard;
                        }
                    }
                }
            }
            if(selectedCard != -1){
                cardPos[selectedCard] = new Vector2(distanceBetweenCards * (selectedCard -1), posBelowPutSelected);
            }


        }else{
            for (int i = 0; i < 3; i++)
            {
                cardPos[i] = -distanceBelowToHideFromCamera * Vector2.up;
            }
        }










        for(int i = 0; i < 3; i ++){
            p1cards[i].position = Vector3.Lerp(p1cards[i].position, cam.transform.rotation * new Vector3(cardPos[i].x, cardPos[i].y, distanceForwardToCamera) + cam.transform.position, 0.1f);
        }







        mouseClickSt = false;
    }

    public void SetCard(){
        Debug.Log("Set cards");
        //Temp
    }

    public void PlayCard(UnityEngine.InputSystem.InputAction.CallbackContext context){
        if(context.ReadValue<float>() == 0){ //temp
            return;
        }

        if(persoPlay && selectedCard != -1){
            BoardTerrain.AddNewCard(null, p1cards[selectedCard].position);
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
    
}
