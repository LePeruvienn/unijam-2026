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

    Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        transform.position = p1HeadPos;
        transform.rotation = Quaternion.Euler(p1HeadRot);
    }
    public void ChangeHeadPos(bool player1to2){
        StartCoroutine(ChangeHeadPosCoroutine(player1to2));
    }

    IEnumerator ChangeHeadPosCoroutine(bool player1to2){

        float tempVar = 0f;
        while(tempVar <= 1f){
            if(player1to2){
                cam.transform.position = Vector3.Lerp(Vector3.Lerp(p1HeadPos, intermediaryHeadPos, tempVar), p2HeadPos, Mathf.Pow(tempVar, 1.5f));
                cam.transform.rotation = Quaternion.Euler(Vector3.Lerp(p1HeadRot, p2HeadRot, tempVar));
            }else{
                cam.transform.position = Vector3.Lerp(Vector3.Lerp(p2HeadPos, intermediaryHeadPos, tempVar), p1HeadPos, Mathf.Pow(tempVar, 1.5f));
                cam.transform.rotation = Quaternion.Euler(Vector3.Lerp(p2HeadRot, p1HeadRot, tempVar));
            }
            tempVar += Time.deltaTime * 1f;
            yield return null;
        }
        if(player1to2){
            cam.transform.position = p2HeadPos;
            cam.transform.rotation = Quaternion.Euler(p2HeadRot);
        }else{
            cam.transform.position = p1HeadPos;
            cam.transform.rotation = Quaternion.Euler(p1HeadRot);
        }
    }
}
