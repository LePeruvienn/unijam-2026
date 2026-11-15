using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AppearingImage : MonoBehaviour
{
    public float speed = 1;
    public Image image;
    public float maxDuration=3;
    private float _duration = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        image.enabled=false;
        StartCoroutine(ImageFadeIn()); 
       
        

    }
    

    // Update is called once per frame
    void Update()
    {
        
        if (_duration < maxDuration)
        {
            _duration += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
            
        }
        
        
       
    }
    
    IEnumerator ImageFadeIn()
    {
        image.color= new Color(image.color.r, image.color.g, image.color.b, 0);
        image.enabled = true;
        float alpha = image.color.a;
        while (alpha < 1 )
        {
            alpha+=Time.deltaTime*speed;
            image.color= new Color(image.color.r, image.color.g, image.color.b, alpha);
            yield return null;
            
        }
        
    }
    
    

  
    
}

