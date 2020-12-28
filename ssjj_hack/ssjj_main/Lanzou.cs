using Hzexe.Lanzou;
using static Hzexe.Lanzou.Model.Lanzou.GetDirResponse;

namespace ssjj_main
{
    public class Lanzou
    {
        const string COOKIE = "phpdisk_info=ADVfbFEzUmpQYA5oAW8GVQJgVWQIWABmUmkJbwYwU2FYbQc1UjUFPVBqVTYJWgBvBjdQMwhmVTcAO1BhDzpUYwBlXz9RYVJnUGQOawFvBmsCYVVlCGcAMlIzCT8GMVNjWG4HPFJlBTlQa1VnCWYAUwZnUGoIZ1UyADNQMQ85VGMAMF9lUTA%3D; ylogin=1104264";
        public bool isDownloading = false;
        public async void Download()
        {
            isDownloading = true;
            LanzouClient client = new LanzouClient(COOKIE);
            var res = await client.LsDirAsync("2762979");
            TextItem item = null;
            foreach (var d in res.text)
            {
                if (item == null || item.name.CompareTo(d.name) <= 0)
                    item = d;
            }
            if (item == null)
                return;
            var res2 = await client.LsFilesAsync(item.fol_id);
            foreach (var f in res2.text)
            {
                var down = await client.FileDownloadAsync(f.downs);
            }

            // var url = client.FileDownloadAsync("https://wws.lanzous.com/ivtVzfmbbuj").Result;
        }
    }
}
