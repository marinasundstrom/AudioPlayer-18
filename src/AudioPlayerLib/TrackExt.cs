namespace AudioPlayer
{
    public static class TrackExt
    {
        public static bool IsStream(this Track track) => track.Type == "HTTP";

        public static bool IsLineIn(this Track track) => track.Type == "LINEIN";
    }
}
