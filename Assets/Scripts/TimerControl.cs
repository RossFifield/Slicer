using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerControl : MonoBehaviour
{
    public GameObject sceneController;
    private TMP_Text text;
    private GameController sceneControl;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        sceneControl = GameController.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Time: " + ((int)sceneControl.currentTime).ToString() + " s";
    }
}
