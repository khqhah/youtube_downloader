using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;
using System.Diagnostics;

namespace You
{
    public partial class Test : System.Web.UI.Page
    {
        private static YoutubeClient youtube = new YoutubeClient();
        private static string videoTitle;  // Store video title

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                var videoId = VideoId.Parse(txtYouTubeURL.Text);
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
                var video = await youtube.Videos.GetAsync(videoId);
                videoTitle = video.Title;  // Store the video title

                // Create a DataTable to hold the video formats
                DataTable dt = new DataTable();
                dt.Columns.Add("Text", typeof(string));
                dt.Columns.Add("Value", typeof(string));

                // Add video formats to the DataTable
                foreach (var stream in streamManifest.Streams)
                {
                    string resolution = "Unknown";
                    string type = "Unknown";
                    string url = stream.Url;

                    if (stream is MuxedStreamInfo muxedStream)
                    {
                        resolution = muxedStream.VideoResolution.ToString() ?? "Unknown";
                        type = "Video + Audio";
                    }
                    else if (stream is VideoOnlyStreamInfo videoStream)
                    {
                        resolution = videoStream.VideoResolution.ToString() ?? "Unknown";
                        type = "Video Only";
                    }
                    else if (stream is AudioOnlyStreamInfo audioStream)
                    {
                        resolution = "Audio only";
                        type = "Audio";
                    }

                    dt.Rows.Add($"{type}: {resolution}", url);
                }

                // Bind DataTable to the DropDownList
                ddlVideoFormats.DataTextField = "Text";
                ddlVideoFormats.DataValueField = "Value";
                ddlVideoFormats.DataSource = dt;
                ddlVideoFormats.DataBind();

                // Ensure DropDownList and Download button are visible
                ddlVideoFormats.Visible = dt.Rows.Count > 0;
                btnDownload.Visible = dt.Rows.Count > 0;

                // Update the video title display
                string script = $"document.getElementById('videoTitleDisplay').innerText = 'Video Title: {videoTitle}';";
                ClientScript.RegisterStartupScript(this.GetType(), "updateVideoTitle", script, true);

                lblMessage.Text = "";  // Clear the label if it has any text
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error processing the video: " + ex.Message;
            }
        }

        protected async void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedUrl = ddlVideoFormats.SelectedValue;

                if (string.IsNullOrEmpty(selectedUrl))
                {
                    lblMessage.Text = "Please select a video format.";
                    return;
                }

                var videoId = VideoId.Parse(txtYouTubeURL.Text);
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);

                // Find the selected stream
                var selectedStream = streamManifest.Streams[ddlVideoFormats.SelectedIndex];

                if (selectedStream == null)
                {
                    lblMessage.Text = "Selected stream is not available.";
                    return;
                }

                // Identify if the stream is video-only, audio-only, or muxed
                if (selectedStream is VideoOnlyStreamInfo videoStream)
                {
                    // Download video-only stream
                    string videoFileName = $"{videoTitle}_video.mp4";
                    string videoFilePath = Server.MapPath($"~/Downloads/{SanitizeFileName(videoFileName)}");

                    await youtube.Videos.Streams.DownloadAsync(videoStream, videoFilePath);
                    lblMessage.Text = "Downloaded video-only stream successfully.";
                }
                else if (selectedStream is AudioOnlyStreamInfo audioStream)
                {
                    // Download audio-only stream
                    string audioFileName = $"{videoTitle}_audio.mp3";
                    string audioFilePath = Server.MapPath($"~/Downloads/{audioFileName}");

                    await youtube.Videos.Streams.DownloadAsync(audioStream, audioFilePath);
                    lblMessage.Text = "Downloaded audio-only stream successfully.";
                }
                else if (selectedStream is MuxedStreamInfo muxedStream)
                {
                    // Download muxed stream (video + audio)
                    string muxedFileName = $"{videoTitle}_muxed.mp4";
                    string muxedFilePath = Server.MapPath($"~/Downloads/{muxedFileName}");

                    await youtube.Videos.Streams.DownloadAsync(muxedStream, muxedFilePath);
                    lblMessage.Text = "Downloaded video + audio stream successfully.";
                }
                else
                {
                    lblMessage.Text = "Selected stream type is not supported.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error downloading the video: " + ex.Message;
            }
        }
        private string SanitizeFileName(string fileName)
        {
            // Remove invalid characters from file name
            return string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
        }
        private void CombineAudioVideo(string videoFilePath, string audioFilePath, string outputFilePath)
        {
            // Ensure FFmpeg is installed and accessible from the system PATH
            var ffmpegPath = @"ffmpeg";  // Change if FFmpeg is located elsewhere

            var processInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{videoFilePath}\" -i \"{audioFilePath}\" -c:v copy -c:a aac \"{outputFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                using (var reader = process.StandardError)
                {
                    string error = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(error))
                    {
                        lblMessage.Text = $"FFmpeg error: {error}";
                    }
                }
                process.WaitForExit();
            }
        }
    }
}
