using Microsoft.AnalysisServices;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PsXmla
{
    [Cmdlet(VerbsLifecycle.Invoke, "Command")]
    public class InvokeCommandCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true
        )]
        [ValidateNotNullOrEmpty()]
        public string Command { get; set; }

        [Parameter(
            Mandatory = true
        )]
        [ValidateNotNullOrEmpty()]
        public Microsoft.AnalysisServices.Tabular.Server Connection { get; set; }

        [Parameter(
            Mandatory = false
        )]
        [ValidateNotNullOrEmpty()]
        public int? MaxRetryAttempts { get; set; }

        [Parameter()]
        public SwitchParameter IgnoreResponseFormatException { get; set; }

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

            XmlaResultCollection results = null;
            bool success = true;
            try
            {
                WriteVerbose(Command);
                results = Connection.Execute(Command);
            }
            catch (ResponseFormatException ex)
            {

                if (IgnoreResponseFormatException.IsPresent)
                {
                    WriteWarning(ex.InnerException.Message);
                }
                else
                {
                    WriteError(new ErrorRecord(
                        exception: ex,
                        errorId: "Error",
                        errorCategory: ErrorCategory.OperationStopped,
                        targetObject: Command));
                    success = false;
                }
            }
            var exceptions = new List<Exception>();

            if (results != null)
            {
                WriteVerbose($"Execution returned {results.Count} results.");
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
                                        WriteVerbose($"{message.Source}: {message.Description}");
                                        var error = dynamicObject?.error;
                                        if (error != null)
                                        {
                                            var code = error?.code;
                                            var pbiError = error?.pbiError;
                                            switch (code.Value)
                                            {
                                                case "DMTS_MonikerWithUnboundDataSources":
                                                    exceptions.Add(new Exception("Datasource not bound to gateway"));
                                                    success = false;
                                                    break;

                                                case "DM_GWPipeline_Gateway_DataSourceAccessError":
                                                    exceptions.Add(new Exception("Database not available or accessable"));
                                                    success = false;
                                                    break;

                                                case "DM_GWPipeline_Gateway_InvalidObjectNameException":
                                                    exceptions.Add(new Exception("Invalid object name in datasource"));
                                                    success = false;
                                                    break;

                                                case "DM_GWPipeline_Gateway_CanceledError":
                                                    exceptions.Add(new Exception("Operation canceled"));
                                                    success = false;
                                                    break;

                                                default:
                                                    exceptions.Add(new Exception(code.Value));
                                                    success = false;
                                                    break;
                                            }
                                            break;
                                        }
                                        else
                                        {
                                            exceptions.Add(new Exception($"{message.Source}: {message.Description}"));
                                            success = false;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    WriteWarning(ex.Message);
                                }
                                break;
                        }
                    }
                }
            }

            if (!success)
                WriteError(new ErrorRecord(
                    exception: new AggregateException("Operation failed.", exceptions), 
                    errorId: "Error", 
                    errorCategory: ErrorCategory.OperationStopped, 
                    targetObject: Command));
            else
                WriteVerbose("Operation succeeded.");
        }
    }
}
