using System;
using System.Collections.Generic;
using UnityEngine;

public class Pluriball : MonoBehaviour ,IClickable
{ 
//    [SerializeField] 
//    private int rows;
//    [SerializeField]
//    private int columns;
    [SerializeField]
    private PoolData normalBubbles;
    [SerializeField]
    private PoolData alreadyPoppedBubbles;
    [SerializeField]
    private BoxCollider2D _collider;



    private Dictionary<EBubbleType, PoolData> poolDataDictionary;
    private Bubble[] bubbles;
    private int remainingBubbles;
    private float width, height;
    private int rows, columns;
    private LevelManager levelManager;
    private Vector3 colliderOriginalSize;

    private void Start()
    {
        levelManager = LevelManager.Get();
        levelManager.OnStart += OnStart;
        colliderOriginalSize = _collider.bounds.size;
        width = _collider.size.x * transform.localScale.x;   //da calcolare
        height = _collider.size.y * transform.localScale.y;


        Pooler.Instance.AddToPool(normalBubbles);
        Pooler.Instance.AddToPool(alreadyPoppedBubbles);
        //Creazione a MANAZZA del dizionario TipoBolla PoolData.  PS. Sì, si potrebbe usare un array serializzato di pool data e poi da ogni elemento
        // risalire al tipo di bolla tramite il prefab associato, ma stica!
        poolDataDictionary = new Dictionary<EBubbleType, PoolData>
        {
            { EBubbleType.Normal, normalBubbles },
            { EBubbleType.AlredyPopped, alreadyPoppedBubbles }
        };

    }



    public void OnStart()
    {
        columns = (int)levelManager.ActiveEntryData.grid_Size.x;
        rows = (int)levelManager.ActiveEntryData.grid_Size.y;
        remainingBubbles = rows * columns;
        bubbles = new Bubble[remainingBubbles];
       
        Generate(rows, columns);

    }

    public void Generate(int rows, int columns)
    {
        
        Vector2 origin = transform.position;

        bubbles = ProceduralGeneration(LevelManager.Get().ActiveEntryData, poolDataDictionary);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                bubbles[index].transform.position = origin + new Vector2(bubbles[index].GetSize().x * col, -(bubbles[index].GetSize().y * row));

                bubbles[index].transform.position += new Vector3(bubbles[index].GetSize().x * 0.5f, -(bubbles[index].GetSize().y * 0.5f), 0);       

                if (bubbles[index].IsAlive)
                    bubbles[index].OnDestroy += OnBubbleDestroy;
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

    public void OnClick(Vector2 point, float radius)
    {
        Bubble b = GetBubbleFromVector(point);
        if (b == null) return;

        //TODO: sto harcodando un pollice e uno schiaffo ma vanno presi dall'aram giusta
        b.InternalOnHit(1, EWeaponType.Finger);

        /*TODOD check if has clicked a real cell
             what cell based on position
            then check on what type of weapon player has
            and switch for specific*/
    }


    private Bubble GetBubbleFromVector(Vector2 point)
    {
        Vector2 origin = transform.position;
        int index = GetIndexBubble(point, origin, new Vector2(width, height));
        return bubbles[index];
    }

    //Al momento assumo che sia un quadrato perfetto, quindi un 2x2 3x3 ecc
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

    public void OnBubbleDestroy()
    {
        remainingBubbles--;
        Debug.Log(remainingBubbles);
        if (remainingBubbles <= 0) {           
            transform.localScale = Vector3.one;
            foreach (Bubble bubble in bubbles)
            {
                bubble.ResetBubble(1);
                bubble.OnDestroy -= OnBubbleDestroy;
                bubble.gameObject.SetActive(false);
            }
            Array.Clear(bubbles, 0, bubbles.Length);
            levelManager.Level += 1;
            //TODO make things
        } else {
            
        }
    }

    #region Procedural Generation
    private Bubble[] ProceduralGeneration(LevelEntryStruct levelStruct, Dictionary<EBubbleType,PoolData> poolDatas)
    {
        int size = (int)levelStruct.grid_Size.x * (int)levelStruct.grid_Size.y;
        Bubble[] bubbles = new Bubble[size];

        BubbleToCreate[] typesToCreate = levelStruct.bubbles;
        uint[] counterTypeBubbles = new uint[typesToCreate.Length];        

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
            i++;         

        }

        //Controllo se le condizioni di bolle minime è stato rispettato. se non è così rigenero da capo
        //if (!CheckGenerationCorrectness(bubbles, typesToCreate))
        //    ProceduralGeneration(levelStruct, poolDatas);

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
            if (!bubbleNumbersType.ContainsKey(bubble.type) || bubbleNumbersType[bubble.type] < bubble.min_Spawn)
                return false;
        }
        return true;
    }
    #endregion


}
