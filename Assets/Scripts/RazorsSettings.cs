using UnityEngine;

[System.Serializable]
public class RazorsSettings
{
    public float razorLength;
    public float razorHelpSphereRadius;
    public Transform[] razors;
    
    public Ray ray;
    public RaycastHit hit;
}