namespace PROG6212_CMCS.Models.ViewModels
{
    public class DashboardData
    {
        public int allClaims { get; set; } = 0;
        public int pendingClaims { get; set; } = 0;
        public int approvedClaims { get; set; } = 0;

        public int rejectedClaims { get; set; } = 0;

        public List<Claim>? claims = null;
    }
}
