namespace LyceumSupportDesk.Models.ViewModels
{
    public class EmployeeWorkloadViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;

        // Analytical Data
        public int ActiveTickets { get; set; }
        public int ResolvedTickets { get; set; }
        public int TotalTickets { get; set; }

        //Calculate workload capacity (e.g., max 10 active tickets)
        public int WorkloadPercentage => TotalTickets > 0
            ? (int)Math.Round((double)ActiveTickets / 10 * 100)
            : 0;
    }
}