using System.Management.Automation;
using Microsoft.AnalysisServices.Tabular;

namespace PsXmla
{
    [Cmdlet(VerbsCommunications.Disconnect, "Server")]
    public class DisconnectServerCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public Server Server { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            Server.Disconnect();
        }
    }
}
