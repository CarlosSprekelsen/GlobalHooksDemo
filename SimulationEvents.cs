using WindowsInput;
using WindowsInput.Events;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class SimulationEvents
{
    // Method to simulate mouse events
    public async Task SimulateMouseEvent(IntPtr wParam, IntPtr lParam)
    {
        var mouseEventFlags = (MOUSEEVENTF)wParam.ToInt32();
        var coordinates = ExtractCoordinates(lParam); // Extract coordinates for mouse move

        switch (mouseEventFlags)
        {
            case MOUSEEVENTF.LEFTDOWN:
                await Simulate.Events().Click(ButtonCode.Left).Invoke();
                break;
            case MOUSEEVENTF.LEFTUP:
                await Simulate.Events().Release(ButtonCode.Left).Invoke();
                break;
            case MOUSEEVENTF.RIGHTDOWN:
                await Simulate.Events().Click(ButtonCode.Right).Invoke();
                break;
            case MOUSEEVENTF.RIGHTUP:
                await Simulate.Events().Release(ButtonCode.Right).Invoke();
                break;
            case MOUSEEVENTF.MIDDLEUP:
                await Simulate.Events().Release(ButtonCode.Middle).Invoke();
                break;
            case MOUSEEVENTF.MIDDLEDOWN:
                await Simulate.Events().Click(ButtonCode.Middle).Invoke();
                break;
            case MOUSEEVENTF.MOUSEMOVE:
                // Move the mouse to the specified coordinates
                await Simulate.Events().MoveTo(coordinates.x, coordinates.y).Invoke();
                break;
            case MOUSEEVENTF.WHEEL:
                // Simulate mouse wheel movement
                var delta = wParam.ToInt32();
                await Simulate.Events().Scroll(delta).Invoke();
                break;
        }
    }

    // Method to simulate keyboard events
    public async Task SimulateKeyboardEvent(IntPtr wParam, IntPtr lParam)
    {
        var keyCode = (VirtualKeyCode)wParam.ToInt32();
        await Simulate.Events().KeyPress(keyCode).Invoke();
    }

    // Utility to extract coordinates from lParam
    private (int x, int y) ExtractCoordinates(IntPtr lParam)
    {
        int x = lParam.ToInt32() & 0xFFFF; // Lower word
        int y = lParam.ToInt32() >> 16;    // Upper word
        return (x, y);
    }
}
