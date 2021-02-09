using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Properties/Game parameters")]
public class GameSettings : ScriptableObject
{
    public int ScoreToWin;
    public float HippoMoveSpeed;
    public float HippoAttackInterval;
    public float HippoStrangeMin;
    public float HippoStrangeMax;
    public float HippoStrangeLerpSpeed;
    public float EnemyMoveSpeed;
    public float EnemyStrange;
    public float EnemyAttackInterval;
    public int EnemyMaxOnScreen;
    public int[] EnemyRewards = new int[3];

    public GameObject SnowBallPrefab;

}
