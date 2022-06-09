using AudioStreaming.Application.DTOs.Requests;

namespace AudioStreaming.Application.DTOs.Tracks
{
    public class GetTrackListByCriterioDto : RequestParameters
    {

    }

    public enum TrackCriterio
    {
        Chart,
        Genre,
        
    }
}
