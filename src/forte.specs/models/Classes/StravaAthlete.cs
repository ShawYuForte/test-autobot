using System;
using Newtonsoft.Json;

namespace forte.models.classes
{
    public partial class StravaAthlete
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("resource_state")]
        public long ResourceState { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("sex")]
        public string Sex { get; set; }

        [JsonProperty("premium")]
        public bool Premium { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("badge_type_id")]
        public long BadgeTypeId { get; set; }

        [JsonProperty("profile_medium")]
        public string ProfileMedium { get; set; }

        [JsonProperty("profile")]
        public string Profile { get; set; }

        [JsonProperty("friend")]
        public object Friend { get; set; }

        [JsonProperty("follower")]
        public object Follower { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("follower_count")]
        public long FollowerCount { get; set; }

        [JsonProperty("friend_count")]
        public long FriendCount { get; set; }

        [JsonProperty("mutual_friend_count")]
        public long MutualFriendCount { get; set; }

        [JsonProperty("athlete_type")]
        public long AthleteType { get; set; }

        [JsonProperty("date_preference")]
        public string DatePreference { get; set; }

        [JsonProperty("measurement_preference")]
        public string MeasurementPreference { get; set; }

        [JsonProperty("clubs")]
        public object[] Clubs { get; set; }

        [JsonProperty("ftp")]
        public object Ftp { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

        [JsonProperty("bikes")]
        public Bike[] Bikes { get; set; }

        [JsonProperty("shoes")]
        public Bike[] Shoes { get; set; }
    }

    public partial class Bike
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("resource_state")]
        public long ResourceState { get; set; }

        [JsonProperty("distance")]
        public long Distance { get; set; }
    }
}
