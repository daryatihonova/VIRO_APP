using System;
using System.Collections.Generic;

namespace VIRO_APP.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateTime Birthday { get; set; }

    public string Education { get; set; } = null!;

    public string Post { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    public string FullName => $"{LastName} {FirstName} {Patronymic}";


}


