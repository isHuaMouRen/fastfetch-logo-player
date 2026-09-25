using FastfetchLogoPlayer.Utils;

namespace FastfetchLogoPlayer
{
    internal class Program
    {
        private static string ExecutablePath = "fastfetch";
        private static FastfetchManager _fm;

        private static string[] Logos;
        private static int currentLogoIndex;

        private const string Version = "v1.0";

        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.InputEncoding = System.Text.Encoding.UTF8;

                //参数解析
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--exe-path" or "-p":
                            if (i + 1 < args.Length)
                                ExecutablePath = args[++i];
                            else
                                throw new ArgumentException($"未提供值: {args[i]}");
                            break;
                    }
                }

                _fm = new FastfetchManager(ExecutablePath);
                //获取logo
                Logos = _fm.GetLogosList();

                currentLogoIndex = 0;
                while (true)
                {
                    Console.Clear();
                    Console.ResetColor();

                    Console.WriteLine($" {currentLogoIndex + 1}/{Logos.Length} | {Logos[currentLogoIndex]}  ||  Fastfetch Logo Player {Version}  by isHuaMouRen");
                    _fm.ShowLogo(Logos[currentLogoIndex]);
                    Console.WriteLine("← 上一个Logo | → 下一个Logo | e 指定一个Logo | h 帮助 | q 退出");

                    var key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            currentLogoIndex = MathHelper.Wrap(currentLogoIndex - 1, 0, Logos.Length - 1);
                            break;
                        case ConsoleKey.RightArrow:
                            currentLogoIndex = MathHelper.Wrap(currentLogoIndex + 1, 0, Logos.Length - 1);
                            break;
                        case ConsoleKey.E:
                            Console.Clear();
                            Console.WriteLine($"输入要显示的Logo，支持输入索引或名称 (总计 {Logos.Length} 个Logo)");
                            var result = Console.ReadLine()?.Trim();
                            if (string.IsNullOrEmpty(result))
                                break;
                            //索引
                            if(int.TryParse(result,out var parsed))
                            {
                                currentLogoIndex = MathHelper.Wrap(parsed-1, 0, Logos.Length - 1);
                                break;
                            }
                            //名称
                            if (Logos.Contains(result))
                            {
                                currentLogoIndex = MathHelper.Wrap(Logos.IndexOf(result), 0, Logos.Length - 1);
                                break;
                            }
                            Console.WriteLine($"没有找到名为 {result} 的Logo...");
                            Console.ReadKey(true);                                

                            break;
                        case ConsoleKey.H:
                            Console.Clear();
                            var text = $"""
                                Fastfetch Logo Player {Version}
                                        made by isHuaMouRen (https://github.com/isHuaMouRen)

                                    提供一个交互式终端界面，用于预览Fastfetch提供的所有内置Logo


                                此软件是一个自由的开源软件，并使用 GPLv3 开源许可协议 (https://www.gnu.org/licenses/gpl-3.0)

                                此软件的Github仓库链接 https://github.com/isHuaMouRen/fastfetch-logo-player
                                """;
                            Console.WriteLine(text);
                            Console.ReadKey(true);
                            break;
                        case ConsoleKey.Q:
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }
    }
}
