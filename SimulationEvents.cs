using WindowsInput;
using WindowsInput.Events;
using System.Threading.Tasks;

public class SimulationEvents
{
    // Simulate mouse events
    public async Task SimulateMouseEvent(KeyCode mouseKeyCode, int? x = null, int? y = null, int? delta = null)
    {
        var simulate = Simulate.Events();
        
        switch (mouseKeyCode)
        {
            case KeyCode.MOUSE_LEFTDOWN:
            case KeyCode.MOUSE_LEFTUP:
            case KeyCode.MOUSE_RIGHTDOWN:
            case KeyCode.MOUSE_RIGHTUP:
                simulate = simulate.Click(mouseKeyCode);
                break;
            case KeyCode.MOUSE_MOVE:
                if (x.HasValue && y.HasValue)
                {
                    simulate = simulate.MoveTo(x.Value, y.Value);
                }
                break;
            case KeyCode.MOUSE_WHEEL:
                if (delta.HasValue)
                {
                    simulate = simulate.Scroll(delta.Value);
                }
                break;
        }

        await simulate.Invoke();
    }

    // Simulate keyboard events
    public async Task SimulateKeyboardEvent(KeyCode keyCode)
    {
        await Simulate.Events().Click(keyCode).Invoke();
    }
}
