using System;

namespace TLDAG.Core
{
    public sealed class SemanticVersion : IEquatable<SemanticVersion>, IComparable<SemanticVersion>
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string PreRelease { get; }
        public string Build { get; }

        private SemanticVersion(int major, int minor, int patch, string preRelease, string build)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            PreRelease = preRelease;
            Build = build;
        }

        public SemanticVersion(string version)
        {
            SemanticVersion sv = Parse(version);

            Major = sv.Major;
            Minor = sv.Minor;
            Patch = sv.Patch;
            PreRelease = sv.PreRelease;
            Build = sv.Build;
        }

        public override string ToString()
        {
            string v = $"{Major}.{Minor}.{Patch}";
            string p = PreRelease.Length > 0 ? $"-{PreRelease}" : "";
            string b = Build.Length > 0 ? $"-{Build}" : "";

            return v + p + b;
        }

        public override int GetHashCode()
        {
            return Major + Minor + Patch + PreRelease.GetHashCode() + Build.GetHashCode();
        }

        private bool EqualsSemanticVersion(SemanticVersion? other)
        {
            if (other is null) return false;

            if (Major != other.Major) return false;
            if (Minor != other.Minor) return false;
            if (Patch != other.Patch) return false;
            if (!PreRelease.Equals(other.PreRelease, StringComparison.Ordinal)) return false;
            if (!Build.Equals(other.Build, StringComparison.Ordinal)) return false;

            return true;
        }

        public bool Equals(SemanticVersion? other)
            => EqualsSemanticVersion(other);

        public override bool Equals(object? obj)
        {
            if (obj is SemanticVersion other)
                return EqualsSemanticVersion(other);

            return false;
        }

        public int CompareTo(SemanticVersion? other)
        {
            if (other is null) return -1;

            int result = Major.CompareTo(other.Major);

            if (result == 0) { result = Minor.CompareTo(other.Minor); }
            if (result == 0) { result = Patch.CompareTo(other.Patch); }

            if (result == 0)
            {
                if (PreRelease.Length == 0)
                {
                    if (other.PreRelease.Length > 0) { result = 1; }
                }
                else
                {
                    if (other.PreRelease.Length == 0)
                    {
                        result = -1;
                    }
                    else
                    {
                        string[] parts1 = PreRelease.Split('.');
                        string[] parts2 = other.PreRelease.Split('.');

                        for (int i = 0, n = Math.Min(parts1.Length, parts2.Length); i < n && result == 0; ++i)
                        {
                            if (parts1[i].IsDigits())
                            {
                                if (parts2[i].IsDigits())
                                {
                                    int part1 = int.Parse(parts1[i]);
                                    int part2 = int.Parse(parts2[i]);

                                    result = part1.CompareTo(part2);
                                }
                                else
                                {
                                    result = StringComparer.Ordinal.Compare(parts1[i], parts2[i]);
                                }
                            }
                            else
                            {
                                result = StringComparer.Ordinal.Compare(parts1[i], parts2[i]);
                            }
                        }

                        if (result == 0)
                        {
                            result = parts1.Length.CompareTo(parts2.Length);
                        }
                    }
                }
            }

            if (result == 0)
            {
                result = StringComparer.Ordinal.Compare(Build, other.Build);
            }

            return result;
        }

        public static bool operator ==(SemanticVersion v1, SemanticVersion v2)
            => v1.EqualsSemanticVersion(v2);

        public static bool operator !=(SemanticVersion v1, SemanticVersion v2)
            => !v1.EqualsSemanticVersion(v2);

        public static bool operator <(SemanticVersion v1, SemanticVersion v2)
            => v1.CompareTo(v2) < 0;

        public static bool operator >(SemanticVersion v1, SemanticVersion v2)
            => v1.CompareTo(v2) > 0;

        public static bool operator <=(SemanticVersion v1, SemanticVersion v2)
            => v1.CompareTo(v2) <= 0;

        public static bool operator >=(SemanticVersion v1, SemanticVersion v2)
            => v1.CompareTo(v2) >= 0;

        public static SemanticVersion Parse(string version)
        {
            int major = 0, minor = 0, patch = 0;
            string preRelease = "", build = "";

            int dashPos = version.IndexOf('-');

            if (dashPos < 0)
            {
                int plusPos = version.IndexOf('+');

                if (plusPos >= 0)
                {
                    build = version.Substring(plusPos + 1);
                    version = version.Substring(0, plusPos);
                }
            }
            else
            {
                int plusPos = version.IndexOf('+', dashPos + 1);

                if (plusPos < 0)
                {
                    preRelease = version.Substring(dashPos + 1);
                }
                else
                {
                    build = version.Substring(plusPos + 1);
                    preRelease = version.Substring(dashPos + 1, plusPos - dashPos - 1);
                }

                version = version.Substring(0, dashPos);
            }

            string[] parts = version.Split('.');

            if (parts.Length > 0)
                major = int.Parse(parts[0]);

            if (parts.Length > 1)
                minor = int.Parse(parts[1]);

            if (parts.Length > 2)
                patch = int.Parse(parts[2]);

            preRelease = preRelease.ToLowerInvariant();
            build = build.ToLowerInvariant();

            return new(major, minor, patch, preRelease, build);
        }
    }
}
