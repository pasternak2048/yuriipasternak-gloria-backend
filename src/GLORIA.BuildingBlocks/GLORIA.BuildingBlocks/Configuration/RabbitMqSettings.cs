﻿namespace GLORIA.BuildingBlocks.Configuration
{
	public class RabbitMqSettings
	{
		public string Host { get; set; } = string.Empty;

		public string Username { get; set; } = string.Empty;

		public string Password { get; set; } = string.Empty;

		public string VirtualHost { get; set; } = "/";
	}
}
