using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using MSProjectParser.Models;

namespace MSProjectParser.ViewModels;

/// <summary>
/// ViewModel for the main window
/// </summary>
public class MainViewModel : INotifyPropertyChanged
{
    private ObservableCollection<ProjectTask> _tasks = new ObservableCollection<ProjectTask>();
    private string _projectName = "MS Project Tasks";
    private readonly Models.MSProjectParser _parser;

    public MainViewModel()
    {
        _parser = new Models.MSProjectParser();
        LoadFileCommand = new RelayCommand(LoadFile);
        LoadSampleData();
    }

    public ObservableCollection<ProjectTask> Tasks
    {
        get => _tasks;
        set
        {
            _tasks = value;
            OnPropertyChanged();
        }
    }

    public string ProjectName
    {
        get => _projectName;
        set
        {
            _projectName = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoadFileCommand { get; }

    /// <summary>
    /// Load an XML file selected by the user
    /// </summary>
    private void LoadFile()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
            Title = "Select MS Project XML File"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            LoadXmlFile(openFileDialog.FileName);
        }
    }

    /// <summary>
    /// Load XML file from specified path
    /// </summary>
    private void LoadXmlFile(string xmlFilePath)
    {
        try
        {
            var parsedTasks = _parser.ParseXml(xmlFilePath);
            Tasks = new ObservableCollection<ProjectTask>(parsedTasks);
            
            // Update project name from file name
            ProjectName = Path.GetFileNameWithoutExtension(xmlFilePath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading XML file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Load sample MS Project XML data
    /// </summary>
    private void LoadSampleData()
    {
        try
        {
            // Get the path to the sample XML file
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var xmlFilePath = Path.Combine(baseDirectory, "Data", "SampleProject.xml");

            if (File.Exists(xmlFilePath))
            {
                LoadXmlFile(xmlFilePath);
            }
            else
            {
                MessageBox.Show($"Sample data file not found at: {xmlFilePath}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading sample data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

/// <summary>
/// Simple RelayCommand implementation for ICommand
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

    public void Execute(object? parameter) => _execute();
}
