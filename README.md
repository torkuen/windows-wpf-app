# MS Project Parser - WPF Application

A Windows Presentation Foundation (WPF) application that reads and displays MS Project XML files in a hierarchical TreeView structure.

## Features

- **XML Parsing**: Reads MS Project XML export files
- **Hierarchical Display**: Shows tasks in a TreeView with parent-child relationships based on OutlineLevel
- **Data Binding**: Full MVVM pattern implementation with INotifyPropertyChanged support
- **Rich UI**: Displays task information including:
  - Task names with icons
  - Start and finish dates
  - Duration
  - Progress bars showing completion percentage
  - Color-coded progress indicators (Green: 100%, Red: 0%)

## Project Structure

```
MSProjectParser/
├── Models/
│   ├── ProjectTask.cs           # Data model for MS Project tasks
│   └── MSProjectParser.cs       # XML parser service
├── ViewModels/
│   └── MainViewModel.cs         # ViewModel with data binding
├── Data/
│   └── SampleProject.xml        # Sample MS Project XML file
├── MainWindow.xaml              # Main UI with TreeView
├── MainWindow.xaml.cs           # Code-behind
├── App.xaml                     # Application resources
└── MSProjectParser.csproj       # Project file
```

## Requirements

- .NET 8.0 SDK or later
- Windows operating system (for running WPF applications)
- Visual Studio 2022 or JetBrains Rider (recommended for development)

## Building the Application

### Using Command Line

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run --project MSProjectParser/MSProjectParser.csproj
```

### Using Visual Studio

1. Open `MSProjectParser.sln`
2. Build the solution (Ctrl+Shift+B)
3. Run the application (F5)

## How It Works

### 1. XML Structure

The application expects MS Project XML files with the following structure:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<Project xmlns="http://schemas.microsoft.com/project">
  <Name>Sample Project</Name>
  <Tasks>
    <Task>
      <UID>1</UID>
      <ID>1</ID>
      <Name>Task Name</Name>
      <OutlineLevel>1</OutlineLevel>
      <Start>2024-01-01T08:00:00</Start>
      <Finish>2024-01-15T17:00:00</Finish>
      <Duration>PT80H0M0S</Duration>
      <PercentComplete>75</PercentComplete>
    </Task>
    <!-- More tasks... -->
  </Tasks>
</Project>
```

### 2. Hierarchical Structure

Tasks are organized hierarchically based on their `OutlineLevel`:
- **Level 1**: Root-level tasks (no parent)
- **Level 2**: Subtasks of Level 1 tasks
- **Level 3+**: Further nested subtasks

### 3. Data Binding

The application uses the MVVM pattern:
- **Model**: `ProjectTask` - Represents task data
- **ViewModel**: `MainViewModel` - Manages data and business logic
- **View**: `MainWindow.xaml` - UI with data binding

### 4. TreeView Layout

Each task node displays:
- 📋 Task icon and name (300px width)
- Start date in MM/dd/yyyy format (120px width)
- Finish date in MM/dd/yyyy format (120px width)
- Duration indicator with clock icon (100px width)
- Progress bar with percentage (150px width)

## Customization

### Loading Your Own XML File

To load a different MS Project XML file:

1. Place your XML file in the `Data` folder
2. Update the file path in `MainViewModel.cs`:

```csharp
var xmlFilePath = Path.Combine(baseDirectory, "Data", "YourFile.xml");
```

Or modify the `LoadSampleData()` method to accept a file path parameter.

### Styling

The UI colors and styling can be customized in `MainWindow.xaml`:
- Header background: `#2C3E50` (dark blue-gray)
- Border colors: `#BDC3C7` and `#E0E0E0` (light grays)
- Footer background: `#ECF0F1` (light gray)
- Progress indicators: Green (100%), Red (0%), default for others

## Technologies Used

- **WPF**: Windows Presentation Foundation for UI
- **.NET 8.0**: Target framework
- **XAML**: Declarative UI markup
- **C#**: Programming language
- **LINQ to XML**: XML parsing
- **MVVM Pattern**: Model-View-ViewModel architecture

## License

This project is open source and available under the MIT License.

