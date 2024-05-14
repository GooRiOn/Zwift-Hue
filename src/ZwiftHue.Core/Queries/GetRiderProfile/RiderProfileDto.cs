namespace ZwiftHue.Core.Queries.GetRiderProfile;

public class RiderProfileDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImageSrc { get; set; }
    public bool Riding { get; set; }
    public int? WorldId { get; set; }
    public int Age { get; set; }
    public int Ftp { get; set; }
    public RiderProfileConfigurationDto Configuration { get; set; }
}

public class RiderProfileConfigurationDto
{
    public int Id { get; set; }
    public bool LightsOnActivityStart { get; set; }
    public int PowerZoneMissRatio { get; set; }
}