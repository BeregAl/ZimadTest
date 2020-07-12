using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : Singleton<PlayerControl>
{
    [SerializeField] private Gem selectedGem;

    public Gem SelectedGem
    {
        get => selectedGem;
        set
        {
            selectedGem = value;
            if (selectedGem == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickOnGem(Gem gem)
    {
        if (SelectedGem == null)
        {
            SelectedGem = gem;
        }
        else
        {
            TrySwapGems(SelectedGem, gem);
        }
    }

    private void TrySwapGems(Gem firstSelectedGem, Gem secondSelectedGem)
    {
        //Debug.Log($"Swapping {firstSelectedGem.currentSlot.coordinates} and {secondSelectedGem.currentSlot.coordinates}");


        if (IsNearGemsSelected(firstSelectedGem, secondSelectedGem) == false)
        {
            SelectedGem = secondSelectedGem;
            EventSystem.current.SetSelectedGameObject(secondSelectedGem.gemView.gameObject);
            return;
        }
        
        if (IsSwapLeadsToMatch())
        {
            var tempCoord = secondSelectedGem.currentSlot.coordinates;
            secondSelectedGem.CurrentSlot = LevelGenerator.slots[firstSelectedGem.currentSlot.coordinates];
            firstSelectedGem.CurrentSlot = LevelGenerator.slots[tempCoord];

            firstSelectedGem.CurrentSlot.gemInSlot = firstSelectedGem;
            secondSelectedGem.CurrentSlot.gemInSlot = secondSelectedGem;
            
            firstSelectedGem.gemView.MoveTo(firstSelectedGem.currentSlot.rectTransform.position, true);
            secondSelectedGem.gemView.MoveTo(secondSelectedGem.currentSlot.rectTransform.position, true);
            
            SelectedGem = null;
            CheckMatches();
        }
        else
        {
            firstSelectedGem.gemView.ShakeGem();
            secondSelectedGem.gemView.ShakeGem();
        }
        

    }

    private bool IsNearGemsSelected(Gem gem1, Gem gem2)
    {
       // Debug.Log($"Distance {(gem1.currentSlot.coordinates - gem2.currentSlot.coordinates).magnitude}");
        return ((gem1.currentSlot.coordinates - gem2.currentSlot.coordinates).magnitude == 1);
    }

    private bool IsSwapLeadsToMatch()
    {
        return true;
    }

    public void CheckMatches()
    {
        
        Vector2Int levelSize = LevelGenerator.instance.LevelSize;
        
        //Horizontal check
        for (int y = 0; y < levelSize.y; y++)
        {
            for (int x = 0; x < levelSize.x; x++)
            {
                if (LevelGenerator.slots.ContainsKey(new Vector2Int(x, y)))
                {
                    int originX = x;
                    GemColor originColor = LevelGenerator.slots[new Vector2Int(x, y)].gemInSlot.gemInfo.gemColor;

                    while (
                        LevelGenerator.slots.ContainsKey(new Vector2Int(originX, y)) &&
                        LevelGenerator.slots[new Vector2Int(originX, y)].gemInSlot.gemInfo.gemColor == originColor
                    )
                    {
                        originX++;
                        
                    }

                    originX--;

                    if (originX - x >= 2)
                    {
                        //Match found
                        Debug.Log($"Match! x:{x}-{originX} y:{y} color {originColor}");
                    }
                    
                    x = originX;

                }
                
            }
        }
        
        
        
        //LevelGenerator
    }
}
