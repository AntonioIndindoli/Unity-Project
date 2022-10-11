using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableButton : MonoBehaviour
{
    public ElevatorController elevatorController1;

    // Start is called before the first frame update
    public void isInteracted()
    {
        if (!elevatorController1.open && !elevatorController1.elevatorCalling)
        {
            elevatorController1.OpenDoors();
        }
        else if (!elevatorController1.elevatorCalling && !elevatorController1.elevatorClosing)
        {
            elevatorController1.CloseDoors();
        }

    }

}
