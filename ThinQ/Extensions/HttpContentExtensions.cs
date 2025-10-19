using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThinQ.Extensions;

public static class HttpContentExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Reads the JSON content from the HTTP response and deserializes it to the specified type.
    /// Throws an exception if the content cannot be deserialized or if the status code is not successful.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON content to.</typeparam>
    /// <param name="response">The HTTP response message.</param>
    /// <returns>The deserialized content.</returns>
    /// <exception cref="HttpRequestException">Thrown if the status code is not successful or if deserialization fails.</exception>
    public static async Task<T> ReadContentFromJsonOrThrowAsync<T>(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            return JsonSerializer.Deserialize<T>(content, JsonSerializerOptions)
                   ?? throw new Exception($"Status code: {response.StatusCode}. Message: {content}");
        }
        catch (Exception)
        {
            throw new Exception($"Status code: {response.StatusCode}. Message: {content}");
        }
    }

    public static async Task<DeviceCapabilities> ReadCapabilitiesFromJsonOrThrowAsync(this HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            var jsonDocument = JsonDocument.Parse(content);
            var root = jsonDocument.RootElement;

            if (root.TryGetProperty("response", out var responseElement) &&
                responseElement.TryGetProperty("property", out var propertyElement))
            {
                var capabilities = new DeviceCapabilities
                {
                    Capabilities = ParseCapabilities(propertyElement)
                };
                return capabilities;
            }

            throw new Exception($"Status code: {response.StatusCode}. Message: {content}");
        }
        catch (Exception)
        {
            throw new Exception($"Status code: {response.StatusCode}. Message: {content}");
        }
    }

    private static IEnumerable<Capability> ParseCapabilities(JsonElement element)
    {
        foreach (var property in element.EnumerateObject())
        {
            if (property.IsLeafNode())
            {
                var leafCapability = new Capability
                {
                    Name = property.Name,
                    Type = property.Value.GetProperty("type").GetString(),
                    Mode = property.Value.GetProperty("mode").ToString(),
                    Value = property.Value.TryGetProperty("value", out var valueElement)
                        ? valueElement.ToString()
                        : null
                };
                yield return leafCapability;
                continue;
            }

            var capability = new Capability
            {
                Name = property.Name,
                SubCapabilities = ParseCapabilities(property.Value)
            };


            yield return capability;
        }
    }

    private static bool IsLeafNode(this JsonProperty element) =>
        element.Value.ValueKind == JsonValueKind.Object &&
        element.Value.TryGetProperty("type", out _) &&
        element.Value.TryGetProperty("mode", out _);
}

public class DeviceCapabilities
{
    public IEnumerable<Capability> Capabilities { get; set; }
}

public class Capability
{
    public required string Name { get; set; }
    public IEnumerable<Capability> SubCapabilities { get; set; } = new List<Capability>();

    public string? Type { get; set; }
    public string? Mode { get; set; }
    public string? Value { get; set; }
    public bool IsLeafNode => !SubCapabilities.Any();
}


/*{
  "messageId" : "fNvdZ1brTn-wWKUlWGoSVw",
  "timestamp" : "2025-10-12T20:30:28.099824",
  "response" : {
    "property" : {
      "runState" : {
        "currentState" : {
          "type" : "enum",
          "mode" : [ "r" ],
          "value" : {
            "r" : [ "NORMAL", "ERROR" ]
          }
        }
      },
      "airConJobMode" : {
        "currentJobMode" : {
          "type" : "enum",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ "AIR_DRY", "COOL", "AUTO", "FAN", "HEAT" ],
            "w" : [ "AIR_DRY", "COOL", "AUTO", "FAN", "HEAT" ]
          }
        }
      },
      "operation" : {
        "airConOperationMode" : {
          "type" : "enum",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ "POWER_OFF", "POWER_ON" ],
            "w" : [ "POWER_OFF", "POWER_ON" ]
          }
        }
      },
      "powerSave" : {
        "powerSaveEnabled" : {
          "type" : "boolean",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ false, true ],
            "w" : [ false, true ]
          }
        }
      },
      "temperature" : {
        "currentTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "unit" : {
          "type" : "enum",
          "mode" : [ "r" ],
          "value" : {
            "r" : [ "C" ]
          }
        },
        "minTargetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "maxTargetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "heatTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 30,
              "min" : 16,
              "step" : 1
            }
          }
        },
        "coolTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 30,
              "min" : 18,
              "step" : 1
            }
          }
        },
        "autoTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 30,
              "min" : 18,
              "step" : 1
            }
          }
        },
        "targetTemperature" : {
          "type" : "range",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : {
              "max" : 30,
              "min" : 16,
              "step" : 1
            },
            "w" : {
              "max" : 30,
              "min" : 18,
              "step" : 1
            }
          }
        }
      },
      "temperatureInUnits" : [ {
        "currentTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "targetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "minTargetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "maxTargetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "coolTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 30,
              "min" : 18,
              "step" : 1
            }
          }
        },
        "heatTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 30,
              "min" : 16,
              "step" : 1
            }
          }
        },
        "autoTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 30,
              "min" : 18,
              "step" : 1
            }
          }
        },
        "unit" : "C"
      }, {
        "currentTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "targetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "minTargetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "maxTargetTemperature" : {
          "type" : "number",
          "mode" : [ "r" ]
        },
        "coolTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 86,
              "min" : 64,
              "step" : 2
            }
          }
        },
        "heatTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 86,
              "min" : 60,
              "step" : 2
            }
          }
        },
        "autoTargetTemperature" : {
          "type" : "range",
          "mode" : [ "w" ],
          "value" : {
            "w" : {
              "max" : 86,
              "min" : 64,
              "step" : 2
            }
          }
        },
        "unit" : "F"
      } ],
      "airFlow" : {
        "windStrength" : {
          "type" : "enum",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ "LOW", "AUTO", "MID", "HIGH" ],
            "w" : [ "LOW", "AUTO", "MID", "HIGH" ]
          }
        },
        "windStrengthDetail" : {
          "type" : "enum",
          "mode" : [ "r" ],
          "value" : {
            "r" : [ "NATURE" ]
          }
        }
      },
      "windDirection" : {
        "rotateUpDown" : {
          "type" : "boolean",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ true, false ],
            "w" : [ true, false ]
          }
        },
        "rotateLeftRight" : {
          "type" : "boolean",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ true, false ],
            "w" : [ true, false ]
          }
        }
      },
      "timer" : {
        "relativeHourToStop" : {
          "type" : "number",
          "mode" : [ "r", "w" ]
        },
        "relativeMinuteToStop" : {
          "type" : "number",
          "mode" : [ "r", "w" ]
        },
        "relativeStopTimer" : {
          "type" : "enum",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ "SET", "UNSET" ],
            "w" : [ "UNSET" ]
          }
        },
        "relativeHourToStart" : {
          "type" : "number",
          "mode" : [ "r", "w" ]
        },
        "relativeMinuteToStart" : {
          "type" : "number",
          "mode" : [ "r", "w" ]
        },
        "relativeStartTimer" : {
          "type" : "enum",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ "SET", "UNSET" ],
            "w" : [ "UNSET" ]
          }
        }
      },
      "sleepTimer" : {
        "relativeHourToStop" : {
          "type" : "number",
          "mode" : [ "r", "w" ]
        },
        "relativeMinuteToStop" : {
          "type" : "number",
          "mode" : [ "r", "w" ]
        },
        "relativeStopTimer" : {
          "type" : "enum",
          "mode" : [ "r", "w" ],
          "value" : {
            "r" : [ "SET", "UNSET" ],
            "w" : [ "UNSET" ]
          }
        }
      }
    },
    "notification" : {
      "push" : [ "WATER_IS_FULL" ]
    }
  }
}*/