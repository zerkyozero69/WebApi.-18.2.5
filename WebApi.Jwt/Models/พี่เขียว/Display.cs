public class DisplayOnApp
{
    public string DisplayOnAppId { get; set; }
    public string DisplayOnAppName { get; set; }
}
public class SP_LoadReportByRId_Model
{
    public string ReportId { get; set; }
    public string ReportName { get; set; }
    public string ReportDescription { get; set; }
    public string ReportData { get; set; }
    public string IsPublish { get; set; }
    public string PublishDate { get; set; }

    public string PublishByUsername { get; set; }
  
           
}

public class LoadReportPermissionBy
{
    public string ReportId { get; set; }
    public string ReportName { get; set; }
}
public class LoadReportsAll_Model
    {
    public string ReportId { get; set; }
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string ReportName { get; set; }
    public string ReportDescription { get; set; }

    public string ReportData { get; set; }
    public string IsPublish { get; set; }
    public string PublishDate { get; set; }
    public string PublishByUsername { get; set; }
    public string DisplayOnAppId { get; set; }
    public string DisplayOnAppName { get; set; }

}
public class DeleteReportsByRID_Model
{

    public string Status { get; set; }
    public string Message { get; set; }

}

public class LoadReportByRoleId_Model
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string Description { get; set; }
    public string FullRole { get; set; }

}