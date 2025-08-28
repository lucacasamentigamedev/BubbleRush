using System;
using System.Collections.Generic;
using UnityEngine;

public class Pluriball : MonoBehaviour, IClickable
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
    private PoolData teleportBubbles;
    [SerializeField]
    private BoxCollider2D _collider;
    [SerializeField]
    private CameraShake cameraShake;
    [SerializeField]
    private GameObject pluriballVisual;
    [SerializeField]
    private Transform[] popLocation;
    [SerializeField]
    private GameObject asset;

    private Dictionary<EBubbleType, PoolData> poolDataDictionary;
    private Bubble[] bubbles;
    private int remainingBubbles;
    private float width, height;
    private int rows, columns;
    private LevelManager levelManager;

    //private Vector3 colliderOriginalSize;

    Vector3 bubbleSizeMax12x5 = new Vector3(0.15f, 0.15f, 0.0f);
    Vector3 bubbleSizeMax13x6 = new Vector3(0.13f, 0.13f, 0.0f);
    Vector3 bubbleSizeMax14x7 = new Vector3(0.12f, 0.12f, 0.0f);
    Vector3 bubbleSizeMax17x8 = new Vector3(0.1f, 0.1f, 0.0f);

    Vector3 currentBubbleSize;
    
    #region Mono
    private void Start()
    {
        levelManager = LevelManager.Get();
        levelManager.OnStartLevel += OnStartLevel;
        levelManager.OnStartEndlessLevel += OnStartEndlessLevel;
        levelManager.OnRetry += OnRetry;
        width = _collider.size.x * transform.localScale.x;   //da calcolare
        height = _collider.size.y * transform.localScale.y;

        #region Set Pooler
        Pooler.Instance.AddToPool(normalBubbles);
        Pooler.Instance.AddToPool(alreadyPoppedBubbles);
        Pooler.Instance.AddToPool(rockBubbles);
        Pooler.Instance.AddToPool(bombBubbles);
        Pooler.Instance.AddToPool(teleportBubbles);

        //Creazione a MANAZZA del dizionario TipoBolla PoolData.  PS. S�, si potrebbe usare un array serializzato
        //di pool data e poi da ogni elemento risalire al tipo di bolla tramite il prefab associato, ma stica!
        poolDataDictionary = new Dictionary<EBubbleType, PoolData>
        {
            { EBubbleType.Normal, normalBubbles },
            { EBubbleType.AlredyPopped, alreadyPoppedBubbles },
            { EBubbleType.Rock, rockBubbles },
            { EBubbleType.Bomb, bombBubbles },
            { EBubbleType.Teleport, teleportBubbles }
        };
        #endregion

        levelManager.OnLoseLevel += OnLoseLevel;
        levelManager.OnLoseEndlessLevel += OnLoseLevel;
    }
    #endregion

    #region LevelManagerCallback
    private void OnStartEndlessLevel()
    {
        InternalStartLevel();
    }
    private void OnStartLevel(uint levelIndex)
    {
        InternalStartLevel();
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
                ((BombBubble)bubble).OnExplode += OnBombExplode;
            }
                
        }
        levelManager.StartTiming();
    }

    private void OnLoseLevel()
    {
        InternalEndLevel(false);
    }
    #endregion

    #region BubblesCallback
    private void OnBubbleDestroy()
    {
        remainingBubbles--;
        //Debug.Log(remainingBubbles);
        if (remainingBubbles <= 0) {
            InternalEndLevel(true);    
        } else {
            int index = UnityEngine.Random.Range(0, popLocation.Length);
            //CAMBIARE ASSOLUTAMENTE -> GESTIRLO TRAMITE POOLER
            GameObject obj = Instantiate(asset, popLocation[index].transform.position, popLocation[index].transform.rotation);
            obj.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(-45, 46)));
        }
    }
    private void OnCamerShake(float shakeMagnitude, float shakeDuration)
    {
        cameraShake.Shake(shakeMagnitude, shakeDuration);
    }
    private void OnTeleportCall(TeleportBubble bubble)
    {
        Bubble emptyBubble = GetRandomBubbleWithTotLife(0);     //prendi una bolla già scoppiata


        SwitchBubblesPosition(bubble, emptyBubble);
    }
    private void OnBombExplode(float arg)
    {
        GlobalEventSystem.CastEvent(EventName.ModulateTimer, EventArgsFactory.ModulateTimerFactory(arg));
    }
    #endregion


    private void InternalStartLevel()
    {
        pluriballVisual.SetActive(true);
        columns = (int)levelManager.ActiveEntryData.grid_Size.x;
        rows = (int)levelManager.ActiveEntryData.grid_Size.y;
        remainingBubbles = rows * columns;

        if (columns > 14 || rows > 7)
            currentBubbleSize = bubbleSizeMax17x8;
        else if (columns > 13 || rows > 6)
            currentBubbleSize = bubbleSizeMax14x7;
        else if (columns > 12 || rows > 5)
            currentBubbleSize = bubbleSizeMax13x6;
        else
            currentBubbleSize = bubbleSizeMax12x5;

        Bubble b = Pooler.Instance.GetPooledObject(poolDataDictionary[EBubbleType.Normal]).GetComponent<Bubble>();
        b.transform.localScale = currentBubbleSize;

        InternalSetPluriballPosition(rows, columns, b.GetSize());

        bubbles = new Bubble[remainingBubbles];

        //timer.InitTimer(levelManager.ActiveEntryData.timer_for_level, levelManager.ActiveEntryData.is_Timer_Activate);
        Generate(levelManager.ActiveEntryData);

        foreach (Bubble bubble in bubbles)
        {
            if (bubble is TeleportBubble)
            {
                (bubble as TeleportBubble).TeleportEvent += OnTeleportCall;
            }
        }
        levelManager.StartTiming();
    }
    private void InternalEndLevel(bool win)
    {
        //Disattiviamo le bolle
        foreach (Bubble bubble in bubbles)
        {
            bubble.gameObject.SetActive(false);
            BombBubble bubbleCast = bubble as BombBubble;
            if (bubbleCast == null) continue;
            bubbleCast.OnExplode -= OnBombExplode;
        }
        pluriballVisual.SetActive(false);

        if (win)
        {
            transform.localScale = Vector3.one;

            foreach (Bubble bubble in bubbles)
            {
                bubble.OnDestroy -= OnBubbleDestroy;
                bubble.OnCamerShake -= OnCamerShake;
                bubble.gameObject.SetActive(false);
            }
            Array.Clear(bubbles, 0, bubbles.Length);
            levelManager.WinLevel();
        }

    }
    private void InternalSetPluriballPosition(int rows, int columns, Vector2 bubbleSize)
    {
        float posx = columns * bubbleSize.x / 2;
        float posy = rows * bubbleSize.y / 2;
        Vector2 newPosition = new Vector2(-posx, posy);
        gameObject.transform.position = newPosition;
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
                if (index + r*columns + c  >= 0 && index + r* columns + c < columns * rows) //se � dentro i range
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

    #endregion

    #region Procedural Generation
    private void Generate(LevelEntryStruct currentLevelData)
    {

        Vector2 origin = transform.position;
        int rows = (int)currentLevelData.grid_Size.y;
        int columns = (int)currentLevelData.grid_Size.x;
        bubbles = ProceduralGeneration(currentLevelData, poolDataDictionary);



        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;

                bubbles[index].transform.localScale = currentBubbleSize;                
                bubbles[index].transform.position = origin + new Vector2(bubbles[index].GetSize().x * col, -(bubbles[index].GetSize().y * row));
                bubbles[index].transform.position += new Vector3(bubbles[index].GetSize().x * 0.5f, -(bubbles[index].GetSize().y * 0.5f), 0);

                if (bubbles[index].BubbleType == EBubbleType.Bomb)
                {
                    ((BombBubble)bubbles[index]).OnExplode += OnBombExplode;
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
        
        transform.localScale = new Vector3(width, height, 1);
    }

    /// <summary>
    /// Istanzia tutte le bolle dall'object pooling corrispondente, poi ne mescola l'ordine e le mantiene disattive in scena
    /// </summary>
    /// <param name="levelStruct">La struttura dove si trovano le info delle bolle della scena</param>
    /// <param name="poolDatas">Un dizionario TipoBolla-PoolData con le pool delle bolle dalle istanziare</param>
    /// <returns></returns>
    private Bubble[] ProceduralGeneration(LevelEntryStruct levelStruct, Dictionary<EBubbleType, PoolData> poolDatas)
    {
        int size = (int)levelStruct.grid_Size.x * (int)levelStruct.grid_Size.y;
        Bubble[] bubbles = new Bubble[size];

        BubbleToCreate[] typesToCreate = levelStruct.bubbles;
        BubbleToCreate bubbleType;
        BubbleToCreate bubbleFillerType = new BubbleToCreate();
        int indexB = 0;
        for (int i = 0; i < typesToCreate.Length; i++)
        {
            bubbleType = typesToCreate[i];
            if (bubbleType.setFiller)
            {
                bubbleFillerType = bubbleType;
                continue;
            }
            int rand = UnityEngine.Random.Range((int)bubbleType.min_Spawn, (int)bubbleType.max_Spawn);
            for(int j = 0; j < rand; j++)
            {
                bubbles[indexB] = Pooler.Instance.GetPooledObject(poolDatas[bubbleType.type]).GetComponent<Bubble>();
                bubbles[indexB].gameObject.SetActive(true);
                int life = UnityEngine.Random.Range((int)bubbleType.min_Pop, (int)bubbleType.max_Pop + 1);
                bubbles[indexB].ResetBubble(life);
                indexB++;
            }
        }
        for(int i = indexB; i< size; i++ )
        {
            bubbles[i] = Pooler.Instance.GetPooledObject(poolDatas[bubbleFillerType.type]).GetComponent<Bubble>();
            bubbles[i].gameObject.SetActive(true);
            int life = UnityEngine.Random.Range((int)bubbleFillerType.min_Pop, (int)bubbleFillerType.max_Pop + 1);
            bubbles[i].ResetBubble(life);
        }


        Reshuffle(bubbles);

        return bubbles;
    }

    private void Reshuffle(Bubble[] array)
    {
        // Knuth shuffle algorithm 
        for (int t = 0; t < array.Length; t++)
        {
            Bubble tmp = array[t];
            int r = UnityEngine.Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
    }
    #endregion

    #region InternalMethods
    private Bubble GetRandomBubbleWithTotLife(uint life)
    {
        List<Bubble> listBubbles = new List<Bubble>();
        int bubblesCounter = 0;
        foreach (Bubble b in bubbles)
        {
            if(b.CurrentLife == life)
            {
                listBubbles.Add(b);
                bubblesCounter++;
            }
        }
        if (bubblesCounter <= 0)
            return null;
        int rand = UnityEngine.Random.Range(0, bubblesCounter);
        return listBubbles[rand];
    }

    private Bubble GetRandomBubbleOfType(EBubbleType type)
    {
        List<Bubble> listBubbles = new List<Bubble>();
        int bubblesCounter=0;
        foreach (Bubble b in bubbles)
        {
            if (b.BubbleType == type)
            {
                listBubbles.Add(b);
                bubblesCounter++;
            }
        }
        if (bubblesCounter <= 0)
            return null;

        int rand = UnityEngine.Random.Range(0, bubblesCounter);

        return listBubbles[rand];
    }

    private void SwitchBubblesPosition(Bubble a, Bubble b)
    {
        if (a ==null || b==null) return;    
        int indexA = GetBubbleIndex(a);
        int indexB = GetBubbleIndex(b);

        bubbles[indexA] = b;
        bubbles[indexB] = a;

        int rowA = indexA / columns;
        int colA = indexA % columns;

        int rowB = indexB / columns;
        int colB = indexB % columns;


        Vector2 origin = transform.position;
        bubbles[indexA].transform.position = origin + new Vector2(bubbles[indexA].GetSize().x * colA, -(bubbles[indexA].GetSize().y * rowA));
        bubbles[indexA].transform.position += new Vector3(bubbles[indexA].GetSize().x * 0.5f, -(bubbles[indexA].GetSize().y * 0.5f), 0);

        bubbles[indexB].transform.position = origin + new Vector2(bubbles[indexB].GetSize().x * colB, -(bubbles[indexB].GetSize().y * rowB));
        bubbles[indexB].transform.position += new Vector3(bubbles[indexB].GetSize().x * 0.5f, -(bubbles[indexB].GetSize().y * 0.5f), 0);
    }

    private int GetBubbleIndex(Bubble bubble)
    {
        for(int i = 0; i < bubbles.Length; i++)
        {
            if (bubbles[i] == bubble)
                return i;
        }
        return -1;
    }
    #endregion

}
