using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    #region weapon
    private Weapon currentWeapon;
    [SerializeField]
    private WeaponsDatabase weaponDatabase;
    [SerializeField]
    private RectTransform currentWeaponRectElem;
    private Image currentWeaponImage;
    private Coroutine coroutineDeleay;
    #endregion


    private bool holdActive = false;
    private Vector2 startPointerPos;
    IDraggable draggable;



    private Vector2 currentPointerPos = Vector2.zero;

    #region Mono
    private void Start() {
        
        //inputs bind
        InputManager.Player.Interact.performed += onInteract;
        InputManager.Player.ChangeWeaponForward.performed += onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed += onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed += onChangeWeaponWheel;
        InputManager.Player.Hold.started += OnHoldStarted;
        InputManager.Player.Hold.canceled += OnHoldReleased;
        
        WeaponManager.Get().OnStartWeaponLevel +=OnStartWeaponLevel;
        WeaponManager.Get().OnChangeWeapon += OnChangeWeapon;

        GlobalEventSystem.AddListener(EventName.ChangeWeapon, OnChangeWeapon);

    }

    
    private void Update()
    {
        MoveWeaponWithInput();
    }

    private void OnDisable()
    {
        Debug.Log("Player Disable");
        WeaponManager.Get().OnStartWeaponLevel -= OnStartWeaponLevel;
        WeaponManager.Get().OnChangeWeapon -= OnChangeWeapon;
    }

    private void OnDestroy()
    {
        Debug.Log("Player Destroy");
        InputManager.Player.Interact.performed -= onInteract;
        InputManager.Player.ChangeWeaponForward.performed -= onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed -= onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed -= onChangeWeaponWheel;
        InputManager.Player.Hold.performed -= OnHoldStarted;
        InputManager.Player.Hold.canceled -= OnHoldReleased;

        GlobalEventSystem.RemoveListener(EventName.ChangeWeapon, OnChangeWeapon);
    }
    #endregion

    #region CallbackInput
    private void OnHoldStarted(InputAction.CallbackContext context)
    {
        startPointerPos = GetPointerPos();
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(startPointerPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null)
        {
            draggable = hit.collider.GetComponent<IDraggable>();
            if (draggable != null)
            {
                holdActive = true;
            }
        }
    }
    private void OnHoldReleased(InputAction.CallbackContext context)
    {
        if (!holdActive) return;
        Vector2 endPointerPos = GetPointerPos();

        //draggable.OnHoldAndRelease(endPointerPos.y > startPointerPos.y);

        ChangeWeapon(endPointerPos.y > startPointerPos.y ? -1 : 1);

        draggable = null;
        holdActive = false;
    }
    private void onChangeWeaponWheel(InputAction.CallbackContext context) {
        ChangeWeapon(context.ReadValue<Vector2>().y > 0 ? 1 : -1);
    }
    private void onChangeWeaponBackward(InputAction.CallbackContext context) {
        ChangeWeapon(-1);
    }
    private void onChangeWeaponForward(InputAction.CallbackContext context) {
        ChangeWeapon(1);
    }
    #endregion

    #region Global event system
    private void OnChangeWeapon(EventArgs message)
    {
        EventArgsFactory.ChangeWeaponParser(message, out int forwardChange);
        ChangeWeapon(forwardChange);
    }
    #endregion


    #region Callbacks
    private void OnStartWeaponLevel()
    {
        currentWeapon = WeaponManager.Get().CurrentWeapon;
        currentWeaponImage = currentWeaponRectElem.GetComponent<Image>();
        currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
    }

    void onInteract(InputAction.CallbackContext cc)
    {
        //Debug.Log("ON INTERACT CALLED " + currentWeapon.weaponData.weaponType.ToString());
        switch (currentWeapon.weaponData.weaponType) {
            case EWeaponType.Chisel:
                AudioManager.PlayOneShotSound("BubbleTool", new FMODParameter[] {
                    new FMODParameter("BUBBLE_TOOL", 1.0f)
                });
                break;
            case EWeaponType.ToyHammer:
                AudioManager.PlayOneShotSound("BubbleTool", new FMODParameter[] {
                    new FMODParameter("BUBBLE_TOOL", 2.0f)
                });
                break;
            case EWeaponType.WireCutter:
                AudioManager.PlayOneShotSound("BubbleTool", new FMODParameter[] {
                    new FMODParameter("BUBBLE_TOOL", 3.0f)
                });
                break;
        }

        if (coroutineDeleay != null)
        {
            StopCoroutine(coroutineDeleay);
        }
        coroutineDeleay = StartCoroutine(ChangeSpriteWithDelay());

        Vector3 screenPoint = InputManager.Player_Mouse_Position;
        screenPoint.z = 10;
        
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(GetPointerPos());
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null) {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null) {
                clickable.OnClick(worldPoint, currentWeapon.weaponData.weaponType, currentWeapon.weaponData.damage, currentWeapon.weaponData.area);
            }
        }
    }
    #endregion

    #region Private Methods
    private void MoveWeaponWithInput()
    {
        currentWeaponRectElem.position = GetPointerPos();
    }
    private void ChangeWeapon(int forward) {
        WeaponManager.Get().ChangeWeapon(forward);
    }

    private void OnChangeWeapon(int useless)
    {
        currentWeapon = WeaponManager.Get().CurrentWeapon;
        currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
        AudioManager.PlayOneShotSound("BubbleToolChange", new FMODParameter[] {
                    new FMODParameter("TOOL_CHANGE", 1.0f)
                });
    }
    private Vector2 GetPointerPos()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)return currentPointerPos;
         currentPointerPos = Touchscreen.current.primaryTouch.position.ReadValue();        
#else
        if (Mouse.current == null) return currentPointerPos;
        currentPointerPos = Mouse.current.position.ReadValue();
#endif
        return currentPointerPos;

    }
    #endregion


    #region Coroutine
    private IEnumerator ChangeSpriteWithDelay() {
        currentWeaponImage.sprite = currentWeapon.weaponData.postInteract;
        yield return new WaitForSeconds(0.15f);
        currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
    }
    #endregion
}
