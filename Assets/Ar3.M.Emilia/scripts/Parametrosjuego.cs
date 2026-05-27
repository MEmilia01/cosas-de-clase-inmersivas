using UnityEngine;

public class GameParameters : MonoBehaviour
{
    private int horizontalPlanes;
    private int verticalPlanes;
    private int prefabsporPlano;
    private float totalTime;
    private bool useOcclusion;

    public int HorizontalPlanes { get => horizontalPlanes; set => horizontalPlanes = value; }
    public int VerticalPlanes { get => verticalPlanes; set => verticalPlanes = value; }
    public int PrefabsporPlano { get => prefabsporPlano; set => prefabsporPlano = value; }
    public float TotalTime { get => totalTime; set => totalTime = value; }
    public bool UseOcclusion { get => useOcclusion; set => useOcclusion = value; }
}