Write-Host -ForegroundColor Blue -BackgroundColor Green "Installing client UI dependencies..."
$proc = Start-Process "npm" -ArgumentList "ci" -WorkingDirectory "PlayGermany.Client/html" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Building client UI..."
$proc = Start-Process "npm" -ArgumentList "run build" -WorkingDirectory "PlayGermany.Client/html" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"

Write-Host -ForegroundColor Blue -BackgroundColor Green "Copying client UI to dist..."
$proc = Start-Process "npm" -ArgumentList "run copy-ui-build" -WorkingDirectory "PlayGermany.Client/html" -PassThru -NoNewWindow
$proc.WaitForExit()
Write-Host -ForegroundColor Blue -BackgroundColor Green " finished!"