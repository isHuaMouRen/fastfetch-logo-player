using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FastfetchLogoPlayer.Utils
{
    public class FastfetchManager
    {
        public string ExecutablePath { get; set; }

        public FastfetchManager(string path = "fastfetch")
        {
            ExecutablePath = path;
        }

        public string[] GetLogosList()
        {
            var psi = new ProcessStartInfo
            {
                FileName = ExecutablePath,
                Arguments = "--list-logos autocompletion",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            using var process = new Process { StartInfo = psi };

            try
            {
                if (!process.Start())
                    throw new InvalidOperationException($"无法启动进程: {ExecutablePath}  提示：如果你的Fastfetch未被写入PATH，请使用-p参数指定绝对路径");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"启动 fastfetch 失败，请检查路径 '{ExecutablePath}'。  提示：如果你的Fastfetch未被写入PATH，请使用-p参数指定绝对路径", ex);
            }

            var stdoutTask = process.StandardOutput.ReadToEndAsync();
            var stderrTask = process.StandardError.ReadToEndAsync();

            process.WaitForExit();

            string stdout = stdoutTask.GetAwaiter().GetResult();
            string stderr = stderrTask.GetAwaiter().GetResult();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException(
                    $"fastfetch 退出码 {process.ExitCode}: {stderr.Trim()}");
            }

            return stdout
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.TrimEnd('\r'))
                .Where(line => line.Length > 0)
                .ToArray();
        }

        public void ShowLogo(string logoName)
        {
            using var proc = Process.Start(new ProcessStartInfo
            {
                FileName = ExecutablePath,
                Arguments = $"--logo \"{logoName}\"",
                UseShellExecute = false
            });
            proc?.WaitForExit();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
        }
    }
}