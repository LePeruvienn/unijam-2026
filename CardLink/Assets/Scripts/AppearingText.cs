using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AppearingText : MonoBehaviour
{
    public float speed = 1;
    public Image image;
    public Text textMess;
    public string message;
    [SerializeField] private float duration;
    public float alpIm;
    public float alpText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        alpIm = image.color.a;
        alpText = textMess.color.a;
        image.enabled=false;
        textMess.enabled=false;
        textMess.text = message; 
        StartCoroutine(ImageFadeInAndOut()); 
        StartCoroutine(TextFadeInAndOut());
       
        

    }
    
    
    IEnumerator ImageFadeInAndOut()
    {
        image.color= new Color(image.color.r, image.color.g, image.color.b, 0);
        image.enabled = true;
        alpIm = image.color.a;
        while (alpIm < 1 )
        {
            alpIm+=Time.deltaTime*speed;
            image.color= new Color(image.color.r, image.color.g, image.color.b, alpIm);
            yield return null;
            
        }
        yield return new WaitForSeconds(duration);
        
        while (alpIm > 0 )
        {
            alpIm-=Time.deltaTime*speed;
            image.color= new Color(image.color.r, image.color.g, image.color.b, alpIm);
            yield return null;
            
        }
        Destroy(gameObject);
        
    }
    IEnumerator TextFadeInAndOut()
    {
        textMess.color= new Color(textMess.color.r, textMess.color.g, textMess.color.b, 0);
        textMess.enabled = true;
        alpText = textMess.color.a;
        while (alpText < 1 )
        {
            alpText+=Time.deltaTime*speed;
            textMess.color= new Color(textMess.color.r, textMess.color.g, textMess.color.b, alpText);
            yield return null;
            
        }
        yield return new WaitForSeconds(duration);
        
        while (alpText > 0 )
        {
            alpText-=Time.deltaTime*speed;
            textMess.color= new Color(textMess.color.r, textMess.color.g, textMess.color.b, alpText);
            yield return null;
            
        }
        Destroy(gameObject);
        
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
