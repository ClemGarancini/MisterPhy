using UnityEngine;

public class DrawingManagement : MonoBehaviour
{
    // Start is called before the first frame update
    private DrawEnergy drawEnergy;
    private DrawKinematic drawKinematic;
    private DrawWork drawWork;

    private DrawForce drawForce;
    public GameObject energySquare;
    public GameObject kinematicArrow;

    public Transform obj;
    public GameObject text;

    private PlayerController playerController;
    public GameObject pendule;

    private PendulumController pd;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        drawEnergy = new();
        drawEnergy.Initialize(energySquare, text);

        drawKinematic = new();
        drawKinematic.Initialize(kinematicArrow, text);

        drawWork = new();
        drawWork.Initialize(energySquare, text);

        drawForce = new();
        drawForce.Initialize(kinematicArrow, transform);

        //p = pendule.GetComponent<PendulumController>();

    }

    // Update is called once per frame
    void Update()
    {
        drawEnergy.Draw(playerController.kineticEnergy, playerController.gravitationalPotentialEnergy, playerController.totalEnergy);
        drawKinematic.Draw(playerController.velocity, playerController.acceleration);
        drawWork.Draw(playerController.gravityWork);
        //drawForce.Draw(-pd.tensionForce, pd.transform);



    }
}
