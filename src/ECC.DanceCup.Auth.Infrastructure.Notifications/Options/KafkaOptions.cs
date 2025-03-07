﻿namespace ECC.DanceCup.Auth.Infrastructure.Notifications.Options;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = string.Empty;

    public Topics Topics { get; set; } = new();
}

public class Topics
{
    public Topic DanceCupEvents { get; set; } = new();
}

public class Topic
{
    public string Name { get; set; } = string.Empty;
}