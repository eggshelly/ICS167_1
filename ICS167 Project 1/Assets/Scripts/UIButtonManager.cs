using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void playGame()
    {
        Application.LoadLevel(1);
    }
    public void gotoMainMenu()
    {
        Application.LoadLevel(0);
    }


}
