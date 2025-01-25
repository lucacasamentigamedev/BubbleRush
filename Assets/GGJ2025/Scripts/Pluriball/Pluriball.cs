using System;
using System.Collections.Generic;
using UnityEngine;

public class Pluriball : MonoBehaviour ,IClickable
{ 

    [SerializeField]
    private PoolData normalBubbles;
    [SerializeField]
    private PoolData alreadyPoppedBubbles;
    [SerializeField]
    private PoolData rockBubbles;
    [SerializeField]
    private PoolData bombBubbles;
    [SerializeField]
    private BoxCollider2D _collider;
    [SerializeField]
    private UITimer timer;
    [SerializeField]
    private CameraShake cameraShake;
    [SerializeField]
    private UIBehavior uiBehavior;
    [SerializeField]
    private GameObject pluriballVisual;

    private Dictionary<EBubbleType, PoolData> poolDataDictionary;
    private Bubble[] bubbles;
    private int remainingBubbles;
    private float width, height;
    private int rows, columns;
    private LevelManager levelManager;
    //private Vector3 colliderOriginalSize;

    private void Start()
    {
        levelManager = LevelManager.Get();
        levelManager.OnStart += OnStart;
        levelManager.OnRetry += OnRetry;
        width = _collider.size.x * transform.localScale.x;   //da calcolare
        height = _collider.size.y * transform.localScale.y;

        #region Set Pooler
        Pooler.Instance.AddToPool(normalBubbles);
        Pooler.Instance.AddToPool(alreadyPoppedBubbles);
        Pooler.Instance.AddToPool(rockBubbles);
        Pooler.Instance.AddToPool(bombBubbles);
        
        //Creazione a MANAZZA del dizionario TipoBolla PoolData.  PS. Sì, si potrebbe usare un array serializzato
        //di pool data e poi da ogni elemento risalire al tipo di bolla tramite il prefab associato, ma stica!
        poolDataDictionary = new Dictionary<EBubbleType, PoolData>
        {
            { EBubbleType.Normal, normalBubbles },
            { EBubbleType.AlredyPopped, alreadyPoppedBubbles },
            { EBubbleType.Rock, rockBubbles },
            { EBubbleType.Bomb, bombBubbles }
        };
        #endregion

        timer.onTimerEnd += OnTimerEnd;

    }

    private void OnStart()
    {
        pluriballVisual.SetActive(true);
        columns = (int)levelManager.ActiveEntryData.grid_Size.x;
        rows = (int)levelManager.ActiveEntryData.grid_Size.y;
        remainingBubbles = rows * columns;
        bubbles = new Bubble[remainingBubbles];
        timer.InitTimer(levelManager.ActiveEntryData.timer_for_level, levelManager.ActiveEntryData.is_Timer_Activate);
        Generate(rows, columns);
        Debug.Log("PLURIBALL - Nuovo livello: " + levelManager.Level);
        uiBehavior.ChangeLevelLabel();
    }

    private void OnRetry()
    {
        pluriballVisual.SetActive(true);
        remainingBubbles = rows * columns;
        //reset all bubbles
        foreach (Bubble bubble in bubbles)
        {
            bubble.gameObject.SetActive(true);
            if (bubble.BubbleType!= EBubbleType.AlredyPopped)
            {
                bubble.ResetBubble();
            }else
                remainingBubbles--;

            if (bubble.BubbleType == EBubbleType.Bomb)
            {
                ((BombBubble)bubble).OnExplode += ReduceGlobalTime;
            }
                
        }
        timer.InitTimer(levelManager.ActiveEntryData.timer_for_level, levelManager.ActiveEntryData.is_Timer_Activate);
    }
      
    private void InternalEndLevel(bool win)
    {
        foreach (Bubble bubble in bubbles)
        {
            bubble.gameObject.SetActive(false);
        }
        if (win)
        {
            int starNumbers = 0;
            float[] startsThreshold = levelManager.ActiveEntryData.stars_for_level;                       
            for (int i = 0;i < startsThreshold.Length;i++)
            {
                if (startsThreshold[i] <= timer.GetTimerPercent())
                {
                    starNumbers++;
                }
                else
                {
                    break;
                }
            }
            uiBehavior.OnpePreLevelMenu(starNumbers);

        }
        else
            uiBehavior.OpenEndLevelMenu();
        pluriballVisual.SetActive(false);

    }

    #region Interface OnClick
    public void OnClick(Vector2 point, EWeaponType weapon, int damage, Vector2 area)
    {
        Bubble[] bubblesToHit = GetNearBubbles(point, area);
        foreach (Bubble b in bubblesToHit)
        {
            b.InternalOnHit(damage, weapon);
        }
    }
    #endregion

    #region TimerForBomb
    private void OnTimerEnd()
    {
        InternalEndLevel(false);
        Debug.Log("onTimerEnd");
    }
    private void ReduceGlobalTime(float arg)
    {
        timer.ReduceTimer(arg);
    }
    #endregion

    #region After Click Methods
    private Bubble[] GetNearBubbles(Vector2 point, Vector2 area)
    {
        List<Bubble> arenaBubbleList = new List<Bubble>();
        int index = GetIndexBubble(point, transform.position, new Vector2(width, height));
        arenaBubbleList.Add(bubbles[index]);
        int offsetRounderX = Mathf.FloorToInt((area.x - 1) / 2);
        int offsetRounderY = Mathf.FloorToInt((area.y - 1) / 2);

        for( int r = -offsetRounderX; r <= offsetRounderX; r++)
        {
            for (int c = -offsetRounderY; c <= offsetRounderY; c++)
            {
                if (index + r*columns + c  >= 0 && index + r* columns + c < columns * rows) //se è dentro i range
                {
                    if (index % columns == 0 && c < 0)        //sto premendo la prima colonna, ignoro la colonna di sx
                        continue;
                    if ((index + 1) % columns == 0 && c > 0)     //sto premendo l'ultima colonna, ignoro la colonna di dx
                        continue;
                    arenaBubbleList.Add(bubbles[index + r * columns + c]);
                }                    
            }
        }

        return arenaBubbleList.ToArray();
    }

    private int GetIndexBubble(Vector2 point, Vector2 pluriballOrigin, Vector2 pluriballDimension)
    {
        float cellDimensionX = pluriballDimension.x / columns;
        float cellDimensionY = pluriballDimension.y / rows;


        float xRelative = point.x - pluriballOrigin.x;
        float yRelative = Math.Abs(point.y - pluriballOrigin.y);


        int column = Mathf.FloorToInt(xRelative / cellDimensionX);
        int row = Mathf.FloorToInt(yRelative / cellDimensionY);

        if (column < 0 || column >= columns || row < 0 || row >= rows)
        {
            return -1; // Indice non valido
        }

        int index = (row * columns) + column;
        return index;
    }
    private void OnBubbleDestroy()
    {
        remainingBubbles--;
        Debug.Log(remainingBubbles);
        if (remainingBubbles <= 0) {
            //probably 20 sarà cambiato poi
            InternalEndLevel(true);
            AudioManager.PlayOneShotSound("WinLose", new FMODParameter[] {
                    new FMODParameter("WIN_LOSE", 0.0f)
            });
            transform.localScale = Vector3.one;
            
            foreach (Bubble bubble in bubbles)
            {                
                bubble.OnDestroy -= OnBubbleDestroy;
                bubble.OnCamerShake -= OnCamerShake;
                bubble.gameObject.SetActive(false);
            }
            Array.Clear(bubbles, 0, bubbles.Length);
            levelManager.Level += 1;
            //TODO make things
        } else {
            
        }
    }
    #endregion

    #region Procedural Generation
    private void Generate(int rows, int columns)
    {

        Vector2 origin = transform.position;
        bubbles = ProceduralGeneration(levelManager.ActiveEntryData, poolDataDictionary);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col; 


                bubbles[index].transform.position = origin + new Vector2(bubbles[index].GetSize().x * col, -(bubbles[index].GetSize().y * row));
                bubbles[index].transform.position += new Vector3(bubbles[index].GetSize().x * 0.5f, -(bubbles[index].GetSize().y * 0.5f), 0);

                if (bubbles[index].BubbleType == EBubbleType.Bomb)
                {
                    ((BombBubble)bubbles[index]).OnExplode += ReduceGlobalTime;
                }

                if (bubbles[index].IsAlive)
                {
                    bubbles[index].OnDestroy += OnBubbleDestroy;
                    bubbles[index].OnCamerShake += OnCamerShake;
                }
                else
                    remainingBubbles--;
            }
        }

        //resize
        width = bubbles[0].GetSize().x * columns;       //+ offset 
        height = bubbles[0].GetSize().y * rows;
        Debug.Log($"width: {width} height {height}");
        Vector3 newScale = transform.localScale;
        newScale.x = width / _collider.bounds.size.x;
        newScale.y = height / _collider.bounds.size.y;
        newScale.z = 1;
        Debug.Log($"New Scale: {newScale}");

        transform.localScale = new Vector3(width, height, 1);
    }

    private void OnCamerShake(float shakeMagnitude, float shakeDuration)
    {
        cameraShake.Shake(shakeMagnitude, shakeDuration);
    }

    private Bubble[] ProceduralGeneration(LevelEntryStruct levelStruct, Dictionary<EBubbleType,PoolData> poolDatas)
    {
        int size = (int)levelStruct.grid_Size.x * (int)levelStruct.grid_Size.y;
        Bubble[] bubbles = new Bubble[size];

        BubbleToCreate[] typesToCreate = levelStruct.bubbles;
        uint[] counterTypeBubbles = new uint[typesToCreate.Length];

        do
        {
            //Creo la mappa di bolle, tenendo conto del numero massimo di tipo di bolla inseribile nel livello
            int i = 0;
            while (i < size)
            {
                int randomVar = UnityEngine.Random.Range(0, typesToCreate.Length);

                BubbleToCreate bubbleChose = typesToCreate[randomVar];

                if (counterTypeBubbles[randomVar] == bubbleChose.max_Spawn)
                {
                    //Raggiunto il numero  di bolle di quel tipo massimo, si riprova 
                    continue;
                }
                counterTypeBubbles[randomVar]++;
                bubbles[i] = Pooler.Instance.GetPooledObject(poolDatas[bubbleChose.type]).GetComponent<Bubble>();
                bubbles[i].gameObject.SetActive(true);
                int life = UnityEngine.Random.Range((int)bubbleChose.min_Pop, (int)bubbleChose.max_Pop + 1);
                bubbles[i].ResetBubble(life);
                i++;
            }
        } while (!CheckGenerationCorrectness(bubbles, typesToCreate));  //Controllo se le condizioni di bolle minime è stato rispettato. se non è così rigenero da capo

        return bubbles;
    }
    private bool CheckGenerationCorrectness(Bubble[] bubbleGenerated, BubbleToCreate[] bubblesToCreate)
    {
        Dictionary<EBubbleType, int> bubbleNumbersType = new Dictionary<EBubbleType, int>();
        foreach (Bubble bubble in bubbleGenerated)
        {
            if (!bubbleNumbersType.ContainsKey(bubble.BubbleType))
                bubbleNumbersType.Add(bubble.BubbleType, 0);
            bubbleNumbersType[bubble.BubbleType]++; 
        }
        
        foreach (BubbleToCreate bubble  in bubblesToCreate)
        {
            if (bubble.max_Spawn>0 && (!bubbleNumbersType.ContainsKey(bubble.type) || bubbleNumbersType[bubble.type] < bubble.min_Spawn))
                return false;
        }
        return true;
    }
    #endregion
}
