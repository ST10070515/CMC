// Inside PROG6212_CMCS.Models.ViewModels/SubmitClaimViewModel.cs

using PROG6212_CMCS.Models;

namespace PROG6212_CMCS.Models.ViewModels
{
    public class SubmitClaimViewModel : Claim
    {
        public string ErrorMessage { get; set; } = "";
    }
}