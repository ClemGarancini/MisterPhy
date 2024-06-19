using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputImpulsion : MonoBehaviour
{
    private float dynamicFrictionCoefficient;
    public InputImpulsion(float dynamicFrictionCoefficient)
    {
        this.dynamicFrictionCoefficient = dynamicFrictionCoefficient;
    }

    public Vector3 ComputeForce(PlayerController.CollisionInformation collisionInformation, PlayerController.FrameInput frameInput)
    {
        if (collisionInformation.isGrounded)
        {
            return frameInput.horizontal * dynamicFrictionCoefficient * collisionInformation.tangent;
        }
        return Vector3.zero;
    }
}

public class InternalForces : MonoBehaviour
{
    private float dynamicFrictionCoefficient = 20.0f;


    InputImpulsion inputImpulsion;
    private Vector3 inputImpulsionForce;

    // Start is called before the first frame update
    void Start()
    {
        inputImpulsion = new InputImpulsion(dynamicFrictionCoefficient);
    }

    // Update is called once per frame
    public List<Vector3> ComputeForces(PlayerController playerController)
    {
        PlayerController.CollisionInformation collisionInformation = playerController.collisionInformation;
        PlayerController.FrameInput frameInput = playerController.frameInput;
        List<Vector3> forces = new();

        inputImpulsionForce = inputImpulsion.ComputeForce(collisionInformation, frameInput);
        forces.Add(inputImpulsionForce);

        return forces;
    }
}
