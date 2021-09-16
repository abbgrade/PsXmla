using System.Management.Automation;
using Microsoft.AnalysisServices.Tabular;

namespace PsXmla
{
    [Cmdlet(VerbsCommunications.Connect, "Server")]
    [OutputType(typeof(Server))]
    public class ConnectServerCommand : PSCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string ConnectionString { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            var connection = new Server();
            WriteVerbose($"Connect to { ConnectionString }");
            connection.Connect(ConnectionString);
            WriteObject(connection);
        }
    }
}
