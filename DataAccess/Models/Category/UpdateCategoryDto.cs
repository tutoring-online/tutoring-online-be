﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.Category;

public class UpdateCategoryDto
{
    public string? Name { get; init; }
    public string? Description { get; init; }

    public int? Type { get; init; }
    public int? Status { get; init; }
}
