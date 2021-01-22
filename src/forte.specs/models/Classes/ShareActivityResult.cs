using System;
using Newtonsoft.Json;

namespace forte.models.classes
{
    public class ShareActivityResult
    {
        public bool AuthorizationRequired { get; set; }

        public string ClientId { get; set; }
    }

    public class StravaActivityDto
    {
        public long ResourceState { get; set; }

        public string Name { get; set; }

        [JsonProperty("total_elevation_gain")]
        public long TotalElevationGain { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("external_id")]
        public object ExternalId { get; set; }

        [JsonProperty("upload_id")]
        public object UploadId { get; set; }

        [JsonProperty("achievement_count")]
        public long AchievementCount { get; set; }

        [JsonProperty("kudos_count")]
        public long KudosCount { get; set; }

        [JsonProperty("comment_count")]
        public long CommentCount { get; set; }

        [JsonProperty("athlete_count")]
        public long AthleteCount { get; set; }

        [JsonProperty("photo_count")]
        public long PhotoCount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("calories")]
        public long Calories { get; set; }

        [JsonProperty("photo_metadata")]
        public PhotoMetadatum[] PhotoMetadata { get; set; }
    }

    public class Map
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("polyline")]
        public object Polyline { get; set; }

        [JsonProperty("resource_state")]
        public long ResourceState { get; set; }
    }

    public class PhotoMetadatum
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("max_size")]
        public long MaxSize { get; set; }
    }

    public class Header
    {
        [JsonProperty("Content-Type")]
        public string ContentType { get; set; }
    }

    public class StravaAuthResult
    {
        public string TokenType { get; set; }

        public string AccessToken { get; set; }
    }

    public class StravaShareRequest
    {
        public string Comment { get; set; }

        public bool? StravaAutoPosting { get; set; }

        public DateTime Date { get; set; }
    }
}
