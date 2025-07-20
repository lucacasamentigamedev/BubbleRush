using System;
using UnityEngine;
using System.Collections;

public enum E_ICON_POSITION
{
    HIDE_UP = 0,
    UP = 1,
    MIDDLE = 2,
    DOWN = 3,
    HIDE_DOWN = 4
}

public class WeaponSelector : MonoBehaviour
{
    public E_ICON_POSITION Position { get; set; }

    private Coroutine moveCoroutine;
    private bool isCoroutineRunning;

    public bool IsPrevented { get { return isCoroutineRunning; } }

    private IEnumerator MoveUpWithDelay(E_ICON_POSITION _movePosition, bool up)
    {
        isCoroutineRunning = true;
        float limitY = 0.0f;
        float limitScale = 0;

        Vector3 posYFactor = new Vector3(0.0f, 0.05f, 0.0f);
        Vector3 resizeFactor = new Vector3(0.004f, 0.004f, 0f);

        Func<float, float, bool> _posCondition;
        Func<float, float, bool> _scaleCondition;
        
        switch (_movePosition)
        {
            case E_ICON_POSITION.HIDE_UP:
                limitY = 1.6f;
                limitScale = 0.0f;
                posYFactor *= 1;
                _posCondition = (x, y) => x < y;
                if (up)
                {
                    resizeFactor *= -4;
                    _scaleCondition = (x, y) => x > y;                    
                }
                else
                {
                    _scaleCondition = (x, y) => false;
                }
                break;
            case E_ICON_POSITION.UP:
                limitY = 1.1f;
                limitScale = 0.7f;
                
                if (up)
                {
                    posYFactor *= 1;
                    _posCondition = (x, y) => x < y;

                    resizeFactor *= -1;
                    _scaleCondition = (x, y) => x > y;
                }
                else
                {

                    posYFactor *= -1;
                    _posCondition = (x, y) => x > y;

                    resizeFactor *= 4;
                    _scaleCondition = (x, y) => x < y;
                }
                break;
            case E_ICON_POSITION.MIDDLE:

                limitY = 0.0f;
                limitScale = 0.9f;
                resizeFactor *= 1;
                _scaleCondition = (x, y) => x < y;
                if (up)
                {
                    posYFactor *= 1;
                    _posCondition = (x, y) => x < y;
                }
                else
                {
                    posYFactor *= -1; 
                    _posCondition = (x, y) => x > y;
                }
                break;
            case E_ICON_POSITION.DOWN:
                limitY = -1.1f;
                limitScale = 0.7f;                
                if (up)
                {
                    posYFactor *= 1;
                    _posCondition = (x, y) => x < y;

                    resizeFactor *= 4;
                    _scaleCondition = (x, y) => x < y;
                }
                else
                {
                    posYFactor *= -1;
                    _posCondition = (x, y) => x > y;

                    resizeFactor *= -1;
                    _scaleCondition = (x, y) => x > y;
                }
                break;
            case E_ICON_POSITION.HIDE_DOWN:
                limitY = -1.6f;
                limitScale = 0.0f;
                posYFactor *= -1;
                _posCondition = (x, y) => x > y;
                if (up)
                {
                    _scaleCondition = (x, y) => false;
                }
                else
                {
                    resizeFactor *= -4;
                    _scaleCondition = (x, y) => x > y;
                }
                break;


            default:
                _posCondition = (x, y) => false;
                _scaleCondition = (x, y) => false;
                break;
        }


        while (_posCondition(gameObject.transform.position.y, limitY) || _scaleCondition(gameObject.transform.localScale.x, limitScale))
        {
            if(_posCondition(gameObject.transform.position.y, limitY))
                gameObject.transform.position += posYFactor;
            if(_scaleCondition(gameObject.transform.localScale.x, limitScale))
                gameObject.transform.localScale += resizeFactor;
            yield return new WaitForEndOfFrame();
        }
        isCoroutineRunning = false;
    }

    public void MoveUp()
    {
        if(isCoroutineRunning) return;
        
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        switch (Position)
        {
            case E_ICON_POSITION.HIDE_UP:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, -1.6f, gameObject.transform.position.z);
                break;
            case E_ICON_POSITION.UP:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.HIDE_UP,true));
                break;
            case E_ICON_POSITION.MIDDLE:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.UP, true));
                break;
            case E_ICON_POSITION.DOWN:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.MIDDLE, true));
                break;
            case E_ICON_POSITION.HIDE_DOWN:                
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.DOWN, true));
                break;

        }
    }

    public void MoveDown()
    {
        if (isCoroutineRunning) return;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        
        switch (Position)
        {
            case E_ICON_POSITION.HIDE_UP:                
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.UP, false));
                break;
            case E_ICON_POSITION.UP:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.MIDDLE, false));
                break;
            case E_ICON_POSITION.MIDDLE:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.DOWN, false));
                break;
            case E_ICON_POSITION.DOWN:
                moveCoroutine = StartCoroutine(MoveUpWithDelay(E_ICON_POSITION.HIDE_DOWN, false));
                break;
            case E_ICON_POSITION.HIDE_DOWN:
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1.6f, gameObject.transform.position.z);                
                break;
        }
    }
}
