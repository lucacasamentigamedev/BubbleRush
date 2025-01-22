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
    
    private Bubble[] bubbles;
    private int remainingBubbles;
    private BoxCollider2D collider;
    private float width, height;


    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        width = collider.size.x * transform.localScale.x;
        height = collider.size.y * transform.localScale.y;

        remainingBubbles = rows * columns;
        bubbles = new Bubble[remainingBubbles];
        Pooler.Instance.AddToPool(normalBubbles); 
        Generate(rows, columns);
    }
    public void Generate(int rows, int columns /*pattern??*/)
    {
        //clear array
        Array.Clear(bubbles, 0, bubbles.Length);

        for (int i = 0;i < remainingBubbles; i++)
        {
            int x = i/rows;
            int y = i%rows;
            Bubble bubble = Pooler.Instance.GetPooledObject(normalBubbles).GetComponent<Bubble>();
            bubble.transform.position = transform.position + new Vector3(bubble.GetSize().x *x, bubble.GetSize().y *y, 0);
            bubble.gameObject.SetActive(true);
            bubbles[i] = bubble;
        }
        //register on every bubble popped
        foreach (Bubble bubble in bubbles)
        {
            bubble.OnDestroy += OnBubbleDestroy;
        }
    }

    public void OnClick(Vector2 point, float radius)
    {
        Debug.Log("Player has clicked position" + point);
        Bubble b = GetBubbleFromVector(point);
        b.OnDestroy?.Invoke();
        /*TODOD check if has clicked a real cell
             what cell based on position
            then check on what type of weapon player has
            and switch for specific*/
    }


    private Bubble GetBubbleFromVector(Vector2 point)
    {
        if (point.x < transform.position.x -(width / 2 ) || point.x > transform.position.x + (width / 2)) return null;
        if (point.y < transform.position.y - (height / 2) || point.x > transform.position.y + (height / 2)) return null;
        Vector2 origin = new Vector2(transform.position.x - (width / 2), transform.position.y - (height / 2));
        return bubbles[GetIndexBubble(point, origin, width, bubbles.Length)];
    }

    //Al momento assumo che sia un quadrato perfetto, quindi un 2x2 3x3 ecc
    private int GetIndexBubble(Vector2 point, Vector2 squareOrigin, float squareDimension, int sphereNumbers)
    {
       
        int spheresInRow = Mathf.RoundToInt(Mathf.Sqrt(sphereNumbers));
        float cellDimension = squareDimension / spheresInRow;


        float xRelative = point.x - squareOrigin.x;
        float yRelative = point.y - squareOrigin.y;

        int column = Mathf.FloorToInt(xRelative / cellDimension);
        int row = Mathf.FloorToInt(yRelative / cellDimension);

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
