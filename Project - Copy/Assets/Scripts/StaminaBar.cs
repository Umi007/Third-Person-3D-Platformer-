using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private float playersp = 100;
    public float playerCurrentsp;
    public Slider PlayerStaminaBar;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentsp = playersp;
    }

    // Update is called once per frame
    void Update()
    {
        playerCurrentsp += 0.04f;
        PlayerStaminaBar.value = playerCurrentsp;

        if (playerCurrentsp >= 100)
        {
            playerCurrentsp = 100;
        }
    }

    public void TakeStamina(float amount)
    {
            playerCurrentsp += amount;
            PlayerStaminaBar.value = playerCurrentsp;
    }
}
