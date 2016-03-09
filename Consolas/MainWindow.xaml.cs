using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Consolas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Tasks tasks;
        Events events;

        const string fileName = "consolas.xml";
        const string eventsPath = "events.xml";


        public Visibility RunVisibility
        {
            get { return (Visibility)GetValue(RunVisibilityProperty); }
            set { SetValue(RunVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RunVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RunVisibilityProperty =
            DependencyProperty.Register("RunVisibility", typeof(Visibility), typeof(Window), new PropertyMetadata(Visibility.Hidden));        



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            events = new Events();

            if (File.Exists(eventsPath))
            {
                try { events.ReadXml(eventsPath); }
                catch (Exception ex)
                {
                    Trace.TraceError("failed to read from xml: {0}", ex.Message);
                }
            }

            events.AddApplicationLog(EventTypes.ApplicationStarted);


            tasks = new Tasks();

            if (File.Exists(fileName))
            {
                try
                {
                    tasks.ReadXml(fileName);
                    events.AddApplicationLog(EventTypes.ApplicationLoaded);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("failed to read from xml: {0}", ex.Message);
                    events.AddApplicationLog(EventTypes.ApplicationError, ex.Message);
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                events.AddApplicationLog(EventTypes.ApplicationEnded);
            }
            catch
            {
            }
            try
            {
                tasks.WriteXml(fileName);
            }
            catch (Exception ex)
            {
                events.AddApplicationLog(EventTypes.ApplicationError, ex.Message);
                Trace.TraceError("failed to write xml: {0}", ex.Message);
            }

            try
            {
                events.WriteXml(eventsPath);
            }
            catch (Exception ex)
            {
                Trace.TraceError("failed to write logs:", ex.Message);
            }
        }

        private void editCommands_Click(object sender, RoutedEventArgs e)
        {
            HideAllCanvasContainer();

            editCommandsCanvas.Visibility = Visibility.Visible;
            editCommandsButton.IsEnabled = false;
            events.AddApplicationLog(EventTypes.OpenTaskEditSection);

            if (dataGrid.ItemsSource == null)
            {
                try
                {
                    dataGrid.ItemsSource = tasks.Task.DefaultView;
                }
                catch (Exception ex)
                {
                    events.AddApplicationLog(EventTypes.ApplicationError, ex.Message);
                    Trace.TraceError("failed to set task itemsource: ", ex.Message);
                }
            }
        }



        private void runComnands_click(object sender, RoutedEventArgs e)
        {
            HideAllCanvasContainer();

            RunVisibility = Visibility.Visible;
            runCommandButton.IsEnabled = false;

            events.AddApplicationLog(EventTypes.OpenTaskRunSection, "running commands");

            if (runCommandListView.ItemsSource == null)
            {
                try
                {
                    runCommandListView.ItemsSource = tasks.Task.DefaultView;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("failed to set task itemsource: ", ex.Message);
                    events.AddApplicationLog(EventTypes.ApplicationError, ex.Message);
                }
            }
        }

        private void HideAllCanvasContainer()
        {
            editCommandsCanvas.Visibility = Visibility.Hidden;
            viewLogsCanvas.Visibility = Visibility.Hidden;
            RunVisibility = Visibility.Hidden;
            editCommandsButton.IsEnabled = true;
            runCommandButton.IsEnabled = true;
            viewLogsButton.IsEnabled = true;
            if(tasks != null)
            {
                tasks.AcceptChanges();
            }
            
        }

        private void viewLogsButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllCanvasContainer();
            viewLogsCanvas.Visibility = Visibility.Visible;
            viewLogsButton.IsEnabled = false;
            events.AddApplicationLog(EventTypes.OpenLogViewSection);

            if (viewLogsDataGrid.ItemsSource == null)
            {
                try
                {
                    viewLogsDataGrid.ItemsSource = events.Event.DefaultView;
                }
                catch (Exception ex)
                {
                    events.AddApplicationLog(EventTypes.ApplicationError, ex.Message);
                    Trace.TraceError(ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tasks.TaskRow task = (Tasks.TaskRow)((System.Data.DataRowView)((FrameworkElement)sender).DataContext).Row;
            events.AddTaskLog(task.ID, EventTypes.TaskStartRequested, task.Title);
            startExecutingTask(task);
        }

        private void startExecutingTask(Tasks.TaskRow task)
        {
            Process process = null;
            TextBlockErrorOutput.Text = string.Empty;
            TextBlockOutput.Text = string.Empty;
            try
            {
                process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = task.FileName,
                    WorkingDirectory = Environment.ExpandEnvironmentVariables(task.WorkingDirectory),                    
                    Arguments = task.Arguments,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true                    
                };
                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                
                process.Start();
                events.AddTaskLog(task.ID, EventTypes.TaskStarted, task.Title);
            }
            catch (Exception ex)
            {
                events.AddTaskLog(task.ID, EventTypes.TaskStartError, task.Title + ": " + ex.Message);
                TextBlockErrorOutput.Text += ex.Message;
            }
            try
            {
                string output = process.StandardOutput.ReadToEnd();
                TextBlockOutput.Text = output;
                string err = process.StandardError.ReadToEnd();
                TextBlockErrorOutput.Text = err;
                Console.WriteLine(err);

                process.WaitForExit();
                events.AddTaskLog(task.ID, EventTypes.TaskCompleted, task.Title);
            }
            catch (Exception ex)
            {
                TextBlockErrorOutput.Text += ex.Message;
                events.AddTaskLog(task.ID, EventTypes.TaskProcessError, task.Title + ": " + ex.Message);
            }

        }
    }
}
