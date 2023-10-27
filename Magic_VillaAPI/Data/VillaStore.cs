using Magic_VillaAPI.Dto;

namespace Magic_VillaAPI.Data
{
    public class VillaStore
    {
       public static List<VillaDTO> villaList =  new List<VillaDTO> {
                new VillaDTO { Id = 1, Name="Pool Villa", Sqft = 200, Occupancy = 3},
                new VillaDTO { Id = 2, Name="Beach Villa", Sqft = 500, Occupancy = 5}
            };
    }
}
