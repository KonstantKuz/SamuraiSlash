using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants : MonoBehaviour
{
    public const string PrefsCurrentLevel = "currentLevel";

    public const string TagPlayer = "Player";
    public const string TagPlayerSword = "PlayerSword";
    public const string TagFightCheckPoint = "FightCheckPoint";
    public const string TagPathPoint = "PathPoint";
    
    public const float SlowMoDelayOnEnemyAttack = 0.1f;
    public const float SlowMoDurationOnEnemyAttack = 0.05f;
}
