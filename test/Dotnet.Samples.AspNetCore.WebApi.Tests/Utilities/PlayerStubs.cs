using System.Data.Common;
using Dotnet.Samples.AspNetCore.WebApi.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Samples.AspNetCore.WebApi.Tests.Utilities
{
    /// <summary>
    /// A Stub provides pre‑configured, hard‑coded responses to method calls.
    /// Its purpose is simply to supply data to the system under test without
    /// any behavior verification. For example, a Stub for a repository might
    /// always return a specific user regardless of the input.
    /// </summary>
    public static class PlayerStubs
    {
        /// <summary>
        /// Creates a ModelStateDictionary with a single validation error.
        /// Used for testing validation failure scenarios.
        /// </summary>
        /// <param name="key">The property name or key for the error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>A ModelStateDictionary containing the specified error.</returns>
        public static ModelStateDictionary CreateModelError(string key, string errorMessage)
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError(key, errorMessage);
            return modelStateDictionary;
        }
    }
}
