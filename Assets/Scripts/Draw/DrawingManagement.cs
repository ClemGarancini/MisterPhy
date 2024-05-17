using UnityEngine;

public class DrawingManagement : MonoBehaviour
{
    // Start is called before the first frame update
    private DrawEnergy drawEnergy;
    private DrawKinematic drawKinematic;
    private DrawWork drawWork;

    public GameObject energySquare;
    public GameObject kinematicArrow;

    public GameObject text;

    private PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        drawEnergy = new();
        drawEnergy.Initialize(energySquare, text);

        drawKinematic = new();
        drawKinematic.Initialize(kinematicArrow, text);

        drawWork = new();
        drawWork.Initialize(energySquare, text);

    }

    // Update is called once per frame
    void Update()
    {
        drawEnergy.Draw(playerController.kineticEnergy, playerController.gravitationalPotentialEnergy, playerController.totalEnergy);
        drawKinematic.Draw(playerController.velocity, playerController.acceleration);
        drawWork.Draw(playerController.gravityWork);


    }
}
