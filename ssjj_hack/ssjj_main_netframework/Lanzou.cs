using Lanzou;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using static Lanzou.GetDirResponse;

namespace ssjj_main_netframework
{
    public class Lanzou
    {
        const string COOKIE = "phpdisk_info=ADVfbFEzUmpQYA5oAW8GVQJgVWQIWABmUmkJbwYwU2FYbQc1UjUFPVBqVTYJWgBvBjdQMwhmVTcAO1BhDzpUYwBlXz9RYVJnUGQOawFvBmsCYVVlCGcAMlIzCT8GMVNjWG4HPFJlBTlQa1VnCWYAUwZnUGoIZ1UyADNQMQ85VGMAMF9lUTA%3D; ylogin=1104264";
        public bool isDownloading = false;
        public async void UpdateDLL()
        {
            try
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
                var _verdir = Settings.root.Combine(item.name);
                if (!Directory.Exists(_verdir))
                    Directory.CreateDirectory(_verdir);
                var res2 = await client.LsFilesAsync(item.fol_id);
                foreach (var f in res2.text)
                {
                    var _fpath = _verdir.Combine(f.name);
                    if (!File.Exists(_fpath))
                    {
                        var share = await client.GetShareUrl(f.id);
                        var url = await client.GetDurl(share.info.url);
                        var down = await DownloadFile(url.url, _fpath);
                    }
                }
                isDownloading = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "启动失败", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public float downloadProgress = 0;
        public async Task<bool> DownloadFile(string url, string path)
        {
            var httpClient = new HttpClient();
            downloadProgress = 0;
            var response = await httpClient.GetAsync(url);
            try
            {
                var n = response.Content.Headers.ContentLength;
                if (File.Exists(path))
                    File.Delete(path);
                var stream = await response.Content.ReadAsStreamAsync();
                using (var fileStream = new FileInfo(path).Create())
                {
                    using (stream)
                    {
                        byte[] buffer = new byte[1024];
                        var readLength = 0;
                        int length;
                        while ((length = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            readLength += length;
                            downloadProgress = (float)readLength / (long)n;
                            fileStream.Write(buffer, 0, length);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return false;
            }
            downloadProgress = 1;
            return true;
        }
    }
}
