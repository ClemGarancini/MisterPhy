using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


public class CharacterMovementManagment : MonoBehaviour
{


    private FrameInput frameInput;
    private Gravity gravity;


    // Start is called before the first frame update
    void Start()
    {
        gravity = GetComponent<Gravity>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void GatherInput()
    {
        // Only interested in the sign of the input (h>0 --> up move)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        horizontalInput = horizontalInput != 0 ? Math.Sign(horizontalInput) : 0;

        frameInput = new FrameInput
        {
            Move = horizontalInput,
        };
    }

    public struct FrameInput
    {
        public float Move;
    }

}
