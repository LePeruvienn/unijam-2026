using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;  

public class InputScript : MonoBehaviour
{
    [SerializeField] bool player1Playing; //temp
    [SerializeField] bool persoAddCard; //temp

    [SerializeField] List<Transform> p1cards; //temp

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        mouseClick = false;
        mouseClickSt = false;
        mousePosScreen = Vector2.zero;
        selectedCard = -1;

        cardPos = new List<Vector2>();
        for (int i = 0; i < p1cards.Count; i++)
        {
            cardPos.Add(-distanceBelowToHideFromCamera * Vector2.up);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(player1Playing && persoAddCard){

            for (int i = 0; i < cardPos.Count; i++)
            {
                cardPos[i] = new Vector2(distanceBetweenCards * (i -1), posBelowPutDown);
            }

            if(mousePos.y <= screenProportionToPlay){
                int newSelectedCard = -1;
                if(mousePos.x > -0.4f && mousePos.x < -0.15f && cardPos.Count > 0){
                    newSelectedCard = 0;
                }
                if(mousePos.x > -0.15f && mousePos.x < 0.15f && cardPos.Count > 1){
                    newSelectedCard = 1;
                }
                if(mousePos.x > 0.15f && mousePos.x < 0.4f && cardPos.Count > 2){
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
            for (int i = 0; i < cardPos.Count; i++)
            {
                cardPos[i] = -distanceBelowToHideFromCamera * Vector2.up;
            }
        }










        for(int i = 0; i < p1cards.Count; i ++){
            p1cards[i].position = Vector3.Lerp(p1cards[i].position, cam.transform.rotation * new Vector3(cardPos[i].x, cardPos[i].y, distanceForwardToCamera) + cam.transform.position, 0.1f);
        }







        mouseClickSt = false;
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
