﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{
    [SerializeField]
    private Vector2Int levelSize;

    [SerializeField]
    private GameObject slotPrefab;
    [SerializeField]
    private GameObject gemPrefab;

    [SerializeField]
    private Transform gridForSlots;

    [SerializeField]
    private Transform gemsParent;




    public static Dictionary<Vector2Int,Slot> slots = new Dictionary<Vector2Int, Slot>();
    public List<GemView> gems = new List<GemView>();

    public Vector2Int LevelSize
    {
        get => levelSize;
        set => levelSize = value;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    public void GenerateLevel()
    {
        StartCoroutine(GenerationCoroutine());
    }

    public IEnumerator GenerationCoroutine()
    {

        PopulatePool(LevelSize);
        PopulateLevelWithSlots(LevelSize);
        
        
        yield return new WaitForEndOfFrame();
        PopulateEmptySlotsWithGems();
    }

    private void PopulatePool(Vector2Int levelSize)
    {
        for (int i = 0; i < levelSize.x*levelSize.y; i++)
        {
            var newGem = Instantiate(gemPrefab, gemsParent);
            GemView gemView = newGem.GetComponent<GemView>();
        
            GemsPool.ReturnToPool(gemView);            
        }

    }


    private void PopulateLevelWithSlots(Vector2Int levelSize)
    {
        slots.Clear();
        for (int i = 0; i < levelSize.y; i++)
        {
            for (int j = 0; j < levelSize.x; j++)
            {
                var newSlotGO = Instantiate(slotPrefab, gridForSlots);
                var newSlot = newSlotGO.GetComponent<Slot>();
                newSlot.coordinates = new Vector2Int(j, i);
                newSlot.gameObject.name = $"Slot {j}x{i}";
                slots.Add(new Vector2Int(j,i),newSlot);
            }
        }
    }
    private void PopulateEmptySlotsWithGems()
    {
        
        
        foreach (var slot in slots.Values)
        {
            var gemView = GemsPool.GetFromPool();
            
            gemView.InitializeWithGemInfo(
                GemsLibrary.CommonGems[UnityEngine.Random.Range(0, GemsLibrary.CommonGems.Count)]
                );
            //Debug.Log(slot.gameObject.GetComponent<RectTransform>().position);
            //gemView.MoveTo(slot.gameObject.GetComponent<RectTransform>().position);
            
            slot.gemInSlot = gemView.gem;
            gemView.gem.CurrentSlot = slot;
            gemView.MoveTo(slot.rectTransform.position);
            gemView.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
