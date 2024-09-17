using Cafe_NET_API.Helper;
using System.ComponentModel.DataAnnotations;

namespace Cafe_NET_API.Entities
{
    public class CafeBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string? Logo { get; set; }

        [Required]
        public string Location { get; set; }

        public CafeBase() { }
    }

    public class CafeEntity: CafeBase
    {
        //string hex
        public string Id { get; set; }

        public CafeEntity() { }

        public CafeEntity(Cafe cafe)
        {
            var uuidByteArray = ((Guid)cafe.Id).ToByteArray();
            string uuidHex = BitConverter.ToString(uuidByteArray).Replace("-", string.Empty);

            Id = cafe.Id.ToHexString();
            Name = cafe.Name;
            Description = cafe.Description;
            Logo = cafe.Logo;
            Location = cafe.Location;
        }
    }

    public class Cafe : CafeBase
    {
        public Guid? Id { get; set; }

        public Cafe() { }

        public Cafe(CafeEntity cafeEntity)
        {
            Id = new Guid(cafeEntity.Id);
            Name = cafeEntity.Name;
            Description = cafeEntity.Description;
            Logo = cafeEntity.Logo;
            Location = cafeEntity.Location;
        }
    }
}
