using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AluraLibrary.Models;

public class CourseInformation
{
    public string Title { get; set; }
    public string Instructors { get; set; }
    public string Hours { get; set; }
    public string Description { get; set; }

    public CourseInformation(string title = "", string instructors = "", string hours = "", string description = "")
    {
        Title = title;
        Instructors = instructors;
        Hours = hours;
        Description = description;
    }
}