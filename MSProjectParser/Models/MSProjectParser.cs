using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MSProjectParser.Models;

/// <summary>
/// Service to parse MS Project XML files
/// </summary>
public class MSProjectParser
{
    private readonly XNamespace _namespace = "http://schemas.microsoft.com/project";

    /// <summary>
    /// Parse MS Project XML file and return a hierarchical list of tasks
    /// </summary>
    public List<ProjectTask> ParseXml(string xmlFilePath)
    {
        var doc = XDocument.Load(xmlFilePath);
        var taskElements = doc.Root?.Element(_namespace + "Tasks")?.Elements(_namespace + "Task");

        if (taskElements == null)
            return new List<ProjectTask>();

        var allTasks = taskElements.Select(ParseTask).ToList();
        return BuildHierarchy(allTasks);
    }

    /// <summary>
    /// Parse a single task element
    /// </summary>
    private ProjectTask ParseTask(XElement taskElement)
    {
        var task = new ProjectTask
        {
            UID = ParseInt(taskElement.Element(_namespace + "UID")),
            ID = ParseInt(taskElement.Element(_namespace + "ID")),
            Name = taskElement.Element(_namespace + "Name")?.Value ?? string.Empty,
            OutlineLevel = ParseInt(taskElement.Element(_namespace + "OutlineLevel")),
            Start = ParseDateTime(taskElement.Element(_namespace + "Start")),
            Finish = ParseDateTime(taskElement.Element(_namespace + "Finish")),
            Duration = taskElement.Element(_namespace + "Duration")?.Value ?? string.Empty,
            PercentComplete = ParseInt(taskElement.Element(_namespace + "PercentComplete")),
            CommitmentType = ParseInt(taskElement.Element(_namespace + "CommitmentType"))
        };

        return task;
    }

    /// <summary>
    /// Build hierarchical structure based on OutlineLevel
    /// </summary>
    private List<ProjectTask> BuildHierarchy(List<ProjectTask> tasks)
    {
        var rootTasks = new List<ProjectTask>();
        var taskStack = new Stack<ProjectTask>();

        foreach (var task in tasks)
        {
            // Pop tasks from stack until we find the parent level
            while (taskStack.Count > 0 && taskStack.Peek().OutlineLevel >= task.OutlineLevel)
            {
                taskStack.Pop();
            }

            if (taskStack.Count == 0)
            {
                // This is a root level task
                rootTasks.Add(task);
            }
            else
            {
                // This is a child of the task at the top of the stack
                taskStack.Peek().Children.Add(task);
            }

            taskStack.Push(task);
        }

        return rootTasks;
    }

    private int ParseInt(XElement? element)
    {
        if (element == null || string.IsNullOrWhiteSpace(element.Value))
            return 0;

        return int.TryParse(element.Value, out var result) ? result : 0;
    }

    private DateTime ParseDateTime(XElement? element)
    {
        if (element == null || string.IsNullOrWhiteSpace(element.Value))
            return DateTime.MinValue;

        return DateTime.TryParse(element.Value, out var result) ? result : DateTime.MinValue;
    }
}
