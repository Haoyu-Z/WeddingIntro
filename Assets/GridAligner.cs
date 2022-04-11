using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAligner : MonoBehaviour
{
    [SerializeField]
    private float pixelPerUnit = 16;

    public void Align()
    {
        Vector3 position = gameObject.transform.position;
        float x = Mathf.Round(position.x * pixelPerUnit) / pixelPerUnit;
        float y = Mathf.Round(position.y * pixelPerUnit) / pixelPerUnit;
        gameObject.transform.position = new Vector3(x, y, 0.0f);
    }
}
