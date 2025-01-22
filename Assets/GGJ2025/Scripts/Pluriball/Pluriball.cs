using log4net.Util;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Pluriball : MonoBehaviour ,IClickable
{ 
//    [SerializeField] 
//    private int rows;
//    [SerializeField]
//    private int columns;
    [SerializeField]
    private PoolData normalBubbles;
    [SerializeField]
    private BoxCollider2D _collider;
    
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
    }

    public void OnStart()
    {
        columns = (int)levelManager.ActiveEntryData.grid_Size.x;
        rows = (int)levelManager.ActiveEntryData.grid_Size.y;
        remainingBubbles = rows * columns;
        bubbles = new Bubble[remainingBubbles];
        Pooler.Instance.AddToPool(normalBubbles);
        Debug.Log("columns: " + columns + " rows: " + rows + " remainingBubbles " + remainingBubbles);
        Generate(rows, columns);

    }

    public void Generate(int rows, int columns /*pattern??*/)
    {
        Debug.Log("columns: " + columns + " rows: " + rows);
        Vector2 origin = transform.position;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Bubble bubble = Pooler.Instance.GetPooledObject(normalBubbles).GetComponent<Bubble>();
                //TODO: qua va cambiato abbiamo hardcodato 3 dopo metteremo life vero e il padre blister lo costruirà adeguatamente
                bubble.ResetBubble(3);

                bubble.transform.position = origin + new Vector2(bubble.GetSize().x * col, -(bubble.GetSize().y * row));

                bubble.transform.position += new Vector3(bubble.GetSize().x *0.5f, -(bubble.GetSize().y *0.5f),0);

                bubble.gameObject.SetActive(true);
                bubble.OnDestroy += OnBubbleDestroy;
                int index = row * columns + col;
                bubbles[index] = bubble;
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

    
}
