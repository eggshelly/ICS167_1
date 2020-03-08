using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonManager : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int index)
    {
        Debug.Log(index);
        SceneManager.LoadScene(index);
    }

   public void playLocal()
    { 
        SceneManager.LoadScene(SceneManager.GetSceneByName("main").buildIndex);
    }
    public void playNetwork()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("Offline").buildIndex);
    }
    public void gotoMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetSceneByName("title").buildIndex);
    }

}
