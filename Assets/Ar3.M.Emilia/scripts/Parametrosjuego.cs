using UnityEngine;

[CreateAssetMenu(fileName = "GameParameters", menuName = "AR Treasure Hunt/GameParameters")]
public class GameParameters : ScriptableObject
{
    public int horizontalPlanes;
    public int verticalPlanes;
    public float totalTime;
    public bool useOcclusion;
}