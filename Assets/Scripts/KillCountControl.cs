using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCountControl : MonoBehaviour
{
    public GameObject sceneController;
    private TMP_Text text;
    private GameController sceneControl;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        sceneControl = sceneController.GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Kills: " + ((int)sceneControl.killCount).ToString();
    }
}
