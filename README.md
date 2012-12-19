## Overview

This implements the TraceListener intended for .NET applicaiton deployed to Windows Azure as WebSites.

## Setup

  * Simple clone and build.cmd to build the project.
  * Add AzureTraceListener.dll as an Assembly reference to your .NET project.
  * Make sure you commit this assembly as part of your repo.

## Include AzureTraceListener as a TraceListener to your app

You may do one of the following.

  * Imperative thru codes in Application_Start.  It is important that the path to LogFiles is `..\..\LogFiles\<yourapp>`.

    `Trace.Listeners.Add(new AzureTraceListener.AzureTraceListener(@"..\..\LogFiles\aspnet01"));`
  
  * Add to web.config

        <system.diagnostics>  
          <trace autoflush="true" indentsize="4">
            <listeners>
              <add name="TextListener"
                type="AzureTraceListener.AzureTraceListener, AzureTraceListener"
                initializeData="..\..\logFiles\aspnet01" />
              <remove name="Default" />
            </listeners>
          </trace>
        </system.diagnostics>

That's it.  You should be able to use `System.Diagnostics.Trace` in your application and see the trace files.
  


