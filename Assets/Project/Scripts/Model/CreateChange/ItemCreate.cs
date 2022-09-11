using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreate : MonoBehaviour
{
    //create
    private MeshRenderer[] mrs;
    private List<Collider> collisionList = new List<Collider>();
    private Collider coll;

    private Material[] saveMaterials;
    private Material correct, incorrect;

    private bool isCorrect;
    
    private void Awake()
    {
        gameObject.layer = 2;
    }

    private void Start()
    {
        mrs = GetComponentsInChildren<MeshRenderer>();
        List<Material> materials = new List<Material>();
        for (int x = 0; x < mrs.Length; x++)
        {
            for (int y = 0; y < mrs[x].sharedMaterials.Length; y++)
            {
                materials.Add(mrs[x].sharedMaterials[y]);
                Debug.Log("material: " + mrs[x].sharedMaterials[y].name);
            }
            Debug.Log("mrs[x].materials.Length: " + mrs[x].sharedMaterials.Length);
        }
        Debug.Log("mrs.Length: " + mrs.Length);
        saveMaterials = materials.ToArray();
        Debug.Log("saveMaterials.Length: " + saveMaterials.Length);

        coll = GetComponentInChildren<Collider>();
        coll.isTrigger = true;
        
        UpdateMaterial(correct);
    }

    public void Init(Material correct, Material incorrect)
    {
        this.correct = correct;
        this.incorrect = incorrect;
    }

    private void OnDestroy()
    {
        gameObject.layer = 0;
        coll.isTrigger = false;
    }

    private void Update()
    {
        if (isCorrect && collisionList.Count > 0)
        {
            isCorrect = false;
            UpdateMaterial(incorrect);
        } else if (!isCorrect && collisionList.Count <= 0)
        {
            isCorrect = true;
            UpdateMaterial(correct);
        }
    }

    public void UpdateMaterial(Material material)
    {
        for (int x = 0; x < mrs.Length; x++)
        {
            Material[] mats = new Material[mrs[x].materials.Length];
            for (int y = 0; y < mrs[x].materials.Length; y++)
            {
                mats[y] = material;
            }

            mrs[x].materials = mats;
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

    private void ResetMaterials()
    {
        for (int x = 0, y = 0; x < mrs.Length; x++)
        {
            Material[] mats = new Material[mrs[x].materials.Length];
            for (int y2 = 0; y2 < mrs[x].materials.Length; y++, y2++)
            {
                mats[y2] = saveMaterials[y];
            }

            mrs[x].materials = mats;
        }
    }

    public bool Standing()
    {
        if (!isCorrect)
            return false;
        
        ResetMaterials();
        return true;
    }
}
