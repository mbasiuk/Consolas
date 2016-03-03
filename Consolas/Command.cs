﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Consolas
{
    [Serializable]
    [XmlType("command")]
    public class Command : IValidatableObject, IProgress<Report>, INotifyPropertyChanged, ICommand
    {
        #region Properties

        private string path;
        protected const string PathString = "WorkingDirectory";
        protected const string pathString = "path";
        [XmlAttribute(pathString)]
        [DisplayName(PathString)]
        public string WorkingDirectory
        {
            get { return path; }
            set { path = value; NotifyPropertyChanged(PathString); }
        }

        private string title;
        protected const string TitleString = "Title";
        protected const string titleString = "title";
        [Required(ErrorMessage = "Please enter a Title")]
        [XmlAttribute(titleString)]
        [DisplayName(TitleString)]
        public string Title
        {
            get { return title; }
            set { title = value; NotifyPropertyChanged(TitleString); }
        }

        private string value;
        protected const string ValueString = "FileName";
        protected const string valueString = "value";
        [XmlAttribute(valueString)]
        [Required(ErrorMessage = "Please Enter a FileName")]
        [DisplayName(ValueString)]
        public string FileName
        {
            get { return value; }
            set { this.value = value; NotifyPropertyChanged(ValueString); }
        }

        private TaskStatus status;
        private const string StatusString = "Status";
        private const string statusString = "status";
        [XmlIgnore]
        public TaskStatus Status
        {
            get { return status; }
            set { status = value; NotifyPropertyChanged(StatusString); }
        }

        public const string LastDurationString = "LastDuration";
        public const string lastDurationString = "lastDuration";
        public const string DurationString = "Duration";
        private TimeSpan lastDuration;        
        [XmlAttribute(lastDurationString)]
        [DisplayName(DurationString)]
        public TimeSpan LastDuration
        {
            get { return lastDuration; }
            set { lastDuration = value; NotifyPropertyChanged(LastDurationString); }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private
        List<Report> report;
        #endregion

        public Command()
        {
            Status = TaskStatus.Created;
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.FrameworkElement()))
            {
                FileName = "filname.exe";
                WorkingDirectory = @"C:\working direcroty";
                Status = TaskStatus.Running;
                Title = "test title";                
            }
            report = new List<Report>();
        }

        public void Report(Report value)
        {
            report.Add(value);
        }

        public bool CanExecute(object parameter)
        {
            return Status == TaskStatus.Created || Status == TaskStatus.RanToCompletion;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Stopwatch watch = new Stopwatch();

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c " + FileName,
                WorkingDirectory = WorkingDirectory,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = new Process
            {
                StartInfo = startInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, args) =>
            {
                Status = TaskStatus.Running;
                Report(new Report() { Message = args.Data, Time = watch.Elapsed, Status = Status });
            };

            process.ErrorDataReceived += (sender, args) =>
            {
                Status = TaskStatus.Running;
                Report(new Report() { Message = args.Data, Time = watch.Elapsed, Status = Status });
            };

            process.Exited += (sender, args) =>
            {
                Status = TaskStatus.RanToCompletion;
                Report(new Report() { Message = Status.ToString(), Time = watch.Elapsed, Status = Status });
                LastDuration = watch.Elapsed;                
                watch.Stop();
            };

            try
            {
                Status = TaskStatus.WaitingForActivation;
                watch.Start();
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }
            catch (Exception e)
            {
                Status = TaskStatus.Faulted;
                Report(new Report() { Message = e.Message, Time = watch.Elapsed, Status = Status });
            }
        }

    }

    public class Report
    {
        /// <summary>
        /// Message that recived from running Thread
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Time when the message is created
        /// </summary>
        public TimeSpan Time { get; set; }

        /// <summary>
        /// Status of task, when report message is created
        /// </summary>
        public TaskStatus Status { get; set; }
    }

    public class CommandCollection : ObservableCollection<Command>
    {
        public CommandCollection()
        {

        }
    }
}
