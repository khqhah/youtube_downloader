<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Test.aspx.cs" Inherits="You.Test" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>YouTube Video Downloader</title>
    <link rel="stylesheet" type="text/css" href="styles.css" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>YouTube Video Downloader</h1>
            <div class="input-group">
                <label for="txtYouTubeURL">YouTube URL:</label>
                <div class="input-wrapper">
                    <asp:TextBox ID="txtYouTubeURL" runat="server" Text="" CssClass="input-text" />
                    <i class="fas fa-link icon"></i>
                </div>
            </div>
            <div class="button-group">
                <asp:Button ID="btnProcess" Text="Process" runat="server" OnClick="btnProcess_Click" CssClass="button-primary" />
                <div id="spinner" class="spinner"></div>
            </div>
            <div class="download-section">
                <asp:DropDownList ID="ddlVideoFormats" runat="server" CssClass="dropdown" Visible="false"></asp:DropDownList>
                <asp:Button ID="btnDownload" Text="Download" runat="server" OnClick="btnDownload_Click" CssClass="button-secondary" Visible="false" />
            </div>
            <div id="videoTitleDisplay" class="video-title-display"></div>
            <asp:Label ID="lblMessage" Text="" runat="server" CssClass="message-label" />
        </div>
    </form>
</body>
</html>
