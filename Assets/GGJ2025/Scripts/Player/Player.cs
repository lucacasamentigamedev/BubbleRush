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
    private void Awake() {
        //prepare first weapon
        avaiableWeapons = new Weapon[(int)EWeaponType.LAST];
        for (int i = 0; i < avaiableWeapons.Length; i++) {
            avaiableWeapons[i] = new Weapon();
            avaiableWeapons[i].prepareWeapon(weaponDatabase.GetWeaponData((EWeaponType)i));

        }
        //inputs bind
        Debug.Log("Player - mi bindo agli input");
        InputManager.Player.Interact.performed += onInteract;
        InputManager.Player.ChangeWeaponForward.performed += onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed += onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed += onChangeWeaponWheel;
        LevelManager.Get().OnStart += onLevelManagerStart;
    }

    private void Update() {
        MoveWeaponWithMouse();
    }

    private void OnDestroy()
    {
        InputManager.Player.Interact.performed -= onInteract;
        InputManager.Player.ChangeWeaponForward.performed -= onChangeWeaponForward;
        InputManager.Player.ChangeWeaponBackward.performed -= onChangeWeaponBackward;
        InputManager.Player.ChangeWeaponWheel.performed -= onChangeWeaponWheel;

    }

    private void MoveWeaponWithMouse() {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        currentWeaponRectElem.position = mousePosition;
    }
    #endregion

    #region Internal Methods
    private void onChangeWeaponWheel(InputAction.CallbackContext context) {
        ChangeWeapon(context.ReadValue<Vector2>().y > 0 ? 1 : -1);
    }

    private void onLevelManagerStart() {

        currentWeapon = avaiableWeapons[0];
        currentWeaponImage = currentWeaponRectElem.GetComponent<Image>();
        //hide fingerone
        currentWeaponRectElem.gameObject.SetActive(false);

        //active fingerone or other current weapon
        InputManager.Player.Enable();
        currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
        currentWeaponRectElem.gameObject.SetActive(false);
        currentWeaponRectElem.gameObject.SetActive(true);
        //TODO: poi rimettere a false
        //Cursor.visible = false;

        uint level = LevelManager.Get().Level;

        Debug.Log("PLAYER - START LIV" + level);
        foreach (Weapon weapon in avaiableWeapons) {
            if(weapon.weaponData.levelToUnlock <= level && !weapon.weaponData.IsUnlocked) {
                weapon.weaponData.IsUnlocked = true;
                Debug.Log("PLAYER - Arma sbloccata: " + weapon.weaponData.weaponType.ToString());
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
            Debug.Log("Cambiata arma in " + currentWeapon.weaponData.weaponType.ToString());
            currentWeaponImage.sprite = currentWeapon.weaponData.preInteract;
            return;
        }
        ChangeWeapon(forward);
    }

    void onInteract(InputAction.CallbackContext cc)
    {
        Debug.Log("ON INTERACT CALLED " + currentWeapon.weaponData.weaponType.ToString());
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
        //currentWeaponImage.sprite = currentWeapon.weaponData.postInteract;

        Vector3 screenPoint = InputManager.Player_Mouse_Position;
        screenPoint.z = 10;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null) {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            if (clickable != null) {
                clickable.OnClick(mousePosition, currentWeapon.weaponData.weaponType, currentWeapon.weaponData.damage, currentWeapon.weaponData.area);
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
