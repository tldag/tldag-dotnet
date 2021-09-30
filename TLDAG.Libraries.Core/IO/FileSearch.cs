using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using static System.IO.SearchOption;

namespace TLDAG.Libraries.Core.IO
{
    public class FileSearchEventArgs : EventArgs
    {
        public DirectoryInfo Directory { get; }
        public double Progress { get; }

        public FileSearchEventArgs(DirectoryInfo directory, double progress)
        {
            Directory = directory;
            Progress = progress;
        }
    }

    public class FileSearchEnumerator : IEnumerator<FileInfo>
    {
        public FileInfo Current => GetCurrent();
        object IEnumerator.Current => GetCurrent();

        public DirectoryInfo Start { get; }
        public string Pattern { get; }
        public bool Recurse { get; }
        public double Progress { get; private set; }

        private readonly Action<DirectoryInfo, double> startCallback;
        private readonly Action<DirectoryInfo, double> endCallback;

        private readonly Queue<DirectoryInfo> directories = new();
        private DirectoryInfo? directory = null;
        private IEnumerator<FileInfo>? enumerator = null;

        public FileSearchEnumerator(DirectoryInfo start, string pattern, bool recurse,
            Action<DirectoryInfo, double> startCallback, Action<DirectoryInfo, double> endCallback)
        {
            Start = start;
            Pattern = pattern;
            Recurse = recurse;

            this.startCallback = startCallback;
            this.endCallback = endCallback;

            Reset();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Reset()
        {
            EndDirectory();

            directories.Clear();
            directories.Enqueue(Start);

            Progress = 0;
        }

        private FileInfo GetCurrent()
        {
            return enumerator?.Current ?? throw new InvalidOperationException();
        }

        public bool MoveNext()
        {
            while (true)
            {
                if (enumerator != null)
                {
                    if (enumerator.MoveNext())
                        return true;
                }

                if (!NextDirectory())
                    return false;
            }
        }

        private bool NextDirectory()
        {
            EndDirectory();

            if (directories.Count == 0) return false;

            StartDirectory();

            return true;
        }

        private void EndDirectory()
        {
            UpdateProgress();

            if (enumerator != null)
            {
                enumerator.Dispose();
                enumerator = null;
            }

            if (directory != null)
            {
                endCallback(directory, Progress);
                directory = null;
            }
        }

        private void StartDirectory()
        {
            directory = directories.Dequeue();
            startCallback(directory, Progress);

            if (Recurse)
            {
                EnqueueSubDirectories(directory);
            }

            enumerator = GetEnumerator(directory);
        }

        private IEnumerator<FileInfo>? GetEnumerator(DirectoryInfo directory)
        {
            try
            {
                return directory
                .EnumerateFiles(Pattern, TopDirectoryOnly)
                .GetEnumerator();
            }
            catch (UnauthorizedAccessException) { /* ignored */ }
            catch (SecurityException) { /* ignored */ }

            return null;
        }

        private void EnqueueSubDirectories(DirectoryInfo directory)
        {
            try
            {
                DirectoryInfo[] subs = directory.GetDirectories();

                foreach (DirectoryInfo sub in subs)
                {
                    directories.Enqueue(sub);
                }
            }
            catch (UnauthorizedAccessException) { /* ignored */ }
            catch (SecurityException) { /* ignored */ }
        }

        private void UpdateProgress()
        {
            double divisor = Math.Max(10, directories.Count);
            double delta = (1.0 - Progress) / divisor;

            Progress += delta;
        }
    }

    public class FileSearch : IEnumerable<FileInfo>
    {
        public DirectoryInfo Start { get; }
        public string Pattern { get; }
        public bool Recurse { get; }

        public delegate void FileSearchHandler(FileSearch source, FileSearchEventArgs eventArgs);

        public event FileSearchHandler? DirectoryStarted;
        public event FileSearchHandler? DirectoryEnded;

        public FileSearch(DirectoryInfo start, string pattern, bool recurse)
        {
            Start = start;
            Pattern = pattern;
            Recurse = recurse;
        }

        public IEnumerator<FileInfo> GetEnumerator()
            => CreateEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => CreateEnumerator();

        private FileSearchEnumerator CreateEnumerator()
        {
            return new(Start, Pattern, Recurse, OnStartDirectory, OnEndDirectory);
        }

        private void OnStartDirectory(DirectoryInfo directory, double progress)
        {
            if (DirectoryStarted != null)
            {
                FileSearchEventArgs eventArgs = new(directory, progress);

                DirectoryStarted.Invoke(this, eventArgs);
            }
        }

        private void OnEndDirectory(DirectoryInfo directory, double progress)
        {
            if (DirectoryEnded != null)
            {
                FileSearchEventArgs eventArgs = new(directory, progress);

                DirectoryEnded.Invoke(this, eventArgs);
            }
        }
    }
}
