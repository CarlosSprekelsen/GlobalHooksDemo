using System.Diagnostics;  // For accurate timing

public class MainForm : Form
{
    private Button btnRecord;
    private Button btnStop;
    private Button btnReplay;
    private Label lblStatus;
    private List<EventRecord> eventRecords;
    private Stopwatch stopwatch;  // For timing events
    private bool isRecording = false;
    private SimulationEvents simulator = new SimulationEvents();

    public MainForm()
    {
        eventRecords = new List<EventRecord>();
        stopwatch = new Stopwatch();
        InitializeComponents();
         GlobalHooks.RegisterEventAction(HandleEvent);
    }

    private void InitializeComponents()
    {
        btnRecord = new Button { Text = "Record", Location = new System.Drawing.Point(10, 10) };
        btnStop = new Button { Text = "Stop", Location = new System.Drawing.Point(10, 40) };
        btnReplay = new Button { Text = "Replay", Location = new System.Drawing.Point(10, 70) };
        lblStatus = new Label { Text = "Ready to record...", AutoSize = true, Location = new System.Drawing.Point(10, 100) };

        btnRecord.Click += BtnRecord_Click;
        btnStop.Click += BtnStop_Click;
        btnReplay.Click += BtnReplay_Click;

        Controls.AddRange(new Control[] { btnRecord, btnStop, btnReplay, lblStatus });

        Text = "Mouse and Keyboard Recorder";
        Size = new System.Drawing.Size(300, 180);
        this.TopMost = true;  // Keep the form always on top
    }

private void HandleEvent(int eventType, IntPtr wParam, IntPtr lParam)
{
    // Logic to create an EventRecord and add it to eventRecords
}
    private void BtnRecord_Click(object sender, EventArgs e)
    {
        isRecording = true;
        eventRecords.Clear();
        stopwatch.Restart();
        GlobalHooks.SetHooks();
        UpdateStatus("Recording started...");
    }

    private void BtnStop_Click(object sender, EventArgs e)
    {
        isRecording = false;
        GlobalHooks.ReleaseHooks();
        stopwatch.Stop();
        UpdateStatus("Recording stopped.");
    }

    private void BtnReplay_Click(object sender, EventArgs e)
    {
        ReplayActions();
    }

private async Task ReplayActions()
{
    UpdateStatus("Replaying actions...");
    foreach (var record in eventRecords)
    {
        await Task.Delay(record.Delay);
        switch (record.EventType)
        {
            case 0: // Mouse event
                // Extract mouse parameters from record; assuming you have methods to do this
                var mouseEvent = ExtractMouseEvent(record.wParam, record.lParam);
                await simulator.SimulateMouseEvent(mouseEvent.MouseKeyCode, mouseEvent.X, mouseEvent.Y, mouseEvent.Delta);
                break;
            case 1: // Keyboard event
                // Extract key code from wParam; assuming KeyCode is compatible with IntPtr conversion
                var keyCode = (KeyCode)record.wParam.ToInt32();  // Make sure this cast is valid based on your KeyCode definitions
                await simulator.SimulateKeyboardEvent(keyCode);
                break;
        }
    }
    UpdateStatus("Replay completed.");
}


private (KeyCode MouseKeyCode, int? X, int? Y, int? Delta) ExtractMouseEvent(IntPtr wParam, IntPtr lParam)
{
    // Example extraction logic; this needs to be adapted to your specific data structure
    KeyCode mouseKeyCode = (KeyCode)wParam.ToInt32(); // Adjust based on actual data
    int? x = null, y = null; // Extract coordinates from lParam if applicable
    int? delta = null; // Extract delta for scroll from wParam or lParam if applicable

    return (mouseKeyCode, x, y, delta);
}



private void SimulateMouseEvent(IntPtr wParam, IntPtr lParam)
{
    // Use methods from an input simulation library or API calls to simulate mouse events
}

private void SimulateKeyboardEvent(IntPtr wParam, IntPtr lParam)
{
    // Use methods from an input simulation library or API calls to simulate keyboard events
}

    private void UpdateStatus(string text)
    {
        lblStatus.Text = text;
    }

    // Define a structure to store event data
    public struct EventRecord
{
    public DateTime TimeStamp;
    public int EventType; // 0 for mouse, 1 for keyboard
    public IntPtr wParam; // Stores wParam to identify the mouse button or key change
    public IntPtr lParam; // Stores lParam to identify the specifics of the event
    public int Delay; // Delay in milliseconds from the previous event
}

    public void RecordEvent(EventRecord eventRecord)
    {
        if (isRecording)
        {
            eventRecord.TimeStamp = DateTime.Now;
            if (eventRecords.Count > 0)
            {
                eventRecord.Delay = (int)(stopwatch.ElapsedMilliseconds - eventRecords[eventRecords.Count - 1].TimeStamp.Subtract(eventRecords[0].TimeStamp).TotalMilliseconds);
            }
            else
            {
                eventRecord.Delay = 0;  // First event
            }
            eventRecords.Add(eventRecord);
        }
    }
}
