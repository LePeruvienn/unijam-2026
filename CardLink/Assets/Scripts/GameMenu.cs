using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneNamePlay;
    [SerializeField] private string gameSceneNameOptions;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        SceneManager.LoadScene(gameSceneNamePlay);
        
        
    }

    // Update is called once per frame
    public void Quit ()
    {
        Application.Quit();
    }
    public void Options()
    {
        SceneManager.LoadScene(gameSceneNameOptions);
    }
}
