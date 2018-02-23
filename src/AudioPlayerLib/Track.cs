namespace Axis.AudioPlayer
{
    public class Track
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Type { get; set; }

        public override string ToString() => $"{Title} - {Artist}";

        public const string AIS_AD_BREAK = "AIS_AD_BREAK";

        public const string STOP_AD_BREAK = "STOP_AD_BREAK";

        public const string LINEIN_ID = "4c494e45-494e-5452-4143-4b2020202000";

        public const string TYPE_LINEIN = "LINEIN";

        public const string TYPE_HTTP = "HTTP";
    }
}
