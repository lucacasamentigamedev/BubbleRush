using System;
using UnityEngine;

public class Pluriball : MonoBehaviour ,IClickable
{
    [SerializeField] 
    private int rows;
    [SerializeField]
    private int columns;
    [SerializeField]
    private PoolData normalBubbles;
    [SerializeField]
    private BoxCollider2D collider;
    
    private Bubble[] bubbles;
    private int remainingBubbles;
    private float width, height;

    private void Start()
    {
        
        width = collider.size.x * transform.localScale.x;   //da calcolare
        height = collider.size.y * transform.localScale.y;

        remainingBubbles = rows * columns;
        bubbles = new Bubble[remainingBubbles];
        Pooler.Instance.AddToPool(normalBubbles); 

        Generate(rows, columns);

    }

    public void Generate(int rows, int columns /*pattern??*/)
    {
        Array.Clear(bubbles, 0, bubbles.Length);
        Vector2 origin = transform.position;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Bubble bubble = Pooler.Instance.GetPooledObject(normalBubbles).GetComponent<Bubble>();
                bubble.transform.position = origin + new Vector2(bubble.GetSize().x * col, -(bubble.GetSize().y * row));

                bubble.transform.position += new Vector3(bubble.GetSize().x *0.5f, -(bubble.GetSize().y *0.5f),0);

                bubble.gameObject.SetActive(true);
                bubble.OnDestroy += OnBubbleDestroy;
                bubbles[row * rows + col] = bubble;
            }
        }
        //resize
        width = bubbles[0].GetSize().x * columns;       //+ offset 
        height = bubbles[0].GetSize().y * rows;

        Vector3 newScale = transform.localScale;
        newScale.x = width / collider.bounds.size.x;
        newScale.y = height / collider.bounds.size.y;
        newScale.z = 1;

        transform.localScale = newScale;
    }

    public void OnClick(Vector2 point, float radius)
    {
        Bubble b = GetBubbleFromVector(point);
        if (b == null) return;
        b.OnDestroy?.Invoke();
       
        
        /*TODOD check if has clicked a real cell
             what cell based on position
            then check on what type of weapon player has
            and switch for specific*/
    }


    private Bubble GetBubbleFromVector(Vector2 point)
    {
        Vector2 origin = transform.position;
        return bubbles[GetIndexBubble(point, origin, width, bubbles.Length)];
    }

    //Al momento assumo che sia un quadrato perfetto, quindi un 2x2 3x3 ecc
    private int GetIndexBubble(Vector2 point, Vector2 pluriballOrigin, float pluriballDimension, int sphereNumbers)
    {
        Debug.Log("Mouse clicca a " + point);
        int spheresInRow = columns;
        float cellDimension = pluriballDimension / spheresInRow;

        Debug.Log("Origine della paintball " + pluriballOrigin);

        float xRelative = point.x - pluriballOrigin.x;
        float yRelative = Math.Abs(point.y - pluriballOrigin.y);

        Debug.Log("xRelative " + xRelative + "; yRelative " + yRelative);

        int column = Mathf.FloorToInt(xRelative / cellDimension);
        int row = Mathf.FloorToInt(yRelative / cellDimension);

        Debug.Log("column " + column + "; row " + row);
        if (column < 0 || column >= spheresInRow || row < 0 || row >= spheresInRow)
        {
            Debug.LogError("Il punto è fuori dal quadrato!");
            return -1; // Indice non valido
        }

        int index = (row * spheresInRow) + column;
        return index;
    }



    public void OnBubbleDestroy()
    {
        remainingBubbles--;
        if (remainingBubbles <= 0) {
            Debug.Log("Hai vinto il livello");
            //TODO make things
        } else {
            
        }
    }

    
}
