﻿using System.Net;

namespace MagicVilla_VillaAPI.Models;

public class APIResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public IEnumerable<string>? ErrorMessages { get; set; }
    public object? Result { get; set; }
}
