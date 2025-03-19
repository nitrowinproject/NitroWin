namespace NitroWin {
    public class NitroWin {
        public static void Main() {
            Console.Title = "NitroWin";

            bool mergeResult = Helper.Prompt("Merge tweaks?");

            if (mergeResult) {
                Tweaks.Merge();
            }
        }
    }
}