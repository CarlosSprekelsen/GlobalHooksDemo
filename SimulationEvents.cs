using WindowsInput;
using WindowsInput.Events;
using System.Threading.Tasks;

public class SimulationEvents
{
    // Simulate mouse events
    public async Task SimulateMouseEvent(MouseEvent mouseEvent)
    {
        var simulate = Simulate.Events();

        switch (mouseEvent.Button)
        {
            case MouseButton.Left:
                if (mouseEvent.Action == MouseAction.Down)
                    simulate = simulate.LeftButtonDown();
                else if (mouseEvent.Action == MouseAction.Up)
                    simulate = simulate.LeftButtonUp();
                break;
            case MouseButton.Right:
                if (mouseEvent.Action == MouseAction.Down)
                    simulate = simulate.RightButtonDown();
                else if (mouseEvent.Action == MouseAction.Up)
                    simulate = simulate.RightButtonUp();
                break;
            case MouseButton.Middle:
                if (mouseEvent.Action == MouseAction.Down)
                    simulate = simulate.MiddleButtonDown();
                else if (mouseEvent.Action == MouseAction.Up)
                    simulate = simulate.MiddleButtonUp();
                break;
            // Add more cases as necessary
        }

        if (mouseEvent.X.HasValue && mouseEvent.Y.HasValue)
        {
            simulate.MoveMouseToPositionOnVirtualDesktop(mouseEvent.X.Value, mouseEvent.Y.Value);
        }

        await simulate.Invoke();
    }

    // Simulate keyboard events
    public async Task SimulateKeyboardEvent(KeyCode keyCode)
    {
        await Simulate.Events().Click(keyCode).Invoke();
    }
}

// Define MouseEvent struct or class as needed
public struct MouseEvent
{
    public MouseButton Button;
    public MouseAction Action;
    public int? X;
    public int? Y;
    public int? Delta;  // For scroll events if necessary
}

public enum MouseButton
{
    Left,
    Right,
    Middle
}

public enum MouseAction
{
    Down,
    Up,
    Move,
    Scroll
}
