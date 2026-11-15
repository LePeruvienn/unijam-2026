using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeadMovement : MonoBehaviour
{

    [SerializeField] Vector3 p1HeadPos;
    [SerializeField] Vector3 p1HeadRot;
    [SerializeField] Vector3 p2HeadPos;
    [SerializeField] Vector3 p2HeadRot;
    [SerializeField] Vector3 intermediaryHeadPos;
    
    [SerializeField] Vector3 p1HeadPosCard;
    [SerializeField] Vector3 p1HeadRotCard;
    [SerializeField] Vector3 p2HeadPosCard;
    [SerializeField] Vector3 p2HeadRotCard;

    [SerializeField] float transitionCardDuration;
    [SerializeField] float transitionPlayerDuration;

    Vector3 posToBe;
    Vector3 posToReallyBe;

    Vector3 rotToBe;
    Vector3 rotToReallyBe;

    int isLooking;

    float howMuchCardLooking;

    public bool mustLook;

    Camera cam;

    private bool _isAtPlayer2 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        transform.position = p1HeadPos;
        transform.rotation = Quaternion.Euler(p1HeadRot);
        howMuchCardLooking = 0f;
        posToReallyBe = transform.position;
        rotToReallyBe = p1HeadRot;
        _isAtPlayer2 = false; // De base on est a P1
        isLooking = 1;
    }

	// NOTE: On comprend pas ce que veut dire le nom de la variable ????
	//       ca devrait etre un truc du genre :
	//       - toPlayer1 (on va au P1 si true et au P2 du coup si false)
	//       - un enum genre PlayerID (P1 et P2)
	//       Sinon la on regarde le code on comprend pas
    public void ChangeHeadPos(bool player1to2){

        if (player1to2 == _isAtPlayer2)
        {
            return;
        }

        _isAtPlayer2 = player1to2;

        StartCoroutine(ChangeHeadPosCoroutine(player1to2));
    }

    IEnumerator ChangeHeadPosCoroutine(bool player1to2){
        isLooking = 0;
        float tempVar = 0f;
        while(tempVar <= 1f){
            if(player1to2){
                posToReallyBe = Vector3.Lerp(Vector3.Lerp(GetHeadOriPos(true), intermediaryHeadPos, tempVar), GetHeadOriPos(false), Mathf.Pow(tempVar, 1.5f));
                rotToReallyBe = Vector3.Lerp(GetHeadOriRot(true), GetHeadOriRot(false), tempVar);
            }else{
                posToReallyBe = Vector3.Lerp(Vector3.Lerp(GetHeadOriPos(false), intermediaryHeadPos, tempVar), GetHeadOriPos(true), Mathf.Pow(tempVar, 1.5f));
                rotToReallyBe = Vector3.Lerp(GetHeadOriRot(false), GetHeadOriRot(true), tempVar);
            }
            tempVar += Time.deltaTime / transitionPlayerDuration;
            yield return null;
        }
        if(player1to2){
            posToReallyBe = GetHeadOriPos(true);
            rotToReallyBe = GetHeadOriRot(false);
        }else{
            posToReallyBe = GetHeadOriPos(false);
            rotToReallyBe = GetHeadOriRot(true);
        }
        isLooking = player1to2 ? 2 : 1;
    }

    Vector3 GetHeadOriPos(bool whichPlayer){
        if(whichPlayer){
            return Vector3.Lerp(p1HeadPos, p1HeadPosCard, howMuchCardLooking);
        }else{
            return Vector3.Lerp(p2HeadPos, p2HeadPosCard, howMuchCardLooking);
        }
    }
    Vector3 GetHeadOriRot(bool whichPlayer){
        if(whichPlayer){
            return Vector3.Lerp(p1HeadRot, p1HeadRotCard, howMuchCardLooking);
        }else{
            return Vector3.Lerp(p2HeadRot, p2HeadRotCard, howMuchCardLooking);
        }
    }


    //appele dans le update de inputscript
    public void LookAtCard(bool atCard){
        if(!atCard){
            howMuchCardLooking = Mathf.Max(howMuchCardLooking -= Time.deltaTime/transitionCardDuration, 0f);
        }else{
            howMuchCardLooking = Mathf.Min(howMuchCardLooking += Time.deltaTime/transitionCardDuration, 1f);
        }
    }

    void Update(){
        if(isLooking == 1){
            posToReallyBe = GetHeadOriPos(true);
            rotToReallyBe = GetHeadOriRot(true);
        } else if(isLooking == 2){
            posToReallyBe = GetHeadOriPos(false);
            rotToReallyBe = GetHeadOriRot(false);
        }
        transform.position = posToReallyBe;
        transform.rotation = Quaternion.Euler(rotToReallyBe);
    }

}
