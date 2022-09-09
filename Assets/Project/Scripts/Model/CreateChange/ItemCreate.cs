using System.Collections.Generic;
using UnityEngine;

public class ItemCreate : MonoBehaviour
{
    //create
    private MeshRenderer[] mrs;
    private List<Collider> collisionList = new List<Collider>();
    private Collider coll;

    private void Awake()
    {
        gameObject.layer = 2;
    }

    private void Start()
    {
        mrs = GetComponentsInChildren<MeshRenderer>();
        coll = GetComponentInChildren<Collider>();
        coll.isTrigger = true;
    }

    private void OnDestroy()
    {
        gameObject.layer = 0;
        coll.isTrigger = false;
        UpdateColor(Color.white);
    }

    private void Update()
    {
        if (collisionList.Count > 0)
        {
            UpdateColor(Color.red);
        }
        else
        {
            UpdateColor(Color.green);
        }
    }
    
    public void UpdateColor(Color color)
    {
        for (int i = 0; i < mrs.Length; i++)
        {
            mrs[i].material.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>())
            collisionList.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Item>())
            collisionList.Remove(other);
    }

    public bool Standing()
    {
        if (collisionList.Count > 0)
            return false;
        return true;
    }
}
