using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public enum E_Resize
{
    GROWN,
    REDUCE
}
public enum E_Move
{
    UP,
    DOWN,
    MIDDLE,
    HIDE_UP,
    HIDE_DOWN
}


public class WeaponSelector : MonoBehaviour
{
    private Coroutine resizeCoroutine;
    private Coroutine moveCoroutine;
    public void Resize(E_Resize _resize)
    {
        if (resizeCoroutine != null)
        {
            StopCoroutine(resizeCoroutine);
        }
        resizeCoroutine = StartCoroutine(ResizeWithDelay(_resize));
    }
    public void Move(E_Move _move)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveWithDelay(_move));
    }

    private IEnumerator ResizeWithDelay(E_Resize _resize)
    {
        Vector3 resizeFactor = Vector3.zero;
        Vector3 limitScale = Vector3.zero;
        if(_resize == E_Resize.REDUCE)
        {
            resizeFactor = new Vector3(-0.006f, -0.006f, 0f);
            limitScale = new Vector3(0.7f, 0.7f, 1f);
            while (gameObject.transform.localScale.x >= limitScale.x)
            {
                gameObject.transform.localScale += resizeFactor;
                yield return new WaitForEndOfFrame();      
            }
        }
        else if (_resize == E_Resize.GROWN)
        {
            resizeFactor = new Vector3(0.006f, 0.006f, 0f);
            limitScale = new Vector3(0.9f,0.9f,1f);

            while (gameObject.transform.localScale.x <= limitScale.x)
            {
                gameObject.transform.localScale += resizeFactor;
                yield return new WaitForEndOfFrame();       
            }
        }
    }
    private IEnumerator MoveWithDelay(E_Move _move)
    {
        Vector3 posYFactor = Vector3.zero;
        float limitY = 0.0f;
        Func<float, float, bool> _condition;
        switch (_move)
        {
            case E_Move.UP:
                    limitY = 1.2f;
                    posYFactor = new Vector3(0.0f, 0.01f, 0.0f);
                    _condition = (x, y) => x <= y;
                break;
            case E_Move.DOWN:
                    limitY = -1.2f;
                    posYFactor = new Vector3(0.0f, -0.01f, 0.0f);
                    _condition = (x, y) => x <= y;
                break;
            case E_Move.MIDDLE:
                    limitY = 0.0f;
                    posYFactor = gameObject.transform.position.y < limitY ? new Vector3(0.0f, 0.01f, 0.0f) : new Vector3(0.0f, -0.01f, 0.0f);
                    _condition = (x, y) => Math.Abs(x - y)<=0.000001f;
                break;
            default:
                 _condition = (x, y) => true; 
                break;
        }

        while (_condition(gameObject.transform.position.y, limitY))
        {
            gameObject.transform.position += posYFactor;
            yield return new WaitForEndOfFrame();
        }

       
    }
}
