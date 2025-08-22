using System;

namespace BioLinkWeb.Models
{
    public class ErrorViewModel
{
    public string? RequestId { get; set; }   // pakai nullable
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

}