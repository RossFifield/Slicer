using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpControl : MonoBehaviour
{
    public GameObject player;

    private PlayerController playerControl;
    private Slider slide;
    // Start is called before the first frame update
    void Start()
    {
        playerControl = player.GetComponent<PlayerController>();
        slide = GetComponent<Slider>();
        slide.maxValue = GameController.GetInstance().currentHP;
    }

    // Update is called once per frame
    void Update()
    {
        slide.value=playerControl.health;
    }
}
