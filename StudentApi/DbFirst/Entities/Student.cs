using System;
using System.Collections.Generic;

namespace StudentApi.DbFirst.Entities;

public partial class Student
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;
}
