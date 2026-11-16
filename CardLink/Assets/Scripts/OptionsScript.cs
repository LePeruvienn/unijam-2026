using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{

    [SerializeField] private string gameSceneNameReturn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Return()
    {
        SceneManager.LoadScene(gameSceneNameReturn);
        
        
    }
}
