using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);
            foreach (Collider2D collider in colliderArray)
            {
                if (collider.TryGetComponent(out BuildInteractable buildInteractable))
                {
                    buildInteractable.Interact();
                }
            }
        }
    }

    public BuildInteractable GetInteractableObject()
    {
        List<BuildInteractable> buildInteractableList = new List<BuildInteractable>();

        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D collider in colliderArray)
        {
            if (collider.TryGetComponent(out BuildInteractable buildInteractable))
            {
                buildInteractableList.Add(buildInteractable);
                return buildInteractable;
            }
        }

        BuildInteractable closestBuildInteractable = null;

        foreach (BuildInteractable buildInteractable in buildInteractableList)
        {
            if (closestBuildInteractable == null)
            {
                closestBuildInteractable = buildInteractable;
            }
            else
            {
                if (Vector2.Distance(transform.position, buildInteractable.transform.position) < Vector2.Distance(transform.position, closestBuildInteractable.transform.position))
                {
                    closestBuildInteractable = buildInteractable;
                }
            }
        }

        return closestBuildInteractable;
    }
}
