using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AppearingText : MonoBehaviour
{
    public float speed = 1;
    public Image image;
    public Text textMess;
    public string message;
    public float maxDuration=3;
    [SerializeField] private float duration;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        image.enabled=false;
        textMess.enabled=false;
        textMess.text = message; 
        StartCoroutine(ImageFadeIn()); 
        StartCoroutine(TextFadeIn());
       
        

    }
    

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, duration);
        
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
            //image.color=new Color(255, 255, 255, alpha);
            yield return null;
            
        }
        
    }
    IEnumerator TextFadeIn()
    {
        textMess.color= new Color(textMess.color.r, textMess.color.g, textMess.color.b, 0);
        textMess.enabled = true;
        float alpha = textMess.color.a;
        while (alpha < 1 )
        {
            alpha+=Time.deltaTime*speed;
            textMess.color= new Color(textMess.color.r, textMess.color.g, textMess.color.b, alpha);
            yield return null;
            
        }
        
    }

  
    
}
