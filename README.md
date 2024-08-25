# youtube_downloader
This C# ASP.NET Web Forms project is designed to allow users to download video and audio streams from YouTube videos. It uses the YoutubeExplode library to interact with YouTube's video data and streams.

Key Features of the Project:
YouTube Video Processing:

Users can input a YouTube URL, and the application fetches the video details and available streams (video-only, audio-only, and muxed streams that contain both video and audio).
Stream Selection:

The available streams are displayed in a dropdown list, allowing the user to select a specific stream based on resolution and type.
Downloading Streams:

Once a stream is selected, the user can download it directly. The project supports downloading video-only streams as MP4 files, audio-only streams as MP3 files, and muxed streams (containing both video and audio) as MP4 files.
Filename Sanitization:

The project includes functionality to sanitize file names, ensuring that invalid characters are removed from the generated file names.
Optional Video-Audio Merging:

The project contains a method that uses FFmpeg to combine separate video and audio files into a single file, though this feature is not invoked in the current workflow. This would be useful for scenarios where video and audio are downloaded separately and need to be merged.
Possible Use Cases:
YouTube Video Archiving: Users can archive their favorite YouTube videos by downloading them in their preferred format.
Content Consumption Offline: Users can download video or audio content to consume offline, which is especially useful in areas with limited internet access.
Audio Extraction: Users may want to extract audio from YouTube videos, such as for creating podcasts or listening to music without the video.
Technical Components:
ASP.NET Web Forms: The project is structured as a web application using ASP.NET Web Forms, which is a traditional framework for building dynamic websites.
YoutubeExplode Library: This library is used to interact with YouTube, allowing the application to retrieve video metadata and stream URLs.
FFmpeg Integration: The project includes a method to invoke FFmpeg, a popular tool for multimedia processing, to combine audio and video streams if needed.
User Interaction Flow:
Input YouTube URL: The user inputs the YouTube video URL.
Fetch Video Data: The application retrieves the video title and available streams.
Select and Download Stream: The user selects a stream and downloads it.
This project is a functional tool for downloading and managing YouTube videos and streams, providing both basic and advanced options like stream selection and file sanitization.
