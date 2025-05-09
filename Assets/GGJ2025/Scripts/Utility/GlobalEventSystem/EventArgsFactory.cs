public static class EventArgsFactory
{

    #region OpenUI
    public static EventArgs OpenUIFactory(EUIType UIType)
    {
        EventArgs message = new EventArgs();
        message.variables = new object[1];
        message.variables[0] = UIType;
        return message;
    }

    public static void OpenUIParser(EventArgs message, out EUIType UIType)
    {
        UIType = (EUIType)message.variables[0];
    }
    #endregion

    /*#region OpenUITutorial
    public static EventArgs OpenUITutorialFactory(EUITutorialType UITutorialType) {
        EventArgs message = new EventArgs();
        message.variables = new object[1];
        message.variables[0] = UITutorialType;
        return message;
    }

    public static void OpenUITutorialParser(EventArgs message, out EUITutorialType UIType) {
        UIType = (EUITutorialType)message.variables[0];
    }
    #endregion*/

    #region ChangeUILevelLabel
    public static EventArgs ChangeUILevelLabelFactory() {
        EventArgs message = new EventArgs();
        return message;
    }

    public static void ChangeUILevelLabelParser(EventArgs message) {}
    #endregion

    #region ChangeWeapon
    public static EventArgs ChangeWeaponFactory(int forward)
    {
        EventArgs message = new EventArgs();
        message.variables = new object[1];
        message.variables[0] = forward;
        return message;
    }
    public static void ChangeWeaponParser(EventArgs message, out int forward) 
    {
        forward = (int)message.variables[0];
    }
    #endregion

    #region Timer
    //StartTimer  Pluriball -> UIController
    //TimerEndend UIController -> LevelManager   ->OnFailLevel  ->(Pluriball) lvlMngRef.OnFailLevel() {peripezie}
    #region StartTimer
    public static EventArgs StartTimerFactory()
    {
        EventArgs message = new EventArgs();
        message.variables = new object[0];
        return message;
    }
    public static void StartTimerParser(EventArgs message)
    {

    }
    #endregion

    #region ModulateTimer
    public static EventArgs ModulateTimerFactory(float time)
    {
        EventArgs message = new EventArgs();
        message.variables = new object[1];
        message.variables[0] = time;
        return message;
    }
    public static void ModulateTimerParser(EventArgs message, out float time)
    {
        time = (float)message.variables[0];
    }
    #endregion

    #region TimerEnded
    public static EventArgs TimerEndedFactory()
    {
        EventArgs message = new EventArgs();
        message.variables = new object[0];
        return message;
    }
    public static void TimerEndedParser(EventArgs message)
    {

    }
    #endregion


    #endregion
}