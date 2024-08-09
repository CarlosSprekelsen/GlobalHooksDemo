using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using WindowsInput;
using WindowsInput.Events;

public class MainForm : Form
{
    private Button btnRecord;
    private Button btnStop;
    private Button btnReplay;
    private Label lblStatus;

    private EventRecorder recorder = new EventRecorder();
    private bool isRecording = false;  // Flag to track recording status

    public MainForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        btnRecord = new Button { Text = "Record", Location = new Point(10, 10) };
        btnStop = new Button { Text = "Stop", Location = new Point(10, 40) };
        btnReplay = new Button { Text = "Replay", Location = new Point(10, 70) };
        lblStatus = new Label { Text = "Ready to record...", AutoSize = true, Location = new Point(10, 100) };

        btnRecord.Click += BtnRecord_Click;
        btnStop.Click += BtnStop_Click;
        btnReplay.Click += BtnReplay_Click;

        Controls.AddRange(new Control[] { btnRecord, btnStop, btnReplay, lblStatus });

        Text = "Mouse and Keyboard Recorder";
        Size = new Size(300, 180);
        TopMost = true;  // Keep the form always on top
    }

    private void BtnRecord_Click(object sender, EventArgs e)
    {
        recorder.StartRecording();
        isRecording = true;
        UpdateStatus("Recording started...");
    }

    private void BtnStop_Click(object sender, EventArgs e)
    {
        recorder.StopRecording();
        isRecording = false;
        UpdateStatus("Recording stopped.");
    }

    private async void BtnReplay_Click(object sender, EventArgs e)
    {
        UpdateStatus("Replaying actions...");
        await ReplayEvents(recorder.RecordedEvents);
        UpdateStatus("Replay completed.");
    }

    private async Task ReplayEvents(List<(IEvent Event, TimeSpan Delay)> events)
    {
        foreach (var (eventItem, delay) in events)
        {
            await Task.Delay(delay); // Wait according to the recorded delay
            await Simulate.Events().Add(eventItem).Invoke(); // Replay the event
        }
    }

    private void UpdateStatus(string text)
    {
        lblStatus.Text = text;
    }
}
