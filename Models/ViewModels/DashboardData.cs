namespace PROG6212_CMCS.Models.ViewModels
{ 
    public class ClaimItem {
        public Claim claim { get; set; }
        public User user { get; set; }
        public List<Document> documents { get; set; }
        public ClaimItem(Claim claim,List<Document> documents, User user) {
            this.claim = claim;
            this.documents = documents;
            this.user = user;
        }
    } 

    public class DashboardData
    {
        public int allClaims { get; set; } = 0;
        public int pendingClaims { get; set; } = 0;
        public int approvedClaims { get; set; } = 0;
        public int rejectedClaims { get; set; } = 0;

        public List<ClaimItem>? claims = null;
    }
}
