namespace EPAM_DataAccessLayer.Enums
{
    /// <summary>
    /// Possible payment statuses
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Payment was canceled
        /// </summary>
        Cancelled,
        /// <summary>
        /// Payment waiting for some actions
        /// </summary>
        Awaiting,
        /// <summary>
        /// Payment was Completed
        /// </summary>
        Completed,
        /// <summary>
        /// Payment actually pended
        /// </summary>
        Pending
    }
}