# Set the working directory to SDK
Set-Location "sdk/"
# Download the file with Invoke-WebRequest
Invoke-WebRequest -Uri "http://www.dev-c.com/files/ScriptHookRDR2_SDK_1.0.1207.73.zip" -OutFile "sdk.zip" -Headers @{ "Referer" = "http://www.dev-c.com/rdr2/scripthookrdr2/" }
# Extract the contents of the file in the current directory
7z x sdk.zip
# Return to the location where we started
Set-Location "..\"