$TASK_TRIGGER_DAILY = 2
# The name of the scheduled task
$TaskName = "Forte Lock workstation on user logon"
# The description of the task
$TaskDescr = "Forte Lock workstation on user logon"
# The Task Action command
$TaskCommand = "c:\forte\script\lock-workstation.bat"
# The Task Action command argument
$TaskArg = "1"

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
$trigger = $triggers.Create(9) # Creates a trigger on logon
$trigger.UserId = "Forte\Forte"
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