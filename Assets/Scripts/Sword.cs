using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private RazorsSettings razorsSettings;

    [SerializeField] private GameObject superAttackVFX;
    
    private bool isAttacking;
    public bool IsAttacking
    {
        get { return isAttacking; }
    }

    public void StartAttack(AttackType attackType)
    {
        isAttacking = true;
        if(attackType == AttackType.Super)
            superAttackVFX?.gameObject.SetActive(true);
    }

    public void StopAttack()
    {
        isAttacking = false;
        superAttackVFX?.gameObject.SetActive(false);
    }

    private void Start()
    {
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
                                   out razorsSettings.hit, razorsSettings.razorLength))
            {
                IDamageable damageable; 
                if (razorsSettings.hit.transform.TryGetComponent(out damageable))
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