// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Text;
using ContainerFeatureSampleComparison.FeatureDefinitions;

var diContainerComparison = ContainerFeatureSampleComparison.Generated.Descriptions.CreateDiContainerComparison();
var featureGroupDescriptions = ContainerFeatureSampleComparison.Generated.Descriptions.CreateFeatureGroupDescriptions();

var diContainerNames = diContainerComparison.DiContainerDescriptions.Keys.OrderBy(x => x).ToList();

var allAvailableFeatureDescriptions = diContainerComparison
    .DiContainerDescriptions
    .SelectMany(kvpContainer =>
        kvpContainer.Value.FeatureSamples.Select(kvpSamples => (
            DiContainerName: kvpContainer.Key, 
            Feature: kvpSamples.Key, 
            FeatureDescription: kvpSamples.Value)))
    .ToImmutableDictionary(t => (t.DiContainerName, t.Feature), t => t.FeatureDescription);

var featureDescriptionToId = allAvailableFeatureDescriptions
    .Values
    .Select((fd, i) => (fd, $"id{i}"))
    .ToImmutableDictionary(t => t.fd, t => t.Item2);

var html = new StringBuilder();
html.AppendLine($$"""
<!DOCTYPE html>
<html>
    <head>
        <title>Dependency Injection Container Feature Comparison</title>
        <meta charset="utf-8" />
        <style>
            /* Material Design Styles */

            body {
              font-family: Roboto, Arial, sans-serif;
              margin: 0;
              padding: 0;
            }

            main {
              margin: 16px;
            }

            h1 {
              font-size: 32px;
              margin: 0;
              padding: 16px 0;
              color: #333;
            }

            h2 {
              font-size: 24px;
              margin: 0;
              padding: 8px 0;
              color: #333;
            }

            p {
              margin: 0;
              padding: 8px 0;
              color: #666;
            }

            table {
              border-collapse: collapse;
              width: 100%;
              margin-top: 16px;
            }

            th {
              text-align: left;
              padding: 8px;
              color: #333;
            }

            td {
              padding: 8px;
              color: #666;
            }

            tbody tr:nth-child(even) {
              background-color: #f9f9f9;
            }

            pre {
              margin: 0;
              padding: 8px;
              background-color: #f9f9f9;
              color: #666;
            }
    </style>
    </head>
    <body>
        <main>
            <div>
                <h1>Dependency Injection Container Feature Comparison</h1>
""");

foreach (var featureGroupDescription in featureGroupDescriptions)
{
    html.AppendLine($$"""
<section>
    <h2>{{featureGroupDescription.Title}}</h2>
    <div>{{(!string.IsNullOrEmpty(featureGroupDescription.Description) ? $"<p>{featureGroupDescription.Description}</p>" : string.Empty)}}</div>
    <table>
        <tbody>
""");
    
    html.AppendLine($$"""
<tr>
    <th>Feature</th>
""");

    foreach (var diContainerName in diContainerNames)
    {
        html.AppendLine($$"""
<th>{{diContainerName}}</th>
""");
    }

    html.AppendLine($$"""
</tr>
""");
    
    foreach (var featureDescription in featureGroupDescription.Features)
    {
        html.AppendLine($$"""
<tr>
    <td>{{featureDescription.Title}}</td>
"""); // TODO: Add description

        foreach (var diContainerName in diContainerNames)
        {
            if (allAvailableFeatureDescriptions.TryGetValue(
                    (diContainerName, featureDescription.Feature), 
                    out var specificFeatureDescription))
            {
                var text = specificFeatureDescription switch
                {
                    FeatureSampleDescription featureSampleDescription => $"<button onclick=\"openDescriptionBox('{featureDescriptionToId[featureSampleDescription]}')\">Sample</button>",
                    MissingFeatureDescription missingFeatureDescription => missingFeatureDescription.Reason switch
                    {
                        MissingFeatureReason.Unimplemented => "Unimplemented",
                        MissingFeatureReason.NotSupported => "Not supported",
                        MissingFeatureReason.DesignDecision => "Design decision",
                        _ => throw new ArgumentOutOfRangeException(nameof(missingFeatureDescription.Reason))
                    }, // TODO: Add hint
                    _ => throw new ArgumentOutOfRangeException(nameof(specificFeatureDescription))
                };
                html.AppendLine($$"""
<td>{{text}}</td>
"""); // TODO: Add description
            }
            else
            {
                html.AppendLine("<td>Unknown</td>");
            }
        }

        html.AppendLine($$"""
</tr>
""");
        

        foreach (var diContainerName in diContainerNames)
        {
            if (allAvailableFeatureDescriptions.TryGetValue(
                    (diContainerName, featureDescription.Feature), 
                    out var specificFeatureDescription)
                && specificFeatureDescription is FeatureSampleDescription featureSampleDescription)
            {
                html.AppendLine($$"""
<tr id="{{featureDescriptionToId[featureSampleDescription]}}" class="description_box">
    <td colspan="{{diContainerNames.Count + 1}}">
        <pre><code>{{featureSampleDescription.SampleCode}}</code></pre>
    </td>
</tr>
""");
            }
        }
    }
    
    html.AppendLine($$"""
        </tbody>
    </table>
</section>
""");
}

html.AppendLine($$"""
            </div>
        </main>

        <script>
const description_boxes = document.querySelectorAll('.description_box');
closeAllDescriptionBoxes();
function closeAllDescriptionBoxes() {
    for (const description_box of description_boxes) {
        description_box.style.display = 'none';
    }
}
function openDescriptionBox(id) {
    closeAllDescriptionBoxes();
    const description_box = document.getElementById(id);
    description_box.style.display = 'table-row';
}
        </script>

    </body>
</html>
""");

File.WriteAllText("./index.html", html.ToString());

Console.WriteLine("Hello, World!");