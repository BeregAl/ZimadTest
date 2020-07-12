using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
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




    public List<Slot> slots = new List<Slot>();
    public List<GemView> gems = new List<GemView>();


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

        PopulateLevelWithSlots(levelSize);
        yield return new WaitForEndOfFrame();
        PopulateEmptySlotsWithGems();
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
                slots.Add(newSlot);
            }
        }
    }
    private void PopulateEmptySlotsWithGems()
    {
        foreach (var slot in slots)
        {
            var newGem = Instantiate(gemPrefab, gemsParent);
            GemView gemView = newGem.GetComponent<GemView>();
            gemView.InitializeWithGemInfo(
                GemsLibrary.CommonGems[UnityEngine.Random.Range(0, GemsLibrary.CommonGems.Count)]
                );
            Debug.Log(slot.gameObject.GetComponent<RectTransform>().position);
            gemView.MoveTo(slot.gameObject.GetComponent<RectTransform>().position);
            slot.gemInSlot = gemView.gem;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
