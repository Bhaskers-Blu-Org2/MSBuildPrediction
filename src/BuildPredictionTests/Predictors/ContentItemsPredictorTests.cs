﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Build.Prediction.Tests.Predictors
{
    using Microsoft.Build.Construction;
    using Microsoft.Build.Evaluation;
    using Microsoft.Build.Prediction.Predictors;
    using Xunit;

    public class ContentItemsPredictorTests
    {
        [Fact]
        public void NoCopy()
        {
            ProjectRootElement projectRootElement = ProjectRootElement.Create();
            projectRootElement.AddProperty(ContentItemsPredictor.OutDirPropertyName, @"bin\");
            projectRootElement.AddItem(ContentItemsPredictor.ContentItemName, "Foo.xml");

            Project project = TestHelpers.CreateProjectFromRootElement(projectRootElement);

            new ContentItemsPredictor()
                .GetProjectPredictions(project)
                .AssertPredictions(
                    project,
                    new[] { new PredictedItem("Foo.xml", nameof(ContentItemsPredictor)) },
                    null,
                    null,
                    null);
        }

        [Fact]
        public void WithCopy()
        {
            ProjectRootElement projectRootElement = ProjectRootElement.Create();
            projectRootElement.AddProperty(ContentItemsPredictor.OutDirPropertyName, @"bin\");

            ProjectItemElement item = projectRootElement.AddItem(ContentItemsPredictor.ContentItemName, "Foo.xml");
            item.AddMetadata("CopyToOutputDirectory", "PreserveNewest");

            Project project = TestHelpers.CreateProjectFromRootElement(projectRootElement);

            new ContentItemsPredictor()
                .GetProjectPredictions(project)
                .AssertPredictions(
                    project,
                    new[] { new PredictedItem("Foo.xml", nameof(ContentItemsPredictor)) },
                    null,
                    new[] { new PredictedItem(@"bin\Foo.xml", nameof(ContentItemsPredictor)) },
                    null);
        }
    }
}