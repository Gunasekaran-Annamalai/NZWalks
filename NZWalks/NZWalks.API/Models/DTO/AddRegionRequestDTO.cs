using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        // [Required] is an annotation and it says that the field is required or not null
        // Annotations are also used to make validations
        // [MinLength(3)] is to set minimum length
        // [MinLength(3)] this it self is fine but if you want to add an error message we can use an optional as shown below
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be maximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageURL { get; set; }
    }
}
