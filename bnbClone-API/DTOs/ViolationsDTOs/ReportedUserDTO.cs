namespace bnbClone_API.DTOs.ViolationsDTOs
{
    public class ReportedUserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Role { get; set; }
        public bool EmailVerified { get; set; }
    }

}
