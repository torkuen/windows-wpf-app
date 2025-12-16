# Application Architecture

## Overview
The MS Project Parser is a WPF application following the MVVM (Model-View-ViewModel) pattern.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         WPF Application                          │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                            VIEW LAYER                            │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ MainWindow.xaml                                           │  │
│  │  - TreeView with HierarchicalDataTemplate                 │  │
│  │  - Data binding to ViewModel                              │  │
│  │  - Task display: Name, Dates, Duration, Progress          │  │
│  │  - Styling: Colors, fonts, layout                         │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ↕ DataContext
┌─────────────────────────────────────────────────────────────────┐
│                        VIEWMODEL LAYER                           │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ MainViewModel.cs                                          │  │
│  │  - ObservableCollection<ProjectTask> Tasks                │  │
│  │  - INotifyPropertyChanged implementation                  │  │
│  │  - LoadSampleData() method                                │  │
│  │  - Uses MSProjectParser to load XML                       │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────────┐
│                          MODEL LAYER                             │
│  ┌────────────────────────────┐  ┌──────────────────────────┐   │
│  │ ProjectTask.cs             │  │ MSProjectParser.cs       │   │
│  │  - Properties:             │  │  - ParseXml()            │   │
│  │    * UID                   │  │  - ParseTask()           │   │
│  │    * ID                    │  │  - BuildHierarchy()      │   │
│  │    * Name                  │  │  - Uses LINQ to XML      │   │
│  │    * OutlineLevel          │  └──────────────────────────┘   │
│  │    * Start/Finish          │                                 │
│  │    * Duration              │                                 │
│  │    * PercentComplete       │                                 │
│  │    * Children (collection) │                                 │
│  │  - INotifyPropertyChanged  │                                 │
│  └────────────────────────────┘                                 │
└─────────────────────────────────────────────────────────────────┘
                              ↕
┌─────────────────────────────────────────────────────────────────┐
│                           DATA LAYER                             │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ SampleProject.xml                                         │  │
│  │  - MS Project XML format                                  │  │
│  │  - Tasks with hierarchical structure                      │  │
│  │  - Namespace: http://schemas.microsoft.com/project        │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

## Data Flow

1. **Application Startup** (App.xaml)
   - Initializes WPF application
   - Loads MainWindow

2. **ViewModel Initialization** (MainViewModel)
   - Created by XAML DataContext
   - Calls LoadSampleData()
   - Instantiates MSProjectParser

3. **XML Parsing** (MSProjectParser)
   - Reads XML file from Data folder
   - Parses each Task element
   - Builds hierarchical structure based on OutlineLevel
   - Returns List<ProjectTask>

4. **Data Binding** (MainWindow.xaml)
   - TreeView binds to Tasks property
   - HierarchicalDataTemplate binds to Children property
   - Each task's properties bind to UI elements

5. **UI Display**
   - TreeView renders hierarchical task structure
   - Progress bars show completion status
   - Dates formatted as MM/dd/yyyy
   - Auto-expands all nodes

## Key Technologies

- **XAML**: Declarative UI definition
- **Data Binding**: Two-way communication between View and ViewModel
- **ObservableCollection**: Automatic UI updates on data changes
- **INotifyPropertyChanged**: Property change notifications
- **LINQ to XML**: XML parsing and querying
- **HierarchicalDataTemplate**: TreeView item rendering

## File Organization

```
MSProjectParser/
├── Models/              # Data models and business logic
├── ViewModels/          # View models (MVVM pattern)
├── Data/                # Sample XML data
├── MainWindow.*         # Main UI window
├── App.*                # Application entry point
└── *.csproj            # Project configuration
```
