namespace LagDaemon.OGE.InterfaceTypes

module MessageTypes =

    type Severity = High | Medium  | Low
            
    type Criticality =
        | Info  of string
        | Warning of string
        | Error of string
        | Exception of string * System.Exception 


    type LogEntry = {
        TimeStamp: System.DateTime;
        Severity: Severity;
        Criticality: Criticality;
    }

    type Credentials = {
        Login: string;
        Password: string;
    }


    let createInfoEntry str = {
                    TimeStamp = System.DateTime.Now;
                    Severity = Low;
                    Criticality = Info str;
                }

    let createWarningEntry str = {
                    TimeStamp = System.DateTime.Now;
                    Severity = Medium;
                    Criticality = Warning str;
                }

    let createErrorEntry str = {
                    TimeStamp = System.DateTime.Now;
                    Severity = High;
                    Criticality = Error str;
                }


    let createExceptionEntry str ex = {
                    TimeStamp = System.DateTime.Now;
                    Severity = High;
                    Criticality = Exception (str, ex);
                }
