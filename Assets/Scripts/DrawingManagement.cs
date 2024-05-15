using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingManagement : MonoBehaviour
{
    // Start is called before the first frame update
    private DrawEnergy drawEnergy;
    public GameObject energySquare;
    public GameObject text;

    private PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        drawEnergy = new();
        drawEnergy.Initialize(playerController.GetEnergy(), energySquare, text);

    }

    // Update is called once per frame
    void Update()
    {
        drawEnergy.Draw(playerController.kineticEnergy, playerController.gravitationalPotentialEnergy, playerController.totalEnergy);
    }
}
