namespace Application.Features.Permissions.ElasticSearchModel
{
    public class Permission
    {
        public string Id { get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public string PermissionTypeId { get; set; }
        public string PermissionDate { get; set; }
    }
}
