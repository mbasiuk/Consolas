using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Consolas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Tasks tasks;
        const string fileName = "consolas.xml";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tasks = new Tasks();

            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    tasks.ReadXml(fileName);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("failed to read from xml: {0}", ex.Message);
                }
            }         

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                tasks.WriteXml(fileName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("failed to write xml: {0}", ex.Message);
            }
        }

        private void editCommands_Click(object sender, RoutedEventArgs e)
        {
            editCommandsCanvas.Visibility = Visibility.Visible;
            editCommandsButton.IsEnabled = false;

            editEventTypesCanvas.Visibility = Visibility.Hidden;            
            editEventTypesButton.IsEnabled = true;

            runCommandCanvas.Visibility = Visibility.Hidden;
            runCommandButton.IsEnabled = true;

            if (dataGrid.ItemsSource == null)
            {
                try
                {
                    dataGrid.ItemsSource = tasks.Task.DefaultView;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("failed to set task itemsource: ", ex.Message);
                }
            }
        }

        private void editEventTypesButton_Click(object sender, RoutedEventArgs e)
        {
            editCommandsCanvas.Visibility = Visibility.Hidden;
            editCommandsButton.IsEnabled = true;

            editEventTypesCanvas.Visibility = Visibility.Visible;
            editEventTypesButton.IsEnabled = false;

            runCommandCanvas.Visibility = Visibility.Hidden;
            runCommandButton.IsEnabled = true;

            if (eventTypesDataGrid.ItemsSource == null)
            {
                try
                {
                    eventTypesDataGrid.ItemsSource = tasks.EventType.DefaultView;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("failed to set EventType itemsource: ", ex.Message);
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            editCommandsCanvas.Visibility = Visibility.Hidden;
            editCommandsButton.IsEnabled = true;

            editEventTypesCanvas.Visibility = Visibility.Hidden;
            editEventTypesButton.IsEnabled = true;

            runCommandCanvas.Visibility = Visibility.Visible;
            runCommandButton.IsEnabled = false;

            if(runCommandListView.ItemsSource == null)
            {
                try
                {
                    runCommandListView.ItemsSource = tasks.Task.DefaultView;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("failed to set task itemsource: ", ex.Message);
                }
            }
        }
    }
}
