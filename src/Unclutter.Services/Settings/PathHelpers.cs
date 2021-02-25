using System;
using System.Collections.Generic;
using System.Linq;

namespace Unclutter.Services.Settings
{
    /// <summary>
    /// Helper methods and constants for manipulating Setting paths
    /// </summary>
    public static class PathHelpers
    {
        /// <summary>
        /// The default delimiter "." used to separate individual keys in a path.
        /// </summary>
        public const string KeyDelimiter = ".";

        /// <summary>
        /// Combine path segments into one path.
        /// </summary>
        /// <param name="pathSegments">The path segments to combine.</param>
        /// <param name="keyDelimiter">The delimiter used to combine the segments.</param>
        /// <returns>The combined path.</returns>
        public static string Combine(string[] pathSegments, string keyDelimiter = KeyDelimiter)
        {
            if (pathSegments == null)
            {
                throw new ArgumentNullException(nameof(pathSegments));
            }
            return string.Join(keyDelimiter, pathSegments);
        }

        /// <summary>
        /// Combine path segments into one path.
        /// </summary>
        /// <param name="pathSegments">The path segments to combine.</param>
        /// <param name="keyDelimiter">The delimiter used to combine the segments.</param>
        /// <returns>The combined path.</returns>
        public static string Combine(IEnumerable<string> pathSegments, string keyDelimiter = KeyDelimiter)
        {
            if (pathSegments == null)
            {
                throw new ArgumentNullException(nameof(pathSegments));
            }
            return string.Join(keyDelimiter, pathSegments);
        }

        /// <summary>
        /// Get the last path segment from the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="keyDelimiter">The delimiter used to separate individual keys in a path.</param>
        /// <returns>The last path segment of the path.</returns>
        public static string GetKey(string path, string keyDelimiter = KeyDelimiter)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var lastDelimiterIndex = path.LastIndexOf(keyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? path : path[(lastDelimiterIndex + 1)..];
        }

        /// <summary>
        /// Get the path corresponding to the parent node for a given path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="keyDelimiter">The delimiter used to separate individual keys in a path.</param>
        /// <returns>The original path minus the last individual segment found in it. Null if the original path corresponds to a top level node.</returns>
        public static string GetParentPath(string path, string keyDelimiter = KeyDelimiter)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var lastDelimiterIndex = path.LastIndexOf(keyDelimiter, StringComparison.OrdinalIgnoreCase);
            return lastDelimiterIndex == -1 ? null : path.Substring(0, lastDelimiterIndex);
        }

        /// <summary>
        /// Get the path segments.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="keyDelimiter">The delimiter used to separate individual keys in a path.</param>
        /// <returns>The path segments.</returns>
        public static string[] GetPathSegments(string path, string keyDelimiter = KeyDelimiter)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return path.Trim().Split(keyDelimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
    }
}
