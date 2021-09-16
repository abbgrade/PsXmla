using Microsoft.AnalysisServices;
using Microsoft.AnalysisServices.Tabular;
using Newtonsoft.Json;
using Polly;
using System;
using System.Management.Automation;

namespace PsXmla
{
    [Cmdlet(VerbsLifecycle.Invoke, "Command")]
    public class InvokeCommandCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public string Command { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public Microsoft.AnalysisServices.Tabular.Server Connection { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty()]
        public int? MaxRetryAttempts { get; set; }

        [Parameter()]
        public bool IgnoreResponseFormatException { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (MaxRetryAttempts.HasValue)
            {
                WriteVerbose($"Execute XMLA Command with max { MaxRetryAttempts } retry attempts.");

                var retryPolicy = Policy
                    .Handle<ConnectionException>()
                    .Retry(MaxRetryAttempts.Value, onRetry: (exception, retryCount, context) =>
                    {
                        WriteWarning($"Attempt {retryCount} returned: {exception.Message}");
                    });

                retryPolicy.Execute(() =>
                {
                    ExecuteCommand();
                });
            }
            else
            {
                WriteVerbose("Execute XMLA Command without retry attempts.");
                ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {

            bool success = true;
            XmlaResultCollection results = null;
            try
            {
                results = Connection.Execute(Command);
            }
            catch (ResponseFormatException ex)
            {
                WriteWarning(ex.InnerException.Message);
                if (IgnoreResponseFormatException == false)
                    throw ex;
            }

            if (results != null)
            {
                foreach (XmlaResult result in results)
                {
                    if (result.Value.Length > 0)
                        WriteWarning($"Result: { result.Value }");

                    foreach (XmlaMessage message in result.Messages)
                    {
                        switch (message.Source)
                        {
                            case "Microsoft Analysis Services":
                                try
                                {
                                    dynamic dynamicObject = JsonConvert.DeserializeObject(message.Description.Split("Technical Details:")[0]);
                                    if (dynamicObject != null)
                                    {
                                        WriteWarning($"{message.Source}: {message.Description}");
                                        WriteObject(dynamicObject);
                                        var error = dynamicObject?.error;
                                        if (error != null)
                                        {
                                            var code = error?.code;
                                            var pbiError = error?.pbiError;
                                            switch (code.Value)
                                            {
                                                case "DMTS_MonikerWithUnboundDataSources":
                                                    WriteWarning("Datasource not bound to gateway");
                                                    break;

                                                case "DM_GWPipeline_Gateway_DataSourceAccessError":
                                                    WriteWarning("Database not available or accessable");
                                                    break;
                                                default:
                                                    throw new NotImplementedException(code.Value);
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            WriteWarning($"{message.Source}: {message.Description}");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteWarning(ex.Message);
                                }
                                break;
                        }

                        success = false;
                    }
                }
            }

            if (!success)
                WriteError(new ErrorRecord(new System.Exception("Operation failed."), "Error", ErrorCategory.OperationStopped, null));
            else
                WriteVerbose("Operation succeeded.");
        }
    }
}
