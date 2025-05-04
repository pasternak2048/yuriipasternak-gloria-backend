using System.Text.Json;

namespace BuildingBlocks.Utils
{
	public static class JsonNormalizer
	{
		public static string Normalize(JsonElement jsonElement)
		{
			return JsonSerializer.Serialize(jsonElement, GetOptions());
		}

		public static string Normalize<T>(T obj)
		{
			return JsonSerializer.Serialize(obj, GetOptions());
		}

		private static JsonSerializerOptions GetOptions()
		{
			return new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = false,
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
			};
		}
	}
}
