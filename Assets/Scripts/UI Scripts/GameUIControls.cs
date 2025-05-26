using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameUIControls : MonoBehaviour
{

    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private TMP_Text textCooldown;
    [SerializeField]
    private GameObject player;
    private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        updateDashUI();
    }

    private void updateDashUI()
    {
        if (playerMovement.getDashCharges() < 2)
        {
            imageCooldown.fillAmount = playerMovement.getDashTimer() / playerMovement.getDashCooldown();
        }
        else
        {
            imageCooldown.fillAmount = 0f;
        }

        textCooldown.text = playerMovement.getDashCharges().ToString();
    }
}
