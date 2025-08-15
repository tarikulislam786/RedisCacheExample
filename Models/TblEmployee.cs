using System;
using System.Collections.Generic;

namespace RedisCacheExample.Models;

public partial class TblEmployee
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? EmpName { get; set; }

    public string? Designation { get; set; }

    public DateOnly? CreatedDate { get; set; }
}
