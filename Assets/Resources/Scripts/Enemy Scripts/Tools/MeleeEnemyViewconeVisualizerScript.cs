using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(MeleeEnemyBehaviorScript))]
public class MeleeEnemyViewconeVisualizerScript : Editor
{
    private void OnSceneGUI()
    {
        MeleeEnemyBehaviorScript Viewcone = (MeleeEnemyBehaviorScript)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(Viewcone.transform.position, Vector3.up, Vector3.forward, 360, Viewcone.SightRange);

        Vector3 viewAngle01 = DirectionFromAngle(Viewcone.transform.eulerAngles.y, -Viewcone.SightAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(Viewcone.transform.eulerAngles.y, Viewcone.SightAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(Viewcone.transform.position, Viewcone.transform.position + viewAngle01 * Viewcone.SightRange);
        Handles.DrawLine(Viewcone.transform.position, Viewcone.transform.position + viewAngle02 * Viewcone.SightRange);

        if (Viewcone.PlayerInSightRange)
        {
            Handles.color = Color.green;
            Handles.DrawLine(Viewcone.transform.position, Viewcone.PlayerObject.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}