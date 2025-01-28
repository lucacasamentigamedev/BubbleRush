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

    #region OpenUITutorial
    public static EventArgs OpenUITutorialFactory(EUITutorialType UITutorialType) {
        EventArgs message = new EventArgs();
        message.variables = new object[1];
        message.variables[0] = UITutorialType;
        return message;
    }

    public static void OpenUITutorialParser(EventArgs message, out EUITutorialType UIType) {
        UIType = (EUITutorialType)message.variables[0];
    }
    #endregion
}