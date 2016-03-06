﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

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
            catch {
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
           
            if (dataGrid.ItemsSource == null)
            {
                try
                {
                    dataGrid.ItemsSource = tasks.Task.DefaultView;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("failed to set task itemsource: ", ex.Message);
                }
            }
        }



        private void runComnands_click(object sender, RoutedEventArgs e)
        {
            HideAllCanvasContainer();

            runCommandCanvas.Visibility = Visibility.Visible;
            runCommandButton.IsEnabled = false;

            if (runCommandListView.ItemsSource == null)
            {
                try
                {
                    runCommandListView.ItemsSource = tasks.Task.DefaultView;
                }
                catch (Exception ex)
                {
                    Trace.TraceError("failed to set task itemsource: ", ex.Message);
                }
            }
        }

        private void HideAllCanvasContainer()
        {
            editCommandsCanvas.Visibility = Visibility.Hidden;
            viewLogsCanvas.Visibility = Visibility.Hidden;
            runCommandCanvas.Visibility = Visibility.Hidden;
            editCommandsButton.IsEnabled = true;
            runCommandButton.IsEnabled = true;
            viewLogsButton.IsEnabled = true;
        }      

        private void viewLogsButton_Click(object sender, RoutedEventArgs e)
        {
            HideAllCanvasContainer();
            viewLogsCanvas.Visibility = Visibility.Visible;
            viewLogsButton.IsEnabled = false;
            if(viewLogsDataGrid.ItemsSource == null)
            {
                try
                {
                    viewLogsDataGrid.ItemsSource = events.Event.DefaultView;
                }
                catch(Exception ex)
                {
                    events.AddApplicationLog(EventTypes.ApplicationError, ex.Message);
                    Trace.TraceError(ex.Message);
                }
            }
        }
    }
}
