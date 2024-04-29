namespace ZwiftHue.UI.Models;

public class ZwiftProfileDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string ImageSrc { get; set; }
    public bool Riding { get; set; }
    public int? WorldId { get; set; }
    public int Age { get; set; }
}