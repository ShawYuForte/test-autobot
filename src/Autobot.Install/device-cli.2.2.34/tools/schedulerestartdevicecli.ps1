$TASK_TRIGGER_DAILY = 2
# The name of the scheduled task
$TaskName = "Forte device-cli restart"
# The description of the task
$TaskDescr = "Forte Device CLI Restart."
# The Task Action command
$TaskCommand = "c:\forte\script\restart-device-cli.bat"
# The Task Action command argument
$TaskArg = ""

# The time zone offset used to calculate when to start daily
$TimezoneOffset = [Regex]::Match([TimeZoneInfo]::Local.BaseUtcOffset.ToString(), '-[0-9]{2}:[0-9]{2}').Value

# attach the Task Scheduler com object
$service = new-object -ComObject("Schedule.Service")
# connect to the local machine. 
# http://msdn.microsoft.com/en-us/library/windows/desktop/aa381833(v=vs.85).aspx
$service.Connect()
$rootFolder = $service.GetFolder("\")

$TaskDefinition = $service.NewTask(0) 
$TaskDefinition.RegistrationInfo.Description = "$TaskDescr"
$TaskDefinition.Settings.Enabled = $true
$TaskDefinition.Settings.AllowDemandStart = $true

$triggers = $TaskDefinition.Triggers
#http://msdn.microsoft.com/en-us/library/windows/desktop/aa383915(v=vs.85).aspx
$trigger = $triggers.Create($TASK_TRIGGER_DAILY) # Creates a "Daily" trigger
$trigger.DaysInterval = 1
$trigger.StartBoundary = "2016-01-01T03:00:00$($TimezoneOffset)"
$trigger.Enabled = $true

# http://msdn.microsoft.com/en-us/library/windows/desktop/aa381841(v=vs.85).aspx
$Action = $TaskDefinition.Actions.Create(0)
$action.Path = "$TaskCommand"
$action.Arguments = "$TaskArg"

# https://www.autoitscript.com/forum/topic/124400-create-task-scheduler-scripting-objects/
$osversion = [System.Environment]::OSVersion.Version.Major
if ($osversion -ge 10){
	$principal = $TaskDefinition.Principal()
	$principal.RunLevel = 1
}

#http://msdn.microsoft.com/en-us/library/windows/desktop/aa381365(v=vs.85).aspx
$rootFolder.RegisterTaskDefinition("$TaskName",$TaskDefinition,6,"Forte\Forte",$null,3)