using bnbClone_API.Models;
using System.Text.Json.Serialization;

namespace bnbClone_API.DTOs.ViolationsDTOs
{
    public class CreateViolationDTO
    {
        public int ReportedById { get; set; } // ID of the reporting user
        public int? ReportedPropertyId { get; set; }
        public int? ReportedHostId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ViolationType ViolationType { get; set; }
        public string Description { get; set; } = string.Empty;
    }

}
