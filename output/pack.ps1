$data = @(
   "./../src/S3.Integration/"
);

$data | ForEach-Object {
    dotnet build $_
    nuget pack $_
}