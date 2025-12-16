using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MSProjectParser.Models;

/// <summary>
/// Represents a task from MS Project
/// </summary>
public class ProjectTask : INotifyPropertyChanged
{
    private int _uid;
    private int _id;
    private string _name = string.Empty;
    private int _outlineLevel;
    private DateTime _start;
    private DateTime _finish;
    private string _duration = string.Empty;
    private int _percentComplete;

    public int UID
    {
        get => _uid;
        set
        {
            _uid = value;
            OnPropertyChanged();
        }
    }

    public int ID
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public int OutlineLevel
    {
        get => _outlineLevel;
        set
        {
            _outlineLevel = value;
            OnPropertyChanged();
        }
    }

    public DateTime Start
    {
        get => _start;
        set
        {
            _start = value;
            OnPropertyChanged();
        }
    }

    public DateTime Finish
    {
        get => _finish;
        set
        {
            _finish = value;
            OnPropertyChanged();
        }
    }

    public string Duration
    {
        get => _duration;
        set
        {
            _duration = value;
            OnPropertyChanged();
        }
    }

    public int PercentComplete
    {
        get => _percentComplete;
        set
        {
            _percentComplete = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ProjectTask> Children { get; set; } = new ObservableCollection<ProjectTask>();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
