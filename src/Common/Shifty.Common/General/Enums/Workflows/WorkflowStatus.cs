namespace Shifty.Common.General.Enums.Workflows;

public enum WorkflowStatus
{
    Unknown,    // Default or not set
    Draft,      // Created but not submitted
    Submitted,  // Awaiting review or triage
    Approved,   // Accepted for execution
    InProgress, // Work is being done
    OnHold,     // Temporarily paused
    Completed,  // Work done, pending final review
    Closed,     // Fully completed and archived
    Rejected,   // Not approved or discarded
    Cancelled   // Intentionally stopped
}