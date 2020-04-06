using FluentValidation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FocusMark.Domain.Validators
{
    [TestClass]
    public class ProjectValidatorTests
    {
        [TestMethod]
        [TestCategory("Domain")]
        public void Project_MissingTitle_Fails()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var project = new Project
            {
                DueDate = DateTime.Now,
                Owner = nameof(ProjectValidatorTests),
                PercentageCompleted = 50,
                Priority = Priority.High,
                StartDate = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                Title = null
            };

            // Act
            var validationResults = projectValidator.Validate(project);

            // Assert
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, validationResults.Errors.Count, "Only expected 1 validation error.");
            Assert.AreEqual(nameof(NotEmptyValidator), validationResults.Errors[0].ErrorCode, $"Expected '{nameof(NotEmptyValidator)}' error code");
            Assert.AreEqual(nameof(Project.Title), validationResults.Errors[0].PropertyName, $"Expected '{nameof(Project.Title)}' property.");
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Project_DueDateCantExceedStartDate()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var project = new Project
            {
                DueDate = DateTime.Now,
                Owner = nameof(ProjectValidatorTests),
                PercentageCompleted = 50,
                Priority = Priority.High,
                StartDate = DateTime.Now.Add(TimeSpan.FromDays(1)),
                Title = nameof(ProjectValidatorTests)
            };

            // Act
            var validationResults = projectValidator.Validate(project);

            // Assert
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, validationResults.Errors.Count, "Only expected 1 validation error.");
            Assert.AreEqual(nameof(Project.DueDate), validationResults.Errors[0].PropertyName, $"Expected '{nameof(Project.DueDate)}' property.");
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Project_MustHaveId()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var project = new Project
            {
                Id = default,
                Owner = nameof(ProjectValidatorTests),
                Title = nameof(ProjectValidatorTests)
            };

            // Act
            var validationResults = projectValidator.Validate(project);

            // Assert
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, validationResults.Errors.Count, "Only expected 1 validation error.");
            Assert.AreEqual(nameof(NotEmptyValidator), validationResults.Errors[0].ErrorCode, $"Expected '{nameof(NotEmptyValidator)}' error code");
            Assert.AreEqual(nameof(Project.Id), validationResults.Errors[0].PropertyName, $"Expected '{nameof(Project.Id)}' property.");
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Project_MustHaveOwner()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var project = new Project
            {
                Owner = string.Empty,
                Title = nameof(ProjectValidatorTests)
            };

            // Act
            var validationResults = projectValidator.Validate(project);

            // Assert
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, validationResults.Errors.Count, "Only expected 1 validation error.");
            Assert.AreEqual(nameof(NotEmptyValidator), validationResults.Errors[0].ErrorCode, $"Expected '{nameof(NotEmptyValidator)}' error code");
            Assert.AreEqual(nameof(Project.Owner), validationResults.Errors[0].PropertyName, $"Expected '{nameof(Project.Owner)}' property.");
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Project_CantExceed100Percent()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var project = new Project
            {
                Owner = nameof(ProjectValidatorTests),
                PercentageCompleted = 101,
                Title = nameof(ProjectValidatorTests)
            };

            // Act
            var validationResults = projectValidator.Validate(project);

            // Assert
            Assert.IsFalse(validationResults.IsValid);
            Assert.AreEqual(1, validationResults.Errors.Count, "Only expected 1 validation error.");
            Assert.AreEqual(nameof(LessThanOrEqualValidator), validationResults.Errors[0].ErrorCode, $"Expected '{nameof(LessThanOrEqualValidator)}' error code");
            Assert.AreEqual(nameof(Project.PercentageCompleted), validationResults.Errors[0].PropertyName, $"Expected '{nameof(Project.PercentageCompleted)}' property.");
        }
    }
}
