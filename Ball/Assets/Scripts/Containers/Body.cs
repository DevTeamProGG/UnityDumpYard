using UnityEngine;
using System.Collections;

public class Body : MonoBehaviour {

    public float rotationSpeed;
    public bool selfRotating;
    public bool enableRotation;
    public int rotationDirection;

    public int getRotationDirection()
    {
        return rotationDirection;
    }

    public void toggleRotationDirection()
    {
        if (rotationDirection == 1)
            rotationDirection = -1;
        else
            rotationDirection = 1;
    }

    public bool getSelfRotating()
    {
        return selfRotating;
    }

    public bool isRotating()
    {
        return enableRotation;
    }

    public void toggleRotation()
    {
        enableRotation = !enableRotation;
    }

    public void setSelfRotating(bool value)
    {
        selfRotating = value;
    }

    public float getRotationSpeed()
    {
        return rotationSpeed;
    }

    public void setRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
