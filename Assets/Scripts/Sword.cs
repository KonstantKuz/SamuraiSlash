using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayers;
    [SerializeField] private RazorsSettings razorsSettings;

    private IDamageable parent;
    private bool isAttacking;
    // public bool IsAttacking
    // {
    //     get { return isAttacking; }
    // }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    private void Awake()
    {
        parent = GetComponentInParent<IDamageable>();
        razorsSettings.ray = new Ray();
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
            return;

        foreach (Transform razor in razorsSettings.razors)
        {
            razorsSettings.ray.origin = razor.position;
            razorsSettings.ray.direction = razor.up;
            
            if (Physics.SphereCast(razorsSettings.ray, razorsSettings.razorHelpSphereRadius, 
                                   out razorsSettings.hit, razorsSettings.razorLength,
                                   interactionLayers))
            {
                IDamageable damageable; 
                if (razorsSettings.hit.transform.TryGetComponent(out damageable) && damageable != parent)
                {
                    Debug.Log($"sword has been attacked {razorsSettings.hit.transform.name}");

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
        for (int i = 0; i < razorsSettings.razors.Length; i++)
        {
            Gizmos.DrawLine(razorsSettings.razors[i].position,razorsSettings.razors[i].position +razorsSettings. razors[i].up * razorsSettings.razorLength);
            Gizmos.DrawWireSphere(razorsSettings.razors[i].position + razorsSettings.razors[i].up * razorsSettings.razorLength, razorsSettings.razorHelpSphereRadius);
        }
    }
}

public enum AttackType
{
    Simple,
    Super,
}