using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Transform[] razors;
    [SerializeField] private float razorLength;
    [SerializeField] private float razorHelpSphereRadius;
    
    private bool isAttacking;

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
            return;

        for (int i = 0; i < razors.Length; i++)
        {
            Ray ray = new Ray(razors[i].position, razors[i].up);
            if (Physics.SphereCast(ray, razorHelpSphereRadius, out RaycastHit hit, razorLength))
            {
                IDamageable damageable = hit.transform.GetComponent<IDamageable>(); 
                if (damageable != null)
                {
                    Debug.Log($"sword has been attacked {hit.transform.name}");

                    damageable.TakeDamage();
                    StopAttack();
                    
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < razors.Length; i++)
        {
            Gizmos.DrawLine(razors[i].position, razors[i].position + razors[i].up * razorLength);
            Gizmos.DrawWireSphere(razors[i].position + razors[i].up * razorLength, razorHelpSphereRadius);
        }
    }
}

public interface IDamageable
{
    void TakeDamage();
}
