# Check format
dotnet format

# Set version
$version = "0.0.0-development"
"Version: $version"

# Build
Remove-Item -Path src/**/bin -recurse
dotnet build --configuration Release --no-restore /p:Version=$version

# Vika [TODO]
# vika --debug --includesource static-analysis.Core.sarif.json static-analysis.Program.sarif.json

# Build release notes
dotnet run --project src/SemanticReleaseNotesParser --no-build --configuration Release -- -g=categories --debug -o="artifacts/ReleaseNotes.html" --pluralizecategoriestitle --includestyle

# Test
dotnet test src --configuration Release --no-build --nologo --verbosity normal --collect:"XPlat Code Coverage"
dotnet reportgenerator "-reports:src/**/TestResults/**/coverage.cobertura.xml" "-targetdir:artifacts/reports" -reporttypes:Html

