using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Transform razorStart;
    [SerializeField] private float razorLength;
    [SerializeField] private float razorHelpSphereRadius;
    
    private bool isAttacking;

    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public void Attack()
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
        
        Ray ray = new Ray(razorStart.position, razorStart.up);
        if (Physics.SphereCast(ray,razorHelpSphereRadius, out RaycastHit hit, razorLength))
        {
            IDamageable damageable = hit.transform.GetComponent<IDamageable>(); 
            if (damageable != null)
            {
                Debug.Log($"sword has been attacked {hit.transform.name}");

                damageable.TakeDamage();
                StopAttack();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(razorStart.position, razorStart.position + razorStart.up * razorLength);
        Gizmos.DrawWireSphere(razorStart.position + razorStart.up * razorLength, razorHelpSphereRadius);
    }
}

public interface IDamageable
{
    void TakeDamage();
}
