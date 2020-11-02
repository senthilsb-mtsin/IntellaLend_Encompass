using System;

namespace MTSIDCReportService
{
  internal class QueueItem
  {
    public long Pk { get; set; }

    public string InstanceId { get; set; }

    public string BatchStatus { get; set; }

    public DateTime LastModified { get; set; }

    public DateTime CreateDate { get; set; }

    public string Review_Operator { get; set; }

    public string Validation_Operator { get; set; }
  }
}
