using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{
    [SerializeField] GameObject playPannel;

    // Start is called before the first frame update
    void Start()
    {
        playPannel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void playLocal()
    {
        Application.LoadLevel(1);
    }
    public void playNetwork()
    {
        Application.LoadLevel(1);
    }
    public void gotoMainMenu()
    {
        Application.LoadLevel(0);
    }
    public void activatePlayPannel()
    {
        playPannel.SetActive(true);
    }

}
