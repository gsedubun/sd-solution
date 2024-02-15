using System.ComponentModel.DataAnnotations;

namespace WebapiStores.DataAccess.ViewModels;

public class AddStore   {
    [Required]
    public string StoreName { get; set; }
    public int DistrictId { get; set; }

}
public class District{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; }

}
public class UpdateStore   {
    public int StoreId { get; set; }
    [Required]
    public string StoreName { get; set; }
    public int DistrictId { get; set; }    

}