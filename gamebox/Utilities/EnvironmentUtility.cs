using System.Collections;

namespace GameBox.Utilities
{
    /// <summary>
    /// Utilities for working with the system environment.
    /// </summary>
    public class EnvironmentUtility
    {
        private static IDictionary? variables;

        /// <summary>
        /// Get an environment variable.
        /// </summary>
        public static String GetVariable(string varName)
        {
            if (EnvironmentUtility.variables == null) {
                EnvironmentUtility.variables = Environment.GetEnvironmentVariables();
            }

            if(!variables.Contains(varName))
                throw new Exception($"Environment variable {varName} is missing, please make sure it is properly set in the registry or parent process");
            if (variables[varName] is not object varObj)
                throw new Exception($"Could not read environment variable {varName}");
            if (varObj is not string varValue)
                throw new Exception($"Environment variable {varName} is invalid");

            return varValue;
        }
    }
}
