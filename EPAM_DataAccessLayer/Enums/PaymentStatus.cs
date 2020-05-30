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
        /// Payment was Completed
        /// </summary>
        Confirmed,
        /// <summary>
        /// Payment actually pended
        /// </summary>
        Pending
    }
}