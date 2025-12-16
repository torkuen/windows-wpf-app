using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
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
                var parsedTasks = _parser.ParseXml(xmlFilePath);
                Tasks = new ObservableCollection<ProjectTask>(parsedTasks);
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
