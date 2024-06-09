using System;
using System.Collections.Generic;

namespace VIRO_APP.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string AudiencyCategory { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int NumberOfHourse { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime FinishDate { get; set; }

    public int NumberOfPeople { get; set; }

    public string CourseSupervisor { get; set; } = null!;

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
