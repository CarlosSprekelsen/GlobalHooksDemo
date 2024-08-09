using WindowsInput.Events.Sources;
using WindowsInput.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class EventRecorder
{
    private IKeyboardEventSource keyboardSource;
    private IMouseEventSource mouseSource;
    public List<(IEvent Event, TimeSpan Delay)> RecordedEvents { get; private set; } = new List<(IEvent, TimeSpan)>();
    private Stopwatch stopwatch = new Stopwatch();

    public EventRecorder()
    {
        keyboardSource = Capture.Global.KeyboardAsync();
        mouseSource = Capture.Global.MouseAsync();

        keyboardSource.KeyEvent += (sender, args) =>
        {
            if (stopwatch.IsRunning)
            {
                // Create the event from the data and record it with the delay
                var keyboardEvent = args.ToEvent();  // Assuming a conversion method exists
                RecordedEvents.Add((keyboardEvent, stopwatch.Elapsed));
                stopwatch.Restart();
            }
        };

        mouseSource.MouseEvent += (sender, args) =>
        {
            if (stopwatch.IsRunning)
            {
                // Create the event from the data and record it with the delay
                var mouseEvent = args.ToEvent();  // Assuming a conversion method exists
                RecordedEvents.Add((mouseEvent, stopwatch.Elapsed));
                stopwatch.Restart();
            }
        };
    }

    public void StartRecording()
    {
        stopwatch.Start();
        keyboardSource.Enabled = true;
        mouseSource.Enabled = true;
    }

    public void StopRecording()
    {
        keyboardSource.Enabled = false;
        mouseSource.Enabled = false;
        stopwatch.Stop();
        stopwatch.Reset();
    }
}
