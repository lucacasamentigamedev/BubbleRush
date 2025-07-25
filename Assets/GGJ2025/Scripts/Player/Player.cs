using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    #region weapon
    private Weapon[] avaiableWeapons;
    private int currentIndexWeapon = 0;
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
    private Vector2 endPointerPos;
    IDraggable draggable;
    #region Mono
    private void Start() {
        //prepare first weapon
        avaiableWeapons = new Weapon[(int)EWeaponType.LAST];
        for (int i = 0; i < avaiableWeapons.Length; i++) {
            avaiableWeapons[i] = new Weapon();
            avaiableWeapons[i].prepareWeapon(weaponDatabase.GetWeaponData((EWeaponType)i));

        }
        //inputs bind
        InputManager.Player.Interact.performed += onInteract;
        InputManager.Player.ChangeWeaponForward.performed += onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed += onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed += onChangeWeaponWheel;
        InputManager.Player.Hold.started += OnHoldStarted;
        InputManager.Player.Hold.canceled += OnHoldReleased;
        LevelManager.Get().OnStartLevel += onLevelManagerStart;
        GlobalEventSystem.AddListener(EventName.ChangeWeapon, OnChangeWeapon);
    }

    private void OnHoldStarted(InputAction.CallbackContext context)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)
            return;

        startPointerPos = Touchscreen.current.primaryTouch.position.ReadValue();
#else
        if (Mouse.current == null)
                return;
        startPointerPos = Mouse.current.position.ReadValue();
#endif
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
#if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)
            return;

        endPointerPos = Touchscreen.current.primaryTouch.position.ReadValue();
#else
        if (Mouse.current == null)
            return;
        endPointerPos = Mouse.current.position.ReadValue();
#endif

        draggable.OnHoldAndRelease(endPointerPos.y > startPointerPos.y);
        draggable = null;
        holdActive = false;
    }

    private void Update() {
        MoveWeaponWithInput();
    }

    private void OnDestroy()
    {
        InputManager.Player.Interact.performed -= onInteract;
        InputManager.Player.ChangeWeaponForward.performed -= onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed -= onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed -= onChangeWeaponWheel;
        InputManager.Player.Hold.performed -= OnHoldStarted;
        InputManager.Player.Hold.canceled -= OnHoldReleased;
        GlobalEventSystem.RemoveListener(EventName.ChangeWeapon, OnChangeWeapon);
    }

    private void MoveWeaponWithInput()
    {
        #if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            currentWeaponRectElem.position = touchPosition;
        }
        #else
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            currentWeaponRectElem.position = mousePosition;
        }
        #endif
    }
    #endregion

    #region Internal Methods
    private void OnChangeWeapon(EventArgs message)
    {
        EventArgsFactory.ChangeWeaponParser(message, out int forwardChange);
        ChangeWeapon(forwardChange);
    }

    private void onChangeWeaponWheel(InputAction.CallbackContext context) {
        ChangeWeapon(context.ReadValue<Vector2>().y > 0 ? 1 : -1);
    }

    private void onLevelManagerStart(uint levelIndex) {
        currentWeapon = avaiableWeapons[0];
        currentWeaponImage = currentWeaponRectElem.GetComponent<Image>();
        currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
        foreach (Weapon weapon in avaiableWeapons) {
            if(weapon.weaponData.levelToUnlock <= levelIndex && !weapon.weaponData.IsUnlocked) {
                weapon.weaponData.IsUnlocked = true;
            }
        }
    }

    private void onChangeWeaponBackward(InputAction.CallbackContext context) {
        ChangeWeapon(-1);
    }

    private void onChangeWeaponForward(InputAction.CallbackContext context) {
        ChangeWeapon(1);
    }

    private void ChangeWeapon(int forward) {
        currentIndexWeapon += forward;
        if (currentIndexWeapon > avaiableWeapons.Length -1)
            currentIndexWeapon = 0;
        else if(currentIndexWeapon < 0)
            currentIndexWeapon = avaiableWeapons.Length -1;
        if (avaiableWeapons[currentIndexWeapon].weaponData != null 
            && avaiableWeapons[currentIndexWeapon].weaponData.IsUnlocked) {

            if(currentWeapon.weaponData == avaiableWeapons[currentIndexWeapon].weaponData) {
                AudioManager.PlayOneShotSound("BubbleToolChange", new FMODParameter[] {
                    new FMODParameter("TOOL_CHANGE", 1.0f)
                });
            } else {
                AudioManager.PlayOneShotSound("BubbleToolChange", new FMODParameter[] {
                    new FMODParameter("TOOL_CHANGE", 0.0f)
                });
            }

            currentWeapon = avaiableWeapons[currentIndexWeapon];
            currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
            return;
        }
        ChangeWeapon(forward);
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
        Vector2 inputPosition;

#if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.isPressed)
            return;

        inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
#else
        if (Mouse.current == null)
            return;

        inputPosition = Mouse.current.position.ReadValue();
#endif
        
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null) {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null) {
                clickable.OnClick(worldPoint, currentWeapon.weaponData.weaponType, currentWeapon.weaponData.damage, currentWeapon.weaponData.area);
            }
        }
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
