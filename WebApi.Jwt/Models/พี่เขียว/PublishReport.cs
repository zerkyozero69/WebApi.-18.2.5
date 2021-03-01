public class PublishReport
{
    public string ReportId { get; set; }
    public string CategoryId { get; set; }
    public string ReportName { get; set; }
    public string ReportDescription { get; set; }
    public string ReportData { get; set; }
    public string PublishByUsername { get; set; }
    public bool IsPublish { get; set; }

    public string DisplayOnAppId { get; set; }
}
public class ReportPermission
{
    public string ReportId { get; set; }

    public string RoleId { get; set; }
}

