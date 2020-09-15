namespace PodcastStats
{
    /// <summary>
    ///     There is quite some inconsistency in the formats of the 'duration' tag between different podcasts. This keeps track
    ///     of that so that it can be formatted properly later.
    /// </summary>
    public enum DurationStyle
    {
        hhmmss,
        hhmmssMixed,
        mmss,
        secondsDuration
    }
}