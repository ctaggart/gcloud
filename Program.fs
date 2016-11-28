let configLogger() =
    let traceAppender = log4net.Appender.TraceAppender()
    traceAppender.Layout <- log4net.Layout.PatternLayout ( ConversionPattern = "%message" )
    traceAppender.ActivateOptions();

    let gcloudAppender = Google.Logging.Log4Net.GoogleStackdriverAppender()
    gcloudAppender.ProjectId <- "ctaggartcom"
    gcloudAppender.LogId <- "StackdriverLoggingFSharpExample"
    gcloudAppender.Layout <- log4net.Layout.PatternLayout ( ConversionPattern = "%message" )
    gcloudAppender.ActivateOptions()

    let hierarchy = log4net.LogManager.GetRepository() :?> log4net.Repository.Hierarchy.Hierarchy
    hierarchy.Root.AddAppender traceAppender
    hierarchy.Root.AddAppender gcloudAppender
    hierarchy.Root.Level <- log4net.Core.Level.Debug
    hierarchy.Configured <- true

[<EntryPoint>]
let main argv =
    configLogger()
    let log = log4net.LogManager.GetLogger "Program"

    log.Info "Yay logging!"
    log.Debug "Works from Mono too."
    log.Warn "Can't wait for the dependency trees in VS 2017."

    // no Flush yet https://github.com/GoogleCloudPlatform/google-cloud-dotnet/issues/582
    System.Threading.Thread.Sleep 10000 // 10 seconds
    0