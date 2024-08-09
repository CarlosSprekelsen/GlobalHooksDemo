using WindowsInput.Events;  // Ensure this using directive is present

public static class EventExtensions
{
    public static IEvent ToEvent(this KeyboardEvent keyEvent)
    {
        // Convert a KeyboardEvent to a suitable IEvent for replaying
        // Assuming you want to capture down and up events
        return keyEvent.State == KeyState.Down
            ? new KeyDown((KeyCode)keyEvent.Key)  // Cast may be necessary depending on the type of keyEvent.Key
            : new KeyUp((KeyCode)keyEvent.Key);
    }

    public static IEvent ToEvent(this MouseEvent mouseEvent)
    {
        // Convert a MouseEvent to a suitable IEvent for replaying
        // Example: handling left button down and up
        switch (mouseEvent.Button)
        {
            case MouseButton.Left:
                return mouseEvent.State == ButtonState.Pressed
                    ? new ButtonDown(ButtonCode.Left)
                    : new ButtonUp(ButtonCode.Left);
            default:
                return null;  // Handle other cases or return null if not applicable
        }
    }
}
