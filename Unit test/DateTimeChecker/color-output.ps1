$input | ForEach-Object {
    $line = $_
    if ($line -match '\bPassed\b') {
        Write-Host ""
        Write-Host "  PASS  $line" -ForegroundColor Green
    }
    elseif ($line -match '\bFailed\b') {
        Write-Host ""
        Write-Host "  FAIL  $line" -ForegroundColor Red
    }
    elseif ($line -match '\bSkipped\b') {
        Write-Host ""
        Write-Host "  SKIP  $line" -ForegroundColor Yellow
    }
    elseif ($line -match 'Test Run Summary|Overall result|Test Count') {
        Write-Host $line -ForegroundColor Cyan
    }
    else {
        Write-Host $line
    }
}
