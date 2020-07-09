Clear-Host
Write-Host "================ Neuralm manual startup tool ================"
Write-Host "MessageQueue        : Press '1' for this option."
Write-Host "RegistryService     : Press '2' for this option."
Write-Host "UserService         : Press '3' for this option."
Write-Host "TrainingRoomService : Press '4' for this option."
Write-Host "Presentation.Web    : Press '5' for this option."
Write-Host "Q                   : Press 'Q' to quit."

$service = Read-Host "Please make a selection"

# set location to neuralm source
$currentPath = $PSScriptRoot + "\src"
Set-Location $currentPath

# ensures the services are ran via the Development settings
$env:ASPNETCORE_ENVIRONMENT = "Development"

$defaultDotnetRunner = "dotnet run"
$dotnetLaunchProfileRunner = $defaultDotnetRunner + " --launch-profile "

$servicePath = ""
$serviceName = ""
$serviceRunner = ""

switch ($service) {
    "1" {
        $serviceName = "Neuralm.Services.MessageQueue.NeuralmMQ"
        $servicePath = $currentPath + "\Neuralm.Services\Neuralm.Services.MessageQueue\" + $serviceName
        $serviceRunner = $defaultDotnetRunner
    }
    "2" {
        $serviceName = "Neuralm.Services.RegistryService.Rest"
        $servicePath = $currentPath + "\Neuralm.Services\Neuralm.Services.RegistryService\" + $serviceName
        $serviceRunner = $dotnetLaunchProfileRunner + $serviceName
    }
    "3" {
        $serviceName = "Neuralm.Services.UserService.Rest"
        $servicePath = $currentPath + "\Neuralm.Services\Neuralm.Services.UserService\" + $serviceName
        $serviceRunner = $dotnetLaunchProfileRunner + $serviceName
    }
    "4" {
        $serviceName = "Neuralm.Services.TrainingRoomService.Rest"
        $servicePath = $currentPath + "\Neuralm.Services\Neuralm.Services.TrainingRoomService\" + $serviceName
        $serviceRunner = $dotnetLaunchProfileRunner + $serviceName
    }
    "5" {
        $serviceName = "Neuralm.Presentation.Web"
        $servicePath = $currentPath + "\" + $serviceName
        $serviceRunner = "npm run serve"
    }
    "q" {
        return
    }
    default {
        Write-Error "service not found"
    }
}

# go to service bin
Set-Location $servicePath

# runs the service
Invoke-Expression $serviceRunner
