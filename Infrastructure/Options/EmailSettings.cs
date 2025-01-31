﻿namespace Infrastructure.Options;

public class EmailSettings
{
    public required string Host { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public int Port { get; set; }
}