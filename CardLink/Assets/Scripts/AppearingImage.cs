using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AppearingImage : MonoBehaviour
{
    public float speed = 1;
    public Image image;
    [SerializeField] private float duration;
    public float alp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        alp = image.color.a;
        image.enabled=false;
        StartCoroutine(ImageFadeInAndOut()); 
     
        
    }
    
    
    IEnumerator ImageFadeInAndOut()
    {
        image.color= new Color(image.color.r, image.color.g, image.color.b, 0);
        image.enabled = true;
       //float alpha = image.color.a;
       alp = image.color.a;
        while (alp < 1 )
        {
            alp+=Time.deltaTime*speed;
            image.color= new Color(image.color.r, image.color.g, image.color.b, alp);
            yield return null;
            
        }
        yield return new WaitForSeconds(duration);
        
        while (alp > 0 )
        {
            alp-=Time.deltaTime*speed;
            image.color= new Color(image.color.r, image.color.g, image.color.b, alp);
            yield return null;
            
        }
        Destroy(gameObject);
        
    }
    IEnumerator ImageFadeOut()
    {
        
        while (alp > 0 )
        {
            alp-=Time.deltaTime*speed;
            image.color= new Color(image.color.r, image.color.g, image.color.b, alp);
            yield return null;
            
        }
        Destroy(gameObject,duration);
        
    }
    
  
    
}

