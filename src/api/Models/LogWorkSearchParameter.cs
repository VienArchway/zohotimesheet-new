namespace api.Models
{
    public class LogWorkSearchParameter
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public IEnumerable<Project>? Projects { get; set; }

        public IEnumerable<string>? OwnerIds { get; set; }

        public int? SprintTypeId { get; set; }
    }
}