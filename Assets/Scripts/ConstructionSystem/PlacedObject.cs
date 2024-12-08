using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, ConstructionType.Dir dir, ConstructionType construction)
    {
        Transform constructionTransform = Instantiate(construction.prefab, worldPosition, Quaternion.identity);

        PlacedObject placedObject = constructionTransform.GetComponent<PlacedObject>();

        placedObject.placedObjectType = construction;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }

    public ConstructionType placedObjectType;
    private Vector2Int origin;
    private ConstructionType.Dir dir;

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectType.GetGridPositionList(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
