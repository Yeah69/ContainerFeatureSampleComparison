﻿// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Text;
using ContainerFeatureSampleComparison.FeatureDefinitions;
using Humanizer;

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

var allAvailableMiscellaneousInformation = diContainerComparison
    .DiContainerDescriptions
    .SelectMany(kvpContainer =>
        kvpContainer.Value.MiscellaneousInformation.Select(kvpSamples => (
            DiContainerName: kvpContainer.Key, 
            Kind: kvpSamples.Key, 
            Information: kvpSamples.Value)))
    .ToImmutableDictionary(t => (t.DiContainerName, t.Kind), t => t.Information);

var idMap = allAvailableFeatureDescriptions
    .Values
    .OfType<object>()
    .Concat(featureGroupDescriptions.SelectMany(g => g.Features))
    .Select((fd, i) => (fd, $"id{i}"))
    .ToImmutableDictionary(t => t.fd, t => t.Item2);

var allResolutionStage = diContainerComparison
    .DiContainerDescriptions
    .Select(kvpContainer => (DiContainerName: kvpContainer.Key, kvpContainer.Value.ResolutionStage))
    .ToImmutableDictionary(t => t.DiContainerName, t => t.ResolutionStage);

var html = new StringBuilder();
html.AppendLine("""
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
            
            button {
              padding: 8px;
              height: 100%;
              width: 100%;
              color: #fff;
              text-align: left;
              border: 0;
              cursor: default;
            }
            
            button.zoom {
              cursor: zoom-in;
            }
            
            /* Background colors picked from Okabe-Ito (should be color-blind-friendly; https://jfly.uni-koeln.de/color/). Hex values from https://mikemol.github.io/technique/colorblind/2018/02/11/color-safe-palette.html. */

            .status_check {
              color: white;
              background-color: #009E73;
              border: 3px solid black;
            }

            .status_warn {
              color: white;
              background-color: #0072B2;
              border: 3px dashed black;
            }

            .status_cross {
              color: white;
              background-color: #D55E00;
              border: 3px dotted black;
            }
    </style>
    </head>
    <body>
        <main>
            <div>
                <h1>Dependency Injection Container Feature Comparison</h1>
""");

html.AppendLine("""
<section>
    <h2>General Information</h2>
    <table>
        <tbody>
""");
    
html.AppendLine("""
            <tr>
                <th/>
""");

foreach (var diContainerName in diContainerNames)
{
    html.AppendLine($$"""
<th>{{diContainerName}}</th>
""");
}

html.AppendLine("""
            </tr>
""");

foreach (var miscellaneousInformation in Enum.GetValues(typeof(MiscellaneousInformation)).OfType<MiscellaneousInformation>())
{
    html.AppendLine($$"""
            <tr>
                <td>{{miscellaneousInformation.Humanize(LetterCasing.Title)}}</td>
""");

    foreach (var diContainerName in diContainerNames)
    {
        if (allAvailableMiscellaneousInformation.TryGetValue((diContainerName, miscellaneousInformation),
                out var information))
        {
            if (Uri.TryCreate(information, UriKind.Absolute, out var uri))
            {
                html.AppendLine($$"""
<td><a href="{{uri}}">Link</a></td>
""");
            }
            else
            {
                html.AppendLine($$"""
<td>{{information}}</td>
""");
            }
        }
        else
        {
            html.AppendLine("""
<td>🤷 Unknown</td>
""");
        }
    }

    html.AppendLine("""
            </tr>
""");
}

html.AppendLine("""
            <tr>
                <td>Resolution Stage</td>
""");

foreach (var diContainerName in diContainerNames)
{
    if (allResolutionStage.TryGetValue(diContainerName, out var resolutionStage))
    {
        var text = resolutionStage is ResolutionStage.CompileTime
            ? "🔨 Compile-Time"
            : resolutionStage is ResolutionStage.RunTime
                ? "🏃 Run-Time"
                : "🤷 Unknown";
        html.AppendLine($$"""
<td>{{text}}</td>
""");
    }
    else
    {
        html.AppendLine("""
<td>🤷 Unknown</td>
""");
    }
}

html.AppendLine("""
            </tr>
""");

html.AppendLine("""
        </tbody>
    </table>
</section>
""");

foreach (var featureGroupDescription in featureGroupDescriptions)
{
    html.AppendLine($$"""
<section>
    <h2>{{featureGroupDescription.Title.Humanize(LetterCasing.Title)}}</h2>
    <div>{{(!string.IsNullOrEmpty(featureGroupDescription.Description) ? $"<p>{featureGroupDescription.Description}</p>" : string.Empty)}}</div>
    <table>
        <tbody>
""");
    
    html.AppendLine("""
<tr>
    <th>Feature</th>
""");

    foreach (var diContainerName in diContainerNames)
    {
        html.AppendLine($$"""
<th>{{diContainerName}}</th>
""");
    }

    html.AppendLine("""
</tr>
""");
    
    foreach (var featureDescription in featureGroupDescription.Features)
    {
        html.AppendLine($$"""
<tr>
    <td><div class="zoom" onclick="openDescriptionBox('{{idMap[featureDescription]}}')">{{featureDescription.Title.Humanize(LetterCasing.Title)}} 🔍</div></td>
""");

        foreach (var diContainerName in diContainerNames)
        {
            if (allAvailableFeatureDescriptions.TryGetValue(
                    (diContainerName, featureDescription.Feature), 
                    out var specificFeatureDescription))
            {
                var text = specificFeatureDescription switch
                {
                    FeatureSampleDescription featureSampleDescription => GenerateFeatureStateCell("✓ Sample", "status_check",featureSampleDescription),
                    MissingFeatureDescription { Hint: null } missingFeatureDescription => missingFeatureDescription.Reason switch
                    {
                        MissingFeatureReason.Unimplemented => GenerateFeatureStateCell("⚠ Unimplemented", "status_warn"),
                        MissingFeatureReason.NotSupported => GenerateFeatureStateCell("⚠ Not supported", "status_warn"),
                        MissingFeatureReason.DesignDecision => GenerateFeatureStateCell("⚠ Design decision", "status_warn"),
                        _ => throw new ArgumentOutOfRangeException(nameof(missingFeatureDescription.Reason))
                    },
                    MissingFeatureDescription { Hint: not null } missingFeatureDescription => missingFeatureDescription.Reason switch
                    {
                        MissingFeatureReason.Unimplemented => GenerateFeatureStateCell("⚠ Unimplemented", "status_warn", missingFeatureDescription),
                        MissingFeatureReason.NotSupported => GenerateFeatureStateCell("⚠ Not supported", "status_warn", missingFeatureDescription),
                        MissingFeatureReason.DesignDecision => GenerateFeatureStateCell("⚠ Design decision", "status_warn", missingFeatureDescription),
                        _ => throw new ArgumentOutOfRangeException(nameof(missingFeatureDescription.Reason))
                    },
                    _ => throw new ArgumentOutOfRangeException(nameof(specificFeatureDescription))
                };
                html.AppendLine($$"""
<td>{{text}}</td>
""");
            }
            else
            {
                html.AppendLine($"<td>{GenerateFeatureStateCell("🤷 Unknown", "status_cross")}</td>");
            }

            string GenerateFeatureStateCell(string label, string styleClass, IFeatureDescription? associatedFeatureDescription = null)
            {
                var onclick = associatedFeatureDescription is not null
                    ? $" onclick=\"openDescriptionBox('{idMap[associatedFeatureDescription]}')\""
                    : "";
                var labelSuffix = associatedFeatureDescription is not null
                    ? " 🔍"
                    : "";
                var cursorClass = associatedFeatureDescription is not null
                    ? " zoom"
                    : "";
                return $"<button class=\"{styleClass}{cursorClass}\"{onclick}>{label}{labelSuffix}</button>";
            }
        }

        html.AppendLine("""
</tr>
""");
            
        html.AppendLine($$"""
<tr id="{{idMap[featureDescription]}}" class="description_box">
    <td colspan="{{diContainerNames.Count + 1}}">
        <pre>{{featureGroupDescription.Description}}</pre>
    </td>
</tr>
""");

        foreach (var diContainerName in diContainerNames)
        {
            if (allAvailableFeatureDescriptions.TryGetValue(
                    (diContainerName, featureDescription.Feature), 
                    out var specificFeatureDescription))
            {
                if (specificFeatureDescription is FeatureSampleDescription featureSampleDescription)
                {
                    html.AppendLine($$"""
<tr id="{{idMap[featureSampleDescription]}}" class="description_box">
    <td colspan="{{diContainerNames.Count + 1}}">
        <pre><code>{{featureSampleDescription.SampleCode}}</code></pre>
    </td>
</tr>
""");
                }
                if (specificFeatureDescription is MissingFeatureDescription { Hint: {} hint } missingFeatureDescription)
                {
                    html.AppendLine($$"""
<tr id="{{idMap[missingFeatureDescription]}}" class="description_box">
    <td colspan="{{diContainerNames.Count + 1}}">
        <pre>{{hint}}</pre>
    </td>
</tr>
""");
                }
            }
        }
    }
    
    html.AppendLine("""
        </tbody>
    </table>
</section>
""");
}

html.AppendLine("""
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

Directory.CreateDirectory("./wwwroot");
File.WriteAllText("./wwwroot/index.html", html.ToString());

Console.WriteLine("Hello, World!");