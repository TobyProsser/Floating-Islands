using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public static NavMeshSurface navSurface;

    private void Awake()
    {
        navSurface = this.transform.GetComponent<NavMeshSurface>();
    }
}
