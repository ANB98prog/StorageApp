namespace Storage.Tests.Common
{
    public static class TestHelper
    {
        public static void RemoveTestData(string path)
        {
            var filesToRemove = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (var file in filesToRemove)
            {
                File.Delete(file);
            }

            var dirToRemove = Directory.GetDirectories(path);

            foreach (var dir in dirToRemove)
            {
                Directory.Delete(dir, true);
            }
        }
    }
}
