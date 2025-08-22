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
        LevelManager.Get().OnStartLevel += onStartLevel;
        LevelManager.Get().OnStartEndlessLevel += onStartEndlessLevel;
        GlobalEventSystem.AddListener(EventName.ChangeWeapon, OnChangeWeapon);
    }

    private void Update() {
        currentWeaponRectElem.position = GetPointerPosition();
    }

    private void OnDestroy()
    {
        InputManager.Player.Interact.performed -= onInteract;
        InputManager.Player.ChangeWeaponForward.performed -= onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed -= onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed -= onChangeWeaponWheel;
        GlobalEventSystem.RemoveListener(EventName.ChangeWeapon, OnChangeWeapon);

    }

    #endregion

    #region InputCallback
    private void onChangeWeaponWheel(InputAction.CallbackContext context) {
        ChangeWeapon(context.ReadValue<Vector2>().y > 0 ? 1 : -1);
    }
    private void onChangeWeaponBackward(InputAction.CallbackContext context) {
        ChangeWeapon(-1);
    }
    private void onChangeWeaponForward(InputAction.CallbackContext context) {
        ChangeWeapon(1);
    }
    private void onInteract(InputAction.CallbackContext cc)
    {
        Interact();
    }
    #endregion

    private void OnChangeWeapon(EventArgs message)
    {
        EventArgsFactory.ChangeWeaponParser(message, out int forwardChange);
        ChangeWeapon(forwardChange);
    }
    private void onStartLevel(uint levelIndex) {
        currentWeapon = avaiableWeapons[0];
        currentWeaponImage = currentWeaponRectElem.GetComponent<Image>();
        currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
        foreach (Weapon weapon in avaiableWeapons) {
            if(weapon.weaponData.levelToUnlock <= levelIndex && !weapon.weaponData.IsUnlocked) {
                weapon.weaponData.IsUnlocked = true;
            }
        }
    }

    private void onStartEndlessLevel()
    {
        //Unlock all weapons 
        currentWeapon = avaiableWeapons[0];
        currentWeaponImage = currentWeaponRectElem.GetComponent<Image>();
        currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
        foreach (Weapon weapon in avaiableWeapons)
        {
            weapon.weaponData.IsUnlocked = true;
        }
    }



    #region Internal Methods
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
    private void Interact()
    {
        //Debug.Log("ON INTERACT CALLED " + currentWeapon.weaponData.weaponType.ToString());
        switch (currentWeapon.weaponData.weaponType)
        {
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

        Vector2 inputPosition = GetPointerPosition();
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hit.collider != null)
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null)
            {
                clickable.OnClick(worldPoint, currentWeapon.weaponData.weaponType, currentWeapon.weaponData.damage, currentWeapon.weaponData.area);
            }
        }
    }
    private Vector2 GetPointerPosition()
    {
    #if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        return Vector2.zero;
    #else
        if (Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }
        return Vector2.zero;
    #endif
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
