# Video Catalogue Web Application

## Overview

This ASP.NET Core 8 MVC application allows users to upload, list, and play MP4 video files stored on the server.  
All functionality is delivered through a single MVC view (`Home/Index`), supported by a Web API upload endpoint.

## Features

**Upload**

- Upload one or more `.mp4` files
- Maximum combined upload size: 200 MB
- Files with the same name are overwritten
- Client-side and server-side validation
- Clear JSON error responses

**Catalogue**

- Lists all MP4 files in `wwwroot/media`
- Shows filename and file size (MB)
- Clicking a row plays the selected video
- Only one video plays at a time

**Video Playback**

- Video player displayed at the top of the page
- “Now playing” label updates automatically
- Videos are served as static files

**Responsive Layout**

- Designed for widths between 400px and 1400px
- No horizontal scrolling
- Works on mobile, tablet, and desktop

## Technologies

- ASP.NET Core 8 MVC
- ASP.NET Core Web API
- Bootstrap 5
- JavaScript (vanilla)
- HTML5 `<video>`

## How to Run

1. Install .NET 8 SDK and Runtime
2. Clone or download the repository
3. Restore dependencies:
   ```bash
   dotnet restore
   Run the application:
   ```

bash
Copy code
dotnet run
Open the application in a browser:

text
Copy code
https://localhost:{port}/
API – Upload Endpoint
Endpoint

POST /api/upload

Accepts multipart/form-data with one or more .mp4 files

Validation rules

Only .mp4 files are accepted

Combined upload size must be 200 MB or less

Files with duplicate names are overwritten
