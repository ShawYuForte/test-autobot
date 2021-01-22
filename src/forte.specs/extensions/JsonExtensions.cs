using System;
using Newtonsoft.Json.Linq;

namespace forte.extensions
{
    /// <summary>
    /// Contains the extension methods for JSON related operations
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Queries the object as JSON.
        /// </summary>
        /// <param name="jsonObject">The object to query.</param>
        /// <param name="jsonPath">The JSON path.</param>
        /// <returns>The JSON token or exception if ot is not found</returns>
        public static JToken QueryJson(this object jsonObject, params string[] jsonPath)
        {
            return jsonObject.QueryJsonToken(true, jsonPath);
        }

        /// <summary>
        /// Queries the object as JSON.
        /// </summary>
        /// <param name="jsonObject">The object to query.</param>
        /// <param name="jsonPath">The JSON path.</param>
        /// <returns>The T value or exception if ot is not found</returns>
        public static T QueryJson<T>(this object jsonObject, params string[] jsonPath)
        {
            var token = jsonObject.QueryJsonToken(true, jsonPath);
            return token.ToObject<T>();
        }

        /// <summary>
        /// Queries the object as JSON safe.
        /// </summary>
        /// <param name="jsonObject">The object to query.</param>
        /// <param name="jsonPath">The JSON path.</param>
        /// <returns>The JSON token or null if ot is not found</returns>
        public static JToken TryQueryJson(this object jsonObject, params string[] jsonPath)
        {
            return jsonObject.QueryJsonToken(false, jsonPath);
        }

        /// <summary>
        /// Queries the object as JSON safe.
        /// </summary>
        /// <param name="jsonObject">The object to query.</param>
        /// <param name="jsonPath">The JSON path.</param>
        /// <returns>The T value or null if ot is not found</returns>
        public static T TryQueryJson<T>(this object jsonObject, params string[] jsonPath)
        {
            var token = jsonObject.QueryJsonToken(false, jsonPath);
            return token == null ? default(T) : token.ToObject<T>();
        }

        private static JToken QueryJsonToken(this object jsonObject, bool isMandatory, params string[] jsonPath)
        {
            const string separator = " -> ";

            if (jsonObject == null)
            {
                var path = string.Join(separator, jsonPath ?? new string[0]);
                throw new NullReferenceException($"Can not perform JSON query '{path}' as the object is null.");
            }

            // Convert everything to JToken, handle separatelly strings and JTokens
            var stringJson = jsonObject as string;
            var json = (stringJson != null)
                ? JToken.Parse(stringJson)
                : (jsonObject as JToken) ?? JObject.FromObject(jsonObject);

            var token = json;
            var currentPath = string.Empty;

            if (jsonPath != null)
            {
                foreach (var level in jsonPath)
                {
                    currentPath += level + separator;
                    token = token[level];
                    if (token == null)
                    {
                        break;
                    }
                }
            }

            if (isMandatory && token == null)
            {
                throw new Exception($"Can not find path '{currentPath}' in JSON object: {json}");
            }

            return token;
        }
    }
}
