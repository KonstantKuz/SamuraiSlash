using System;

public interface IDamageable
{
    Action OnTakeDamage { get; set; }
    void TakeDamage();
}