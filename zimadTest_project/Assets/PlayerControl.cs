using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public RectTransform gravityImageRectTransform;

    public bool InputBlocked = false;
    [HideInInspector]
    public GravityDirection gravityDirection;

    public static bool gravityChangePending = false;


    const int CHARGED_GEM_COMBO_COUNT=4;
    private Coroutine CheckMatchCoroutineLink;
    // Start is called before the first frame update
    void Start()
    {
        gravityDirection = GravityDirection.Down;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ClickOnGem(Gem gem)
    {
        if (InputBlocked == false)
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
        

        SwapGems(firstSelectedGem,secondSelectedGem);


        if (IsSwapLeadsToMatch(firstSelectedGem, secondSelectedGem) == false)
        {
            SwapGems(firstSelectedGem,secondSelectedGem);
        }
        else
        {
            firstSelectedGem.gemView.MoveTo(firstSelectedGem.currentSlot.rectTransform.position, true);
            secondSelectedGem.gemView.MoveTo(secondSelectedGem.currentSlot.rectTransform.position, true);
        
            SelectedGem = null;
            CheckMatches();         
        }
        


        

    }

    private void SwapGems(Gem firstSelectedGem, Gem secondSelectedGem)
    {
        var tempCoord = secondSelectedGem.currentSlot.coordinates;
        secondSelectedGem.CurrentSlot = LevelGenerator.slots[(firstSelectedGem.currentSlot.coordinates.x, firstSelectedGem.currentSlot.coordinates.y)];
        firstSelectedGem.CurrentSlot = LevelGenerator.slots[(tempCoord.x, tempCoord.y)];

        firstSelectedGem.CurrentSlot.gemInSlot = firstSelectedGem;
        secondSelectedGem.CurrentSlot.gemInSlot = secondSelectedGem;
    }

    private bool IsSwapLeadsToMatch(Gem firstSelectedGem, Gem secondSelectedGem)
    {
        return IsGemPartOfMatch(firstSelectedGem) || IsGemPartOfMatch(secondSelectedGem);
    }

    private bool IsGemPartOfMatch(Gem firstSelectedGem)
    {
        Vector2Int originCoordinates = firstSelectedGem.currentSlot.coordinates;

        var chainSize = 1;
        for (int i = originCoordinates.x-1; i >= 0; i--)
        {
            if (LevelGenerator.slots[(i, originCoordinates.y)].gemInSlot.gemInfo.gemColor ==
                firstSelectedGem.gemInfo.gemColor)
            {
                chainSize++;
            }
            else
            {
                break;
            }
        }
        for (int i = originCoordinates.x+1; i < LevelGenerator.instance.LevelSize.x; i++)
        {
            if (LevelGenerator.slots[(i, originCoordinates.y)].gemInSlot.gemInfo.gemColor ==
                firstSelectedGem.gemInfo.gemColor)
            {
                chainSize++;
            }
            else
            {
                break;
            }
        }

        if (chainSize >= 3)
        {
//            Debug.Log($"Chain Size: {chainSize}");
            return true;
        }

        chainSize = 1;
        for (int i = originCoordinates.y-1; i >= 0; i--)
        {
            if (LevelGenerator.slots[(originCoordinates.x, i)].gemInSlot.gemInfo.gemColor ==
                firstSelectedGem.gemInfo.gemColor)
            {
                chainSize++;
            }
            else
            {
                break;
            }
        }
        for (int i = originCoordinates.y+1; i < LevelGenerator.instance.LevelSize.y; i++)
        {
            if (LevelGenerator.slots[(originCoordinates.x, i)].gemInSlot.gemInfo.gemColor ==
                firstSelectedGem.gemInfo.gemColor)
            {
                chainSize++;
            }
            else
            {
                break;
            }
        }
        if (chainSize >= 3)
        {
  //          Debug.Log($"Chain Size: {chainSize}");
            return true;
        }
        else
        {
   //         Debug.Log($"Chain Size: {chainSize}");
            return false;
        }
    }

    private bool IsNearGemsSelected(Gem gem1, Gem gem2)
    {
       // Debug.Log($"Distance {(gem1.currentSlot.coordinates - gem2.currentSlot.coordinates).magnitude}");
        return ((gem1.currentSlot.coordinates - gem2.currentSlot.coordinates).magnitude == 1);
    }



    public void CheckMatches()
    {
        if (InputBlocked == false)
        {
            CheckMatchCoroutineLink = StartCoroutine(CheckMatchesCoroutine());
        }
    }
    
    private IEnumerator CheckMatchesCoroutine()
    {
        Vector2Int levelSize = LevelGenerator.instance.LevelSize;

        InputBlocked = true;
        var chains = new List<GemChain>();
        do
        {
            chains.Clear();
            //Horizontal check
            for (int y = 0; y < levelSize.y; y++)
            {
                for (int x = 0; x < levelSize.x; x++)
                {
                    if (LevelGenerator.slots.ContainsKey((x, y)))
                    {
                        int originX = x;
                        GemColor originColor = LevelGenerator.slots[(x, y)].gemInSlot.gemInfo.gemColor;

                        while (
                            LevelGenerator.slots.ContainsKey((originX, y)) &&
                            LevelGenerator.slots[(originX, y)].gemInSlot.gemInfo.gemColor == originColor
                        )
                        {
                            originX++;

                        }

                        originX--;

                        if (originX - x >= 2)
                        {
                            var gemsInMatch = new List<Gem>();

                            for (int i = x; i <= originX; i++)
                            {
                                gemsInMatch.Add(LevelGenerator.slots[(i, y)].gemInSlot);
                            }

                            var chain = new GemChain(gemsInMatch);
                            chains.Add(chain);
                            //Match found
//                            Debug.Log($"Match! x:{x}-{originX} y:{y} color {originColor}");
                        }

                        x = originX;

                    }

                }
            }

            //Vertical check
            for (int x = 0; x < levelSize.x; x++)
            {
                for (int y = 0; y < levelSize.y; y++)
                {
                    if (LevelGenerator.slots.ContainsKey((x, y)))
                    {
                        int originY = y;
                        GemColor originColor = LevelGenerator.slots[(x, y)].gemInSlot.gemInfo.gemColor;

                        while (
                            LevelGenerator.slots.ContainsKey((x, originY)) &&
                            LevelGenerator.slots[(x, originY)].gemInSlot.gemInfo.gemColor == originColor
                        )
                        {
                            originY++;

                        }

                        originY--;

                        if (originY - y >= 2)
                        {
                            var gemsInMatch = new List<Gem>();

                            for (int i = y; i <= originY; i++)
                            {
                                gemsInMatch.Add(LevelGenerator.slots[(x, i)].gemInSlot);
                            }

                            var chain = new GemChain(gemsInMatch);
                            chains.Add(chain);
                            //Match found
//                            Debug.Log($"Match! x:{x} y:{y}-{originY} color {originColor}");
                        }

                        y = originY;

                    }

                }
            }

            ClearChains(chains);


            ShiftGems();
            
            if (gravityChangePending)
            {
                SwitchGravity();
                gravityChangePending = false;
            }
            
            yield return new WaitForSeconds(0.3f);

        } while (chains.Count > 0);


        InputBlocked = false;
    }

    private void SwitchGravity()
    {
        if (gravityDirection == GravityDirection.Down)
        {
            gravityDirection = GravityDirection.Up;
            gravityImageRectTransform.DORotate(Vector3.zero, 0.1f);
        }
        else if (gravityDirection == GravityDirection.Up)
        {
            gravityDirection = GravityDirection.Down;
            gravityImageRectTransform.DORotate(Vector3.back * 180, 0.1f);
        }
    }

    private void ShiftGems()
    {
        Vector2Int levelSize = LevelGenerator.instance.LevelSize;


        if (gravityDirection == GravityDirection.Down)
        {
            for (var x = levelSize.x-1; x >= 0; x--)
            {
                for (var y = levelSize.y-1; y >=0 ; y--)
                {
                    if (LevelGenerator.slots.ContainsKey((x, y)) && 
                           LevelGenerator.slots[(x, y)].gemInSlot == null)
                    {
                        
                        //Debug.Log($"Found empty x:{x} y:{y}");
                        var shift = 0;
                        for (int i = y; i >= 0; i--)
                        {
                            if (LevelGenerator.slots[(x, i)].gemInSlot != null)
                            {
                                break;
                            }
                            else
                            {
                                shift++;
                                //Debug.Log($"Null at {x} {i}");
                            }
                        }
                        //Debug.Log($"Shifting {shift}");
                        
                        for (int i = y; i >= shift; i--)
                        {
                            //Debug.Log($"Shifting existing gem {x}:{i}");
                            LevelGenerator.slots[(x, i)].gemInSlot = LevelGenerator.slots[(x, i - shift)].gemInSlot;
                            if (LevelGenerator.slots[(x, i)].gemInSlot != null)
                            {
                                LevelGenerator.slots[(x, i)].gemInSlot.currentSlot = LevelGenerator.slots[(x, i)];
                                LevelGenerator.slots[(x, i)].gemInSlot.gemView.UpdatePosition(true);
                            }
                        }

                        for (int i = 0; i < shift; i++)
                        {
                            //Debug.Log($"Adding new gems {x}:{i}");
                            LevelGenerator.slots[(x, i)].gemInSlot = GemsPool.GetFromPool().gem;
                            LevelGenerator.slots[(x, i)].gemInSlot.currentSlot = LevelGenerator.slots[(x, i)];
                            
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.InitializeWithGemInfo(
                                GemsLibrary.CommonGems[Random.Range(0, GemsLibrary.CommonGems.Count)]
                            );
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.MoveTo(LevelGenerator.slots[(x, 0)].rectTransform.position +Vector3.up*shift);
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.UpdatePosition(true);
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.gameObject.SetActive(true);
                            
                        }
                        
                    }
                }
            }            
        }
        
        else if (gravityDirection == GravityDirection.Up)
        {
            for (var x = 0; x < levelSize.x; x++)
            {
                for (var y = 0; y < levelSize.y; y++)
                {
                    if (LevelGenerator.slots.ContainsKey((x, y)) && 
                           LevelGenerator.slots[(x, y)].gemInSlot == null)
                    {
                        
                        //Debug.Log($"Found empty x:{x} y:{y}");
                        var shift = 0;
                        for (int i = y; i < levelSize.y; i++)
                        {
                            if (LevelGenerator.slots[(x, i)].gemInSlot != null)
                            {
                                break;
                            }
                            else
                            {
                                shift++;
                                //Debug.Log($"Null at {x} {i}");
                            }
                        }
                        //Debug.Log($"Shifting {shift}");
                        
                        for (int i = y; i < levelSize.y-shift; i++)
                        {
                            //Debug.Log($"Shifting existing gem {x}:{i}");
                            LevelGenerator.slots[(x, i)].gemInSlot = LevelGenerator.slots[(x, i + shift)].gemInSlot;
                            if (LevelGenerator.slots[(x, i)].gemInSlot != null)
                            {
                                LevelGenerator.slots[(x, i)].gemInSlot.currentSlot = LevelGenerator.slots[(x, i)];
                                LevelGenerator.slots[(x, i)].gemInSlot.gemView.UpdatePosition(true);
                            }
                        }

                        for (int i = levelSize.y-shift; i < levelSize.y; i++)
                        {
                            //Debug.Log($"Adding new gems {x}:{i}");
                            LevelGenerator.slots[(x, i)].gemInSlot = GemsPool.GetFromPool().gem;
                            LevelGenerator.slots[(x, i)].gemInSlot.currentSlot = LevelGenerator.slots[(x, i)];
                            
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.InitializeWithGemInfo(
                                GemsLibrary.CommonGems[Random.Range(0, GemsLibrary.CommonGems.Count)]
                            );
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.MoveTo(LevelGenerator.slots[(x, levelSize.y-1)].rectTransform.position +Vector3.down*shift);
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.UpdatePosition(true);
                            LevelGenerator.slots[(x, i)].gemInSlot.gemView.gameObject.SetActive(true);
                            
                        }
                        
                    }
                }
            }                  
        }

        
        
        
    }

    public void ClearChains(List<GemChain> chains)
    {
        //Debug.Log($"Clearing {chains.Count} chains");
        foreach (var chain in chains)
        {
            if (chain.chainedGems.Count >= CHARGED_GEM_COMBO_COUNT)
            {
                var rndGemInChain = Random.Range(0, chain.chainedGems.Count);
                chain.chainedGems[rndGemInChain].gemView.InitializeWithGemInfo(
                    GemsLibrary.gems.First(x=>x.gemType == GemType.Charged && x.gemColor == chain.chainedGems[rndGemInChain].gemInfo.gemColor)
                    );

                chain.chainedGems.RemoveAt(rndGemInChain);
            }
            
            foreach (var gem in chain.chainedGems)
            {
                if (gem.currentSlot != null)
                {
                    
                    gem.currentSlot.gemInSlot = null;
                    gem.currentSlot = null;
                    
                    gem.gemBehaviour.DestroyOnMatch();
                    GemsPool.ReturnToPool(gem.gemView);
                }
            }
        }
    }
}